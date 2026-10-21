using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class FolderAdd : Base.BasePageReport
    {
        public string IsAdd { get; set; }

        public string IsKo { get; set; }

        public string IsEn { get; set; }

        public string IsJa { get; set; }

        public string IsFr { get; set; }

        public string IsZhCn { get; set; }

        public string IsZhTw { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.FolderAdd).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

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

            SetMultiLang();

            int parentFolderID = 0;

            DAL.Folder dal = new DAL.Folder();

            //수정 및 키값 존재시
            if (IsAdd == "0" && FolderID > 0)
            {   
                var dtr = dal.GetFolder(CurrentUserInfo.CompanyID, FolderID, false);
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ctbFolderName.Text = dtr["NameBase"].ToString();
                            chkIsActive.Checked = (dtr["ActiveFolder"].ToString() == "1") ? true : false;
                            ctbNameKO.Text = dtr["NameKo"].ToString();
                            ctbNameEN.Text = dtr["NameEn"].ToString();
                            ctbNameJA.Text = dtr["NameJa"].ToString();
                            ctbNameFR.Text = dtr["NameFr"].ToString();
                            ctbNameZHCN.Text = dtr["NameZhCn"].ToString();
                            ctbNameZHTW.Text = dtr["NameZhTw"].ToString();

                            parentFolderID = dtr["ParentFolderID"].ToString().ToNumber<int>();
                        }
                    }
                }
            }
            else if (IsAdd == "1")
            {
                parentFolderID = FolderID;
            }

            if (parentFolderID > 0)
            {
                var dtr = dal.GetFolder(CurrentUserInfo.CompanyID, parentFolderID, false);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ltlParentFolderName.Text = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the multi language.
        /// </summary>
        private void SetMultiLang()
        {
            IsKo = "display:none";
            IsEn = "display:none";
            IsJa = "display:none";
            IsFr = "display:none";
            IsZhCn = "display:none";
            IsZhTw = "display:none";

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            for (int i = 0; i < dv.Count; i++)
            {
                switch (dv[i]["CodeID"].ToString().ToLower())
                {
                    case "ko":
                        IsKo = "display:";
                        break;

                    case "en":
                        IsEn = "display:";
                        break;

                    case "ja":
                        IsJa = "display:";
                        break;

                    case "fr":
                        IsFr = "display:";
                        break;

                    case "zhcn":
                        IsZhCn = "display:";
                        break;

                    case "zhtw":
                        IsZhTw = "display:";
                        break;
                }
            }
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            IsAdd = GetDecryptParam("isAdd", "");
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
                cbtnConfirm.Text = resMng.GetString("ConfirmCheck");
                cbtnClose.Text = resMng.GetString("Close");
                ltlTitle.Text = (IsAdd == "0" && FolderID > 0) ? resMng.GetString("Modify") : resMng.GetString("Add");
            }
        }

        /// <summary>
        /// Additions this instance.
        /// </summary>
        /// <returns></returns>
        private bool Addition()
        {
            BE.FolderInfo info = new BE.FolderInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ParentFolderID = FolderID;
            info.NameBase = ctbFolderName.Text.Trim();
            info.NameKo = ctbNameKO.Text.Trim();
            info.NameEn = ctbNameEN.Text.Trim();
            info.NameJa = ctbNameJA.Text.Trim();
            info.NameFr = ctbNameFR.Text.Trim();
            info.NameZhcn = ctbNameZHCN.Text.Trim();
            info.NameZhtw = ctbNameZHTW.Text.Trim();
            info.ActiveFolder = (chkIsActive.Checked) ? "1" : "0";
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Folder fDal = new DAL.Folder();
            bool rtnval = (fDal.Add(info) > 0) ? true : false;
            return rtnval;
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <returns></returns>
        private bool Mod()
        {
            BE.FolderInfo info = new BE.FolderInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.FolderID = FolderID;
            info.NameBase = ctbFolderName.Text.Trim();
            info.NameKo = ctbNameKO.Text.Trim();
            info.NameEn = ctbNameEN.Text.Trim();
            info.NameJa = ctbNameJA.Text.Trim();
            info.NameFr = ctbNameFR.Text.Trim();
            info.NameZhcn = ctbNameZHCN.Text.Trim();
            info.NameZhtw = ctbNameZHTW.Text.Trim();
            info.ActiveFolder = (chkIsActive.Checked) ? "1" : "0";
            info.ModifyUserID = CurrentUserInfo.UserID;

            DAL.Folder fDal = new DAL.Folder();
            return fDal.Mod(info);
        }

        /// <summary>
        /// Handles the Click event of the lbtnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                SetMultiLang();

                bool rtnval = (IsAdd == "0" && FolderID > 0) ? Mod() : Addition();

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("Processing"), "FolderAdd.SubmitClose();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("ProcessingError"), "FolderAdd.SubmitClose();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("ProcessingError"), "");
            }
        }
    }
}