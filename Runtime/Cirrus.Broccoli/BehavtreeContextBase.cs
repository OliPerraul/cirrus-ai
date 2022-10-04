using Cirrus.Collections;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public partial class BehavtreeContextBase
	{
		public override void Awake()
		{
			base.Awake();

			_clock = WorldContextComponent.GetClock();
			_blackboard = new Blackboard(_clock);
		}

		public void Stop()
		{
			if (State != NodeState.Inactive)
			{
				_root.Stop();
				_scheduled.Clear();
			}
		}

		public void Start_()
		{
			if (State == NodeState.Inactive)
			{
				_root.Start();
			}
		}

		public void Restart()
		{
			Stop();
			Start_();
		}

		public void Schedule(TaskNodeInstanceBase node)
		{
			Assert(!_scheduled.Contains(node), true);
			_scheduled.Add(node);
		}

		public void Unschedule(TaskNodeInstanceBase node)
		{
			_scheduled.Remove(node);
		}

		public void ForEachTask(Action<TaskNodeInstanceBase> action) => ForEachTask<TaskNodeInstanceBase>(action);

		public void ForEachTask<TNode>(Action<TNode> action)
			where TNode : ITaskNodeInstance
		{
			if (_running)
			{
				_ForEachTask(action);
			}
		}

		public bool AnyTask<TNode>(Func<TNode, bool> action)
			where TNode : ITaskNodeInstance
		{
			if (_running)
			{
				return _AnyTask(action);
			}

			return false;
		}

		private void _ForEachTask(Action<TaskNodeInstanceBase> action) => _ForEachTask<TaskNodeInstanceBase>(action);

		private void _ForEachTask<TNode>(Action<TNode> action)
			where TNode : ITaskNodeInstance, INodeInstance
		{
			for (int i = 0; i < _scheduled.Count; i++)
			{
				if (_scheduled[i].State == NodeState.Active) action((TNode)_scheduled[i]);
			}
		}

		private bool _AnyTask<TNode>(Func<TNode, bool> action)
			where TNode : ITaskNodeInstance, INodeInstance
		{
			for (int i = 0; i < _scheduled.Count; i++)
			{
				if (_scheduled[i].State == NodeState.Active)
				{
					if (action((TNode)_scheduled[i])) return true;
				}
			}

			return false;
		}


		public void Update()
		{
			if (_running)
			{
				_scheduled.Foreach(node => node.Update());
#if UNITY_EDITOR
				if (Label != null) Label.name = string.Join(";", _scheduled.Select(x => x.Name));
#endif
			}
		}

		public virtual void FixedUpdate()
		{
			ForEachTask(node => node.FixedUpdate());
		}

		public virtual void LateUpdate()
		{
			ForEachTask(node => node.LateUpdate());
		}

		public virtual void OnDrawGizmos()
		{
			ForEachTask(node => node.OnDrawGizmos());
		}

		public override void OnDestroy()
		{
			Stop();
		}

		public virtual void CustomUpdate1(float dt)
		{
			ForEachTask(node => node.CustomUpdate1(dt));
		}

		public virtual void CustomUpdate2(float dt)
		{
			ForEachTask(node => node.CustomUpdate2(dt));
		}

		public virtual void CustomUpdate3(float dt)
		{
			ForEachTask(node => node.CustomUpdate3(dt));
		}
	}
}