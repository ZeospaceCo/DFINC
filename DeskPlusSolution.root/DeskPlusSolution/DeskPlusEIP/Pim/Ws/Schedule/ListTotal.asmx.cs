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

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListTotal : Base.BaseWSPim
    {

        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUserScheduleList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.ListTotal).Assembly;	//어셈블리 선언
            #endregion
            this.GetResourceManager();

            List<object> listobj = new List<object>();

            int _UserID = 0;
            int _CompanyID = 0;
            int _pagesize = 0;
            int _pagesizepageno = 0;

            string _UserList = string.Empty;
            string _btitle = string.Empty;
            string _OpenType = string.Empty;
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;

            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _OpenType = GetDecryptParam("OpenType", "");
            _UserList = GetDecryptParam("UserList", "");

            if (_UserList.Length > 0)
            {
                string[] arUser = _UserList.Split(',');
                _UserList = string.Empty;

                for (int i = 0; i < arUser.Length; i++)
                {
                    if (arUser[i].Length > 0)
                    {
                        string[] arFor = arUser[i].Split('$');
                        _UserList += arFor[1] + ",";
                    }

                }
            }
            else
            {
                return listobj;
            }

            switch (GetDecryptParam("StartDate", "").ToString())
            {

                case "0":
                    _StartDate = _StartDate = DateTime.Parse("1901-01-01");
                    _EndDate = DateTime.Now.AddDays(1);
                    break;
                case "W":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Now;
                    break;
                case "M":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Now;
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                    break;
            }


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

            _btitle = GetDecryptParam("title", "").ToString().Trim();


            DAL.Schedule biz = new DAL.Schedule();
            DataTable dt = new DataTable();
            DataTableReader dtr = new DataTableReader(dt);

            dtr = biz.GetUserListSearch(iRecordPage, iPage, _UserID, _CompanyID, _StartDate, _EndDate, _OpenType, _UserList, _btitle);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();

                        data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        data.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                        string _name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                            );
                        string _title = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(),
                            dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(),
                            dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang
                            );
                        data.UserName = _name + "(" + _title + ")";
                        data.Title = dtr["Title"].ToString();

                        data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("yyyy-MM-dd") + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd");
                        data.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        //종류

                        data.OpenType = dtr["Kind"].ToString();

                        switch (dtr["Kind"].ToString())
                        {
                            case "-1"://나의일정
                                data.KindDesk = GetResource("MYSCHEDULE");
                                break;
                            case "0"://개인일정
                                data.KindDesk = GetResource("pim1");
                                break;
                            case "3"://부서일정
                                data.KindDesk = GetResource("pim2");
                                break;
                            case "1"://공유일정
                                data.KindDesk = GetResource("pim3");
                                break;
                            case "2"://전체일정
                                data.KindDesk = GetResource("pim4");
                                break;
                            case "8"://전체일정
                                data.KindDesk = GetResource("TODO");
                                break;
                        }


                        listobj.Add(data);
                    }
                }
            }


            return listobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUserScheduleListCnt(string p)
        {
            int iRt = 0;

            int _UserID = 0;
            int _CompanyID = 0;


            string _UserList = string.Empty;
            string _btitle = string.Empty;
            string _OpenType = string.Empty;
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;

            SetDecryptString(p);

            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            _OpenType = GetDecryptParam("OpenType", "");
            _UserList = GetDecryptParam("UserList", "");

            if (_UserList.Length > 0)
            {
                string[] arUser = _UserList.Split(',');
                _UserList = string.Empty;

                for (int i = 0; i < arUser.Length; i++)
                {
                    if (arUser[i].Length > 0)
                    {
                        string[] arFor = arUser[i].Split('$');
                        _UserList += arFor[1] + ",";
                    }

                }
            }
            else
            {
                return iRt;
            }
            switch (GetDecryptParam("StartDate", "").ToString())
            {

                case "0":
                    _StartDate = _StartDate = DateTime.Parse("1901-01-01");
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "W":
                    _StartDate = DateTime.Now.AddDays(-7);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                case "M":
                    _StartDate = DateTime.Now.AddMonths(-1);
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));
                    break;
                default:
                    _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
                    _EndDate = DateTime.Parse(GetDecryptParam("EndDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 23:59")));
                    break;
            }
            _btitle = GetDecryptParam("title", "").ToString().Trim();

            DAL.Schedule biz = new DAL.Schedule();
            iRt = biz.GetUserListSearchCnt(_UserID, _CompanyID, _StartDate, _EndDate, _OpenType, _UserList, _btitle);

            return iRt;
        }

        #endregion

        #region Set
        #endregion
    }
}
