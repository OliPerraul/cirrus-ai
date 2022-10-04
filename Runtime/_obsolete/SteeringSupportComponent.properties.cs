//using System.Collections.Generic;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class SteeringSupportComponent : MonoBehaviour
//	{
//		[Header("General")]

//		public float MaxVelocity = 3.5f;

//		public float MaxAcceleration = 10f;

//		public float TurnSpeed = 20f;

//		[Header("Arrive")]

//		/// <summary>
//		/// The radius from the target that means we are close enough and have arrived
//		/// </summary>
//		public float TargetRadius = 0.005f;

//		/// <summary>
//		/// The radius from the target where we start to slow down
//		/// </summary>
//		public float SlowRadius = 1f;

//		/// <summary>
//		/// The time in which we want to achieve the targetSpeed
//		/// </summary>
//		public float TimeToTarget = 0.1f;


//		[Header("Look Direction Smoothing")]

//		/// <summary>
//		/// Smoothing controls if the character's look direction should be an
//		/// average of its previous directions (to smooth out momentary changes
//		/// in directions)
//		/// </summary>
//		public bool Smoothing = true;

//		public int NumSamplesForSmoothing = 5;

//		private Queue<Vector3> _velocitySamples = new Queue<Vector3>();


//		private SteeringComponent _rb;
//		private SteeringComponent Steering =>
//			_rb == null ?
//			_rb = GetComponent<SteeringComponent>() :
//			_rb;

//	}
//}