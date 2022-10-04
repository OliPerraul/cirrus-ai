using Cirrus.Collections;
using Cirrus.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	public abstract partial class CompositeNodeInstanceBase
	{
		public override object Copy()
		{
			var instance = (CompositeNodeInstanceBase)base.Copy();
			instance._children = new List<NodeInstanceBase>(Children.Count);
			for(int i = 0; i < Children.Count; i++)
			{
				if(Children[i] == null) continue;
				instance.Add((NodeInstanceBase)Children[i].Copy());
			}
			return instance;
		}

		public abstract void StopLowerPriorityChildrenForChild(NodeInstanceBase child, bool immediateRestart);

		protected override void _OnStopped(bool success)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].OnParentCompositeStopped(this);
			}
			base._OnStopped(success);
		}

		protected virtual void _AddChild(NodeInstanceBase child)
		{
			if (child == null) return;
			Assert(child.Parent == null, "Adding a child with existing parent", true);
			_children.Add(child);
			child.Parent = this;
			//Assert(Root != null, "Assigning null root to child.", true);
			child.Root = Root;
		}

		protected override void _InitChildren()
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Init();
			}
		}

		public override void Add(NodeInstanceBase child)
		{
			_AddChild(child);
		}

		public override void Add(IEnumerable<NodeInstanceBase> children)
		{
			foreach(var child in children) _AddChild(child);
		}

	}
}