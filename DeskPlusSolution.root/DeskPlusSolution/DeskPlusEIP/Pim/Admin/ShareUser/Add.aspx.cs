using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.Web.Admin.ShareUser
{
    public partial class Add : Base.BasePagePimAdmin
    {
        #region 변수설정
        public int CatID = 0;//최상위 트리 나올 카테고리
        public int CategoryID = 0;//포커스 설정할 트리 카테고리
        public int pCategoryID = 0;//상위부서 카테고리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public int CompanyID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.ShareUser.Add).Assembly;	//어셈블리 선언
            #endregion

            CompanyID = CurrentUserInfo.CompanyID;

            if (!IsPostBack)
            {
                #region 이미지 처리
                //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";                
                #endregion

                #region 초기화
                //DataLoad();
                SetLanguage();

                DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
                DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
                DeptUserTreeScript1.UseActiveDept = true;
                DeptUserTreeScript1.UseCheckBoxMember = true;
                DeptUserTreeScript1.UseIcon = true;
                DeptUserTreeScript1.UseCheckBoxGroup = true;
                DeptUserTreeScript1.UseMyRootDept = false;
                DeptUserTreeScript1.UseVirtualTree = false;
                DeptUserTreeScript1.UseRootCheckBox = true;

                #endregion
            }
        }

        #endregion

        #region Method (함수)

        //
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnAdd.Text = GetResource("ADD");
                cbtnDel.Text = GetResource("Del");
                CbtnClose.Text = GetResource("Close");
                CbtnConfirm.Text = GetResource("Confirm");

            }
        }






        private void AddOpenUser()
        {
            DAL.OpenUser dal = new DAL.OpenUser();

            //삭제
            BE.OpenUserInfo be = new BE.OpenUserInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            dal.OpenUserDel(be);

            //추가


            string[] strTemp = tbAuthValue.Text.Split('#');

            for (int i = 0; i < strTemp.Length; i++)
            {
                string[] temp1 = strTemp[i].Split('|');
                if (temp1.Length > 1)
                {
                    string[] temp2 = temp1[1].Split('$');

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
                        dal.OpenUserAdd(be);
                    }
                }


            }

            string strScript = "<script language='JScript'> if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }"
                + "<" + "/script>";

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



        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}