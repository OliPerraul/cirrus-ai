
using Cirrus.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public struct ContextMapVelocity
	{
		public int Direction;
		public float Weight;
	}

	public partial class ContextMap : IEnumerable<float>
	{
		//public Color Color { get; }


		//public float factor = 1.0f;

		//private readonly DiscreteDirections _resolution;

		//public DiscreteDirections Resolution => _resolution;

		// Directions map the direction and the intensity
		// TODO : Ideally this is a vector of floats
		// the directions are constant
		//public readonly Vector3[] Directions;

		public float[] _weights;

		public int Count => _weights.Length;

		public void Multiply(float factor)
		{
			for(int i = 0; i < _weights.Length; i++)
			{
				_weights[i] = factor * _weights[i];
			}
		}

		public float this[int index]
		{
			get
			{
				return _weights[index.Mod(_weights.Length)];
			}
			set
			{
				_weights[index.Mod(_weights.Length)] = value;
			}
		}

		public float Total
		{
			get
			{
				float total = 0;
				for (int i = 0; i < Count; i++) total += this[i];
				return total;
			}
		}

		public IEnumerator<float> GetEnumerator()
		{
			return ((IEnumerable<float>)_weights).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _weights.GetEnumerator();
		}
	}
}