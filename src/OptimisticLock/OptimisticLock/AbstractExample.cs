using log4net.Config;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace OptimisticLock
{
	public abstract class AbstractExample
	{
		private Configuration cfg;
		protected ISessionFactoryImplementor sessionFactory;

		static AbstractExample()
		{
			XmlConfigurator.Configure();
		}

		[TestFixtureSetUp]
		public void DbCreation()
		{
			cfg = new Configuration();
			cfg.AddResource(GetMappingResourcePath(), GetType().Assembly);
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			sessionFactory = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
		}

		protected abstract string GetMappingResourcePath();

		[TestFixtureTearDown]
		public void DbDrop()
		{
			new SchemaExport(cfg).Drop(false, true);
			sessionFactory.Close();
			sessionFactory = null;
			cfg = null;
		}
	}
}