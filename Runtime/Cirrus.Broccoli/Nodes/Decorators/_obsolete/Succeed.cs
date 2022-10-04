//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Cirrus.Broccoli {
//	public class Succeed : DecoratorNode {
//		protected override void OnStart() {
//		}

//		protected override void OnStop() {
//		}

//		protected override State OnUpdate() {
//			var state = Child.Update();
//			if (state == State.Failure) {
//				return State.Success;
//			}
//			return state;
//		}
//	}
//}