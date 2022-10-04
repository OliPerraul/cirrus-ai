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

namespace Cirrus.Arpg.Content.AI
{
	public class AvoidantNodeInstance : DecoratorInstanceBase
	{
		public override object Data { get => null; set { } }

		private AvoidantNode _resource;

		public Action<AiBehavtree, AiBtFlags> targetAbilityCb;

		public AvoidantNodeInstance(AvoidantNode resource)
		{
			_resource = resource;
		}
	}

	public class AvoidantNode
	: NodeBase
	{
		[SerializeField]
		public EntityFlags targets;

		[SerializeField]
		public Range_ targetTime = new Range_(1, 2);

		[SerializeField]
		[Range(0f, 1f)]
		public float _dodgeProbability = 0.6f;

		protected override NodeInstanceBase _CreateInstance()
		{
			Comparer<EntityObjectBase> comparer = null;

			return new AvoidantNodeInstance(this)
			{
				new InitNodeInstance<AiBehavtree, AvoidantNodeInstance>((context, node) =>
				{
					node.data.targetAbilityCb = (source, flags) =>
					{
						if((flags & AiBtFlags.Ability) != 0)
						{
							context.Ai.Behavtree.Blackboard.Set(
							_dodgeProbability.Chance() ?
							AiBtFirstMsg.Dodge :
							AiBtFirstMsg.Defend);
						}

					};
					comparer = Comparer<EntityObjectBase>.Create((i1, i2) =>
					{
						return -
						(i1.Position - context.Position).magnitude.CompareTo(
						(i2.Position - context.Position).magnitude);
					});
				})
				, new SequenceNodeInstance
				{
					new ActionNodeInstance<AiBehavtree, AvoidantNodeInstance>(
					(context, node) =>
					{
						var chara = context.CharacterObject;
						if(chara.Encounter.Get(context.targets, targets))
						{
							context.targets.Sort(comparer);
							if(context.target != null)
							{
								context.target.Behavtree.onFlagsChangedHandler -= node.data.targetAbilityCb;
							}
							context.target = context.targets[0];
							context.target.Behavtree.onFlagsChangedHandler += node.data.targetAbilityCb;

							return NodeResult.Success;
						}

						return NodeResult.Failed;
					})
					, new TimeDecoratorInstance(targetTime)
					{
						new CustomUpdateNodeInstance<AiBehavtree, AvoidantNodeInstance>(
						(context, node, dt) =>
						{
							Vector3 direction = (context.target.Position - context.Position).normalized;
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
					}
				}
			};
		}
	}
}
