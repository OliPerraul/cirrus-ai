//using Cirrus.States;
//using Cirrus.Arpg.Entities.Collectibles;
//using Cirrus.Unity.Numerics;

//namespace Cirrus.Arpg.AI
//{
//	public class SearchState : UtilityAgentState
//	{
//		public float Time { get; set; } = 1f;

//		// Go close to object and speech with question mark
//		private Timer _timer;

//		public int EntityFlags { get; set; }

//		public ItemFlags ItemFlags { get; set; }

//		public SearchState() : base()
//		{ 
//			_timer = new Timer(OnTimeout);
//		}

//		public override StateMachineStatus Enter(params object[] args)
//		{
//			base.Enter(args);

//			_timer.Reset(Time, true);

//			return StateMachineStatus.Success;
//		}

//		public void OnTimeout()
//		{ 

//		}

//	}
//}
