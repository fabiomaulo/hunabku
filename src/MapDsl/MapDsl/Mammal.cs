using System;

namespace MapDsl
{
	public class Mammal : Animal
	{
		public Mammal()
		{
			Birthdate = DateTime.Today;
		}

		public virtual bool Pregnant { get; set; }

		public virtual DateTime Birthdate { get; set; }
	}
}