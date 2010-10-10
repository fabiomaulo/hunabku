using System;
using System.Collections.Generic;
using EntityNameInAction.Abstraction.Adapters.DependencyInjection;
using NHibernate.Engine;
using NHibernate;

namespace EntityNameInAction.Abstraction.NhAdapters
{
	public class NhEntityClassResolver : IClassResolver
	{
		private readonly Dictionary<Type, string> serviceToEntityName = new Dictionary<Type, string>();
		public NhEntityClassResolver(ISessionFactoryImplementor factory)
		{
			if(factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			Factory = factory;
			InitializeTypedPersisters();
		}

		private void InitializeTypedPersisters()
		{
			foreach (var entityName in Factory.GetAllClassMetadata().Keys)
			{
				serviceToEntityName.Add(Factory.GetEntityPersister(entityName).GetConcreteProxyClass(EntityMode.Poco), entityName);
			}
		}

		public ISessionFactoryImplementor Factory { get; private set; }

		#region Implementation of IDisposable

		public void Dispose()
		{
		}

		#endregion

		#region Implementation of IClassResolver

		public T Resolve<T>() where T : class
		{
			string entityName;
			if(serviceToEntityName.TryGetValue(typeof(T), out entityName))
			{
				return Resolve<T>(entityName);
			}
			return null;
		}

		public T Resolve<T>(string service) where T: class
		{
			return Factory.GetEntityPersister(service).Instantiate(null, EntityMode.Poco) as T;
		}

		#endregion
	}
}