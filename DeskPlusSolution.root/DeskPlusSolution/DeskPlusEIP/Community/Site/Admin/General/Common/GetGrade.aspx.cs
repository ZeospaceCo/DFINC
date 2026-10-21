using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.General.Common
{
    public partial class GetGrade : Base.BasePageCommunitySysop
    {
        #region 변수설정
        public int CommunityID;
        public int GroupID;

        public string NameBase = "";
        public string NameKo = "";
        public string NameEn = "";
        public string NameJa = "";
        public string NameFr = "";
        public string NameZhcn = "";
        public string NameZhtw = "";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.General.Common.GetGrade).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetGradeName();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-21
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-30
        /// 작성목적 : 등급명, 다국어 바인딩
        /// </summary>
        private void GetGradeName()
        {
            DAL.UserGroup dal = new DAL.UserGroup();

            DataTableReader dtr = dal.GetUserGroup(CurrentUserInfo.CompanyID, CommunityID, GroupID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        NameBase = dtr["NameBase"].ToString().ToReverseXSS();
                        NameKo = dtr["NameKo"].ToString().ToReverseXSS();
                        NameEn = dtr["NameEn"].ToString().ToReverseXSS();
                        NameJa = dtr["NameJa"].ToString().ToReverseXSS();
                        NameFr = dtr["NameFr"].ToString().ToReverseXSS();
                        NameZhcn = dtr["NameZhcn"].ToString().ToReverseXSS();
                        NameZhtw = dtr["NameZhtw"].ToString().ToReverseXSS();

                        // 회원등급명
                        tbGradeName.Text = NameBase;

                        NameBase = Regex.Replace(NameBase, "</script>", "", RegexOptions.IgnoreCase);
                        NameKo = Regex.Replace(NameKo, "</script>", "", RegexOptions.IgnoreCase);
                        NameEn = Regex.Replace(NameEn, "</script>", "", RegexOptions.IgnoreCase);
                        NameJa = Regex.Replace(NameJa, "</script>", "", RegexOptions.IgnoreCase);
                        NameFr = Regex.Replace(NameFr, "</script>", "", RegexOptions.IgnoreCase);
                        NameZhcn = Regex.Replace(NameZhcn, "</script>", "", RegexOptions.IgnoreCase);
                        NameZhtw = Regex.Replace(NameZhtw, "</script>", "", RegexOptions.IgnoreCase);

                        // 다국어
                        ctbNameKo.Text = NameKo;
                        ctbNameEn.Text = NameEn;
                        ctbNameJa.Text = NameJa;
                        ctbNameFr.Text = NameFr;
                        ctbNameZhcn.Text = NameZhcn;
                        ctbNameZhtw.Text = NameZhtw;
                    }
                }
            }
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}