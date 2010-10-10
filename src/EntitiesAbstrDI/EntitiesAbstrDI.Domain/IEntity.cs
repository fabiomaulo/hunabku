using System;

namespace EntitiesAbstrDI.Domain
{
	public interface IEntity: IEquatable<IEntity>
	{
		int Id { get; }
	}
}