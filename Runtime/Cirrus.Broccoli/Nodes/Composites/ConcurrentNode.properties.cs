using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cirrus.Collections;
using Cirrus.Objects;
using System;

namespace Cirrus.Broccoli 
{
	public partial class ConcurrentNodeInstance : CompositeNodeInstanceBase
	{
		// private Wait waitForPendingChildrenRule;
		public NodeResultQuantifier Failure = NodeResultQuantifier.One;
		public NodeResultQuantifier Success = NodeResultQuantifier.One;
		private int _runningCount = 0;
		private int _succeededCount = 0;
		private int _failedCount = 0;
		private Dictionary<NodeInstanceBase, bool> _childrenResults = new Dictionary<NodeInstanceBase, bool>();
		private bool _successState;
		private bool _areChildrenAborted;

		public Action<ConcurrentNodeInstance> InitCb;
	}
}