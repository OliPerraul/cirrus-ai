using Cirrus.Numerics;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	// This is to support multiple LODs

	//VeryLow = 4,
	//Low = 8,
	//Medium = 16,
	//High = 32,
	//VeryHigh = 64
	public static class ContextSteeringUtils
	{
		public const int ContextMapPropagation = 8;

		//public const int SignificantNeighbourDirections = 2;

		public static ContextMap Combine(
			this ContextMap self,
			ContextMap other,
			float decay)
		{
			for(int i = 0; i < self.Count; i++)
			{
				if(
					self[i] < other[i] &&
					other[i] * decay > self[i]
					)
				{
					self[i] = other[i] * decay;
				}
			}

			return self;
		}

		public static ContextMap Combine(
			this ContextMap self,
			ContextMap other
			)
		{
			for(int i = 0; i < self.Count; i++)
			{
				if(self[i] < other[i])
				{
					self[i] = other[i];
				}
			}

			return self;
		}

		public static SteeringContext Combine(
			this SteeringContext self,
			SteeringContext other
			)
		{
			self.interests.Combine(other.interests);
			self.avoidances.Combine(other.avoidances);
			return self;
		}

		public static SteeringContext Combine(
			this SteeringContext self,
			SteeringContext other,
			float decay
			)
		{
			self.interests.Combine(other.interests, decay);
			self.avoidances.Combine(other.avoidances, decay);
			return self;
		}


		// NOTE: Assume moving at a constant speed
		// TODO speed should depend on context, but does not at the moment
		// The context map simply indicate how good the decision is relative to others
		// not how fast you should go.

		/// <summary>
		/// Compute the direction and account for neighbours to smooth the direction
		/// </summary>
		/// <param name="context"></param>
		/// <param name="significant">number of significant neighbour to account for direction smoothing</param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		public static bool ComputeVelocity(
			this SteeringContext context,
			out Vector3 velocity)
		{
			velocity = Vector3.zero;			
			for (int i = 0; i < context.Count; i++)
			{
				velocity += (context.interests[i] - context.avoidances[i]) * context.directions[i];
			}
			
			velocity /= context.directions.Count;

			return true;
		}

		//public static void UpdateSteering(this SteeringContext context, SteeringComponent steering)
		//{
		//}
	}
}