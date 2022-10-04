using System;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Collections;
using Cirrus.Objects;
using Cirrus.Unity.Objects;

using UnityEngine.Assertions;
using static Cirrus.Debugging.DebugUtils;


namespace Cirrus.Broccoli
{
	/// <summary>
	/// TODO: subclass this if extra update method needed (see control node)
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	/// <typeparam name="TData"></typeparam>
	public abstract partial class TaskDecoratorInstanceBase 
		: TaskNodeInstanceBase
		, IEnumerable<NodeInstanceBase>
	{
		public TaskDecoratorInstanceBase(string name) : base(name)
		{
		}
		public TaskDecoratorInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public TaskDecoratorInstanceBase(object obj) : base(obj)
		{
		}

		public TaskDecoratorInstanceBase() : base()
		{
		}

		protected virtual void _AddChild(NodeInstanceBase child)
		{
			if (child != null)
			{
				Assert(child.Parent == null, "Adding a child with existing parent", true);
				_child = child;
				_child.Parent = this;
				_child.Root = Root;

			}
		}

		protected override void _Init()
		{
			_child.Init();
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

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);
			Parent.Unschedule(this);
		}

		public override void Schedule(TaskNodeInstanceBase node)
		{
			_scheduled.Add(node);
		}

		public override void Unschedule(TaskNodeInstanceBase node)
		{
			_scheduled.Remove(node);
		}

		protected override void _Start()
		{
			NodeResult result = _Enter();
			if (			
				result == NodeResult.Failed ||
				result == NodeResult.Success
				)
			{
				_OnStopped(result == NodeResult.Success);
			}
			else if(result == NodeResult.Running)
			{
				result = _Update();
				if (result != NodeResult.None)
				{
					if (
						result == NodeResult.Running ||
						result == NodeResult.Blocked
						)
					{
						Parent.Schedule(this);
						if (result == NodeResult.Blocked)	_blocked = true;
						Child.Start();

					}
					else
					{
						_OnStopped(result == NodeResult.Success);
					}
				}
			}
		}

		public override void Update()
		{
			_blocked = false;
			NodeResult result = _Update();
			if (
				result != NodeResult.Running &&
				result != NodeResult.Blocked)
			{
				_OnStopped(result == NodeResult.Success);
			}
			else if (result != NodeResult.Blocked)
			{
				_scheduled.Foreach(node => node.Update());
			}
			else _blocked = true;
		}

		protected override void _Stop()
		{
			NodeResult result = _Exit();
			Assert(result == NodeResult.Running, "The Task has to return Result.SUCCESS, Result.FAILED/BLOCKED after beeing cancelled!");
			_OnStopped(result == NodeResult.Success);
			Child.Stop();
		}

		public override void FixedUpdate()
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.FixedUpdate());
			}
		}

		public override void LateUpdate()
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.LateUpdate());
			}
		}

		public override void OnDrawGizmos()
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.LateUpdate());
			}
		}

		public override void CustomUpdate1(float dt)
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.CustomUpdate1(dt));
			}
		}

		public override void CustomUpdate2(float dt)
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.CustomUpdate2(dt));
			}
		}

		public override void CustomUpdate3(float dt)
		{
			if (!_blocked)
			{
				_scheduled.Foreach(node => node.CustomUpdate3(dt));
			}
		}
	}	

	public partial class TaskDecorator	
	{
		protected override void _Init()
		{
			//InitCb?.Invoke(Context, this);
		}

		protected override NodeResult _Exit()
		{
			//return ExitCb == null ? base._Exit() : ExitCb.Invoke(Context, this);
			return NodeResult.Success;
		}

		protected override NodeResult _Enter()
		{
			//if (!Context.IsValid) return ActionNodeResult.Error;
			return EnterCb == null ? base._Enter() : EnterCb.Invoke(this);
			//return ActionNodeResult.Success;
		}

		protected override NodeResult _Update()
		{
			return UpdateCb == null ? base._Update() : UpdateCb.Invoke(this);
		}
	}
}