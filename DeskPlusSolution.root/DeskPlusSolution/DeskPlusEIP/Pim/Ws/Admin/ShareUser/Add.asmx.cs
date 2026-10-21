using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Admin.ShareUser
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSPim
    {

        #region Get

        /// <summary>
        /// 관리자 공유 추가 목록
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminSharePopList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Admin.ShareUser.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            List<object> listobj = new List<object>();

            int _CompanyID = 0;
            int _UserID = 0;

            SetDecryptString(p);
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _UserID = GetDecryptParam("UserID", "").ToString().ToNumber<int>();

            DAL.OpenUser dal = new OpenUser();

            DataTableReader dtr = dal.GetOpenUserListPop(_CompanyID, _UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.OpenUserInfo _info = new OpenUserInfo();

                        string _strname = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                          dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                          dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                          );
                        string _Deptname = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                            dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                            dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang
                            );
                        string _Titlename = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang
                            );

                        string _Dutyname = this.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(),
                            dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(),
                            dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang
                            );

                        _info.Name = _strname;
                        _info.DeptName = _Deptname;
                        _info.TitleName = _Titlename;
                        _info.DutyName = _Dutyname;
                        _info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        _info.kind = dtr["kind"].ToString();
                        _info.OpenUserID = dtr["OpenUserID"].ToString().ToNumber<int>();
                        _info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                        _info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        _info.ParentDeptID = dtr["ParentDeptID"].ToString().ToNumber<int>();
                        _info.DelStr = resMng.GetString("Del");
                        listobj.Add(_info);
                    }
                }
            }


            return listobj;
        }



        #endregion


        #region Set
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddOpenUserList(string p)
        {

            string strPara = string.Empty;
            int iAdduser = 0;

            SetDecryptString(p);
            strPara = GetDecryptParam("UserList", "");
            iAdduser = GetDecryptParam("AddUserID", "0").ToNumber<int>();

            string[] arUser = strPara.Split('|');

            DAL.OpenUser dalopen = new OpenUser();



            foreach (string _s in arUser)
            {
                string[] arVal = _s.Split('_');

                if (arVal.Length > 1)
                {
                    BE.OpenUserInfo info = new OpenUserInfo();

                    if (arVal[0] == "M") //유저
                    {
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = iAdduser;
                        info.OpenUserID = arVal[1].ToNumber<int>();
                        info.GroupID = 0;
                        info.kind = "G";
                    }

                    if (arVal[0] == "G") //그룹
                    {
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = iAdduser;
                        info.OpenUserID = 0;
                        info.GroupID = arVal[1].ToNumber<int>();
                        info.kind = "G";
                    }
                    dalopen.OpenUserAdd(info);
                }
            }



            return 1;
        }



        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelOpenuserAdmin(string p)
        {
            int CompanyID = 0;
            int OpenUserID = 0;
            int GroupID = 0;
            int UserID = 0;
            SetDecryptString(p);

            DAL.OpenUser dal = new OpenUser();
            CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            OpenUserID = GetDecryptParam("OpenUserID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("GroupID", "0").ToNumber<int>();
            UserID = GetDecryptParam("UserID", "0").ToNumber<int>();

            //public void OpenUserAdminDelUserid(int _companyID, int _UserID, int _OpenUserID)
            //public void OpenUserAdminDelDeptid(int _companyID, int _UserID, int _GroupID)
            //public void OpenUserAdminDelAllid(int _companyID, int _UserID)

            if (OpenUserID > 0)
                dal.OpenUserAdminDelUserid(CompanyID, UserID, OpenUserID);

            if (GroupID > 0)
                dal.OpenUserAdminDelDeptid(CompanyID, UserID, GroupID);

            if (OpenUserID == 0 && GroupID == 0)
                dal.OpenUserAdminDelAllid(CompanyID, UserID);

            return 1;
        }

        #endregion


    }
}
