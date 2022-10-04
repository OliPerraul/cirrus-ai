using Cirrus.Animations;
using Cirrus.Broccoli;
using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Objects;
using Cirrus.Numerics;
using Cirrus.Unity.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;
using static UnityEngine.GraphicsBuffer;
using Cirrus.Arpg.Abilities;
using Cirrus.Unity.Randomness;
using Pathfinding.RVO;
using Cirrus.Unity.Editor;
using Cirrus.Objects;
using UnityEngine.Serialization;
//using Pathfinding.RVO.Sampled;

namespace Cirrus.Arpg.Content.AI
{
	public class VigilantNodeInstance : DecoratorInstanceBase
	, ISteeringNodeData
	, ILocalAvoidanceAgentInstance
	, IWallsData
	, IEvadeData
	{
		public override object Data { get => null; set { } }
		public float LocalAvoidanceWeight { get; set; } = 1;
		public HashSet<LocalAvoidanceAgentInstance> Ignore { get; set; } = new HashSet<LocalAvoidanceAgentInstance>();
		public float LocalAvoidanceMaxSpeed => _resource.LocalAvoidanceMaxSpeed;
		public LocalAvoidancePriority[] LocalAvoidancePriorities => _resource.LocalAvoidancePriorities;
		public float LocalAvoidancePriority => _resource.LocalAvoidancePriority;
		public float LocalAvoidanceRadius => _resource.LocalAvoidanceRadius;
		public float LocalAvoidanceHeight => _resource.LocalAvoidanceHeight;
		public LocalAvoidanceLayers LocalAvoidanceLayer => _resource.LocalAvoidanceLayer;
		public LocalAvoidanceLayers LocalAvoidanceCollidesWith => _resource.LocalAvoidanceCollidesWith;
		public float LocalAvoidanceCenter => _resource.LocalAvoidanceCenter;
		public float LocalAvoidanceTimeHorizon => _resource.LocalAvoidanceTimeHorizon;
		public float LocalAvoidanceObstacleTimeHorizon => _resource.LocalAvoidanceObstacleTimeHorizon;
		public int LocalAvoidanceMaxNeighbours => _resource.LocalAvoidanceMaxNeighbours;
		public bool LocalAvoidanceLocked => _resource.LocalAvoidanceLocked;
		public bool LocalAvoidanceLockedWhenNotMoving => _resource.LocalAvoidanceLockedWhenNotMoving;
		//public float WallAvoidDistance { get => ((IWallsData)_resource).WallAvoidDistance; set => ((IWallsData)_resource).WallAvoidDistance = value; }
		//public float WallAvoidSideWhiskerAngle { get => ((IWallsData)_resource).WallAvoidSideWhiskerAngle; set => ((IWallsData)_resource).WallAvoidSideWhiskerAngle = value; }
		//public float WallAvoidWhiskerLength { get => ((IWallsData)_resource).WallAvoidWhiskerLength; set => ((IWallsData)_resource).WallAvoidWhiskerLength = value; }
		public float WallsRaycastDistance { get => ((IWallsData)_resource).WallsRaycastDistance; set => ((IWallsData)_resource).WallsRaycastDistance = value; }
		//public float WallAvoidRayCastMaxDistance { get => ((IWallsData)_resource).WallAvoidRayCastMaxDistance; set => ((IWallsData)_resource).WallAvoidRayCastMaxDistance = value; }
		public LayerMask WallsLayers { get => ((IWallsData)_resource).WallsLayers; set => ((IWallsData)_resource).WallsLayers = value; }
		public float SteeringInterestEpsilon => ((ISteeringNodeData)_resource).SteeringInterestEpsilon;
		public float SteeringAvoidanceEpsilon => ((ISteeringNodeData)_resource).SteeringAvoidanceEpsilon;
		public float SteeringSpeedLerp => ((ISteeringNodeData)_resource).SteeringSpeedLerp;
		//public AnimationCurve WallsInterestCurve { get => ((IWallsData)_resource).WallsInterestCurve; set => ((IWallsData)_resource).WallsInterestCurve = value; }
		//public Range_ WallsInterest { get => ((IWallsData)_resource).WallsInterest; set => ((IWallsData)_resource).WallsInterest = value; }
		public AnimationCurve WallsAvoidanceCurve { get => ((IWallsData)_resource).WallsAvoidanceCurve; set => ((IWallsData)_resource).WallsAvoidanceCurve = value; }
		public Range_ WallsAvoidance { get => ((IWallsData)_resource).WallsAvoidance; set => ((IWallsData)_resource).WallsAvoidance = value; }
		public float EvadeMaxPredictionTime => ((IEvadeData)_resource).EvadeMaxPredictionTime;
		public AnimationCurve EvadeAvoidanceCurve => ((IEvadeData)_resource).EvadeAvoidanceCurve;
		public Range_ EvadeAvoidance => ((IEvadeData)_resource).EvadeAvoidance;
		public float EvadeMaxDistance => ((IEvadeData)_resource).EvadeMaxDistance;
		private VigilantNode _resource;
		public Action<AiBehavtree, AiBtFlags> targetAbilityCb;
		public Action<CharacterObject, DirectorTokenInstance> targetTokenAcquiredCb;
		public Action<CharacterObject, DirectorTokenInstance> targetTokenReturnedCb;
		public float avoidSteeringWeight = 1;
		public DirectorTokenInstance targetToken;
		public Quaternion desiredLerpRotation = Quaternion.identity;

		public VigilantNodeInstance(VigilantNode resource)
		{
			_resource = resource;
		}
	}

