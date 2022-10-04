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

//namespace Cirrus.Arpg.Content.AI
//{
//	public class DistractedData
//	{
//		public Vector3 pos;
//		public Func<ControlBt, IActiveAbility> getAbilityCb;
//		public Func<ControlBt, Vector3> directionCb;
//		public EntityObjectFlags targets;
//		public IActiveAbility[] abilities;
//		public Action<CharacterBt, CharaBtLocoMsg> targetCb;
//		public Action<CharacterBt, CharaBtLocoMsg> sourceCb;
//	}

//	public class DistractedInitNode : NodeBase
//	{
//		protected override NodeInstanceBase _GetInstance()
//		{
//			return new ControlNode<DistractedData>
//			{
//				EnterCb = (context, node) =>
//				{
//					var chara = context.CharacterObject;
//					var data = node.data;
//					if (
//						chara
//						.Encounter.Get(data.targets, out List<EntityObjectBase> targets)
//						)
//					{
//						targets.Sort(Comparer<EntityObjectBase>.Create((i1, i2) =>
//						{
//							return
//							-
//							(i1.Position - chara.Position).magnitude.CompareTo(
//							(i2.Position - chara.Position).magnitude);
//						}));
//						context.Target = targets[0];
//					}
//					return ActionNodeResult.Success;
//				}
//			};
//		}
//	}
//}
