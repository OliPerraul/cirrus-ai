//using Cirrus.Collections;
//using Cirrus.Arpg.Abilities;
//using Cirrus.Arpg.Conditions;
////using Cirrus.Arpg.Content.Abilities;
//using Cirrus.Arpg.Content.AI;
//using Cirrus.Arpg.Content;
//using Cirrus.Arpg.AI;
//using Cirrus.Objects;
//using Cirrus.Unity.Numerics;
//using System.Collections.Generic;
//using static Cirrus.Objects.LazyUtils;
//using static Cirrus.Objects.PrototypeUtils;

//namespace Cirrus.Arpg.Content
//{
//	public static partial class AI
//	{
//		public static Lazily<Option> Option_Idle =
//			Func((() => Prepare<Option>(
//			option =>
//			{
//				option.Name = nameof(Option_Idle);
//				option.IsSpontaneous = true;
//			},
//			option =>
//			{
//				option.State = new IdleAiState
//				{
//					IsReconsidered = true,
//					ReconsideredTime = new Range(2, 6)
//				};
//			}));

// public static Lazily<Option> Option_Attack_AttackBase = Func((() => Prepare<Option>( option => {
// // TODO add attack tags option.Name = nameof(Option_Attack_AttackBase); option.Tag =
// Token_Option_Attack; }, option => { option.Acces = new OptionAccess { AdmissionCapacity = 1,
// AdmissionUtility = -1 }; option.FailedAccessState = new ConcurrentUtilityAgentState { States =
// new IAiState[] { new SteeringAiNode { ContextEvaluator = new TargetAbilityContextEvaluator { }
// } } }; }));

// public static Lazily<Option> Option_SpontaneousBase = Func((() => Prepare<Option>(option => {
// option.Name = nameof(Option_SpontaneousBase); option.IsSpontaneous = true; }));

// public static Lazily<Option> Option_Attack_Tackle = Func((() =>
// Option_Attack_AttackBase.Prepare( option => { option.Name = nameof(Option_Attack_Tackle); },
// option => { var ability = ItemLibrary.Equip_BeginnerSword.Resource as IConcreteActiveAbility;
// option.AppraisalFlags = AppraisalFlags.Hostile; option.RequiredTargets = 1; option.Utility = 0;
// option.TargetConsiderations = new List<ConsiderationBase> { new DistanceConsideration { Name =
// "Distance01", Utility = 2, InBound = DistanceConsideration.InBoundMode.UniformUtility, } }; }));

// public static Lazily<Option> Option_Attack_Weapon = Func((() =>
// Option_Attack_AttackBase.Prepare( option => { option.Name = nameof(Option_Attack_Weapon);
// option.AppraisalFlags = AppraisalFlags.Hostile; }, option => { var ability =
// ItemLibrary.Equip_BeginnerSword.Resource.As<IConcreteActiveAbility>(); option.RequiredTargets =
// 1; option.Utility = 0; option.TargetConsiderations = new List<ConsiderationBase> { new
// DistanceConsideration { Name = "Distance01", Utility = 2, //DistanceRange = new
// Range(ability.Range.Min, ability.Range.Max), InBound =
// DistanceConsideration.InBoundMode.UniformUtility, } }; }));

// public static Lazily<Option> Option_Attack_Weapon01 = Func((() =>
// Option_Attack_Weapon.Prepare(option => { option.Utility = 10; }));

// public static Lazily<Option> Option_Attack_Weapon02 = Func((() => Option_Attack_Weapon.Prepare(
// option => { option.Utility = 10; option.Name = nameof(Option_Attack_Weapon02); }, option => {
// option.State = new ConcurrentUtilityAgentState { States = new IAiState[] { new SteeringAiNode {
// ContextEvaluator = new TargetAbilityContextEvaluator { } } } }; }));

