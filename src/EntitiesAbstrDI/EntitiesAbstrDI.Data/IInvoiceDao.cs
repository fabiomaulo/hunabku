using System.Collections.Generic;
using EntitiesAbstrDI.Domain;

namespace EntitiesAbstrDI.Data
{
	public interface IInvoiceDao: ICrudDao<IInvoice>
	{
		IList<IInvoice> LastDayList();
	}
}