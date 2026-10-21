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
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Main
{
    public partial class Join : Base.BasePageCommunity
    {
        #region 변수설정
        public int Type;
        public int JoinType;
        public int CommunityID;
        public int JoinCheck = 0;
        public int StatusCheck = 0;

        private string eventArgs = String.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Main.Join).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                LoadJoinCheck();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 가입할 커뮤니티 정보
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.Com dal = new DAL.Com();
            DataTableReader dtr = dal.GetComunifyInfo(CurrentUserInfo.CompanyID, CommunityID);

            DateTime dt;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        // 개설자 이름
                        lbOpenName.Text = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(), dtr["NameZhTw"].ToString(),PreLang);
                        // 회원수
                        lbMembership.Text = dtr["UserCnt"].ToString();
                        
                        // 가입유형
                        lbType.Text = GetType(dtr["Type"].ToString());
                        // 개설신청일
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            lbOpenDate.Text = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            lbOpenDate.Text = string.Empty;

                        // 커뮤니티명
                        lbCmnName.Text = dtr["Name"].ToString();
                        // 커뮤니티 소개
                        lbDescription.Text = dtr["Description"].ToString();

                        Type = dtr["Type"].ToString().ToNumber<int>();

                        // 가입유형이 퀴즈일 경우
                        if (Type == 3)
                        {
                            // 퀴즈
                            lbQuestion.Text = dtr["Question"].ToString();
                            this.cbtnConfirm.Text = GetResource("JoinActive1");
                        }

                        // 회원가입방식
                        JoinType = dtr["JoinType"].ToString().ToNumber<int>();

                        // 회원가입방식에 따른 버튼 설명 변경
                        if (JoinType == 1)
                        {
                            this.cbtnConfirm.Text = GetResource("JoinActive1");
                        }
                    }
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 20112-05-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                this.cbtnConfirm.Text = resMng.GetString("CMNJoin");
                this.cbtnClose.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-10
        /// 작성목적 : 가입유형(1:공개,2:비공개,2:퀴즈)
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <returns></returns>
        private string GetType(string _type)
        {
            string typeName = string.Empty;

            switch (_type)
            {
                case "1":
                    typeName = GetResource("Type1");
                    break;
                case "2":
                    typeName = GetResource("Type2");
                    break;
                case "3":
                    typeName = GetResource("Type3");
                    break;
                default:
                    typeName = "";
                    break;
            }

            return typeName;
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-13
        /// 작성목적 : 가입신청 여부 체크
        /// </summary>
        private void LoadJoinCheck()
        {
            #region 1. 가입신청 했는지 여부 확인

            DAL.UserApply dalApply = new DAL.UserApply();
            DataTableReader dtrUser = dalApply.GetList(CurrentUserInfo.CompanyID, CommunityID, CurrentUserInfo.UserID);

            if (dtrUser != null)
            {
                if (dtrUser.HasRows) JoinCheck = 1;
            }

            #endregion

            #region 2. 가입한 커뮤니티 중 관리자에 의해 강제탈퇴 되었는지 여부 확인 (1: 강제탈퇴됨)

            DAL.User dalUser = new DAL.User();
            StatusCheck = dalUser.GetUserStatus(CurrentUserInfo.CompanyID, CommunityID, CurrentUserInfo.UserID);

            #endregion

        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        
        #endregion
    }
}