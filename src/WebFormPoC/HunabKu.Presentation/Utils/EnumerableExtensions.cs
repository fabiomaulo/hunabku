using System;
using System.Collections.Generic;
using System.Linq;

namespace HunabKu.Presentation.Utils
{

	public class NameValueElement
	{
		public int Value { get; set; }
		public string Name { get; set; }
	}

	public static class EnumerableExtensions
	{
		public static IEnumerable<NameValueElement> ToNameValue<T>(this IEnumerable<T> source, Func<T, int> valueGetter,
																															 Func<T, string> nameGetter)
		{
			return source.Select(element => new NameValueElement { Value = valueGetter(element), Name = nameGetter(element) });
		}
	}
}