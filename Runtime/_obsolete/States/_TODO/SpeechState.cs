//using Cirrus.Collections;
//using Cirrus.FSM;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using System.Collections;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	// TODO : Speech to one at the time (closest one)
//	// TODO if multiple targets nearby talk to the group
//	public partial class SpeechState : AIStateBase
//	{		
//		public SpeechState() : base()
//		{ 			
//		}

//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while(true)
//			{
//				Vector3 centroid = GetClosestPosition();

//				if(_CharacterObject.Position.IsBetween(
//					centroid,
//					Range.Max,
//					Range.Min))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if(_destination.IsBetween(
//					centroid,
//					Range.Max,
//					Range.Min))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if(_chanceRandomPosition.IsTrue)
//				{
//					if(NavMeshUtils.GetRandomPointAround(
//						centroid,
//						Range.Min,
//						Range.Max,
//						out _destination))
//					{
//						_Agent.NavMeshAgent.SetDestination(_destination);
//					}
//				}
//				else if(NavMeshUtils.GetRandomPointBetween(
//					 _CharacterObject.Position,
//					 centroid,
//					 Range.Min,
//					 Range.Max,
//					 out _destination))
//				{
//					_Agent.NavMeshAgent.SetDestination(_destination);
//				}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}		

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			base.Enter(args);

//			_timer = new Timer(OnTimeout);
//			_timer.Reset(Time, true);

//			_CharacterObject.LeftAxis = Vector2.zero;
//			_Control.TargetAxes.Left = Vector2.zero;
//			_Control.TargetAxes.Right = Vector2.zero;

//			if(Goal.Targets.FastCount() != 0)
//			{
//				if(GetCurrentSpeechPosition(
//					_CharacterObject.Position,
//					out Vector3 targetPosition))
//				{
//					_destination = _CharacterObject.Position;
//					StartSpeech();
//				}
//				else
//				{
//					StopCalculatingPath();
//					StartCalculatingPath();
//				}
//			}

//			return StateMachineStatus.Success;
//		}

//		public Vector3 GetClosestPosition()
//		{
//			float min = Mathf.Infinity;
//			Vector3 speakToPosition = Vector3.zero;
//			foreach(var target in Goal.Targets)
//			{
//				float mag = (target.ObjectPosition - _CharacterObject.Position).magnitude;
//				if(mag < min)
//				{
//					min = mag;
//					speakToPosition = target.ObjectPosition;
//				}
//			}

//			return speakToPosition;
//		}

//		public bool GetCurrentSpeechPosition(
//			Vector3 position,
//			out Vector3 speakToPosition)
//		{			
//			int count = 0;
//			speakToPosition = Vector3.zero;
//			foreach(var target in Goal.Targets)
//			{
//				if(target.ObjectPosition.Approximately(position, Range.Max))
//				{
//					speakToPosition += target.ObjectPosition;
//					count++;
//				}
//			}

//			if(count != 0) speakToPosition /= count;

//			return count != 0;
//		}

//		public void OnTimeout()
//		{
//			Context.NextState();
//		}

//		public void StartSpeech()
//		{
//			_speechStartPosition = _CharacterObject.Position;
//			_CharacterObject.Speeches.SetSpeech(
//				Emotes.Random(),
//				SpeechTime);
//		}

//		public override void Update_()
//		{
//			base.Update_();

//			if(
//				_CharacterObject.Speeches.Enabled &&
//				GetCurrentSpeechPosition(
//					_CharacterObject.Position, 
//					out Vector3 targetPosition))				
//			{
//				Vector3 direction =	(targetPosition - _CharacterObject.Position).normalized;
//				_Control.TargetAxes.Right = new Vector2(direction.x, direction.z).normalized;
//			}
//			else if(_speechStartPosition.Approximately(
//				_destination,
//				ArrivalTolerance))
//			// if previous position where we used the ability is close enough to the destination
//			// Just use the ability at the same loction
//			{
//				StartSpeech();
//			}
//			else if(_CharacterObject.Position.Approximately(
//				_destination,
//				ArrivalTolerance))
//			// If current position is close enough to destination
//			// just use it there
//			{
//				_Control.TargetAxes.Left = Vector2.zero;
//				_CharacterObject.LeftAxis /= 2;

//				StartSpeech();
//			}
//			else
//			{
//				var dir = _destination - _CharacterObject.Position;
//				_Control.TargetAxes.Right = new Vector2(dir.x, dir.z);
//				UpdateMovement();
//			}
//		}


//		public override StateMachineStatus Exit()
//		{
//			StopCalculatingPath();

//			//if(_ability != null) _ability.OnEndLagFinishedHandler -= OnAbilityEndLagFinished;

//			_CharacterObject.LeftAxis = Vector2.zero;
//			_Control.TargetAxes.Left = Vector2.zero;
//			_Control.TargetAxes.Right = Vector2.zero;

//			base.Exit();
//			return StateMachineStatus.Exited;
//		}
//	}
//}
