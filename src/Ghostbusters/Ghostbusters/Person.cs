namespace Ghostbusters
{
	public enum Sex
	{
		Unspecified,
		Male,
		Female
	}
	public class Person
	{
		public virtual int Id { get; set; }
		public virtual Sex Sex { get; set; }
	}
}