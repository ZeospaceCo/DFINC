using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Menu
{
    /// <summary>
    /// TopMenu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class TopMenu : Base.BaseWSEIP
    {
        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-01 
        /// 작성목적 : 다국어에 맞게 XML 가져오기
        /// </summary>
        private DataSet GetDSAdminMenu()
        {
            DataSet dsAdminMenu = (DataSet)Session["dsAdminMenu"];

            if (dsAdminMenu == null)
            {
                dsAdminMenu = new DataSet();

                try
                {
                    string xmlPath = "";
                    try
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/EIP/App_Data/AdminMenu/" + PreLang + ".xml");
                    }
                    catch
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/EIP/App_Data/AdminMenu/en.xml");
                    }
                    dsAdminMenu.ReadXml(xmlPath);
                    Session["dsAdminMenu"] = dsAdminMenu;
                }
                finally
                {
                }
            }

            return dsAdminMenu;
        }

        /// <summary>
        /// 관리자 메인 메뉴 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminMenu()
        {
            List<object> listobj = new List<object>();

            DataRow[] drArr = GetDSAdminMenu().Tables[0].Select("ParentID = ''");

            try
            {
                foreach (DataRow dr in drArr)
                {
                    Zeospace.DeskPlus.EIP.BE.AdminMenuInfo info = new Zeospace.DeskPlus.EIP.BE.AdminMenuInfo();
                    info.ID = dr["ID"].ToString();
                    info.Text = dr["Text"].ToString();
                    info.ParentID = dr["ParentID"].ToString();
                    info.ClientSideCommand = dr["ClientSideCommand"].ToString();

                    if (info.ID == "COM") //시스템 관리자만 회사 관리를 할수 있다
                    {
                        if (CurrentUserInfo.IsSysAdmin == false) continue;
                    }

                    listobj.Add(info);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAdminSubMenu[_id] : " + ex);
            }

            return listobj;
        }

        /// <summary>
        ///  서브 메뉴 정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminSubMenu(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);
            string parentID = GetDecryptParam("ID", "");

            List<object> listobj = new List<object>();

            if (String.IsNullOrEmpty(parentID)) return listobj;


            DataRow[] drArr = GetDSAdminMenu().Tables[0].Select("ParentID = '" + parentID + "'");

            try
            {
                //서브메뉴 만들기
                foreach (DataRow dr in drArr)
                {
                    Zeospace.DeskPlus.EIP.BE.AdminMenuInfo info = new Zeospace.DeskPlus.EIP.BE.AdminMenuInfo();
                    info.ID = dr["ID"].ToString();
                    info.Text = dr["Text"].ToString();
                    info.ParentID = dr["ParentID"].ToString();
                    info.ClientSideCommand = dr["ClientSideCommand"].ToString();

                    //메일 도메인 경로 변경하기, shimpark
                    if (info.ClientSideCommand.ToLower().IndexOf("/zmail/") != -1)
                    {
                        string mailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
                        info.ClientSideCommand = string.Format(info.ClientSideCommand, mailServerIP);
                    }

                    listobj.Add(info);
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAdminSubMenu[_id] : " + ex);
            }

            return listobj;
        }
    }
}