	public class VigilantNode
	: NodeBase
	, ILocalAvoidance
	, IWallsData
	, ISteeringNodeData
	, IEvadeData
	{
		[SerializeField]
		public EntityFlags targets;

		[SerializeField]
		public Range_ targetTime = new Range_(1, 2);
		
		[SerializeField]
		[Range(0f, 1f)]
		public float _dodgeProbability = 0.6f;

		[SerializeField]
		private float _vigilantDistance = 5f;

		[SerializeField]
		private float _rotationLerpSpeed = 20f;


		[SerializeField]
		[SerializeEmbedded]
		[FormerlySerializedAs("localAvoidance")]
		public LocalAvoidance _localAvoidance;

		public float LocalAvoidanceWeight { get => 1; set { } }
		public float LocalAvoidancePriority => _localAvoidance.LocalAvoidancePriority;
		public float LocalAvoidanceRadius => _localAvoidance.LocalAvoidanceRadius;
		public float LocalAvoidanceHeight => _localAvoidance.LocalAvoidanceHeight;
		public float LocalAvoidanceMaxSpeed => _localAvoidance.LocalAvoidanceMaxSpeed;
		public float LocalAvoidanceCenter => _localAvoidance.LocalAvoidanceCenter;
		public float LocalAvoidanceTimeHorizon => _localAvoidance.LocalAvoidanceTimeHorizon;
		public float LocalAvoidanceObstacleTimeHorizon => _localAvoidance.LocalAvoidanceObstacleTimeHorizon;
		public int LocalAvoidanceMaxNeighbours => _localAvoidance.LocalAvoidanceMaxNeighbours;
		public LocalAvoidanceLayers LocalAvoidanceLayer => _localAvoidance.LocalAvoidanceLayer;
		public LocalAvoidanceLayers LocalAvoidanceCollidesWith => _localAvoidance.LocalAvoidanceCollidesWith;
		public bool LocalAvoidanceLocked => _localAvoidance.LocalAvoidanceLocked;
		public bool LocalAvoidanceLockedWhenNotMoving => _localAvoidance.LocalAvoidanceLockedWhenNotMoving;
		public LocalAvoidancePriority[] LocalAvoidancePriorities => _localAvoidance.LocalAvoidancePriorities;


		[field: SerializeField]
		public float WallsRaycastDistance { get; set; } = 2f;


		[field: SerializeField]
		public LayerMask WallsLayers { get; set; }

		[field: SerializeField]
		public AnimationCurve WallsAvoidanceCurve { get; set; }

		[field: SerializeField]
		public Range_ WallsAvoidance { get; set; } = 1..2;

		[field: SerializeField]
		public float SteeringInterestEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringAvoidanceEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringSpeedLerp { get; set; } = 25f;

		[field: SerializeField]
		public float EvadeMaxPredictionTime { get; set; } = 2;

		[field: SerializeField]
		public AnimationCurve EvadeAvoidanceCurve { get; set; }

		[field: SerializeField]
		public Range_ EvadeAvoidance { get; set; } = 1..4;

		[field: SerializeField]
		public float EvadeMaxDistance { get; set; } = 25f;

		private static void _UpdateTarget(
			AiBehavtree context
			, ActionNodeInstance<AiBehavtree, VigilantNodeInstance> node
			, EntityObjectBase target)
		{
			if(context.target != null)
				context.target.Behavtree.onFlagsChangedHandler -= node.data.targetAbilityCb;
			context.target = target;
			context.target.Behavtree.onFlagsChangedHandler += node.data.targetAbilityCb;
		}

		protected override NodeInstanceBase _CreateInstance()
		{
			Comparer<EntityObjectBase> comparer = null;

			return new VigilantNodeInstance(this)
			{
				new ActionNodeInstance<AiBehavtree, VigilantNodeInstance>(
				(context, node) =>
				{
					node.data.targetAbilityCb = (source, flags) =>
					{
						if((flags & AiBtFlags.Ability) != 0)
						{
							//context.Ai.Behavtree.Blackboard.Set(
							//_dodgeProbability.Chance() ?
							//AiBtFirstMsg.Dodge :
							//AiBtFirstMsg.Defend);
						}
					};
					node.data.targetTokenAcquiredCb = (target, token) =>
					{
						node.data.targetToken = token;
						context.target = target;
					};
					node.data.targetTokenReturnedCb = (target, token) =>
					{
						node.data.targetToken = null;
						context.target = null;
						if(context.CharacterObject.Encounter.Get(context.targets, targets))
						{
							context.targets.Sort(comparer);
							_UpdateTarget(context, node, context.targets[0]);
						}
					};
					comparer = Comparer<EntityObjectBase>.Create((i1, i2) =>
					{
						return
						(i1.Position - context.Position).magnitude.CompareTo(
						(i2.Position - context.Position).magnitude);
					});
				}
				, (context, node) =>
				{
					if(context.CharacterObject.Encounter.Get(context.targets, EntityInstanceFlags.Leader))
					{
						foreach(var target in context.targets)
						{
							if(target.group == null) continue;
							if(target.TestFlags(targets))
							{
								target.group.director.onTokenAcquiredHandler += node.data.targetTokenAcquiredCb;
								target.group.director.onTokenReturnedHandler += node.data.targetTokenReturnedCb;
							}
						}
					}

					return NodeResult.Success;
				}
				, (context, node) =>
				{
					if(context.CharacterObject.Encounter.Get(context.targets, EntityInstanceFlags.Leader))
					{
						foreach(var target in context.targets)
						{
							if(target.TestFlags(targets))
							{
								target.group.director.onTokenAcquiredHandler -= node.data.targetTokenAcquiredCb;
								target.group.director.onTokenReturnedHandler -= node.data.targetTokenReturnedCb;
							}
						}
					}

					return NodeResult.Success;
				})
				, new ConcurrentNodeInstance
				{
					new SequenceNodeInstance
					{
						new ActionNodeInstance<AiBehavtree, VigilantNodeInstance>(
						(context, node) =>
						{
							if(node.data.targetToken == null)
							{
								if(context.CharacterObject.Encounter.Get(context.targets, targets))
								{
									context.targets.Sort(comparer);
									_UpdateTarget(context, node, context.targets[0]);

									return NodeResult.Success;
								}
							}

							return NodeResult.Failed;
						})
					}
					, new CustomUpdateNodeInstance<AiBehavtree, VigilantNodeInstance>(
					(context, node, dt) =>
					{
						// TODO: only change head direction if far away

						Vector3 displ = (context.target.Position - context.Position);
						displ.y = 0;
						context.CharacterObject.CharacterAnimations.HeadDirection = displ.normalized;
					}
					, (context, node, dt) =>
					{
						//var anims = context.CharacterObject.CharacterAnimations;
						//anims.UpperChestRotation = node.data.desiredLerpRotation;
						//anims.UpperChestRotation = Quaternion.Lerp(
						//	anims.UpperChestRotation,
						//	node.data.desiredLerpRotation,
						//	_rotationLerpSpeed * dt);
					})
					, new UpdateNodeInstance<AiBehavtree, VigilantNodeInstance>((context, node) =>
					{
						node.data.LocalAvoidanceWeight = 1 - (context.target - context.CharacterObject).magnitude / _vigilantDistance;
						return NodeResult.Running;
					})					
					, new SteeringNodeInstance<AiBehavtree, VigilantNode>("Steering (Vigilant)", this
					, (context, node) =>
					{
						Vector3 displ = (context.target.Position - context.Position);
						return NodeResult.Running;
					})
					{
						new EvadeEvaluator<AiBehavtree, VigilantNode>()
						, new WallsEvaluator<AiBehavtree, VigilantNode>()
					}
					, new SteeringCombineNodeInstance<AiBehavtree, VigilantNode>("Steering Combine (Vigilant)", this)
					, new SteeringLocomotionNodeInstance(this)
					, new CustomUpdateNodeInstance2<AiBehavtree, VigilantNode>("Steering Rotation (Vigilant)"
					, (context, node, dt) =>
					{
					}
					, (context, node, dt) =>
					{
						context.Control.rotation = Quaternion.Lerp(
							context.Control.rotation,
							context.Control.desiredLerpRotation,
							context.Control.rotationLerpSpeed * dt);
					})
				}
			};
		}
	}
}
