using System;

namespace EntitiesAbstrDI.Adapters
{
	public interface IClassResolver : IDisposable
	{
		T Resolve<T>() where T : class;
		T Resolve<T>(string service) where T : class;
	}
}