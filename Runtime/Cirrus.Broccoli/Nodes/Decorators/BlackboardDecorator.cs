using Cirrus.Objects;
using Cirrus.Debugging;
using System;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public partial class BlackboardDecoratorInstance<TValue>
	{
		public BlackboardDecoratorInstance(
			string name
			, TValue value
			) : base(name)
		{
			Key = value.GetType();
			Value = value;
		}


		/// <summary>
		/// TODO: we must have blackboards which handles multiple states (if the nodes are redundante)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="ctorCb"></param>
		public BlackboardDecoratorInstance(TValue value, Func<TValue, TValue, bool> compare=null) : base()
		{
			Name = value.ToString();
			Key = value.GetType();
			Value = value;
			compareCb = compare;
		}
	}

	public partial class BlackboardDecoratorInstance<TKey, TValue>
	{
		public BlackboardDecoratorInstance() : base()
		{
		}

		public BlackboardDecoratorInstance(string name) : base(name)
		{
		}
		public BlackboardDecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public BlackboardDecoratorInstance(object obj) : base(obj)
		{
		}

		public BlackboardDecoratorInstance(TKey key, TValue value, Func<TValue, TValue, bool> compare=null) : base()
		{
			Key = key;
			Value = value;
			compareCb = compare;
		}

		protected override void _Init()
		{
			//InitCb?.Invoke(Context, this);
		}

		protected override void _OnObservedSubjectChanged()
		{
			_Evaluate(Root.Blackboard.Get(Key));
		}

		protected override void _StartObserving()
		{
			Root.Blackboard.AddObserver(Key, _OnBlackboardValueChanged);
		}

		protected override void _StopObserving()
		{
			Root.Blackboard.RemoveObserver(Key, _OnBlackboardValueChanged);
		}

		private void _OnBlackboardValueChanged(BtBlackboardOp op, object newValue)
		{
			_Evaluate(newValue);
		}

		protected override ObserverNodeResult _IsSatisfiedInternal()
		{
			object result = Root.Blackboard.Get(Key);
			return _IsSatisfiedInternal(result);
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			// TODO: problem here/ for now just make sure to set blackboard value on exit then
			//if (
			//	Cleanup &&
			//	SatisfCb == null &&
			//	Root.Blackboard.Get<TValue>(Key).CompareTo(Value) == 0
			//	)
			//{
			//	Root.Blackboard.Unset(Key, true);
			//}
		}

		protected bool _IsSatisfied(TValue value)
		{
			return compareCb == null ? ((IComparable)value).CompareTo(Value) == 0 : compareCb(value, Value);
		}

		protected override ObserverNodeResult _IsSatisfiedInternal(object newValue)
		{
			//if(!AssertDidNotFail(newValue != null, AssertType.One, "", true)) return ObserverNodeResult.Failure;
			if (newValue == null) return ObserverNodeResult.Failure;
			if (_IsSatisfied((TValue)newValue))
			{
				onSatisfiedHandler?.Invoke(this);
				return ObserverNodeResult.Success;
			}

			return ObserverNodeResult.Failure;
		}
	}
}