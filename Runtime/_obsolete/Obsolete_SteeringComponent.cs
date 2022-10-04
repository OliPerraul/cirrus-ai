//using Cirrus.Unity.Objects;
//using Cirrus.Unity.Objects;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	// TODO deprecate
//	// Most of the work in SteeringSupportComponent

//	/// <summary>
//	/// This is a wrapper class for either a Rigidbody or Rigidbody2D, so that either can be used with the Unity Movement AI code. 
//	/// </summary>
//	public partial class SteeringComponent : ComponentBase
//	{

//		public override void Awake()
//		{
//			IsEnabled = false;
//		}

//		public override void Start()
//		{
//			base.Start();
//		}



//		/// <summary>
//		/// Rotates the rigidbody to angle (given in degrees)
//		/// </summary>
//		/// <param name="angle"></param>
//		/// 
//		// TODO
//		public void MoveRotation(float angle)
//		{
//			//Quaternion rot = Quaternion.Euler((new Vector3(0f, angle, 0f)));
//			//_Character.MoveRotation(rot);
//		}


//		/// <summary>
//		/// Converts the vector based what kind of character the rigidbody is on. 
//		/// If it is a 2D character then the Z component will be zeroed out. If it
//		/// is a grounded 3D character then the Y component will be zeroed out. 
//		/// And if it is flying 3D character no changes will be made to the vector.
//		/// </summary>
//		/// <param name="v"></param>
//		/// <returns></returns>
//		public Vector3 ConvertVector(Vector3 v)
//		{
//			/* if the charater is a 3D character who can't fly then ignore the y component */
//			v.y = 0;
//			return v;
//		}

//		// TODO

//		public void Jump(float speed)
//		{
//			//if(_Rigidbody.useGravity == false)
//			//{
//			//	_Rigidbody.useGravity = true;
//			//	Vector3 vel = _Rigidbody.velocity;
//			//	vel.y = speed;
//			//	_Rigidbody.velocity = vel;
//			//}
//		}



//		/* Make the spherecast offset slightly bigger than the max allowed collider overlap. This was
//		 * known as Physics.minPenetrationForPenalty and had a default value of 0.05f, but has since
//		 * been removed and supposedly replaced by Physics.defaultContactOffset/Collider.contactOffset.
//		 * My tests show that as of Unity 5.3.0f4 this is not %100 true and Unity still seems to be 
//		 * allowing overlaps of 0.05f somewhere internally. So I'm setting my spherecast offset to be
//		 * slightly bigger than 0.05f */
//		readonly float _SpherecastOffset = 0.051f;

//		bool _SphereCast(Vector3 dir, out RaycastHit hitInfo, float dist, int layerMask, Vector3 planeNormal = default(Vector3))
//		{
//			dir.Normalize();

//			/* Make sure we use the collider's origin for our cast (which can be different
//			 * then the transform.position).
//			 *
//			 * Also if we are given a planeNormal then raise the origin a tiny amount away
//			 * from the plane to avoid problems when the given dir is just barely moving  
//			 * into the plane (this can occur due to floating point inaccuracies when the 
//			 * dir is calculated with cross products) */
//			Vector3 origin = ColliderPosition + (planeNormal * 0.001f);

//			/* Start the ray with a small offset from inside the character, so it will
//			 * hit any colliders that the character is already touching. */
//			origin += -_SpherecastOffset * dir;

//			float maxDist = (_SpherecastOffset + dist);

//			if(Physics.SphereCast(origin, Radius, dir, out hitInfo, maxDist, layerMask))
//			{
//				/* Remove the small offset from the distance before returning*/
//				hitInfo.distance -= _SpherecastOffset;
//				return true;
//			}
//			else
//			{
//				return false;
//			}
//		}

//		// TODO
//		void _FoundGround(Vector3 normal, Vector3 newPos)
//		{
//			//MovementNormal = normal;
//			//_Rigidbody.useGravity = false;
//			//_Rigidbody.MovePosition(newPos);

//			///* Reproject the velocity onto the ground plane in case the ground plane has changed this frame.
//			// * Make sure to multiple by the movement velocity's magnitude, rather than the actual velocity
//			// * since we could have been falling and now found ground so all the downward y velocity is not
//			// * part of our movement speed. Technically I am projecting the actual velocity onto the ground
//			// * plane rather than finding the real movement velocity's speed.*/
//			//_Rigidbody.velocity = _DirOnPlane(_Rigidbody.velocity, MovementNormal) * Velocity.magnitude;
//		}

//		bool _IsWall(Vector3 surfNormal)
//		{
//			/* If the normal of the surface is greater then our slope limit then its a wall */
//			return Vector3.Angle(Vector3.up, surfNormal) > slopeLimit;
//		}


//		bool _IsMovingInto(Vector3 dir, Vector3 normal)
//		{
//			return Vector3.Angle(dir, normal) > 90f;
//		}

//		/// <summary>
//		/// Creates a vector that maintains x/z direction but lies on the plane.
//		/// </summary>
//		Vector3 _DirOnPlane(Vector3 vector, Vector3 planeNormal)
//		{
//			Vector3 newVel = vector;
//			newVel.y = (-planeNormal.x * vector.x - planeNormal.z * vector.z) / planeNormal.y;
//			return newVel.normalized;
//		}	  
//	}
//}