using System;
using Castle.DynamicProxy;

namespace LessThanFew
{
	public class EntityFactory
	{
		private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

		public T NewEntity<T>()
		{
			Type t = typeof (T);
			return
				(T)
				proxyGenerator.CreateInterfaceProxyWithoutTarget(t, new[] {typeof (IProxyMarker), t},
				                                                 new DataProxyHandler(t.FullName, 0));
		}
	}
}