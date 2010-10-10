using System;

namespace EntityNameInAction.Abstraction.Adapters.DependencyInjection
{
	public class DI
	{
		private static IClassResolver resolver;
		private DI() {}

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
	}
}