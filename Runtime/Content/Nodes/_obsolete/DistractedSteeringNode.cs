
//using Cirrus.Animations;
//using Cirrus.Broccoli;
//using Cirrus.Controls;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Arpg.Entities;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Unity.Objects;
//using Cirrus.Numerics;
//using Cirrus.Unity.Editor;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static Cirrus.Debugging.DebugUtils;

//namespace Cirrus.Arpg.Content.AI
//{
//	public class DistractedSteeringNode : NodeBase
//	{
//		protected override NodeInstanceBase _GetInstance()
//		{
//			return new ConcurrentNodeInstance
//			{
//				new ActionNodeInstance<AiBtBase>(
//				(context, node) =>
//				{
//					Assert(context.CharacterObject != null, true);
//					context.CharacterObject.Bt.onSecondMessageHandler += context.sourceCb;
//					context.Target.CharacterObject.Bt.onSecondMessageHandler += node.data.targetCb;
//					return ActionNodeResult.Running;
//				},
//				(context, node) =>
//				{
//					context.CharacterObject.Bt.onSecondMessageHandler -= node.data.sourceCb;
//					context.target.CharacterObject.Bt.onSecondMessageHandler -= node.data.targetCb;
//					return ActionNodeResult.Success;
//				},
//				(context, node) =>
//				{
//					EntityObjectBase obj = context.EntityObject;
//					SteeringComponent steering = context.Steering;

//					context.ability = node.data.getAbilityCb(context).CreateInstance();
//					if(context.ability != null)
//					{
//						Range_ range = context.ability.Range;
//						Vector3 sourceTarget = context.target.Position - obj.Position;
//						if(sourceTarget.magnitude < range.Max)
//						{
//							steering.acceleration = Vector3.zero;
//							context.Control.locomotion = Vector3.zero;
//							context.Control.desiredLerpRotation = Quaternion.LookRotation(
//								sourceTarget.normalized,
//								obj.Transform.up);
//						}
//					}

//					return ActionNodeResult.Running;
//				},
//				(context, node, dt) =>
//				{
//					if(context.ability != null)
//					{
//						EntityObjectBase obj = context.CharacterObject;
//						SteeringComponent steering = context.Steering;
//						Range_ range = context.ability.Range;

//						// Get the distance to the target If we are within the stopping
//						// radius then stop (minimum distance)
//						// If past the stopping radius then stop
//						Vector3 sourceTarget = context.target.Position - steering.Position;
//						float dist = sourceTarget.magnitude;
//						if(dist < range.Max)
//						{
//							float remainingRangeDist = dist - range.Min;
//							float rangeDist = range.Max - range.Min;
//							float elapsedRangeDist = rangeDist - remainingRangeDist;
//							steering.Control.locomotion = Vector3.Lerp(
//								steering.Control.locomotion,
//								Vector3.zero,
//								elapsedRangeDist / rangeDist
//								);
//							context.Control.desiredLerpRotation = Quaternion.LookRotation(
//								sourceTarget.normalized,
//								steering.EntityObject.Transform.up);
//						}
//					}
//				}),
//				new SteeringNodePhase1<DistractedData>
//				{
//					new ContextEvaluator<DistractedData>((context, node, eval) =>
//					{
//						Vector3 direction = node.data.directionCb(context);
//						for(int i = 0; i < eval.context.Directions.Count; i++)
//						{
//							eval.context.interests[i] = 0;
//							if (eval.context.Directions[i].Dot(direction, out float dot))
//							{
//								eval.context.interests[i] = dot;
//							}
//						}
//						return true;
//					})
//				},
//				new SteeringNodePhase2(),
//				new SteeringNodePhase3(),
//				new LocalAvoidanceNode()
//			};
//		}
//	}
//}
