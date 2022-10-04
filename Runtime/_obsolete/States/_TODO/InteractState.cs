//using Cirrus.States;
//using Cirrus.Unity.Numerics;

//namespace Cirrus.Arpg.AI
//{
//	public class InteractState : CharacterAiStateBase
//	{


//		public float Time { get; set; } = new Range(1f, 2.5f);

//		// Go close to object and speech with question mark
//		private Timer _timer;

//		public InteractState() : base()
//		{ 
//			_timer = new Timer(OnTimeout);			
//		}

//		public override StateMachineStatus Enter()
//		{
//			base.Enter();

//			_timer.Reset(Time, true);

//			return StateMachineStatus.Success;
//		}

//		public void OnTimeout()
//		{ 

//		}

//	}
//}
