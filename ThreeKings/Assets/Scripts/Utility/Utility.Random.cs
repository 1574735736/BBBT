
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LieyouFramework
{
	public static partial class Utility
	{
		/// <summary>
		/// 随机相关的实用函数。
		/// </summary>
		public static class Random
		{
			private static System.Random s_Random = new System.Random((int)System.DateTime.Now.Ticks);

			/// <summary>
			/// 设置随机数种子。(随机表Index只能从头开始)
			/// </summary>
			/// <param name="seed">随机数种子。</param>
			public static void SetSeed(int seed)
			{
				s_Random = new System.Random(seed);
			}

			/// <summary>
			/// 返回非负随机数。
			/// </summary>
			/// <returns>大于等于零且小于 System.Int32.MaxValue 的 32 位带符号整数。</returns>
			public static int GetRandom()
			{
				return s_Random.Next();
			}

			/// <summary>
			/// 返回一个小于所指定最大值的非负随机数。
			/// </summary>
			/// <param name="maxValue">要生成的随机数的上界（随机数不能取该上界值）。maxValue 必须大于等于零。</param>
			/// <returns>大于等于零且小于 maxValue 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 maxValue。不过，如果 maxValue 等于零，则返回 maxValue。</returns>
			public static int GetRandom(int maxValue)
			{
				return s_Random.Next(maxValue);
			}

			/// <summary>
			/// 返回一个指定范围内的随机数。
			/// </summary>
			/// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
			/// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。maxValue 必须大于等于 minValue。</param>
			/// <returns>一个大于等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。如果 minValue 等于 maxValue，则返回 minValue。</returns>
			public static int GetRandom(int minValue, int maxValue)
			{
				return s_Random.Next(minValue, maxValue);
			}

			/// <summary>
			/// 返回一个介于 0.0 和 1.0 之间的随机数。
			/// </summary>
			/// <returns>大于等于 0.0 并且小于 1.0 的双精度浮点数。</returns>
			public static double GetRandomDouble()
			{
				return s_Random.NextDouble();
			}

			/// <summary>
			/// 乱序一个List
			/// </summary>
			/// <param name="_list">要乱序的List</param>
			public static void DisorderList<T>(List<T> _list)
			{
				for (int i = 0; i < _list.Count; i++)
				{
					int index0 = GetRandom(0, _list.Count);
					int index1 = GetRandom(0, _list.Count);

					T temp = _list[index0];
					_list[index0] = _list[index1];
					_list[index1] = temp;
				}
			}

			/// <summary>
			/// 从数组中随机获取几个元素的索引
			/// </summary>
			/// <param name="list">要获取的数组</param>
			/// <param name="count">要获取的数量</param>
			/// <returns>结果的索引列表</returns>
			public static List<int> GetRadoms<T>(T[] list, int count)
			{
				List<int> needList = new List<int>();
				if (list.Length <= count)
				{
					Debug.LogWarning("get error count > list.Length");
					return needList;
				}

				List<int> indexs = new List<int>();
				for (int i = 0; i < list.Length; i++)
				{
					indexs.Add(i);
				}

				int GetIndex()
				{
					int index = GetRandom(0, indexs.Count);
					return indexs[index];
				}

				for (int i = 0; i < count; i++)
				{
					int index = GetIndex();
					indexs.Remove(index);
					needList.Add(index);
				}
				return needList;
			}

			//==================================权重和概率通过==================================

			/// <summary>
			/// 给定权重数组，获取区域索引
			/// </summary>
			/// <param name="weights">权重数组</param>
			/// <param name="sum">权重总和，避免每次调用都求和</param>
			/// <returns>返回区域索引</returns>
			public static int GetWeight(int[] weights, int sum = -1)
			{
				if (sum == -1)
				{
					sum = weights.Sum();
				}
				int tw = 0;
				int index = GetRandom(0, sum);
				for (int i = 0; i < weights.Length; i++)
				{
					int w = weights[i];
					if (index >= tw && index < (tw + w))
					{
						return i;
					}
					tw += w;
				}
				Debug.LogWarning("There was an error calculating the weight");
				return -1;
			}

			/// <summary>
			/// 是否通过概率
			/// </summary>
			/// <param name="probability">概率值范围:0-100</param>
			/// <returns>是否通过</returns>
			public static bool PassedProbability(int probability)
			{
				int index = GetRandom(0, 100);
				bool isIn = index < probability;
				return isIn;
			}

			/// <summary>
			/// 是否通过概率
			/// </summary>
			/// <param name="probability">概率值范围:0-100</param>
			/// <returns>是否通过</returns>
			public static bool PassedProbability(float probability)
			{
				float index = UnityEngine.Random.Range(0, 100.00f);
				bool isIn = index < probability;
				return isIn;
			}
		}
	}
}
