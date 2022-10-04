using Cirrus.Broccoli;
using Cirrus.Arpg.AI;
using Cirrus.Numerics;
using Cirrus.Unity;
using Cirrus.Unity.Numerics;
using Cirrus.Unity.Randomness;

using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Cirrus.Arpg.Content.AI
{
	public class WanderNodeInstance
	: DecoratorInstanceBase
	, ISteeringNodeData
	{
		public override object Data { get => null; set { } }

		public float SteeringInterestEpsilon => ((ISteeringNodeData)_resource).SteeringInterestEpsilon;

		public float SteeringAvoidanceEpsilon => ((ISteeringNodeData)_resource).SteeringAvoidanceEpsilon;

		public float SteeringSpeedLerp => ((ISteeringNodeData)_resource).SteeringSpeedLerp;	

		private WanderNode _resource;

		public Vector3 start = Vector3.zero;
		public Vector3 direction = Vector3.zero;
		public NoiseInstance noise = null;

		public WanderNodeInstance(WanderNode resource)
		{
			_resource = resource;
		}
	}

	public class WanderNode
	: NodeBase
	, ISteeringNodeData
	{
		[SerializeField]
		public Range_ idleTime = new Range_(1, 2);

		[SerializeField]
		public SteeringNodesFlags wanderSteeringFlags;

		[SerializeField]
		public float wanderCircleRadius = 4f;

		[SerializeField]
		public float wanderLookAhead = 5;

		[SerializeField]
		public float wanderTurnSpeed = 0.25f * Mathf.PI;

		[SerializeField]
		public LayerMask wanderLayers;

		[SerializeField]
		public Noise wanderNoise;

		[SerializeField]
		public Range_ wanderTime = new Range_(1, 2);

		[field: SerializeField]
		public float SteeringInterestEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringAvoidanceEpsilon { get; set; } = 0.01f;

		[field: SerializeField]
		public float SteeringSpeedLerp { get; set; } = 25f;

		protected override NodeInstanceBase _CreateInstance()
		{
			return new WanderNodeInstance(this)
			{
				new SequenceNodeInstance
				{
					new InitNodeInstance<AiBehavtree, WanderNodeInstance>((context, node) =>
					{ 
					
					})
					, new TimeDecoratorInstance("Wander Time", wanderTime)
					{
						new ConcurrentNodeInstance("Wander")
						{
							!wanderSteeringFlags.Intersects(SteeringNodesFlags.Context) ? null :
							new SteeringNodeInstance<AiBehavtree, WanderNodeInstance>("Steering 1",
							(context, node) =>
							{
								node.data.noise = wanderNoise.CreateInstance(RandomManager.Instance.seed);
								node.data.start = context.CharacterObject.start;
							})
							{
								new ContextEvaluator<AiBehavtree, WanderNodeInstance>(
								(context, node, eval) =>
								{
									float delta = Time.fixedDeltaTime;

									if(!context.Control.locomotion.magnitude.Almost(0))
									{
										if(Physics.Raycast(context.Position, node.data.direction, out RaycastHit hit, wanderLookAhead, wanderLayers))
										{
											node.data.direction = hit.normal;
										}
									}
									if(node.data.direction.Almost(Vector3.zero))
									{
										node.data.direction = VectorUtils.ToDirection3(Random.Range(-Mathf.PI, Mathf.PI));
									}

									Vector3 displacement = node.data.start - context.Position;
									float outerRadius = 2.0f * wanderCircleRadius;
									float distanceWeight = Numerics.MathUtils.Normalize(displacement.magnitude, wanderCircleRadius, outerRadius);
									float angleDelta = node.data.direction.AngleBetween(displacement) * distanceWeight;
									////
									//// Change wanderAngle just a bit, so it
									//// won't have the same value in the
									//// next game frame.
									float n = node.data.noise.Noise1(Time.timeSinceLevelLoad);
									float wanderDelta = 1.0f * Mathf.PI * n * delta;
									angleDelta += wanderDelta;
									angleDelta = Numerics.MathUtils.Wrap(angleDelta, -Mathf.PI, Mathf.PI);
									
									var rotation = Mathf.Clamp(angleDelta, -wanderTurnSpeed * delta, wanderTurnSpeed * delta);
									node.data.direction = node.data.direction.GroundRotate(rotation);

									for(int i = 0; i < context.Steering.resolution.Count; i++)
									{
										eval._steering.interests[i] = 0;
										if(Vector3.Dot(context.Steering.resolution[i], node.data.direction).Out(out float dot) > 0)
										{
											eval._steering.interests[i] = dot;
										}
									}
								})
								//new WallsEvaluator(this),
							}
							, !wanderSteeringFlags.Intersects(SteeringNodesFlags.Context) ? null :
							new SteeringCombineNodeInstance<AiBehavtree, WanderNodeInstance>()
							, !wanderSteeringFlags.Intersects(SteeringNodesFlags.Locomotion) ? null :
							new SteeringLocomotionNodeInstance("Steering Locomotion", this)
							, !wanderSteeringFlags.Intersects(SteeringNodesFlags.Rotation) ? null :
							new SteeringRotationNodeInstance("Steering Rotation", this)
						}
					}
					, new WaitNodeInstance("Idle Time", idleTime)
				}
			};
		}
	}
}