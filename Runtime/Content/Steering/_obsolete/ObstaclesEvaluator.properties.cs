//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Cirrus.Broccoli;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public interface IObstaclesContext
//	{
//		float ObstaclesRadius { get; set; }

//		LayerMask ObstaclesLayers { get; set; }

//		public AnimationCurve ObstaclesAvoidanceCurve { get; set; }

//		public Range_ ObstaclesAvoidance { get; set; }
//	}

//	public partial class ObstaclesEvaluator<TContext> : ContextEvaluatorBase<TContext, ISteeringNode>
//	where TContext : AiBehavtree
//	{
//	}
//}
