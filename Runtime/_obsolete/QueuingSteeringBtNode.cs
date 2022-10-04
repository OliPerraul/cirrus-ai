//// using Cirrus.Unity.AI.BehaviourTrees;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Unity.Objects;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using System.Linq;

//using System;
//using System.Collections.Generic;

//using UnityEngine;
//using UnityEngine.SocialPlatforms;

//using static Cirrus.FuncGenerics;
////using static Cirrus.Objects.PrototypeUtils;
//using static Cirrus.Debugging.DebugUtils;

//using Range = Cirrus.Unity.Numerics.Range_;
//using System.Security.Cryptography;
//using UnityEngine.UIElements;
//using Cirrus.Collections;
//using System.Drawing;
//using Cirrus.Arpg.UI;
//using Cirrus.Unity.Objects;

//namespace Cirrus.Arpg.Content
//{
//	public partial class QueuingSteeringNode
//	{
//		public Vector3 Queueing(ControlBt ctx)
//		{
//			Vector3 velocity = ctx.LocomotionVelocity;
//			Vector3 brake = new Vector3();
//			CharacterComponent neighbour = GetNeighborAhead(ctx);


//			if(neighbour != null)
//			{
//				//brake.x = -steering.x * 0.8;
//				//brake.y = -steering.y * 0.8;
//				//int forward = eval.SteeringContext.Resolution.GetDirection(context.Forward);
//				//eval.SteeringContext.Avoidances[]

//				//v.scaleBy(-1);
//				//brake = brake.add(v);
//				//brake = brake.add(separation());

//				//if(distance(position, neighbour.Position) <= MaxQueueRadius)
//				//{
//				//	velocity.scaleBy(0.3);
//				//}
//			}

//			return brake;
//		}

//		public CharacterComponent GetNeighborAhead(ControlBt context)
//		{
//			CharacterComponent ret = null;
//			Vector3 velocity = context.LocomotionVelocity;
//			Vector3 qa = velocity.normalized * MaxQueueAhead;
//			Vector3 ahead = qa + context.Position;
//			for(int i = 0; i < Neighbours.Count; i++)
//			{
//				var neighbour = Neighbours[i];
//				if(neighbour == context) continue;
//				float d = (ahead - neighbour.Position).magnitude;
//				if(d <= MaxQueueRadius)
//				{
//					ret = Neighbours[i];
//					break;
//				}
//			}

//			return ret;
//		}

//		public Vector3 Seek(ControlBt context)
//		{
//			_desired = (Door.Position - context.Position).normalized;
//			_desired = MaxVelocity * _desired;
//			// return force
//			return _desired - context.LocomotionVelocity;
//		}

//		public Vector3 CollisionAvoidance(ControlBt ctx)
//		{
//			Vector3 position = ctx.Position;
//			Vector3 vel = ctx.LocomotionVelocity;
//			Vector3 tv = vel.normalized;						
//			tv = (MaxAvoidForce * vel.magnitude / MaxVelocity) * tv;

//			_ahead = position + tv;

//			ObjectComponentBase mostThreatening = null;

//			for(int i = 0; i < ctx.Steering.Resolution.Count; i++)
//			{				
//				if(!Physics.Raycast(
//					ctx.Steering.BodyMiddlePosition,
//					ctx.Steering.Resolution[i],
//					out RaycastHit hit,
//					RaycastDist,
//					ObstaclesLayers
//					))
//				{
//					if(hit.collider.TryGetComponentInParent(out ObjectComponentBase obstacle))
//					{
//						if(
//							mostThreatening == null ||
//							(position - obstacle.Position).magnitude < (position - mostThreatening.Position).magnitude)
//						{
//							mostThreatening = obstacle;
//						}
//					}
//				}
//			}

//			if(mostThreatening != null)
//			{
//				_avoidance = (_ahead - mostThreatening.Position).normalized;
//				_avoidance = AvoidForce * _avoidance;
//			}
//			else
//			{
//				_avoidance = Vector3.zero;
//			}

//			return _avoidance;
//		}

//		public QueuingSteeringNode()
//		{
//			Evals = new ContextEvaluator((ctx, eval) =>
//			{
//				_steering = Vector3.zero;
//				_steering += Seek(ctx);
//				_steering += CollisionAvoidance(ctx);
//				_steering += Queueing(ctx);
				

//				return true;
//			});
//		}
//	}
//}
