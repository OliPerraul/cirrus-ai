using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrus.Collections;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using Cirrus.Unity.Numerics;

using static Cirrus.FuncGenerics;
using UnityEngine;
using Cirrus.Animations;
using System.Xml.Linq;

namespace Cirrus.Arpg.AI
{
	public interface IArriveData
	{
		// Slow up speed until given radius (max speed outside outside that radius)
		float ArrivalRadius { get; set; } // :Number = 200;

		AnimationCurve ArrivalInterestCurve { get; set; }

		Range_ ArrivalInterest { get; set; }
	}

	//public partial class ArriveEvaluator : ArriveEvaluator<AiBehavtree>
	//{
	//	public ArriveEvaluator(IArriveData data) : base(data)
	//	{
	//	}
	//}

	public partial class ArriveEvaluator<TContext, TNodeData>
	: ContextEvaluatorBase<TContext, TNodeData>
	where TContext : AiBehavtree
	where TNodeData : ISteeringNodeData, IArriveData
	{
		//protected IArriveData _data;

		public ArriveEvaluator() : base()
		{
		}

		//protected override void _Start(TContext context, SteeringContextNodeInstance<TContext, ISteeringData> node)
		//{
		//}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, TNodeData> node)
		{
			VectorUtils.FromVelocity(context.destination - context.GroundPosition,
			out Vector3 direction,
			out float distance
			);
			float weight = Mathf.Clamp(distance / node.data.ArrivalRadius, 0, 1);
			weight = node.data.ArrivalInterestCurve.Evaluate(weight);
			weight = node.data.ArrivalInterest.Lerp(weight);

			for (int i = 0; i < _directions.Count; i++)
			{
				if (_directions[i].Dot(direction, out float dot))
				{
					_interests[i] = weight * dot;
				}
			}
		}
	}
}
