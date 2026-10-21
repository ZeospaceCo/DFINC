using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Public
{
    public partial class Add : Base.BasePageWebHard
    {
        #region 변수처리
        public int FolderID;
        public string Gubun = String.Empty;
        private string MultiName = String.Empty;

        public string PublicArrayList = "";
        public string PublicTable = "";
        public int RowMax = 1;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Folder.Add).Assembly;	//어셈블리 선언
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
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();            
            LoadFolderInfo();

            LoadTree();

            if (Gubun == "mod")
            {
                LoadPublic();
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Save");       //"확인";
                CbtnAdd.Text = resMng.GetString("Add");            //"추가";
            }

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "add");
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 선택한 폴더의 정보 가져오기
        /// </summary>
        private void LoadFolderInfo()
        {
            DataSet ds = null;

            DAL.Folder dal = new DAL.Folder();
            ds = dal.GetFolder(CurrentUserInfo.CompanyID, FolderID);


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string pName = this.GetLangName(dr["pNameBase"].ToString(), dr["pNameKo"].ToString(), dr["pNameEn"].ToString(), dr["pNameJa"].ToString(), dr["pNameFr"].ToString(), dr["pNameZhcn"].ToString(), dr["pNameZhtw"].ToString(), PreLang);

                string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                if (dr["UserID"].ToString() == "0")
                {
                    pName = (pName == "ROOT") ? GetResource("PublicFolder") : pName;
                    name = (name == "ROOT") ? GetResource("PublicFolder") : name;
                }
                else
                {
                    pName = (pName == "ROOT") ? GetResource("PrivateFolderName") : pName;
                    name = (name == "ROOT") ? GetResource("PrivateFolderName") : name;
                }


                if (Gubun == "mod")
                {
                    ltHeader.Text = GetResource("PublicFolderMod");
                    //ltHeader.Text = dr["NameBase"].ToString().ToNull().ToReverseXSS();
                    CTBParentName.Text = pName.ToReverseXSS();
                    CTBName.Text = dr["NameBase"].ToString().ToNull().ToReverseXSS();
                    CTBMultiName.Text = this.GetMuiltiLang(dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString()).ToReverseXSS();

                    CTBFolderLength.Text = dr["FolderLength"].ToString();
                    CTBFileLength.Text = dr["FileLength"].ToString();
                }
                else
                {
                    //ltHeader.Text = GetResource("PublicFolderAdd");
                    ltHeader.Text = GetResource("PublicFolder") + GetResource("Add");
                    CTBParentName.Text = name.ToReverseXSS();
                }
            }

        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-21
        /// 작성목적 : 트리
        /// </summary>
        private void LoadTree()
        {
            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-21
        /// 작성목적 : 사용자권한
        /// </summary>
        private void LoadPublic()
        {
            DAL.PublicFolder dal = new DAL.PublicFolder();
            DataTableReader dtr = dal.GetPublic(CurrentUserInfo.CompanyID, FolderID);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        #region 다국어 이름 설정
                        string name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        #endregion

                        sb.Append(dtr["TargetType"].ToString()
                            + "$" + dtr["UserID"].ToString()
                            + "$" + name
                            + ";");

                        PublicTable += CreateTableShare(i, dtr["TargetType"].ToString() + "$" + dtr["UserID"].ToString(),
                            name, dtr["Authority"].ToString().ToNumber<int>());

                        i += 1;
                        RowMax += 1;
                    }
                }
            }

            PublicArrayList = sb.ToString().TrimEnd(';');
        }

        /// <summary>
        /// 공유 사용자 정보 테이블 그리기
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_keyID"></param>
        /// <param name="_name"></param>
        /// <param name="_auth"></param>
        /// <returns></returns>
        private string CreateTableShare(int _idx, string _keyID, string _name, int _auth)
        {
            string selected1 = "";
            string selected2 = "";

            switch (_auth)
            {
                case 1:
                    selected1 = "checked";
                    break;
                case 2:
                    selected2 = "checked";
                    break;
                case 3:
                    selected1 = "checked";
                    selected2 = "checked";
                    break;
                default:

                    break;
            }

            string tmpKeyID = _keyID.Replace("$", "");

            string tempStr = "<tr id='webHardSend" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'><input type='checkbox' id='chxDown" + tmpKeyID + "'" + selected1 + " ></td>"
            + "<td class='center'><input type='checkbox' id='chxUpload" + tmpKeyID + "'" + selected2 + "></td>"
            + @"<td class='center'><a class='btn' onclick=""Add.DelAuthRow(" + _idx + ",'" + _keyID + @"')"">" + resMng.GetString("Del") + "</a></td>"
            //+ @"<td class='center'><a onclick=""Add.DelAuthRow(" + _idx + ",'" + _keyID + @"')""><img src='" + BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
            + "</tr>";


            return tempStr;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 폴더 추가
        /// </summary>
        private void FolderAdd()
        {
            WebHard.BE.FolderInfo folderInfo = new BE.FolderInfo();
            folderInfo.CompanyID = CurrentUserInfo.CompanyID;
            folderInfo.ParentFolderID = FolderID;
            folderInfo.UserID = 0;
            folderInfo.NameBase = CTBName.Text;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                folderInfo.NameKo = MultiName.Split(',')[0];
                folderInfo.NameEn = MultiName.Split(',')[1];
                folderInfo.NameJa = MultiName.Split(',')[2];
                folderInfo.NameFr = MultiName.Split(',')[3];
                folderInfo.NameZhcn = MultiName.Split(',')[4];
                folderInfo.NameZhtw = MultiName.Split(',')[5];
            }

            folderInfo.FolderLength = CTBFolderLength.Text.ToNumber<int>();
            folderInfo.FileLength = CTBFileLength.Text.ToNumber<int>();

            DAL.Folder dal = new DAL.Folder();

            string Auth = tbAuthValue.Text;
            int folderID = dal.Add(folderInfo);

            if (folderID > 0)
            {
                BLL.PublicFolder bll = new BLL.PublicFolder();
                bll.Add(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID, Auth);

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
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 폴더 수정
        /// </summary>
        private void FolderMod()
        {
            WebHard.BE.FolderInfo folderInfo = new BE.FolderInfo();
            folderInfo.CompanyID = CurrentUserInfo.CompanyID;
            folderInfo.FolderID = FolderID;
            folderInfo.UserID = CurrentUserInfo.UserID;
            folderInfo.NameBase = CTBName.Text;

            MultiName = CTBMultiName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                folderInfo.NameKo = MultiName.Split(',')[0];
                folderInfo.NameEn = MultiName.Split(',')[1];
                folderInfo.NameJa = MultiName.Split(',')[2];
                folderInfo.NameFr = MultiName.Split(',')[3];
                folderInfo.NameZhcn = MultiName.Split(',')[4];
                folderInfo.NameZhtw = MultiName.Split(',')[5];
            }

            folderInfo.FolderLength = CTBFolderLength.Text.ToNumber<int>();
            folderInfo.FileLength = CTBFileLength.Text.ToNumber<int>();

            DAL.Folder dal = new DAL.Folder();

            string Auth = tbAuthValue.Text;

            if (dal.Mod(folderInfo))
            {
                BLL.PublicFolder bll = new BLL.PublicFolder();

                bll.Add(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FolderID, Auth);

                string sc = "Add.GoTree();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {                
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebHard.Public.Add.Mod()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
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