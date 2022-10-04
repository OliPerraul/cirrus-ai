//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using Cirrus.Numerics;

//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	// TODO: moving obstacles avoidance (RVO)
//	// Perhaps a technique to adopt (extract from navmesh agent) :
//	// https://forum.unity.com/threads/how-to-extract-avoidance-steering-from-the-navmeshagent-component.658765/
//	// Unity navmesh agent is a blackbox
//	// do not roll out your own local collision avoidance, instead

//	public partial class ObstaclesEvaluator<TData>
//	{
//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			//If we aren't moving much, we don't need to try avoid
//			if (context.Kinematics.Velocity.sqrMagnitude <= node.Data.ObstaclesRadius)
//			{
//				return true;
//			}

//			Vector3 raycastOrigin = context.CharaTransforms.BodyMiddlePosition;

//			for (int i = 0; i < Context.Directions.Count; i++)
//			{
//				Vector3 direction = Context.Directions[i];
//				if (direction.Dot(context.Kinematics.Velocity, out float dot))
//				{
//					if (Physics.Raycast(raycastOrigin, direction, out RaycastHit hit, dot, node.Data.ObstaclesLayers))
//					{
//						VectorUtils.FromVelocity(
//						hit.point - context.GroundPosition,
//						out Vector3 dir,
//						out float dist
//						);

//						float weight = 1f - (dist / dot);
//						weight = weight.Clamp(0, 1);
//						weight = node.Data.ObstaclesAvoidanceCurve(weight);
//						weight = node.Data.ObstaclesAvoidance.Lerp(weight);

//						Context.Avoidances[i] = weight;
//					}
//				}
//			}

//			return true;
//		}
//	}
//}
