//using Cirrus.Collections;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.States;
//using System;
//using System.Collections;
//using System.Collections.Generic;
////using System.Numerics;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class CharacterAiStateBase	
//		: CharacterControlStateBase
//		, IAiState
//	{
//		//object ICloneable.Clone()
//		//{
//		//	return _Clone();
//		//}

//		public CharacterAiStateBase() : base() { }

//		public CharacterAiStateBase(CharacterControlStateMachine context) : base(context) { }

//		//protected override CharacterControlStateBase _Clone()
//		//{
//		//	return base._Clone();
//		//}

//		public override void OnResourceRealized()
//		{
//			base.OnResourceRealized();
//			_reconsideredTimer = new Timer(ReconsideredTime, start: false);
//			_reconsideredTimer.OnTimeoutHandler += () => UpdateGoal();
//		}

//		public override StateMachineStatus Enter()
//		{
//			base.Enter();

//			_Control.Axes.Left = Vector2.zero;
//			_Control.DesiredAxes.Left = Vector2.zero;
//			StartAdaptEnvironment();

//			return StateMachineStatus.Success;
//		}

//		public override StateMachineStatus Exit()
//		{
//			StopAdaptEnvironment();

//			return StateMachineStatus.Success;
//		}

//		public override void Update_()
//		{
//			_Control.Axes.Left = Vector3.Lerp(_Control.Axes.Left,
//				_Control.DesiredAxes.Left,
//				_Control.AxesLeftStep);

//			_Control.Axes.Right = Vector3.Lerp(
//				_Control.Axes.Right,
//				_Control.DesiredAxes.Right,
//				_Control.AxesRightStep);
//		}

//		public virtual Vector3 GetCentroid(IEnumerable<ObjectBase> targets)
//		{
//			// Calc point to run away from
//			Vector3 centroid = Vector3.zero;
//			float offset = 1;
//			foreach (var t in targets)
//			{
//				if (t == null) continue;
//				// Closer target have bigger weight
//				// TODO: use utility as weight instead. utility should take distance into account anyways
//				float weight = offset / targets.FastCount();
//				centroid += weight * t.Position;

//				offset += 1;
//			}

//			return centroid;
//		}

//		public virtual Vector3 GetCentroid()
//		{
//			return GetCentroid(Context.AI.Targets);
//		}

//		public override void UpdateMovement()
//		{
//			base.UpdateMovement();

//			Vector3 desiredVelocity = _Agent.NavMeshAgent.desiredVelocity;
//			Vector3 desiredDirection = desiredVelocity.normalized;

//			_Agent.NavMeshAgent.transform.position = _Character.Position;
//			_Control.LocomotionVelocity = _Agent.NavMeshAgent.velocity;

//			//public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
//			if(Physics.Raycast(
//				base._Character.Position + Vector3.up * 0.5f,
//				desiredDirection,
//				out _,
//				base._Control.LedgeRaycastLenght,
//				Layers.LayoutFlag))
//			{
//				base._Character.Jump();
//			}
//		}

//		public override void UpdateDirection() 
//		{
//			base.UpdateDirection();

//			Vector2 dir = new Vector2(_Agent.NavMeshAgent.desiredVelocity.x, _Agent.NavMeshAgent.desiredVelocity.z).normalized;
//			_Control.DesiredAxes.Right = Vector2.Lerp(_Control.DesiredAxes.Right, dir, 0.5f);
//		}

//		protected virtual IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			yield return null;
//		}

//		public override void Jump()
//		{
//			_Character.Jump();
//		}

//		public override void OnDrawGizmos_()
//		{
//			//throw new System.NotImplementedException();
//		}

//		public void CursorPosition(Vector2 value)
//		{

//		}

//		public override void MousePosition(Vector2 value)
//		{
//			//throw new System.NotImplementedException();
//		}

//		public override void MouseScroll(float value)
//		{

//		}

//		public override void MouseLeftPressed(bool v)
//		{
//		}

//		public override void MouseLeftHeld(bool v)
//		{
//		}

//		public override void MouseLeftReleased(bool v)
//		{			
//		}

//		protected void StopCalculatingPath()
//		{
//			if (_calculatePathCoroutine != null)
//			{
//				_Control.StopCoroutine(_calculatePathCoroutine);
//				_calculatePathCoroutine = null;
//			}
//		}

//		protected void StartCalculatingPath()
//		{
//			if (_calculatePathCoroutine == null)
//			{
//				_calculatePathCoroutine = _Control.StartCoroutine(
//					_CalculatePathCoroutine(NavmeshRefreshTime));
//			}
//		}

//		public void StartAdaptEnvironment()
//		{
//			if (IsReconsidered)
//			{
//				_reconsideredTimer.Reset();
//			}
//		}

//		public void StopAdaptEnvironment()
//		{
//			if (IsReconsidered)
//			{
//				_reconsideredTimer.Reset();
//			}
//		}

//		// Override if you do not want to reconsider
//		public override void UpdateGoal(
//			OptionBase option, 
//			params object[] args)
//		{
//			_Agent.UpdateGoalPrimaryOptions(option);
//		}

//		public override void UpdateGoal()
//		{			
//			_Agent.UpdateGoal();
//		}
//	}
//}
