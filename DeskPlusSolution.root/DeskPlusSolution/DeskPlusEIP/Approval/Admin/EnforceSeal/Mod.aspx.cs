using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.DAL;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal
{
    public partial class Mod : Base.BasePageApprovalAdmin
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.Mod).Assembly;	//어셈블리 선언
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
            LoadStampInfo(EnforceStampID);
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
                CbtnDel.Text = resMng.GetString("Del");
                CbtnConfirm.Text = resMng.GetString("Btn_Save");
                CbtnCancel.Text = resMng.GetString("Cancel");     //"취소";
                cbtnAdminAdd.Text = resMng.GetString("AddUser");
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
            EnforceStampID = GetDecryptParam("stampID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
        }
        /// <summary>
        /// 최상위부서 리스트 가져오기(계열사)
        /// </summary>
        private void LoadDepts()
        {
            Zeospace.DeskPlus.Account.DAL.Dept dal = new Dept();
            List<Zeospace.DeskPlus.Account.BE.DeptInfo> list = dal.GetRootDeptLists(CurrentUserInfo.CompanyID, true);

            ddlDept.Items.Add(new ListItem(this.GetResource("EnforceMsg00"), ""));

            if (list != null)
            {
                foreach (Account.BE.DeptInfo info in list)
                {
                    ddlDept.Items.Add(new ListItem(GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang), info.DeptID.ToString()));
                }
            }
        }
        /// <summary>
        /// 시행문정보
        /// </summary>
        /// <param name="_stampID"></param>
        private void LoadStampInfo(int _stampID)
        {
            if (_stampID.Equals(0)) return;

            DAL.EnforceStamp dal = new DAL.EnforceStamp();
            List<BE.EnforceStampInfo> list = dal.GetEnforceStamp(CurrentUserInfo.CompanyID, _stampID);

            if (list != null)
            {
                foreach (BE.EnforceStampInfo info in list)
                {
                    if (ddlDept.Items.FindByValue(info.DeptID.ToString()) != null)
                    {
                        ddlDept.SelectedValue = info.DeptID.ToString();

                        #region 표현명
                        ctbExpFirstBase.Text = info.ExpFirstBase.ToReverseXSS();
                        ctbExpMidBase.Text = info.ExpMidBase.ToReverseXSS();
                        ctbExpEndBase.Text = info.ExpEndBase.ToReverseXSS();
                        tbExpFirstMulti.Text = GetMuiltiLang(
                            info.ExpFirstKo.ToReverseXSS(),
                            info.ExpFirstEn.ToReverseXSS(),
                            info.ExpFirstJa.ToReverseXSS(),
                            info.ExpFirstFr.ToReverseXSS(),
                            info.ExpFirstZhcn.ToReverseXSS(),
                            info.ExpFirstZhtw.ToReverseXSS());
                        tbExpMidMulti.Text = GetMuiltiLang(
                            info.ExpMidKo.ToReverseXSS(),
                            info.ExpMidEn.ToReverseXSS(),
                            info.ExpMidJa.ToReverseXSS(),
                            info.ExpMidFr.ToReverseXSS(),
                            info.ExpMidZhcn.ToReverseXSS(),
                            info.ExpMidZhtw.ToReverseXSS());
                        tbExpEndMulti.Text = GetMuiltiLang(
                            info.ExpEndKo.ToReverseXSS(),
                            info.ExpEndEn.ToReverseXSS(),
                            info.ExpEndJa.ToReverseXSS(),
                            info.ExpEndFr.ToReverseXSS(),
                            info.ExpEndZhcn.ToReverseXSS(),
                            info.ExpEndZhtw.ToReverseXSS());
                        #endregion

                        ctbZipCode.Text = info.ZipCode.ToReverseXSS();

                        #region 주소
                        ctbAddressFirstBase.Text = info.AddressFirstBase.ToReverseXSS();
                        ctbAddressEndBase.Text = info.AddressEndBase;
                        tbAddressFirstMulti.Text = info.AddressFirstKo.ToReverseXSS() + "|" + info.AddressFirstEn.ToReverseXSS() + "|" +
                                                    info.AddressFirstJa.ToReverseXSS() + "|" + info.AddressFirstFr.ToReverseXSS() + "|" +
                                                    info.AddressFirstZhcn.ToReverseXSS() + "|" + info.AddressFirstZhtw.ToReverseXSS();
                        tbAddressEndMulti.Text = info.AddressEndKo.ToReverseXSS() + "|" + info.AddressEndEn.ToReverseXSS() + "|" +
                                                info.AddressEndJa.ToReverseXSS() + "|" + info.AddressEndFr.ToReverseXSS() + "|" +
                                                info.AddressEndZhcn.ToReverseXSS() + "|" + info.AddressEndZhtw.ToReverseXSS();
                        #endregion

                        #region 주소
                        ctbPhoneBase.Text = info.PhoneBase.ToReverseXSS();
                        ctbPhoneMulti.Text = GetMuiltiLang(
                            info.PhoneKo.ToReverseXSS(),
                            info.PhoneEn.ToReverseXSS(),
                            info.PhoneJa.ToReverseXSS(),
                            info.PhoneFr.ToReverseXSS(),
                            info.PhoneZhcn.ToReverseXSS(),
                            info.PhoneZhtw.ToReverseXSS());
                        #endregion

                        #region 팩스
                        ctbFaxBase.Text = info.FaxBase.ToReverseXSS();
                        ctbFaxMulti.Text = GetMuiltiLang(
                            info.FaxKo.ToReverseXSS(),
                            info.FaxEn.ToReverseXSS(),
                            info.FaxJa.ToReverseXSS(),
                            info.FaxFr.ToReverseXSS(),
                            info.FaxZhcn.ToReverseXSS(),
                            info.FaxZhtw.ToReverseXSS());
                        #endregion

                        ctbHomepage.Text = info.Homepage.ToReverseXSS();
                        FilePath = info.FilePath;
                        FileName = info.FileName;
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

            string stampImage = FilePath + FileName;
            string noImage = this.BaseImgPath + "/Icon/ico_noimg2.gif";
            string targetImage = (File.Exists(Server.MapPath(stampImage))) ? stampImage : noImage;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", targetImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            targetImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            if (File.Exists(Server.MapPath(stampImage)))
            {
                StampImage = "<img src='" + targetImage + "' width='94' height='124' alt='' title='' />";
            }
            else
            {
                StampImage = "<img src='" + noImage + "' width='60' height='50' alt='' title='' />";
            }
        }
        
        /// <summary>
        /// 수정
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            BE.EnforceStampInfo info = new BE.EnforceStampInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.EnforceStampID = EnforceStampID;
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
            info.ModifyUserID = CurrentUserInfo.UserID;

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
                if (File.Exists(Server.MapPath(FilePath + FileName)))
                {
                    info.FilePath = FilePath;
                    info.FileName = FileName;
                }
                else
                {
                    info.FilePath = "";
                    info.FileName = "";
                }
            }
            #endregion

            DAL.EnforceStamp dal = new DAL.EnforceStamp();
            return dal.Mod(info);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del(int _enforceStampID, int _deptID)
        {
            if (_enforceStampID.Equals(0) || _deptID.Equals(0)) return false;

            BLL.EnforceStamp bll = new BLL.EnforceStamp();
            return bll.Del(CurrentUserInfo.CompanyID, _enforceStampID, _deptID);
        }
        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 
        /// </summary>
        public int EnforceStampID
        {
            get { return GetViewStateInt("EnforceStampID", "0"); }
            set { ViewState["EnforceStampID"] = value; }
        }
        public int DeptID
        {
            get { return GetViewStateInt("DeptID", "0"); }
            set { ViewState["DeptID"] = value; }
        }
        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }
        public string FilePath
        {
            get { return GetViewStateString("FilePath", ""); }
            set { ViewState["FilePath"] = value; }
        }
        public string FileName
        {
            get { return GetViewStateString("FileName", ""); }
            set { ViewState["FileName"] = value; }
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
                bool rtnval = false;
                switch (EventArgs.ToUpper())
                {
                    case "MOD":
                        rtnval = Modify();
                        break;
                    case "DEL":
                        rtnval = Del(EnforceStampID, DeptID);
                        break;
                }

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Mod.Cancel();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "Mod.Cancel();");
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