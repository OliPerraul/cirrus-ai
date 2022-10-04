//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Cirrus.Collections;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;

//using static Cirrus.FuncGenerics;
//using UnityEngine;
//using Cirrus.Animations;

//namespace Cirrus.Arpg.AI
//{
//	public interface IPursuitData
//	{
//		// Slow up speed until given radius (max speed outside outside that radius)
//		float PursuitRadius { get; set; } // :Number = 200;

//		Vector3 PursuitPosition { get; set; }
//		//Vector3 DesiredArrival { get; set; }

//		Func<float, float> PursuitInterestCurve { get; set; }

//		Range_ PursuitInterest { get; set; }
//	}

//	// Doubles as seek evaluator
//	public partial class PursuitEvaluator<TData>
//		: ContextEvaluatorBase<TData>
//		where TData : IArriveData
//	{
//		public override void Start(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//		}

//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			VectorUtils.Velocity(node.Data.ArrivalPosition - context.GroundPosition, out Vector3 direction, out float distance);
//			float weight = distance / node.Data.ArrivalRadius;
//			weight = node.Data.ArrivalInterestCurve(weight);
//			weight = node.Data.ArrivalInterest.Lerp(weight);

//			for (int i = 0; i < Context.Directions.Count; i++)
//			{
//				if (Context.Directions[i].Dot(direction, out float dot))
//				{
//					Context.Interests[i] = weight * dot;
//				}
//			}

//			//force = desired - velocity;
//			return true;
//		}
//	}
//}
