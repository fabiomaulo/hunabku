using System;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace MappingSource
{
	public class Configuration : NHibernate.Cfg.Configuration
	{
		public void Register(Type entity)
		{
			Dialect dialect = Dialect.GetDialect(Properties);
			Mappings mappings = CreateMappings(dialect);
			SetDefaultMappingsProperties(mappings);
			new EntityMapper(mappings, dialect).Bind(entity);
		}

		private static void SetDefaultMappingsProperties(Mappings mappings)
		{
			mappings.SchemaName = null;
			mappings.DefaultCascade = "none";
			mappings.DefaultAccess = "property";
			mappings.DefaultLazy = true;
			mappings.IsAutoImport = true;
			mappings.DefaultNamespace = null;
			mappings.DefaultAssembly = null;
		}
	}
}