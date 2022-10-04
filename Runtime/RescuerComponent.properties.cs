using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Arpg.Entities.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
	public partial class RescuerComponent
		: EntitySupportBase
	{
		//public float Strength => Entity.Attrs.Attr_Strength;
		public float Strength => 0;

		public RescueParticipantGroup Participants;

		private Mutex _isRescuingMutex = new Mutex();

		public bool IsRescuing
		{
			get
			{
				_isRescuingMutex.WaitOne();
				bool result =
				(Ai.Behavtree.Flags & AiBtFlags.Rescuer) != 0;
				_isRescuingMutex.ReleaseMutex();
				return result;
			}
			set
			{
				_isRescuingMutex.WaitOne();
				Ai.Behavtree.Flags = value ?
				Ai.Behavtree.Flags | AiBtFlags.Rescuer :
				Ai.Behavtree.Flags & ~AiBtFlags.Rescuer;
				_isRescuingMutex.ReleaseMutex();
			}
		}


	}
}
