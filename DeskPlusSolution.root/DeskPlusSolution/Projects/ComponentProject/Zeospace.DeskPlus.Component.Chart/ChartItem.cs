using System;
using System.Drawing;

namespace Zeospace.DeskPlus.Component.Chart
{
	/// <summary>
	/// ChartItem에 대한 요약 설명입니다.
	/// </summary>
	public class ChartItem
	{
		private string _label;
		private string _description;
		private float _value;
		private Color _color;
		private float _startPos;
		private float _sweepSize;

		private ChartItem()	{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="label"></param>
		/// <param name="desc"></param>
		/// <param name="data"></param>
		/// <param name="start"></param>
		/// <param name="sweep"></param>
		/// <param name="clr"></param>
		public ChartItem (string label, string desc, float data, float start, float sweep, Color clr)
		{
			_label = label;
			_description = desc;
			_value = data;
			_startPos = start;
			_sweepSize = sweep;
			_color = clr;
		}

		/// <summary>
		/// 
		/// </summary>
		public string Label 
		{
			get{ return _label; }
			set{ _label = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Description 
		{
			get{ return _description; }
			set{ _description = value; }
		} 

		/// <summary>
		/// 
		/// </summary>
		public float Value 
		{
			get{ return _value; }
			set{ _value = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Color ItemColor 
		{
			get{ return _color; }
			set{ _color = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public float StartPos
		{
			get{ return _startPos; }
			set{ _startPos = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public float SweepSize
		{
			get{ return _sweepSize; }
			set{ _sweepSize = value; }
		}
	}
}
