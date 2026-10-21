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

namespace Zeospace.DeskPlus.Pim.Web.ShortReply
{
    public partial class GetUser : Base.BasePagePim
    {
        #region 변수설정
        public int CatID = 0;//최상위 트리 나올 카테고리
        public int CategoryID = 0;//포커스 설정할 트리 카테고리
        public int pCategoryID = 0;//상위부서 카테고리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public int CompanyID = 0;
        public int UserID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ShortReply.GetUser).Assembly;	//어셈블리 선언
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
                UCUserDeptTreeScript1.UseCheckBoxMember = true;
                UCUserDeptTreeScript1.UseIcon = true;
                UCUserDeptTreeScript1.UseCheckBoxGroup = true;
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
                CbtnAdd.Text = GetResource("ADD");
            }
        }
       

        private void DataLoad()
        {
            

            DAL.OpenUserComment dal = new DAL.OpenUserComment();

            List<BE.OpenUserCommentInfo> info = new List<BE.OpenUserCommentInfo>();
            DataSet ds = dal.GetOpenUserCommentDs(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            
            string strTable = string.Empty;
            int iIndex = 1;
            StringBuilder sb = new StringBuilder();


            foreach (DataTable _dt in ds.Tables)
            {
                foreach (DataRow dtr in _dt.Rows)
                {
                    
                
                string strName = string.Empty;
                strTable += "<tr id='boardAuth" + iIndex + "'>";
                string Param = string.Empty;

                if (!dtr["GroupID"].ToString().Equals("0"))
                {
                    Param = "G$" + dtr["GroupID"].ToString();
                    strName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                    dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                    dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                    );
                }
                else
                {
                    Param = "M$" + dtr["OpenUserID"].ToString();

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
                }
                strTable += "<td class='center'>" + strName + "</td>";

                sb.Append(Param
                  + "$" + strName
                  + ";");
                //strTable += @"<td class='center'><a onclick=""GetUser.DelAuthRow(" + iIndex + ",'" + Param + "','" + strName + @"')""><img src='" + BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_del.gif' /></a></td>";
                strTable += @"<td class='center'><a class=btn onclick=""GetUser.DelAuthRow(" + iIndex + ",'" + Param + "','" + strName + @"')"">" + GetResource("Del") + "</a></td>";
                strTable += "</tr>";

                iIndex++;

                }
            }
            
            AuthTable = strTable;
            AuthArrayList = sb.ToString().TrimEnd(';');
        }

        private void AddOpenUser()
        {
            DAL.OpenUserComment dal = new DAL.OpenUserComment();

            //삭제
            BE.OpenUserCommentInfo be=new BE.OpenUserCommentInfo();
            be.CompanyID =CurrentUserInfo.CompanyID;
            be.UserID =CurrentUserInfo.UserID;
            dal.OpenUserCommentDel(be);

            //추가
            

            string [] strTemp = tbAuthValue.Text.Split('#');

            for (int i = 0; i < strTemp.Length; i++)
            {
                string [] temp1=strTemp[i].Split('|');
                if (temp1.Length > 1)
                {
                    string [] temp2 = temp1[1].Split('$');

                    if (temp2.Length > 0)
                    {
                        be.OpenUserID = int.Parse(temp2[1]);

                        if (temp2[0] == "M") //개인
                        {
                            be.OpenUserID = int.Parse(temp2[1]);
                            be.GroupID = 0;
                        }
                        else if (temp2[0] == "G")//그룹
                        {
                            be.OpenUserID = 0;
                            be.GroupID = int.Parse(temp2[1]);
                        }
                        dal.OpenUserCommentAdd(be);
                    }
                }
                
                
            }

            string strScript = "<script language='JScript'> if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }" + "<" + "/script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "close", strScript);


        }

        #endregion

        #region Event Handler (이벤트 핸들러)



        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            AddOpenUser();
        }



        protected void LBTNGetData_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}