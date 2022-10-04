//using Cirrus.Collections;
//using Cirrus.Unity.Numerics;
//using System;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	// TODO : Eventually we could have mechanism
//	// where multiple steering state could be merged
//	// in order to use option utility in a continuous fashion
//	// This is not the case for now. All sub behaviour in a steering state are statically defined.
//	public partial class SteeringState : AgentStateBase
//	{
//		[field: NonSerialized]
//		public float Time { get; set; } = new Range(5, 10f);

//		// Go close to object and speech with question mark
//		[field: NonSerialized]
//		private Timer _timer;

//		private SteeringComponent _Steering => _AI.Steering;

//		public SteeringContext _context;

//		[SerializeReference]
//		public ContextEvaluatorBase[] ContextEvaluators = ArrayUtils.Empty<ContextEvaluatorBase>();

//		public ContextEvaluatorBase ContextEvaluator
//		{
//			set => ContextEvaluators = value == null ?
//				ArrayUtils.Empty<ContextEvaluatorBase>() :
//				value.ToArray();
//		}

//		[SerializeReference]
//		public SteeringEvaluator[] Evaluators = ArrayUtils.Empty<SteeringEvaluator>();

//		public SteeringEvaluator Evaluator
//		{
//			set => Evaluators = value == null ?
//				ArrayUtils.Empty<SteeringEvaluator>() :
//				value.ToArray();
//		}
//	}
//}