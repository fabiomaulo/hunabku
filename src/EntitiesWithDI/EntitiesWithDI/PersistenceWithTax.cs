using System.Collections.Generic;
using Castle.Core;
using EntitiesWithDI.Domain;
using NHibernate;
using NUnit.Framework;

namespace EntitiesWithDI
{
	[TestFixture]
	public class PersistenceWithTax : BaseTest
	{
		protected override void ConfigureWindsorContainer()
		{
			container.AddComponent<IInvoiceTotalCalculator, SumAndTaxTotalCalculator>();
			container.AddComponentLifeStyle(typeof (Invoice).FullName, 
				typeof (IInvoice), typeof (Invoice), LifestyleType.Transient);
		}

		[Test]
		public void CRUD()
		{
			Product p1;
			Product p2;
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					p1 = new Product {Description = "P1", Price = 10};
					p2 = new Product {Description = "P2", Price = 20};
					s.Save(p1);
					s.Save(p2);
					tx.Commit();
				}
			}

			var invoice = DI.Container.Resolve<IInvoice>();
			invoice.Tax = 1000;
			invoice.AddItem(p1, 1);
			invoice.AddItem(p2, 2);
			Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));

			object savedInvoice;
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					savedInvoice = s.Save(invoice);
					tx.Commit();
				}
			}

			using (ISession s = sessions.OpenSession())
			{
				invoice = s.Get<Invoice>(savedInvoice);
				Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));
			}

			using (ISession s = sessions.OpenSession())
			{
				invoice = (IInvoice) s.Load(typeof (Invoice), savedInvoice);
				Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));
			}

			using (ISession s = sessions.OpenSession())
			{
				IList<IInvoice> l = s.CreateQuery("from Invoice").List<IInvoice>();
				invoice = l[0];
				Assert.That(invoice.Total, Is.EqualTo((decimal) (10 + 40 + 1000)));
			}

			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Delete("from Invoice");
					s.Delete("from Product");
					tx.Commit();
				}
			}
		}
	}
}