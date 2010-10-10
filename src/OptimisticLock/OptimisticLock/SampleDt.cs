using System;

namespace OptimisticLock
{
	public class SampleDt
	{
		public virtual DateTime Version { get; private set; }
		public virtual string Description { get; set; }
	}
}