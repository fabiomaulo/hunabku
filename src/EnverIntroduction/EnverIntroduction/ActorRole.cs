namespace EnverIntroduction
{
	public class ActorRole : Entity
	{
		public virtual Actor Actor { get; set; }
		public virtual string Role { get; set; }

		public override string ToString()
		{
			return string.Format("{0}, Role: {1}", Actor, Role);
		}
	}
}