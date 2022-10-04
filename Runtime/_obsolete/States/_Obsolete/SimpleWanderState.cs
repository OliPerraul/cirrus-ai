////using Cirrus.Arpg.World.Entities.Actions;
//using Cirrus.States;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using Pathfinding;
//using System;
//using System.Collections;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	/// <summary>
//	/// Moves an object along a spline.
//	/// Helper script in the example scene called 'Moving'.
//	/// </summary>	
//	public class BezierMover : MonoBehaviour
//	{
//		public Vector3[] points;

//		public BezierMover(Vector3[] points)
//		{ 
//		}

//		public float speed = 1;
//		public float tiltAmount = 1f;

//		float time = 0;

//		Vector3 Position(float t)
//		{
//			int c = points.Length;
//			int pt = Mathf.FloorToInt(t) % c;			

//		}

//		/// <summary>Update is called once per frame</summary>
//		public void Update()
//		{
//			float mn = time;
//			float mx = time + 1;

//			while(mx - mn > 0.0001f)
//			{
//				float mid = (mn + mx) / 2;

//				Vector3 p = Position(mid);
//				if((p - transform.position).sqrMagnitude > (speed * Time.deltaTime) * (speed * Time.deltaTime))
//				{
//					mx = mid;
//				}
//				else
//				{
//					mn = mid;
//				}
//			}

//			time = (mn + mx) / 2;

//			const float dt = 0.001f;
//			const float dt2 = 0.15f;
//			Vector3 p1 = Position(time);
//			Vector3 p2 = Position(time + dt);
//			transform.position = p1;

//			Vector3 p3 = Position(time + dt2);
//			Vector3 p4 = Position(time + dt2 + dt);

//			// Estimate the acceleration at the current point and use it to tilt the object inwards on the curve
//			var acceleration = ((p4 - p3).normalized - (p2 - p1).normalized) / (p3 - p1).magnitude;
//			var up = new Vector3(0, 1 / (tiltAmount + 0.00001f), 0) + acceleration;
//			transform.rotation = Quaternion.LookRotation(p2 - p1, up);
//		}

//		void OnDrawGizmos()
//		{
//			if(points.Length >= 3)
//			{
//				for(int i = 0; i < points.Length; i++) if(points[i] == null) return;

//				Gizmos.color = Color.white;
//				Vector3 pp = Position(0);
//				for(int pt = 0; pt < points.Length; pt++)
//				{
//					for(int i = 1; i <= 100; i++)
//					{
//						var p = Position(pt + (i / 100f));
//						Gizmos.DrawLine(pp, p);
//						pp = p;
//					}
//				}
//			}
//		}
//	}



//	[Serializable]
//	public partial class SimpleWanderState : UtilityAgentState
//	{
//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while (true)
//			{
//				if (NavMeshUtils.GetRandomPointAround(
//					_center, 
//					Range.Min, 
//					Range.Max, 
//					out _destination))
//				{
//					_Agent.NavMeshAgent.SetDestination(_destination);
//				}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}

//		public override void OnDrawGizmos_()
//		{
//			base.OnDrawGizmos_();

//			Gizmos.color = Color.yellow;
//			Gizmos.DrawSphere(_destination, .2f);

//		}

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			base.Enter(args);

//			_destination = Vector3.positiveInfinity;
//			StopCalculatingPath();
//			StartCalculatingPath();
//			_center = FromStart ? _Character.StartPosition : _Character.Position;

//			return StateMachineStatus.Success;
//		}

//		//public override void Reenter(params object[] args)
//		//{
//		//	base.Reenter(args);

//		//	_destination = Vector3.positiveInfinity;
//		//	StopCalculatingPath();
//		//	StartCalculatingPath();

//		//	_center = _resource.IsFromStartPosition ? Character.StartPosition : Character.Transform.position;
//		//}

//		public override StateMachineStatus Exit()
//		{
//			StopCalculatingPath();

//			_Control.Axes.Left = Vector2.zero;
//			base.Exit();
//			return StateMachineStatus.Exited;
//		}


//		public override void Update_()
//		{
//			base.Update_();

//			if (_Character.Position.IsApproximately(
//				_destination,
//				ArrivalTolerance))
//			{
//				_Control.DesiredAxes.Left = Vector2.zero;
//				_Control.Axes.Left /= 2;

//				_Agent.NavMeshAgent.transform.position = _Character.Position;
//				_Agent.NavMeshAgent.velocity = _Character.Kinematics.Motor.Velocity;

//				//Controller.StateMachine.TrySetState(StateID.Idle);
//			}
//			else
//			{
//				UpdateMovement();
//			}
//		}
//	}
//}
