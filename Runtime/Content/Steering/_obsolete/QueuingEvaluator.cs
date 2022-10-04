//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//// using Cirrus.Unity.AI.BehaviourTrees;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Arpg.UI;
//using Cirrus.Objects;
//using Cirrus.Unity.Objects;

//using UnityEngine;
//using Cirrus.Unity.Numerics;
//using UnityEngine.InputSystem.Utilities;

//using Void = Cirrus.Objects.None;

//namespace Cirrus.Arpg.AI
//{
//	public partial class QueuingEvaluator<TData> 
//		: ContextEvaluatorBase<TData>
//		where TData : IQueuingData
//	{
//		public CharacterComponent GetNeighborAhead(ControlBt context, SteeringNodePhase1<TData> node)
//		{
//			var characters = context.Encounter.Get<CharacterInstance>();

//			CharacterComponent ret = null;
//			Vector3 velocity = context.LocomotionVelocity;
//			Vector3 qa = velocity.normalized * node.Data.MaxQueueAhead;
//			Vector3 ahead = qa + context.Position;
//			for(int i = 0; i < characters.Count; i++)
//			{
//				CharacterInstance neighbour = characters[i];
//				if(neighbour == context.Character) continue;
//				float d = (ahead - neighbour.Position).magnitude;
//				if(d <= node.Data.MaxQueueRadius)
//				{
//					ret = characters[i];
//					break;
//				}
//			}

//			return ret;
//		}

//		public override bool Update(
//			ControlBt context, 
//			SteeringNodePhase1<TData> node
//			)
//		{
//			CharacterComponent neighbour = GetNeighborAhead(context, node);

//			if(neighbour != null)
//			{
//				for(int i = 0; i < Context.Directions.Count; i++)
//				{
//					Vector3 between = context.Position - neighbour.Position;
//					if(
//						Context.Directions[i].Dot(between, out float dot) &&
//						between.magnitude <= node.Data.MaxQueueRadius
//						)
//					{
//						Context.Avoidances[i] = dot;
//					}
//				}
//			}

//			return true;
//		}
//	}
//}
