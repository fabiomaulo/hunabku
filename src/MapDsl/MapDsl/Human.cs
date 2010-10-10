using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace MapDsl
{
	public class Human : Mammal
	{
		public virtual Name Name { get; set; }

		public virtual string NickName { get; set; }

		public virtual ICollection<Human> Friends { get; set; }

		public virtual ICollection<DomesticAnimal> Pets { get; set; }

		public virtual IDictionary<string, Human> Family { get; set; }

		public virtual double Height { get; set; }

		public virtual long BigIntegerValue { get; set; }

		public virtual decimal BigDecimalValue { get; set; }

		public virtual int IntValue { get; set; }

		public virtual float FloatValue { get; set; }

		public virtual ISet<string> NickNames { get; set; }

		public virtual IDictionary<string, Address> Addresses { get; set; }
	}
}