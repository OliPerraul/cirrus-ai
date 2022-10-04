using Cirrus.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public class SteeringContext
	{
		//public float factor = 1.0f;

		//public float Factor
		//{
		//	set {
		//		interests.factor = value;
		//		avoidances.factor = value;
		//	}
		//}

		public ContextMap interests;

		public ContextMap avoidances;

		public void Multiply(float factor)
		{
			interests.Multiply(factor);
			avoidances.Multiply(factor);
		}


		[NonSerialized]
		public DiscreteDirections directions;

		// TODO this should not be dynamic
		public int Count => directions.Count;

		public Vector3 Direction(int i) => directions[i];

		public float Interest(int i) => interests[i];

		public float Interest(int i, float val) => interests[i] = val;

		public float Avoidance(int i) => avoidances[i];

		public float Avoidance(int i, float val) => avoidances[i] = val;


		public SteeringContext(DiscreteDirections r1, DiscreteDirections r2)
		{
			interests = new ContextMap(r1);
			avoidances = new ContextMap(r2);
		}

		public SteeringContext(DiscreteDirections res)
		{
			directions = res;
			interests = new ContextMap(res);
			avoidances = new ContextMap(res);
		}

		public SteeringContext(SteeringContext other)
		{
			directions = other.directions;
			interests = new ContextMap(other.interests);
			avoidances = new ContextMap(other.avoidances);
		}

		public void Clear()
		{			
			interests.Clear();
			avoidances.Clear();
		}
	}

	public partial class ContextMap : IEnumerable<float>, IList<float>
	{
		public bool IsReadOnly => ((ICollection<float>)_weights).IsReadOnly;

		public ContextMap(DiscreteDirections resolution)
		{
			//_resolution = resolution;
			_weights = new float[resolution.Count];
		}

		public ContextMap(ContextMap map)
		{
			//factor = map.factor;
			_weights = new float[map.Count];
			for(int i = 0; i < map._weights.Length; i++)
			{
				_weights[i] = map._weights[i];
			}
		}

		public void UniformMap(int passes)
		{
			for(int n = 0; n < passes; n++)
			{
				for(int i = 0; i < this.Count; i++)
				{
					this[i] = this[i - 1] + this[i + 1] / 2;
				}
			}
		}

		public void InsertValue(int index, float value, int propagation)
		{
			float newDirection = Mathf.Clamp(value, 0, 1f);

			// Place the value exactly to the designated slot
			if(newDirection > this[index]) this[index] = newDirection;

			for(int i = 1; i < propagation; i++)
			// Propagate value to neighbouring slots if the resolution allows it
			{
				float newValue = Mathf.Clamp(value / (1 + i), 0f, 1f);

				float newDirectionL = Mathf.Abs((index - i) % this.Count) * newValue;
				float newDirectionR = Mathf.Abs((index + i) % this.Count) * newValue;

				if(newDirectionL > this[Mathf.Abs((index - i) % _weights.Length)])
				{
					this[Mathf.Abs((index - i) % this.Count)] = newDirectionL;
				}

				if(newDirectionR > this[Mathf.Abs((index + i) % this.Count)])
				{
					this[Mathf.Abs((index + i) % this.Count)] = newDirectionR;
				}
			}
		}

		public void Blend(ContextMap b)
		{
			for(int i = 0; i < Count; i++)
			{
				this[i] = (this[i] + b[i]) / 2;
			}
		}

		public void Clear()
		{
			//factor = 1;
			for(int i = 0; i < _weights.Length; i++) _weights[i] = 0;
		}

		public void DebugMap(Vector3 startPosition)
		{
			//foreach(Vector3 t in _weights)
			//{
			//	Debug.DrawRay(startPosition, t * 10f, Color);
			//}
		}

		public int IndexOf(float item)
		{
			return Array.IndexOf(_weights, item);
		}

		public void Insert(int index, float item)
		{
			_weights[index] = item;
		}

		public void RemoveAt(int index)
		{
		}

		public void Add(float item)
		{
		}

		public bool Contains(float item)
		{
			return false;
		}

		public void CopyTo(float[] array, int arrayIndex)
		{
			((ICollection<float>)_weights).CopyTo(array, arrayIndex);
		}

		public bool Remove(float item)
		{
			return false;
		}
	}
}