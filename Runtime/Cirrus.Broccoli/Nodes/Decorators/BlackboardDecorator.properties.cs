using Cirrus.Objects;
using Cirrus.Debugging;
using System;
using static Cirrus.Debugging.DebugUtils;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public partial class BlackboardDecoratorInstance<TValue> 
	: BlackboardDecoratorInstance<Type, TValue>
	{
	}

	public partial class BlackboardDecoratorInstance<TKey, TValue> 
	: ObserverDecoratorInstanceBase
	{
		public Action<BlackboardDecoratorInstance<TKey, TValue>> onSatisfiedHandler;

		public Func<TValue, TValue, bool> compareCb;


		public object data;
		public override object Data { get => data; set => data = value; }

		public TKey Key { get; set; }

		public TValue Value { get; set; }
	}
}