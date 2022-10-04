using Cirrus.Objects;
using Cirrus.Unity.Randomness;
using System;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public class ChanceDecoratorInstance : DecoratorInstanceBase
	{
		public Chance Chance = 0.5f;

		public override object Data { get => null; set { } }

		//public Action<TContext, ChanceDecorator<TContext, TData>> InitCb;

		public ChanceDecoratorInstance(Chance chance) : base()
		{
			Chance = chance;
		}

		public ChanceDecoratorInstance(string name) : base(name)
		{
		}
		public ChanceDecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public ChanceDecoratorInstance(object obj) : base(obj)
		{
		}


		protected override void _Init()
		{
		}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			if (Chance) Child.Start(); else _OnStopped(false);
		}

		protected override void _Stop()
		{
			Child.Stop();
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool result)
		{
			_OnStopped(result);
		}
	}
}