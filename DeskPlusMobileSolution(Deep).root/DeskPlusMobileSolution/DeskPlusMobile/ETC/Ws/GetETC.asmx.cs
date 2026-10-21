using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Xml;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.ETC.Ws
{
    /// <summary>
    /// GetETC의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetETC : Helper.MobileBaseService
    {

        /// <summary>
        /// 사용자 검색
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUser(string p)
        {
            SetDecryptString(p);
            string searchValue = GetDecryptParam("Sv", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.ETC.Ws.GetETC).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar,500)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_ACC_SelName", "dsUser", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            StringBuilder sb = new StringBuilder();
                            BE.UserInfo info = new BE.UserInfo();

                            string name = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhTw"].ToString(), PreLang);
                            string deptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhTw"].ToString(), PreLang);
                            string titleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhTw"].ToString(), PreLang);

                            //겸직,사용자일 경우 -> basedept : 0 겸직, 1은 지정
                            if (dr["BaseDept"].ToString() == "0")
                            {
                                string baseDept = "(" + resMng.GetString("OrganAddtionJob") + ")";
                                info.NameText = name + baseDept + "/" + titleName + "/" + deptName;
                            }
                            else
                            {
                                info.NameText = name + "/" + titleName + "/" + deptName;
                            }

                            sb.Append(dr["UserID"].ToString());
                            sb.Append("|");
                            sb.Append(name);
                            sb.Append("|");
                            sb.Append(dr["DeptID"].ToString());
                            sb.Append("|");
                            sb.Append(deptName);

                            info.NameValue = sb.ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetUser()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-08-22
        /// 작성목적 : 특근신청서 가져오기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetWorkCount(string p)
        {
            SetDecryptString(p);
            string PageNo = GetDecryptParam("PageNo", "0");

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                                 new SqlParameter("@CompanyID", SqlDbType.Int)
                                                ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = UserInfo.CompanyID;
                parm[1].Value = UserInfo.UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_PAY_Work_SelPageCnt", parm);
                int Total = Convert.ToInt32(obj);

                int recsInLastPage = Total % 5;
                int TotalPage = 0;

                //마지막 페이지의 리스트 개수 구하기
                if (recsInLastPage == 0)
                {
                    TotalPage = (Total / 5);
                }
                else
                {
                    TotalPage = (Total / 5) + 1;
                }

                if (TotalPage < PageNo.ToNumber<int>())
                {
                    PageNo = "1";
                }

                return TotalPage.ToString() + "^" + PageNo.ToString() + "^" + Total.ToString();

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetWorkCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
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
        /// 작성자   : famo23
        /// 작성목적 : 특근신청서 리스트 가져오기 
        /// 작성일자 : 2019-08-22
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetWorkList(string p)
        {
            SetDecryptString(p);
            string totalCount = GetDecryptParam("TotalCount", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");

            try
            {
                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@PageSize", SqlDbType.Int)
                                            ,new SqlParameter("@PageSizePageNo", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = setPageingStr[0];
                parm[3].Value = setPageingStr[1];

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_PAY_Work_SelPageList", "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    WorkListEntity info = new WorkListEntity();
                    info.DocumentID = dr["Documentid"].ToString().ToNumber<int>();
                    info.WorkID = dr["WorkID"].ToString().ToNumber<int>();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.Subject = dr["Subject"].ToString();
                    info.NameBase = dr["NameBase"].ToString();

                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        info.WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetWorkList('" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 특근신청서 가져오기 
        /// 작성일자 : 2019-08-22
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetWorkID(string p)
        {
            SetDecryptString(p);
            string WorkID = GetDecryptParam("WorkID", "0");

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@WorkID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = WorkID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_PAY_Work_SelPageWorkList", "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    WorkListEntity info = new WorkListEntity();
                    info.WorkID = dr["WorkID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.DeptCodeID = dr["DeptCodeID"].ToString().ToNumber<int>();
                    info.CustomerName = dr["CustomerName"].ToString();
                    info.StartDate = dr["StartDate"].ToString();
                    info.EndDate = dr["EndDate"].ToString();
                    info.StartHour = dr["StartHour"].ToString().ToNumber<int>();
                    info.EndHour = dr["EndHour"].ToString().ToNumber<int>();
                    info.Duration = dr["Duration"].ToString().ToNumber<int>();
                    info.Hour = dr["Hour"].ToString().ToNumber<int>();
                    info.Reason = dr["Reason"].ToString();
                    info.ResonDetails = dr["ReasonDetails"].ToString();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetWorkID('" + WorkID + "')", ex);
                throw;
            }
        }
    }

    /// <summary>
    /// WorkListEntity
    /// </summary>
    [Serializable]
    public class WorkListEntity
    {
        public int CompanyID { get; set; }

        public int WorkID { get; set; }

        public int DocumentID { get; set; }

        public int UserID { get; set; }

        public int DeptCodeID { get; set; }

        public string CustomerName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int StartHour { get; set; }

        public int EndHour { get; set; }

        public float Duration { get; set; }

        public int Hour { get; set; }

        public string Reason { get; set; }

        public string ResonDetails { get; set; }

        public string NameBase { get; set; }

        public string DocNumber { get; set; }

        public string Subject { get; set; }

        public string WriteTime { get; set; }

    }
}
