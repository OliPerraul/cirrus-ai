//using Cirrus.States;
//using Cirrus.Unity.AI;
//using Cirrus.Unity.Numerics;
//using System.Collections;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	//https://gamedev.stackexchange.com/questions/163250/ai-world-exploration
//	// https://github.com/Unity-Technologies/NavMeshComponents/issues/126

//	// Chunks can have multiple point of interest
//	// An area is fully explored once all the points of interests are explored
//	// TODO : Use navmesh exploration (exhaustive search) if point of interests are not enough

//	// NOTE
//	//.OrderBy(x => (x.Position - _Character.Position).magnitude)
//	//.ThenBy(x => Vector3.Dot((x.Position - _Character.Position).normalized, _Character.Transform.forward));
//	// The point of OrderBy is to provide the "most important" ordering projection; then use ThenBy (repeatedly) to specify secondary, tertiary etc ordering projections.


//	public partial class ExploreState : UtilityAgentState
//	{
//		public ExploreState() : base()
//		{
//		}

//		// NONONO 
//		// Instead only navigate to adjacent nodes
//		//public bool GetNextExplorationNode1(out ExplorationNode area)
//		//{
//		//	//var areas = Layout.Instance.Sections.OrderBy(x => -x.Weight).ToArray();
//		//	//	// Then by Largest area first
//		//	//	.ThenBy(x => -x.Area).ToArray();

//		//	// TODO closes and on same level
//		//	area = ExplorationGraphComponent.Instance.Nodes
//		//		.OrderBy(x => -x.Weight)
//		//		.ThenBy(x => x.Height)
//		//		// Order by in ascending order of closest area
//		//		.ThenBy(x => (x.Position - CharacterObject.Position).magnitude)
//		//		//.ThenBy(x => -x.Weight)
//		//		// Then by Largest area first
//		//		.ThenBy(x => -x.Area)
//		//		.First(x => !Agent.Model.EpisodicKnowledge.VisitedSections[x.ID]);
//		//	return true;
//		//	//area = Agent.Model.WorkingMemory.NextArea;
//		//	//return true;

//		//	//Layout.Instance.Areas.Max()
//		//	//Agent.Model.WorkingMemory.NextArea = 
//		//	//Agent.Model.EpisodicKnowledge.Areas.

//		//}

//		//public void VisitNeighbourhoud(ExplorationNode node, ExplorationNode next)
//		//{
//		//	Agent.EpisodicKnowledge.VisitedNodes[CurrentNode.ID] = true;
//		//	foreach(var i in node.Neighbours)
//		//	{
//		//		if(ExplorationGraph.Nodes[i] == next) continue;
//		//		Agent.EpisodicKnowledge.VisitedNodes[i] = true;
//		//	}
//		//}

//		public bool GetNextNode(out ExplorationNode next)
//		{
//			float max = float.MinValue;
//			next = null;
//			var nodes = ExplorationGraph.Nodes;
//			foreach(var i in AgentModel.NextNode.Neighbours)
//			{
//				var node = nodes[i];

//				int visited = 0;
//				int opened = 0;
//				foreach(var j in node.Neighbours)
//				{
//					//if(Agent.EpisodicKnowledge.VisitedNodes[j])
//					//	visited++;
//					//else
//					//	opened++;
//				}

//				float utility = 0;
//				//float utility =
//				//	node.Weight * NodeWeightScalar +
//				//	visited * VisitedNeighboursScalar +
//				//	opened * OpenedNeighboursScalar +
//				//	-node.Height +
//				//	-(node.Position - CharacterObject.Position).magnitude +
//				//	node.Area +
//				//	(Agent.EpisodicKnowledge.VisitedNodes[i] ? -AlreadyExploredCost : 0);
//				if(utility > max)
//				{
//					next = node;
//					max = utility;
//				}
//			}

//			return next != null;
//		}

//		public bool Reroute()
//		{
//			if(GetNextNode(out ExplorationNode next))
//			{
//				if(NavMeshUtils.GetRandomPointAround(
//					 next.Position,
//					 DestinationEpsilon,
//					 out _destination))
//				{
//					if(CurrentNode != null)
//					{
//						//VisitNeighbourhoud(CurrentNode, next);
//					}
//					AgentModel.NextNode = next;
//					_Agent.NavMeshAgent.SetDestination(_destination);
//					return true;
//				}
//			}

//			return false;
//		}

//		protected override IEnumerator _CalculatePathCoroutine(float waitTime)
//		{
//			while(true)
//			{
//				// TODO
//				//if(CharacterObject.Position.Approximately(
//				//	_destination, 
//				//	ArrivalTolerance))
//				//{
//				//	Reroute();

//				//	yield return new WaitForSeconds(waitTime);
//				//}

//				yield return new WaitForSeconds(waitTime);
//			}
//		}


//		public override StateMachineStatus Enter(params object[] args)
//		{
//			base.Enter(args);

//			float min = float.MaxValue;
//			for(int i = 0; i < ExplorationGraph.Nodes.Count; i++)
//			{
//				float dist = (ExplorationGraph.Nodes[i].Position - _Character.Position).magnitude;
//				if(dist < min)
//				{
//					min = dist;
//					AgentModel.NextNode = ExplorationGraph.Nodes[i];
//				}
//			}

//			return
//				Reroute() ?
//				StateMachineStatus.Success :
//				StateMachineStatus.EnteredFailed;
//		}

//		public void OnTimeout()
//		{

//		}

//		public override void Update_()
//		{
//			base.Update_();

//			if(!_Character.Position.IsApproximately(
//				_destination,
//				ArrivalTolerance))
//			{
//				var dir = (_destination - _Character.Position).normalized;
//				_Control.DesiredAxes.Right = new Vector2(dir.x, dir.z);
//				UpdateMovement();
//			}
//			else
//			{
//				Reroute();
//			}
//		}


//		public override StateMachineStatus Exit()
//		{
//			StopCalculatingPath();

//			//if(_ability != null) _ability.OnEndLagFinishedHandler -= OnAbilityEndLagFinished;
//			AgentModel.NextNode = null;
//			_Control.Axes.Left = Vector2.zero;
//			_Control.DesiredAxes.Left = Vector2.zero;
//			_Control.DesiredAxes.Right = Vector2.zero;

//			base.Exit();
//			return StateMachineStatus.Exited;
//		}

//	}
//}
