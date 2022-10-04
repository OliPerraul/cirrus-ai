//using Cirrus.Arpg.AI;
//using Cirrus.Unity.Numerics;
//using Cirrus.Unity.Objects;
//using UnityEngine;
//namespace Cirrus.Arpg.Content.AI
//{
//	public class Option_OnGroupFormed : ResourceProviderComponent<OptionBase>
//	{
//		[SerializeField]
//		private Sprite[] _emotes;

//		[SerializeField]
//		private Range_ _speakingTime = 2f;

//		[SerializeField]
//		private Range_ _speakingDistance = 2f;


//		//protected override OptionBase[] _CreateMany()
//		//{
//		//	//Option speechOption = _SpeechOption.Clone<Option>();
//		//	var speechOption = new Option
//		//	{
//		//		AppraisalFlags = AppraisalFlags.GroupMember,
//		//		Name = "DEBUG_OnGroupFormed_Speech",
//		//		//State = new SpeechState
//		//		//{
//		//		//	Emotes = _emotes,
//		//		//	Time = _speakingTime,
//		//		//	Range = _speakingDistance
//		//		//}
//		//	};

//		//	return new OptionBase[]
//		//	{
//		//		new SequenceOption
//		//		{
//		//			Name = "OnGroupFormed_Sequence",
//		//			Options = new Option[]
//		//			{
//		//				speechOption,
//		//				//new Option
//		//				//{
//		//				//	Name = "OnGroupFormed_Follow",
//		//				//	State =  new FollowState
//		//				//	{
//		//				//	}
//		//				//}
//		//			}
//		//		},
//		//		new SequenceOption
//		//		{
//		//			Name = "OnGroupFormed_Sequence_Leader",
//		//			Options = new Option[]
//		//			{
//		//				speechOption,
//		//				new Option
//		//				{
//		//					Name = "OnGroupFormed_Explore",
//		//					State =  new ExploreState
//		//					{
//		//						ArrivalTolerance = 2,
//		//						DestinationEpsilon = 1.5
//		//					}
//		//				}
//		//			}
//		//		},
//		//	};
//		//}
//	}
//}
