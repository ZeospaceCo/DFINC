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
using Zeospace.DeskPlus.Pim.DAL;
using Zeospace.DeskPlus.Pim.BE;

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
    public class List : Base.BaseWSPim
    {
        #region Get
        /// <summary>
        /// 관리자 일정공유 리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminShareList(string p)
        {
            List<object> listobj = new List<object>();

            int _CompanyID = 0;
            int _pagesize = 0;
            int _pagesizepageno = 0;

            string _Name = string.Empty;
            string _kind = string.Empty;

            SetDecryptString(p);
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _Name = GetDecryptParam("NAME", "").ToString();
            _kind = GetDecryptParam("KIND", "P").ToString();
            _pagesize = GetDecryptParam("PageSize", "0").ToNumber<int>();
            _pagesizepageno = GetDecryptParam("PageSizePageNo", "0").ToNumber<int>();

            int iPage = 0;
            int iRecordPage = 0;

            if (_pagesizepageno == 1)
            {
                iPage = 0;
                iRecordPage = _pagesize;
            }
            else
            {
                iRecordPage = (_pagesizepageno) * _pagesize;
                iPage = ((_pagesizepageno - 1) * _pagesize) + 1;

            }
            DAL.OpenUser dal = new OpenUser();


            DataTableReader dtr = dal.GetAdminShareListInfo(iPage, iRecordPage, _CompanyID, _Name, _kind);
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
                        listobj.Add(_info);
                    }
                }
            }


            return listobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetAdminShareListCnt(string p)
        {
            int _CompanyID = 0;
            string _Name = string.Empty;
            string _kind = string.Empty;
            SetDecryptString(p);
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _Name = GetDecryptParam("NAME", "").ToString();
            _kind = GetDecryptParam("KIND", "P").ToString();
            DAL.OpenUser dal = new OpenUser();

            return dal.GetAdminShareListInfoCnt(_CompanyID, _Name, _kind);


        }


        #endregion

        #region Set
        #endregion
    }
}
