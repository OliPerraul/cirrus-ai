//using Cirrus.AI.States;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;

//namespace Cirrus.Arpg.AI
//{
//	public partial class AiNodeBase<TData>
//		: ActionNodeBase<ControlBt, TData>
//		, IRealizableResource
//	{
//		public AiNodeBase(string name="") : base(name)
//		{
//		}

//		public override object MemberwiseCopy()
//		{
//			return MemberwiseClone();
//		}

//		public override void OnResourceRealized()
//		{
//			//base.OnResourceRealized();
//			//_timer = new Timer(OnTimeout);
//		}

//		public void UpdateGoal(OptionBase option, params object[] args)
//		{
//		}

//		public void UpdateGoal()
//		{
//		}
//	}
//}