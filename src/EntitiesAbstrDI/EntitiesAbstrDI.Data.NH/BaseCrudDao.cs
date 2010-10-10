using EntitiesAbstrDI.Domain;
using NHibernate;

namespace EntitiesAbstrDI.Data.NH
{
	public abstract class BaseCrudDao<TEntity> : ICrudDao<TEntity> where TEntity : IEntity
	{
		private readonly ISessionFactory factory;

		protected BaseCrudDao(ISessionFactory factory)
		{
			this.factory = factory;
		}

		protected ISession Session
		{
			get { return factory.GetCurrentSession(); }
		}

		#region ICrudDao<TEntity> Members

		public TEntity MakePersistent(TEntity entity)
		{
			Session.SaveOrUpdate(entity);
			return entity;
		}

		public void MakeTransient(TEntity entity)
		{
			Session.Delete(entity);
		}

		public TEntity Get(int id)
		{
			return Session.Get<TEntity>(id);
		}

		public TEntity GetDelayed(int id)
		{
			return Session.Load<TEntity>(id);
		}

		#endregion
	}
}