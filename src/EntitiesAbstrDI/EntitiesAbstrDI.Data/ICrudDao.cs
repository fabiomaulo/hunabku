using EntitiesAbstrDI.Domain;

namespace EntitiesAbstrDI.Data
{
	public interface ICrudDao<TEntity> : IDao<TEntity> where TEntity : IEntity
	{
		TEntity MakePersistent(TEntity entity);
		void MakeTransient(TEntity entity);
	}
}