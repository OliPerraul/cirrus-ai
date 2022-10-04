using Cirrus.Arpg.Abilities;
using Cirrus.Unity.Randomness;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public partial class __AbilityState
	{
		public Chance ChanceRandomPosition = new Chance(0.5f);

		public long AbilityID = -1;

		public int AbilityFlags = 0;

		public IActiveAbilityInstance Ability { get; set; }

		//private bool _isAbilityActive = false;

		private Vector3 _abilityStartPosition;

		private IActiveAbilityInstance _ability;
	}
}

