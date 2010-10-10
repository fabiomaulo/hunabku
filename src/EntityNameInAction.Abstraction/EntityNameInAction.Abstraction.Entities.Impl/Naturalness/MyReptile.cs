using EntityNameInAction.Abstraction.Entities.Naturalness;

namespace EntityNameInAction.Abstraction.Entities.Impl.Naturalness
{
	public class MyReptile : MyAnimal, IReptile
	{
		public float BodyTemperature { get; set; }
	}
}