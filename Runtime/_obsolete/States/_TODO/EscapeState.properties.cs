//using Cirrus.Arpg.Entities.Characters.Controls;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{

//	public partial class EscapeState
//	{
//		//public override int Id => (int)Controls.Id.Action;
//		[SerializeField]
//		public float MinSafeTime { get; set; } = 5f;

//		[SerializeField]
//		public float SafeDistance { get; set; } = 10f;

//		[SerializeField]
//		public float RefreshRate { get; set; } = 5f;

//		[SerializeField]
//		public float TargetAreaCost { get; set; } = 10f;

//		[SerializeField]
//		public float SafeTime { get; set; } = 0;

//		private bool _destSet = false;

//		public EscapeState(CharacterControlStateMachine machine) : base(machine)
//		{
//		}
//	}
//}