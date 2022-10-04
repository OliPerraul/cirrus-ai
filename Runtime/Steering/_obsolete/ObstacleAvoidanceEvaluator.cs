//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;

//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public class CollisionAvoidanceEvaluator<TData> : ContextEvaluatorBase<TData>
//	{
//		public int CollisionLayers = BitwiseUtils.Full;

//		public float RaycastDistance = 10f;

//		public float K = 1;

//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			//interestMap = new InterestMap(0f, _contextSteering.SteeringResolution);
//			//dangerMap = new DangerMap(0f, _contextSteering.SteeringResolution);

//			Vector3 forward = context.EntComp.Forward;
//			for(int i = 0; i < Context.Directions.Count; i++)
//			{
//				if(!Context.Directions.Raycast(
//					context.Position,
//					i,
//					RaycastDistance,
//					out RaycastHit hit
//					))
//				{
//					continue;
//				}

//				if (hit.collider == null) continue;
//				ObjectComponentBase o = hit.collider.GetComponentInParent<ObjectComponentBase>();
//				if(o == context.EntComp) continue;

//				float dot = Mathf.Abs(
//					Vector3.Dot(Context.Directions[i],
//					forward
//					));

//				float distance = Vector3.Distance(hit.point, hit.collider.ClosestPoint(hit.point));

//				//Context.Avoidances.InsertValue(
//				//	i, Mathf.Min(K / (distance * distance), dot), (int)Context.Directions.Resolution / 8);
//				Context.Avoidances[i] = Mathf.Min(K / (distance * distance), dot);

//				//Context.Interests.InsertValue(
//				//	Context.Directions.GetOppositeDirection(i), 
//				//	Mathf.Min(K / (distance * distance), dot), 
//				//	(int)Context.Directions.Resolution / 8);
//				Context.Interests[Context.Directions.GetOppositeDirection(i)] = Mathf.Min(K / (distance * distance), dot);
//			}
//			return true;
//		}
//	}
//}
