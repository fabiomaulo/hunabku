using System;

namespace EntityNameInAction.Abstraction.Entities.Naturalness
{
	public interface IHuman : IAnimal
	{
		string Name { get; set; }
		string NickName { get; set; }
		DateTime Birthdate { get; set; }
	}
}