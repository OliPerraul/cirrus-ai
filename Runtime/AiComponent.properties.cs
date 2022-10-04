using Cirrus.Broccoli;
using Cirrus.Arpg.Abilities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Editor;
using Cirrus.Unity.Objects;
using Pathfinding.RVO;
//using Pathfinding.RVO.Sampled;

using System;
using System.Collections.Generic;

using UnityEngine;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
	// TODO rename
	// AISupport can exist on non character objects...
	public partial class AiComponent
	{
		private enum AiAbilityState
		{
			Idle,
			Started,
			Sustained,
			//Ended
		}

		private SteeringComponent _steeringSupport;

		public SteeringComponent Steering =>
			_steeringSupport == null ?
			_steeringSupport = GetComponent<SteeringComponent>() :
			_steeringSupport;

		private RescueeComponent _rescueeComponent;

		public RescueeComponent Rescuee =>
			_rescueeComponent == null ?
			_rescueeComponent = GetComponent<RescueeComponent>() :
			_rescueeComponent;


		private RescuerComponent _rescuerComponent;

		public RescuerComponent Rescuer =>
			_rescuerComponent == null ?
			_rescuerComponent = GetComponent<RescuerComponent>() :
			_rescuerComponent;

		public CharacterKinematicsComponent Kinematics => CharacterObject.Kinematics;

		public EntityPhysicsComponent Phys => EntityObject.Phys;


		private LocalAvoidanceAgent _rvoController;
		public LocalAvoidanceAgent Rvo => _rvoController == null ?
			_rvoController = CharacterObject.GetComponentInChildren<LocalAvoidanceAgent>() :
			_rvoController;

		private AiBehavtree _behavtree;
		public AiBehavtree Behavtree => _behavtree == null ?
			_behavtree = GetComponent<AiBehavtree>() :
			_behavtree;

		private LocalAvoidanceAgent _localAvoidance;
		public LocalAvoidanceAgent LocalAvoidance => _localAvoidance == null ?
			_localAvoidance = GetComponent<LocalAvoidanceAgent>() :
			_localAvoidance;


		// TODO: use this to achieve dynamic priorities
		// simply mark expiry as stale, and then reinsert into waitlist
		public DirectorTokenRequest directorRequest = null;
		// whether we are already waiting in a waitlist
		public Dictionary<DirectorTokenInstance, DirectorTokenRequest> directorWaitlistRequests = new Dictionary<DirectorTokenInstance, DirectorTokenRequest>();

		public DirectorInstance Director => CharacterObject.group.director;

		public Action<IActiveAbilityInstance, int> OnAbilityEquippedHandler;

		public Action<IActiveAbilityInstance, int> OnAbilitySelectedHandler;

		public Action<IActiveAbilityInstance, int> OnAbilityUnselectedHandler;

		private IActiveAbilityInstance _currentAbility;

		[NonSerialized]
		private int _selectedAbilityIdx = -1;

		private AiAbilityState _currentAbilityState;

	}
}