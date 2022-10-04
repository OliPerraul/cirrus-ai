using Cirrus.Objects;

using System.Collections;

using static Cirrus.Debugging.DebugUtils;


namespace Cirrus.Broccoli
{
	public interface ITaskNodeInstance : INodeInstance
	{
		void Update();

		void FixedUpdate();

		void LateUpdate();

		void OnDrawGizmos();
		void CustomUpdate1(float dt);

		void CustomUpdate2(float dt);

		void CustomUpdate3(float dt);

		string Name { get; }
	}

	public abstract partial class TaskNodeInstanceBase
	: NodeInstanceBase
	, ITaskNodeInstance
	{
		private RootNodeInstance _root = null;

		public override RootNodeInstance Root
		{
			get => _root;
			set => _root = value;
		}
	}

	public abstract partial class TaskNodeInstanceBase<TContext, TData> 
	: TaskNodeInstanceBase
	, IEnumerable
	where TContext : IContext
	{
		
		public TData data;
		public override object Data { get => data; set => data = (TData)value; }

		// Covariant return type
		public TContext context;

		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext)value; }
	}

	public abstract partial class TaskNodeInstanceBase<TContext>
	: TaskNodeInstanceBase
	, IEnumerable
	where TContext : IContext
	{
		// Covariant return type
		public TContext context;

		public override BehavtreeContextBase Context { get => (BehavtreeContextBase)(IContext)context; set => context = (TContext)(IContext)value; }
	}
}