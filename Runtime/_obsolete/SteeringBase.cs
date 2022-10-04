//using Cirrus.Unity.Objects;
//using Cirrus.Objects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public abstract class SteeringBase : IPrototype
//	{
//		public object Prototype { get; set; }

//		object ICloneable.Clone()
//		{
//			return MemberwiseClone();
//		}

//		void IPrototype.OnCloned()
//		{ 
//		}


//		public abstract void FixedUpdate(CharacterAI ai);

//	}

//	public abstract class WanderSteeringBase : SteeringBase
//	{
//		protected abstract Vector3 _GetSteering(CharacterAI ai);

//		public override void FixedUpdate(CharacterAI ai)
//		{
//			Vector3 accel = _GetSteering(ai);

//			context.SteeringSupport.Steer(accel);
//			context.SteeringSupport.LookWhereYoureGoing();
//		}				
//	}
//}
