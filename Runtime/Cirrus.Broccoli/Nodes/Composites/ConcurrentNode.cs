using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cirrus.Collections;
using static Cirrus.Debugging.DebugUtils;
using System;

namespace Cirrus.Broccoli 
{
	public partial class ConcurrentNodeInstance : CompositeNodeInstanceBase
	{
		public ConcurrentNodeInstance() : base()
		{
		}

		public ConcurrentNodeInstance(string name) : base(name)
		{
		}
		public ConcurrentNodeInstance(string name, object obj) : base(name, obj)
		{
		}

		public ConcurrentNodeInstance(object obj) : base(obj)
		{
		}

		public ConcurrentNodeInstance(
			NodeResultQuantifier success = NodeResultQuantifier.One
			, NodeResultQuantifier failure = NodeResultQuantifier.One
			) : base()
		{
			Success = success;
			Failure = failure;
		}

		public ConcurrentNodeInstance(
		string name,
		NodeResultQuantifier success = NodeResultQuantifier.One
		, NodeResultQuantifier failure = NodeResultQuantifier.One
		) : base(name)
		{
			Success = success;
			Failure = failure;
		}


		protected override void _Init()
		{
			base._Init();
		}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			for (int i = 0; i < Children.Count; i++)
			{
				Assert(Children[i].Parent == this, "Child's parent is invalid.", true);
				Assert(Children[i].State == NodeState.Inactive, true);
			}

			_areChildrenAborted = false;
			_runningCount = 0;
			_succeededCount = 0;
			_failedCount = 0;
			for (int i = 0; i < Children.Count; i++)
			{
				_runningCount++;
				Children[i].Start();
			}
		}

		protected override void _Stop()
		{
			Assert(
				_runningCount + _succeededCount + _failedCount == Children.Count,
				$"running:{_runningCount}, succeeded:{_succeededCount}, failed:{_failedCount}, children:{Children.Count}");

			for (int i = 0; i < Children.Count; i++)
			{
				if (Children[i].State == NodeState.Active)
				{
					Children[i].Stop();
				}
			}
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool result)
		{
			_runningCount--;
			if (result)
			{
				_succeededCount++;
			}
			else
			{
				_failedCount++;
			}
			_childrenResults[child] = result;

			bool allChildrenStarted = _runningCount + _succeededCount + _failedCount == Children.Count;
			if (allChildrenStarted)
			{
				if (_runningCount == 0)
				{
					if (!_areChildrenAborted) // if children got aborted because rule was evaluated previously, we don't want to override the successState
					{
						if (Failure == NodeResultQuantifier.One && _failedCount > 0)
						{
							_successState = false;
						}
						else if (Success == NodeResultQuantifier.One && _succeededCount > 0)
						{
							_successState = true;
						}
						else if (Success == NodeResultQuantifier.All && _succeededCount == Children.Count)
						{
							_successState = true;
						}
						else
						{
							_successState = false;
						}
					}
					_OnStopped(_successState);
				}
				else if (!_areChildrenAborted)
				{
					Assert(_succeededCount != Children.Count);
					Assert(_failedCount != Children.Count);

					if (Failure == NodeResultQuantifier.One && _failedCount > 0/* && waitForPendingChildrenRule != Wait.ON_FAILURE && waitForPendingChildrenRule != Wait.BOTH*/)
					{
						_successState = false;
						_areChildrenAborted = true;
					}
					else if (Success == NodeResultQuantifier.One && _succeededCount > 0/* && waitForPendingChildrenRule != Wait.ON_SUCCESS && waitForPendingChildrenRule != Wait.BOTH*/)
					{
						_successState = true;
						_areChildrenAborted = true;
					}

					if (_areChildrenAborted)
					{
						for (int i = 0; i < Children.Count; i++)
						{
							if (Children[i].State == NodeState.Active)
							{
								Children[i].Stop();
							}
						}
					}
				}
			}
		}

		public override void StopLowerPriorityChildrenForChild(NodeInstanceBase abortForChild, bool immediateRestart)
		{
			if (immediateRestart)
			{
				Assert(abortForChild.State != NodeState.Active);
				if (_childrenResults[abortForChild])
				{
					_succeededCount--;
				}
				else
				{
					_failedCount--;
				}
				_runningCount++;
				abortForChild.Start();
			}
			else
			{
				throw new Exception("On Parallel Nodes all children have the same priority, thus the method does nothing if you pass false to 'immediateRestart'!");
			}
		}
	}
}