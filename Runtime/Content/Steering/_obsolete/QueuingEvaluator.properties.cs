//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//// using Cirrus.Unity.AI.BehaviourTrees;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Objects;

//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public interface IQueuingData
//	{
//		//List<CharacterComponent> Neighbours { get; set; }

//		float MaxSeeAhead { get; set; }
//		float MaxAvoidAhead { get; set; }
//		float AvoidForce { get; set; }

//		float MaxAvoidForce { get; set; }
//		float ObjRadius { get; set; }

//		float MaxQueueAhead { get; set; }
//		float MaxQueueRadius { get; set; }
		
//		float AvoidDist { get; set; }

//		float SepDist { get; set; }
//		float CohesionDist { get; set; }
//		float AlignDist { get; set; }

//		float SeekMaxVelocity { get; set; }
//		float RaycastDist { get; set; }
//		float SeparationDist { get; set; }
//}

//	public partial class QueuingEvaluator<TData> 
//		: ContextEvaluatorBase<TData>
//		where TData : IQueuingData
//	{
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
