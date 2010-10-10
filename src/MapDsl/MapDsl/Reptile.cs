namespace MapDsl
{
	public class Reptile : Animal
	{
		public virtual float BodyTemperature { get; set; }
	}

	public class Lizard : Reptile {}
}