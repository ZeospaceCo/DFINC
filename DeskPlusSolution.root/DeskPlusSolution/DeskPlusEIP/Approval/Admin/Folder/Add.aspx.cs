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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Folder
{
    public partial class Add : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int FolderID = 0;
        public string FolderPath = "";
        public string TypeInfo = "";
        public string IsAdd = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Folder.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
            LoadParam();
            SetLanguage();

            lbFolder.Text = FolderPath;

            if (IsAdd.Equals("0"))
            {
                LoadFolderInfo(FolderID);
            }
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
                cbtnConfirm.Text = this.GetResource("Confirm");
                cbtnClose.Text = this.GetResource("Close");
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

            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            FolderPath = GetDecryptParam("folderPath", "");
            IsAdd = GetDecryptParam("isAdd", "");
            TypeInfo = GetDecryptParam("typeInfo", "");
        }
        /// <summary>
        /// 폴더정보 가져오기
        /// </summary>
        /// <param name="_folderID"></param>
        private void LoadFolderInfo(int _folderID)
        {
            if (_folderID.Equals(0)) return;

            DAL.Folder fDal = new DAL.Folder();
            List<BE.FolderInfo> list = fDal.GetFolder(CurrentUserInfo.CompanyID, _folderID);

            foreach (BE.FolderInfo info in list)
            {
                lbFolder.Text = GetLangName(info.PathNameBase, info.PathNameKo, info.PathNameEn, info.PathNameJa, info.PathNameFr, info.PathNameZhcn, info.PathNameZhtw, PreLang);
                ctbFolderName.Text = GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                ctbNameKO.Text = info.NameKo;
                ctbNameEN.Text = info.NameEn;
                ctbNameJA.Text = info.NameJa;
                ctbNameFR.Text = info.NameFr;
                ctbNameZHCN.Text = info.NameZhcn;
                ctbNameZHTW.Text = info.NameZhtw;

                chkIsActive.Checked = (info.IsActive.Equals("1")) ? true : false;
            }
        }
        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private bool Addition()
        {
            BE.FolderInfo info = new BE.FolderInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ParentID = FolderID;
            info.TypeInfo = TypeInfo;
            info.NameBase = ctbFolderName.Text;
            info.NameKo = ctbNameKO.Text;
            info.NameEn = ctbNameEN.Text;
            info.NameJa = ctbNameJA.Text;
            info.NameFr = ctbNameFR.Text;
            info.NameZhcn = ctbNameZHCN.Text;
            info.NameZhtw = ctbNameZHTW.Text;
            info.IsActive = (chkIsActive.Checked) ? "1" : "0";
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
            info.NameBase = ctbFolderName.Text;
            info.NameKo = ctbNameKO.Text;
            info.NameEn = ctbNameEN.Text;
            info.NameJa = ctbNameJA.Text;
            info.NameFr = ctbNameFR.Text;
            info.NameZhcn = ctbNameZHCN.Text;
            info.NameZhtw = ctbNameZHTW.Text;
            info.IsActive = (chkIsActive.Checked) ? "1" : "0";
            info.ModifyUserID = CurrentUserInfo.UserID;

            DAL.Folder fDal = new DAL.Folder();
            return fDal.Mod(info);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// [추가][수정]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = (IsAdd.Equals("1")) ? Addition() : Mod();

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Add.SubmitClose();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "Add.SubmitClose();");
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