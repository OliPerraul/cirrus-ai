using Cirrus.Unity.Editor;
using Cirrus.Unity.Numerics;
using Cirrus.Unity.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cirrus.Broccoli 
{
	public class WaitNodeInstance : TaskNodeInstanceBase
	{
		private Range_ _seconds = -1;

		public override object Data { get => null; set { } }

		public WaitNodeInstance(string name, Range_ seconds) : base(name)
		{
			_seconds = seconds;
		}

		public WaitNodeInstance(Range_ seconds)
		{
			_seconds = seconds;
		}

		public WaitNodeInstance(string name) : base(name)
		{
		}

		public WaitNodeInstance(string name, object obj) : base(name, obj)
		{
		}

		public WaitNodeInstance(object obj) : base(obj)
		{
		}


		protected override void _Init()
		{
		}

		protected override void _Start()
		{
			Clock.AddTimer(_seconds.Random(), 0, _OnTimer);
		}

		protected override void _Stop()
		{
			Clock.RemoveTimer(_OnTimer);
			_OnStopped(false);
		}

		private void _OnTimer()
		{
			Clock.RemoveTimer(_OnTimer);
			_OnStopped(true);
		}
	}

	public partial class WaitNode : ResourceAssetBase<NodeInstanceBase>
	{
#if UNITY_EDITOR
		//[MenuItem("Assets/Create/Cirrus.Broccoli/Nodes/Wait Node", false, priority = PackageUtils.MenuItemAssetFrameworkPriority)]
		//public static void CreateAsset() => EditorScriptableObjectUtils.CreateAsset<WaitNode>();
#endif

		[SerializeField]
		private Range_ _waitSeconds = new Range_(2, 4);

		protected override NodeInstanceBase _CreateInstance()
		{
			return new WaitNodeInstance(_waitSeconds);
		}
	}
}
