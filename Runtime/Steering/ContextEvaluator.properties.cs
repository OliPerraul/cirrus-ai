using Cirrus.Objects;

using System;

using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public abstract partial class ContextEvaluatorBase<TContext, TData>
	{
		public SteeringContext _steering;

		public DiscreteDirections _directions => _steering.directions;

		public int Count => _steering.Count;

		public Vector3 Direction(int i) => _steering.Direction(i);

		public float Interest(int i) => _steering.Interest(i);

		public float Interest(int i, float val) => _steering.Interest(i, val);

		public float Avoidance(int i) => _steering.Avoidance(i);

		public float Avoidance(int i, float val) => _steering.Avoidance(i, val);

		public ContextMap _interests => _steering.interests;

		public ContextMap _avoidances => _steering.avoidances;

		public Func<TContext, SteeringNodeInstance<TContext, TData>, ContextEvaluatorBase<TContext, TData>, float> weightCb;

	}
}
