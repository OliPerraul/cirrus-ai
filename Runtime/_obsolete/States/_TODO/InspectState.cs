//using Cirrus.States;
//using Cirrus.Unity.Numerics;

//namespace Cirrus.Arpg.AI
//{
//	public class InspectState : CharacterAiStateBase
//	{
//		public static readonly Range DefaultInspectionTime = new Range(1f, 2.5f);
//		public float Time { get; set; } = DefaultInspectionTime;

//		// Go close to object and speech with question mark
//		private Timer _timer;

//		public InspectState() : base()
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
