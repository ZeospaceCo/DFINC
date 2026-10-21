using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Task.DAL;

namespace Zeospace.DeskPlus.Task.Web.Admin.Menu
{
    public partial class ItemSubUser : Base.BasePageTaskAdmin
    {
        #region 변수설정
        public string SubToArrayList = "";
        public string SubToTable = "";
        public int ToRowMax = 0;

        public string SubCcArrayList = "";
        public string SubCcTable = "";
        public int CcRowMax = 0;
        private string eventArgs = String.Empty;
        #endregion
        
        
        #region Page Process (페이지로드)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.ItemSubUser).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }


        #endregion


        #region DataLoad(컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-14
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnSave.Text = resMng.GetString("Btn_Save");
                CbtnToAdd.Text = resMng.GetString("SpecifierUserAdd");
                CbtnCcAdd.Text = resMng.GetString("SpecifierUserAdd");
                CbtnCancel.Text = resMng.GetString("Close");
            }

            //CbtnToAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
            //CbtnCcAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-14
        /// 작성목적 : 데이터로드
        /// </summary>
        private void LoadData()
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

            LoadItemSubToUser();
            LoadItemSubCcUser();

        }


        /// <summary>
        /// 소분류 수신 지정자 가져오기
        /// </summary>
        public void LoadItemSubToUser()
        {
            DAL.ItemSubUser itemSubUser = new DAL.ItemSubUser();

            DataTableReader dtr = itemSubUser.GetList(CurrentUserInfo.CompanyID, ItemSubID);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        if (dtr["IsType"].ToString().ToUpper() == "TO")//수신 사용자만 가져오기
                        {
                            #region 다국어 이름 설정
                            string name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(),
                                dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(),
                                dtr["NameZhCn"].ToString(),
                                dtr["NameZhTw"].ToString(), PreLang);

                            string deptname = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(),
                                dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(),
                                dtr["DeptzhCn"].ToString(),
                                dtr["DeptZhTw"].ToString(), PreLang);

                            string titleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(),
                                dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(),
                                dtr["TitlezhCn"].ToString(),
                                dtr["TitleZhTw"].ToString(), PreLang);
                            #endregion

                            //(G$1$영업부)
                            sb.Append(dtr["IsGroup"].ToString()
                                + "$" + dtr["UserID"].ToString()
                                + "$" + dtr["RDeptID"].ToString()
                                + "$" + name
                                + ";");

                            SubToTable += CreateToTableAuth(i, dtr["IsGroup"].ToString() + "$" + dtr["UserID"].ToString(), name, deptname, titleName);

                            i += 1;
                        }
                    }

                    ToRowMax = i;
                }
            }

            SubToArrayList = sb.ToString().TrimEnd(';');

        }

        /// <summary>
        /// 소분류 참조 지정자 가져오기
        /// </summary>
        public void LoadItemSubCcUser()
        {
            DAL.ItemSubUser itemSubUser = new DAL.ItemSubUser();

            DataTableReader dtr = itemSubUser.GetList(CurrentUserInfo.CompanyID, ItemSubID);
            StringBuilder sb = new StringBuilder();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        if (dtr["IsType"].ToString().ToUpper() == "CC")//참조 사용자만 가져오기
                        {
                            #region 다국어 이름 설정
                            string name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(),
                                dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(),
                                dtr["NameZhCn"].ToString(),
                                dtr["NameZhTw"].ToString(), PreLang);

                            string deptname = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(),
                                dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(),
                                dtr["DeptzhCn"].ToString(),
                                dtr["DeptZhTw"].ToString(), PreLang);

                            string titleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(),
                                dtr["TitleEn"].ToString(),
                                dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(),
                                dtr["TitlezhCn"].ToString(),
                                dtr["TitleZhTw"].ToString(), PreLang);
                            #endregion
                            if (dtr["IsGroup"].ToString().ToUpper() == "G")
                            {
                                sb.Append(dtr["IsGroup"].ToString()
                                    + "$" + dtr["RDeptID"].ToString()
                                    + "$" + dtr["RDeptID"].ToString()
                                    + "$" + name
                                    + ";");
                                SubCcTable += CreateCcTableAuth(i, dtr["IsGroup"].ToString() + "$" + dtr["RDeptID"].ToString(), name, name, titleName);
                            }
                            else
                            {
                                sb.Append(dtr["IsGroup"].ToString()
                                    + "$" + dtr["UserID"].ToString()
                                    + "$" + dtr["RDeptID"].ToString()
                                    + "$" + name
                                    + ";");
                                SubCcTable += CreateCcTableAuth(i, dtr["IsGroup"].ToString() + "$" + dtr["UserID"].ToString(), name, deptname, titleName);
                            }                            

                            i += 1;
                        }
                    }

                    CcRowMax = i;
                }
            }

            SubCcArrayList = sb.ToString().TrimEnd(';');

        }
        #endregion


        #region 함수
        /// <summary>
        /// 수신 테이블 그리기
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_keyID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        private string CreateToTableAuth(int _idx, string _keyID, string _name, string _deptName, string _titleName)
        {

            string tempStr = "<tr id='itemSubUserTo" + _idx + "'>"
            + "<td>"
            + _deptName
            + " / "
            + _titleName
            + " / "
            + _name
            + "</td>"
            //+ @"<td class='center'><a onclick=""ItemSubUser.DelToAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
            + @"<td class='center'><a class=btn onclick=""ItemSubUser.DelToAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"
            + "</tr>";

            return tempStr;
        }

        /// <summary>
        /// 참조 테이블 그리기
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_keyID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        private string CreateCcTableAuth(int _idx, string _keyID, string _name, string _deptName, string _titleName)
        {
            string tempStr = "";
            if (_titleName.Length > 0)
            {
                tempStr = "<tr id='itemSubUserCc" + _idx + "'>"
                  + "<td>"
                  + _deptName
                  + " / "
                  + _titleName
                  + " / "
                  + _name
                  + "</td>"
                  //+ @"<td class='center'><a onclick=""ItemSubUser.DelCcAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
                  + @"<td class='center'><a class=btn onclick=""ItemSubUser.DelCcAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"
                  + "</tr>";
            }
            else
            {
                tempStr = "<tr id='itemSubUserCc" + _idx + "'>"
                  + "<td>"
                  + _deptName
                  + "</td>"
                  //+ @"<td class='center'><a onclick=""ItemSubUser.DelCcAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')""><img src='" + BaseImgPath + "/Lang/" + PreLang + "/ico_folderset_del.gif' /></a></td>"
                  + @"<td class='center'><a class=btn onclick=""ItemSubUser.DelCcAuthRow(" + _idx + ",'" + _keyID + "','" + _name + @"')"">" + resMng.GetString("Del") + "</a></td>"
                  + "</tr>";
            }

            return tempStr;
        }
        #endregion


        #region 이벤트핸들러
        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            Register();
        }

        /// <summary>
        /// 사용자 저장하기
        /// </summary>
        private void Register()
        {

            DAL.ItemSubUser dal = new DAL.ItemSubUser();

            string toUser = tbItemSubToUserValue.Text;
            string ccUser = tbItemSubCcUserValue.Text;

            if (dal.DelItemSubUser(CurrentUserInfo.CompanyID, ItemSubID))
            {
                //수신자 정보
                if (toUser.Length > 0)
                {
                    foreach (string _toUser in toUser.Split(','))
                    {
                        //구분$키값$이름
                        string[] authValStr = _toUser.Split('$');

                        BE.ItemUser itemToUser = new BE.ItemUser();
                        itemToUser.CompanyID = CurrentUserInfo.CompanyID;
                        itemToUser.ItemID = ItemSubID;
                        itemToUser.UserID = authValStr[1].ToNumber<int>();
                        itemToUser.DeptID = authValStr[2].ToNumber<int>();
                        itemToUser.IsGroup = "M";
                        itemToUser.IsType = "TO";
                        itemToUser.WriteUserID = CurrentUserInfo.UserID;

                        dal.SubAdd(itemToUser);
                    }
                }


                //참조자 정보
                if (ccUser.Length > 0)
                {
                    foreach (string _ccUser in ccUser.Split(','))
                    {
                        //구분$키값$이름
                        string[] authValStr = _ccUser.Split('$');

                        BE.ItemUser itemCcUser = new BE.ItemUser();
                        itemCcUser.CompanyID = CurrentUserInfo.CompanyID;
                        itemCcUser.ItemID = ItemSubID;
                        itemCcUser.IsGroup = authValStr[0].ToString();
                        if (authValStr[0].ToString().ToUpper() == "G")
                        {
                            itemCcUser.UserID = 0;
                        }
                        else
                        {
                            itemCcUser.UserID = authValStr[1].ToNumber<int>();
                        }

                        itemCcUser.DeptID = authValStr[2].ToNumber<int>();
                        itemCcUser.IsType = "CC";
                        itemCcUser.WriteUserID = CurrentUserInfo.UserID;


                        dal.SubAdd(itemCcUser);
                    }
                }
            }

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteConfirm"), "location='" + Request.Url.ToString() + "';");

        }
        #endregion


    }
}