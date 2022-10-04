//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;

//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public interface ISeekData
//	{
//		float SeekStoppingDistance { get; }

//		// Slow up speed until given radius (max speed outside outside that radius)
//		float SeekRadius { get; set; } // :Number = 200;

//		Func<float, float> SeekInterestCurve { get; set; }

//		Range_ SeekInterest { get; set; }
//	}

//	public class SeekEvaluator<TData> : ContextEvaluatorBase<TData> where TData : ISeekData 
//	{
//		public override bool Update(
//			ControlBt context, 
//			SteeringNodePhase1<TData> node
//			)
//		{
//			ObjectComponentBase target = context.Target;
//			Vector3 direction = (target.Position.X_Z() - context.Position.X_Z()).normalized;
//			float distance = context.EntComp.ColliderDistance(target);
//			for(int i = 0; i < Context.Directions.Count; i++)
//			{				
//				if(Context.Directions[i].Dot(direction, out float dot))
//				{
//					Context.Interests[i] = dot * Mathf.Clamp(distance - node.Data.SeekStoppingDistance, 0f, 1f);
//				}
//			}

//			return true;
//		}
//	}
//}
