using System;

namespace EntityNameInAction.Abstraction.Entities
{
	public interface IEntity<TIdentity>: IEquatable<IEntity<TIdentity>>
	{
		TIdentity Id { get; }
	}
}