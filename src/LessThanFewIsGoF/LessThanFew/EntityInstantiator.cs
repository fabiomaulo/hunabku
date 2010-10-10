using System;
using Castle.DynamicProxy;
using NHibernate.Tuple;

namespace LessThanFew
{
	public class EntityInstantiator : IInstantiator
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
		private readonly Type t;

		public EntityInstantiator(Type entityType)
		{
			t = entityType;
		}

		public object Instantiate(object id)
		{
			return
				proxyGenerator.CreateInterfaceProxyWithoutTarget(t, new[] {typeof (IProxyMarker), t},
				                                                 new DataProxyHandler(t.FullName, id));
		}

		public object Instantiate()
		{
			return Instantiate(null);
		}

		public bool IsInstance(object obj)
		{
			try
			{
				return t.IsInstanceOfType(obj);
			}
			catch (Exception e)
			{
				throw new Exception("could not get handle to entity-name as interface : " + e);
			}
		}
	}
}