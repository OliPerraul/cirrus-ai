//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.States;
//using Cirrus.Unity.Numerics;
//using System;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public class IdleAiState : CharacterAiStateBase
//	{
//		[field: NonSerialized]
//		public Range Time { get; set; } = new Range(5, 10f);

//		private Timer _timer;

//		//protected override CharacterControlStateBase _Clone()
//		//{
//		//	var inst = (IdleAiState)base._Clone();
//		//	return inst;
//		//}

//		public IdleAiState()
//		{
//			_timer = new Timer(OnTimeout);
//		}

//		public override StateMachineStatus Enter()
//		{
//			var stat = base.Enter();
//			if(stat >= StateMachineStatus.Success)
//			{
//				_timer.Reset(Time, true);

//				_Control.LocomotionVelocity = Vector3.zero;

//				return StateMachineStatus.Success;
//			}

//			return stat;
//		}

//		public override StateMachineStatus Exit()
//		{
//			var stat = base.Exit();
//			if(stat >= StateMachineStatus.Success)
//			{
//				_Control.LocomotionVelocity = Vector3.zero;
//				return StateMachineStatus.Success;
//			}

//			return stat;
//		}

//		public void OnTimeout()
//		{
//		}
//	}
//}
