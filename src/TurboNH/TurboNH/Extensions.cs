using System.Reflection;
using NHibernate.Engine;

namespace TurboNH
{
	public static class Extensions
	{
		private static readonly FieldInfo statusFieldInfo = 
			typeof (EntityEntry).GetField("status",BindingFlags.NonPublic | BindingFlags.Instance);
		private static readonly FieldInfo rowIdFieldInfo =
			typeof(EntityEntry).GetField("rowId", BindingFlags.NonPublic | BindingFlags.Instance);
		
		public static void BackSetStatus(this EntityEntry entry, Status status)
		{
			statusFieldInfo.SetValue(entry, status);
		}

		public static void BackSetTracer(this EntityEntry entry, object tracer)
		{
			rowIdFieldInfo.SetValue(entry, tracer);
		}
	}
}