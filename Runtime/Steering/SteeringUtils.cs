using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public static class SteeringUtils
	{
		/// <summary>
		/// Returns the given orientation (in radians) as a unit vector
		/// </summary>
		/// <param name="radians">the orientation in radians</param>
		/// <param name="is3DGameObj">is the orientation for a 3D game object or a 2D game object</param>
		/// <returns></returns>
		public static Vector3 OrientationToVector(this float radians)
		{
			/* Mulitply the orientation by -1 because counter clockwise on the y-axis is in the negative
				* direction, but Cos And Sin expect clockwise orientation to be the positive direction */
			return new Vector3(Mathf.Cos(-radians), 0, Mathf.Sin(-radians));
		}

		public static Vector3 OrientationToVector(this Quaternion orientation)
		{
			float radians = orientation.RotationInRadians();

			/* Mulitply the orientation by -1 because counter clockwise on the y-axis is in the negative
				* direction, but Cos And Sin expect clockwise orientation to be the positive direction */
			return new Vector3(Mathf.Cos(-radians), 0, Mathf.Sin(-radians));
		}

		/// <summary>
		/// Gets the orientation of a vector as radians. For 3D it gives the orienation around the Y axis.
		/// For 2D it gaves the orienation around the Z axis.
		/// </summary>
		/// <param name="direction">the direction vector</param>
		/// <param name="is3DGameObj">is the direction vector for a 3D game object or a 2D game object</param>
		/// <returns>orientation in radians</returns>
		public static float VectorToOrientation(Vector3 direction)
		{
			/* Mulitply by -1 because counter clockwise on the y-axis is in the negative direction */
			return -1 * Mathf.Atan2(direction.z, direction.x);
		}

		/// <summary>
		/// Creates a debug cross at the given position in the scene view to help with debugging.
		/// </summary>
		public static void DebugCross(Vector3 position, float size = 0.5f, Color color = default(Color), float duration = 0f, bool depthTest = true)
		{
			Vector3 xStart = position + Vector3.right * size * 0.5f;
			Vector3 xEnd = position - Vector3.right * size * 0.5f;

			Vector3 yStart = position + Vector3.up * size * 0.5f;
			Vector3 yEnd = position - Vector3.up * size * 0.5f;

			Vector3 zStart = position + Vector3.forward * size * 0.5f;
			Vector3 zEnd = position - Vector3.forward * size * 0.5f;

			Debug.DrawLine(xStart, xEnd, color, duration, depthTest);
			Debug.DrawLine(yStart, yEnd, color, duration, depthTest);
			Debug.DrawLine(zStart, zEnd, color, duration, depthTest);
		}

		public static float RotationInRadians(this Quaternion rotation)
		{
			return rotation.eulerAngles.y * Mathf.Deg2Rad;
		}

		public static int Clamp(this int value, int inclusiveMin,
			int exclusiveMax)
		{
			if(inclusiveMin == exclusiveMax)
			{
				return inclusiveMin;
			}
			else if(value < inclusiveMin)
			{
				return inclusiveMin;
			}
			else if(value >= exclusiveMax)
			{
				return exclusiveMax - 1;
			}
			else
			{
				return value;
			}
		}

		public static int Min(this int x, int y)
		{
			return (x > y) ? y : x;
		}

		public static int Max(this int x, int y)
		{
			return (x > y) ? x : y;
		}

		/// <summary>
		/// Projects the given vector onto this vector.
		/// </summary>
		/// <param name="target">The vector that will be projected onto.</param>
		/// <param name="source">The vector that will be projected onto this vector.</param>
		/// <returns>A projected vector.</returns>
		public static Vector2 Project(this Vector2 target, Vector2 source)
		{
			return target * source.magnitude * Vector2.Angle(target, source);
		}

		public static Vector2 Rotate(this Vector2 v, float delta)
		{
			return new Vector2(
				v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
				v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
			);
		}

		public static Vector2 MinClamp(this Vector2 v, float floor)
		{
			return v.SmallerThan(floor) ? v.normalized * floor : v;
		}

		public static Vector2 ClampMagnitude(this Vector2 v, float floor = float.MinValue, float ceiling = float.MaxValue)
		{
			var sqMag = v.sqrMagnitude;
			floor *= floor;
			ceiling *= ceiling;
			if(sqMag < floor)
			{
				return v.normalized * floor;
			}
			if(sqMag > ceiling)
			{
				return v.normalized * ceiling;
			}
			return v;
		}

		public static bool IsZero(this Vector2 vector) => vector == Vector2.zero;

		public static Vector2 Position2D(this Transform transform) => transform.position;

		public static Vector2 Position2D(this MonoBehaviour behavior) => behavior.transform.position;

		/// <summary>
		/// Checks to see if two points are further apart than a given distance, but in an efficient way.
		/// </summary>
		/// <param name="here">Start of distance to compare.</param>
		/// <param name="there">End of distance to compare.</param>
		/// <param name="compareDistance">How long the distance between the two vectors should be.</param>
		/// <returns>A float. Negative if distance is smaller than the given, 0 if equal, and positive if larger.</returns>
		public static float FastDistanceCheck(this Vector2 here, Vector2 there, float compareDistance)
		{
			return FastDistanceCheck(there - here, compareDistance);
		}

		/// <summary>
		/// Checks to see if two points are further apart than a given distance, but in an efficient way.
		/// </summary>
		/// <param name="distanceVector">Vector representing the distance between two points.</param>
		/// <param name="compareDistance">How long the distance between the two vectors should be.</param>
		/// <returns>A float. Negative if distance is smaller than the given, 0 if equal, and positive if larger.</returns>
		public static float FastDistanceCheck(this Vector2 distanceVector, float compareDistance)
		{
			return distanceVector.sqrMagnitude - Mathf.Pow(compareDistance, 2);
		}

		public static bool LargerThan(this Vector2 distanceVector, float compareDistance)
		{
			return FastDistanceCheck(distanceVector, compareDistance) > 0;
		}

		/// <summary>
		/// Checks if two Vector2 positions are farther than the given distance.
		/// </summary>
		/// <param name="here">First position.</param>
		/// <param name="there">Second position.</param>
		/// <param name="compareDistance">Distance to compare.</param>
		/// <returns>True if the positions are farther apart than the given distance. False otherwise.</returns>
		public static bool FartherThan(this Vector2 here, Vector2 there, float compareDistance)
		{
			return FastDistanceCheck(here, there, compareDistance) > 0;
		}

		public static bool SmallerThan(this Vector2 distanceVector, float compareDistance)
		{
			return FastDistanceCheck(distanceVector, compareDistance) < 0;
		}

		/// <summary>
		/// Checks if two Vector2 positions are closer than the given distance.
		/// </summary>
		/// <param name="here">First position.</param>
		/// <param name="there">Second position.</param>
		/// <param name="compareDistance">Distance to compare.</param>
		/// <returns>True if the positions are closer together than the given distance. False otherwise.</returns>
		public static bool CloserThan(this Vector2 here, Vector2 there, float compareDistance)
		{
			return FastDistanceCheck(here, there, compareDistance) < 0;
		}

		public static bool CloserThan(this MonoBehaviour here, Vector2 there, float compareDistance)
		{
			return FastDistanceCheck(here.transform.position, there, compareDistance) < 0;
		}

		public static bool FartherThan(this MonoBehaviour here, Vector2 there, float compareDistance)
		{
			return FastDistanceCheck(here.transform.position, there, compareDistance) > 0;
		}
	}

	public static class SteeringBehaviourUtils
	{
	}
}