using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cirrus.Arpg.Abilities;
using Cirrus.Collections;
using Cirrus.Unity.Objects;
using Object = UnityEngine.Object;
using System.Threading;
using Cirrus.Objects;
using Cirrus.Unity.Randomness;
using Cirrus.Randomness;
using Cirrus.Unity.Numerics;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
	[Serializable]
	public class DirectorTokenPriority : IFlagged<EntityFlags>
	{
		[SerializeField]
		public Range_ weight = 1f;

		[SerializeField]
		public EntityBase entity;

		[SerializeField]
		public List<Object> _features;
		private HashSet<Object> _features_;
		public HashSet<Object> Features => _features_ == null ?
			_features_ = new HashSet<Object>(_features) :
			_features_;

		[SerializeField]
		public EntityFlags flags;
		EntityFlags IFlagged<EntityFlags>.Flags
		{
			get => flags;
			set { flags = (EntityFlags)value; }
		}
	}
}