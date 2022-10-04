
//using Cirrus.Arpg.Content.AI;
//using Cirrus.Arpg.AI;
//using Cirrus.Reflection;
//using Cirrus.Unity.Objects;
//using UnityEditor;
//using UnityEngine;

//namespace Cirrus.Arpg.Content.AI
//{
//	// TODO there could be a way to tell which fields were dirty and assign it back
//	// after a reset so that the values are preserved

//	// TODO choose between show in inspector and always pull from library

//	public class CustomOptionComponent : ProviderComponent<OptionBase>
//	{
//		[SerializeField]
//		public AILibrary.ID _prototype = AILibrary.ID.Unknown;

//		public OptionBase Prototype => AILibrary.Instance.Get<OptionBase>((int)_prototype);

//		[SerializeField]
//		[SerializeReference]
//		public OptionBase _option;

//		protected override OptionBase _Create()
//		{
//			return UpdateContent();
//		}

//		public void ResetContent()
//		{
//			_option = AILibrary.Instance.Get<OptionBase>((int)_prototype);
//		}

//		public OptionBase UpdateContent()
//		{
//			_option.ReflectionUpdate(Prototype);
//			return _option;
//		}
//	}

//#if UNITY_EDITOR
//	[CustomEditor(typeof(CustomOptionComponent))]
//	public class OptionComponentEditor : Editor
//	{
//		private CustomOptionComponent _option;

//		public void OnEnable()
//		{
//			_option = target as CustomOptionComponent;
//		}

//		public override void OnInspectorGUI()
//		{
//			DrawDefaultInspector();

//			if (GUILayout.Button("Reset"))
//			{
//				_option.ResetContent();
//				EditorUtility.SetDirty(_option);
//			}

//			if (GUILayout.Button("Update"))
//			{
//				_option.ResetContent();
//				EditorUtility.SetDirty(_option);
//			}
//		}
//	}

//#endif

//}
