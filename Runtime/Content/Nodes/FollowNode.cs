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

namespace Cirrus.Arpg.Content.AI
{
	public class FollowNode 
	: NodeBase
	, IArriveData
	, ISteeringNodeData
	, IWallsData
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

		[field: SerializeField]
		public float WallAvoidDistance { get; set; } = 2.0f;

		[field: SerializeField]
		public float WallsRaycastDistance { get; set; } = 2.0f;

		[field: SerializeField]
		public float WallAvoidRayCastMaxDistance { get; set; } = 2.0f;

		[field: SerializeField]
		public LayerMask WallsLayers { get; set; }

		[field: SerializeField]
		public AnimationCurve WallsAvoidanceCurve { get; set; }

		[field: SerializeField]
		public Range_ WallsAvoidance { get; set; } = 1..2;

		//[SerializeField]
		//public SteeringNodesFlags followSteeringFlags = (SteeringNodesFlags)BitwiseUtils.Everything;

		protected override NodeInstanceBase _CreateInstance()
		{
			return new SequenceNodeInstance
			{
				new ActionNodeInstance<AiBehavtree>("Init", (context, node) =>
				{
					context.target = context.CharacterObject.Party.Leader;
					Assert(context.target != context.CharacterObject, true);
					Assert(!context.target.CharacterObject.IsPrefab(), true);
					return NodeResult.Success;
				})
				, new ConcurrentNodeInstance("Follow")
				{
					new ActionNodeInstance<AiBehavtree>("Action", null, null, (context, node) =>
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
					, new SteeringNodeInstance<AiBehavtree, FollowNode>("Steering (Follow)", this)
					{
						new ArriveEvaluator<AiBehavtree, FollowNode>()
						, new WallsEvaluator<AiBehavtree, FollowNode>()
					}
					, new SteeringCombineNodeInstance<AiBehavtree, FollowNode>("Steering Combine (Follow)", this)
					, new SteeringLocomotionNodeInstance("Steering Locomotion (Follow)", this)
					, new SteeringRotationNodeInstance("Steering Rotation (Follow)", this)
				}
			};
		}
	}
}
