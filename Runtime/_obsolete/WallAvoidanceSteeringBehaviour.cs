//using Cirrus.Unity.Objects;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
////using System.Numerics;

//namespace Cirrus.Arpg.AI
//{
//	public enum SteeringWallDetection { Raycast, Spherecast }

//	[Serializable]
//	public class WallAvoidanceSteeringBehaviour : SteeringBehaviourBase
//	{
//		[SerializeField]
//		public float MaxAcceleration = 40f;

//		[SerializeField]
//		public SteeringWallDetection WallDetection = SteeringWallDetection.Spherecast;

//		[SerializeField]
//		public LayerMask CastMask = Layers.LayoutFlags;// Physics.DefaultRaycastLayers;

//		/// <summary>
//		/// The distance away from the collision that we wish go
//		/// </summary>
//		[SerializeField]
//		public float WallAvoidDistance = 0.5f;

//		/// <summary>
//		/// How far ahead the ray should extend
//		/// </summary>
//		[SerializeField]
//		public float MainWhiskerLen = 1.25f;

//		[SerializeField]
//		public float SideWhiskerLen = 0.701f;

//		[SerializeField]
//		public float SideWhiskerAngle = 45f;


//		//MovementAIRigidbody rb;
//		//SteeringBasics steeringBasics;

//		//protected override SteeringStatus _GetSteering(
//		//	SteeringComponent steering, 
//		//	out Vector3 accel)
//		//{
//		//	return _GetSteering(steering, out accel);			
//		//}

//		// TODO : deltaTime?
//		// TODO : do not collision with self (wall collision)!!!
//		protected override SteeringStatus _GetSteering(
//			SteeringComponent steering,
//			float deltaTime,
//			out Vector3 acceleration)
//		{
//			return steering.RigidbodyVelocity.magnitude > 0.005f ?
//				_GetSteering(steering, steering.RigidbodyVelocity, out acceleration) :
//				_GetSteering(steering, steering.TransformRotation.OrientationToVector(), out acceleration);			
//		}

//		private SteeringStatus _GetSteering(
//			SteeringComponent steering, 
//			Vector3 facingDir,
//			out Vector3 acceleration)
//		{
//			acceleration = Vector3.zero;

//			GenericCastHit hit;

//			/* If no collision do nothing */
//			if(!_FindObstacle(steering, facingDir, out hit))
//			{
//				return SteeringStatus.Failed_Continue;
//			}

//			/* Create a target away from the wall to seek */
//			Vector3 targetPostition = hit.point + hit.normal * WallAvoidDistance;

//			/* If velocity and the collision normal are parallel then move the target a bit to
//			 * the left or right of the normal */
//			float angle = Vector3.Angle(steering.RigidbodyVelocity, hit.normal);
//			if(angle > 165f)
//			{				
//				Vector3 perp = new Vector3(-hit.normal.z, hit.normal.y, hit.normal.x);

//				/* Add some perp displacement to the target position propotional to the angle between the wall normal
//				 * and facing dir and propotional to the wall avoidance distance (with 2f being a magic constant that
//				 * feels good) */
//				targetPostition = targetPostition + (perp * Mathf.Sin((angle - 165f) * Mathf.Deg2Rad) * 2f * WallAvoidDistance);
//			}

//			//SteeringBasics.debugCross(targetPostition, 0.5f, new Color(0.612f, 0.153f, 0.69f), 0.5f, false);

//			acceleration = steering.Seek(targetPostition, MaxAcceleration);
//			return SteeringStatus.Success_Break;
//		}

//		bool _FindObstacle(
//			SteeringComponent steering, 
//			Vector3 facingDir, 
//			out GenericCastHit firstHit)
//		{
//			/* Create the direction vectors */
//			Vector3[] dirs = new Vector3[3];
//			dirs[0] = facingDir;

//			float orientation = SteeringUtils.VectorToOrientation(facingDir);

//			dirs[1] = SteeringUtils.OrientationToVector(orientation + SideWhiskerAngle * Mathf.Deg2Rad);
//			dirs[2] = SteeringUtils.OrientationToVector(orientation - SideWhiskerAngle * Mathf.Deg2Rad);

//			return _CastWhiskers(steering, dirs, out firstHit);
//		}

//		bool _CastWhiskers(
//			SteeringComponent steering, 
//			Vector3[] dirs, 
//			out GenericCastHit firstHit)
//		{
//			firstHit = new GenericCastHit();
//			bool foundObs = false;

//			for(int i = 0; i < dirs.Length; i++)
//			{
//				float dist = (i == 0) ? MainWhiskerLen : SideWhiskerLen;

//				GenericCastHit hit;

//				if(_GenericCast(steering, dirs[i], out hit, dist))
//				{
//					foundObs = true;
//					firstHit = hit;
//					break;
//				}
//			}

//			return foundObs;
//		}

//		struct GenericCastHit
//		{
//			public Vector3 point;
//			public Vector3 normal;

//			public GenericCastHit(RaycastHit h)
//			{
//				point = h.point;
//				normal = h.normal;
//			}

//			public GenericCastHit(RaycastHit2D h)
//			{
//				point = h.point;
//				normal = h.normal;
//			}
//		}

//		bool _GenericCast(
//			SteeringComponent steering, 
//			Vector3 direction, 
//			out GenericCastHit hit, 
//			float distance = Mathf.Infinity)
//		{
//			bool result = false;
//			Vector3 origin = steering.ColliderPosition;


//			RaycastHit h;

//			if(WallDetection == SteeringWallDetection.Raycast)
//			{
//				result = Physics.Raycast(origin, direction, out h, distance, CastMask.value);
//			}
//			else
//			{
//				result = Physics.SphereCast(origin, (steering.Radius * 0.5f), direction, out h, distance, CastMask.value);
//			}

//			hit = new GenericCastHit(h);						

//			return result;
//		}
//	}
//}