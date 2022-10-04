using System;

using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public abstract partial class ObserverDecoratorInstanceBase
	{
		protected override void _AddChild(NodeInstanceBase decoratee)
		{
			base._AddChild(decoratee);
		}

		//protected override void _Start()
		//{
		//	if (!Context.IsValid) return;
		//	_onStart();
		//	Child.Start();
		//}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			// TODO should we really start observing and _Stopped at the same time.
			// there's an overhead for adding a callback and removing it immediately.
			// NOTE: Only start observing when not satisfied causes some issue with BkbDec
			if (StopsOnChange != ObserverNodeStopMode.None)
			{
				if (!_isObserving)
				{
					_isObserving = true;
					_StartObserving();
				}
			}

			ObserverNodeResult result = _IsSatisfiedInternal();
			if (result == ObserverNodeResult.Failure)
			{
				_OnStopped(false);
			}
			else if (result == ObserverNodeResult.Success)
			{
				Assert(Child != null, true);
				Child.Start();
			}
		}

		protected override void _Stop()
		{
			if(Child.State == NodeState.Active)
			{
				Child.Stop();
				return;
			}

			_OnStopped(true);
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool result)
		{
			Assert(State != NodeState.Inactive);
			if (
				StopsOnChange == ObserverNodeStopMode.None ||
				StopsOnChange == ObserverNodeStopMode.Self)
			{
				if (_isObserving)
				{
					_isObserving = false;
					_StopObserving();
				}
			}
			_OnStopped(result);
		}

		protected override void _ParentCompositeStopped(CompositeNodeInstanceBase parentComposite)
		{
			if (_isObserving)
			{
				_isObserving = false;
				_StopObserving();
			}
		}

		protected void _Evaluate(object o)
		{
			var result = _IsSatisfiedInternal(o);
			if (
				State == NodeState.Active &&
				result == ObserverNodeResult.Failure
				)
			{
				if (
					StopsOnChange == ObserverNodeStopMode.Self
					|| StopsOnChange == ObserverNodeStopMode.Both
					|| StopsOnChange == ObserverNodeStopMode.ImmediateRestart
					)
				{
					// Debug.Log( key + " stopped self ");
					Stop();
				}
			}
			else if (
				State == NodeState.Active &&
				result == ObserverNodeResult.Success
				)
			{
				if (Child.State == NodeState.Inactive)
				{
					Assert(Child != null, true);
					Child.Start();
				}
			}
			else if (
				State != NodeState.Active &&
				result == ObserverNodeResult.Success
				)
			{
				if (
					StopsOnChange == ObserverNodeStopMode.LowerPriority
					|| StopsOnChange == ObserverNodeStopMode.Both
					|| StopsOnChange == ObserverNodeStopMode.ImmediateRestart
					|| StopsOnChange == ObserverNodeStopMode.LowerPriorityImmediateRestart
					)
				{
					// Debug.Log( key + " stopped other ");
					NodeInstanceBase parentNode = Parent;
					NodeInstanceBase childNode = this;
					while (parentNode != null && !(parentNode is CompositeNodeInstanceBase))
					{
						childNode = parentNode;
						parentNode = parentNode.Parent;
					}
					Assert(parentNode != null, "NTBtrStops is only valid when attached to a parent composite", true);
					Assert(childNode != null);
					if (parentNode is ConcurrentNodeInstance)
					{
						Assert(StopsOnChange == ObserverNodeStopMode.ImmediateRestart, "On Parallel Nodes all children have the same priority, thus Stops.LOWER_PRIORITY or Stops.BOTH are unsupported in this context!", true);
					}

					if (
						StopsOnChange == ObserverNodeStopMode.ImmediateRestart
						|| StopsOnChange == ObserverNodeStopMode.LowerPriorityImmediateRestart
						)
					{
						if (_isObserving)
						{
							_isObserving = false;
							_StopObserving();
						}
					}

					((CompositeNodeInstanceBase)parentNode)
						.StopLowerPriorityChildrenForChild(
						childNode,
						StopsOnChange == ObserverNodeStopMode.ImmediateRestart ||
						StopsOnChange == ObserverNodeStopMode.LowerPriorityImmediateRestart);
				}
			}
		}

		protected virtual void _OnObservedSubjectChanged()
		{
		}


		protected virtual void _StartObserving() { }

		protected virtual void _StopObserving() { }

		protected virtual ObserverNodeResult _IsSatisfiedInternal() { return ObserverNodeResult.Failure; }

		protected virtual ObserverNodeResult _IsSatisfiedInternal(object o) { return ObserverNodeResult.Failure; }
	}
}