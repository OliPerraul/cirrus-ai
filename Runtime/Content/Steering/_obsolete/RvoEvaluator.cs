//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//// using Cirrus.Unity.AI.BehaviourTrees;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;
//using Cirrus.Unity.Objects;

//using UnityEngine;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public interface IRvoData
//	{		
//	}

//	public partial class RvoEvaluator<TData>
//		: ContextEvaluatorBase<TData>
//		where TData : IRvoData
//	{
//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			Vector3 delta=  context.Ai.Rvo.CalculateMovementDelta(context.GroundPosition, Time.deltaTime);
//			return true;
//		}

//	}
//}
