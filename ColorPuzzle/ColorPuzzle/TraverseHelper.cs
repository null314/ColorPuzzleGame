using System.Collections.Generic;
using System.Linq;

namespace TraverseHelperLib
{
	public static class TraverseHelper
	{
		public static IEnumerable<int> Traverse(this int x)
		{
			return Enumerable.Range(0, x);
		}
	}

}
