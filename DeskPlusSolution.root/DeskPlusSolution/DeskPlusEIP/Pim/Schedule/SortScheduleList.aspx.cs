using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class SortScheduleList : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID;
        public int UserID;
        
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.SortScheduleList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            List<object> listobj = new List<object>();

            Ws.Schedule.Schedule _web = new Ws.Schedule.Schedule();

            listobj = _web.GetScheduleAllKind(GetParm("p",null));


            foreach (BE.ScheduleInfo _info in listobj)
            {
                ulList.InnerHtml += "<li><a onclick='parent.MoveScheduleView(" + _info.DisplayID + ")' style='cursor:hand' >(" + _info.KindDesk + ")&nbsp;" + _info.Title + "</a><div class=dotLine></div></li>";
                
            }
          
        }



        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            //CatID = GetDecryptParam("catID", "0").ToNumber<int>();

            string param = GetParm("p", "!NULL");
            
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClosetodo.Text = GetResource("Close");
                divPopTitle.InnerText = GetResource("SCHEDULECADALOG");
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }    
}