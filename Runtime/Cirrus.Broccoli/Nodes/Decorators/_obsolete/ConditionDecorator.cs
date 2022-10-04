//using System;
//using static Cirrus.Debugging.DebugUtils;

//namespace Cirrus.Unity.AI.BehaviourTrees
//{
//	public class ConditionDecorator<TContext, TData>
//		: DecoratorBase<TContext, TData>
//		where TContext : IBtContext<TContext>
//	{
//		public Func<TContext, ConditionDecorator<TContext, TData>, bool> InitCb;

//		public Func<ConditionDecorator<TContext, TData>, bool> CondCb = node => true;

//		public ConditionDecorator() : base("")
//		{
//		}

//		public ConditionDecorator(string name) : base(name)
//		{
//		}

//		public ConditionDecorator(Func<ConditionDecorator<TContext, TData>, bool> cond) : base(nameof(ConditionDecorator<TContext, TData>))
//		{
//			CondCb = cond;
//		}

//		protected override void _Init()
//		{
//			InitCb?.Invoke(Context, this);
//		}

//		protected override void _Start()
//		{
//			if (!Context.IsValid) return;
//			Assert(Child != null, true);
//			if (CondCb(this)) Child.Start(); else _OnStopped(false);
//		}

//		protected override void _Stop()
//		{
//			Child.Stop();
//		}

//		protected override void _ChildStopped(NodeInstanceBase<TContext> child, bool result)
//		{
//			_OnStopped(result);
//		}
//	}
//}