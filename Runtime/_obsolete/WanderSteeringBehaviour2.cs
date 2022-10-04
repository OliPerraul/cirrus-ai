//using Cirrus.Unity.Objects;
//using Cirrus.Unity.Editor;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Random = UnityEngine.Random;

//namespace Cirrus.Arpg.AI
//{
//	[Serializable]
//	public class WanderSteeringBehaviour2 : WanderSteeringBehaviourBase
//	{

//		[SerializeField]
//		
//		public Number WanderRadius = 1.2f;

//		[SerializeField]
//		
//		public Number WanderDistance = 2f;

//		/// <summary>
//		/// Maximum amount of random displacement a second
//		/// </summary>
//		[SerializeField]
//		
//		public Number WanderJitter = 40f;

//		Vector3 _wanderTarget;

//		private static readonly Range _range = new Range(-1f, 1f);

//		protected override SteeringStatus _GetSteering(
//			SteeringComponent steering,
//			float deltaTime,
//			out Vector3 accel)
//		{			
//			//  Get the jitter for this time frame
//			float jitter = WanderJitter * deltaTime;

//			// Add a small random vector to the target's position
//			_wanderTarget += new Vector3(_range.Random() * jitter, 0f, _range.Random() * jitter);

//			// Make the wanderTarget fit on the wander circle again
//			//_wanderTarget.Normalize();
//			_wanderTarget = _wanderTarget.normalized * WanderRadius;

//			//  Move the target in front of the character
//			Vector3 targetPosition = steering.Position + steering.Transform.right * WanderDistance + _wanderTarget;

//			//Debug.DrawLine(transform.position, targetPosition);

//			accel = steering.Seek(targetPosition);

//			return SteeringStatus.Success_Break;
//		}
//	}
//}
