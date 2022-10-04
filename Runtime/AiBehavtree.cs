
// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Broccoli;
using Cirrus.Arpg.Abilities;
using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Objects;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

namespace Cirrus.Arpg.AI
{
	public enum AiBtFlags
	{
		None = 0,
		RescueDecision = 1 << 0,
		// Is rescuing teammate
		Rescuer = 1 << 1,
		Injured = 1 << 2,
		Hostile = 1 << 3,
		Bystander = 1 << 4,
		Ability = 1 << 5,
	}

	public enum AiBtMsg
	{
		Default,
		Inactive,
	}

	public enum AiBtFirstMsg
	{
		None
		, Default
		, Retreat
		// Watch colleagues as long as they are still hostile
		, Bystander
		, Hostile
		, Defend
		, Dodge
		, Retaliate
		, Distracted
		, Vigilant
		, Exiting
		, Idle
		, Following
		, Restrain
	}

	//
	public enum AiBtSecondMsg
	{
		Attacked
	}


	public class AiBehavtree
	: CharacterBehavtreeBase
	{
		//public Func<IAiBehavtree, IActiveAbilityInstance> AbilityCb { get; set; }

		public Vector3 destination;

		public IActiveAbilityInstance ability;

		public Vector3 abilityPosition;

		public NavMeshPath Path { get; }

		private SteeringComponent _steeringSupport;

		public SteeringComponent Steering =>
			_steeringSupport == null ?
			_steeringSupport = GetComponent<SteeringComponent>() :
			_steeringSupport;

		public Action<AiBehavtree, AiBtFlags> onFlagsChangedHandler;

		private AiBtFlags _flags;

		public AiBtFlags Flags
		{
			get => _flags;
			set
			{
				_flags = value;
				onFlagsChangedHandler?.Invoke(this, _flags);
			}
		}

		public override GameObject Label => null;

		public EntityObjectBase target;

		public List<EntityObjectBase> targets = new List<EntityObjectBase>();


		// TODO
		public void _OnEffectApplied(IEffectContext context, IEffectInstance damage)
		{
			if(damage.EffectFlags.Intersects(EffectFlags.Distraction))
			{
				Blackboard.Set(AiBtFirstMsg.Distracted);
			}
		}

		public override void Awake()
		{
			base.Awake();
			CharacterObject.onEntityInitHandler += _OnEntityInit;
			CharacterObject.onEntityDestroyHandler += _OnEntityDestroyed;
			CharacterObject.onEntityLateInitHandler += _EntityLateInit;
		}

		protected void _OnEntityDestroyed(EntityObjectBase obj)
		{
			_running = false;
			_root.Stop();
		}

		protected void _EntityLateInit(EntityInstanceBase e)
		{
			if(Character.Ai.behavtree.StartMessage != AiBtFirstMsg.None) Blackboard.Set(Character.Ai.behavtree.StartMessage);

			_root.Start();
		}

		protected void _OnEntityInit(EntityInstanceBase e)
		{
			_root = new RootNodeInstance(this)
			{
				new RepeatDecoratorInstance
				{
					new SelectorNodeInstance
					{
						new BlackboardDecoratorInstance<AiBtMsg>(AiBtMsg.Inactive)
						{
							new UpdateNodeInstance<AiBehavtree, None>((context, node) => NodeResult.Running)
						},
						new BlackboardDecoratorInstance<AiBtMsg>(AiBtMsg.Default)
						{
							Character.Ai.behavtree.CopyInstance()
						}
					}
				}
			};
		}
	}
}