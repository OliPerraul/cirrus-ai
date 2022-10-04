using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cirrus.Debugging.DebugUtils;

namespace Cirrus.Broccoli
{
	public abstract partial class NodeInstanceBase
	{
		public NodeInstanceBase(string name)
		{
			Name = name;
		}

		public NodeInstanceBase(string name, object data)
		{
			Name = name;
			Data = data;
		}

		public NodeInstanceBase(object data)
		{
			Data = data;
		}

		public NodeInstanceBase()
		{
			Name = GetType().Name.FormatNodeName();
		}

		public override object Copy()
		{
			var inst = (NodeInstanceBase)base.Copy();
			inst.Parent = null;
			return inst;
		}

		public virtual void Add(NodeInstanceBase child)
		{
			Assert(false, true);
		}

		public virtual void Add(IEnumerable<NodeInstanceBase> children)
		{
			Assert(false, true);
		}

		public virtual void Clear()
		{
			Assert(false, true);
		}

		public T Ancestor<T>(int position = -1)
		{
			return (T)Ancestor(typeof(T), position);
		}

		public object Ancestor(Type type, int position=-1)
		{
			NodeInstanceBase parent = Parent;
			for(int i = 0; i <= position || position < 0; i++)
			{
				if(parent == null) break;
				if(i == position || (position == -1 && parent.IsAssignableTo(type))) return parent;
				parent = parent.Parent;
			}

			return null;
		}



		public NodeInstanceBase Ancestor(int position)
		{
			NodeInstanceBase parent = Parent;
			for (int i = 0; i <= position; i++)
			{
				if(parent == null) break;
				if (i == position) return parent;
				parent = parent.Parent;
			}

			return null;
		}


		public void OnChildStopped(NodeInstanceBase child, bool succeeded)
		{
			// Assert.AreNotEqual(this.currentState, State.INACTIVE, "The Child " + child.Name + "
			// of Container " + this.Name + " was stopped while the container was inactive. PATH: "
			// + GetPath());
			Assert(State != NodeState.Inactive, "A Child of a Container was stopped while the container was inactive.", true);
			_ChildStopped(child, succeeded);
		}

		protected virtual void _ChildStopped(NodeInstanceBase child, bool succeeded) { }

		protected virtual void _Init() { }

		protected virtual void _InitChildren() { }

		public void Init()
		{
			if (_state == NodeState.Uninit && Root != null)
			{
				if(Parent != null && Parent.Data != null && Data == null) Data = Parent.Data;
				//if(this.IsAssignableTo<IAncestorDataHelper>())
				_Init();
				_InitChildren();

				_state = NodeState.Inactive;
			}
		}

		public void Start()
		{
			Init();
			// Assert.AreEqual(currentState, State.INACTIVE, "can only start inactive nodes, tried
			// to start: " + Name + "! PATH: " + GetPath());
			Assert(State == NodeState.Inactive, "can only start inactive nodes", true);
			State = NodeState.Active;
			_Start();
		}

		/// <summary>
		/// TODO: Rename to "Cancel" in next API-Incompatible version
		/// </summary>
		public void Stop()
		{
			// Assert.AreEqual(currentState, State.ACTIVE, "can only stop active nodes, tried to
			// stop " + Name + "! PATH: " + GetPath());
			Assert(State == NodeState.Active, "can only stop active nodes, tried to stop", true);
			State = NodeState.Stopping;
			_Stop();
		}

		protected virtual void _Start()
		{
		}

		protected virtual void _Stop()
		{
		}

		protected virtual void _OnStarted()
		{
		}

		public virtual void Schedule(TaskNodeInstanceBase node)
		{
			Assert(Parent != null, true);
			Parent.Schedule(node);
		}

		public virtual void Unschedule(TaskNodeInstanceBase node)
		{
			Assert(Parent != null, true);
			Parent.Unschedule(node);
		}


		/// THIS ABSOLUTLY HAS TO BE THE LAST CALL IN YOUR FUNCTION, NEVER MODIFY ANY STATE AFTER
		/// CALLING Stopped !!!!
		protected virtual void _OnStopped(bool success)
		{
			// Assert.AreNotEqual(currentState, State.INACTIVE, "The Node " + this + " called
			// 'Stopped' while in state INACTIVE, something is wrong! PATH: " + GetPath());
			Assert(State != NodeState.Inactive, "Called 'Stopped' while in state INACTIVE, something is wrong!", true);
			State = NodeState.Inactive;
			if (Parent != null)
			{
				Parent.OnChildStopped(this, success);
			}
		}

		public virtual void OnParentCompositeStopped(CompositeNodeInstanceBase composite)
		{
			_ParentCompositeStopped(composite);
		}

		/// THIS IS CALLED WHILE YOU ARE INACTIVE, IT's MEANT FOR DECORATORS TO REMOVE ANY PENDING OBSERVERS
		protected virtual void _ParentCompositeStopped(CompositeNodeInstanceBase composite)
		{
			/// be careful with this!
		}

		// public Composite ParentComposite { get { if (ParentNode != null && !(ParentNode is
		// Composite)) { return ParentNode.ParentComposite; } else { return ParentNode as Composite;
		// } } }

		override public string ToString()
		{
			return Name.IsNullOrEmpty() ? GetType().Name : Name;
		}

		protected string GetPath()
		{
			return Parent != null ? Parent.GetPath() + "/" + Name : Name;
		}
	}
}