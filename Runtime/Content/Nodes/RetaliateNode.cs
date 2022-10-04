using Cirrus.Animations;
using Cirrus.Broccoli;
using Cirrus.Controls;
using Cirrus.Arpg.Abilities;
using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Objects;
using Cirrus.Numerics;
using Cirrus.Unity.Editor;
using Cirrus.Unity.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;
using Cirrus.Arpg.AI;
using Cirrus.Unity;
using Cirrus.Objects;

namespace Cirrus.Arpg.Content.AI
{

	public class RetaliateNodeInstance
	: DecoratorInstanceBase
	, ISteeringNodeData
	, ISeekData
	, IWallsData
	{
		public override object Data { get => null; set { } }

		public float abilitySteeringWeight = 0.0f;

		public float abilityRangeSteeringWeight = 0.0f;

		public Action<IActiveAbilityInstance> onAbilityEndedCb;
		public Func<AiBehavtree, RetaliateNodeInstance, IActiveAbilityInstance> abilityCb { get; set; }

		public Func<AiBehavtree, RetaliateNodeInstance, Vector3> directionCb;

		public Comparer<EntityObjectBase> comparer = null;

		public Action<AiBehavtree, CharacterBehavtree, CharaBtLocoMsg> targetCb;

		public Action<AiBehavtree, CharacterBehavtree, CharaBtLocoMsg> sourceCb;

		public float SteeringInterestEpsilon => _resource.SteeringInterestEpsilon;

		public float SteeringAvoidanceEpsilon => _resource.SteeringAvoidanceEpsilon;

		public float SteeringSpeedLerp => _resource.SteeringSpeedLerp;

		//public float WallAvoidDistance { get => ((IWallsData)_resource).WallAvoidDistance; set => ((IWallsData)_resource).WallAvoidDistance = value; }
		//public float WallAvoidSideWhiskerAngle { get => ((IWallsData)_resource).WallAvoidSideWhiskerAngle; set => ((IWallsData)_resource).WallAvoidSideWhiskerAngle = value; }
		//public float WallAvoidWhiskerLength { get => ((IWallsData)_resource).WallAvoidWhiskerLength; set => ((IWallsData)_resource).WallAvoidWhiskerLength = value; }
		public float WallsRaycastDistance { get => ((IWallsData)_resource).WallsRaycastDistance; set => ((IWallsData)_resource).WallsRaycastDistance = value; }
		//public float WallAvoidRayCastMaxDistance { get => ((IWallsData)_resource).WallAvoidRayCastMaxDistance; set => ((IWallsData)_resource).WallAvoidRayCastMaxDistance = value; }
		public LayerMask WallsLayers { get => ((IWallsData)_resource).WallsLayers; set => ((IWallsData)_resource).WallsLayers = value; }
		//public AnimationCurve WallsInterestCurve { get => ((IWallsData)_resource).WallsInterestCurve; set => ((IWallsData)_resource).WallsInterestCurve = value; }
		//public Range_ WallsInterest { get => ((IWallsData)_resource).WallsInterest; set => ((IWallsData)_resource).WallsInterest = value; }
		public AnimationCurve WallsAvoidanceCurve { get => ((IWallsData)_resource).WallsAvoidanceCurve; set => ((IWallsData)_resource).WallsAvoidanceCurve = value; }
		public Range_ WallsAvoidance { get => ((IWallsData)_resource).WallsAvoidance; set => ((IWallsData)_resource).WallsAvoidance = value; }

		private RetaliateNodeBase _resource;

		public RetaliateNodeInstance(RetaliateNodeBase resource)
		{
			_resource = resource;
		}
	}

