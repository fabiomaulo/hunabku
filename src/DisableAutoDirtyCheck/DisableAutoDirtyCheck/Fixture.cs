using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using uNhAddIns.TestUtils.NhIntegration;
using NUnit.Framework;

namespace DisableAutoDirtyCheck
{
	[TestFixture]
	public class Fixture
	{
		private const string ReptilesfamilyEntityName = "ReptilesFamily";
		private const string HumanfamilyEntityName = "HumanFamily";
		private Configuration cfg;
		private ISessionFactoryImplementor sessionFactory;

		[TestFixtureSetUp]
		public void DbCreation()
		{
			cfg = new Configuration();
			cfg.AddResource("DisableAutoDirtyCheck.Domain.hbm.xml", typeof(Animal).Assembly);
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			sessionFactory = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
		}

		[TestFixtureTearDown]
		public void DbDrop()
		{
			new SchemaExport(cfg).Drop(false, true);
			sessionFactory.Close();
			sessionFactory = null;
			cfg = null;
		}

		public void FillDb()
		{
			sessionFactory.EncloseInTransaction(session =>
			{
				for (int i = 0; i < 100; i++)
				{
					var reptileFamily = ReptileFamilyBuilder
						.StartRecording()
						.WithChildren(2)
						.Build();

					session.Save(ReptilesfamilyEntityName, reptileFamily);
				}

				for (int i = 0; i < 100; i++)
				{
					var humanFamily = HumanFamilyBuilder
						.StartRecording()
						.WithChildren(1)
						.Build();

					session.Save(HumanfamilyEntityName, humanFamily);
				}
			});
		}

		public void CleanDb()
		{
			sessionFactory.EncloseInTransaction(session =>
			{
				session.Delete("from HumanFamily");
				session.Delete("from ReptilesFamily");
			});
		}

		[Test]
		public void ShouldNotAutoUpdate()
		{
			FillDb();

			using (ISession s = sessionFactory.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var reptiles = s.CreateQuery("from ReptilesFamily")
					.Future<Family<Reptile>>();

				var humans = s.CreateQuery("from HumanFamily")
					.Future<Family<Human>>();
				
				ModifyAll(reptiles);
				ModifyAll(humans);

				sessionFactory.Statistics.Clear();

				s.Update(ReptilesfamilyEntityName, reptiles.First());
				s.Update(HumanfamilyEntityName, humans.First());

				tx.Commit();
			}

			sessionFactory.Statistics.EntityUpdateCount
				.Should().Be.EqualTo(7);

			CleanDb();
		}

		[Test]
		public void ShouldUpdateCollection()
		{
			sessionFactory.EncloseInTransaction(session =>
			{
				var reptileFamily = ReptileFamilyBuilder.StartRecording().WithChildren(2).Build();
				session.Save(ReptilesfamilyEntityName, reptileFamily);
			});

			Family<Reptile> reptil;
			sessionFactory.EncloseInTransaction(session =>
			{
				reptil = session.CreateQuery("from ReptilesFamily")
					.SetMaxResults(1).UniqueResult<Family<Reptile>>();

				reptil.Children.Remove(reptil.Children.First());
				session.Update(reptil);
			});

			sessionFactory.EncloseInTransaction(session =>
			{
				reptil = session.CreateQuery("from ReptilesFamily")
					.SetMaxResults(1).UniqueResult<Family<Reptile>>();

				reptil.Children.Count.Should().Be.EqualTo(1);
			});

			CleanDb();
		}

		[Test]
		public void ShouldDelete()
		{
			sessionFactory.EncloseInTransaction(session =>
			{
				var reptileFamily = ReptileFamilyBuilder.StartRecording().WithChildren(2).Build();
				session.Save(ReptilesfamilyEntityName, reptileFamily);
			});

			sessionFactory.EncloseInTransaction(session =>
			{
				var reptil = session.CreateQuery("from ReptilesFamily")
					.SetMaxResults(1).UniqueResult<Family<Reptile>>();

				session.Delete(reptil);
			});

			IList instancesInDb = null;
			sessionFactory.EncloseInTransaction(session => 
				instancesInDb = session.CreateQuery("from System.Object").List());

			instancesInDb.Count.Should().Be.EqualTo(0);
		}

		public static void ModifyAll<T>(IEnumerable<Family<T>> families) where T: Animal
		{
			int i = 0;
			foreach (var family in families)
			{
				i++;
				family.Father.Description = family.Father.Description + i;
				family.Mother.Description = family.Father.Description + i;
				foreach (var child in family.Children)
				{
					child.Description = child.Description + " of " + i;
				}
			}
		}
	}
}
