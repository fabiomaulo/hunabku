namespace EntitiesAbstrDI.Domain.Impl
{
	public class BaseEntity : IEntity
	{
		private int? requestedHashCode;

		#region IEntity Members

		public int Id { get; set; }

		public virtual bool Equals(IEntity other)
		{
			if (null == other || !GetType().IsInstanceOfType(other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			bool otherIsTransient = Equals(other.Id, 0);
			bool thisIsTransient = IsTransient();
			if (otherIsTransient && thisIsTransient)
			{
				return ReferenceEquals(other, this);
			}

			return other.Id.Equals(Id);
		}

		#endregion

		protected bool IsTransient()
		{
			return Equals(Id, 0);
		}

		public override bool Equals(object obj)
		{
			var that = obj as IEntity;
			return Equals(that);
		}

		public override int GetHashCode()
		{
			if (!requestedHashCode.HasValue)
			{
				requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
			}
			return requestedHashCode.Value;
		}
	}
}