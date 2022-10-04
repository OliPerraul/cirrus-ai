//using Cirrus.Arpg.AI;
//using Cirrus.Randomness;
//using UnityEngine;

//namespace Cirrus.Arpg.Content
//{
//	public partial class WanderNearStartContextEvaluator : ContextEvaluatorBase
//	{
//		private Vector3 direction;

//		// https://www.youtube.com/watch?v=6BrZryMz-ac 6:54

//		private Vector2 _perlinNoisePosition;

//		private Vector3 _startPosition;

//		public float _wanderAngle = 2f;

//		public override void Start(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			base.Start(node);

//			_startPosition = context.Character.StartPosition;
//			_wanderAngle = 0;
//		}

//		public override bool Update(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			_perlinNoisePosition += new Vector2(
//				Mathf.Abs(context.Control.LocomotionVelocity.x),
//				Mathf.Abs(context.Control.LocomotionVelocity.z));

//			// D1
//			Vector3 direction1 = new Vector3(0, 0, -1) * CircleRadius;
//			// Randomly change the vector direction by making it change its current angle Set angle..
//			direction1 = new Vector3(
//				Mathf.Cos(_wanderAngle),
//				direction1.y,
//				Mathf.Sin(_wanderAngle))
//				.normalized;

//			// D2 When far away then face completely back to the spawn (w=1)
//			Vector3 direction2 = _startPosition - context.Character.Position;
//			float weight = direction2.magnitude / MaxDistance;
//			direction2.Normalize();

//			direction = (weight * direction2) + (weight - 1) * direction1;

//			for(int i = 0; i < Context.Resolution.Count; i++)
//			{
//				Context.Interests[i] = 0;
//				if(Vector3.Dot(Context.Resolution[i], direction).Out(out float dot) > 0)
//				{
//					Context.Interests[i] = dot;
//				}
//			}

//			////
//			//// Change wanderAngle just a bit, so it
//			//// won't have the same value in the
//			//// next game frame.
//			_wanderAngle += (RandomUtils.PerlinNoise(_perlinNoisePosition) * AngleChange) - (AngleChange * .5f);

//			return true;
//		}

//		public override void OnDrawGizmos(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			Gizmos.DrawLine(context.BodyMiddlePosition, context.BodyMiddlePosition + direction);
//		}
//	}
//}