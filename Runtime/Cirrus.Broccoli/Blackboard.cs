using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli
{
	public partial class Blackboard
	{
		public Blackboard(Blackboard parent, Clock clock)
		{
			_clock = clock;
			_parentBlackboard = parent;
		}

		public Blackboard(Clock clock)
		{
			_parentBlackboard = null;
			_clock = clock;
		}

		public void Enable()
		{
			if(_parentBlackboard != null)
			{
				_parentBlackboard._children.Add(this);
			}
		}

		public void Disable()
		{
			if(_parentBlackboard != null)
			{
				_parentBlackboard._children.Remove(this);
			}
			if(_clock != null)
			{
				_clock.RemoveTimer(_NotifiyObservers);
			}
		}

		public object this[object key]
		{
			get
			{
				return Get(key);
			}
			set
			{
				Set(key, value);
			}
		}

		public void Set(object value)
		{
			Set(value.GetType(), value);
		}

		public void Set(object key, object value)
		{
			if(_parentBlackboard != null && _parentBlackboard.IsSet(key))
			{
				_parentBlackboard.Set(key, value);
			}
			else
			{
				if(!_data.ContainsKey(key))
				{
					_data[key] = value;
					_notifications.Add(new Notification(key, BtBlackboardOp.Add, value));
					_clock.AddTimer(0f, 0, _NotifiyObservers);
				}
				else
				{
					if((_data[key] == null && value != null) || (_data[key] != null && !_data[key].Equals(value)))
					{
						_data[key] = value;
						_notifications.Add(new Notification(key, BtBlackboardOp.Change, value));
						_clock.AddTimer(0f, 0, _NotifiyObservers);
					}
				}
			}
		}

		public void Unset(object key, bool silent=false)
		{
			if(_data.ContainsKey(key))
			{
				_data.Remove(key);
				if (!silent)
				{
					_notifications.Add(new Notification(key, BtBlackboardOp.Remove, null));
					_clock.AddTimer(0f, 0, _NotifiyObservers);
				}
			}
		}

		public T Get<T>(object key)
		{
			object result = Get(key);
			if(result == null)
			{
				return default(T);
			}
			return (T)result;
		}

		public T Get<T>()
		{
			object result = Get(typeof(T));
			if (result == null)
			{
				return default(T);
			}
			return (T)result;
		}


		public object Get(object key)
		{
			if(_data.ContainsKey(key))
			{
				return _data[key];
			}
			else if(_parentBlackboard != null)
			{
				return _parentBlackboard.Get(key);
			}
			else
			{
				return null;
			}
		}

		public bool IsSet(object key)
		{
			return _data.ContainsKey(key) || (_parentBlackboard != null && _parentBlackboard.IsSet(key));
		}

		public void AddObserver(object key, Action<BtBlackboardOp, object> observer)
		{
			List<Action<BtBlackboardOp, object>> observers = GetObserverList(_observers, key);
			if(!_isNotifiyng)
			{
				if(!observers.Contains(observer))
				{
					observers.Add(observer);
				}
			}
			else
			{
				if(!observers.Contains(observer))
				{
					List<Action<BtBlackboardOp, object>> addObservers = GetObserverList(_addObservers, key);
					if(!addObservers.Contains(observer))
					{
						addObservers.Add(observer);
					}
				}

				List<Action<BtBlackboardOp, object>> removeObservers = GetObserverList(_removeObservers, key);
				if(removeObservers.Contains(observer))
				{
					removeObservers.Remove(observer);
				}
			}
		}

		public void RemoveObserver(object key, Action<BtBlackboardOp, object> observer)
		{
			List<Action<BtBlackboardOp, object>> observers = GetObserverList(_observers, key);
			if(!_isNotifiyng)
			{
				if(observers.Contains(observer))
				{
					observers.Remove(observer);
				}
			}
			else
			{
				List<Action<BtBlackboardOp, object>> removeObservers = GetObserverList(_removeObservers, key);
				if(!removeObservers.Contains(observer))
				{
					if(observers.Contains(observer))
					{
						removeObservers.Add(observer);
					}
				}

				List<Action<BtBlackboardOp, object>> addObservers = GetObserverList(_addObservers, key);
				if(addObservers.Contains(observer))
				{
					addObservers.Remove(observer);
				}
			}
		}

		private void _NotifiyObservers()
		{
			if(_notifications.Count == 0)
			{
				return;
			}

			_notificationsDispatch.Clear();
			_notificationsDispatch.AddRange(_notifications);
			foreach(var child in _children)
			{
				child._notifications.AddRange(_notifications);
				child._clock.AddTimer(0f, 0, child._NotifiyObservers);
			}
			_notifications.Clear();

			_isNotifiyng = true;
			for(int i = 0; i < _notificationsDispatch.Count; i++)
			{
				var notif = _notificationsDispatch[i];
				if(!_observers.ContainsKey(notif.Key))
				{
					// Debug.Log("1 do not notify for key:" + notification.key + " value: " + notification.value);
					continue;
				}

				List<Action<BtBlackboardOp, object>> observers = GetObserverList(_observers, notif.Key);
				for(int j = 0; j < observers.Count; j++)
				{
					if(_removeObservers.ContainsKey(notif.Key) && _removeObservers[notif.Key].Contains(observers[j]))
					{
						continue;
					}
					observers[j](notif.Op, notif.Value);
				}
			}

			foreach(object key in _addObservers.Keys)
			{
				GetObserverList(_observers, key).AddRange(_addObservers[key]);
			}
			foreach(object key in _removeObservers.Keys)
			{
				foreach(Action<BtBlackboardOp, object> action in _removeObservers[key])
				{
					GetObserverList(_observers, key).Remove(action);
				}
			}
			_addObservers.Clear();
			_removeObservers.Clear();

			_isNotifiyng = false;
		}

		private List<Action<BtBlackboardOp, object>> GetObserverList(Dictionary<object, List<Action<BtBlackboardOp, object>>> target, object key)
		{
			List<Action<BtBlackboardOp, object>> observers;
			if(target.ContainsKey(key))
			{
				observers = target[key];
			}
			else
			{
				observers = new List<Action<BtBlackboardOp, object>>();
				target[key] = observers;
			}
			return observers;
		}
	}
}