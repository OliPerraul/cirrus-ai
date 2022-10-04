using Cirrus.Unity.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public class WorldContextComponent : ComponentBase
	{
		[NonSerialized]
		private static WorldContextComponent _instance = null;

		[NonSerialized]
		private Dictionary<string, Blackboard> _blackboards = null;

		[NonSerialized]
		private Clock _clock = null;

		private static WorldContextComponent _Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = nameof(WorldContextComponent);
					_instance = (WorldContextComponent)gameObject.AddComponent(typeof(WorldContextComponent));
					_instance._clock = new Clock();
					_instance._blackboards = new Dictionary<string, Blackboard>();
					gameObject.isStatic = true;
#if !UNITY_EDITOR
				gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
				}
				return _instance;
			}
		}

		public static Clock GetClock()
		{
			return _Instance._clock;
		}

		public static Blackboard GetBlackboard(string key)
		{
			WorldContextComponent context = _Instance;
			if (!context._blackboards.ContainsKey(key))
			{
				context._blackboards.Add(key, new Blackboard(context._clock));
			}
			return context._blackboards[key];
		}

		public override void Awake()
		{
			_instance = null;
			_clock = new Clock();
		}

		public void Update()
		{
			_clock.Update(Time.deltaTime);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();

			_instance = null;
			_clock = null;
		}
	}
}