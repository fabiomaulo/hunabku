using EntitiesAbstrDI.Domain;

namespace EntitiesAbstrDI.Data
{
	public interface IDaoFactory
	{
		IDao<T> DaoFor<T>() where T : IEntity;
		ICrudDao<T> CrudDaoFor<T>() where T : IEntity;
		TDao GetDao<TDao>() where TDao : class;
	}
}