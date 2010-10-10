using System.Collections.Generic;

namespace EntitiesAbstrDI.Domain.Impl
{
	public class Invoice : BaseEntity, IInvoice
	{
		private readonly IInvoiceTotalCalculator calculator;

		public Invoice(IInvoiceTotalCalculator calculator)
		{
			this.calculator = calculator;
			Items = new List<IInvoiceItem>();
		}

		#region IInvoice Members

		public string Description { get; set; }
		public decimal Tax { get; set; }
		public IList<IInvoiceItem> Items { get; set; }

		public decimal Total
		{
			get { return calculator.GetTotal(this); }
		}

		public IInvoiceItem AddItem(IProduct product, int quantity)
		{
			var result = new InvoiceItem(product, quantity);
			Items.Add(result);
			return result;
		}

		#endregion
	}
}