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
using Cirrus.Controls;
using Cirrus.Objects;

namespace Cirrus.Arpg.Content.AI
{
	public class RestrainNode
	: CachedCopiableResourceAssetBase<NodeInstanceBase>
	, IArriveData
	, ISteeringNodeData
	{
		[field: SerializeField]
		public float ArrivalRadius { get; set; }

		[field: SerializeField]
		public Range_ ArrivalInterest { get; set; }

		[field: SerializeField]
		public AnimationCurve ArrivalInterestCurve { get; set; }

		[field: SerializeField]
		public float SteeringInterestEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringAvoidanceEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringSpeedLerp { get; set; } = 25f;

		protected override NodeInstanceBase _CreateInstance()
		{
			return new SequenceNodeInstance
			{
				new ActionNodeInstance<AiBehavtree>((context, node) =>
				{
					context.target = context.CharacterObject.Party.Leader.EntityObject;
					//Assert(context.Target != context.Entity, true);
					//Assert(!context.Target.IsPrefab(), true);
					return NodeResult.Success;
				})
				, new RepeatDecoratorInstance
				{
					new SequenceNodeInstance
					{
						new ConcurrentNodeInstance(NodeResultQuantifier.One)
						{
							new ConcurrentNodeInstance("Restrain Steering")
							{
								new UpdateNodeInstance<AiBehavtree, None>("Action", (context, node) =>
								{
									CharacterObject target = context.target as CharacterObject;
									context.destination = context.target.Position;
									Vector3 distance = target.GroundPosition - context.GroundPosition;

									//  instead of a constant value for T, a dynamic one is used
									//  The new T is calculated based on the distance between the two
									//  characters and the maximum velocity the target can achieve.
									//  In simple words the new T means "how many updates the target
									//  needs to move from its current position to the pursuer position".
									//
									//  The longer the distance, the higher T will be, so the pursuer will
									//  seek a point far ahead the target. The shorter the distance, the lower T
									//  will be, meaning it will seek a point very close to the target.
									//  The new code for that implementation is:

									float t = distance.magnitude / context.Kinematics.MaxSpeed;
									context.destination = target.Position + t * target.Kinematics.Velocity;
									return NodeResult.Running;
								})
								, new SteeringNodeInstance<AiBehavtree, RestrainNode>("Steering 1")
								{
									//new ArriveEvaluator<AiBehavtree, RestrainNode>()
								}
								, new SteeringLocomotionNodeInstance("Steering Locomotion", this)
								, new SteeringRotationNodeInstance("Steering Rotation", this)
							}
							, new UpdateNodeInstance<AiBehavtree, None>("Hold Down", (context, node) =>
							{
								if(
								context.EntityObject
								.ColliderDistance(context.target)
								.Almost(context.Control.GrabDistance))
								{
									return NodeResult.Success;
								}

								return NodeResult.Running;
							})
						}
						, new ActionNodeInstance<AiBehavtree>(
						(context, node) =>
						{
							if(
							context.EntityObject
							.ColliderDistance(context.target)
							.Almost(context.Control.GrabDistance))
							{
								return NodeResult.Failed;
							}
							context.Avatar.Interact.Grab(KeyPressState.Pressed, context.target);
							return  NodeResult.Running;
						},
						(context, node) =>
						{
							context.Avatar.Interact.Grab(KeyPressState.Released, context.target);
							return NodeResult.Success;
						})
					}
				}
			};
		}
	}
}
