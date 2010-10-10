using System;
using EntityNameInAction.Abstraction.Entities.Naturalness;

namespace EntityNameInAction.Abstraction.Entities.Impl.Naturalness
{
	public class MyHuman : MyAnimal, IHuman
	{
		public string Name { get; set; }

		public string NickName { get; set; }

		public DateTime Birthdate { get; set; }
	}
}