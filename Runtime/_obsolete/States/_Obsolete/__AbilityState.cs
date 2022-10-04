//using Cirrus;
//using Cirrus.Collections;
//using Cirrus.States;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Objects;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using System.Collections;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class __AbilityState : UtilityAgentState
//	{
//		public override string Name => $"Ability:{(_ability == null ? StringUtils.UnknownName : ((INameHash)_ability).Name)}";

//		private void _PopulateAbility()
//		{
//			// Provide embeded ability otherwise get one from the character
//			// using other provided fields
//			if (Ability != null) _ability = Ability.Clone<IConcreteActiveAbility>();
//		}

//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while (true)
//			{
//				if (_ability == null)
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if (_ability.IsActive)
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				Vector3 centroid = GetCentroid();

//				if (_Character.Position.IsBetween(
//					centroid,
//					_ability.Range.Max,
//					_ability.Range.Min))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if (_destination.IsBetween(
//					centroid,
//					_ability.Range.Min,
//					_ability.Range.Max))
//				{
//					yield return new WaitForSeconds(waitTime);
//					continue;
//				}

//				if (ChanceRandomPosition.IsTrue)
//				{
//					if (NavMeshUtils.GetRandomPointAround(
//						centroid,
//						_ability.Range.Min,
//						_ability.Range.Max,
//						out _destination))
//					{
//						_Agent.NavMeshAgent.SetDestination(_destination);
//					}
//				}
//				else if(NavMeshUtils.GetRandomPointBetween(
//					 _Character.Position,
//					 centroid,
//					 _ability.Range.Min,
//					 _ability.Range.Max,
//					 out _destination))
//				{
//					_Agent.NavMeshAgent.SetDestination(_destination);
//				}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}

//		// TODO: Where to maintain goals??
//		// Should not be globally held.
//		// What to do when goals expire..

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			base.Enter(args);

//			_PopulateAbility();

//			_ability.OnEndLagEndedHandler += _OnAbilityEndLagFinished;

//			StopCalculatingPath();
//			StartCalculatingPath();

//			return StateMachineStatus.Success;
//		}

//		private void _UseAbility()
//		{
//			//if (!Character.AbilityUser.IsLagging && _ability.IsAvailable(Character))
//			if (_ability.EvaluateAvailability(_Character.CharacterEntity))
//			{
//				if (Goal.Targets.FastCount() == 1 &&
//					_Control.Interaction.StartAbility(
//						_ability,
//						Goal.Targets.First()))
//				{
//					StopCalculatingPath();
//					_abilityStartPosition = _Character.Position;
//				}
//				else if (_Control.Interaction.StartAbility(_ability))
//				{
//					StopCalculatingPath();
//					_abilityStartPosition = _Character.Position;
//				}
//			}
//		}

//		// If ability is finished 
//		// i.e finished lagging means a new ability can be used
//		private void _OnAbilityEndLagFinished(IActiveAbility ab)
//		{
//			//_isAbilityActive = false;
//			StartCalculatingPath();
//		}

//		public override void Update_()
//		{
//			base.Update_();

//			if (_ability != null &&
//				_ability.IsActive)
//			{
//				_Control.Velocity = Vector3.zero;
//				_Control.DesiredAxes.Right = new Vector2(
//					_ability.Direction.x,
//					_ability.Direction.z).normalized;
//			}
//			else if (_abilityStartPosition.IsApproximately(
//				_destination,
//				ArrivalTolerance))
//			// if previous position where we used the ability is close enough to the destination
//			// Just use the ability at the same loction
//			{
//				_Control.Velocity = Vector3.zero;
//				_UseAbility();
//			}
//			else if (_Character.Position.IsApproximately(
//				_destination,
//				ArrivalTolerance))
//			// If current position is close enough to destination
//			// just use it there
//			{
//				_Control.Velocity = Vector3.zero;
//				_UseAbility();
//			}
//			else
//			{
//				var dir = _destination - _Character.Position;
//				_Control.DesiredAxes.Right = new Vector2(dir.x, dir.z);
//				UpdateMovement();
//			}
//		}


//		public override StateMachineStatus Exit()
//		{
//			StopCalculatingPath();

//			if (_ability != null) _ability.OnEndLagEndedHandler -= _OnAbilityEndLagFinished;

//			_Control.Axes.Left = Vector2.zero;
//			_Control.DesiredAxes.Left = Vector2.zero;
//			_Control.DesiredAxes.Right = Vector2.zero;

//			base.Exit();
//			return StateMachineStatus.Exited;
//		}

//		public override void OnDrawGizmos_()
//		{
//			base.OnDrawGizmos_();


//			//Handles.Label(Character.Transform.position, ""+_val);

//			Gizmos.color = Color.yellow;
//			Gizmos.DrawSphere(_destination, .2f);

//			//Control.Behaviour.DrawGizmos(Control, Agent, Character);
//		}
//	}
//}

