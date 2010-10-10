using NHibernate;

namespace LessThanFew
{
	public class EntityNameInterceptor : EmptyInterceptor
	{
		public override string GetEntityName(object entity)
		{
			string entityName = ExtractEntityName(entity) ?? base.GetEntityName(entity);
			return entityName;
		}

		private static string ExtractEntityName(object entity)
		{
			// Our custom Proxy instances actually bundle
			// their appropriate entity name, so we simply extract it from there
			// if this represents one of our proxies; otherwise, we return null
			var pm = entity as IProxyMarker;
			if (pm != null)
			{
				var myHandler = pm.DataHandler;
				return myHandler.EntityName;
			}
			return null;
		}
	}
}