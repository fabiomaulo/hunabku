using Company.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;
using System;

namespace DiffAssemblyVersion
{
	[TestFixture]
	public class DomainFixture
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			new SchemaExport(cfg).Drop(false, true);
			sessions.Close();
			sessions = null;
			cfg = null;
		}

		[Test]
		public void SomebodySaidIsABug()
		{
			var version = typeof (Person).Assembly.GetName().Version.ToString();
			Console.WriteLine("Running version {0}", version);

			object savedId;
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var a = new Person {FirstName = "Pasqual", LastName = "Angulo"};
				savedId = s.Save(a);
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var a = s.Get<Person>(savedId);
				if(version == "1.0.0.0")
					Assert.That(a.FullName, Is.EqualTo("Pasqual-Angulo"));
				else
					Assert.That(a.FullName, Is.EqualTo("Angulo*Pasqual"));
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
	}
}