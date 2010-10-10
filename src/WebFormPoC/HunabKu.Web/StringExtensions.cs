using System;

namespace HunabKu.Web
{
	public static class StringExtensions
	{
		public static object ChangeType(this string source, Type destType)
		{
			// TODO: will need some additional test for complex type... for sure
			if (destType == typeof (string))
			{
				return source;
			}
			if (destType.IsValueType && source == null)
			{
				return null;
			}

			return Convert.ChangeType(source, destType);
		}
	}
}