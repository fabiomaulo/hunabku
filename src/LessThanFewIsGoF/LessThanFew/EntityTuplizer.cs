using NHibernate.Mapping;
using NHibernate.Tuple;
using NHibernate.Tuple.Entity;

namespace LessThanFew
{
	public class EntityTuplizer : PocoEntityTuplizer
	{
		public EntityTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappedEntity) 
			: base(entityMetamodel, mappedEntity) {}

		protected override IInstantiator BuildInstantiator(PersistentClass persistentClass)
		{
			return new EntityInstantiator(persistentClass.MappedClass);
		}
	}
}