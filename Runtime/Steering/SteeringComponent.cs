using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
	public partial class SteeringComponent : EntitySupportBase
	{
		public bool IsEnabled
		{
			get => enabled;
			set => enabled = value;
		}
	}
}