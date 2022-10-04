
//using Cirrus.Animations;
//using Cirrus.Broccoli;
//using Cirrus.Controls;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Arpg.Entities;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Unity.Objects;
//using Cirrus.Numerics;
//using Cirrus.Unity.Editor;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static Cirrus.Debugging.DebugUtils;
//using Cirrus.Arpg.AI;

//namespace Cirrus.Arpg.Content.AI
//{
//	public class DistractedAbilityNode : NodeBase
//	{
//		protected override NodeInstanceBase _GetInstance()
//		{
//			return new UpdateNodeInstance<AiBtBase>((context, node) =>
//			{
//				if(context.Ability != null)
//				{
//					Range_ range = context.Ability.Range;
//					EntityObjectBase obj = node.context.EntityObject;
//					Vector3 sourceTarget = context.Target.Position - obj.Position;
//					if(sourceTarget.magnitude < range.Max)
//					{
//						if(
//						context.Ability
//						.IsAvailable(context.CharacterObject) &&
//						context.Ability
//						.Start(context.CharacterObject, context.Target)
//						)
//						{
//							context.AbilityPosition = context.CharacterObject.Position;
//						}
//					}
//				}

//				return ActionNodeResult.Running;
//			});
//		}
//	}	
//}
