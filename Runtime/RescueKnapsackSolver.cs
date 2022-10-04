using Cirrus.Collections;
using Cirrus.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Arpg.AI
{
	public interface IKnapsackItem : IComparable
	{
		float Weight { get; }
		float Reward { get; }
	}	

	public class KnapsackResult
	{
		public float itemVals;
		public ArrayList<float[]> clause;
		public bool satisfiable;
		public float[][] a;
		public bool[] literals;
		public bool[][] b;
		public IKnapsackItem[] items;

		public KnapsackResult()
		{ 
		}

		public KnapsackResult(float itemsVal, ArrayList<IKnapsackItem> items)
		{
			itemVals = itemsVal;
			this.items = new IKnapsackItem[items.Length];
			for (int k = 0; k < items.Length; ++k)
			{
				this.items[k] = items[k];
			}
		}
	}

	// Greedy approximation solver for knapsack problem to confirm rescue effort..

	public class RescueKnapsackSolver
	{
		/**
		* Greedy approach to knapsack problem, sort items by value/weight and take until full
		* Find the item with the max value, if that value is higher than the accumulated
		*	value of the items taken, take that item instead
		*
		*	@param W maximum weight you can carry
		*	@param items list of items you can take
		**/
		public static KnapsackResult ModifiedGreedyKnapsack(float W, List<IKnapsackItem> items)
		{
			if (items.Count == 0) return ObjectUtils.Empty<KnapsackResult>();

			float w = W;
			int i = items.Count - 1;
			items.Sort();			

			ArrayList<IKnapsackItem> itemList = new ArrayList<IKnapsackItem>();
			while (w > 0 && i > -1)
			{
				if (items[i].Weight < w)
				{
					itemList.Add(items[i]);
					w -= items[i].Weight;
				}
				--i;
			}
			IKnapsackItem max = items[0];
			for (i = 1; i < items.Count; ++i)
			{
				if (items[i].Reward > max.Reward)
					max = items[i];
			}
			float itemVals = 0;
			for (i = 0; i < itemList.Length; ++i)
			{
				itemVals += itemList[i].Reward;
			}
			if (max.Reward > itemVals)
			{
				itemVals = max.Reward;
				itemList = new ArrayList<IKnapsackItem>();
				itemList.Add(max);
			}

			return new KnapsackResult(itemVals, itemList);
		}
	}
}
