namespace EntitiesWithDI.Domain
{
	public interface IInvoiceTotalCalculator
	{
		decimal GetTotal(IInvoice invoice);
	}
}