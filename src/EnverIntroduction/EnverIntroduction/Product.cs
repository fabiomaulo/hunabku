using System;

namespace EnverIntroduction
{
	public class Product : Entity
	{
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual Decimal UnitPrice { get; set; }
		public override string ToString()
		{
			return string.Format("({0} {1})", Name, UnitPrice.ToString("C"));
		}
	}
}
