namespace EntitiesAbstrDI.Domain
{
	public interface IInvoiceItem
	{
		IProduct Product { get; set; }
		int Quantity { get; set; }
	}
}