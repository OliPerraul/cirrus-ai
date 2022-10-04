//using Cirrus.FSM;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using System.Collections;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public class FollowState : AIStateBase
//	{
//		public FollowState(CharacterControlStateMachine machine) : base(machine)
//		{
//		}

//		public FollowState() : base()
//		{
//		}

//		[SerializeField]
//		public float MinSafeTime { get; set; } = 5f;

//		[SerializeField]
//		public Range Range { get; set; } = new Range(5f, 8f);


//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while (true)
//			{
//				Vector3 centroid = GetCentroid();

//				if (_CharacterObject.Position.Approximately(centroid, ArrivalTolerance))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if (_destination.Approximately(centroid, ArrivalTolerance))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if (NavMeshUtils.GetRandomPointBetween(
//						_CharacterObject.Position,
//						centroid,
//						Range.Min,
//						Range.Max,
//						out _destination))
//				{
//					_Agent.NavMeshAgent.SetDestination(_destination);
//				}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			//base.Enter(args);

//			StartCalculatingPath();
//			return StateMachineStatus.Success;
//		}

//		public override void Update_()
//		{
//			base.Update_();


//			if (_CharacterObject.Position.Approximately(
//				_destination,
//				ArrivalTolerance))
//			{
//				_Control.TargetAxes.Left = Vector2.zero;
//				_CharacterObject.LeftAxis /= 2;

//				_Agent.NavMeshAgent.transform.position = _CharacterObject.Position;
//				_Agent.NavMeshAgent.velocity = _CharacterObject.Kinematics.Motor.Velocity;
//			}
//			else
//			{
//				UpdateMovement();
//			}
//		}


//		//public override void Reenter(params object[] args)
//		//{
//		//	//base.Reenter(args);

//		//	StartCalculatingPath();
//		//}

//		public override StateMachineStatus Exit()
//		{
//			base.Exit();

//			StopCalculatingPath();

//			return StateMachineStatus.Exited;
//		}

//	}
//}
