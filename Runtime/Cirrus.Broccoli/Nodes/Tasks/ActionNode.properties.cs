using System;

using Cirrus.Objects;
using Cirrus.Unity.Objects;

using UnityEngine.Assertions;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public enum NodeResult
	{
		Success,
		Failed,
		Blocked,
		Running,
		None,
		Error
	}

	public abstract partial class ActionNodeInstanceBase
	: TaskNodeInstanceBase
	{
		public ActionNodeInstanceBase() : base()
		{
		}

		public ActionNodeInstanceBase(string name) : base(name)
		{
		}
		public ActionNodeInstanceBase(string name, object obj) : base(name, obj)
		{
		}

		public ActionNodeInstanceBase(object obj) : base(obj)
		{
		}

	}

	public partial class ActionNodeInstance<TContext, TData> : ActionNodeInstanceBase
		where TContext : IContext
	{
		public ActionNodeInstance() : base()
		{
		}

		public ActionNodeInstance(string name) : base(name)
		{
		}

		public TData data;
		public override object Data { get => data; set => data = (TData)value; }

		public TContext context;

		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext)value; }


		public Action<TContext, ActionNodeInstance<TContext, TData>> initCb = null;
		public Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb = null;
		public Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null;
		public Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null;

		public Action<TContext, ActionNodeInstance<TContext, TData>> fixedUpdateCb = null;
		public Action<TContext, ActionNodeInstance<TContext, TData>> lateUpdateCb = null;
		public Action<TContext, ActionNodeInstance<TContext, TData>> onDrawGizmosCb = null;		
		public Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null;
		public Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null;
		public Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null;

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
			string name
	, Action<TContext, ActionNodeInstance<TContext, TData>> initCb
	, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
	, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null
	, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null
	, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null
	, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
	, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
	) : base()
		{
			this.Name = name;
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base()
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(name)
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		TData data,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(data)
		{
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		TData data,
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(data)
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		TData data,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(data)
		{
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		TData data,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(data)
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		TData data,
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb,
		Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		) : base(data)
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

	}

	public partial class ActionNodeInstance<TContext> : ActionNodeInstance<TContext, None>
	where TContext : IContext
	{
		public ActionNodeInstance() : base()
		{
		}

		public ActionNodeInstance(string name) : base(name)
		{
		}

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, None>> initCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			base.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, None>> initCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, None>> initCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}


		public ActionNodeInstance(
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base()
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb = null
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> updateCb = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.updateCb = updateCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Action<TContext, ActionNodeInstance<TContext, None>> initCb,
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base(name)
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base(name)
		{
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public ActionNodeInstance(
		string name,
		Action<TContext, ActionNodeInstance<TContext, None>> initCb,
		Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, None>, NodeResult> exitCb
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, None>, float> customCb3 = null
		) : base(name)
		{
			this.initCb = initCb;
			this.enterCb = enterCb;
			this.exitCb = exitCb;
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}
	}

	public partial class UpdateNodeInstance<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		public UpdateNodeInstance(Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb)
		{
			this.updateCb = updateCb;
		}

		public UpdateNodeInstance(
			Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
			, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb
		)
		{
			this.updateCb = updateCb;
			this.enterCb = enterCb;
		}

		public UpdateNodeInstance(
		string name
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb)
		: base(name)
		{
			this.updateCb = updateCb;
		}


		public UpdateNodeInstance(
		string name
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb
		)
		: base(name)
		{
			this.updateCb = updateCb;
			this.enterCb = enterCb;
		}

		public UpdateNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb
		)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}

		public UpdateNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb
		)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
			this.enterCb = enterCb;
		}


		public UpdateNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb)
		: base(name)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
		}

		public UpdateNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> enterCb
		, Func<TContext, ActionNodeInstance<TContext, TData>, NodeResult> updateCb
		)
		: base(name)
		{
			this.initCb = initCb;
			this.updateCb = updateCb;
			this.enterCb = enterCb;
		}
	}

	public partial class FixedUpdateNodeInstance<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		public FixedUpdateNodeInstance(Action<TContext, ActionNodeInstance<TContext, TData>> fixedUpdateCb)
		{
			this.fixedUpdateCb = fixedUpdateCb;
		}

		public FixedUpdateNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> fixedUpdateCb)
		: base(name)
		{
			this.fixedUpdateCb = fixedUpdateCb;
		}

		public FixedUpdateNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Action<TContext, ActionNodeInstance<TContext, TData>> fixedUpdateCb
		)
		{
			this.initCb = initCb;
			this.fixedUpdateCb = fixedUpdateCb;
		}

		public FixedUpdateNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Action<TContext, ActionNodeInstance<TContext, TData>> fixedUpdateCb)
		: base(name)
		{
			this.initCb = initCb;
			this.fixedUpdateCb = fixedUpdateCb;
		}
	}


	public partial class CustomUpdateNodeInstance<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		public CustomUpdateNodeInstance(
		Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		{
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public CustomUpdateNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb1
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2 = null
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		: base(name)
		{
			this.customCb1 = customCb1;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}
	}

	public partial class CustomUpdateNodeInstance2<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		public CustomUpdateNodeInstance2(
		Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		{
			this.initCb = initCb;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public CustomUpdateNodeInstance2(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> initCb
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		: base(name)
		{
			this.initCb = initCb;
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public CustomUpdateNodeInstance2(
		Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		{
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}

		public CustomUpdateNodeInstance2(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb2
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3 = null
		)
		: base(name)
		{
			this.customCb2 = customCb2;
			this.customCb3 = customCb3;
		}
	}

	public partial class CustomUpdateNodeInstance3<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		public CustomUpdateNodeInstance3(Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3)
		{
			this.customCb3 = customCb3;
		}

		public CustomUpdateNodeInstance3(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>, float> customCb3)
		: base(name)
		{
			this.customCb3 = customCb3;
		}
	}

	public partial class InitNodeInstance<TContext, TData> : ActionNodeInstance<TContext, TData>
	where TContext : IContext
	{
		protected override NodeResult _Enter()
		{
			return enterCb == null ? NodeResult.Success : enterCb.Invoke(context, this);
		}

		protected override NodeResult _Update()
		{
			return updateCb == null ? NodeResult.Success : updateCb.Invoke(context, this);
		}

		public InitNodeInstance(Action<TContext, ActionNodeInstance<TContext, TData>> initCb)
		{
			this.initCb = initCb;
		}

		public InitNodeInstance(
		string name
		, Action<TContext, ActionNodeInstance<TContext, TData>> initCb)
		: base(name)
		{
			this.initCb = initCb;
		}
	}
}