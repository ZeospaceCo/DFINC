using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Opinion.Web.Admin.Content
{
    public partial class VoteList : Base.BasePageOpinion
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int QuesID = 0;
        public int UnitID = 0;
        public int UnitItemID = 0;
        public string UnitItemCont = "";
        public string Type = "";
        public int TotCnt = 0;    //총 게시물수        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";           //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.VoteList).Assembly;	//어셈블리 선언
            #endregion

            //PageSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.OPINION, "ListCount", "10").ToNumber<int>();
            PageSize = 5;

            if (!IsPostBack)
            {
                SetLanguage();  // 다국어처리
                LoadParm();     // 파라미터처리

                //총 리스트 수
                TotCnt = this.GetListCnt(CurrentUserInfo.CompanyID, QuesID, UnitID, UnitItemID, UnitItemCont);
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion

        #region (함수)

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //Title

                //Button
                this.cbtnClose.Text = this.GetResource("BtnClose");
            }
        }

        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            QuesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            UnitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            UnitItemID = GetDecryptParam("unitItemID", "0").ToNumber<int>();
            TotCnt = GetDecryptParam("totCnt", "0").ToNumber<int>();
            PageNo = GetDecryptParam("pageNo", "1").ToNumber<int>();
            UnitItemCont = GetDecryptParam("unitItemCont", "");
            Type = GetDecryptParam("type", "");
        }

        /// <summary>
        /// 리스트 총개수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        private int GetListCnt(int _companyID, int _quesID, int _unitID, int _unitItemID, string _unitItemCont)
        {
            DAL.Vote vdal = new DAL.Vote();
            if (Type == "")
            {
                // A:단일선택형, B:다중선택형
                return vdal.GetVotePageListAdmin(_companyID, _quesID, _unitID, _unitItemID);
            }
            else if (Type == "D")
            {
                // D: 다중가점형
                return vdal.GetVotePageListAdminD(_companyID, _quesID, _unitID, _unitItemID, _unitItemCont);
            }
            else
            {
                // C: 단일가점형, E: 주관식
                return vdal.GetVotePageListAdminC(_companyID, _quesID, _unitID, _unitItemCont);
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}