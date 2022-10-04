// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Collections;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Numerics;
using Cirrus.Unity.Editor;
using Cirrus.Unity.Numerics;
using System;

//using System.Numerics;
using UnityEngine;
using Cirrus.Broccoli;
using static Cirrus.Debugging.DebugUtils;
using System.Xml.Linq;
using System.Collections.Generic;

//using Cirrus.Unity.States._ObsoleteAI2;
//using Cirrus.States._ObsoleteAI;

namespace Cirrus.Arpg.AI
{
	[Flags]
	public enum SteeringNodesFlags
	{
		Context = 1 << 0,
		Locomotion = 1 << 1,
		Rotation = 1 << 2
	}

	public abstract partial class SteeringNodeInstanceBase
	{
	}

	// TODO : Eventually we could have mechanism where multiple steering state could be merged in
	// order to use option utility in a continuous fashion This is not the case for now. All sub
	// behaviour in a steering state are statically defined.
	public partial class SteeringNodeInstance<TContext, TData>
	{
		protected override void _Init()
		{
			base._Init();
			Type type = typeof(TData);
			if(type.IsAssignableTo<NodeInstanceBase>())
				data = (TData)Ancestor(type);
			context = (TContext)(IContext)Root.Context;
			context.Steering.nodes.Add(this);
			initCb?.Invoke(context, this);
		}

		public void Add(ContextEvaluatorBase<TContext, TData> eval)
		{
			evals.Add(eval);
		}

		protected override void _Start()
		{
			base._Start();

			context.Steering.IsEnabled = true;

			steering = new SteeringContext(context.Steering.resolution);
			for(int i = 0; i < evals.Count; i++)
			{
				evals[i]._steering = new SteeringContext(context.Steering.resolution);
				evals[i].Start(context, this);
			}
			
			Parent.Schedule(this);
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			if(context.Steering != null)
			context.Steering.IsEnabled = false;
			context.Unschedule(this);
		}

		//beannnn

		public void OnTimeout()
		{
			//Context_.SetNextState();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		public override void Update()
		{
			base.Update();

			// Merge contexts and not make a decision TODO can we pool these? ;	
			steering.Clear();
			updateCb?.Invoke(context, this);

			for(int i = 0; i < evals.Count; i++)
			{
				evals[i].Clear();
				evals[i].Update(context, this);
				steering.Combine(evals[i]._steering);
			}
		}

#if DEVELOPMENT_BUILD || UNITY_EDITOR
		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			for(int i = 0; i < evals.Count; i++)
			{
				evals[i].OnDrawGizmos(context, this);
			}
		}
#endif
	}

	public partial class SteeringCombineNodeInstance<TContext, TData>
	{
		protected override void _Init()
		{
			base._Init();
			Type type = typeof(TData);
			if(type.IsAssignableTo<NodeInstanceBase>())
				data = (TData)Ancestor(type);
			context = (TContext)(IContext)Root.Context;
			initCb?.Invoke(context, this);
		}


		//public override void OnResourceRealized()
		//{
		//	base.OnResourceRealized();
		//}

		protected override void _Start()
		{
			base._Start();

			context.Steering.IsEnabled = true;

			steering = new SteeringContext(context.Steering.resolution);
			//for(int i = 0; i < evals.Count; i++)
			//{
			//	evals[i].context = new SteeringContext(context.Steering.resolution);
			//}

			Parent.Schedule(this);
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			context.Steering.IsEnabled = false;
			context.Unschedule(this);
		}

		//beannnn

		public void OnTimeout()
		{
			//Context_.SetNextState();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		public override void Update()
		{
			base.Update();

			// Merge contexts and not make a decision TODO can we pool these? ;	
			steering.Clear();
			for(int i = 0; i < context.Steering.nodes.Count; i++)
			{
				if(context.Steering.nodes[i].Parent != Parent) continue;
				steering.Combine(context.Steering.nodes[i].steering);
			}
		}

		public override void CustomUpdate1(float dt)
		{
			base.CustomUpdate1(dt);

			context.Steering.acceleration = Vector3.zero;

			if(
			!steering.interests.Total.Almost(0, data.SteeringInterestEpsilon) ||
			!steering.avoidances.Total.Almost(0, data.SteeringAvoidanceEpsilon)
			)
			{
				if(steering.ComputeVelocity(out Vector3 velocity))
				{
					context.Steering.acceleration += context.Steering.LinearAcceleration * velocity;
				}
			}
		}

#if DEVELOPMENT_BUILD || UNITY_EDITOR
		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			Color[] colors = new Color[steering.Count];
			for(int i = 0; i < steering.Count; i++) colors[i] = Color.yellow;

			for(int i = 0; i < steering.Count; i++)
			{
				float interest = steering.interests[i];
				if(interest > 0) colors[i] = Color.Lerp(Color.yellow, Color.green, interest);
				float avoidance = steering.avoidances[i];
				if(avoidance > 0) colors[i] = Color.Lerp(Color.yellow, Color.red, avoidance);

				var direction = steering.directions[i];
				var position = context.Position + (context.Steering.GizmosDistance * direction);

				using(new ScopedGizmosColor(colors[i]))
				{
					Gizmos.DrawCube(position, context.Steering.GizmosSize * Vector3.one);
				}
			}
		}
#endif
	}

}