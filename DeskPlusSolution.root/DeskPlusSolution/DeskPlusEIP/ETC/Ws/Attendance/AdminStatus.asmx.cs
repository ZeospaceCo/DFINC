using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.ETC.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.ETC.Web.Ws.Attendance
{
    /// <summary>
    /// AdminStatus의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AdminStatus : Base.BaseWSETC
    {
        #region 리스트

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-10
        /// 작성목적 : 페이징
        /// </summary>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태형황 일별 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusDListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();
                icnt = biz.GetStatusDListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), startDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-27
        /// 작성목적 : 근태현황 일별 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusDList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();

                DataTableReader dtr = biz.GetStatusDList(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(),
                    startDate, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1]);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceAdminStatusInfo info = new BE.AttendanceAdminStatusInfo();
                            #region 부서 다국어 설정
   
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.DeptCount = dtr["DeptCount"].ToString().ToNull();
                            info.HoliCount = dtr["HoliCount"].ToString().ToNull();
                            info.HoliPer = dtr["HoliPer"].ToString().ToNull();
                            info.TodayCount = dtr["TodayCount"].ToString().ToNull();
                            info.TodayPer = dtr["TodayPer"].ToString().ToNull();

                            info.WSTimeCount = dtr["WSTimeCount"].ToString().ToNull();
                            info.WSTimePer = dtr["WSTimePer"].ToString().ToNull();
                            info.WSTimeMIN = dtr["WSTimeMIN"].ToString().ToNull();
                            info.WSTimeMAX = dtr["WSTimeMAX"].ToString().ToNull();
                            info.WSTimeAVG = dtr["WSTimeAVG"].ToString().ToNull();

                            info.WSTimeCountLate = dtr["WSTimeCountLate"].ToString().ToNull();
                            info.WSTimePerLate = dtr["WSTimePerLate"].ToString().ToNull();
                            info.WSTimeMINLate = dtr["WSTimeMINLate"].ToString().ToNull();
                            info.WSTimeMAXLate = dtr["WSTimeMAXLate"].ToString().ToNull();
                            info.WSTimeAVGLate = dtr["WSTimeAVGLate"].ToString().ToNull();                            

                            info.WCTimeCount = dtr["WCTimeCount"].ToString().ToNull();
                            info.WCTimePer = dtr["WCTimePer"].ToString().ToNull();
                            info.WCTimeMIN = dtr["WCTimeMIN"].ToString().ToNull();
                            info.WCTimeMAX = dtr["WCTimeMAX"].ToString().ToNull();
                            info.WCTimeAVG = dtr["WCTimeAVG"].ToString().ToNull();

                            info.AbsenteeismCount = dtr["AbsenteeismCount"].ToString().ToNull();
                            info.AbsenteeismPer = dtr["AbsenteeismPer"].ToString().ToNull();
                            info.NowCount = dtr["NowCount"].ToString().ToNull();
                            info.NowPer = dtr["NowPer"].ToString().ToNull();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태형황 월별 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusMListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string statusMonth = GetDecryptParam("statusMonth", "");        //조회월
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();
                icnt = biz.GetStatusMListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, statusMonth, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-27
        /// 작성목적 : 근태현황 월별 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusMList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string statusMonth = GetDecryptParam("statusMonth", "");        //조회월
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();

                DataTableReader dtr = biz.GetStatusMList(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(),
                    statusYear, statusMonth, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1]);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceAdminStatusInfo info = new BE.AttendanceAdminStatusInfo();
                            #region 부서 다국어 설정

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.TodayCount = dtr["TodayCount"].ToString().ToNull();
                            info.TodayPer = dtr["TodayPer"].ToString().ToNull();
                            info.TodayMIN = dtr["TodayMIN"].ToString().ToNull();
                            info.TodayMAX = dtr["TodayMAX"].ToString().ToNull();
                            info.TodayAVG = dtr["TodayAVG"].ToString().ToNull();

                            info.WSTimeCount = dtr["WSTimeCount"].ToString().ToNull();
                            info.WSTimePer = dtr["WSTimePer"].ToString().ToNull();
                            info.WSTimeMIN = dtr["WSTimeMIN"].ToString().ToNull();
                            info.WSTimeMAX = dtr["WSTimeMAX"].ToString().ToNull();
                            info.WSTimeAVG = dtr["WSTimeAVG"].ToString().ToNull();

                            info.WSTimeCountLate = dtr["WSTimeCountLate"].ToString().ToNull();
                            info.WSTimePerLate = dtr["WSTimePerLate"].ToString().ToNull();
                            info.WSTimeMINLate = dtr["WSTimeMINLate"].ToString().ToNull();
                            info.WSTimeMAXLate = dtr["WSTimeMAXLate"].ToString().ToNull();
                            info.WSTimeAVGLate = dtr["WSTimeAVGLate"].ToString().ToNull();

                            info.WCTimeCount = dtr["WCTimeCount"].ToString().ToNull();
                            info.WCTimePer = dtr["WCTimePer"].ToString().ToNull();
                            info.WCTimeMIN = dtr["WCTimeMIN"].ToString().ToNull();
                            info.WCTimeMAX = dtr["WCTimeMAX"].ToString().ToNull();
                            info.WCTimeAVG = dtr["WCTimeAVG"].ToString().ToNull();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 근태형황 년별 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetStatusYListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();
                icnt = biz.GetStatusYListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), statusYear, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-28
        /// 작성목적 : 근태현황 년별 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetStatusYList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Ws.Attendance.AdminStatus).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string statusYear = GetDecryptParam("statusYear", "");          //조회년
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceAdminStatus biz = new DAL.AttendanceAdminStatus();

                DataTableReader dtr = biz.GetStatusYList(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(),
                    statusYear, searchField, searchValue,
                    setPageingStr[0], setPageingStr[1]);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.AttendanceAdminStatusInfo info = new BE.AttendanceAdminStatusInfo();
                            #region 부서 다국어 설정

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.TodayCount = dtr["TodayCount"].ToString().ToNull();
                            info.TodayPer = dtr["TodayPer"].ToString().ToNull();
                            info.TodayMIN = dtr["TodayMIN"].ToString().ToNull();
                            info.TodayMAX = dtr["TodayMAX"].ToString().ToNull();
                            info.TodayAVG = dtr["TodayAVG"].ToString().ToNull();

                            info.WSTimeCount = dtr["WSTimeCount"].ToString().ToNull();
                            info.WSTimePer = dtr["WSTimePer"].ToString().ToNull();
                            info.WSTimeMIN = dtr["WSTimeMIN"].ToString().ToNull();
                            info.WSTimeMAX = dtr["WSTimeMAX"].ToString().ToNull();
                            info.WSTimeAVG = dtr["WSTimeAVG"].ToString().ToNull();

                            info.WSTimeCountLate = dtr["WSTimeCountLate"].ToString().ToNull();
                            info.WSTimePerLate = dtr["WSTimePerLate"].ToString().ToNull();
                            info.WSTimeMINLate = dtr["WSTimeMINLate"].ToString().ToNull();
                            info.WSTimeMAXLate = dtr["WSTimeMAXLate"].ToString().ToNull();
                            info.WSTimeAVGLate = dtr["WSTimeAVGLate"].ToString().ToNull();

                            info.WCTimeCount = dtr["WCTimeCount"].ToString().ToNull();
                            info.WCTimePer = dtr["WCTimePer"].ToString().ToNull();
                            info.WCTimeMIN = dtr["WCTimeMIN"].ToString().ToNull();
                            info.WCTimeMAX = dtr["WCTimeMAX"].ToString().ToNull();
                            info.WCTimeAVG = dtr["WCTimeAVG"].ToString().ToNull();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-11-27
        /// 작성목적 : 사용자별 페이지사이즈 설정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "AdminStatusD");

                BE.FolderUserInfo info = new FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }        
        #endregion
    }
}
