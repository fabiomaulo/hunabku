namespace Company.Domain
{
	public class Person
	{
		public virtual int Id { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }

		public virtual string FullName
		{
			//get { return string.Concat(FirstName, "-", LastName); }
			get { return string.Concat(LastName, "*", FirstName); }
		}
	}
}