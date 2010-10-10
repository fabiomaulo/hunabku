using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;

namespace LessThanGoF.Prototype
{
	public static class SessionExtensions
	{
		public static object SaveDynamic(this ISession session, string entityName, object entity)
		{
			object objToSave = GetObjToSave(entity);
			return session.Save(entityName, objToSave);
		}

		private static object GetObjToSave(object entity)
		{
			object objToSave;
			Type t = entity.GetType();
			if (t.Name.StartsWith("<>"))
			{
				var dynEntity = new Dictionary<string, object>();
				PropertyInfo[] properties = t.GetProperties();
				foreach (var property in properties)
				{
					object propValue = GetObjToSave(property.GetValue(entity, null));
					dynEntity[property.Name] = propValue;
				}
				objToSave= dynEntity;
			}
			else
			{
				objToSave = entity;
			}
			return objToSave;
		}
	}
}