// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Broccoli;
using Cirrus.Collections;
using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Arpg.UI.Legacy;
using Cirrus.Unity.Numerics;

using System;
using System.Collections.Generic;

//using System.Numerics;
using UnityEngine;

using Range = Cirrus.Unity.Numerics.Range_;

namespace Cirrus.Arpg.AI
{
	public partial class SteeringRotationNodeInstance
	{
		private ISteeringNodeData _data;
		public override object Data { get => _data; set => _data = (ISteeringNodeData)value; }

		public AiBehavtree behavtree;
		public override BehavtreeContextBase Context { get => behavtree; set => behavtree = (AiBehavtree)value; }

		public SteeringRotationNodeInstance() : base()
		{
		}

		public SteeringRotationNodeInstance(object data) : base(data)
		{
		}

		public SteeringRotationNodeInstance(string name) : base(name)
		{
		}

		public SteeringRotationNodeInstance(string name, object data) : base(name, data)
		{
		}

		public SteeringRotationNodeInstance(string name, ISteeringNodeData data) : base(name, data)
		{
			_data = data;
		}

		public SteeringRotationNodeInstance(ISteeringNodeData data) : base(data)
		{
			_data = data;
		}

		//public float speedLerp = 25f;

		//public Vector3 DesiredLocomotionVelocity;
	}
}