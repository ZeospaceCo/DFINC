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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Admin.Content
{
    public partial class UnitAdd : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinionAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string Section = string.Empty;     //메뉴(질문:0 or 예시:1)
        public int TotCnt = 0;                //총 게시물수
        public int UnitID = 0;                //질문ID
        public string PageTitle = string.Empty;     //페이지제목
        public string EventArgs = string.Empty;
        public int ExpUnitID = 0;                //질문생성 용
        public DateTime WriteDate = DateTime.Now;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.UnitAdd).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();     //파라미터처리
                SetLanguage();  //다국어처리

                // 파일경로
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                SwfUploadPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

                if (!UnitID.Equals(0))
                {
                    //수정모드
                    this.GetUnitInfo(CurrentUserInfo.CompanyID, UnitID);
                }
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 질문정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        private void GetUnitInfo(int _companyID, int _unitID)
        {
            if (_companyID < 0 || _unitID < 0) return;

            DAL.Unit dUnit = new DAL.Unit();
            List<BE.UnitInfo> list = dUnit.GetUnit(_companyID, _unitID);
            foreach (BE.UnitInfo uInfo in list)
            {
                this.ctbTitle.Text = uInfo.Title.ToReverseXSS();
                this.ddlType.SelectedValue = uInfo.Type.ToUpper();
                this.ddlReq.SelectedValue = uInfo.IsReq;
                this.ddlActive.SelectedValue = uInfo.IsActive;
                this.ddlIsHorizon.SelectedValue = uInfo.IsHorizon;
                this.tbPoint.Value = uInfo.Point.ToString();
                WriteDate = uInfo.WriteDate;
            }

            this.tbHiddenUnits.Text = "";
            DAL.UnitItem dItem = new DAL.UnitItem();
            List<BE.UnitItemInfo> slist = dItem.GetUnitItem(_companyID, _unitID);
            foreach (BE.UnitItemInfo sInfo in slist)
            {
                this.tbHiddenUnits.Text += sInfo.Type + "$" + sInfo.Cont.ToReverseXSS() + "$" + sInfo.ImgName + ";";

                // 임시폴더에 데이타 생성
                if (!sInfo.ImgUrl.Trim().Equals(""))
                {
                    DirectoryInfo di = new DirectoryInfo(SwfUploadPath);
                    if (!di.Exists)
                    {
                        FileHelper.CreateSubFolder(SwfUploadPath);
                    }

                    FileHelper.CopyFile(sInfo.ImgUrl, SwfUploadPath + sInfo.ImgName);
                }
            }
            this.tbHiddenUnits.Text = this.tbHiddenUnits.Text.TrimEnd(';');
        }

        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
            /*
        'menuID': MenuID
        , 'section': Section
        , 'totCnt': TotCnt
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'unitID': _unitID
        */

            //파라미터
            MenuID = (MenuID.Equals("")) ? "exam" : MenuID;
            Section = GetDecryptParam("section", "0");
            UnitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            ExpUnitID = GetDecryptParam("expID", "0").ToNumber<int>();
            Section = (ExpUnitID > 0) ? "0" : Section;  /* unitID : expunitID -> 질문추가 0 : 0 , 질문수정 1 : 0  , 질문생성 1 : 1 */

            //버튼Visible
            cbtnSeq.Visible = (UnitID.Equals(0) && ExpUnitID.Equals(0)) ? false : true;
            cbtnDel.Visible = (UnitID.Equals(0) && ExpUnitID.Equals(0)) ? false : true;
            cbtnUnitAdd.Visible = (MenuID.ToUpper().IndexOf("UNIT") < 0 && !UnitID.Equals(ExpUnitID)) ? true : false;   //예시관리(수정일때만)
        }

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //Title
                this.PageTitle = this.GetResource("Title1") + " > " + this.GetResource("Title2") + " > ";
                this.PageTitle += (Section.Equals("0")) ? this.GetResource("Title4") + " > " : this.GetResource("Title3") + " > ";
                this.PageTitle += (UnitID.Equals(ExpUnitID)) ? this.GetResource("Add") + " > " : this.GetResource("Mod") + " > ";
                /* unitID : expunitID -> 질문추가 0 : 0 (추가) / 질문수정 1 : 0 (수정) / 질문생성 1 : 1 (추가)*/

                //Button
                this.cbtnConfirm.Text = this.GetResource("BtnSubmit");
                this.cbtnDel.Text = this.GetResource("BtnDel");
                this.cbtnCancel.Text = this.GetResource("BtnCancel");
                this.cbtnSeq.Text = this.GetResource("BtnSeqUnit");
                this.cbtnUnitAdd.Text = this.GetResource("BtnUnitAdd");

                this.ddlType.Items.Add(new ListItem(this.GetResource("Unit_TypeA"), "A"));
                this.ddlType.Items.Add(new ListItem(this.GetResource("Unit_TypeB"), "B"));
                this.ddlType.Items.Add(new ListItem(this.GetResource("Unit_TypeC"), "C"));
                this.ddlType.Items.Add(new ListItem(this.GetResource("Unit_TypeD"), "D"));
                this.ddlType.Items.Add(new ListItem(this.GetResource("Unit_TypeE"), "E"));

                this.ddlSelItem.Items.Add(new ListItem(this.GetResource("Option"), "0"));
                this.ddlSelItem.Items.Add(new ListItem(this.GetResource("OptionNon"), "1"));

                this.ddlReq.Items.Add(new ListItem(this.GetResource("ReqItem"), "1"));
                this.ddlReq.Items.Add(new ListItem(this.GetResource("ReqItemNon"), "0"));

                this.ddlActive.Items.Add(new ListItem(this.GetResource("Unit_Active"), "1"));
                this.ddlActive.Items.Add(new ListItem(this.GetResource("Unit_ActiveNon"), "0"));

                this.ddlIsHorizon.Items.Add(new ListItem(this.GetResource("Unit_Vertical"), "0"));
                this.ddlIsHorizon.Items.Add(new ListItem(this.GetResource("Unit_Horizon"), "1"));
            }
        }

        /// <summary>
        /// 추가/수정
        /// </summary>
        private void Add()
        {
            //질문정보
            BE.UnitInfo uInfo = new BE.UnitInfo();
            uInfo.CompanyID = CurrentUserInfo.CompanyID;
            uInfo.UnitID = UnitID;
            uInfo.Section = Section;
            uInfo.Title = ctbTitle.Text;
            uInfo.Type = this.ddlType.SelectedValue;        //단일선택(A), 다중선택(B), 단일가점(C), 다중가점(D), 주관식(E)
            uInfo.IsReq = this.ddlReq.SelectedValue;        //필수(1), 비필수(0)
            uInfo.Point = (this.tbPoint.Value.Equals("")) ? 0 : this.tbPoint.Value.ToNumber<int>();
            uInfo.IsActive = this.ddlActive.SelectedValue;  //활성(1), 비활성(0)
            uInfo.WriteUserID = CurrentUserInfo.UserID;
            uInfo.HiddenSpoolPath = SwfUploadPath;
            uInfo.WriteDate = WriteDate;
            uInfo.IsHorizon = this.ddlIsHorizon.SelectedValue;

            //질문항목정보
            List<BE.UnitItemInfo> list = null;
            if (!this.tbHiddenUnits.Text.Trim().Equals(""))
            {
                list = new List<BE.UnitItemInfo>();

                string[] arrTmp = this.tbHiddenUnits.Text.TrimEnd(';').Split(';');
                int tmpSeq = 0;
                foreach (string str in arrTmp)
                {
                    tmpSeq++;

                    BE.UnitItemInfo uItemInfo = new BE.UnitItemInfo();
                    uItemInfo.CompanyID = CurrentUserInfo.CompanyID;
                    uItemInfo.UnitID = UnitID;
                    uItemInfo.Type = str.Split('$')[0];
                    uItemInfo.Cont = str.Split('$')[1];
                    uItemInfo.ImgUrl = str.Split('$')[2];
                    uItemInfo.Seq = tmpSeq;

                    list.Add(uItemInfo);
                }
            }

            bool rtnval = false;
            BLL.Unit uBll = new BLL.Unit();
            if (UnitID.Equals(ExpUnitID))
            {
                rtnval = (uBll.Add(uInfo, list) > 0) ? true : false;    //추가   
            }
            else
            {
                rtnval = (uBll.Mod(uInfo, list)) ? true : false;        //수정(삭제후,입력)   
            }

            // 임시폴더삭제
            if (SwfUploadPath != "")
            {
                DirectoryInfo di = new DirectoryInfo(SwfUploadPath);
                if (di.Exists)
                {
                    FileHelper.DelFolder(SwfUploadPath, false);
                }
            }

            if (rtnval)
            {
                RedirectUrl(AlertMsgEnum.Alert_Success.ToString());
            }
            else
            {
                RedirectUrl(AlertMsgEnum.Alert_Error.ToString());
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private void Del()
        {
            if (CurrentUserInfo.CompanyID < 0 || UnitID < 0) return;

            BLL.Unit uBll = new BLL.Unit();
            if (uBll.Del(CurrentUserInfo.CompanyID, UnitID, WriteDate))
            {
                RedirectUrl(AlertMsgEnum.Alert_Success.ToString());
            }
            else
            {
                RedirectUrl(AlertMsgEnum.Alert_Error.ToString());
            }
        }

        /// <summary>
        /// 페이지이동처리
        /// </summary>
        /// <param name="_alertType"></param>
        private void RedirectUrl(string _alertType)
        {
            Dictionary<string, string> parmDic = new Dictionary<string, string>();
            parmDic.Add("menuID", MenuID);
            parmDic.Add("section", Section);
            parmDic.Add("totCnt", TotCnt.ToString());
            parmDic.Add("pageSize", PageSize.ToString());
            parmDic.Add("pageNo", PageNo.ToString());
            parmDic.Add("sKey", SKey);
            parmDic.Add("sVal", SVal);
            parmDic.Add("sPeriod", SPeriod);
            parmDic.Add("sPeriodVal", SPeriodVal);
            parmDic.Add("order", Order);
            parmDic.Add("unitID", UnitID.ToString());

            parmDic.Add("alertType", _alertType);     //AlertMsgEnum.Alert_Success.ToString()
            parmDic.Add("alertMsg", " ");
            parmDic.Add("goURL", "/DeskPlusEIP/Opinion/Admin/Content/UnitList.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
            Utility.Redirect(encParams);
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 파일업로드
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }
        /// <summary>
        /// 파일업로드
        /// </summary>
        public string SwfUploadPath
        {
            get { return GetViewStateString("SwfUploadPath", ""); }
            set { ViewState["SwfUploadPath"] = value; }
        }
        #endregion

        #region (이벤트핸들러)
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
                        this.Add();
                        break;
                    case "DEL":
                        this.Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("Msg_Fail"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParm(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}