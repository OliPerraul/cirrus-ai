//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.States;
//using Cirrus.States.AI;
//using Cirrus.Unity.States.AI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Cirrus.Arpg.AI
//{
//	public partial class HierarchicalAgentState 
//		: CharacterControlStateBase
//		, IBehaviourTree
//	{
//		NodeBase IBehaviourTree.Root => BehaviourTree.Root;

//		public HierarchicalAgentState(
//			CharacterControlStateMachine machine) 
//			: base(machine)
//		{ 
//		}

//		public override StateMachineStatus Enter()
//		{
//			return StateMachineStatus.Success;
//		}

//		public override StateMachineStatus Exit()
//		{
//			return StateMachineStatus.Exited;
//		}

//		public override StateMachineStatus Update_()
//		{
//			return BehaviourTree.Update_();
//		}

//		public override void FixedUpdate_()
//		{
//			BehaviourTree.FixedUpdate_();
//		}

//		public override void LateUpdate_()
//		{
//			BehaviourTree.LateUpdate_();
//		}

//		public override void CustomUpdate1(float deltaTime)
//		{
//			BehaviourTree.CustomUpdate1(deltaTime);
//		}

//		public override void CustomUpdate2(float deltaTime)
//		{
//			BehaviourTree.CustomUpdate2(deltaTime);
//		}

//		public override void CustomUpdate3(float deltaTime)
//		{
//			BehaviourTree.CustomUpdate3(deltaTime);
//		}


//		public void AddActiveNode(NodeBase node)
//		{
//			BehaviourTree.AddActiveNode(node);
//		}

//		public void RemoveActiveNode(NodeBase node)
//		{
//			BehaviourTree.RemoveActiveNode(node);
//		}
//	}
//}
