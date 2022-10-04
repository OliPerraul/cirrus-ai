using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public partial class BlackboardUtils
	{
		public static BlackboardDecoratorInstance<TValue> CreateNodeInstance<TValue>(string name, TValue value)
		{
			return new BlackboardDecoratorInstance<TValue>(name, value);
		}

		public static BlackboardDecoratorInstance<TValue> CreateNodeInstance<TValue>(TValue value, Func<TValue, TValue, bool> compare, NodeInstanceBase node)
		{
			return new BlackboardDecoratorInstance<TValue>(value, compare)
			{
				node
			};
		}


		public static BlackboardDecoratorInstance<TValue> CreateNodeInstance<TValue>(TValue value, NodeInstanceBase node)
		{
			return new BlackboardDecoratorInstance<TValue>(value)
			{
				node
			};
		}
	}
}
