using Cirrus.Arpg.Abilities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using Cirrus.Unity.Numerics;
using Cirrus.Unity.Objects;

using System;

using UnityEngine;
using Cirrus.Arpg.Entities;
//using System.Numerics;

namespace Cirrus.Arpg.AI
{
	// TODO rename
	// AISupport can exist on non character objects...
	public partial class AiComponent : EntitySupportBase
	{
		protected override void _OnEntityInit(EntityInstanceBase e)
		{
			base._OnEntityInit(e);

			if(e is not CharacterInstanceBase character) return;

			if(character.Character == null) return;
		}

		public void _OnAbilityEndLagEnded(IActiveAbilityInstance ability)
		{
			//Ai.Director.ReturnToken(_currentAbility);
			//_currentAbility.OnAvailableHandler -= _OnAbilityEndLagEnded;
			//_currentAbilityState = AiAbilityState.Idle;
			//_currentAbility = null;
		}

		public bool StartAbility(IActiveAbilityInstance ab, EntityObjectBase target)
		{
			if(
				_currentAbility == null &&
				_currentAbilityState == AiAbilityState.Idle
				)
			{
				Range_ range = ab.Range;
				Vector3 toTarget = target.Position - EntityObject.Position;
				if(toTarget.magnitude < range.max)

				if(ab.IsAvailable(CharacterObject) && ab.Start(CharacterObject, target))
				{
					_currentAbility = ab;
					_currentAbilityState = AiAbilityState.Started;
					_currentAbility.OnAvailableHandler += _OnAbilityEndLagEnded;
					_currentAbility.OnAvatarAbilityStartedHandler?.Invoke(_currentAbility);
				}

				return true;
			}

			return false;
		}

		// End ability (i.e. Mostly sustained abilities)
		public bool EndAbility(int index = -1)
		{
			if(CharacterInst.ActiveAbilities.Get(index, out IActiveAbilityInstance ab))
			{
				return _EndAbility(ab);
			}

			return false;
		}

		public bool _EndAbility(IAbilityInstance ab)
		{
			if(
				_currentAbilityState == AiAbilityState.Started &&
				_currentAbility == ab
				)
			{
				if(CharacterObject
					.Avatar
					.Interact
					.EndAbility(_currentAbility))
				{
					return true;
				}
			}

			return false;
		}
	}	
}