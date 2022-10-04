using Cirrus.Unity.Numerics;

using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Cirrus.Broccoli
{
	public partial class TimeDecoratorInstance
	{
		private Task _task;
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		//public Action<TContext, RepeatDecorator<TContext, TData>> InitCb;

		public override object Copy()
		{
			TimeDecoratorInstance inst = (TimeDecoratorInstance)base.Copy();
			inst._tokenSource = new CancellationTokenSource();
			return inst;
		}

		protected override void _Init()
		{
			//InitCb?.Invoke(Context, this);
		}

		protected override void _Start()
		{
			if(!Context.IsValid) return;

			CancellationToken token = _tokenSource.Token;

			_task = AwaitTimeout(_tokenSource.Token);

			Child.Start();
		}

		async Task AwaitTimeout(CancellationToken token)
		{
			await Task.Delay(TimeUtils.SecondToMilisecond(_timeLimit.Random()));
			token.ThrowIfCancellationRequested();

			if(State == NodeState.Active)
			{
				Stop();
			}
		}

		protected override void _OnStopped(bool success)
		{
			base._OnStopped(success);

			_tokenSource.Cancel();
		}
	}

	//public partial class TimeDecoratorInstance<TContext, TData>
	//{
	//	private Task _task;

	//	protected override void _Init()
	//	{
	//		base._Init();
	//		context = (TContext)(IContext)Root.Context;
	//	}

	//	protected override void _Start()
	//	{
	//		if(!Context.IsValid) return;

	//		_task = AwaitTimeout();

	//		Child.Start();
	//	}

	//	async Task AwaitTimeout()
	//	{
	//		await Task.Delay(TimeUtils.SecondToMilisecond(_timeCb(context, data)));

	//		if(State == NodeState.Active)
	//		{
	//			Stop();
	//		}
	//	}
	//}
}