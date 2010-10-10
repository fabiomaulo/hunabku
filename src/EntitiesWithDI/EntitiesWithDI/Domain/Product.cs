using System;

namespace EntitiesWithDI.Domain
{
	public class Product
	{
		public virtual Guid Id { get; set; }
		public virtual string Description { get; set; }
		public virtual decimal Price { get; set; }
	}
}