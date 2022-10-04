//using Cirrus.AI.States;
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
//using static Cirrus.Objects.PrototypeUtils;

//using Range = Cirrus.Unity.Numerics.Range_;

//namespace Cirrus.Arpg.Content
//{
//	public partial class Ai
//	{
//		public struct ___ExitSteeringData
//		{			
//			public Func<Vector3> DirectionCb;
//			public Entities.EntityFlags Targets;			
//		}

//		public static Lazily<ControlNode<___ExitSteeringData>> _OBSOL_Node_ExitSteering_Setup = Func(() => new ControlNode<___ExitSteeringData>
//		{
//			EnterCb = (context, node) =>
//			{
//				var chara = context.CharaComp;
//				var intern = node.Data;
//				if(chara.Encounter.Get(out List<Door> targets))
//				{
//					context.Target = targets
//					.Where(x => x.DoorComp.Status == DoorStatus.Open)
//					.Where(x => x.DoorComp.Type == DoorType.Exit)
//					.OrderBy(x => (x.Position - chara.Position).magnitude)
//					.FirstOrDefault();
					
//					return ActionNodeResult.Success;
//				}

//				return ActionNodeResult.Failed;
//			}
//		});

//		public static Lazily<SteeringNode<___ExitSteeringData>> _OBSOL_Bt_ExitSteering = Func(() => new SteeringNode<___ExitSteeringData>
//		{
//			EnterCb = (context, node) =>
//			{
//				node.SteeringFlags |= (
//					SteeringFlags.Update_ContextEvaluator_Update |
//					SteeringFlags.CustomUpdate1_CharacterControl |
//					SteeringFlags.CustomUpdate1_Postprocess |
//					SteeringFlags.CustomUpdate1_SteeringContext_UpdateSteering
//					);

//				return ActionNodeResult.Running;
//			},
//			ExitCb = (context, node) =>
//			{
//				node.SteeringFlags &= ~(
//					SteeringFlags.Update_ContextEvaluator_Update |
//					SteeringFlags.CustomUpdate1_CharacterControl |
//					SteeringFlags.CustomUpdate1_Postprocess |
//					SteeringFlags.CustomUpdate1_SteeringContext_UpdateSteering
//					);

//				return ActionNodeResult.Success;
//			},
//			UpdateCb = (context, node) =>
//			{
//				ObjectBase obj = context.Object;
//				SteeringComponent steering = context.Steering;
				
//				Vector3 sourceTarget = context.Target.Position - obj.Position;
//				if(sourceTarget.magnitude <= 0)
//				{
//					steering.Acceleration = Vector3.zero;
//					context.Control.LocomotionVelocity = Vector3.zero;
//					steering.DesiredLerpRotation = Quaternion.LookRotation(
//						sourceTarget.normalized,
//						obj.Transform.up);
//					return ActionNodeResult.Success;
//				}

//				return ActionNodeResult.Running;
//			},
//			Evals = new ContextEvaluator<___ExitSteeringData>
//			{
//				UpdateCb = (context, node, eval) =>
//				{
//					Vector3 direction = node.Data.DirectionCb();
//					for(int i = 0; i < eval.Context.Directions.Count; i++)
//					{
//						eval.Context.Interests[i] = 0;
//						float dot = Vector3.Dot(eval.Context.Directions[i], direction);
//						if(dot > 0) eval.Context.Interests[i] = dot;
//					}
//					return true;
//				}
//			}
//		});
//	}
//}
