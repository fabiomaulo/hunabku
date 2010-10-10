using System;
using Castle.Windsor;
using NHibernate.Bytecode;
using NHibernate.ByteCode.Castle;
using NHibernate.Properties;

namespace EntitiesWithDI
{
	public class BytecodeProvider : IBytecodeProvider
	{
		private readonly IObjectsFactory objectsFactory = new ActivatorObjectsFactory();

		private readonly IWindsorContainer container;

		public BytecodeProvider(IWindsorContainer container)
		{
			this.container = container;
		}

		#region IBytecodeProvider Members

		public IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters)
		{
			return new ReflectionOptimizer(container, clazz, getters, setters);
		}

		public IProxyFactoryFactory ProxyFactoryFactory
		{
			get { return new ProxyFactoryFactory(); }
		}

		public IObjectsFactory ObjectsFactory
		{
			get { return objectsFactory; }
		}

		#endregion
	}
}