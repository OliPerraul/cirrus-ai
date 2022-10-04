using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Numerics;
using MathUtils = Cirrus.Numerics.MathUtils;

using System;

using UnityEngine;

// TODO: Local Avoidance avoid walls using navmesh..


//using System.Numerics;
//using static Cirrus.Arpg.AI.ContextSteeringUtils;

//using static Cirrus.Arpg.AI.ContextSteeringUtils;

namespace Cirrus.Arpg.Content
{
	//public enum SteeringWallDetection { Raycast, Spherecast }

	public interface IWallsData
	{
		//float WallAvoidDistance { get; set; }

		//float WallAvoidSideWhiskerAngle { get; set; }

		//float WallAvoidWhiskerLength { get; set; }

		// NOTE: Cast must be smaller than Wall Avoid Dist
		// to prevent always detecting and walking alongside the wall
		/// <summary>
		/// The distance away from the collision that we wish go
		/// </summary>		
		float WallsRaycastDistance { get; set; }

		//float WallAvoidRayCastMaxDistance { get; set; }

		LayerMask WallsLayers { get; set; }

		//AnimationCurve WallsInterestCurve { get; set; }

		//Range_ WallsInterest { get; set; }

		AnimationCurve WallsAvoidanceCurve { get; set; }

		Range_ WallsAvoidance { get; set; }
	}

	public partial class WallsEvaluator<TContext, TNodeData>
	: ContextEvaluatorBase<TContext, TNodeData>
	where TContext : AiBehavtree
	where TNodeData : ISteeringNodeData, IWallsData
	{
		public WallsEvaluator() : base()
		{
		}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, TNodeData> node)
		{
			//Vector3 forward = steering.Transform.forward;
			for(int i = 0; i < context.Steering.resolution.Count; i++)
			{
				if(!Physics.Raycast(
					context.Steering.BodyMiddlePosition,
					context.Steering.resolution[i],
					out RaycastHit hit,
					node.data.WallsRaycastDistance,
					node.data.WallsLayers
					))
				{
					continue;
				}

				// Dot product as a mesure of similarity
				// How similar if the raycast to current character direction
				//float dot = Mathf.Abs(Vector3.Dot(steering.ContextResolution[i], forward));

				// How close are we to impact? Greater utility (more avoidance) the closer we are
				// the closer the distance the larger the avoidance
				float distance = Vector3.Distance(hit.point, context.Steering.Collider.ClosestPoint(hit.point));

				// We should avoid entering the wall
				//avoidances.InsertValue(
				//	i,
				//	1 - (distance / RaycastDistance),
				//	context.Ai.Steering.Resolution.Resolution / ContextMapPropagation);

				distance = MathUtils.Clamp(distance / node.data.WallsRaycastDistance); // when very far away =1, the avoidance is low
				float avoidance = 1 - distance;

				//interest = node.data.WallsInterestCurve.Evaluate(interest);
				//interest = node.data.WallsInterest.Lerp(interest);

				avoidance = node.data.WallsAvoidanceCurve.Evaluate(avoidance);
				avoidance = node.data.WallsAvoidance.Lerp(avoidance);


				base._steering.avoidances[i] = avoidance;
				base._steering.interests[i] = 0;
			}
		}
	}
}