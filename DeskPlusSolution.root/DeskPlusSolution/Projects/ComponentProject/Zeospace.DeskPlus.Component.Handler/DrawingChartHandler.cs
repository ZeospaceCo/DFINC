using System;
using System.Web;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Design;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// DrawingChartHandler에 대한 요약 설명입니다.
	/// </summary>
	public class DrawingChartHandler: IHttpHandler
	{
		private string xValues, yValues, chartType, print, styleSection;
		private bool boolPrint;

		/// <summary>
		/// 차트생성
		/// </summary>
		/// <param name="context"></param>
		public void CreateDrawingChart(HttpContext context)
		{
			if (chartType == null)
				chartType = "";

			if (print == null)
			{
				boolPrint = false;
			}
			else
			{
				try
				{
					boolPrint = Convert.ToBoolean(print);
				}
				catch
				{
					boolPrint = false;
				}
			}

			if(styleSection == null)
			{
				styleSection = "0";
			}

			if (xValues != null && yValues != null)
			{
				Color bgColor;
				Bitmap StockBitMap;

				if (boolPrint)
					bgColor = Color.White;
				else
					bgColor = Color.FromArgb(255,253,244);

				switch(chartType)
				{
					case "Bar" :
						Zeospace.DeskPlus.Component.Chart.GaugeBarGraph bar = new Zeospace.DeskPlus.Component.Chart.GaugeBarGraph();
						bar.VerticalLabel = "최대값";
						bar.VerticalTickCount = 5;
						bar.ShowLegend = true;
						bar.ShowData = false;
						bar.Height = 400;
						bar.Width = 700;
						bar.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()), styleSection);
						StockBitMap = new Bitmap(bar.Draw());
						StockBitMap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
						StockBitMap.Dispose();
						break;
					case "Pie" :
						Zeospace.DeskPlus.Component.Chart.GaugePie pie = new Zeospace.DeskPlus.Component.Chart.GaugePie(bgColor);
						pie.CollectDataPoints(xValues.Split("|".ToCharArray()), yValues.Split("|".ToCharArray()), styleSection);
						StockBitMap = new Bitmap(pie.Draw());
						StockBitMap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
						StockBitMap.Dispose();
						break;

					default :
						break;

				}
			}
		}

		/// <summary>
		/// ProcessRequest
		/// </summary>
		/// <param name="_context"></param>
		public void ProcessRequest(HttpContext _context)
		{
			chartType = _context.Request.QueryString["chartType"];
			//xValues = _context.Server.UrlDecode(_context.Request.QueryString["xValues"]);
			//yValues = _context.Server.UrlDecode(_context.Request.QueryString["yValues"]);
			xValues = _context.Request.QueryString["xValues"];
			yValues = _context.Request.QueryString["yValues"];
			print = _context.Request.QueryString["Print"];
			styleSection = _context.Request.QueryString["styleSection"];
			this.CreateDrawingChart(_context);
		}

		/// <summary>
		/// 다른 요청에서 <see cref="T:System.Web.IHttpHandler"/> 인스턴스를 사용할 수 있는지 여부를 나타내는 값을 가져옵니다.
		/// </summary>
		/// <value></value>
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

