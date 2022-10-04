// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Broccoli;
using Cirrus.Collections;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Arpg.UI.Legacy;
using Cirrus.Unity.Numerics;
using System;
using System.Collections.Generic;

//using System.Numerics;
using UnityEngine;

using Range = Cirrus.Unity.Numerics.Range_;

namespace Cirrus.Arpg.AI
{
	// 0.01f
	public interface ISteeringNodeData
	{
		float SteeringInterestEpsilon { get; }

		float SteeringAvoidanceEpsilon { get; }

		float SteeringSpeedLerp { get; }
	}

	public abstract partial class SteeringNodeInstanceBase
	: TaskNodeInstanceBase
	{
		public SteeringContext steering;

		public SteeringNodeInstanceBase() : base()
		{
		}

		public SteeringNodeInstanceBase(string name) : base(name)
		{
		}

		public SteeringNodeInstanceBase(object data) : base(data)
		{
		}

		public SteeringNodeInstanceBase(string name, object data) : base(name, data)
		{
		}
	}

	// TODO : Eventually we could have mechanism where multiple steering state could be merged in
	// order to use option utility in a continuous fashion This is not the case for now. All sub
	// behaviour in a steering state are statically defined.
	public partial class SteeringNodeInstance<TContext, TData>
	: SteeringNodeInstanceBase
	where TContext : AiBehavtree
	where TData : ISteeringNodeData
	{
		public TData data;
		public override object Data { get => data; set => data = (TData)value; }

		public TContext context;
		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext)value; }

		public Action<TContext, SteeringNodeInstance<TContext, TData>> initCb = null;

		public Func<TContext, SteeringNodeInstance<TContext, TData>, NodeResult> updateCb = null;

		public List<ContextEvaluatorBase<TContext, TData>> evals = new List<ContextEvaluatorBase<TContext, TData>>();

		public SteeringNodeInstance() : base()
		{
		}

		public SteeringNodeInstance(string name, TData data) : base(name, data)
		{
			this.data = data;
		}


		public SteeringNodeInstance(
		string name
		, TData data
		, Action<TContext, SteeringNodeInstance<TContext, TData>> initCb
		, Func<TContext, SteeringNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base(name, data)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}

		public SteeringNodeInstance(
		string name
		, Action<TContext, SteeringNodeInstance<TContext, TData>> initCb
		, Func<TContext, SteeringNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base(name)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}


		public SteeringNodeInstance(
		string name
		, TData data
		, Func<TContext, SteeringNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base(name, data)
		{
			this.updateCb = updateCb;
		}

		public SteeringNodeInstance(
		string name
		, Func<TContext, SteeringNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base(name)
		{
			this.updateCb = updateCb;
		}


		public SteeringNodeInstance(TData data) : base(data)
		{
			this.data = data;
		}

		public SteeringNodeInstance(object data) : base(data)
		{
		}


		public SteeringNodeInstance(string name) : base(name)
		{
		}

		public SteeringNodeInstance(string name, object data) : base(name, data)
		{
		}
	}

	public partial class SteeringCombineNodeInstance<TContext, TData>
	: SteeringNodeInstanceBase
	where TContext : AiBehavtree
	where TData : ISteeringNodeData
	{
		public TData data;
		public override object Data { get => data; set => data = (TData)value; }

		public TContext context;
		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext)value; }

		public Action<TContext, SteeringCombineNodeInstance<TContext, TData>> initCb = null;

		public Func<TContext, SteeringCombineNodeInstance<TContext, TData>, NodeResult> updateCb = null;

		public SteeringCombineNodeInstance() : base()
		{
		}

		public SteeringCombineNodeInstance(string name, TData data) : base(name, data)
		{
			this.data = data;
		}

		public SteeringCombineNodeInstance(
		string name
		, Action<TContext, SteeringCombineNodeInstance<TContext, TData>> initCb
		, Func<TContext, SteeringCombineNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base(name)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}

		public SteeringCombineNodeInstance(
		Action<TContext, SteeringCombineNodeInstance<TContext, TData>> initCb
		, Func<TContext, SteeringCombineNodeInstance<TContext, TData>, NodeResult> updateCb = null
		) : base()
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}

		public SteeringCombineNodeInstance(TData data) : base(data)
		{
			this.data = data;
		}

		public SteeringCombineNodeInstance(object data) : base(data)
		{
			this.data = (TData)data;
		}

		public SteeringCombineNodeInstance(string name, object data) : base(name, data)
		{
			this.data = (TData)data;
		}

		public SteeringCombineNodeInstance(string name) : base(name)
		{
		}
	}
}