using Cirrus.Broccoli;
using System;

using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public enum ObserverNodeResult
	{
		Undetermined,
		Success,
		Failure
	}

	/// <summary>
	/// stopsOnChange
	/// </summary>
	public enum ObserverNodeStopMode
	{
		/// <summary>
		/// the decorator will only check it's condition once it is started and will never stop any
		/// running nodes.
		/// </summary>
		None,

		/// <summary>
		/// the decorator will check it's condition once it is started and if it is met, it will
		/// observe the blackboard for changes. Once the condition is no longer met, it will stop
		/// itself allowing the parent composite to proceed with it's next node.
		/// </summary>
		Self,

		/// <summary>
		/// the decorator will check it's condition once it is started and if it's not met, it will
		/// observe the blackboard for changes. Once the condition is met, it will stop the lower
		/// priority node allowing the parent composite to proceed with it's next node.
		/// </summary>
		LowerPriority,

		/// <summary>
		/// the decorator will stop both: self and lower priority nodes.
		/// </summary>
		Both,

		/// <summary>
		/// the decorator will check it's condition once it is started and if it's not met, it will
		/// observe the blackboard for changes. Once the condition is met, it will stop the lower
		/// priority node and order the parent composite to restart the Decorator immediately
		/// </summary>
		ImmediateRestart,

		/// <summary>
		/// the decorator will check it's condition once it is started and if it's not met, it will
		/// observe the blackboard for changes. Once the condition is met, it will stop the lower
		/// priority node and order the parent composite to restart the Decorator immediately. As in
		/// BOTH it will also stop itself as soon as the condition is no longer met.
		/// </summary>
		LowerPriorityImmediateRestart
	}

	public abstract partial class ObserverDecoratorInstanceBase : DecoratorInstanceBase
	{
		public ObserverNodeStopMode StopsOnChange = ObserverNodeStopMode.Self;

		private bool _isObserving;

		public ObserverDecoratorInstanceBase() : base()
		{
		}

		public ObserverDecoratorInstanceBase(string name) : base(name)
		{
		}
		public ObserverDecoratorInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public ObserverDecoratorInstanceBase(object obj) : base(obj)
		{
		}

		public ObserverDecoratorInstanceBase(ObserverNodeStopMode stopsOnChange) : base()
		{
			StopsOnChange = stopsOnChange;
			_isObserving = false;
		}
	}
}