//using Cirrus.Unity.Objects;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class SteeringSupportComponent : MonoBehaviour
//	{
//		public void Awake()
//		{			
//		}

//		/// <summary>
//		/// Updates the velocity of the current game object by the given linear
//		/// acceleration
//		/// </summary>
//		public void Steer(Vector3 linearAcceleration)
//		{
//			Steering.Velocity += linearAcceleration * Time.deltaTime;

//			if(Steering.Velocity.magnitude > MaxVelocity)
//			{
//				Steering.Velocity = Steering.Velocity.normalized * MaxVelocity;
//			}
//		}

//		/// <summary>
//		/// A seek steering behavior. Will return the steering for the current game object to seek a given position
//		/// </summary>
//		public Vector3 Seek(Vector3 targetPosition, float maxSeekAccel)
//		{
//			/* Get the direction */
//			Vector3 acceleration = Steering.ConvertVector(targetPosition - transform.position);

//			acceleration.Normalize();

//			/* Accelerate to the target */
//			acceleration *= maxSeekAccel;

//			return acceleration;
//		}

//		public Vector3 Seek(Vector3 targetPosition)
//		{
//			return Seek(targetPosition, MaxAcceleration);
//		}

//		/// <summary>
//		/// Makes the current game object look where he is going
//		/// </summary>
//		public void LookWhereYoureGoing()
//		{
//			Vector3 direction = Steering.Velocity;

//			if(Smoothing)
//			{
//				if(_velocitySamples.Count == NumSamplesForSmoothing)
//				{
//					_velocitySamples.Dequeue();
//				}

//				_velocitySamples.Enqueue(Steering.Velocity);

//				direction = Vector3.zero;

//				foreach(Vector3 v in _velocitySamples)
//				{
//					direction += v;
//				}

//				direction /= _velocitySamples.Count;
//			}

//			LookAtDirection(direction);
//		}

//		public void LookAtDirection(Vector3 direction)
//		{
//			direction.Normalize();

//			/* If we have a non-zero direction then look towards that direciton otherwise do nothing */
//			if(direction.sqrMagnitude > 0.001f)
//			{
//				/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
//				float toRotation = -1 * (Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg);
//				float rotation = Mathf.LerpAngle(Steering.Rotation.eulerAngles.y, toRotation, Time.deltaTime * TurnSpeed);

//				Steering.Rotation = Quaternion.Euler(0, rotation, 0);
//			}
//		}

//		public void LookAtDirection(Quaternion toRotation)
//		{
//			LookAtDirection(toRotation.eulerAngles.y);
//		}

//		/// <summary>
//		/// Makes the character's rotation lerp closer to the given target rotation (in degrees).
//		/// </summary>
//		/// <param name="toRotation">the desired rotation to be looking at in degrees</param>
//		public void LookAtDirection(float toRotation)
//		{
//			float rotation = Mathf.LerpAngle(Steering.Rotation.eulerAngles.y, toRotation, Time.deltaTime * TurnSpeed);

//			Steering.Rotation = Quaternion.Euler(0, rotation, 0);
//		}

//		/// <summary>
//		/// Returns the steering for a character so it arrives at the target
//		/// </summary>
//		public Vector3 Arrive(Vector3 targetPosition)
//		{
//			Debug.DrawLine(transform.position, targetPosition, Color.cyan, 0f, false);

//			targetPosition = Steering.ConvertVector(targetPosition);

//			/* Get the right direction for the linear acceleration */
//			Vector3 targetVelocity = targetPosition - Steering.Position;
//			//Debug.Log("Displacement " + targetVelocity.ToString("f4"));

//			/* Get the distance to the target */
//			float dist = targetVelocity.magnitude;

//			/* If we are within the stopping radius then stop */
//			if(dist < TargetRadius)
//			{
//				Steering.Velocity = Vector3.zero;
//				return Vector3.zero;
//			}

//			/* Calculate the target speed, full speed at slowRadius distance and 0 speed at 0 distance */
//			float targetSpeed;
//			if(dist > SlowRadius)
//			{
//				targetSpeed = MaxVelocity;
//			}
//			else
//			{
//				targetSpeed = MaxVelocity * (dist / SlowRadius);
//			}

//			/* Give targetVelocity the correct speed */
//			targetVelocity.Normalize();
//			targetVelocity *= targetSpeed;

//			/* Calculate the linear acceleration we want */
//			Vector3 acceleration = targetVelocity - Steering.Velocity;
//			/* Rather than accelerate the character to the correct speed in 1 second, 
//			 * accelerate so we reach the desired speed in timeToTarget seconds 
//			 * (if we were to actually accelerate for the full timeToTarget seconds). */
//			acceleration *= 1 / TimeToTarget;

//			/* Make sure we are accelerating at max acceleration */
//			if(acceleration.magnitude > MaxAcceleration)
//			{
//				acceleration.Normalize();
//				acceleration *= MaxAcceleration;
//			}
//			//Debug.Log("Accel " + acceleration.ToString("f4"));
//			return acceleration;
//		}

//		public Vector3 Interpose(SteeringComponent target1, SteeringComponent target2)
//		{
//			Vector3 midPoint = (target1.Position + target2.Position) / 2;

//			float timeToReachMidPoint = Vector3.Distance(midPoint, transform.position) / MaxVelocity;

//			Vector3 futureTarget1Pos = target1.Position + target1.Velocity * timeToReachMidPoint;
//			Vector3 futureTarget2Pos = target2.Position + target2.Velocity * timeToReachMidPoint;

//			midPoint = (futureTarget1Pos + futureTarget2Pos) / 2;

//			return Arrive(midPoint);
//		}

//		/// <summary>
//		/// Checks to see if the target is in front of the character
//		/// </summary>
//		public bool IsInFront(Vector3 target)
//		{
//			return IsFacing(target, 0);
//		}

//		public bool IsFacing(Vector3 target, float cosineValue)
//		{
//			Vector3 facing = transform.right.normalized;

//			Vector3 directionToTarget = (target - transform.position);
//			directionToTarget.Normalize();

//			return Vector3.Dot(facing, directionToTarget) >= cosineValue;
//		}
//	}
//}