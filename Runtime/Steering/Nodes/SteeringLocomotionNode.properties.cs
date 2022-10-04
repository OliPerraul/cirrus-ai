// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Broccoli;
using Cirrus.Collections;
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
	public partial class SteeringLocomotionNodeInstance
	{
		private ISteeringNodeData _data;
		public override object Data { get => _data; set => _data = (ISteeringNodeData)value; }

		public AiBehavtree behavtree;
		public override BehavtreeContextBase Context { get => behavtree; set => behavtree = (AiBehavtree)value; }

		public SteeringLocomotionNodeInstance() : base()
		{
		}

		public SteeringLocomotionNodeInstance(object data) : base(data)
		{
		}

		public SteeringLocomotionNodeInstance(string name) : base(name)
		{
		}

		public SteeringLocomotionNodeInstance(string name, object data) : base(name, data)
		{
		}

		public SteeringLocomotionNodeInstance(string name, ISteeringNodeData data) : base(name, data)
		{
			_data = data;
		}

		public SteeringLocomotionNodeInstance(ISteeringNodeData data) : base(data)
		{
			_data = data;
		}

		//public float speedLerp = 25f;

		//public Vector3 DesiredLocomotionVelocity;}

	}
}