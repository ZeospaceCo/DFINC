using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.Web.Share
{
    public partial class GetShare : Base.BasePageWebHard
    {
        #region 변수처리
        private string eventArgs = String.Empty;
        public int FolderID;
        public int UserID;
        public string ShareArrayList = "";
        public string ShareTable = "";
        public int RowMax = 1;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Share.GetShare).Assembly;	//어셈블리 선언
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
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            UserID = CurrentUserInfo.UserID;
            GetParam();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;

            LoadShare();
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-21
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");                     //"확인";
                CbtnClose.Text = resMng.GetString("Close");                         //"닫기";
                CbtnAdd.Text = resMng.GetString("Add");                             //"추가";
                CBSubDept.Text = resMng.GetString("GetSubDept");                    //"하위부서 가져오기";                   
                CbtnDel.Text = resMng.GetString("FolderShareDelete");       //"폴더공유해제";              
            }

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-27
        /// 작성목적 : Gets the param.
        /// </summary>
        private void GetParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-28
        /// 작성목적 : 해당 폴더를 공유한 사용자 정보 가져오기
        /// </summary>
        private void LoadShare()
        {
            DAL.ShareFolder dal = new DAL.ShareFolder();
            DataTableReader dtr = dal.GetShare(CurrentUserInfo.CompanyID, UserID , FolderID);
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
                            + "$" + dtr["TargetID"].ToString()
                            + "$" + name
                            + ";");

                        ShareTable += CreateTableShare(i, dtr["TargetType"].ToString() + "$" + dtr["TargetID"].ToString(),
                            name, dtr["Authority"].ToString().ToNumber<int>());

                        i += 1;
                        RowMax += 1;
                    }
                }
            }

            ShareArrayList = sb.ToString().TrimEnd(';');
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
                default :

                    break;
            }

            string tmpKeyID = _keyID.Replace("$","");

            string tempStr = "<tr id='webHardSend" + _idx + "'>"
            + "<td class='center'>"
            + _name
            + "</td>"
            + "<td class='center'><input type='checkbox' id='chxDown" + tmpKeyID + "'" + selected1 + " ></td>"
            + "<td class='center'><input type='checkbox' id='chxUpload" + tmpKeyID + "'" + selected2 + "></td>"
            + @"<td class='center'><a class='btn' onclick=""GetShare.DelAuthRow(" + _idx + ",'" + _keyID + @"')"">" + resMng.GetString("Del") + "</a></td>"
            //+ @"<td class='center'><a onclick=""GetShare.DelAuthRow(" + _idx + ",'" + _keyID + @"')""><img src='" + BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_del.gif' style='cursor:hand' /></a></td>"
            + "</tr>";


            return tempStr;
        }


        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}