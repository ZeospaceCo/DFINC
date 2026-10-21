using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Approve
{
    public partial class Add : Base.BasePageApproval
    {
        #region 변수설정
        public string CabinetID;
        public int FolderID;
        //Add : 추가; Mod : 수정
        public string Gubun;
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Approve.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();
            GetLoad();


        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CabinetID = GetDecryptParam("cabinetID", "");
            Gubun = GetDecryptParam("gubun", "Add");

            if (CabinetID.IndexOf("ApprovalFolder_") != -1) FolderID = CabinetID.Split('_')[1].ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnMove.Text = resMng.GetString("Orders");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                cbtnDel.Text = resMng.GetString("Btn_Del");   //"삭제";

                ltlHeader.Text = (Gubun == "Mod") ? resMng.GetString("ModFolder") : resMng.GetString("AddFolder");

                ddlActive.Items.Clear();
                ddlActive.Items.Add(new ListItem(resMng.GetString("Active"), "1"));
                ddlActive.Items.Add(new ListItem(resMng.GetString("NonActive"), "0"));
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 내용가져오기
        /// </summary>
        private void GetLoad()
        {
            cbtnMove.Visible = false;
            cbtnAdd.Visible = true;
            cbtnDel.Visible = true;


            if (Gubun == "Mod")
            {
                DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
                var alList = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FolderID);

                foreach (var info in alList)
                {
                    ctbName.Text = info.FolderBase.ToReverseXSS();
                    ctbMuliLang.Text = GetMuiltiLang(
                        info.FolderKo.ToReverseXSS(), info.FolderEn.ToReverseXSS(), info.FolderJa.ToReverseXSS(),
                        info.FolderFr.ToReverseXSS(), info.FolderZhcn.ToReverseXSS(), info.FolderZhtw.ToReverseXSS());

                    ddlActive.SelectedValue = info.ActiveFolder;

                    if (info.ParentFolderID > 0)
                    {
                        SetHeader(info.ParentFolderID);
                    }
                }
            }
            else if (Gubun == "Add")
            {
                cbtnAdd.Visible = true;
                cbtnDel.Visible = false;
                cbtnMove.Visible = false;

                SetHeader(FolderID);
            }

        }

        /// <summary>
        /// 폴더 사이트맵
        /// </summary>
        /// <param name="_folderID">The _folder ID.</param>
        private void SetHeader(int _folderID)
        {
            if (FolderID > 0)
            {
                //상위폴더명
                DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
                var alPList = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, _folderID);
                foreach (var infoPID in alPList)
                {
                    string name = this.GetLangName(
                        infoPID.PathNameBase,
                        infoPID.PathNameKo,
                        infoPID.PathNameEn, infoPID.PathNameJa,
                        infoPID.PathNameFr, infoPID.PathNameZhcn,
                        infoPID.PathNameZhtw, PreLang);

                    switch (PreLang)
                    {
                        case "ko":
                            name = (infoPID.FolderKo == "") ? infoPID.PathNameBase : name;
                            break;
                        case "en":
                            name = (infoPID.FolderEn == "") ? infoPID.PathNameBase : name;
                            break;
                        case "ja":
                            name = (infoPID.FolderJa == "") ? infoPID.PathNameBase : name;
                            break;
                        case "fr":
                            name = (infoPID.FolderFr == "") ? infoPID.PathNameBase : name;
                            break;
                        case "zhcn":
                            name = (infoPID.FolderZhcn == "") ? infoPID.PathNameBase : name;
                            break;
                        case "zhtw":
                            name = (infoPID.FolderZhtw == "") ? infoPID.PathNameBase : name;
                            break;
                    }

                    ltlParentName.Text = name.Substring(1, name.Length - 1);
                }
            }
        }




        #endregion

        #region Event Handler (이벤트 핸들러)



        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}