using System;

namespace EntitiesAbstrDI.Adapters
{
	public class ServiceLocator
	{
		private static IClassResolver resolver;
		private ServiceLocator() {}

		public static IClassResolver Resolver
		{
			get
			{
				if (resolver == null)
				{
					throw new InvalidOperationException("Resolver was not initialized. Use StackResolver.");
				}

				return resolver;
			}
		}

		public static void StackResolver(IClassResolver dependencyResolver)
		{
			resolver = dependencyResolver;
		}

		public static T Locate<T>() where T : class
		{
			return Resolver.Resolve<T>();
		}
	}
}