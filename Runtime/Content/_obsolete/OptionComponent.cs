//using Cirrus.Unity.Objects;
//using Cirrus.Arpg.AI;
//using Cirrus.Objects;
//using Cirrus.Unity.Objects;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.Content.AI
//{
//	public class OptionComponent : ProviderComponent<OptionBase>
//	{
//		//[SerializeField]
//		public AILibrary.ID _prototype = AILibrary.ID.Unknown;

//		public OptionBase Prototype => AILibrary.Instance.Get<OptionBase>((int)_prototype);

//		[SerializeField]
//		private List<ObjectBase> _staticTargets;

//		[SerializeReference]
//		public float _utility = 1f;

//		[SerializeReference]
//		public float _targetUtility = 1f;

//		protected override OptionBase _Create()
//		{
//			return Prototype.Clone(inst =>
//			{
//				inst.StaticTargets = _staticTargets.Select(x => new KnowledgeItem(x.Entity));
//				inst.Utility = _utility;
//				inst.TargetUtility = _targetUtility;
//			});
//		}
//	}
//}
