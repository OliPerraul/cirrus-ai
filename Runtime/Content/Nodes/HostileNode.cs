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
	public class HostileNodeInstance
	: DecoratorInstanceBase
	, ISteeringNodeData
	, ISeekData
	, IWallsData
	{
		public override object Data { get => null; set { } }

		//public float strafeContextWeight = 1.0f;

		// value used to lerp between different hostile steering mechanism
		public float strafeSteeringWeight = 1.0f;

		public float abilitySteeringWeight = 0.0f;

		public float abilityRangeSteeringWeight = 0.0f;

		//public float hostileSteeringWeight = 1.0f;

		public Action<IActiveAbilityInstance> onAbilityEndedCb;
		public Func<AiBehavtree, HostileNodeInstance, IActiveAbilityInstance> abilityCb { get; set; }

		public Func<AiBehavtree, HostileNodeInstance, Vector3> directionCb;

		public Comparer<EntityObjectBase> comparer = null;

		public Action<AiBehavtree, AiBtFlags> targetBehavtreeFlagsCb;

		public Action<AiBehavtree, CharacterBehavtree, CharaBtLocoMsg> targetCb; // TODO remove

		public Action<AiBehavtree, CharacterBehavtree, CharaBtLocoMsg> sourceCb; // TODO remove

		public float SteeringInterestEpsilon => _resource.SteeringInterestEpsilon;

		public float SteeringAvoidanceEpsilon => _resource.SteeringAvoidanceEpsilon;

		public float SteeringSpeedLerp => _resource.SteeringSpeedLerp;
		public float WallsRaycastDistance { get => ((IWallsData)_resource).WallsRaycastDistance; set => ((IWallsData)_resource).WallsRaycastDistance = value; }

		public LayerMask WallsLayers { get => ((IWallsData)_resource).WallsLayers; set => ((IWallsData)_resource).WallsLayers = value; }

		public AnimationCurve WallsAvoidanceCurve { get => ((IWallsData)_resource).WallsAvoidanceCurve; set => ((IWallsData)_resource).WallsAvoidanceCurve = value; }
		public Range_ WallsAvoidance { get => ((IWallsData)_resource).WallsAvoidance; set => ((IWallsData)_resource).WallsAvoidance = value; }

		private HostileNodeBase _resource;

		public HostileNodeInstance(HostileNodeBase resource)
		{
			_resource = resource;
		}
	}

	public abstract class HostileNodeBase
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
		public AnimationCurve WallsAvoidanceCurve { get; set; }

		[field: SerializeField]
		public Range_ WallsAvoidance { get; set; } = 1..2;

		[field: SerializeField]
		public LayerMask WallsLayers { get; set; }

		protected override void _PopulateInstance(NodeInstanceBase node)
		{
			_PopulateInstance((HostileNodeInstance)node);
		}

		protected virtual void _PopulateInstance(HostileNodeInstance node)
		{
		}

		private void _UpdateTarget(AiBehavtree context, HostileNodeInstance node, EntityObjectBase target)
		{
			if(context.target != null)
			{
				context.Ai.LocalAvoidance.Unignore(target);
				context.target.Ai.Behavtree.onFlagsChangedHandler -= node.targetBehavtreeFlagsCb;
			}
			context.target = target;
			context.target.Ai.Behavtree.onFlagsChangedHandler += node.targetBehavtreeFlagsCb;
			context.Ai.LocalAvoidance.Ignore(target);
		}

		protected override NodeInstanceBase _CreateInstance()
		{
			return new HostileNodeInstance(this)
			{
				new SequenceNodeInstance("Hostile Sequence (Hostile)")
				{
					new ActionNodeInstance<AiBehavtree, HostileNodeInstance>(
					(context, node) =>
					{
						node.data.targetBehavtreeFlagsCb = (behavtree, flags) =>
						{
							if((flags & AiBtFlags.Injured) != 0)
							{
								//node.data.hostileSteeringWeight = 0;
							}
						};

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
							_UpdateTarget(context, node.data, context.targets[0]);
						}

						context.Flags |= AiBtFlags.Hostile;
						return NodeResult.Success;
					})
					, new ConcurrentNodeInstance
					{
						new ActionNodeInstance<AiBehavtree, HostileNodeInstance>(
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

								Vector3 displ = (context.target.Position - context.Position);
								var anims = context.CharacterObject.CharacterAnimations;
								anims.HeadDirection = displ;

							}
						})
						// Steering
						, _SteeringNodesOverride != null ? _SteeringNodesOverride :
						new SteeringNodeInstance<AiBehavtree, HostileNodeInstance>("Steering (Hostile)"
						, (context, node) =>
						{
							Vector3 displ = (context.target.Position - context.Position);
							// anything below strafeDistance is strafe=1, anything above pursueDistance is pursue=1
							node.data.strafeSteeringWeight = 1.0f - Numerics.MathUtils.Normalize(displ.magnitude, strafeDistance, pursueDistance);
							node.data.abilityRangeSteeringWeight = Numerics.MathUtils.Normalize(displ.magnitude, context.ability.Range.min, context.ability.Range.max);
							return NodeResult.Running;
						})
						{
							// Pursue
							new SeekEvaluator<AiBehavtree, HostileNodeInstance>((context, node, eval) =>
							{
								return (1.0f - node.data.abilitySteeringWeight) * (1.0f - node.data.strafeSteeringWeight);
							})
							// Ability
							, new SeekEvaluator<AiBehavtree, HostileNodeInstance>((context, node, eval) =>
							{
								return node.data.abilitySteeringWeight * node.data.abilityRangeSteeringWeight;
							})
							, new WallsEvaluator<AiBehavtree, HostileNodeInstance>()
							// Strafe
							, new ContextEvaluator<AiBehavtree, HostileNodeInstance>(
							(context, node, eval) => (1.0f - node.data.abilitySteeringWeight) * node.data.strafeSteeringWeight
							, (context, node, eval) =>
							{
								float angle = Vector3.Angle(Vector3.right, context.Position - context.target.Position);
								if (context.Position.z < context.target.Position.z)
								{
									angle = 360f - angle;
								}

								Vector3 distancePercept = context.Position - context.target.Position;
								if (distancePercept.sqrMagnitude > (strafeRadius - strafeMinDeviation) * (strafeRadius - strafeMinDeviation) &&
									distancePercept.sqrMagnitude < (strafeRadius + strafeMaxDeviation) * (strafeRadius + strafeMaxDeviation))
								{
									angle -= strafeTheta;
								}

								Vector3 destination = context.target.Position;

								destination.x += strafeRadius * Mathf.Cos(angle * (Mathf.PI / 180f));
								destination.z += strafeRadius * Mathf.Sin(angle * (Mathf.PI / 180f));

								Vector3 direction = (destination - context.Position);

								for(int i = 0; i < eval._steering.directions.Count; i++)
								{
									if(direction.Dot(eval._steering.directions[i], strafeThreshold, out float dot))
									{
										eval._steering.interests[i] = dot * strafeSpeed;
									}
								}
							})
						}
						, _SteeringNodesOverride != null ? _SteeringNodesOverride :
						new SteeringCombineNodeInstance<AiBehavtree, HostileNodeBase>("Steering Combine (Hostile)", this)
						, new SteeringLocomotionNodeInstance("Steering Locomotion (Hostile)", this)
						, new CustomUpdateNodeInstance2<AiBehavtree, HostileNodeInstance>("Steering Rotation (Hostile)"
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

							// weigh pursue direction against strafe direction
							direction = 
								(node.data.strafeSteeringWeight * (context.target.Position - context.Position))  + 
								((1.0f - node.data.strafeSteeringWeight) * direction );

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
						, new RepeatDecoratorInstance("Ability Sequence (Hostile)")
						{
							new UpdateNodeInstance<AiBehavtree, HostileNodeInstance>("Ability Request (Hostile)"
							, (context, node) =>
							{
								node.data.abilitySteeringWeight = 0.0f;
								if(
									context.ability != null
									&& context.Ai.Director != null
									&& context.Ai.Director.Request(context.CharacterObject, context.ability))
								{
									node.data.abilitySteeringWeight = 1f;
									return NodeResult.Success;
								}

								return NodeResult.Running;
							})
							, new UpdateNodeInstance<AiBehavtree, HostileNodeInstance>("Ability (Hostile)"
							, (context, node) =>
							{
								node.data.onAbilityEndedCb = (ab) =>
								{
									context.Flags &= ~AiBtFlags.Ability;
									context.Ai.Director.Return(context.CharacterObject, ab);
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
									if(
									context.ability.IsAvailable(context.CharacterObject)
									&& context.ability.Start(context.CharacterObject, context.target))
									{
										context.Flags |= AiBtFlags.Ability;
										context.ability.OnAvailableHandler += node.data.onAbilityEndedCb;
										return NodeResult.Success;
									}
								}

								return NodeResult.Running;
							})
							, new UpdateNodeInstance<AiBehavtree, HostileNodeInstance>("Ability Return (Hostile)"
							, (context, node) =>
							{
								if(context.Ai.directorRequest == null)
								{
									node.data.abilitySteeringWeight = 0;
									return NodeResult.Success;
								}

								return NodeResult.Running;
							})
							, new WaitNodeInstance("Ability Wait (Hostile)", abilityWait)
						}
					}
					, new ActionNodeInstance<AiBehavtree>(
					(context, node) =>
					{
						context.Flags &= ~AiBtFlags.Hostile;
						return NodeResult.Success;
					})
				}
			};
		}
	}
}
