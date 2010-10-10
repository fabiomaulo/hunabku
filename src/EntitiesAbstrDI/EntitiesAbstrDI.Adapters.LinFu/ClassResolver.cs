using LinFu.IoC.Interfaces;
using LinFu.IoC;

namespace EntitiesAbstrDI.Adapters.LinFu
{
	public class ClassResolver: IClassResolver
	{
		private readonly IServiceContainer container;

		public ClassResolver(IServiceContainer container)
		{
			this.container = container;
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			
		}

		#endregion

		#region Implementation of IClassResolver

		public T Resolve<T>() where T : class
		{
			return container.GetService<T>();
		}

		public T Resolve<T>(string service) where T : class
		{
			return container.GetService<T>(service);
		}

		#endregion
	}
}