using Cirrus.Objects;

using System;

using UnityEngine;

using static Cirrus.Debugging.DebugUtils;


namespace Cirrus.Broccoli
{
	public abstract partial class TaskNodeInstanceBase
	: NodeInstanceBase
	, ITaskNodeInstance
	{
		public TaskNodeInstanceBase(string name) : base(name)
		{
		}

		public TaskNodeInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public TaskNodeInstanceBase(object obj) : base(obj)
		{
		}

		public TaskNodeInstanceBase() : base()
		{
		}

		public virtual void Update()
		{
		}

		protected virtual NodeResult _Enter()
		{
			return NodeResult.Running;
		}

		protected virtual NodeResult _Update()
		{
			return NodeResult.Running;
		}

		protected virtual NodeResult _Exit()
		{
			return NodeResult.Success;
		}

		/// <summary>
		/// TODO investigate this
		/// </summary>
		protected override void _Stop()
		{
			NodeResult result = _Exit();
			Assert(result != NodeResult.Running, "The Task has to return Result.SUCCESS, Result.FAILED/BLOCKED after beeing cancelled!");
			_OnStopped(result == NodeResult.Success);
		}

		public virtual void FixedUpdate()
		{
		}

		public virtual void LateUpdate()
		{
		}

		public virtual void OnDrawGizmos()
		{
		}

		public virtual void CustomUpdate1(float dt)
		{
		}

		public virtual void CustomUpdate2(float dt)
		{
		}

		public virtual void CustomUpdate3(float dt)
		{
		}
	}

	public abstract partial class TaskNodeInstanceBase<TContext, TData>
	{
		public TaskNodeInstanceBase() : base()
		{
		}

		public TaskNodeInstanceBase(string name) : base(name)
		{
		}

		public TaskNodeInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public TaskNodeInstanceBase(object obj) : base(obj)
		{
		}

		protected override void _Init()
		{
			Type type = typeof(TData);
			if(type.IsAssignableTo<NodeInstanceBase>())
				data = (TData)Ancestor(type);
			context = (TContext)(IContext)Root.Context;
		}
	}

	public abstract partial class TaskNodeInstanceBase<TContext>
	{
		public TaskNodeInstanceBase() : base()
		{
		}

		public TaskNodeInstanceBase(string name) : base(name)
		{
		}

		public TaskNodeInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public TaskNodeInstanceBase(object obj) : base(obj)
		{
		}

		protected override void _Init()
		{
			context = (TContext)(IContext)Root.Context;
		}

	}
}