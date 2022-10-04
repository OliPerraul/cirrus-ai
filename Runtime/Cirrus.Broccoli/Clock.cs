using System;
using System.Collections.Generic;

//using Collections.Generic;
using UnityEngine.Assertions;

namespace Cirrus.Broccoli
{
	public partial class Clock
	{
		/// <summary>
		/// Register a timer function
		/// </summary>
		/// <param name="time">time in milliseconds</param>
		/// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
		/// <param name="action">method to invoke</param>
		public void AddTimer(float time, int repeat, Action action)
		{
			AddTimer(time, 0f, repeat, action);
		}

		/// <summary>
		/// Register a timer function with random variance
		/// </summary>
		/// <param name="time">time in milliseconds</param>
		/// <param name="randomVariance">deviate from time on a random basis</param>
		/// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
		/// <param name="action">method to invoke</param>
		public void AddTimer(float delay, float randomVariance, int repeat, Action action)
		{
			_Timer timer = null;

			if (!_isInUpdate)
			{
				if (!_timers.ContainsKey(action))
				{
					_timers[action] = _GetTimerFromPool();
				}
				timer = _timers[action];
			}
			else
			{
				if (!_addTimers.ContainsKey(action))
				{
					_addTimers[action] = _GetTimerFromPool();
				}
				timer = _addTimers[action];

				if (_removeTimers.Contains(action))
				{
					_removeTimers.Remove(action);
				}
			}

			Assert.IsTrue(timer._used);
			timer._delay = delay;
			timer._randomVariance = randomVariance;
			timer._repeat = repeat;
			timer.ScheduleAbsoluteTime(_elapsedTime);
		}

		public void RemoveTimer(Action action)
		{
			if (!_isInUpdate)
			{
				if (_timers.ContainsKey(action))
				{
					_timers[action]._used = false;
					_timers.Remove(action);
				}
			}
			else
			{
				if (_timers.ContainsKey(action))
				{
					_removeTimers.Add(action);
				}
				if (_addTimers.ContainsKey(action))
				{
					Assert.IsTrue(_addTimers[action]._used);
					_addTimers[action]._used = false;
					_addTimers.Remove(action);
				}
			}
		}

		public bool HasTimer(Action action)
		{
			if (!_isInUpdate)
			{
				return _timers.ContainsKey(action);
			}
			else
			{
				if (_removeTimers.Contains(action))
				{
					return false;
				}
				else if (_addTimers.ContainsKey(action))
				{
					return true;
				}
				else
				{
					return _timers.ContainsKey(action);
				}
			}
		}

		/// <summary>
		/// Register a function that is called every frame
		/// </summary>
		/// <param name="action">function to invoke</param>
		public void AddUpdateObserver(Action action)
		{
			if (!_isInUpdate)
			{
				_updateObservers.Add(action);
			}
			else
			{
				if (!_updateObservers.Contains(action))
				{
					_addObservers.Add(action);
				}
				if (_removeObservers.Contains(action))
				{
					_removeObservers.Remove(action);
				}
			}
		}

		public void RemoveUpdateObserver(Action action)
		{
			if (!_isInUpdate)
			{
				_updateObservers.Remove(action);
			}
			else
			{
				if (_updateObservers.Contains(action))
				{
					_removeObservers.Add(action);
				}
				if (_addObservers.Contains(action))
				{
					_addObservers.Remove(action);
				}
			}
		}

		public bool HasUpdateObserver(Action action)
		{
			if (!_isInUpdate)
			{
				return _updateObservers.Contains(action);
			}
			else
			{
				if (_removeObservers.Contains(action))
				{
					return false;
				}
				else if (_addObservers.Contains(action))
				{
					return true;
				}
				else
				{
					return _updateObservers.Contains(action);
				}
			}
		}

		public void Update(float deltaTime)
		{
			_elapsedTime += deltaTime;

			_isInUpdate = true;

			for (int i = 0; i < _updateObservers.Count; i++)
			{
				Action action = _updateObservers[i];
				if (!_removeObservers.Contains(action))
				{
					action.Invoke();
				}
			}

			Dictionary<Action, _Timer>.KeyCollection keys = _timers.Keys;
			foreach (Action callback in keys)
			{
				if (_removeTimers.Contains(callback))
				{
					continue;
				}

				_Timer timer = _timers[callback];
				if (timer._scheduledTime <= _elapsedTime)
				{
					if (timer._repeat == 0)
					{
						RemoveTimer(callback);
					}
					else if (timer._repeat >= 0)
					{
						timer._repeat--;
					}
					callback.Invoke();
					timer.ScheduleAbsoluteTime(_elapsedTime);
				}
			}

			foreach (Action action in _addObservers)
			{
				_updateObservers.Add(action);
			}
			foreach (Action action in _removeObservers)
			{
				_updateObservers.Remove(action);
			}
			foreach (Action action in _addTimers.Keys)
			{
				if (_timers.ContainsKey(action))
				{
					Assert.AreNotEqual(_timers[action], _addTimers[action]);
					_timers[action]._used = false;
				}
				Assert.IsTrue(_addTimers[action]._used);
				_timers[action] = _addTimers[action];
			}
			foreach (Action action in _removeTimers)
			{
				Assert.IsTrue(_timers[action]._used);
				_timers[action]._used = false;
				_timers.Remove(action);
			}
			_addObservers.Clear();
			_removeObservers.Clear();
			_addTimers.Clear();
			_removeTimers.Clear();

			_isInUpdate = false;
		}

		private _Timer _GetTimerFromPool()
		{
			int i = 0;
			int l = _timerPool.Count;
			_Timer timer = null;
			while (i < l)
			{
				int timerIndex = (i + _currentTimerPoolIndex) % l;
				if (!_timerPool[timerIndex]._used)
				{
					_currentTimerPoolIndex = timerIndex;
					timer = _timerPool[timerIndex];
					break;
				}
				i++;
			}

			if (timer == null)
			{
				timer = new _Timer();
				_currentTimerPoolIndex = 0;
				_timerPool.Add(timer);
			}

			timer._used = true;
			return timer;
		}
	}
}