using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Zeospace.DeskPlus.Base.Web.Html.ErrorMessage
{
	/// <summary>
	/// ErrorDefault에 대한 요약 설명입니다.
	/// </summary>
    public partial class ErrorDefault : Base.BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			#region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Base.Web.Resource.GlobalString.string";			//리소스 명 선언
			this.AssemblyName = typeof(Zeospace.DeskPlus.Base.Web.Html.ErrorMessage.ErrorDefault).Assembly;		//어셈블리 선언
			#endregion

			#region if (!IsPostBack) - 폼필드 / 컨트롤 초기화(다국어처리)
			if(!IsPostBack)
			{
			}
			#endregion
		}

		#region Web Form 디자이너에서 생성한 코드
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
