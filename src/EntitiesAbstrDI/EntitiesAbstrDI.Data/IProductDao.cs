using System.Collections.Generic;
using EntitiesAbstrDI.Domain;

namespace EntitiesAbstrDI.Data
{
	public interface IProductDao: ICrudDao<IProduct>
	{
		void PersistsSerie(IEnumerable<IProduct> products);
	}
}