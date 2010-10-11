using System;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using NUnit.Framework;

namespace TurboNH
{
	public class Demo
	{
		private NHibernateInitializer nhinit;
		private ISessionFactory sf;

		[TestFixtureTearDown]
		public void DbDrop()
		{
			sf.Close();
			nhinit.DropSchema();
		}

		[TestFixtureSetUp]
		public void DbCreation()
		{
			nhinit = new NHibernateInitializer();
			nhinit.Initialize();
			nhinit.CreateSchema();
			sf = nhinit.SessionFactory;
			FillDb();
		}

		public void FillDb()
		{
			using (var s = sf.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				for (int i = 0; i < 1000; i++)
				{
					s.Persist(new PocoEntity { Description = "pocoValue" + i });
				}
				tx.Commit();
			}

			using (var s = sf.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				for (int i = 0; i < 1000; i++)
				{
					s.Persist(new SelfTrackingEntity { Description = "SelfTrackingEntityValue" + i });
				}
				tx.Commit();
			}
		}

		[Test]
		public void TimeToFlushPocoEntities()
		{
			using (var s = sf.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var entities = s.QueryOver<PocoEntity>().List();
				var someToModify = entities.Skip(4).Take(10);
				foreach (var entity in someToModify)
				{
					entity.Description = "Modified";
				}
				var stopWath = new Stopwatch();
				stopWath.Start();
				tx.Commit();
				stopWath.Stop();
				Console.WriteLine("Milliseconds to flush and commit:" + stopWath.ElapsedMilliseconds);
			}			
		}

		[Test]
		public void TimeToFlushSelfTrackingEntities()
		{
			using (var s = sf.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var entities = s.QueryOver<SelfTrackingEntity>().List();
				var someToModify = entities.Skip(4).Take(10);
				foreach (var entity in someToModify)
				{
					entity.Description = "Modified";
				}
				var stopWath = new Stopwatch();
				stopWath.Start();
				tx.Commit();
				stopWath.Stop();
				Console.WriteLine("Milliseconds to flush and commit:" + stopWath.ElapsedMilliseconds);
			}
		}
	}
}