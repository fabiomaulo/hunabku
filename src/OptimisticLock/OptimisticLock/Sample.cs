namespace OptimisticLock
{
	public class Sample
	{
		public virtual int Version { get; private set; }
		public virtual string Description { get; set; }
	}
}