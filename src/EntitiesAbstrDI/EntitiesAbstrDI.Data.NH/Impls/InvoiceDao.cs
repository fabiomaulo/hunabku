using System.Collections.Generic;
using EntitiesAbstrDI.Domain;
using NHibernate;

namespace EntitiesAbstrDI.Data.NH.Impls
{
	public class InvoiceDao : BaseCrudDao<IInvoice>, IInvoiceDao
	{
		public InvoiceDao(ISessionFactory factory) : base(factory) {}

		#region Implementation of IInvoiceDao

		public IList<IInvoice> LastDayList()
		{
			return Session.GetNamedQuery("Invoice.LastDayList").List<IInvoice>();
		}

		#endregion
	}
}