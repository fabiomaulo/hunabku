using System.Reflection;
using NHibernate.Engine;

namespace DisableAutoDirtyCheck
{
	public static class Extensions
	{
		private static readonly FieldInfo statusFieldInfo = 
			typeof (EntityEntry).GetField("status",BindingFlags.NonPublic | BindingFlags.Instance);
		
		public static void BackSetStatus(this EntityEntry entry, Status status)
		{
			statusFieldInfo.SetValue(entry, status);
		}
	}
}