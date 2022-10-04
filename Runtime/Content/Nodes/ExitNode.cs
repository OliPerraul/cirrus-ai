using Cirrus.Animations;
using Cirrus.Broccoli;
using Cirrus.Controls;
using Cirrus.Arpg.Abilities;
using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Objects;
using Cirrus.Numerics;
using Cirrus.Unity.Editor;
using Cirrus.Unity.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;
using Cirrus.Objects;

namespace Cirrus.Arpg.Content.AI
{
	// NOTE: the only difference between Retreat and Exit is the door type

	public class ExitNode
	: NodeBase
	, IArriveData
	, ISteeringNodeData
	{
		[SerializeField]
		public DoorType _doorType = DoorType.Exit;

		[field: SerializeField]
		public float ArrivalRadius { get; set; }
		
		[field: SerializeField]
		public AnimationCurve ArrivalInterestCurve { get; set; }

		[field: SerializeField]
		public Range_ ArrivalInterest { get; set; }

		[field: SerializeField]
		public float SteeringInterestEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringAvoidanceEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringSpeedLerp { get; set; } = 25f;

		protected override NodeInstanceBase _CreateInstance()
		{
			return new SequenceNodeInstance("Exit Sequence")
			{
				new ActionNodeInstance<AiBehavtree>((context, node) =>
				{
					if (context.Encounter.Get(out List<DoorObject> targets))
					{
						context.target = targets
						.Where(x => x.Status == DoorStatus.Open)
						.Where(x => x.Type == _doorType)
						.OrderBy(x => (x.Position - context.Position).magnitude)
						.FirstOrDefault();

						return NodeResult.Success;
					}

					return NodeResult.Failed;
				})
				, new ConcurrentNodeInstance
				{
					new UpdateNodeInstance<AiBehavtree, None>((context, node) =>
					{
						context.destination = context.GroundPosition;
						return NodeResult.Running;
					})
					, new SteeringNodeInstance<AiBehavtree, ExitNode>(this)
					{
						//new ArriveEvaluator<AiBehavtree, ExitNode>()
					}
					, new SteeringLocomotionNodeInstance("Steering Locomotion", this)
					, new SteeringRotationNodeInstance("Steering Rotation", this)
				}
			};
		}
	}
}
