namespace EntitiesAbstrDI.Domain.Impl
{
	public class InvoiceItem : IInvoiceItem
	{
		protected InvoiceItem() {}

		public InvoiceItem(IProduct product, int quantity)
		{
			Product = product;
			Quantity = quantity;
		}

		#region IInvoiceItem Members

		public IProduct Product { get; set; }
		public int Quantity { get; set; }

		#endregion
	}
}