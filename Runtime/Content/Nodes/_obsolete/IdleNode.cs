//using System;
//using System.Collections.Generic;
//using Cirrus.Animations;
//using Cirrus.Broccoli;
//using Cirrus.Controls;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Arpg.Entities;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Unity.Objects;
//using Cirrus.Numerics;
//using Cirrus.Unity.Editor;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using static Cirrus.Debugging.DebugUtils;
//using Cirrus.Arpg.Conditions;

//namespace Cirrus.Arpg.Content.AI
//{
//	public class IdleNode : NodeComponentBase
//	{
//		[SerializeField]
//		private Range_ _waitSeconds = new Range_(1f, 4f);

//		protected override NodeInstanceBase _CreateInstance()
//		{
//			return new WaitNodeInstance(_waitSeconds);
//		}
//	}
//}
