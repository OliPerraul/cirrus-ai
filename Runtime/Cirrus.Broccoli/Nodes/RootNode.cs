using Cirrus.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	public partial class RootNodeInstance
	{
		public RootNodeInstance()
		{ 
		}

		public RootNodeInstance(BehavtreeContextBase context)
		{
			_context = context;
			State = NodeState.Inactive;
		}

		/// <summary>
		/// Defer the scheduling of task until the root node..
		/// </summary>
		/// <param name="node"></param>
		public override void Schedule(TaskNodeInstanceBase node)
		{
			Context.Schedule(node);
		}

		public override void Unschedule(TaskNodeInstanceBase node)
		{
			Context.Unschedule(node);
		}

		protected override void _Init()
		{
			_child.Init();
		}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			Blackboard.Enable();
			Assert(_child != null, true);
			Child.Start();
		}

		protected override void _Stop()
		{
			if (Child.State == NodeState.Active)
			{
				Child.Stop();
			}
			else
			{
				Clock.RemoveTimer(Child.Start);
			}
		}

		protected override void _ChildStopped(NodeInstanceBase node, bool success)
		{
			if (State != NodeState.Stopping)
			{
				// wait one tick, to prevent endless recursions
				Clock.AddTimer(0, 0, Child.Start);
			}
			else
			{
				Blackboard.Disable();
				_OnStopped(success);
			}
		}

		private void _AddChild(NodeInstanceBase child)
		{
			if (child != null)
			{
				Assert(child.Parent == null, "Adding a child with existing parent", true);
				_child = child;
				_child.Parent = this;
				_child.Root = this;

			}
		}

		public override void Add(NodeInstanceBase decoratee)
		{
			_AddChild(decoratee);
		}

		public override IEnumerator<NodeInstanceBase> GetEnumerator()
		{
			return EnumerableUtils.ToEnumerable(Child).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override void OnParentCompositeStopped(CompositeNodeInstanceBase composite)
		{
			base.OnParentCompositeStopped(composite);
			_child.OnParentCompositeStopped(composite);
		}
	}
}