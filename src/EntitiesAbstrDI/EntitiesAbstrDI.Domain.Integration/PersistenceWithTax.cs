using System.Collections.Generic;
using EntitiesAbstrDI.Adapters;
using EntitiesAbstrDI.Data;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace EntitiesAbstrDI.Domain.Integration
{
	[TestFixture]
	public class PersistenceWithTax
	{
		[Test]
		public void CRUD()
		{
			var entityFactory = ServiceLocator.Locate<IEntityFactory>();
			var daoFactory = ServiceLocator.Locate<IDaoFactory>();

			#region Pre conditions

			var p1 = entityFactory.Create<IProduct>();
			p1.Description = "P1";
			p1.Price = 10;

			var p2 = entityFactory.Create<IProduct>();
			p1.Description = "P2";
			p1.Price = 20;

			var productDao = daoFactory.GetDao<IProductDao>();
			productDao.PersistsSerie(new[] {p1, p2});

			#endregion

			#region The same test of previous article

			var invoice = entityFactory.Create<IInvoice>();
			invoice.Tax = 1000;
			invoice.AddItem(p1, 1);
			invoice.AddItem(p2, 2);
			Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));

			ICrudDao<IInvoice> invoiceCrud = daoFactory.CrudDaoFor<IInvoice>();
			invoiceCrud.MakePersistent(invoice);
			int savedId = invoice.Id;

			invoice = invoiceCrud.Get(savedId);
			Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));

			invoice = invoiceCrud.GetDelayed(savedId);
			Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));

			var invoiceDao = daoFactory.GetDao<IInvoiceDao>();

			IList<IInvoice> l = invoiceDao.LastDayList();
			invoice = l[0];
			Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));

			#endregion
		}
	}
}