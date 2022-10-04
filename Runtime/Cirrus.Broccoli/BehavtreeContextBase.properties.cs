using Cirrus.Unity.Objects;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public interface IContext
	{
		Blackboard Blackboard { get; }
		Clock Clock { get; }

		void Schedule(TaskNodeInstanceBase node);

		void Unschedule(TaskNodeInstanceBase node);

		bool IsValid { get; }
	}

	public abstract partial class BehavtreeContextBase
		: ComponentBase
		, IContext
	{
		public NodeState State => _Root.State;

		protected RootNodeInstance _root;
		protected RootNodeInstance _Root => _root;

		protected bool _running = true;
		public bool IsRunning => _running;

		[NonSerialized]
		private Blackboard _blackboard = null;
		public Blackboard Blackboard => _blackboard;

		[NonSerialized]
		private Clock _clock;
		public Clock Clock => _clock;

		public abstract GameObject Label { get; }

		//Maybe scheduled items should be a member of the node itself, so that it can be scheduled and unscheduled easily.
		private List<ITaskNodeInstance> _scheduled = new List<ITaskNodeInstance>();

		public bool IsValid => gameObject != null;

		//private bool _insideStopped = false;
		//public bool InsideStopped { get => _insideStopped; set => _insideStopped = value; }
	}

	//public class ScopedContextStopped
	//{
	//	private bool _previous;
	//	private ContextBase _context;

	//	public ScopedContextStopped(ContextBase context, bool insideStopped)
	//	{
	//		_previous = context.InsideStopped;
	//		_context = context;
	//		context.InsideStopped = insideStopped;
	//	}

	//	~ScopedContextStopped()
	//	{
	//		_context.InsideStopped = _previous;
	//	}

	//}
}