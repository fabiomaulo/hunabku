using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LinFu.DynamicProxy;
using NHibernate.Collection;
using NHibernate.Tuple;

namespace DuckTyping
{
	public static class ObjectExtensions
	{
		private static readonly MethodInfo DynamicConverter = typeof (ObjectExtensions).GetMethod("AsDynamic");
		private static readonly List<Type> IgnoreTypes = new List<Type> {typeof (string)};

		private static readonly ProxyFactory DynamicGenerator = new ProxyFactory();

		public static T AsDynamic<T>(this object entity) where T : class
		{
			if (ReferenceEquals(null, entity))
			{
				return null;
			}
			var fromNh = entity as Hashtable;
			if(fromNh != null)
			{
				if (fromNh.ContainsKey(DynamicMapInstantiator.KEY))
				{
					return DynamicGenerator.CreateProxy<T>(new DynamicHandler(typeof (T), fromNh), typeof (IDynamicEntity),
					                                typeof (IDictionary));
				}
			}
			return DynamicGenerator.CreateProxy<T>(DynamicHandler.For<T>(entity), typeof (IDynamicEntity), typeof (IDictionary));
		}

		internal static IDictionary<string, object> ToDictionary(this object entity)
		{
			return (IDictionary<string, object>) new ObjectToDictionaryConverter().ToDictionary(entity.GetType(), entity);
		}

		internal class ObjectToDictionaryConverter
		{
			private readonly Dictionary<object, object> convContext;
			public ObjectToDictionaryConverter()
			{
				convContext = new Dictionary<object, object>();
			}

			public object ToDictionary(Type destType, object sourceValue)
			{
				if (ReferenceEquals(null, sourceValue))
				{
					return null;
				}
				var sourceType = sourceValue.GetType();
				if (sourceType == typeof(DynamicHandler) || typeof(IDynamicEntity).IsAssignableFrom(sourceType))
				{
					return sourceValue;
				}
				object resultValue;
				Type t = sourceType;
				if (ShouldBeConverted(t))
				{
					if (typeof(IEnumerable).IsAssignableFrom(t))
					{
						return GetEnumerableProperty(destType, sourceValue);
					}
					var dynEntity = new Dictionary<string, object>(50);
					convContext[sourceValue] = dynEntity;
					PropertyInfo[] properties = t.GetProperties();
					foreach (var property in properties)
					{
						PropertyInfo destProp = destType != null ? destType.GetProperty(property.Name) : property;
						if (destProp != null)
						{
							object propValue = property.GetValue(sourceValue, null);
							object convertedValue = null;
							if (!ReferenceEquals(null, propValue) && !convContext.TryGetValue(propValue, out convertedValue))
							{
							  convContext[propValue] = convertedValue;
								convertedValue = ToDictionary(destProp.PropertyType, propValue);
								convContext[propValue] = convertedValue;
							}
							dynEntity[destProp.Name] = convertedValue;
						}
					}
					resultValue = dynEntity;
				}
				else
				{
					resultValue = sourceValue;
				}
				return resultValue;
			}

			public static bool ShouldBeConverted(Type objType)
			{
				return !objType.IsPrimitive && !objType.IsValueType && !IgnoreTypes.Contains(objType);
			}

			public object GetEnumerableProperty(Type destType, object enumerable)
			{
				Type t = destType;
				Type elementType = GetElementType(t, enumerable);
				if (elementType != null && ShouldBeConverted(elementType))
				{
					return !elementType.Name.StartsWith("<>")
									? ConvertKnowType(elementType, (IEnumerable)enumerable)
									: ConvertAnonymousType(elementType, (IEnumerable)enumerable);
				}
				return enumerable;
			}

			private static Type GetElementType(Type t, object enumerable)
			{
				if (t.IsArray || t.IsGenericType)
				{
					Type[] genericTypes = t.IsArray ? new[] { t.GetElementType() } : t.GetGenericArguments();
					if (genericTypes.Length > 1)
					{
						throw new NotSupportedException();
					}
					return genericTypes[0];
				}
				else
				{
					var enumerator = ((IEnumerable)enumerable).GetEnumerator();
					if (enumerator.MoveNext())
					{
						return enumerator.Current.GetType();
					}
				}
				return null;
			}

			private object ConvertAnonymousType(Type elementType, IEnumerable enumerable)
			{
				var result = (IList)Activator.CreateInstance(typeof(DynamicList<>).MakeGenericType(typeof(IDictionary)));
				foreach (var element in enumerable)
				{
					result.Add(ToDictionary(elementType, element));
				}
				return result;
			}

			private static object ConvertKnowType(Type elementType, IEnumerable enumerable)
			{
				var pBag = enumerable as PersistentBag;
				if (pBag != null)
				{
					return Activator.CreateInstance(typeof(PersistentBagGenericWrapper<>).MakeGenericType(elementType), pBag);
				}
				var result = (IList)Activator.CreateInstance(typeof(DynamicList<>).MakeGenericType(elementType));
				MethodInfo conv = DynamicConverter.MakeGenericMethod(elementType);

				foreach (var element in enumerable)
				{
					result.Add(conv.Invoke(null, new[] { element }));
				}
				return result;
			}
		}
	}
}