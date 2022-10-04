//using Cirrus.Arpg.AI;
//using UnityEngine;
//using Random = UnityEngine.Random;

//namespace Cirrus.Arpg.Content
//{
//	public partial class WanderContextEvaluator : ContextEvaluatorBase
//	{
//		private Vector3 _direction;

//		public override bool Update(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			_direction = new Vector3(0, 0, -1) * CircleRadius;
//			// Randomly change the vector direction by making it change its current angle Set angle..
//			_direction = new Vector3(
//				Mathf.Cos(_wanderAngle),
//				_direction.y,
//				Mathf.Sin(_wanderAngle))
//				.normalized;

//			for(int i = 0; i < Context.Resolution.Count; i++)
//			{
//				Context.Interests[i] = 0;
//				if(Vector3.Dot(Context.Resolution[i], _direction).Out(out float dot) > 0)
//				{
//					Context.Interests[i] = dot;
//				}
//			}

//			// Change wanderAngle just a bit, so it won't have the same value in the next game frame.
//			_wanderAngle += (Random.value * AngleChange) - (AngleChange * .5f);

//			return true;
//		}

//		public override void OnDrawGizmos(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			Gizmos.DrawLine(context.BodyMiddlePosition, context.BodyMiddlePosition + _direction);
//		}
//	}
//}