using Cirrus.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public static class BehavtreeUtils
	{
		public static string FormatNodeName(this string name)
		{
			name = name.Replace("Node", "");
			name = name.Replace("Instance", "");
			if(name != "Decorator") name = name.Replace("Decorator", "");
			return name;
		}

		public static NodeInstanceBase Parent(NodeInstanceBase node, params NodeInstanceBase[] children)
		{
			for(int i = 0; i < children.Length; i++)
			{
				if(children[i] == null) continue;
				node.Add(children[i]);
			}

			return node;
		}
	}
}
