//namespace Cirrus.Arpg.AI
//{
//	public partial class AiNodeBase
//	{
//		public float ArrivalTolerance => 0;

//		public float NavmeshRefreshTime => 0;

//		public bool IsReconsidered => false;

//		public float ReconsideredTime => 0;

//		public float DestinationRange => 0;

//		//public CharacterAiComponent AiComp => _CharaComp.CharacterAi;

//		////CharacterControlStateMachine IState<CharacterControlStateMachine>.Context { get; set; }

//		//[NonSerialized]
//		//private CharacterComponent _charaComp;

//		//protected virtual CharacterComponent _CharaComp
//		//{
//		//	set => _charaComp = value;
//		//	get =>
//		//	  _charaComp == null ?
//		//	  _charaComp = Context != null ? CharaComp : null :
//		//	  _charaComp;
//		//}

//		//public virtual CharacterControl _control { get; set; }

//		//protected virtual CharacterControl _Control
//		//{
//		//	set => _control = value;
//		//	get =>
//		//		_control == null ?
//		//		_control = Context != null ? Context.Control : null :
//		//		_control;
//		//}

//		public object[] CustomContext;

//		private Timer _timer = null;
//	}
//}