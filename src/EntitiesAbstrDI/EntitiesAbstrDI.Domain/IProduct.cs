namespace EntitiesAbstrDI.Domain
{
	public interface IProduct : IEntity
	{
		string Description { get; set; }
		decimal Price { get; set; }
	}
}