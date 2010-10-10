using System;
using System.Collections;
using LinFu.DynamicProxy;

namespace DuckTyping
{
	public class DynamicHandler : IInterceptor
	{
		private readonly IDictionary dataDictionary;
		private readonly Type destinationType;

		private DynamicHandler(Type type, object source)
		{
			destinationType = type;
			var converter = new ObjectExtensions.ObjectToDictionaryConverter();
			dataDictionary = (IDictionary)converter.ToDictionary(destinationType, source);
		}

		internal DynamicHandler(Type type, IDictionary data)
		{
			destinationType = type;
			dataDictionary = data;
		}

		public IDictionary Data
		{
			get { return dataDictionary; }
		}

		internal static DynamicHandler For<T>(object source) where T : class
		{
			if (ReferenceEquals(null, source))
			{
				return null;
			}
			return new DynamicHandler(typeof (T), source);
		}

		#region Implementation of IInterceptor

		public object Intercept(InvocationInfo info)
		{
			string methodName = info.TargetMethod.Name;
			if ("get_DynamicHandler".Equals(methodName))
			{
				return this;
			}
			else if (methodName.StartsWith("set_"))
			{
				string propertyName = methodName.Substring(4);
				dataDictionary[propertyName] = info.Arguments[0];
			}
			else if (methodName.StartsWith("get_"))
			{
				string propertyName = methodName.Substring(4);
				var t = info.TargetMethod.ReturnType;
				var dictionaryValue = dataDictionary[propertyName];
				if (dictionaryValue !=null && typeof(IEnumerable).IsAssignableFrom(t) && 
					ObjectExtensions.ObjectToDictionaryConverter.ShouldBeConverted(t) &&
					!typeof(IDynamicEntity).IsAssignableFrom(dictionaryValue.GetType()) &&
					!typeof(IDynamicList).IsAssignableFrom(dictionaryValue.GetType()))
				{
					return new ObjectExtensions.ObjectToDictionaryConverter().GetEnumerableProperty(t, dictionaryValue);
				}
				return dictionaryValue;
			}
			else if ("ToString".Equals(methodName))
			{
				return destinationType.ToString();
			}
			return null;
			// here should manage the possible implementation when the target is a concrete class
			// info.TargetMethod.Invoke(Implementation, info.Arguments);
		}

		#endregion
	}
}