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

namespace Cirrus.Arpg.Content.AI
{
	public class BystanderNode : NodeBase
	{
		[SerializeField]
		public float ObstaclesRadius;

		[SerializeField]
		public LayerMask ObstaclesLayers;

		[SerializeField]
		public AnimationCurve ObstaclesAvoidanceCurve;

		[SerializeField]
		public Range_ ObstaclesAvoidance;
	
		protected override NodeInstanceBase _CreateInstance()
		{
			return new SequenceNodeInstance("Bystander:Sequence")
			{
				new EventDecoratorInstance<AiBehavtree>(
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
				, (context, flags, other) =>
				{
					if(other == default) return ObserverNodeResult.Undetermined;

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
						var blkb1 = source.Ai.Behavtree.Blackboard;
						int k = 0;
						for (k = 0; k < source.CharacterObject.group.Count; k++)
						{
							var bt2 = source.CharacterObject.group[k].CharacterObject.Ai.Behavtree;
							if (bt2.Flags.Intersects(AiBtFlags.Injured)) break;
						}

						// If all injured teamates, then return early, continue bystander
						if(k != source.CharacterObject.group.Count) return NodeResult.Success;

						blkb1.Set(AiBtFirstMsg.Retreat);
						return NodeResult.Failed;
					})
				}
			};
		}
	}
}
