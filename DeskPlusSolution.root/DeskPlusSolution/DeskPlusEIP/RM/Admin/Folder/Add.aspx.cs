using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Admin.Folder
{
    public partial class Add : Zeospace.DeskPlus.RM.Web.Base.BasePageRMAdmin
    {
        #region 변수처리
        public int CategoryID;
        public string Gubun = String.Empty;
        private string MultiName = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Folder.Add).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-02-06
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadFolderInfo();
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-06
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Submit");       //"확인";
                CTBParentName.TitleAtt = resMng.GetString("ParentFolder"); // 상위폴더이름
                CTBName.TitleAtt = resMng.GetString("PresentFolder"); // 현재폴더이름
                CTBMultiName.TitleAtt = resMng.GetString("MultiNames"); // 다국어이름
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-06
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();            
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 선택한 폴더의 정보 가져오기
        /// </summary>
        private void LoadFolderInfo()
        {
            if (CategoryID > 0)
            {
                DataSet ds = null;

                DAL.Category dal = new DAL.Category();
                ds = dal.GetFolder(CurrentUserInfo.CompanyID, CategoryID);


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string pName = this.GetLangName(dr["pNameBase"].ToString(), dr["pNameKo"].ToString(), dr["pNameEn"].ToString(), dr["pNameJa"].ToString(), dr["pNameFr"].ToString(), dr["pNameZhcn"].ToString(), dr["pNameZhtw"].ToString(), PreLang);
                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                    if (Gubun == "mod")
                    {
                        ltHeader.Text = GetResource("FolderMod");
                        CTBParentName.Text = (pName == "") ? GetResource("FolderRoot") : pName.ToReverseXSS();
                        CTBName.Text = dr["NameBase"].ToString().ToReverseXSS();
                        CTBMultiName.Text = this.GetMuiltiLang(dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString()).ToReverseXSS();
                    }
                    else
                    {
                        ltHeader.Text = GetResource("FolderAdd");
                        CTBParentName.Text = name.ToReverseXSS();
                    }
                }
            }
            else
            {
                ltHeader.Text = GetResource("FolderAdd");
                CTBParentName.Text = GetResource("FolderRoot");
            }
        }

        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 폴더 추가
        /// </summary>
        private void FolderAdd()
        {
            BE.CategoryInfo Info = new BE.CategoryInfo();
            Info.CompanyID = CurrentUserInfo.CompanyID;
            Info.ParentCategoryID = CategoryID;
            Info.WriteUserID = CurrentUserInfo.UserID;
            Info.WriteUserName = CurrentUserInfo.Name;
            Info.NameBase = CTBName.Text;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                Info.NameKo = MultiName.Split(',')[0];
                Info.NameEn = MultiName.Split(',')[1];
                Info.NameJa = MultiName.Split(',')[2];
                Info.NameFr = MultiName.Split(',')[3];
                Info.NameZhcn = MultiName.Split(',')[4];
                Info.NameZhtw = MultiName.Split(',')[5];
            }

            DAL.Category dal = new DAL.Category();

            if (dal.Add(Info) > 0)
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM.Admin.Folder.Add.Add()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
        
        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 폴더 수정
        /// </summary>
        private void FolderMod()
        {
            BE.CategoryInfo Info = new BE.CategoryInfo();
            Info.CompanyID = CurrentUserInfo.CompanyID;
            Info.CategoryID = CategoryID;            
            Info.NameBase = CTBName.Text;
            Info.ModifyUserID = CurrentUserInfo.UserID;
            Info.ModifyUserName = CurrentUserInfo.Name;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                Info.NameKo = MultiName.Split(',')[0];
                Info.NameEn = MultiName.Split(',')[1];
                Info.NameJa = MultiName.Split(',')[2];
                Info.NameFr = MultiName.Split(',')[3];
                Info.NameZhcn = MultiName.Split(',')[4];
                Info.NameZhtw = MultiName.Split(',')[5];
            }

            DAL.Category dal = new DAL.Category();

            if (dal.Mod(Info))
            {
                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM.Admin.Folder.Add.Mod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
        

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
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