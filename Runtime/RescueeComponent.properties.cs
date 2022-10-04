using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	public partial class RescueeComponent 
		: EntitySupportBase
		, IKnapsackItem
	{
		public int CompareTo(object o)
		{
			IKnapsackItem item = (IKnapsackItem)o;
			return (Reward / Weight).CompareTo(item.Reward / item.Weight);
		}

		//public float Reward => Character.Attrs.Attr_Reward;
		public float Reward => 0;

		//public float Weight => Character.Attrs.Attr_Weight;
		public float Weight => 0;

		private List<CharacterObject> _rescuers { get; set; } = new List<CharacterObject>();

		private Mutex _rescuersMutex = new Mutex();

		public void AddRescuer(CharacterObject rescuer)
		{
			_rescuersMutex.WaitOne();
			_rescuers.Add(rescuer);
			//_rescueStrength += rescuer.Attrs.Attr_Strength;
			_rescuersMutex.ReleaseMutex();
		}

		private float _rescueStrength { get; set; } = 0;

		/// <summary>
		/// Current strength of combined member rescuing this instance.
		/// </summary>
		public float RescueStrength
		{
			get
			{
				_rescuersMutex.WaitOne();
				float rescueStrength = _rescueStrength;
				_rescuersMutex.ReleaseMutex();
				return rescueStrength;
			}
		}

		//public bool IsInjured => CharaComp.Bt.Blackboard.Get<CharacterBtSecondaryMsg>() == CharacterBtSecondaryMsg.Injured;

		public bool IsRescuePossible(CharacterObject rescuer)
		{
			// TODO: replace with flag
			//if (CharaComp.Bt.Blackboard.Get<CharaBtLocoMsg>() != CharaBtLocoMsg.Injured) return false;

			_rescuersMutex.WaitOne();

			float strength = 0;
			foreach (CharacterObject member in EntityObject.group)
			{
				//strength += member.Attrs.Attr_Strength;
			}

			bool result = (_rescuers.Count == 0 || !EntityObject.IsCarriable(strength));
			_rescuersMutex.ReleaseMutex();
			return result;
		}
	}
}
