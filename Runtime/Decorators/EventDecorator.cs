using System;

using Cirrus.Collections;
using System.Collections;
using System.Collections.Generic;

using static Cirrus.Debugging.DebugUtils;
using Cirrus.Objects;

// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using Cirrus.Arpg.Conditions;
using Cirrus.Broccoli;
using UnityEngine.UIElements;
using Cirrus.Events;
using System.Xml.Linq;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
	public class EventDecoratorInstance<TContext>
		: ObserverDecoratorInstanceBase
		where TContext
		: BehavtreeContextBase
		, IEntitySupport
	{
		public override object Data { get => null; set { } }

		public TContext behavtree;

		public override BehavtreeContextBase Context { get => behavtree; set => behavtree = (TContext)value; }

		public EventListenerBase<TContext, ObserverNodeResult>[] events = ArrayUtils.Empty<EventListenerBase<TContext, ObserverNodeResult>>();

		public Action<TContext, EventDecoratorInstance<TContext>> initCb;

		protected override void _Init()
		{
			base._Init();
			behavtree = (TContext)Root.Context;
			initCb?.Invoke(behavtree, this);
		}

		public EventDecoratorInstance(
			params EventListenerBase<TContext, ObserverNodeResult>[] conditions)
			: base()
		{
			this.events = conditions;
		}

		public EventDecoratorInstance(
			ObserverNodeStopMode stopsOnChange
			, params EventListenerBase<TContext, ObserverNodeResult>[] conditions)
			: base(stopsOnChange)
		{
			this.events = conditions;
		}

		protected override void _StartObserving()
		{
			for(int i = 0; i < events.Length; i++)
			{
				events[i].Subscribe(behavtree, _OnConditionedStateChanged);
			}
		}

		public void _OnConditionedStateChanged(TContext node, ObserverNodeResult res)
		{
			_Evaluate(res);
		}

		protected override void _StopObserving()
		{
			for(int i = 0; i < events.Length; i++)
			{
				events[i].Unsubscribe(_OnConditionedStateChanged);
			}
		}

		protected override ObserverNodeResult _IsSatisfiedInternal(object o)
		{
			return (ObserverNodeResult)o;
		}

		protected override ObserverNodeResult _IsSatisfiedInternal()
		{
			for(int i = 0; i < events.Length; i++)
			{
				var result = events[i].Evaluate();
				if(result == ObserverNodeResult.Failure) return ObserverNodeResult.Failure;
				if(result == ObserverNodeResult.Undetermined) return ObserverNodeResult.Undetermined;
			}

			return ObserverNodeResult.Success;
		}
	}

	public class EventDecoratorInstance<TContext, TData> 
		: ObserverDecoratorInstanceBase
		where TContext 
		: BehavtreeContextBase
		,IEntitySupport
	{
		public TData data;

		public override object Data { get => data; set => data = (TData)value; }

		public TContext context;

		public override BehavtreeContextBase Context { get => context; set => context = (TContext)value; }

		public EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>[] events = ArrayUtils.Empty<EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>>();

		public Action<TContext, EventDecoratorInstance<TContext, TData>> initCb;

		public override object Copy()
		{
			var inst = (EventDecoratorInstance<TContext, TData>)base.Copy();
			inst.events = new EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>[events.Length];
			for(int i = 0; i < events.Length; i++)
			{
				inst.events[i] = (EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>)events[i].Copy();
			}
			return inst;
		}

		protected override void _Init()
		{
			base._Init();
			context = (TContext)Root.Context;
			initCb?.Invoke(context, this);
		}

		public EventDecoratorInstance(
			params EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>[] conditions)
			: base()
		{
			this.events = conditions;
		}

		public EventDecoratorInstance(
			ObserverNodeStopMode stopsOnChange
			, params EventListenerBase<EventDecoratorInstance<TContext, TData>, ObserverNodeResult>[] conditions)
			: base(stopsOnChange)
		{
			this.events = conditions;
		}

		protected override void _StartObserving()
		{
			for (int i = 0; i < events.Length; i++)
			{
				events[i].Subscribe(this, _OnConditionedStateChanged);
			}
		}

		public void _OnConditionedStateChanged(EventDecoratorInstance<TContext, TData> node, ObserverNodeResult res)
		{
			_Evaluate(res);
		}

		protected override void _StopObserving()
		{
			for (int i = 0; i < events.Length; i++)
			{
				events[i].Unsubscribe(_OnConditionedStateChanged);
			}
		}		

		protected override ObserverNodeResult _IsSatisfiedInternal(object o)
		{
			return (ObserverNodeResult)o;
		}

		protected override ObserverNodeResult _IsSatisfiedInternal()
		{
			for (int i = 0; i < events.Length; i++)
			{
				var result = events[i].Evaluate();
				if (result == ObserverNodeResult.Failure) return ObserverNodeResult.Failure;
				if (result == ObserverNodeResult.Undetermined) return ObserverNodeResult.Undetermined;
			}

			return ObserverNodeResult.Success;
		}
	}
}