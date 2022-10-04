//using Cirrus.Arpg.AI;
//using UnityEngine;

////using System.Numerics;
//using static Cirrus.Arpg.AI.ContextSteeringUtils;

//namespace Cirrus.Arpg.Content
//{
//	public partial class AvoidContextEvaluator : ContextEvaluatorBase
//	{
//		public static int _ObjectLayers = Layers.CharacterFlag;

//		public override bool Update(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			ContextMap interests = new ContextMap(context.Ai.Steering.Resolution);
//			ContextMap avoidances = new ContextMap(context.Ai.Steering.Resolution);
//			var ai = context.Ai;

//			bool dirty = false;
//			//Vector3 forward = steering.Transform.forward;
//			for(int i = 0; i < context.Ai.Steering.Resolution.Count; i++)
//			{
//				// NOTE: Physics.Raycast will ignore if starts inside of the collider See project
//				// settings: "Queries Hit Backfaces"
//				if(!Physics.Raycast(
//					context.Ai.Steering.BodyMiddlePosition,
//					context.Ai.Steering.Resolution[i],
//					out RaycastHit hit,
//					RaycastDistance,
//					_ObjectLayers
//					))
//				{
//					continue;
//				}

//				float distance = Vector3.Distance(hit.point, context.Steering.Collider.ClosestPoint(hit.point));

//				// We should avoid entering the wall
//				avoidances.InsertValue(
//					i,
//					1 - (distance / RaycastDistance),
//					context.Steering.Resolution.Resolution / ContextMapPropagation);

//				// Opposite direction to the wall is useful
//				//int opposite = GetOppositeDirection(steering.Resolution, i);
//				//interests.InsertValue(
//				//	opposite,
//				//	distance / RaycastDistance,
//				//	steering.Resolution.Resolution / ContextMapPropagation);
//				dirty = true;
//			}

//			if(dirty)
//			{
//				Context.Interests = interests.Combine(Context.Interests, InterestFalloff);
//				Context.Avoidances = avoidances.Combine(Context.Avoidances, AvoidanceFalloff);
//				return true;
//			}

//			return false;
//		}
//	}

//	public partial class AvoidTargetsBehaviour : ContextEvaluatorBase
//	{
//	}
//}