// public static Lazily<Option> Option_Attack_Weapon03 = Func((() => Option_Attack_Weapon.Prepare(
// option => { option.Utility = 10; option.Name = nameof(Option_Attack_Weapon03); }, option => {
// option.Ability = ItemLibrary.Equip_BeginnerSword.Resource as IConcreteActiveAbility; option.State
// = new ConcurrentUtilityAgentState { States = new IAiState[] { new AbilityAiNode { }, new
// SteeringAiNode { ContextEvaluator = new TargetAbilityContextEvaluator { }, PostprocessEvaluator
// = new AbilityArrivalEvaluator { } } } }; }));

// public static Lazily<Option> Option_Attack_Attack01 = Func((() =>
// Option_Attack_AttackBase.Prepare( option => { option.Name = nameof(Option_Attack_Tackle01);
// option.AppraisalFlags = AppraisalFlags.Hostile; option.RequiredTargets = 1; option.Utility = 0;
// }, option => { option.Utility = 10; option.Name = nameof(Option_Attack_Attack01);
// //option.Ability = ItemLibrary.Instance.Item_Equip_BeginnerSword.Resource as
// IConcreteActiveAbility; option.State = new ConcurrentUtilityAgentState { States = new IAiState[]
// { new AbilityAiNode { }, new SteeringAiNode { ContextEvaluator = new
// TargetAbilityContextEvaluator { }, PostprocessEvaluator = new AbilityArrivalEvaluator { } } } }; }));

//		public static Lazily<Option> Option_Attack_Tackle01 =
//			Func((() => Option_Attack_AttackBase.Prepare(
//			option =>
//			{
//				option.Name = nameof(Option_Attack_Tackle01);
//				option.AppraisalFlags = AppraisalFlags.Hostile;
//				option.RequiredTargets = 1;
//				option.Utility = 0;
//			},
//			option =>
//			{
//				IConcreteActiveAbility ability = null;
//#if CIRRUS_ARPG_SKILL_LIBRARY
//				ability = SkillLibrary.ActiveSkill_Attack_Tackle01.Resource as IConcreteActiveAbility;
//#endif
//				option.Considerations = new ConsiderationBase[]
//				{
//					new ConditionalConsideration
//					{
//						Name = "Health01",
//						Condition = new AttributeCondition
//						{
//							Mode = AttributeConditionBase.ConditionMode.Ratio,
//							Attribute = Attributes.AttrsLib.ID.Attr_Health,
//							Comparison = new Comparison("<", new Range(0.8f, 0.9f)),
//						}
//					}
//				};
//				option.TargetConsiderations = new List<ConsiderationBase>
//				{
//					new DistanceConsideration
//					{
//						Name = "Distance01",
//						Utility = 2,
//						DistanceRange = new Range(ability.Range.Min, ability.Range.Max),
//						InBound = DistanceConsideration.InBoundMode.UniformUtility,
//					}
//				};
//			}));

//		//public static Lazily<Option> Option_Wander01 =
//		//	Func((() => Option_SpontaneousBase.Copy(
//		//	option =>
//		//	{
//		//		option.Name = nameof(Option_Wander01);
//		//		option.Utility = 5;
//		//		option.RequiredTargets = 0;
//		//		option.IsSpontaneous = true;
//		//	},
//		//	option =>
//		//	{
//		//		option.State = new UtilityAgentStateSequence
//		//		{
//		//			States = new RepeatedList<IAiState>
//		//			{
//		//				new IdleAiState
//		//				{
//		//					Time = new Range(1,4)
//		//				},
//		//				new SteeringAiState
//		//				{
//		//					ContextEvaluators = new ContextEvaluatorBase[]
//		//					{
//		//						new WallRepulsionBehaviour
//		//						{
//		//							Name = "Wall Repulsion",
//		//						},
//		//						//new AvoidContextEvaluator
//		//						//{
//		//						//	Name = "Avoid"
//		//						//},
//		//						new WanderNearStartContextEvaluator
//		//						{
//		//							Name = "Wandering",
//		//						},
//		//					}
//		//				}
//		//			}
//		//		};
//		//	}));
//	}
//}