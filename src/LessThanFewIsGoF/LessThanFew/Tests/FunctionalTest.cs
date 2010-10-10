using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace LessThanFew.Tests
{
	public class FunctionalTest
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;
		protected EntityFactory entityFactory = new EntityFactory();

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			cfg.SetInterceptor(new EntityNameInterceptor());
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
	}
}