using EntitiesAbstrDI.Domain;

namespace EntitiesAbstrDI.Data
{
	public interface IDao<TEntity> where TEntity: IEntity
	{
		TEntity Get(int id);
		TEntity GetDelayed(int id);
	}
}