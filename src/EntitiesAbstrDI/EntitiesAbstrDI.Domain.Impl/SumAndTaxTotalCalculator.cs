namespace EntitiesAbstrDI.Domain.Impl
{
	public class SumAndTaxTotalCalculator : IInvoiceTotalCalculator
	{
		#region Implementation of IInvoiceTotalCalculator

		public decimal GetTotal(IInvoice invoice)
		{
			decimal result = invoice.Tax;
			foreach (IInvoiceItem item in invoice.Items)
			{
				result += item.Product.Price * item.Quantity;
			}
			return result;
		}

		#endregion
	}
}