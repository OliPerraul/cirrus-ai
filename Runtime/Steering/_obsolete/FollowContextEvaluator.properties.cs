//using Cirrus.Arpg.AI;
//using Cirrus.Objects;

//using UnityEngine;

//namespace Cirrus.Arpg.Content
//{
//	public partial class FollowContextEvaluator : ContextEvaluatorBase<None>
//	{
//		[SerializeField]
//		public float MaxAcceleration = 40f;

//		/// <summary>
//		/// The distance away from the collision that we wish go
//		/// </summary>
//		[SerializeField]
//		public float WallAvoidDistance = 8;

//		[SerializeField]
//		public float SideWhiskerAngle = 45f;

//		[SerializeField]
//		public float WhiskerLength = 4f;

//		// NOTE: Cast must be smaller than Wall Avoid Dist
//		// to prevent always detecting and walking alongside the wall
//		/// <summary>
//		/// The distance away from the collision that we wish go
//		/// </summary>
//		[SerializeField]
//		public float RaycastDistance = 6f;

//		[SerializeField]
//		public float RayCastMaxDistance = 3f;
//	}
//}