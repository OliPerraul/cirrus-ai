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
	// Doubles as seek evaluator
	public interface ISeekData
	{
		//float SeekContextWeight { get; }
	}

	public partial class SeekEvaluator<TContext, TNodeData>
	: ContextEvaluatorBase<TContext, TNodeData>
	where TContext : AiBehavtree
	where TNodeData : ISteeringNodeData
	{
		public SeekEvaluator(Func<TContext, SteeringNodeInstance<TContext, TNodeData>, ContextEvaluatorBase<TContext, TNodeData>, float> weightCb)
		: base(weightCb)
		{
		}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, TNodeData> node)
		{
			Vector3 displ = context.target.Position - context.Position;
			for(int i = 0; i < _steering.directions.Count; i++)
			{
				if(displ.Dot(_steering.directions[i], out float weight))
				{
					_steering.interests[i] = weight;
				}
			}
		}
	}
}
