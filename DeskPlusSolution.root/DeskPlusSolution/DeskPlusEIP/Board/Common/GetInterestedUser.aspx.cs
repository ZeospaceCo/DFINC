using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Common
{
    public partial class GetInterestedUser : Base.BasePageBoard
    {
        #region 변수설정
        public string InterestedUserArrayList = "";
        public string InterestedUserTable = "";
        public int CompanyID = 0;
        public int UserID = 0;

        public string Kind; //홈구분
        public string AccActive = "0"; //accordianActive
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.GetInterestedUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                #endregion

                #region 초기화
                SetLanguage();
                DataLoad();

                CompanyID = CurrentUserInfo.CompanyID;
                UserID = CurrentUserInfo.UserID;

                UCUserDeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
                UCUserDeptTreeScript1.DeptID = CurrentUserInfo.DeptID;
                UCUserDeptTreeScript1.UseActiveDept = true;                
                UCUserDeptTreeScript1.UseIcon = true;                
                UCUserDeptTreeScript1.UseCheckBoxGroup = false;
                UCUserDeptTreeScript1.UseCheckBoxMember = true;
                UCUserDeptTreeScript1.UseMyRootDept = false;
                UCUserDeptTreeScript1.UseVirtualTree = false;
                UCUserDeptTreeScript1.UseRootCheckBox = false;
                #endregion
            }
        }

        #endregion

        #region Method (함수)
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnSubmit.Text = GetResource("Confirm");
                CbtnClose.Text = GetResource("Close");
                CbtnAdd.Text = GetResource("Add");
            }
        }

        private void DataLoad()
        {
            LoadParam();

            DAL.InterestedUser dal = new DAL.InterestedUser();

            List<BE.InterestedUserInfo> info = new List<BE.InterestedUserInfo>();
            DataSet ds = dal.GetInterestedUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            string strTable = string.Empty;
            int iIndex = 1;
            StringBuilder sb = new StringBuilder();

            foreach (DataTable _dt in ds.Tables)
            {
                foreach (DataRow dtr in _dt.Rows)
                {
                    string strName = string.Empty;
                    strTable += "<tr id='boardInterestedUser" + iIndex + "'>";
                    string Param = string.Empty;

                    Param = dtr["InterestedUserID"].ToString();

                    strName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                    dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                    dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang
                    ) + "/" + this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                    dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                    dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                    ) + "/" + this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                    dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                    dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                    );
 
                    strTable += "<td class='center'>" + strName + "</td>";

                    sb.Append(Param
                      + "$" + strName
                      + ";");

                    strTable += @"<td class='center'><a class=btn onclick=""GetInterestedUser.DelInterestedUserRow(" + iIndex + ",'" + Param + "','" + strName + @"')"">" + GetResource("Del") + "</a></td>";
                    //strTable += @"<td class='center'><a  onclick=""GetInterestedUser.DelInterestedUserRow(" + iIndex + ",'" + Param + "','" + strName + @"')""><img src='" + BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_del.gif' /></a></td>";
                    strTable += "</tr>";

                    iIndex++;

                }
            }

            InterestedUserTable = strTable;
            InterestedUserArrayList = sb.ToString().TrimEnd(';');
        }
        #endregion

        /// <summary>
        /// 작성자  : 이지원
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            Kind = GetDecryptParam("kind", "");
            AccActive = GetDecryptParam("accActive", "0");
        }

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}