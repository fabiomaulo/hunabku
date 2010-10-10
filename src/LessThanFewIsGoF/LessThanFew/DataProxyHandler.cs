using System.Collections.Generic;
using Castle.Core.Interceptor;

namespace LessThanFew
{
	public sealed class DataProxyHandler : IInterceptor
	{
		private readonly Dictionary<string, object> data = new Dictionary<string, object>(50);
		private readonly string entityName;

		public DataProxyHandler(string entityName, object id)
		{
			this.entityName = entityName;
			data["Id"] = id;
		}

		public string EntityName
		{
			get { return entityName; }
		}

		public IDictionary<string, object> Data
		{
			get { return data; }
		}

		#region IInterceptor Members

		public void Intercept(IInvocation invocation)
		{
			invocation.ReturnValue = null;
			string methodName = invocation.Method.Name;
			if ("get_DataHandler".Equals(methodName))
			{
				invocation.ReturnValue = this;
			}
			else if (methodName.StartsWith("set_"))
			{
				string propertyName = methodName.Substring(4);
				data[propertyName] = invocation.Arguments[0];
			}
			else if (methodName.StartsWith("get_"))
			{
				string propertyName = methodName.Substring(4);
				object value;
				data.TryGetValue(propertyName, out value);
				invocation.ReturnValue = value;
			}
			else if ("ToString".Equals(methodName))
			{
				invocation.ReturnValue = entityName + "#" + data["Id"];
			}
			else if ("GetHashCode".Equals(methodName))
			{
				invocation.ReturnValue = GetHashCode();
			}
		}

		#endregion
	}
}