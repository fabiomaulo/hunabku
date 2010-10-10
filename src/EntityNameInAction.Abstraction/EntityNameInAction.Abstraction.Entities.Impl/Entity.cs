namespace EntityNameInAction.Abstraction.Entities.Impl
{
	public class Entity: AbstractEntity<int>
	{
		private int id;

		#region Overrides of AbstractEntity<int>

		public override int Id
		{
			get { return id; }
			set { id= value; }
		}

		#endregion
	}
}