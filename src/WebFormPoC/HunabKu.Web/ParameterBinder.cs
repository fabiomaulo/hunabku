using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace HunabKu.Web
{
	/// <summary>
	/// Bind a <see cref="NameValueCollection"/> (QuryString) to method parameter array
	/// </summary>
	public class ParameterBinder
	{
		private const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

		private MethodInfo method;

		public ParameterBinder(Type target, string methodName, NameValueCollection parameters)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (string.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}
			Target = target;
			MethodName = methodName;
			Parameters = parameters;
		}

		public Type Target { get; private set; }
		public string MethodName { get; private set; }
		public NameValueCollection Parameters { get; private set; }
		public MethodInfo Method
		{
			get
			{
				if(method == null)
				{
					var candidates = Target.GetMember(MethodName, MemberTypes.Method, DefaultBindingFlags);
					method = GetBestMethodMatch(candidates);
				}
				return method;
			}
		}

		private MethodInfo GetBestMethodMatch(MemberInfo[] candidates)
		{
			var c = GetParameterCount();
			if(candidates.Length == 1)
			{
				return (MethodInfo) candidates[0];
			}
			return candidates.Cast<MethodInfo>().FirstOrDefault(m => m.GetParameters().Length == c);
		}

		private int GetParameterCount()
		{
			return Parameters != null ? Parameters.Count: 0;
		}

		public void Invoke(object instance)
		{
			Method.Invoke(instance, GetParametersValue());
		}

		private object[] GetParametersValue()
		{
			ParameterInfo[] pi = Method.GetParameters();
			var result = new object[pi.Length];
			for (int i = 0; i < pi.Length; i++)
			{
				var parameterInfo = pi[i];
				var parameterType = parameterInfo.ParameterType;
				if (!parameterType.IsClass || typeof(IConvertible).IsAssignableFrom(parameterType))
				{
					result[i] = Parameters[parameterInfo.Name].ChangeType(parameterType);
				}
				else
				{
					var instance = Activator.CreateInstance(parameterType);
					var props = parameterType.GetProperties(DefaultBindingFlags);
					var metaObj = new QueryStringToGraph().Parse(Parameters);
					FillInstance(props, metaObj, instance);
					result[i] = instance;
				}
			}
			return result;
		}

		private void FillInstance(PropertyInfo[] props, ParamObject metaObj, object instance)
		{
			foreach (var propertyInfo in props)
			{
				bool destIsCollection = typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
				                        && propertyInfo.PropertyType != typeof (string);
				var metap = metaObj.Properties.FirstOrDefault(p => p.PropertyName == propertyInfo.Name);
				if(metap == null)
				{
					continue;
				}
				if (metap.PropertyType == PropertyType.ValueType && !destIsCollection)
				{
					propertyInfo.SetValue(instance, ((string)metap.Value).ChangeType(propertyInfo.PropertyType), null);
				}
				else if (metap.PropertyType == PropertyType.ParamObject && propertyInfo.PropertyType.IsClass)
				{
					var rinstance = Activator.CreateInstance(propertyInfo.PropertyType);
					var rprops = propertyInfo.PropertyType.GetProperties(DefaultBindingFlags);
					var rmetaObj = (ParamObject)metap.Value;
					FillInstance(rprops, rmetaObj, rinstance);
					propertyInfo.SetValue(instance, rinstance, null);
				}
				else if (destIsCollection)
				{
					propertyInfo.SetValue(instance, ResolveCollection(propertyInfo, metap.Value), null);
				}
				else
				{
					throw new AmbiguousMatchException("A no collection property may have multiple values... or something like that.");
				}
			}
		}

		private object ResolveCollection(PropertyInfo propertyInfo, object pvalues)
		{
			string[] values;
			if(pvalues is string)
			{
				values = new[] {(string) pvalues};
			}
			else
			{
				values = (string[]) pvalues;
			}
			var type = propertyInfo.PropertyType;
			Type elementType = typeof(object);
			if(type.IsGenericType)
			{
				var genericArguments = type.GetGenericArguments();
				if (genericArguments.Length > 1 || !genericArguments[0].IsValueType)
				{
					throw new NotSupportedException();
				}
				elementType = genericArguments[0];
				if(!type.IsAssignableFrom(typeof(IEnumerable<>).MakeGenericType(elementType)))
				{
					throw new NotSupportedException();					
				}
				else
				{
					var result = (IList) Activator.CreateInstance(typeof (List<>).MakeGenericType(elementType));
					foreach (var value in values)
					{
						result.Add(value.ChangeType(elementType));
					}
					return result;
				}
			}
			else
			{
				return values;
			}
		}
	}
}