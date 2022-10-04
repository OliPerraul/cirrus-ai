using Cirrus.Arpg.AI;
using Cirrus.Arpg.AI;
using Cirrus.Unity.Editor;

using NaughtyAttributes;

using Pathfinding.RVO;
//using Pathfinding.RVO.Sampled;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public partial class Ai : ILocalAvoidance
    {
		[SerializeField]
		[SerializeEmbedded]
		public SubtreeNodeBase behavtree;

		[SerializeField]
		[SerializeEmbedded]
		public LocalAvoidance localAvoidance;

		public float LocalAvoidancePriority => localAvoidance.LocalAvoidancePriority;
		public LocalAvoidancePriority[] LocalAvoidancePriorities => localAvoidance.LocalAvoidancePriorities;
		public float LocalAvoidanceRadius => localAvoidance.LocalAvoidanceRadius;
		public float LocalAvoidanceHeight => localAvoidance.LocalAvoidanceHeight;
		public float LocalAvoidanceMaxSpeed => localAvoidance.LocalAvoidanceMaxSpeed;
		public float LocalAvoidanceCenter => localAvoidance.LocalAvoidanceCenter;
		public float LocalAvoidanceTimeHorizon => localAvoidance.LocalAvoidanceTimeHorizon;
		public float LocalAvoidanceObstacleTimeHorizon => localAvoidance.LocalAvoidanceObstacleTimeHorizon;
		public int LocalAvoidanceMaxNeighbours => localAvoidance.LocalAvoidanceMaxNeighbours;
		public LocalAvoidanceLayers LocalAvoidanceLayer => localAvoidance.LocalAvoidanceLayer;
		public LocalAvoidanceLayers LocalAvoidanceCollidesWith => localAvoidance.LocalAvoidanceCollidesWith;
		public bool LocalAvoidanceLocked => localAvoidance.LocalAvoidanceLocked;
		public bool LocalAvoidanceLockedWhenNotMoving => localAvoidance.LocalAvoidanceLockedWhenNotMoving;		
	}
}
