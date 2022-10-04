//using Cirrus.Arpg.Entities.AI;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.States;
//using Cirrus.States.AI;
//using Cirrus.Unity.States.AI;
////using Cirrus.States._ObsoleteAI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Cirrus.Arpg.AI
//{
//	public partial class HierarchicalAgentState
//	{
//		public override int ID => (int)ControlStateID.BehaviourTree;

//		public BehaviourTree<HierarchicalAgentState> BehaviourTree { get; set; }

//		public NodeBase Root => BehaviourTree.Root;

//		public int TickCount => BehaviourTree.TickCount;

//		private CharacterObject _character;

//		public CharacterObject Character => _character == null ?
//			_character = Context.Character :
//			_character;

//		public ObjectAI AI => Character.AI;

//		public SteeringComponent Steering => AI.Steering;

//		public CharacterControl Control => Character.Control;
//	}
//}
