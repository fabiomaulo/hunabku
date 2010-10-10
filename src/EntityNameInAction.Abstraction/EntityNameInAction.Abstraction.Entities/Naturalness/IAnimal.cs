namespace EntityNameInAction.Abstraction.Entities.Naturalness
{
	public interface IAnimal: IEntity<int>
	{
		string Description { get; set; }
	}
}