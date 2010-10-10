using System.Collections.Generic;

namespace EntitiesAbstrDI.Domain
{
	public interface IInvoice : IEntity
	{
		string Description { get; set; }
		IList<IInvoiceItem> Items { get; set; }
		IInvoiceItem AddItem(IProduct product, int quantity);
		decimal Tax { get; set; }
		decimal Total { get; }
	}
}