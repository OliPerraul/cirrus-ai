using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Cirrus.Broccoli;
using Cirrus.Collections;
using Cirrus.Arpg.Entities;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Objects;
using Cirrus.Unity.Numerics;

using UnityEngine;

using Void = Cirrus.Objects.None;

namespace Cirrus.Arpg.AI
{
	public interface ISeparationData
	{
		//List<CharacterComponent> Neighbours { get; set; }

		float SeparationDist { get; set; }
	}

	public class SeparationEvaluator<TContext>
		: ContextEvaluatorBase<TContext, ISteeringNodeData>
		where TContext : AiBehavtree

	{
		private List<int> _indices = new List<int>();

		private ISeparationData _resource;

		private List<CharacterInstanceBase> _characters = new List<CharacterInstanceBase>();

		public SeparationEvaluator(ISeparationData resource) : base(null)
		{
			_resource = resource;
		}

		protected override void _Start(TContext context, SteeringNodeInstance<TContext, ISteeringNodeData> node)
		{
			base._Start(context, node);

			_characters = context.Encounter.Get(_characters) ? _characters : ListUtils.Empty<CharacterInstanceBase>();
		}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, ISteeringNodeData> node)
		{
			_indices.Clear();
			_characters.Foreach(context, node, this, (context, node, eval, target, i) =>
			{
				if(
					target != context.EntityInst &&
					target.ColliderDistance(context.EntityInst) <= eval._resource.SeparationDist
					)
				{
					Vector3 direction = target.Position - context.Position;
					for(int j = 0; j < eval.Count; j++)
					{
						if(Vector3
							.Dot(eval.Direction(i), direction)
							.Out(out float dot) >= 0)
						{
							eval._steering.avoidances[i] += dot;
						}
					}
					eval._indices.Add(i);
				}
			});

			for (int i = 0; i < _indices.Count; i++)
			{
				int j = _indices[i];
				_steering.avoidances[j] /= _indices.Count;
			}
		}
	}

	public class SeparationEvaluator2<TContext>
	: ContextEvaluatorBase<TContext, ISteeringNodeData>
	where TContext : AiBehavtree
	{
		private List<int> _indices = new List<int>();

		private ISeparationData _resource;

		private List<CharacterInstanceBase> _characters = new List<CharacterInstanceBase>();

		public SeparationEvaluator2(ISeparationData resource) : base(null)
		{
			_resource = resource;
		}

		protected override void _Update(TContext context, SteeringNodeInstance<TContext, ISteeringNodeData> node)
		{
			var characters = context.Encounter.Get(_characters) ? _characters : ListUtils.Empty<CharacterInstanceBase>();
			_indices.Clear();
			for (int i = 0; i < characters.Count; i++)
			{
				var character = characters[i];
				if (
					character != context.Character &&
					character.ColliderDistance(context.CharacterInst) <= _resource.SeparationDist)
				{
					Vector3 direction = character.Position - context.Position;
					for (int j = 0; j < this._steering.directions.Count; j++)
					{
						if (Vector3
							.Dot(this._steering.directions[i], direction)
							.Out(out float dot) >= 0)
						{
							this._steering.avoidances[i] += dot;
						}
					}
					_indices.Add(i);
				}
			}

			for (int i = 0; i < _indices.Count; i++)
			{
				int j = _indices[i];
				this._steering.avoidances[j] /= _indices.Count;
			}
		}
	}
}
