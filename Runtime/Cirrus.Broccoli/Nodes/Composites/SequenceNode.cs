using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli 
{
	public partial class SequenceNodeInstance
	{
		public SequenceNodeInstance() : base()
		{
		}
		public SequenceNodeInstance(string name) : base(name)
		{
		}
		public SequenceNodeInstance(string name, object obj) : base(name, obj)
		{
		}

		public SequenceNodeInstance(object obj) : base(obj)
		{
		}


		protected override void _Init()
		{
			base._Init();
		}

		protected override void _AddChild(NodeInstanceBase decoratee)
		{
			base._AddChild(decoratee);
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
			if (result) ProcessChildren();

			else _OnStopped(false);
		}

		private void ProcessChildren()
		{
			if (++_currentIndex < Children.Count)
			{
				if (State == NodeState.Stopping) _OnStopped(false);

				else Children[_currentIndex].Start();
			}
			else _OnStopped(true);
		}

		public override void StopLowerPriorityChildrenForChild(NodeInstanceBase abortForChild, bool immediateRestart)
		{
			int indexForChild = 0;
			bool found = false;
			for (int i = 0; i < Children.Count; i++)
			{
				if (Children[i] == abortForChild)
				{
					found = true;
				}
				else if (!found)
				{
					indexForChild++;
				}
				else if (found && Children[i].State == NodeState.Active)
				{
					if (immediateRestart)
					{
						_currentIndex = indexForChild - 1;
					}
					else
					{
						_currentIndex = Children.Count;
					}
					Children[i].Stop();
					break;
				}
			}
		}

		override public string ToString()
		{
			return base.ToString() + "[" + _currentIndex + "]";
		}
	}
}