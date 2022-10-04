//using Cirrus.Collections;
//using Cirrus.Unity.Objects;
//using Cirrus.Arpg.AI;
//using Cirrus.Arpg.Entities.Collectibles;
//using Cirrus.Unity.Objects.Libraries;
//using UnityEngine;
//using Cirrus.Unity.Objects;
//namespace Cirrus.Arpg.Content.AI
//{
//	class Option_Door : ResourceProviderComponent<OptionBase>
//	{
//		private DoorObject _door;
//		private DoorObject _Door =>
//			_door == null ?
//			_door = GetComponent<DoorObject>() :
//			_door;

//		// TODO factor in cost for searching key

//		[SerializeField]
//		private float _enterUtility = 1f;

//		[SerializeField]
//		private KeyFlags _keyFlags;

//		protected override OptionBase _Create()
//		{
//			return _Door.IsClosed ?
//				(_Door.IsLocked ?
//					new SequenceOption
//					{
//						Utility = _enterUtility,
//						Options = 
//							new Option
//							{
//								//State = new SearchState
//								//{
//								//	ItemFlags = (ItemFlags)_keyFlags
//								//}

//							}.ToEnumerable()
//					} :
//					new SequenceOption
//					{

//					}).As<Option>() :
//				new Option
//				{
//					State = new InteractState(),
//					Utility = _enterUtility
//				};
//		}
//	}
//}
