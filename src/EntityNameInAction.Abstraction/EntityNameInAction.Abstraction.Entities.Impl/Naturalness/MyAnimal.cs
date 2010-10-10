using EntityNameInAction.Abstraction.Entities.Naturalness;

namespace EntityNameInAction.Abstraction.Entities.Impl.Naturalness
{
	public class MyAnimal: Entity, IAnimal
	{
		public string Description { get; set; }
	}
}