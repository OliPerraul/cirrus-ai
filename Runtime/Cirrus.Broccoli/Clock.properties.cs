using Cirrus.Collections;
using System;
using System.Collections.Generic;

//using Collections.Generic;

namespace Cirrus.Broccoli
{
	public partial class Clock
	{
		private class _Timer
		{
			public double _scheduledTime = 0f;
			public int _repeat = 0;
			public bool _used = false;
			public double _delay = 0f;
			public float _randomVariance = 0.0f;

			public void ScheduleAbsoluteTime(double elapsedTime)
			{
				_scheduledTime = elapsedTime + _delay - _randomVariance * 0.5f + _randomVariance * UnityEngine.Random.value;
			}
		}

		private List<Action> _updateObservers = new List<Action>();
		private Dictionary<Action, _Timer> _timers = new Dictionary<Action, _Timer>();
		private HashSet<Action> _removeObservers = new HashSet<Action>();
		private HashSet<Action> _addObservers = new HashSet<Action>();
		private HashSet<Action> _removeTimers = new HashSet<Action>();
		private Dictionary<Action, _Timer> _addTimers = new Dictionary<Action, _Timer>();
		private bool _isInUpdate = false;

		public int DebugPoolSize => _timerPool.Count;

		public int NumUpdateObservers => _updateObservers.Count;

		public int NumTimers => _timers.Count;

		public double ElapsedTime => _elapsedTime;

		private double _elapsedTime = 0f;

		private List<_Timer> _timerPool = new List<_Timer>();
		private int _currentTimerPoolIndex = 0;
	}
}