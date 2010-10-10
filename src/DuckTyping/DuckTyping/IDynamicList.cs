using System;
using System.Collections;
using System.Collections.Generic;

namespace DuckTyping
{
	public interface IDynamicList
	{
		
	}

	public class DynamicList<T>: List<T>, IDynamicList
	{

	}
}