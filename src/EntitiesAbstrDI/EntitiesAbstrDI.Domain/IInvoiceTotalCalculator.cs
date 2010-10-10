namespace EntitiesAbstrDI.Domain
{
	public interface IInvoiceTotalCalculator
	{
		decimal GetTotal(IInvoice invoice);
	}
}