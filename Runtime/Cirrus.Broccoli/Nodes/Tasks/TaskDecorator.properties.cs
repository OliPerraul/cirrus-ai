using System;
using System.Collections.Generic;
using Cirrus.Objects;
using Cirrus.Unity.Objects;

using UnityEngine.Assertions;


namespace Cirrus.Broccoli
{
	public abstract partial class TaskDecoratorInstanceBase
	: TaskNodeInstanceBase		
	//where TContext : IBtContext
	{
		public object _data;
		public override object Data { get => _data; set => _data = value; }

		public bool _blocked = false;

		//Maybe scheduled items should be a member of the node itself, so that it can be scheduled and unscheduled easily.
		private List<ITaskNodeInstance> _scheduled = new List<ITaskNodeInstance>();

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

	public partial class TaskDecorator : TaskDecoratorInstanceBase
	{
		public Func< TaskDecorator, bool> InitCb;

		public Func< TaskDecorator, NodeResult> EnterCb = null;
		public Func< TaskDecorator, NodeResult> UpdateCb = null;
		public Func< TaskDecorator, NodeResult> ExitCb = null;
	}
}