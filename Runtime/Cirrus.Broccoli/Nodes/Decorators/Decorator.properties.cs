using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public abstract partial class DecoratorInstanceBase : NodeInstanceBase
	{
		//public object _data;
		//public override object Data { get ; set => _data = value; }
		public DecoratorInstanceBase() : base()
		{
		}
		public DecoratorInstanceBase(string name) : base(name)
		{
		}
		public DecoratorInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public DecoratorInstanceBase(object obj) : base(obj)
		{
		}


		protected NodeInstanceBase _child;

		public NodeInstanceBase Child
		{
			get => _child;
			set => _AddChild(value);
		}

		private RootNodeInstance _root;

		public override RootNodeInstance Root
		{
			get => _root;
			set
			{
				_root = value;
				if (Child != null)
				{
					Child.Root = value;
				}
			}
		}
	}

	public partial class DecoratorInstance<TContext, TData> 
	: DecoratorInstanceBase
	where TContext : IContext
	{
		public DecoratorInstance() : base()
		{
		}

		public DecoratorInstance(string name) : base(name)
		{
		}
		public DecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public DecoratorInstance(object obj, Action<TContext, DecoratorInstance<TContext, TData>> initCb)
		: base(obj)
		{
			this.initCb = initCb;
		}

		public DecoratorInstance(
		object obj
		)
		: base(obj)
		{
		}

		public DecoratorInstance(Action<TContext, DecoratorInstance<TContext, TData>> initCb)
		{
			this.initCb = initCb;
		}

		public TContext context;

		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext) value; }

		public TData data;

		public override object Data { get => data; set => data = (TData)value; }

		public Action<TContext, DecoratorInstance<TContext, TData>> initCb = null;

	}

	public partial class DecoratorInstance<TData> : DecoratorInstance<BehavtreeContextBase, TData>
	{
		public DecoratorInstance() : base()
		{
		}

		public DecoratorInstance(string name) : base(name)
		{
		}
		public DecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public DecoratorInstance(object obj) : base(obj)
		{
		}

		public DecoratorInstance(Action<BehavtreeContextBase, DecoratorInstance<BehavtreeContextBase, TData>> initCb) : base(initCb)
		{
		}
	}
}
