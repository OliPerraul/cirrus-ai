//using Cirrus.AI.States;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class AbilityAiNode<TData> : CharacterAiNodeBase<TData>
//	{
//		public override string Name => $"Ability:{(_ability == null ? StringUtils.UnknownName : ((INameHash)_ability).Name)}";

//		public AbilityAiNode(string name="") : base(name)
//		{
//		}

//		private void _PopulateAbility()
//		{
//			// Provide embeded ability otherwise get one from the character using other provided fields
//			if(Ability != null) _ability = Ability.Realize();
//		}

//		// TODO: Where to maintain goals?? Should not be globally held. What to do when goals expire..

//		//public override bool _Condition()
//		//{
//		//	return Context.Ai.Target != null;
//		//}

//		protected override ActionNodeResult _Enter()
//		{
//			base._Enter();

//			_PopulateAbility();

//			_ability.OnEndLagEndedHandler += _OnAbilityEndLagFinished;

//			return ActionNodeResult.Running;
//		}

//		private void _UseAbility()
//		{
//			var chara = Context;
//			var ai = Context.Ai;

//			//if (!Character.AbilityUser.IsLagging && _ability.IsAvailable(Character))
//			if(
//				_ability.IsAvailable(chara.Character) &&
//				_ability.Start(chara.Character, context.Target)
//				)
//			{
//				_abilityStartPosition = chara.Position;
//			}
//		}

//		// If ability is finished i.e finished lagging means a new ability can be used
//		private void _OnAbilityEndLagFinished(IActiveAbility ab)
//		{
//			//_isAbilityActive = false;
//			//StartCalculatingPath();
//		}

//		protected override ActionNodeResult _Update()
//		{
//			base._Update();

//			var chara = Context;
//			var ai = Context.CharacterAi;

//			if(chara.Position.Approx(
//				context.Target.Position,
//				ArrivalTolerance))
//			// if previous position where we used the ability is close enough to the destination
//			// Just use the ability at the same loction
//			{
//				//_Control.Velocity = Vector3.zero;
//				_UseAbility();
//				return ActionNodeResult.Failed;
//			}

//			return ActionNodeResult.Running;
//		}

//		protected override ActionNodeResult _Exit()
//		{
//			//StopCalculatingPath();

//			if(_ability != null) _ability.OnEndLagEndedHandler -= _OnAbilityEndLagFinished;

//			var control = Context.Control;
//			control.Axes.Left = Vector2.zero;
//			control.DesiredAxes.Left = Vector2.zero;
//			control.DesiredAxes.Right = Vector2.zero;

//			base._Exit();
//			return ActionNodeResult.Success;
//		}

//		public override void OnDrawGizmos()
//		{
//			base.OnDrawGizmos();

//			//Handles.Label(Character.Transform.position, ""+_val);

//			Gizmos.color = Color.yellow;
//			//Gizmos.DrawSphere(_destination, .2f);

//			//Control.Behaviour.DrawGizmos(Control, Agent, Character);
//		}
//	}
//}