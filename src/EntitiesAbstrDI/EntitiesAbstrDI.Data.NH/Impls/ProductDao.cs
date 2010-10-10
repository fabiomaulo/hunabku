using System.Collections.Generic;
using EntitiesAbstrDI.Domain;
using NHibernate;

namespace EntitiesAbstrDI.Data.NH.Impls
{
	public class ProductDao : BaseCrudDao<IProduct>, IProductDao
	{
		public ProductDao(ISessionFactory factory) : base(factory) {}

		#region Implementation of IProductDao

		public void PersistsSerie(IEnumerable<IProduct> products)
		{
			foreach (IProduct product in products)
			{
				Session.SaveOrUpdate(product);
			}
		}

		#endregion
	}
}