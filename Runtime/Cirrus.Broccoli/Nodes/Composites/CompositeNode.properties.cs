using Cirrus.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	// Existential, Universal
	public enum NodeResultQuantifier
	{
		One,
		All,
	}

	public abstract partial class CompositeNodeInstanceBase
	: NodeInstanceBase
	{
		public CompositeNodeInstanceBase() : base()
		{
		}

		public CompositeNodeInstanceBase(string name) : base(name)
		{
		}
		public CompositeNodeInstanceBase(object obj) : base(obj)
		{
		}

		public CompositeNodeInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		protected object _data;

		public override object Data { get => _data; set => _data = value; }
		

		private RootNodeInstance _root = null;

		public override RootNodeInstance Root
		{
			get => _root;
			set
			{
				if (value != null)
				{
					_root = value;
					for (int i = 0; i < Children.Count; i++)
					{
						var node = Children[i];
						if (!ReturnAssert(node != null)) continue;
						node.Root = value;						
					}
				}
			}
		}

		protected List<NodeInstanceBase> _children = new List<NodeInstanceBase>();

		public IReadOnlyList<NodeInstanceBase> Children
		{
			get => _children;
			set
			{
				if (value != null)
				{
					_children = new List<NodeInstanceBase>(value.Count);
					for (int i = 0; i < value.Count; i++) _AddChild(value[i]);
				}
			}
		}

		public override IEnumerator<NodeInstanceBase> GetEnumerator()
		{
			return ((IEnumerable<NodeInstanceBase>)Children).GetEnumerator();
		}
	}
	
}