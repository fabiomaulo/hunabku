using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace EnverIntroduction
{
	public class Customer : Entity
	{
		private Iesi.Collections.Generic.ISet<Product> preferredProducts;

		public Customer()
		{
			preferredProducts = new HashedSet<Product>();
		}

		public virtual string Name { get; set; }

		public ICollection<Product> PreferredProducts
		{
			get { return preferredProducts; }
		}

		public override string ToString()
		{
			var result = new StringBuilder(128);
			result.AppendLine(Name);
			result.AppendLine("Products:" + string.Join(",", preferredProducts.Select(x=> x.ToString()).ToArray()));
			return result.ToString();
		}
	}
}