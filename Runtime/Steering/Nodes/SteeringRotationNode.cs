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
	public partial class SteeringRotationNodeInstance : TaskNodeInstanceBase
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
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			Parent.Unschedule(this);
		}

		public override void CustomUpdate1(float dt)
		{
			Vector3 direction = behavtree.Transform.forward;

			if(!behavtree
				.Phys
				.locomotion
				.Almost(Vector3.zero))
			{
				// How much of velocity is self-directed (vs induced)
				// how much of "velocity" is "loco velocity"
				float dot = Vector3.Dot(
					behavtree.Phys.locomotion.X_Z().normalized,
					behavtree.Kinematics.Velocity.X_Z().normalized);

				dot = (dot + 1) / 2; // normalize dot as a measure of similarity
				direction = (dot * behavtree.Phys.locomotion) + ((1 - dot) * behavtree.Transform.forward); // use dot as weight to negate direction change by knockback
				direction = direction.X_Z().normalized;
			}

			behavtree.Control.desiredLerpRotation = Quaternion.LookRotation(
				direction,
				behavtree.Transform.up);
		}

		public override void CustomUpdate2(float dt)
		{
			// TODO : Other type of orientation ?
			behavtree.Control.rotation = Quaternion.Lerp(
				behavtree.Control.rotation,
				behavtree.Control.desiredLerpRotation,
				behavtree.Control.rotationLerpSpeed * dt);
		}
	}
}