//using Cirrus.Collections;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.States;
//using Cirrus.Unity.Numerics;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	// TODO : Eventually we could have mechanism
//	// where multiple steering state could be merged
//	// in order to use option utility in a continuous fashion
//	// This is not the case for now. All sub behaviour in a steering state are statically defined.
//	public partial class SteeringState : CharacterAIStateBase, ISteeringState
//	{
//		protected override CharacterControlStateBase _Clone()
//		{
//			var inst = (SteeringState)base._Clone();
//			inst.ContextEvaluators = ContextEvaluators.Select(x => x.Clone()).ToArray();
//			return inst;
//		}

//		public override void OnCloned()
//		{
//			base.OnCloned();
//			_timer = new Timer(OnTimeout);
//		}

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			if(base.Enter(args).Succeeded(out StateMachineStatus stat))
//			{
//				_timer.Reset(Time, true);

//				_Steering.IsEnabled = true;

//				_context = new SteeringContext(_Steering.Resolution);

//				_Control.Velocity = Vector3.zero;

//				foreach(var behaviour in ContextEvaluators)
//				{
//					behaviour.Start(_Steering, this);
//				}

//				return StateMachineStatus.Success;
//			}

//			return stat;
//		}

//		public override StateMachineStatus Exit()
//		{
//			if(base.Exit().Out(out StateMachineStatus stat) >= StateMachineStatus.Succeeded)
//			{
//				_Control.Velocity = Vector3.zero;
//				_Steering.IsEnabled = false;
//				return StateMachineStatus.Exited;
//			}

//			return stat;
//		}

//		public void OnTimeout()
//		{
//			Context.SetNextState();
//		}

//		public override void FixedUpdate_()
//		{
//			base.FixedUpdate_();

//			// Merge contexts and not make a decision
//			// TODO can we pool these?			;
//			_context.Clear();

//			foreach(var evaluator in ContextEvaluators)
//			{
//				if(evaluator.UpdateContext(_Steering, this))
//				{
//					_context.Combine(evaluator.Context);
//				}
//			}
//		}

//		public override void CustomUpdate1(float deltaTime)
//		{
//			_Steering.Acceleration = Vector3.zero;
//			_context.UpdateSteering(_Steering);
//			_Steering.LerpRotation =
//				Quaternion.LookRotation(_Steering.CharacterObject.Kinematics.Velocity.IsApproximately(Vector3.zero) ?
//					_Steering.Entity.Transform.forward :
//					_Steering.CharacterObject.Kinematics.Velocity.X_Z(0).normalized,
//					_Character.Transform.up);

//			foreach(var evaluator in Evaluators)
//			{
//				if(evaluator.UpdateSteering(
//					_Steering,
//					this,
//					deltaTime))
//				{
//					break;
//				}
//			}

//			_Control.Velocity += _Steering.Acceleration;
//			if(_Control.Velocity.magnitude >= _Steering.MaxSpeed)
//			{
//				_Control.Velocity = _Control.Velocity.normalized * _Steering.MaxSpeed;
//			}
//		}

//		public override void OnDrawGizmos_()
//		{
//			base.OnDrawGizmos_();

//			foreach(var evaluator in ContextEvaluators)
//			{
//				evaluator.OnDrawGizmos(_Steering);
//			}
//		}

//		public override void CustomUpdate2(float deltaTime)
//		{
//			// TODO : Other type of orientation ?
//			_Control.Rotation = Quaternion.Lerp(_Control.Rotation, _Steering.LerpRotation, _Steering.RotationLerpSpeed * deltaTime);
//		}
//	}
//}