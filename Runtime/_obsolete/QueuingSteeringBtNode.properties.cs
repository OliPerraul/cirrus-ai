////// using Cirrus.Unity.AI.BehaviourTrees;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Unity.Objects;
//using Cirrus.Arpg.Entities.Characters;
//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using System.Linq;

//using System;
//using System.Collections.Generic;

//using UnityEngine;
//using UnityEngine.SocialPlatforms;

//using static Cirrus.FuncGenerics;
////using static Cirrus.Objects.PrototypeUtils;
//using static Cirrus.Debugging.DebugUtils;

//using Range = Cirrus.Unity.Numerics.Range_;
//using System.Security.Cryptography;
//using UnityEngine.UIElements;
//using Cirrus.Collections;
//using None = Cirrus.Objects.None;

//namespace Cirrus.Arpg.Content
//{
//	//public class QueingEvaluator : ContextEvaluator<QueingNode>
//	//{
//	//	// tries to create a less "robotic" result by canceling all active steering forces using a brake force:
//	//	public Vector3 GetQueuingForce(ControlBtComponent context, QueingEvaluator eval)
//	//	{
//	//		Vector3 velocity = context.LocomotionVelocity;
//	//		Vector3 brake = new Vector3();
//	//		CharacterComponent neighbour = GetNeighborAhead(context);

//	//		if(neighbour != null)
//	//		{
//	//			//brake.x = -steering.x * 0.8;
//	//			//brake.y = -steering.y * 0.8;
//	//			int forward = eval.SteeringContext.Resolution.GetDirection(context.Forward);
//	//			//eval.SteeringContext.Avoidances[]

//	//			//v.scaleBy(-1);
//	//			brake = brake.add(v);
//	//			brake = brake.add(separation());

//	//			if(distance(position, neighbour.Position) <= MaxQueueRadius)
//	//			{
//	//				velocity.scaleBy(0.3);
//	//			}
//	//		}

//	//		return brake;
//	//	}

//	//	public CharacterComponent GetNeighborAhead(ControlBtComponent context)
//	//	{
//	//		CharacterComponent ret = null;
//	//		Vector3 velocity = context.LocomotionVelocity;
//	//		Vector3 qa = velocity.normalized * MaxQueueAhead;
//	//		Vector3 ahead = qa + context.Position;
//	//		for(int i = 0; i < Neighbours.Count; i++)
//	//		{
//	//			var neighbour = Neighbours[i];
//	//			if(neighbour == context) continue;
//	//			float d = (ahead - neighbour.Position).magnitude;
//	//			if(d <= MaxQueueRadius)
//	//			{
//	//				ret = Neighbours[i];
//	//				break;
//	//			}
//	//		}

//	//		return ret;
//	//	}

//	//	public void Seek(ControlBtComponent context)
//	//	{
//	//		_desired = (Door.Position - context.Position).normalized;
//	//		_desired = MaxVelocity * _desired;
//	//		// return force
//	//		_steering = _desired - context.LocomotionVelocity;
//	//	}

//	//	public override bool Update(ControlBtComponent context, SteeringAiNode<QueingNode> state)
//	//	{
//	//		return base.Update(context, state);
//	//	}

//	//	public override void Start(ControlBtComponent context, SteeringAiNode<QueingNode> node)
//	//	{
			
//	//	}
//	//}

//	public partial class QueuingSteeringNode : SteeringNodePhase1<None>
//	{
//		public float MaxSeeAhead = 32.0f;
//		public float MaxAvoidAhead = 50f;
//		public float AvoidForce = 600.0f;

//		public float MaxAvoidForce = 1.0f;
//		public float ObjRadius = 20.0f;

//		public float MaxQueueAhead = 16.0f;
//		public float MaxQueueRadius = 16.0f;
//		public float AvoidDist = 16.0f;
//		public float SepDist = 32.0f;
//		public float CohesionDist = 32.0f;
//		public float AlignDist = 32.0f;

//		public float MaxVelocity = 6;
//		public float RaycastDist = 10f;

//		public Vector3 _desired;
//		public Vector3 _steering;
//		private Vector3 _ahead;
//		private Vector3 _avoidance;
//		public List<CharacterComponent> Neighbours;
//		public List<ObjectComponentBase> Obstacles;
//		public ObjectComponentBase Door;

//		public int ObstaclesLayers = Layers.EntityFlags;
//	}
//}
