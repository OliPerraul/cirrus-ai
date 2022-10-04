//using Cirrus.Arpg.AI;
//using UnityEngine;

//namespace Cirrus.Arpg.Content
//{
//	public class TargetAbilityContextEvaluator : ContextEvaluatorBase
//	{
//		private Vector3 _direction;

//		public override void Start(SteeringAiNode<object> node)
//		{
//			base.Start(node);
//		}

//		public override bool Update(SteeringAiNode<object> node)
//		{
//			//if(state.AiContext == null) return false;
//			var context = node.Context;
//			_direction = (context.Ai.Target.Position - context.Ai.Steering.Position).normalized;

//			for(int i = 0; i < Context.Resolution.Count; i++)
//			{
//				Context.Interests[i] = 0;
//				if(Vector3.Dot(Context.Resolution[i], _direction).Out(out float dot) > 0)
//				{
//					Context.Interests[i] = dot;
//				}
//			}

//			return true;
//		}

//		public override void OnDrawGizmos(SteeringAiNode<object> node)
//		{
//			var context = node.Context;
//			Gizmos.DrawLine(
//				context.BodyMiddlePosition,
//				context.BodyMiddlePosition + _direction);
//		}
//	}
//}