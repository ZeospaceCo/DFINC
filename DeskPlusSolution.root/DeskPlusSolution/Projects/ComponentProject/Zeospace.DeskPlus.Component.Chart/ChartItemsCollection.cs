using System;
using System.Collections;

namespace Zeospace.DeskPlus.Component.Chart
{
	/// <summary>
	/// ChartItemsCollection에 대한 요약 설명입니다.
	/// </summary>
	public class ChartItemsCollection : CollectionBase
	{
		/// <summary>
		/// 
		/// </summary>
		public ChartItem this[int index] 
		{
			get{ return (ChartItem)(List[index]); }
			set{ List[index] = value; }
		}
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(ChartItem value) 
		{
			return List.Add(value);
		}
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(ChartItem value) 
		{
			return List.IndexOf(value);
		}
 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(ChartItem value) 
		{
			return List.Contains(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(ChartItem value) 
		{
			List.Remove(value);
		}
	}
}
