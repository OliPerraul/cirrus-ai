//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//namespace Cirrus.Broccoli {
//	public class RandomSelector : CompositeNode {
//		protected int current;

//		protected override void OnStart() {
//			current = Random.Range(0, Count);
//		}

//		protected override void OnStop() {
//		}

//		protected override State OnUpdate() {
//			var child = this[current];
//			return child.Update();
//		}
//	}
//}