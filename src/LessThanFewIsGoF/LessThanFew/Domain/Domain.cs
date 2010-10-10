using Iesi.Collections.Generic;

namespace LessThanFew.Domain
{
	public interface IPerson
	{
		int Id { get; set; }
		string Name { get; set; }
		IAddress Address { get; set; }
		ISet<IPerson> Family { get; set; }
	}

	public interface ICustomer : IPerson
	{
		ICompany Company { get; set; }
	}

	public interface ICompany
	{
		int Id { get; set; }
		string Name { get; set; }
	}

	public interface IAddress
	{
		int Id { get; set; }
		string Street { get; set; }
		string City { get; set; }
		string PostalCode { get; set; }
	}
}