//using System;
//using System.Collections.Generic;
//using System.Xml.Linq;

//using Cirrus.Arpg.Conditions;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using Cirrus.Objects;

//namespace Cirrus.Arpg.AI
//{
//	// public class SteeringEvaluatorBase : SteeringEvaluatorBase<object>
//	// { 
//	// }

//	public class EvaluatorBase<TData>
//	{
//		public static implicit operator List<EvaluatorBase<TData>>(EvaluatorBase<TData> t)
//		{
//			return new List<EvaluatorBase<TData>> { t };
//		}

//		public static implicit operator EvaluatorBase<TData>[](EvaluatorBase<TData> t)
//		{
//			return new EvaluatorBase<TData>[] { t };
//		}


//		/// <summary>
//		/// </summary>
//		/// <param name="steering"></param>
//		/// <param name="state"></param>
//		/// <param name="data"></param>
//		/// <param name="deltaTime"></param>
//		/// <returns>False: continue postprocessing, True: break</returns>
//		public virtual bool Update(
//			ControlBt context,
//			SteeringNodePhase1<TData> node,
//			float deltaTime
//			)
//		{
//			return false;
//		}
//	}

//	public class Evaluator<TData> : EvaluatorBase<TData>
//	{
//		public Func<ControlBt, SteeringNodePhase1<TData>, float, bool> UpdateCb;

//		public static implicit operator Evaluator<TData>(Func<ControlBt, SteeringNodePhase1<TData>, float, bool> fn)
//		{
//			return new Evaluator<TData>(fn);
//		}

//		public Evaluator(Func<ControlBt, SteeringNodePhase1<TData>, float, bool> fn)
//		{
//			UpdateCb = fn;
//		}

//		public override bool Update(ControlBt context, SteeringNodePhase1<TData> node, float deltaTime)
//		{
//			return UpdateCb.Invoke(context, node, deltaTime);
//		}
//	}
//}