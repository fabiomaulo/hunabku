using EntityNameInAction.Abstraction.Entities.Naturalness;
using Iesi.Collections.Generic;

namespace EntityNameInAction.Abstraction.Entities.Impl.Naturalness
{
	public class MyFamily<T> : Entity, IFamily<T> where T : IAnimal
	{
		public T Father { get; set; }

		public T Mother { get; set; }

		public ISet<T> Childs { get; set; }
	}
}