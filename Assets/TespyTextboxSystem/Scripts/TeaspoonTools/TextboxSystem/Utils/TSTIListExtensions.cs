using System;
using System.Collections.Generic;


public static class TSTIListExtensions
{
	public static void AddRange<T>(this IList<T> list, IEnumerable<T> textToAdd)
	{
		foreach (T text in textToAdd)
			list.Add (text);
	}
		
	public static T LastItem<T>(this IList<T> list)
	{
		return list [list.Count - 1];
	}
}


