using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrus.Collections;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using Cirrus.Unity.Numerics;

using static Cirrus.FuncGenerics;
using UnityEngine;
using Cirrus.Animations;
using System.Xml.Linq;
using Cirrus.Arpg.Entities.Characters;

namespace Cirrus.Arpg.AI
{
	// Doubles as seek evaluator
	public interface IEvadeData
	{
		float EvadeMaxPredictionTime { get; }

		AnimationCurve EvadeAvoidanceCurve { get; }

		Range_ EvadeAvoidance { get; }

		float EvadeMaxDistance { get; }
	}

	public partial class EvadeEvaluator<TContext, TNodeData>
	: ContextEvaluatorBase<TContext, TNodeData>
	where TContext : AiBehavtree
	where TNodeData : ISteeringNodeData, IEvadeData
	{
		private List<CharacterInstanceBase> _targets = new List<CharacterInstanceBase>();

		private Vector3 _desired;

		public EvadeEvaluator(Func<TContext, SteeringNodeInstance<TContext, TNodeData>, ContextEvaluatorBase<TContext, TNodeData>, float> weightCb=null)
		: base(weightCb)
		{
		}

		protected override void _Start(TContext context, SteeringNodeInstance<TContext, TNodeData> node)
		{
			base._Start(context, node);

			context.Encounter.Get(_targets);
		}

		private Vector3 _Flee(TContext context, Vector3 target)
		{
			Vector3 force;

			_desired = context.Position - target;
			_desired = context.Kinematics.MaxSpeed * _desired.normalized;

			force = _desired - context.Velocity;
			return force;
		}

		//private Vector3 _Pursuit(TContext context, Vector3 target)
		//{
		//	Vector3 distance = target - context.Position;

		//	float updatesNeeded = distance.magnitude / MAX_VELOCITY;

		//	var tv :Vector3D = target.velocity.clone();
		//	tv.scaleBy(updatesNeeded);

		//	targetFuturePosition = target.position.clone().add(tv);

		//	return seek(targetFuturePosition);
		//}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, TNodeData> node)
		{
			_targets.Foreach(context, node, this, (context, node, eval, target) =>
			{
				Vector3 displ = target.Position - context.Position;
				float dist = displ.magnitude;
				float updatesNeeded = dist / context.Kinematics.MaxSpeed;
				Vector3 targetFuturePosition = target.Position + updatesNeeded * target.Velocity;
				Vector3 flee = eval._Flee(context, targetFuturePosition).normalized;

				if(flee.Almost(Vector3.zero)) return;

				float weight, dot;
				for(int i = 0; i < eval._steering.Count; i++)
				{
					if(eval.Direction(i).Dot(flee, out dot))
					{
						weight = 1.0f - Mathf.Clamp(dist / node.data.EvadeMaxDistance, 0, 1);
						weight = node.data.EvadeAvoidanceCurve.Evaluate(weight);
						weight = node.data.EvadeAvoidance.Lerp(weight);

						eval._interests[i] += dot * weight;
					}
				}
			});

			_interests.For(_targets.Count, (list, count, i) =>
			{
				list[i] /= count;
			});
		}
	}
}
