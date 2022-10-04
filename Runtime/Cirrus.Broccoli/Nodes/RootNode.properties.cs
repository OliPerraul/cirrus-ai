using Cirrus.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cirrus.Broccoli 
{

	public partial class RootNodeInstance
		: NodeInstanceBase
		, IEnumerable<NodeInstanceBase>
	{
		//private Node inProgressNode;

		protected BehavtreeContextBase _context;
		public override BehavtreeContextBase Context => _context;

		public override object Data { get => null; set { } }

		protected NodeInstanceBase _child;

		public NodeInstanceBase Child
		{
			get => _child;
			set => _AddChild(value);
		}

		public override RootNodeInstance Root
		{
			get => this;
			set { }
		}	
	}
}