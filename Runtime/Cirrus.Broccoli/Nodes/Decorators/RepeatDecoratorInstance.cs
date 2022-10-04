using Cirrus.Objects;
using Cirrus.Broccoli;
using System;

namespace Cirrus.Broccoli
{
	public class RepeatDecoratorInstance : DecoratorInstanceBase
	{
		public int loopCount = -1;
		private int _currentLoop;

		public bool repeatOnFailed = true;

		public override object Data { get => null; set { } }

		public RepeatDecoratorInstance() : base()
		{
		}


		//public RepeatDecoratorInstance(string name) : base(name)
		//{
		//}
		public RepeatDecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public RepeatDecoratorInstance(object obj) : base(obj)
		{
		}

		//public Action<TContext, RepeatDecorator<TContext, TData>> InitCb;

		public RepeatDecoratorInstance(string name, int loopCount=-1, bool repeatFailures=true) : base(name)
		{
			this.loopCount = loopCount;
			this.repeatOnFailed = repeatFailures;
		}
		public RepeatDecoratorInstance(int loopCount = -1, bool repeatFailures = true) : base()
		{
			this.loopCount = loopCount;
			this.repeatOnFailed = repeatFailures;
		}

		protected override void _Init()
		{
			//InitCb?.Invoke(Context, this);
		}

		protected override void _Start()
		{
			if (!Context.IsValid) return;
			if (loopCount != 0)
			{
				_currentLoop = 0;
				Child.Start();
			}
			else
			{
				_OnStopped(true);
			}
		}

		protected override void _Stop()
		{
			Clock.RemoveTimer(_RestartDecoratee);

			if(Child.State == NodeState.Active)
			{
				Child.Stop();
			}
			else
			{
				_OnStopped(false);
			}
		}

		protected override void _ChildStopped(NodeInstanceBase child, bool result)
		{
			if(repeatOnFailed ? !result : result) 
			{
				if(
					State == NodeState.Stopping 
					|| (loopCount > 0 && ++_currentLoop >= loopCount)
					)
				{
					_OnStopped(true);
				}
				else
				{
					Clock.AddTimer(0, 0, _RestartDecoratee);
				}
			}
			else
			{
				_OnStopped(false);
			}
		}

		protected void _RestartDecoratee()
		{
			Child.Start();
		}
	}
}