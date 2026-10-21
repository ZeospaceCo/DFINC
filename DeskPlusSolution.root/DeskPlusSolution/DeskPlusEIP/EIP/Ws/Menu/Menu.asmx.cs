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
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Menu
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Menu : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 하위메뉴 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.MenuInfo> GetSubMenuList(string p)
        {
            SetDecryptString(p);
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            List<BE.MenuInfo> al = new List<MenuInfo>();
            if (menuID == 0) return al;

            HttpContext context = HttpContext.Current;
            string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            BLL.Menu biz = new BLL.Menu();
            DataView dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            if (dv == null) return al;

            BLL.MenuDataLink bizMDL = new BLL.MenuDataLink();
            DAL.MenuDataLink dal = new DAL.MenuDataLink();
            List<BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ParentMenuID"].ToString().ToNull().ToNumber<int>() == menuID)
                {
                    //string parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dv[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                    //string url = dv[i]["TargetURL"].ToString();
                    //if (!string.IsNullOrEmpty(parameters)) url = url + "?p=" + GetEncryptParam(parameters);

                    string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dv[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                    string url = dv[i]["TargetURL"].ToString();
                    string param = String.Empty;
                    string dcParam = String.Empty;
                    if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(dv[i]["DirectCall"].ToString())) url = url + "?";


                    //if (!string.IsNullOrEmpty(parameters[0])) param = "p=" + GetEncryptParam(parameters[0]);
                    //if (param.Length > 0) param += "&";
                    //if (!string.IsNullOrEmpty(parameters[1])) param = param + parameters[1];

                    if (!string.IsNullOrEmpty(dv[i]["DirectCall"].ToString()))
                    {
                        dcParam = "dcparm=" + dv[i]["DirectCall"].ToString();
                    }

                    if (!string.IsNullOrEmpty(parameters[0])) param = parameters[0];   // 메뉴 호출시 암호화된 파라미터
                    if (!string.IsNullOrEmpty(dcParam))  //dcparam이 존재하면 추가(호출 메뉴를 알기 위해서)
                    {
                        if (!string.IsNullOrEmpty(param)) param += "&";
                        param += dcParam;
                    }

                    if (!string.IsNullOrEmpty(param)) param = "p=" + GetEncryptParam(param);

                    if (!string.IsNullOrEmpty(parameters[1]))  // 메뉴 호출시 암호화되지 않은 파라미터 
                    {
                        if (!string.IsNullOrEmpty(param)) param += "&";
                        param = param + parameters[1];
                    }


                    url = url + param;


                    #region 다국어 이름 설정
                    string name = this.GetLangName(
                        dv[i]["NameBase"].ToString(),
                        dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang);
                    #endregion

                    BE.MenuInfo info = new MenuInfo();
                    info.CompanyID = dv[i]["CompanyID"].ToString().ToNumber<int>();
                    info.MenuID = dv[i]["MenuID"].ToString().ToNumber<int>();
                    info.MenuOrder = dv[i]["MenuOrder"].ToString().ToNumber<int>();
                    info.MenuName = name.ToReverseXSS();
                    info.TargetURL = url;
                    info.DirectCall = dv[i]["DirectCall"].ToString();
                    info.OpenURL = dv[i]["OpenURL"].ToString();
                    al.Add(info);
                }
            }

            return al;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 용도 : directcall로 메뉴 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.MenuInfo> GetMenuByDirectCall(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            string dcparam = GetDecryptParam("dcparam", "");

            DAL.Menu dal = new DAL.Menu();
            return dal.GetMenuByDirectCall(companyID, dcparam);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : basedept 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModUserBaseDept(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            int deptID = GetDecryptParam("deptID", "").ToNumber<int>();
            int titleID = GetDecryptParam("titleID", "").ToNumber<int>();
            int dutyID = GetDecryptParam("dutyID", "").ToNumber<int>();


            try
            {
                Zeospace.DeskPlus.Account.BE.UserDeptInfo be = new Zeospace.DeskPlus.Account.BE.UserDeptInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.DeptID = deptID;
                be.CodeTitleID = titleID;
                be.CodeDutyID = dutyID;
                be.BaseDept = "1";

                Zeospace.DeskPlus.Account.DAL.UserDept dal = new Account.DAL.UserDept();
                dal.ModBaseDept(be);

                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companyID);
                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companyID);

                #region 메일 업체 전송 (메일의 자동화 완성을 위해)

                Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
                List<UserInfo> userList = dalUser.GetUserInfo(companyID, userID, false, false);
                if (userList.Count > 0)
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory comBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                    comBiz.ChangeDeptUser(userList[0].MailID, userList[0].DeptID);
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModUserBaseDept : " + ex);
                throw;
            }

        }
       
    }
}

