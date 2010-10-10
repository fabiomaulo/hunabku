using System;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Environment=NHibernate.Cfg.Environment;

namespace EntitiesWithDI
{
	public abstract class BaseTest
	{
		protected static readonly WindsorContainer container;
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;

		static BaseTest()
		{
			container = new WindsorContainer();
			DI.StackContainer(container);
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			ConfigureWindsorContainer();
			Environment.BytecodeProvider = new BytecodeProvider(container);
			cfg = new Configuration();
			cfg.AddAssembly("EntitiesWithDI");
			cfg.Configure();
			cfg.Interceptor = new WindsorInterceptor(container);
			new SchemaExport(cfg).Create(false, true);
			sessions = (ISessionFactoryImplementor) cfg.BuildSessionFactory();
		}

		protected abstract void ConfigureWindsorContainer();

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			new SchemaExport(cfg).Drop(false, true);
			sessions.Close();
			sessions = null;
			cfg = null;
		}

		public class WindsorInterceptor : EmptyInterceptor
		{
			private readonly WindsorContainer cont;

			public WindsorInterceptor(WindsorContainer container)
			{
				cont = container;
			}

			public override object Instantiate(string entityName, EntityMode entityMode, object id)
			{
				if (Environment.UseReflectionOptimizer)
				{
					return null;
				}
				else
				{
					try
					{
						return cont.Resolve(entityName);
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
		}
	}
}