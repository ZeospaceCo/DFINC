using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Folder
{
    public partial class Add : Base.BasePagePMSAdmin
    {
        #region 변수처리
        public int FolderID;
        public string Gubun = String.Empty;
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Folder.Add).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (FolderID > 0)
            {
                LoadFolderInfo();
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Save");       //"확인";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
        /// 작성목적 : 선택한 폴더의 정보 가져오기
        /// </summary>
        private void LoadFolderInfo()
        {
            DAL.Folder dal = new DAL.Folder();
            DataTableReader dr = dal.GetFolderByID(CurrentUserInfo.CompanyID, FolderID);

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string pName = this.GetLangName(
                            dr["PFolderBase"].ToString().ToNull(),
                            dr["PFolderKo"].ToString().ToNull(),
                            dr["PFolderEn"].ToString().ToNull(),
                            dr["PFolderJa"].ToString().ToNull(),
                            dr["PFolderFr"].ToString().ToNull(),
                            dr["PFolderZhcn"].ToString().ToNull(),
                            dr["PFolderZhtw"].ToString().ToNull(), PreLang);

                        string name = this.GetLangName(
                            dr["FolderBase"].ToString().ToNull(),
                            dr["FolderKo"].ToString().ToNull(), dr["FolderEn"].ToString().ToNull(),
                            dr["FolderJa"].ToString().ToNull(), dr["FolderFr"].ToString().ToNull(),
                            dr["FolderZhcn"].ToString().ToNull(), dr["FolderZhtw"].ToString().ToNull(), PreLang);

                        if (Gubun == "mod")
                        {
                            ltHeader.Text = GetResource("FolderMod");
                            CTBParentName.Text = pName.ToReverseXSS();
                            CTBName.Text = dr["FolderBase"].ToString().ToReverseXSS();

                            CTBMultiName.Text = this.GetMuiltiLang(
                                dr["FolderKo"].ToString().ToReverseXSS(),
                                dr["FolderEn"].ToString().ToReverseXSS(),
                                dr["FolderJa"].ToString().ToReverseXSS(),
                                dr["FolderFr"].ToString().ToReverseXSS(),
                                dr["FolderZhCn"].ToString().ToReverseXSS(),
                                dr["FolderZhTw"].ToString().ToReverseXSS());


                        }
                        else
                        {
                            ltHeader.Text = GetResource("FolderAdd");
                            CTBParentName.Text = name.ToReverseXSS();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 폴더 추가
        /// </summary>
        private void FolderAdd()
        {
            PMS.BE.FolderInfo folderInfo = new BE.FolderInfo();
            folderInfo.CompanyID = CurrentUserInfo.CompanyID;
            folderInfo.ParentFolderID = FolderID;            
            folderInfo.FolderBase = CTBName.Text.Trim();
            folderInfo.WriteUserID = CurrentUserInfo.UserID;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                folderInfo.FolderKo = MultiName.Split(',')[0];
                folderInfo.FolderEn = MultiName.Split(',')[1];
                folderInfo.FolderJa = MultiName.Split(',')[2];
                folderInfo.FolderFr = MultiName.Split(',')[3];
                folderInfo.FolderZhcn = MultiName.Split(',')[4];
                folderInfo.FolderZhtw = MultiName.Split(',')[5];
            }

            DAL.Folder dal = new DAL.Folder();

            if (dal.Add(folderInfo) > 0)
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebHard.Folder.Add.Add()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 폴더 수정
        /// </summary>
        private void FolderMod()
        {
            PMS.BE.FolderInfo folderInfo = new BE.FolderInfo();
            folderInfo.CompanyID = CurrentUserInfo.CompanyID;
            folderInfo.FolderID = FolderID;
            folderInfo.FolderBase = CTBName.Text.Trim().Trim();
            folderInfo.WriteUserID = CurrentUserInfo.UserID;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                folderInfo.FolderKo = MultiName.Split(',')[0];
                folderInfo.FolderEn = MultiName.Split(',')[1];
                folderInfo.FolderJa = MultiName.Split(',')[2];
                folderInfo.FolderFr = MultiName.Split(',')[3];
                folderInfo.FolderZhcn = MultiName.Split(',')[4];
                folderInfo.FolderZhtw = MultiName.Split(',')[5];
            }

            DAL.Folder dal = new DAL.Folder();

            if (dal.Mod(folderInfo))
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebHard.Folder.Add.Mod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            if (Gubun == "add")
            {
                FolderAdd();
            }
            else if (Gubun == "mod")
            {
                FolderMod();
            }

        }

        #endregion
    }
}