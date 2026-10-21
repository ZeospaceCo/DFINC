using System;
using System.Drawing;
using System.Collections;

namespace Zeospace.DeskPlus.Component.Chart
{
	/// <summary>
	/// Gauge에 대한 요약 설명입니다.
	/// </summary>
	abstract public class Gauge
	{
		#region 변수
		private const int _colorLimit = 12;

		private Color[] _defaultColor = 
			{
				Color.Chocolate,
				Color.YellowGreen,
				Color.Olive,
				Color.DarkKhaki,
				Color.Sienna,
				Color.PaleGoldenrod,
				Color.Peru,
				Color.Tan,
				Color.Khaki,
				Color.DarkGoldenrod,
				Color.Maroon,
				Color.OliveDrab
			};

		private Color[] _blueColor = 
			{
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212),
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212),
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212),
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212),
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212),
				Color.FromArgb(0, 126, 255),
				Color.FromArgb(212, 212, 212)
			};

		private ChartItemsCollection _dataPoints = new ChartItemsCollection();

		/// <summary>
		/// Draw()
		/// </summary>
		/// <returns></returns>
		public abstract Bitmap Draw();
		#endregion 

		#region 생성자
		/// <summary>
		/// 생성자
		/// </summary>
		public Gauge()		{		}
		#endregion

		#region 프로퍼티
		/// <summary>
		/// 
		/// </summary>
		public ChartItemsCollection DataPoints
		{
			get
			{
				return _dataPoints;
			}

			set
			{
				_dataPoints = value;
			}
		}

		/// <summary>
		/// 그래프 내의 item 별 색상가져오기
		/// </summary>
		/// <param name="_section"></param>
		/// <param name="_index"></param>
		/// <returns></returns>
		public Color GetColor(string _section, int _index)
		{
			if(_index < _colorLimit)
			{
				switch (_section)
				{
					case "1" :
								//Blue Tone
								return this._blueColor[_index];
								//break;
					default :
								return this._defaultColor[_index];
								//break;
				}
			}
			else
			{
				throw new Exception("Color Limit is " + _colorLimit);
			}
		}
		#endregion
	}
}
