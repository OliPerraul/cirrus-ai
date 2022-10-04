using Cirrus.Unity.Numerics;
using Cirrus.Unity.Randomness;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Arpg.AI
{
	// 

	// TODO
	// Speech in response to another character speech
	// TODO
	// One agent in the party says something stupid. The leader turns around and smack him on the head
	// Loses HP. (Adventer vs. Adventurer conflict), Let's see Monster vs Monster conflict?
	public partial class SpeechState
	{
		//public override string Name => "Speech";

		// Emotes
		public ICollection<Sprite> Emotes = new List<Sprite>();

		[SerializeField]
		public Range_ Range = new Range_(1f, 1.5f);

		public float Time { get; set; } = new Range_(5f, 10.5f);

		public float SpeechTime { get; set; } = new Range_(1f, 2.5f);

		// Go close to object and speech with question mark
		// TODO make sure that timer isn;t cloned!
		private Timer _timer;

		private Chance _chanceRandomPosition = new Chance(0.4f);

		private Vector3 _speechStartPosition = Vector3.zero;
	}
}
