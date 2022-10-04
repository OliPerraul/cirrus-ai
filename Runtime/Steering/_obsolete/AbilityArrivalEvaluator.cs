//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using UnityEngine;

////using System.Numerics;

//namespace Cirrus.Arpg.Content
//{
//	public partial class AbilityArrivalEvaluator : SteeringEvaluatorBase<Void>
//	{
//		/// <summary>
//		/// The radius for the current game object (either the radius of a sphere or circle
//		/// collider). If the game object does not have a sphere or circle collider this will return -1.
//		/// </summary>
//		public float Radius(SteeringComponent steering) =>
//			Mathf.Max(
//				steering.Transform.localScale.x,
//				steering.Transform.localScale.y,
//				steering.Transform.localScale.z) * steering.Collider.radius;

//		/// <summary>
//		/// Returns the steering for a character so it arrives at the target
//		/// </summary>
//		// https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-flee-and-arrival--gamedev-1303
//		public override bool Update(ControlBtComponent context, SteeringNode<Void> node, float dt)
//		{
//			if(node == null) return false;

//			Range range = new Range(); // state.Option.Ability.Range;

//			// Get the distance to the target If we are within the stopping radius then stop (
//			// minimum distance)			
//			var control = context.Control;

//			Vector3 sourceTargetVector = context.Target.Position - context.Steering.Position;
//			float dist = sourceTargetVector.magnitude;
//			sourceTargetVector.Normalize();
//			if(dist < range.Min)
//			{
//				context.Steering.Acceleration = Vector3.zero;
//				context.Steering.Control.LocomotionVelocity = Vector3.zero;
//				context.Steering.DesiredLerpRotation = Quaternion.LookRotation(
//					sourceTargetVector.normalized,
//					context.Steering.Transform.up);
//				return true;
//			}

//			if(dist < range.Max)
//			{
//				float remainingRangeDist = dist - range.Min;
//				float rangeDist = range.Max - range.Min;
//				float elapsedRangeDist = rangeDist - remainingRangeDist;
//				control.LocomotionVelocity = Vector3.Lerp(control.LocomotionVelocity, Vector3.zero, elapsedRangeDist / rangeDist);
//				context.Steering.DesiredLerpRotation = Quaternion.LookRotation(
//					sourceTargetVector.normalized,
//					context.Steering.EntComp.Transform.up);
//			}

//			return false;
//		}
//	}
//}