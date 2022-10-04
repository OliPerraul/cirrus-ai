using System;

using Cirrus.Objects;
using Cirrus.Unity.Objects;

using UnityEngine.Assertions;
using static Cirrus.Debugging.DebugUtils;

// SUBTREE DECORATOR

// Make any node appear as a task to be scheduled

namespace Cirrus.Broccoli
{
	public abstract partial class ActionNodeInstanceBase
	{
		protected override void _Init()
		{
		}

		protected override void _OnStopped(bool success)
		{			
			base._OnStopped(success);
			Parent.Unschedule(this);
		}

		protected override void _Start()
		{
			NodeResult result = _Enter();
			if (
				result == NodeResult.Failed
				|| result == NodeResult.Success
				)
			{
				_OnStopped(result == NodeResult.Success);
			}
			else if (
				result == NodeResult.Running
				)
			{
				result = _Update();
				if (result != NodeResult.None)
				{
					if (
						result == NodeResult.Running
						|| result == NodeResult.Blocked
						)
					{
						Parent.Schedule(this);
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
			NodeResult result = _Update();
			if (State != NodeState.Active) return;
			State = NodeState.Active;
			if (
				result != NodeResult.Running &&
				result != NodeResult.Blocked
				)
			{
				_OnStopped(result == NodeResult.Success);
			}
		}

		protected override void _Stop()
		{
			NodeResult result = _Exit();
			Assert(result != NodeResult.Running, "The Task has to return Result.SUCCESS, Result.FAILED/BLOCKED after beeing cancelled!");
			_OnStopped(result == NodeResult.Success);
		}
	}

	public partial class ActionNodeInstance<TContext, TData>
	{
		public override object Copy()
		{
			var instance = (ActionNodeInstance<TContext, TData>)base.Copy();
			if(instance.data != null && instance.data.IsAssignableTo(out ICopiable data))
			{
				if(ReturnAssert(!data.IsAssignableTo<NodeInstanceBase>()))
				instance.data = (TData)data.MemberwiseCopy();
			}
			return instance;
		}

		protected override void _Init()
		{
			base._Init();
			Type type = typeof(TData);
			if(type.IsAssignableTo<NodeInstanceBase>())
				data = (TData)Ancestor(type);
			context = (TContext)(IContext)Root.Context;
			initCb?.Invoke(context, this);
		}

		protected override NodeResult _Exit()
		{
			return exitCb == null ? base._Exit() : exitCb.Invoke(context, this);
		}

		protected override NodeResult _Enter()
		{
			//if (!Context.IsValid) return ActionNodeResult.Error;
			return enterCb == null ? base._Enter() : enterCb.Invoke(context, this);
		}

		protected override NodeResult _Update()
		{
			return updateCb == null ? base._Update() : updateCb.Invoke(context, this);
		}

		public override void LateUpdate()
		{
			lateUpdateCb?.Invoke(context, this);
		}

		public override void CustomUpdate1(float dt)
		{
			customCb1?.Invoke(context, this, dt);
		}

		public override void CustomUpdate2(float dt)
		{
			customCb2?.Invoke(context, this, dt);
		}

		public override void CustomUpdate3(float dt)
		{
			customCb3?.Invoke(context, this, dt);
		}

		public override void OnDrawGizmos()
		{
			onDrawGizmosCb?.Invoke(context, this);
		}
	}

	public partial class ActionNodeInstance<TContext> : ActionNodeInstance<TContext, None>		
	{		

		protected override void _Init()
		{
			base._Init();
			context = (TContext)(IContext)Root.Context;
		}
	}
}