//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Cirrus.Collections;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;

//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public interface IEvadeData
//	{
//		float MaxForce { get; set; } // = 0.6;
//		float MaxVelocity { get; set;} //  :Number = 3;
//		//float SlowingRadius { get; set; } // :Number = 200;

//		//public static const MAX_FORCE			:Number = 5.4;
//		//public static const MAX_VELOCITY		:Number = 6;
//	}

//	public partial class EvadeEvaluator<TData>
//		: ContextEvaluatorBase<TData>
//		where TData : IEvadeData
//	{
//		public Vector3 position;
//		public Vector3 velocity;
//		public Vector3 target;
//		public Vector3 desired;
//		public Vector3 steering;
//		public float mass;
		
//		public Vector3 distance;
//		public Vector3 targetFuturePosition;

//		public override void Start(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//		}

//		private Vector3 _Flee(ControlBt context, SteeringNodePhase1<TData> node, Vector3 targetFuturePosition) 
//		{
//			Vector3 force;
			
//			desired = position - targetFuturePosition;
//			// TODO: Normalize on return does not work WARNING
//			desired.Normalize();
//			desired = node.Data.MaxVelocity * desired;
			
//			force = desired - velocity;
			
//			return force;
//		}

//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			CharacterComponent target = context.Target;

//			distance = target.GroundPosition - context.GroundPosition;

//			float updatesNeeded = distance.magnitude / node.Data.MaxVelocity;

//			Vector3 targetVelocity = target.Kinematics.Velocity;
//			targetVelocity = updatesNeeded * targetVelocity;

//			targetFuturePosition = target.GroundPosition + targetVelocity;

//			_Flee(context, node, targetFuturePosition);


//			//force = desired - velocity;
//			return true;
//		}
//	}
//}
