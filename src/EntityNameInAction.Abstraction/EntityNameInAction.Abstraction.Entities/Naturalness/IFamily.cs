using Iesi.Collections.Generic;

namespace EntityNameInAction.Abstraction.Entities.Naturalness
{
	public interface IFamily<T>: IEntity<int> where T : IAnimal
	{
		T Father { get; set; }
		T Mother { get; set; }
		ISet<T> Childs { get; set; }
	}
}