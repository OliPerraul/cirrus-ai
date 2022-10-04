//using Cirrus.Unity.Objects;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Random = UnityEngine.Random;

//namespace Cirrus.Arpg.AI
//{
//	public class WanderSteering2 : WanderSteeringBase
//	{

//		public Number WanderRadius = 1.2f;

//		public Number WanderDistance = 2f;

//		/// <summary>
//		/// Maximum amount of random displacement a second
//		/// </summary>
//		public Number WanderJitter = 40f;

//		Vector3 _wanderTarget;

//		protected override Vector3 _GetSteering(CharacterAI ai)
//		{
//			//  Get the jitter for this time frame
//			float jitter = WanderJitter * Time.deltaTime;

//			// Add a small random vector to the target's position
//			_wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, 0f, Random.Range(-1f, 1f) * jitter);

//			// Make the wanderTarget fit on the wander circle again
//			_wanderTarget.Normalize();
//			_wanderTarget *= WanderRadius;

//			//  Move the target in front of the character
//			Vector3 targetPosition = context.Character.Transform.position + context.Character.Transform.right * WanderDistance + _wanderTarget;

//			//Debug.DrawLine(transform.position, targetPosition);

//			return context.SteeringSupport.Seek(targetPosition);
//		}
//	}
//}
