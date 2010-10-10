using System;
using System.Collections;
using NHibernate;

namespace DuckTyping
{
	public static class SessionExtensions
	{
		public static void PersistDynamic(this ISession session, string entityName, object entity)
		{
			session.Persist(entityName, GetObjToSave(entity));
		}

		public static object SaveDynamic(this ISession session, string entityName, object entity)
		{
			return session.Save(entityName, GetObjToSave(entity));
		}

		public static void DeleteDynamic(this ISession session, string entityName, object entity)
		{
			session.Delete(entityName, GetObjToSave(entity));
		}

		private static object GetObjToSave(object entity)
		{
			var proxy = entity as IDynamicEntity;
			object objToSave;
			if (proxy != null)
			{
				objToSave = proxy.DynamicHandler.Data;
			}
			else if (entity is Hashtable)
			{
				objToSave = entity;
			}
			else
			{
				objToSave = entity.ToDictionary();
			}

			return objToSave;
		}
	}
}