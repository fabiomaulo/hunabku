using System;
using Castle.Windsor;

namespace EntitiesWithDI
{
	public class DI
	{
		private static IWindsorContainer container;
		private DI() {}

		public static IWindsorContainer Container
		{
			get
			{
				if (container == null)
				{
					throw new InvalidOperationException("Container was not initialized. Use StackContainer.");
				}

				return container;
			}
		}

		public static void StackContainer(IWindsorContainer c)
		{
			container = c;
		}
	}
}