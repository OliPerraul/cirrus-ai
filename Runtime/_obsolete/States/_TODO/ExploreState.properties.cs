//using Cirrus.Unity.Editor;
//using Cirrus.Unity.Numerics;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class ExploreState
//	{
//		public float AlreadyExploredCost { get; set; } = 10f;

//		public float OpenedNeighboursScalar = 2f;

//		public float VisitedNeighboursScalar = -1f;

//		public float NodeWeightScalar { get; set; } = 20f;

//		// TODO Reward if same direction
//		public float SameDirectionScalar { get; set; } = 2f;

//		public float Time { get; set; } = 1f;

//		// Go close to object and speech with question mark
//		//private Timer _timer;

//		//public ICollection<Sprite> Emotes = new List<Sprite>();


//		[SerializeReference]
//		public float DestinationEpsilon = new Range(1, 2.5f);

//		//public Numerical Time { get; set; } = new Range(5f, 10.5f);

//		//public Numerical SpeechTime { get; set; } = new Range(1f, 2.5f);

//		// Go close to object and speech with question mark
//		// TODO make sure that timer isn;t cloned!
//		private Timer _timer;

//		private Chance _chanceRandomPosition = new Chance(0.4f);

//		private Vector3 _speechStartPosition = Vector3.zero;

//		public ExplorationNode CurrentNode => _Agent.Properties.NextNode;

//		public ExplorationGraphComponent ExplorationGraph => ExplorationGraphComponent.Instance;

//		//public float 
//	}
//}
