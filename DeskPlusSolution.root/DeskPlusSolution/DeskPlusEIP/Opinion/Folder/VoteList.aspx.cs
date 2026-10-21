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

namespace Zeospace.DeskPlus.Opinion.Web.Folder
{
    public partial class VoteList : Base.BasePageOpinion
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int QuesID = 0;
        public int TotCnt = 0;    //총 게시물수        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Folder.VoteList).Assembly;	//어셈블리 선언
            #endregion

            //PageSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.OPINION, "ListCount", "10").ToNumber<int>();
            PageSize = 5;

            if (!IsPostBack)
            {
                SetLanguage();  // 다국어처리
                LoadParm();     // 파라미터처리

                //총 리스트 수
                TotCnt = this.GetListCnt(CurrentUserInfo.CompanyID, QuesID);
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
            TotCnt = GetDecryptParam("totCnt", "0").ToNumber<int>();
            PageNo = GetDecryptParam("pageNo", "1").ToNumber<int>();
        }

        /// <summary>
        /// 리스트 총개수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        private int GetListCnt(int _companyID, int _quesID)
        {
            DAL.Vote vdal = new DAL.Vote();
            return vdal.GetVotePageList(_companyID, _quesID);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}