using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

namespace HunabKu.Web
{
	public static class WebPageExtesions
	{
		private const string LookupActionParamName = "LookupAction";

		public static void ResolveAjaxCallBack(this Page source)
		{
			var queryString = source.Request.QueryString;
			string lookupAction = queryString[LookupActionParamName];
			ResolveAjaxCallBack(source, lookupAction);
		}

		public static void ResolveAjaxCallBack(this Page source, string lookupAction)
		{
			if (string.IsNullOrEmpty(lookupAction))
			{
				return;
			}
			NameValueCollection parameters = source.Request.HttpMethod.Equals("GET")
			                                 	? new NameValueCollection(source.Request.QueryString)
			                                 	: new NameValueCollection(source.Request.Form);
			parameters.Remove(LookupActionParamName);
			InvokeBestMatchMethod(source, lookupAction, parameters);
		}

		private static void InvokeBestMatchMethod(object source, string methodName, NameValueCollection queryString)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			new ParameterBinder(source.GetType(), methodName, queryString).Invoke(source);
		}

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
		}
	}
}