	public abstract class RetaliateNodeBase
	: NodeBase
	, ISteeringNodeData
	, IWallsData
	{
		// TODO
		[SerializeField]
		public Range_ targetTime;

		[SerializeField]
		public EntityFlags targets;

		[SerializeField]
		public Noise _noise;

		[SerializeField]
		private Range_ abilityWait = 1..2;

		public float strafeTheta = 20.0f;

		[SerializeField]
		public float strafeThreshold = 0.05f;

		[SerializeField]
		public float strafeDistance = 6f;

		[SerializeField]
		public float strafeRadius = 5f;

		[SerializeField]
		public float strafeSpeed = 5f;

		[SerializeField]
		public float strafeGizmoSize = 0.5f;

		[SerializeField]
		public float strafeMinDeviation = 1f;

		[SerializeField]
		public float strafeMaxDeviation = 1f;

		[SerializeField]
		public float pursueDistance = 12.0f;


		[field: SerializeField]
		public float SteeringInterestEpsilon { get; set; } = 0.01f;


		[field: SerializeField]
		public float SteeringAvoidanceEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringSpeedLerp { get; set; } = 25f;

		private IEnumerable<NodeInstanceBase> _steeringNodesOverride;
		protected virtual NodeInstanceBase _CreateSteeringNodesOverride() => null;
		private IEnumerable<NodeInstanceBase> _SteeringNodesOverride => _steeringNodesOverride == null ?
			_steeringNodesOverride = _CreateSteeringNodesOverride() :
			_steeringNodesOverride;

		[field: SerializeField]
		public float WallAvoidDistance { get; set; } = 2f;

		[field: SerializeField]
		public float WallsRaycastDistance { get; set; } = 2f;

		[field: SerializeField]
		public float WallAvoidRayCastMaxDistance { get; set; } = 2f;


		[field: SerializeField]
		public LayerMask WallsLayers { get; set; }

		[field: SerializeField]
		public AnimationCurve WallsAvoidanceCurve { get; set; }

		[field: SerializeField]
		public Range_ WallsAvoidance { get; set; } = 1..2;

		protected override void _PopulateInstance(NodeInstanceBase node)
		{
			_PopulateInstance((RetaliateNodeInstance)node);
		}

		protected virtual void _PopulateInstance(RetaliateNodeInstance node)
		{
		}

		private static void _UpdateTarget(AiBehavtree context, EntityObjectBase target)
		{
			if(context.target != null)
				context.Ai.LocalAvoidance.Unignore(target);
			context.target = target;
			context.Ai.LocalAvoidance.Ignore(target);
		}

		protected override NodeInstanceBase _CreateInstance()
		{
			return new RetaliateNodeInstance(this)
			{
				new SequenceNodeInstance("Retaliate Sequence")
				{
					new ActionNodeInstance<AiBehavtree, RetaliateNodeInstance>(
					(context, node) =>
					{
						node.data.comparer = Comparer<EntityObjectBase>.Create((i1, i2) =>
						{
							return
							-
							(i1.Position - context.Position).magnitude.CompareTo(
							(i2.Position - context.Position).magnitude);
						});
					}
					, (context, node) =>
					{
						var chara = context.CharacterObject;
						if (chara.Encounter.Get(context.targets, targets))
						{
							context.targets.Sort(node.data.comparer);
							context.target = context.targets[0];
						}

						context.Flags |= AiBtFlags.Hostile;
						return NodeResult.Success;
					})
					, new ConcurrentNodeInstance
					{
						new ActionNodeInstance<AiBehavtree, RetaliateNodeInstance>(
						(context, node) =>
						{
							Assert(context.CharacterObject != null, true);
							context.ability = node.data.abilityCb(context, node.data);
							return NodeResult.Running;
						}
						, (context, node) =>
						{
							return NodeResult.Success;
						}
						, (context, node, dt) =>
						{
							if(context.ability != null)
							{
								EntityObjectBase obj = context.EntityObject;
								SteeringComponent steering = context.Steering;
								Range_ range = context.ability.Range;

								// Get the distance to the target If we are within the stopping
								// radius then stop (minimum distance)
								// If past the stopping radius then stop
								Vector3 sourceTarget = context.target.Position - steering.Position;
								float dist = sourceTarget.magnitude;
								if(dist < range.max)
								{
									float remainingRangeDist = dist - range.min;
									float rangeDist = range.max - range.min;
									float elapsedRangeDist = rangeDist - remainingRangeDist;
									context.Control.locomotion = Vector3.Lerp(
										steering.Control.locomotion,
										Vector3.zero,
										elapsedRangeDist / rangeDist
										);
									context.Control.desiredLerpRotation = Quaternion.LookRotation(
										sourceTarget.normalized,
										obj.Transform.up);
								}
							}
						})
						// Steering
						, _SteeringNodesOverride != null ? _SteeringNodesOverride :
						new SteeringNodeInstance<AiBehavtree, RetaliateNodeInstance>("Steering"
						, (context, node) =>
						{
							Vector3 displ = (context.target.Position - context.Position);
							node.data.abilityRangeSteeringWeight = Numerics.MathUtils.Normalize(displ.magnitude, context.ability.Range.min, context.ability.Range.max);
							return NodeResult.Running;
						})
						{
							// Pursue
							new SeekEvaluator<AiBehavtree, RetaliateNodeInstance>((context, node, eval) =>
							{
								return (1.0f - node.data.abilitySteeringWeight);
							})
							// Ability
							, new SeekEvaluator<AiBehavtree, RetaliateNodeInstance>((context, node, eval) =>
							{
								return node.data.abilitySteeringWeight * node.data.abilityRangeSteeringWeight;
							})
							, new WallsEvaluator<AiBehavtree, RetaliateNodeInstance>()
						}
						, _SteeringNodesOverride != null ? _SteeringNodesOverride :
						new SteeringCombineNodeInstance<AiBehavtree, RetaliateNodeBase>("Steering Combine", this)
						, new SteeringLocomotionNodeInstance("Steering Locomotion", this)
						, new CustomUpdateNodeInstance2<AiBehavtree, RetaliateNodeInstance>("Steering Rotation"
						, (context, node, dt) =>
						{
							Vector3 direction = context.Transform.forward;

							if(!context
								.Phys
								.locomotion
								.Almost(Vector3.zero))
							{
								// How much of velocity is self-directed (vs induced)
								// how much of "velocity" is "loco velocity"
								float dot = Vector3.Dot(
									context.Phys.locomotion.X_Z().normalized,
									context.Kinematics.Velocity.X_Z().normalized);

								dot = (dot + 1) / 2; // normalize dot as a measure of similarity
								direction = (dot * context.Phys.locomotion) + ((1 - dot) * context.Transform.forward); // use dot as weight to negate direction change by knockback
								direction = direction.X_Z().normalized;
							}	

							context.Control.desiredLerpRotation = Quaternion.LookRotation(
								direction,
								context.Transform.up);
						}
						, (context, node, dt) =>
						{
							context.Control.rotation = Quaternion.Lerp(
								context.Control.rotation,
								context.Control.desiredLerpRotation,
								context.Control.rotationLerpSpeed * dt);
						})
						, new SteeringNodeInstance<AiBehavtree, RetaliateNodeBase>("Steering (Retaliate)", this
						, (context, node) =>
						{
							Vector3 displ = (context.target.Position - context.Position);
							return NodeResult.Running;
						})
						{
							new WallsEvaluator<AiBehavtree, RetaliateNodeBase>()
						}
						, new SteeringCombineNodeInstance<AiBehavtree, RetaliateNodeBase>("Steering Combine (Retaliate)", this)
						, new RepeatDecoratorInstance("Ability Sequence")
						{
							new UpdateNodeInstance<AiBehavtree, RetaliateNodeInstance>("Ability Request"
							, (context, node) =>
							{
								node.data.abilitySteeringWeight = 0.0f;
								if(context.ability != null)
								{
									node.data.abilitySteeringWeight = 1f;
									return NodeResult.Success;
								}

								return NodeResult.Running;
							})
							, new UpdateNodeInstance<AiBehavtree, RetaliateNodeInstance>("Ability"
							, (context, node) =>
							{
								node.data.onAbilityEndedCb = (ab) =>
								{
									context.ability.OnAvailableHandler -= node.data.onAbilityEndedCb;
								};
							}
							, (context, node) =>
							{
								Range_ range = context.ability.Range;
								EntityObjectBase obj = node.context.EntityObject;
								Vector3 toTarget = context.target.Position - obj.Position;
								if(toTarget.magnitude < range.max)
								{
									if(context.ability.IsAvailable(context.CharacterObject)
									&& context.ability.Start(context.CharacterObject, context.target))
									{
										context.ability.OnAvailableHandler += node.data.onAbilityEndedCb;
										return NodeResult.Success;
									}
								}

								return NodeResult.Running;
							})
						}
					}
				}
			};
		}
	}
}
