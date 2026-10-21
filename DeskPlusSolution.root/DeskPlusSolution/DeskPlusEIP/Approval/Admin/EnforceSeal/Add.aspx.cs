using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.DAL;

namespace Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal
{
    public partial class Add : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public string StampImage = string.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();

                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadDepts();    // 계열사정보 가져오기
            LoadStampSealInfo();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Btn_Save");
                CbtnCancel.Text = resMng.GetString("Cancel");     //"취소";

                //ctbExpFirstBase.Title = resMng.GetString("EnforceExpFirstTitle");
                //ctbExpMidBase.Title = resMng.GetString("EnforceExpMidTitle");
                //ctbExpEndBase.Title = resMng.GetString("EnforceExpEndTitle");
                
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
        }

        /// <summary>
        /// 최상위부서 리스트 가져오기(계열사)
        /// </summary>
        private void LoadDepts()
        {
            Zeospace.DeskPlus.Account.DAL.Dept dal = new Dept();
            List<Zeospace.DeskPlus.Account.BE.DeptInfo> list = dal.GetRootDeptLists(CurrentUserInfo.CompanyID, true);

            Zeospace.DeskPlus.Approval.DAL.EnforceStamp appDal = new DAL.EnforceStamp();
            List<BE.EnforceStampInfo> listStamp = appDal.GetEnforceStamp(CurrentUserInfo.CompanyID);

            ddlDept.Items.Add(new ListItem(this.GetResource("EnforceMsg00"), ""));

            if (list != null && listStamp != null)
            {
                foreach (Account.BE.DeptInfo info in list)
                {
                    if (listStamp.Where(x => x.DeptID == info.DeptID).ToList<BE.EnforceStampInfo>().Count.Equals(0))
                    {
                        ddlDept.Items.Add(new ListItem(GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang), info.DeptID.ToString()));
                    }
                }
            }
        }
        /// <summary>
        /// 시행문직인정보
        /// </summary>
        /// <param name="_deptID"></param>
        private void LoadStampSealInfo()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            string noImage = this.BaseImgPath + "/Icon/ico_noimg2.gif";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", noImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            noImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            //StampImage = "<img src='" + noImage + "' width='94' height='124' alt='' title='' />";
            StampImage = "<img src='" + noImage + "' width='60' height='50' alt='' title='' />";
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            BE.EnforceStampInfo info = new BE.EnforceStampInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.DeptID = ddlDept.SelectedValue.ToString().ToNumber<int>();

            #region 표현명
            info.ExpFirstBase = ctbExpFirstBase.Text;
            string multiName = tbExpFirstMultiHidden.Text.Trim();            
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.ExpFirstKo = multiName.Split(',')[0];
                info.ExpFirstEn = multiName.Split(',')[1];
                info.ExpFirstJa = multiName.Split(',')[2];
                info.ExpFirstFr = multiName.Split(',')[3];
                info.ExpFirstZhcn = multiName.Split(',')[4];
                info.ExpFirstZhtw = multiName.Split(',')[5];
            }
            info.ExpMidBase = ctbExpMidBase.Text;
            multiName = tbExpMidMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.ExpMidKo = multiName.Split(',')[0];
                info.ExpMidEn = multiName.Split(',')[1];
                info.ExpMidJa = multiName.Split(',')[2];
                info.ExpMidFr = multiName.Split(',')[3];
                info.ExpMidZhcn = multiName.Split(',')[4];
                info.ExpMidZhtw = multiName.Split(',')[5];
            }
            info.ExpEndBase = ctbExpEndBase.Text;
            multiName = tbExpEndMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.ExpEndKo = multiName.Split(',')[0];
                info.ExpEndEn = multiName.Split(',')[1];
                info.ExpEndJa = multiName.Split(',')[2];
                info.ExpEndFr = multiName.Split(',')[3];
                info.ExpEndZhcn = multiName.Split(',')[4];
                info.ExpEndZhtw = multiName.Split(',')[5];
            }
            #endregion

            info.ZipCode = ctbZipCode.Text;

            #region 주소
            info.AddressFirstBase = ctbAddressFirstBase.Text;
            multiName = tbAddressFirstMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf("|") != -1)
            {
                info.AddressFirstKo = multiName.Split('|')[0];
                info.AddressFirstEn = multiName.Split('|')[1];
                info.AddressFirstJa = multiName.Split('|')[2];
                info.AddressFirstFr = multiName.Split('|')[3];
                info.AddressFirstZhcn = multiName.Split('|')[4];
                info.AddressFirstZhtw = multiName.Split('|')[5];
            }
            info.AddressEndBase = ctbAddressEndBase.Text;
            multiName = tbAddressEndMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf("|") != -1)
            {
                info.AddressEndKo = multiName.Split('|')[0];
                info.AddressEndEn = multiName.Split('|')[1];
                info.AddressEndJa = multiName.Split('|')[2];
                info.AddressEndFr = multiName.Split('|')[3];
                info.AddressEndZhcn = multiName.Split('|')[4];
                info.AddressEndZhtw = multiName.Split('|')[5];
            }
            #endregion

            #region 전화번호
            info.PhoneBase = ctbPhoneBase.Text;
            multiName = tbPhoneMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.PhoneKo = multiName.Split(',')[0];
                info.PhoneEn = multiName.Split(',')[1];
                info.PhoneJa = multiName.Split(',')[2];
                info.PhoneFr = multiName.Split(',')[3];
                info.PhoneZhcn = multiName.Split(',')[4];
                info.PhoneZhtw = multiName.Split(',')[5];
            }
            #endregion

            #region 팩스
            info.FaxBase = ctbFaxBase.Text;
            multiName = tbFaxMultiHidden.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.FaxKo = multiName.Split(',')[0];
                info.FaxEn = multiName.Split(',')[1];
                info.FaxJa = multiName.Split(',')[2];
                info.FaxFr = multiName.Split(',')[3];
                info.FaxZhcn = multiName.Split(',')[4];
                info.FaxZhtw = multiName.Split(',')[5];
            }
            #endregion

            info.Homepage = ctbHomepage.Text;
            info.WriteUserID = CurrentUserInfo.UserID;

            #region 파일처리(직인)

            if (!tbFileName.Text.Equals(""))
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbFileName.Text);

                string destFolder = baseFileDirectory + "{0}/Approval/Enforce/Stamp/{1}/";
                destFolder = string.Format(destFolder, CurrentUserInfo.CompanyID, info.DeptID.ToString());
                FileHelper.CreateSubFolder(destFolder);

                string[] fileStr = tbFileName.Text.Split('.');
                string destFile = "Stamp_" + SwfUploadGUID + "." + fileStr[1];

                System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFolder + destFile));

                info.FilePath = destFolder;
                info.FileName = destFile;
            }
            else
            {
                info.FilePath = "";
                info.FileName = "";
            }
            #endregion

            DAL.EnforceStamp dal = new DAL.EnforceStamp();
            return dal.Add(info);
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
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
                int stampID = 0;
                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        stampID = Addition();
                        if (stampID > 0)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();

                            parmDic.Add("currentPage", CurrentPage.ToString());
                            parmDic.Add("recordPerPage", RecordPerPage.ToString());
                            parmDic.Add("searchField", SearchField);
                            parmDic.Add("searchValue", SearchValue);
                            parmDic.Add("searchTerm", SearchTerm);
                            parmDic.Add("searchTermValue", SearchTermValue);
                            parmDic.Add("order", Order);

                            parmDic.Add("stampID", stampID.ToString());
                            parmDic.Add("deptID", ddlDept.SelectedValue.ToString());

                            parmDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                            parmDic.Add("alertMsg", this.GetResource("GoToInputInfo"));
                            parmDic.Add("goURL", "/DeskPlusEIP/Approval/Admin/EnforceSeal/Mod.aspx");

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "");
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
    }
}