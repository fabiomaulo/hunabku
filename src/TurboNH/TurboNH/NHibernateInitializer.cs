using System.Data;
using ConfOrm;
using ConfOrm.NH;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Tool.hbm2ddl;

namespace TurboNH
{
	public class NHibernateInitializer
	{
		private const string ConnectionString =
	@"Data Source=localhost\SQLEXPRESS;Initial Catalog=TurboNH;Integrated Security=True;Pooling=False";
		Configuration configure;
		private ISessionFactory sessionFactory;

		#region NH Startup

		public void Initialize()
		{
			configure = new Configuration();
			configure.SessionFactoryName("Demo");
			configure.Proxy(p =>
			{
				p.Validation = false;
				p.ProxyFactoryFactory<ProxyFactoryFactory>();
			});
			configure.DataBaseIntegration(db =>
			{
				db.Dialect<MsSql2008Dialect>();
				db.Driver<SqlClientDriver>();
				db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
				db.IsolationLevel = IsolationLevel.ReadCommitted;
				db.ConnectionString = ConnectionString;
				db.Timeout = 10;
				db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
				db.BatchSize = 500;
			});
			configure.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] { new PostLoadEventListener() };
			configure.EventListeners.DeleteEventListeners = new IDeleteEventListener[] { new PreDeleteEventListener(), new DefaultDeleteEventListener() };
			Map();
		}

		public void CreateSchema()
		{
			new SchemaExport(configure).Create(false, true);
		}

		public void DropSchema()
		{
			new SchemaExport(configure).Drop(false, true);
		}

		private void Map()
		{
			configure.AddDeserializedMapping(GetMapping(), "Domain");
		}

		public ISessionFactory SessionFactory
		{
			get { return sessionFactory ?? (sessionFactory = configure.BuildSessionFactory()); }
		}

		#endregion

		public HbmMapping GetMapping()
		{
			var orm = new ObjectRelationalMapper();
			var mapper = new Mapper(orm);
			orm.TablePerClass<PocoEntity>();
			orm.TablePerClass<SelfTrackingEntity>();
			return mapper.CompileMappingFor(new[] { typeof(PocoEntity), typeof(SelfTrackingEntity) });
		}
	}
}