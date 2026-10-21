using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Flow
{
    public partial class Add : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string EventArgs = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Flow.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
            //DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            //DeptNm = GetDecryptParam("deptNm", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        private void LoadData()
        {
        }

        /// <summary>
        /// 추가
        /// </summary>
        private int Addition()
        {
            BE.FavSeqInfo info = new FavSeqInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.Subject = ctbSubject.Text.Trim().ToXSS();
            info.Memo = ctbMemo.Text.Trim().ToXSS();
            info.FormID = 0;
            info.IsAdmin = "0";

            DAL.FavSeq fDal = new FavSeq();
            return fDal.Add(info);
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// [확인][삭제]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                switch (EventArgs.ToUpper())
                {
                    case "ADD":

                        int favSeqID = Addition();
                        if (favSeqID > 0)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();

                            parmDic.Add("currentPage", CurrentPage.ToString());
                            parmDic.Add("recordPerPage", RecordPerPage.ToString());
                            parmDic.Add("searchField", SearchField);
                            parmDic.Add("searchValue", SearchValue);
                            parmDic.Add("searchTerm", SearchTerm);
                            parmDic.Add("searchTermValue", SearchTermValue);
                            parmDic.Add("order", Order);

                            parmDic.Add("favSeqID", favSeqID.ToString());

                            parmDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                            parmDic.Add("alertMsg", this.GetResource("GoToInputInfo"));
                            parmDic.Add("goURL", "/DeskPlusEIP/Approval/Preference/Flow/Mod.aspx");

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", AlertMsgEnum.Alert_Error.ToString(), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}