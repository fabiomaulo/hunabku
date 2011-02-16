namespace EnverIntroduction
{
	public class Actor: Entity
	{
		public virtual string Name { get; set; }
		public virtual string Surname { get; set; }

		public override string ToString()
		{
			return string.Format("{0} {1}", Name, Surname);
		}
	}
}