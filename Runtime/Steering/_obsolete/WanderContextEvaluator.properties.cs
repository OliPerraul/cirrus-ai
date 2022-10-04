using Cirrus.Unity.Numerics;
using System;
using UnityEngine;

using Range_ = Cirrus.Unity.Numerics.Range_;

namespace Cirrus.Arpg.Content
{
	[Serializable]
	public partial class WanderContextEvaluator
	{
		// TODO: perlin noise for smoother values?

		[SerializeField]

		public float CircleRadius = 4f;


		public float _wanderAngle = 2f;


		public float AngleChange = 0.5f;

		private static readonly Range_ _range = new Range_(-1f, 1f);
	}
}