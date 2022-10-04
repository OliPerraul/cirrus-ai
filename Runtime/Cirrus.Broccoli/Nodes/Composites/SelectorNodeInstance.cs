using Cirrus.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	public partial class SelectorNodeInstance
		: CompositeNodeInstanceBase
	{
		private int _currentIndex = -1;


		public SelectorNodeInstance() : base()
		{
		}

		public SelectorNodeInstance(string name) : base(name)
		{
		}

		public SelectorNodeInstance(string name, object obj) : base(name, obj)
		{
		}

		public SelectorNodeInstance(object obj) : base(obj)
		{
		}

		protected override void _Start()
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Assert(Children[i].Parent == this, "Child's parent is invalid.", true);
				Assert(Children[i].State == NodeState.Inactive);
			}

			_currentIndex = -1;

			ProcessChildren();
		}

		protected override void _Stop()
		{
			Children[_currentIndex].Stop();
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool result)
		{
			if (result)
			{
				_OnStopped(true);
			}
			else
			{
				ProcessChildren();
			}
		}

		private void ProcessChildren()
		{
			if (++_currentIndex < Children.Count)
			{
				if (State == NodeState.Stopping)
				{
					_OnStopped(false);
				}
				else
				{
					Children[_currentIndex].Start();
				}
			}
			else
			{
				_OnStopped(false);
			}
		}

		public override void StopLowerPriorityChildrenForChild(NodeInstanceBase abortForChild, bool immediateRestart)
		{
			int indexForChild = 0;
			bool found = false;
			foreach (var currentChild in Children)
			{
				if (currentChild == abortForChild)
				{
					found = true;
				}
				else if (!found)
				{
					indexForChild++;
				}
				else if (found && currentChild.State == NodeState.Active)
				{
					if (immediateRestart)
					{
						_currentIndex = indexForChild - 1;
					}
					else
					{
						_currentIndex = Children.Count;
					}
					currentChild.Stop();
					break;
				}
			}
		}

		override public string ToString()
		{
			return base.ToString() + "[" + this._currentIndex + "]";
		}
	}
}