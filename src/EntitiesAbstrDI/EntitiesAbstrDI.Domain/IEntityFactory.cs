namespace EntitiesAbstrDI.Domain
{
	public interface IEntityFactory
	{
		T Create<T>();
	}
}