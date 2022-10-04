using System;
using System.Collections.Generic;
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
using Cirrus.Arpg.Conditions;
using Cirrus.Events;
using Cirrus.Arpg.UI.Legacy;
using Cirrus.Objects;

namespace Cirrus.Arpg.Content.AI
{
	public class RescueData
	{
		//public CompositeEvent barrier;
	}

	public class RescueNode 
	: NodeBase
	, ISteeringNodeData
	{

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
					if(context.Flags.Intersects(AiBtFlags.Injured)) return NodeResult.Failed;
					context.Flags |= AiBtFlags.RescueDecision | AiBtFlags.Rescuer;
					return NodeResult.Success;
				})
				, new EventDecoratorInstance<AiBehavtree>(
				new EventListener<AiBehavtree, AiBehavtree, AiBtFlags, ObserverNodeResult>(
				(context, callback) =>
				{
					var source = context.EntityObject;
					for(int i = 0; i < source.group.Count; i++)
					{
						var chara = source.group[i].CharacterObject;
						if (context.Flags.Intersects(AiBtFlags.Injured)) continue;
						context.onFlagsChangedHandler += callback;
					}
				}
				, (context, callback) =>
{
					var source = context.EntityObject;
					for(int i = 0; i < source.group.Count; i++)
					{
						var chara = source.group[i].CharacterObject;
						context.onFlagsChangedHandler -= callback;
					}
				}
				, (context, changed, flags) =>
				{
					if(changed == default) return ObserverNodeResult.Undetermined;

					var source = context.EntityObject;
					for (int i = 0; i < source.group.Count; i++)
					{
						var chara = source.group[i].CharacterObject;
						if (!context.Flags.Intersects(AiBtFlags.RescueDecision | AiBtFlags.Injured))
						{
							return ObserverNodeResult.Undetermined;
						}
					}

					return ObserverNodeResult.Success;
				}))				
				{
					new ActionNodeInstance<AiBehavtree>((context, node) =>
					{
						var source = context.CharacterObject;
						int k = 0;
						for (k = 0; k < source.group.Count; k++)
						{
							var chara = source.group[k].CharacterObject;
							var bt = chara.Ai.Behavtree;
							if (bt.Flags.Intersects(AiBtFlags.Injured)) break;
						}
						// If no injured teamates, then return early
						if(k == source.group.Count) return NodeResult.Failed;


						List<IKnapsackItem> rescuees = new List<IKnapsackItem>();

						for (int i = 0; i<context.Group.Count; i++)
						{
							CharacterObject chara = context.Group[i].CharacterObject;
							var bt = chara.Ai.Behavtree;
							// Similar to knapsack problem:
							// How to maximise possible rescue
							if (chara != null && bt.Flags.Intersects(AiBtFlags.Injured)) rescuees.Add(chara.Ai.Rescuee);
						}

						float rescuerStrength = 0;
						List<RescuerComponent> rescuers = new List<RescuerComponent>();
						for (int i = 0; i<context.Group.Count; i++)
						{
							CharacterObject chara = context.Group[i].CharacterObject;
							var bt = chara.Ai.Behavtree;
							// Similar to knapsack problem:
							// How to maximise possible rescue
							if (chara != null && bt.Flags.Intersects(AiBtFlags.Rescuer))
							{
								rescuerStrength += chara.Ai.Rescuer.Strength;
								rescuers.Add(chara.Ai.Rescuer);
							}
						}

						KnapsackResult result = RescueKnapsackSolver.ModifiedGreedyKnapsack(rescuerStrength, rescuees);

						return NodeResult.Failed;
					})
				}
				, new ConcurrentNodeInstance("Rescue Steering")
				{
					new SteeringNodeInstance<AiBehavtree, ISteeringNodeData>("Steering 1", this)
					{				
						//new SeekEvaluator<ExitData>(),
						//new CollisionAvoidanceEvaluator<ExitData>(),
						//new QueuingEvaluator<ExitData>(),
						//new SeparationEvaluator<ExitData>()
					}
					, new SteeringCombineNodeInstance<AiBehavtree, ISteeringNodeData>("Steering Context", this)
					, new SteeringLocomotionNodeInstance("Steering Locomotion", this)
					, new SteeringRotationNodeInstance("Steering Rotation", this)
				}
			};
		}
	}
}
