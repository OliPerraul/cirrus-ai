using Cirrus.Collections;
using Cirrus.Objects;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	public abstract partial class DecoratorInstanceBase
	{
		public override object Copy()
		{
			var instance = (DecoratorInstanceBase)base.Copy();
			instance.Clear();
			if(Child != null) instance.Add((NodeInstanceBase)Child.Copy());
			return instance;
		}

		public override void OnParentCompositeStopped(CompositeNodeInstanceBase composite)
		{
			base.OnParentCompositeStopped(composite);
			if(Child != null) Child.OnParentCompositeStopped(composite);
		}


		protected override void _Init()
		{		
		}

		protected override void _InitChildren()
		{			
			if (Child != null) Child.Init();
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool succeeded)
		{
			_OnStopped(succeeded);
		}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			if(Child != null) Child.Start();
		}

		protected override void _Stop()
		{
			if(Child != null) Child.Stop();
		}

		protected virtual void _AddChild(NodeInstanceBase child)
		{
			if (child != null)
			{
				Assert(child.Parent == null, "Adding a child with existing parent", true);

				if (_child == null)
				{
					_child = child;
					_child.Parent = this;
				}
				else
				{
					if (_child is not SequenceNodeInstance)
					{
						_child.Parent = null;
						_child = new SequenceNodeInstance { _child };
						_child.Parent = this;
					}

					((SequenceNodeInstance)_child).Add(child);
				}

				child.Root = Root;
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

		public override void Clear()
		{
			_child = null;
		}

		public override IEnumerator<NodeInstanceBase> GetEnumerator()
		{
			return EnumerableUtils.ToEnumerable(Child).GetEnumerator();
		}
	}


	public partial class DecoratorInstance<TContext, TData> : DecoratorInstanceBase
	{
		public override object Copy()
		{
			var instance = (DecoratorInstance<TContext, TData>) base.Copy();
			if(instance.data.IsAssignableTo(out ICopiable data))
			{
				if(ReturnAssert(!data.IsAssignableTo<NodeInstanceBase>()))
				instance.data = (TData)data.Copy();
			}
			return instance;
		}

		protected override void _Init()
		{
			base._Init();
			context = (TContext) (IContext) Root.Context;
			initCb?.Invoke(context, this);
		}
	}
}
