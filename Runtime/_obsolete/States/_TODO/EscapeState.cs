//using Cirrus.FSM;
//using Cirrus.Objects;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using System.Collections;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	// DECISION CONTAINS LIST OF ALL TARGETS WE NEED TO AVOID
//	// The consideration det whether we should put it in

//	// STEP 1 Determine who I need to run away from


//	// STEP 2 Determine shortest path that would prevent to go near them


//	// FIND A POINT AWAY FROM ALL TARGETS
//	// WE DONT REALLY CARE WHICH WAY TO EXIT (Navmesh obstacle should do the job)

//	// FIND POINT IN THE NEXT STATE ITSELF NOT HERE (We may need recalc)


//	public partial class EscapeState : AIStateBase
//	{
//		private Vector3 _RandomPointOnCircleEdge(float radius)
//		{
//			var vector2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
//			return new Vector3(vector2.x, 0, vector2.y);
//		}

//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while (true)
//			{
//				Vector3 centroid = GetCentroid();

//				var between = _CharacterObject.Position - centroid;
//				if (between.magnitude >= SafeDistance)
//				{
//					NavMeshUtils.GetRandomPointAround(
//						_CharacterObject.Position,
//						1,
//						1,
//						out _destination);

//					_Agent.NavMeshAgent.SetDestination(_destination);
//				}
//				else if 
//					(
//						!_destSet || 
//						(_destination - centroid).magnitude < SafeDistance
//					)
//				{
//					_destSet = true;
//					var dir = between.normalized;

//					NavMeshUtils.GetRandomPointAround(
//						_CharacterObject.Position + dir * (SafeDistance + DestinationRange),
//						DestinationRange,
//						DestinationRange,
//						out _destination);

//					_Agent.NavMeshAgent.SetDestination(_destination);

//				}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}

//		public override void Update_()
//		{
//			base.Update_();

//			if (_CharacterObject.Position.Approximately(_destination, ArrivalTolerance))
//			{
//				_Control.TargetAxes.Left = Vector2.zero;
//				_CharacterObject.LeftAxis /= 2;

//				if (SafeTime >= MinSafeTime)
//				{
//					UpdateGoal();
//				}
//				else
//				{
//					SafeTime += Time.deltaTime;
//				}
//			}
//			else
//			{
//				SafeTime = 0;
//				UpdateMovement();
//			}
//		}

//		// Case where run away from same type of area?
//		//  Does not matter because will always start from there
//		public override StateMachineStatus Enter(params object[] args)
//		{
//			_destSet = false;

//			//foreach (var t in Agent.Model.Goal.Targets)
//			//{
//			//	Agent.NavMeshAgent.SetAreaCost(t.ObjectInstance.EnvironmentObject.NavMeshModifierVolume.area, TargetAreaCost);
//			//}

//			_calculatePathCoroutine = _Control.StartCoroutine(
//				_CalculatePathCoroutine(RefreshRate));

//			return StateMachineStatus.Success;
//		}


//		public override StateMachineStatus Exit()
//		{
//			_Control.StopCoroutine(_calculatePathCoroutine);

//			return StateMachineStatus.Exited;
//		}

//		public override void OnDrawGizmos_()
//		{
//			Gizmos.color = Color.magenta;
//			Gizmos.DrawSphere(_destination, .2f);
//		}
//	}
//}