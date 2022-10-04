//using Cirrus.Unity.Objects;
//using Cirrus.Unity.Numerics;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
////using System.Numerics;

//namespace Cirrus.Arpg.AI
//{
//	public class WanderSteering1 : WanderSteeringBase
//	{

//		// THIS ONE IS MORE SPORADIC

//		/// <summary>
//		/// The forward offset of the wander square
//		/// </summary>
//		public Number WanderOffset = 1.5f;

//		/// <summary>
//		/// The radius of the wander square
//		/// </summary>
//		public Number WanderRadius = 4;

//		/// <summary>
//		/// The rate at which the wander orientation can change in radians
//		/// </summary>
//		public Number WanderRate = 0.4f;

//		private float _wanderOrientation = 0;

//		//GameObject debugRing;

//		/* Returns a random number between -1 and 1. Values around zero are more likely. */
//		float _RandomBinomial()
//		{
//			return Random.value - Random.value;
//		}

//		protected override Vector3 _GetSteering(SteeringComponent steering)
//		{
//			float characterOrientation = steering.RotationInRadians;

//			/* Update the wander orientation */
//			_wanderOrientation += _RandomBinomial() * WanderRate;

//			/* Calculate the combined target orientation */
//			float targetOrientation = _wanderOrientation + characterOrientation;

//			/* Calculate the center of the wander circle */
//			Vector3 targetPosition = steering.Character.Transform.position + (SteeringUtils.OrientationToVector(characterOrientation) * WanderOffset);

//			//debugRing.transform.position = targetPosition;

//			/* Calculate the target position */
//			targetPosition = targetPosition + (SteeringUtils.OrientationToVector(targetOrientation) * WanderRadius);

//			//Debug.DrawLine (transform.position, targetPosition);

//			return steering.Seek(targetPosition);
//		}
//	}
//}
