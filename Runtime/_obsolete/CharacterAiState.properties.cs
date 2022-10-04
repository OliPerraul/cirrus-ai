//using System;
////using System.Numerics;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	[Serializable]
//	public partial class CharacterAiStateBase
//	{
//		[SerializeField]
//		public float ArrivalTolerance { get; set; } = 0.5f;

//		[SerializeField]
//		public float NavmeshRefreshTime { get; set; } = 2f;

//		[SerializeField]
//		public bool IsReconsidered { get; set; } = false;

//		public float ReconsideredTime { get; set; } = 6f;

//		[SerializeField]
//		public float DestinationRange { get; set; } = 2.0f;

//		protected Coroutine _calculatePathCoroutine;

//		protected Vector3 _sampledPosition = Vector3.zero;

//		protected Vector3 _destination = Vector3.positiveInfinity;

//		protected Timer _reconsideredTimer;
//		public override int ID { get; set; }

//		protected OptionBase _option;
//	}
//}
