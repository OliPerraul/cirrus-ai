// using Cirrus.Unity.AI.BehaviourTrees;
using Cirrus.Collections;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Numerics;
using Cirrus.Unity.Editor;
using Cirrus.Unity.Numerics;
using System;

//using System.Numerics;
using UnityEngine;
using Cirrus.Broccoli;
using static Cirrus.Debugging.DebugUtils;
using System.Xml.Linq;

//using Cirrus.Unity.States._ObsoleteAI2;
//using Cirrus.States._ObsoleteAI;

namespace Cirrus.Arpg.AI
{
	public partial class SteeringLocomotionNodeInstance : TaskNodeInstanceBase
	{
		protected override void _Init()
		{
			base._Init();

			behavtree = (AiBehavtree)Root.Context;
		}

		protected override void _Start()
		{
			base._Start();

			Parent.Schedule(this);

			//context.Control.locomotion = Vector3.zero;
		}

		protected override NodeResult _Exit()
		{
			return base._Exit();
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			behavtree.Control.locomotion = Vector3.zero;

			Parent.Unschedule(this);
		}

		public override void CustomUpdate1(float dt)
		{
			behavtree.Control.locomotion = Vector3.Lerp(
				behavtree.Control.locomotion,
				Vector3.zero,
				_data.SteeringSpeedLerp * dt);
			behavtree.Control.locomotion += behavtree.Steering.acceleration;
			if(behavtree.Control.locomotion.magnitude > behavtree.Kinematics.MaxSpeed)
			{
				behavtree.Control.locomotion = behavtree.Kinematics.MaxSpeed * behavtree.Control.locomotion.normalized;
			}

			// TODO : Other type of orientation ?			
		}
	}

}