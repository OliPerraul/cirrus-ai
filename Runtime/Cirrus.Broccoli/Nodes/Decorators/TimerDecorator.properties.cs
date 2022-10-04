using Cirrus.Unity.Numerics;

using System;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

using UnityEngine;

namespace Cirrus.Broccoli
{	
	public partial class TimeDecoratorInstance : DecoratorInstanceBase
	{
		public override object Data { get => null; set { } }

		private Range_ _timeLimit = 2.0f;

		public TimeDecoratorInstance() : base()
		{
		}

		public TimeDecoratorInstance(string name, object obj) : base(name, obj)
		{
		}

		public TimeDecoratorInstance(object obj) : base(obj)
		{
		}

		public TimeDecoratorInstance(string name, Range_ timeLimit) : base(name)
		{
			_timeLimit = timeLimit;
		}

		public TimeDecoratorInstance(Range_ timeLimit) : base()
		{
			_timeLimit = timeLimit;
		}
	}

	//public partial class TimeDecoratorInstance<TContext, TData> : DecoratorInstanceBase
	//{
	//	public TContext context;

	//	public override ContextBase Context { get => (ContextBase)(IContext)context; set => context = (TContext)(IContext)value; }
	//	public TData data;

	//	public override object Data { get => data; set => data = (TData)value; }

	//	public Func<TContext, TData, float> _timeCb = null;


	//	public TimeDecoratorInstance() : base()
	//	{
	//	}

	//	public TimeDecoratorInstance(string name, TData data) : base(name, data)
	//	{
	//	}

	//	public TimeDecoratorInstance(TData data) : base(data)
	//	{
	//	}

	//	public TimeDecoratorInstance(string name, TData data, Func<TContext, TData, float> timeCb) : base(name, data)
	//	{
	//		_timeCb = timeCb;
	//	}

	//	public TimeDecoratorInstance(TData data, Func<TContext, TData, float> timeCb) : base(data)
	//	{
	//		_timeCb = timeCb;
	//	}
	//}
}