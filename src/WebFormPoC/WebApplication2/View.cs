using Newtonsoft.Json;

namespace WebFormPoC
{
	public abstract class View : System.Web.UI.Page
	{
		protected void JsonResponse(object result)
		{
			string res = JsonConvert.SerializeObject(result);
			Response.ContentType = "application/json";
			Response.Write(res);
			Response.End();
		}
	}
}