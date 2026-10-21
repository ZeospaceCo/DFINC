using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Design;

namespace Zeospace.DeskPlus.Component.Handler
{
	/// <summary>
	/// ThumbnailHandler에 대한 요약 설명입니다.
	/// </summary>
	public class ThumbnailHandler : IHttpHandler
	{
		/// <summary>
		/// Creates the thumbnail.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="source">The source.</param>
		/// <param name="thumbnailSize">Size of the thumbnail.</param>
		public void CreateThumbnail(HttpContext context, string source, int thumbnailSize)
		{
			int num1;
			int num2;
			Bitmap bitmap1;
			System.Drawing.Image image1 = System.Drawing.Image.FromFile(source);
			float single1 = image1.Width;
			float single2 = image1.Height;
			if (Math.Max(single1, single2) > thumbnailSize)
			{
				float single3;
				if (single1 > single2)
				{
					single3 = Convert.ToSingle((float) (single2 / single1));
					num1 = thumbnailSize;
					num2 = Convert.ToInt32((float) (thumbnailSize * single3));
					goto Label_0069;
				}
				single3 = Convert.ToSingle((float) (single1 / single2));
				num1 = Convert.ToInt32((float) (thumbnailSize * single3));
				num2 = thumbnailSize;
			}
			else
			{
				num1 = Convert.ToInt32(single1);
				num2 = Convert.ToInt32(single2);
			}
			Label_0069:
				bitmap1 = new Bitmap(image1, num1, num2);
			bitmap1.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
			bitmap1.Dispose();
			image1.Dispose();
		}
 

		/// <summary>
		/// 	<see cref="T:System.Web.IHttpHandler"/> 인터페이스를 구현하는 사용자 지정 <see langword="HttpHandler"/>를 사용하여 HTTP 웹 요청을 처리할 수 있도록 합니다.
		/// </summary>
		/// <param name="context">내부 서버 개체(예: <see langword="Request"/>, <see langword="Response"/>, <see langword="Session"/> 및 <see langword="Server"/>)<see langword=""/>에 대한 참조를 제공하는 <see cref="T:System.Web.HttpContext"/> 개체는 HTTP 요청을 지원하는 데 사용됩니다.</param>
		public void ProcessRequest(HttpContext context)
		{
			string imageURL = context.Server.MapPath(context.Request["src"].ToString());
			int imageSize = Convert.ToInt32(context.Request["size"]);

			if (File.Exists(imageURL))
			{
				this.CreateThumbnail(context, imageURL, imageSize);
			}
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
