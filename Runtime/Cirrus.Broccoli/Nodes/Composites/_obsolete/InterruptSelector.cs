//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Cirrus.Broccoli {
//	public class InterruptSelector : Selector {
//		protected override State OnUpdate() {
//			int previous = current;
//			base.OnStart();
//			var status = base.OnUpdate();
//			if (previous != current) {
//				if (this[previous].state == State.Running) {
//					this[previous].Abort();
//				}
//			}

//			return status;
//		}
//	}
//}