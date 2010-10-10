namespace EntitiesAbstrDI.Domain.Impl
{
	public class Product : BaseEntity, IProduct
	{
		#region IProduct Members

		public virtual string Description { get; set; }
		public virtual decimal Price { get; set; }

		#endregion
	}
}