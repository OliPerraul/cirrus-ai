// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Broccoli;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

using None = Cirrus.Objects.None;

namespace Cirrus.Arpg.AI
{
	public abstract partial class ContextEvaluatorBase<TContext, TData> 
	: CopiableBase
	where TContext : AiBehavtree
	where TData : ISteeringNodeData
	{
		public static implicit operator ContextEvaluatorBase<TContext, TData>[](ContextEvaluatorBase<TContext, TData> eval)
		{
			return new ContextEvaluatorBase<TContext, TData>[] { eval };
		}
		public static implicit operator List<ContextEvaluatorBase<TContext, TData>>(ContextEvaluatorBase<TContext, TData> eval)
		{
			return new List<ContextEvaluatorBase<TContext, TData>> { eval };
		}

		public ContextEvaluatorBase()
		{
		}

		public ContextEvaluatorBase(Func<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluatorBase<TContext, TData>, float> weightCb)
		{
			this.weightCb = weightCb;
		}


		public void Clear()
		{
			_steering.Clear();
		}

		protected virtual void _Start(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
		}


		public void Start(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			_Start(context, node);
		}

		protected virtual void _Update(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
		}

		protected float _GetWeight(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			return weightCb == null ? 1.0f : weightCb.Invoke(context, node, this);
		}


		public void Update(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			_Update(context, node);
			this._steering.Multiply(_GetWeight(context, node));
		}

		protected virtual void _OnDrawGizmos(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
		}

		public void OnDrawGizmos(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			_OnDrawGizmos(context, node);
		}
	}

	//public partial class ContextEvaluator<TContext> : ContextEvaluatorBase<TContext, ISteeringData>
	//where TContext : AiBehavtree
	////where TData : ISteeringNodeData
	//{
	//	public Func<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>, bool> updateCb;

	//	public Action<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>> onDrawGizmosCb;

	//	public Action<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>> startCb;

	//	public ContextEvaluator(
	//	//Action<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>> start
	//	Func<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>, bool> update
	//	)
	//	{
	//		//this.startCb = start;
	//		this.updateCb = update;
	//	}


	//	//public ContextEvaluator(Func<TContext, SteeringContextNodeInstance<TContext, ISteeringData>, ContextEvaluator<TContext>, bool> fn = null)
	//	//{
	//	//	updateCb = fn;
	//	//}

	//	//protected override void _Start(TContext context, SteeringContextNodeInstance<TContext, ISteeringData> node)
	//	//{
	//	//	startCb?.Invoke(context, node, this);
	//	//}

	//	protected override void _OnDrawGizmos(TContext context, SteeringContextNodeInstance<TContext, ISteeringData> node)
	//	{
	//		onDrawGizmosCb?.Invoke(context, node, this);
	//	}

	//	protected override void _Update(TContext context, SteeringContextNodeInstance<TContext, ISteeringData> node)
	//	{
	//		updateCb.Invoke(context, node, this);
	//	}
	//}

	public partial class ContextEvaluator<TContext, TData> : ContextEvaluatorBase<TContext, TData>
	where TContext : AiBehavtree
		where TData : ISteeringNodeData
	{
		public Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> updateCb;

		public Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> gizmosCb;

		public Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> startCb;

		public ContextEvaluator(
		Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> updateCb
		, Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> gizmosCb = null
		) : base(null)
		{
			this.updateCb = updateCb;
			this.gizmosCb = gizmosCb;
		}

		public ContextEvaluator(
		Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> startCb
		, Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> updateCb
		, Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> gizmosCb = null
		) : base(null)
		{
			this.startCb = startCb;
			this.updateCb = updateCb;
			this.gizmosCb = gizmosCb;
		}


		public ContextEvaluator(
		Func<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluatorBase<TContext, TData>, float> weightCb
		, Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> updateCb
		, Action<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluator<TContext, TData>> gizmosCb=null
		) : base(weightCb)
		{
			this.updateCb = updateCb;
			this.weightCb = weightCb;
			this.gizmosCb = gizmosCb;
		}

		protected override void _OnDrawGizmos(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			gizmosCb?.Invoke(context, node, this);
		}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			updateCb.Invoke(context, node, this);
		}

		protected override void _Start(TContext context, SteeringNodeInstance<TContext, TData> node)
		{
			startCb?.Invoke(context, node, this);
		}
	}

	//public partial class ContextEvaluator<TContext, TData> : ContextEvaluatorBase<TContext>
	//where TContext : IContext
	//{
	//	public Func<TContext, ContextEvaluator<TContext, TData>, bool> updateCb;

	//	public Action<TContext, ContextEvaluator<TContext, TData>> onDrawGizmosCb;

	//	public Action<TContext, ContextEvaluator<TContext, TData>> startCb;

	//	public TData data;

	//	public ContextEvaluator(
	//	TData data
	//	, Action<TContext, ContextEvaluator<TContext, TData>> start
	//	, Func<TContext, ContextEvaluator<TContext, TData>, bool> update
	//	)
	//	{
	//		this.data = data;
	//		this.startCb = start;
	//		this.updateCb = update;
	//	}

	//	public ContextEvaluator(
	//	TData data
	//	, Func<TContext, ContextEvaluator<TContext, TData>, bool> update
	//	)
	//	{
	//		this.data = data;
	//		this.updateCb = update;
	//	}

	//	public override void Start(TContext context)
	//	{
	//		startCb?.Invoke(context, this);
	//	}

	//	public override void OnDrawGizmos(TContext context)
	//	{
	//		onDrawGizmosCb?.Invoke(context, this);
	//	}

	//	public override bool Update(TContext context)
	//	{
	//		return updateCb.Invoke(context, this);
	//	}
	//}

}