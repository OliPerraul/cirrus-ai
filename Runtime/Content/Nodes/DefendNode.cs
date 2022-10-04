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

namespace Cirrus.Arpg.Content.AI
{
	public class DefendNodeInstance : DecoratorInstanceBase
	{
		public override object Data { get => null; set { } }

		private DefendNode _resource;

		public Action<AiBehavtree, AiBtFlags> targetAbilityCb;

		public DefendNodeInstance(DefendNode resource)
		{
			_resource = resource;
		}
	}

	public class DefendNode
	: NodeBase
	{
		[SerializeField]
		public EntityFlags targets;

		[SerializeField]
		public Range_ targetTime = new Range_(1, 2);

		protected override NodeInstanceBase _CreateInstance()
		{
			return new DefendNodeInstance(this)
			{
				
			};
		}
	}
}
