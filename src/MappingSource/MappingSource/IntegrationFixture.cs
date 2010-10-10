using NHibernate;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace MappingSource
{
	[TestFixture]
	public class IntegrationFixture
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.Configure();
			cfg.Register(typeof(Animal));
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
		public void EntityNameDemo()
		{
			object savedId;
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var a = new Animal { Description = "Dog" };
				savedId = s.Save(a);
				tx.Commit();
				Assert.That(a.Id, Is.GreaterThan(0));
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var a = s.Get<Animal>(savedId);
				Assert.That(a.Description, Is.EqualTo("Dog"));
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Animal");
				tx.Commit();
			}
		}
	}
}