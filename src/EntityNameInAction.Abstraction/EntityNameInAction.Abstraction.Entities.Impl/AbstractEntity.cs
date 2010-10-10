namespace EntityNameInAction.Abstraction.Entities.Impl
{
	public abstract class AbstractEntity<TIdentity> : IEntity<TIdentity>
	{
		#region Implementation of IEntity<TIdentity>

		public abstract TIdentity Id { get; set; }

		#endregion

		#region Implementation of IEquatable<IEntity<TIdentity>>

		public virtual bool Equals(IEntity<TIdentity> other)
		{
			if (null == other || !GetType().IsInstanceOfType(other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			bool otherIsTransient = Equals(other.Id, default(TIdentity));
			bool thisIsTransient = IsTransient();
			if (otherIsTransient && thisIsTransient)
				return ReferenceEquals(other, this);

			return other.Id.Equals(Id);
		}

		protected bool IsTransient()
		{
			return Equals(Id, default(TIdentity));
		}

		public override bool Equals(object obj)
		{
			var that = obj as IEntity<TIdentity>;
			return Equals(that);
		}

		public override int GetHashCode()
		{
			return IsTransient() ? base.GetHashCode() : Id.GetHashCode();
		}
		#endregion
	}
}