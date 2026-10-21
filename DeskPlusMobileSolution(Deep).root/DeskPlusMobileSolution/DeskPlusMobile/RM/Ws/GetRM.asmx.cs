using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws
{
    /// <summary>
    /// GetRM의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetRM : Helper.MobileBaseService
    {

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCount(string p)
        {
            SetDecryptString(p);

            RM.Ws.Data.Reserve dal = new Data.Reserve();
            string tmpCount = dal.GetUsalbleListCount(CompanyID, UserID, "0", "").ToString();

            return tmpCount;
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 나의예약현황 리스트
        /// 작성일자 : 2017-06-12
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMyReserveList(string p)
        {
            SetDecryptString(p);
            string tmpDate = GetDecryptParam("TmpDate", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Ws.GetRM).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string proc = "";

                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_SelMyReserve");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@Tmpdate", SqlDbType.VarChar, 10)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = tmpDate;


                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsMyReserveList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.ReservationID = Convert.ToInt32(dr["ReservationID"].ToString());
                    info.Title = dr["Title"].ToString();
                    info.ReserveDate = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + " ~ " + Convert.ToDateTime(dr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                    info.Subject = dr["Subject"].ToString();
                    info.ModifyDateStr = Convert.ToDateTime(dr["ModifyDate"].ToString()).ToString();

                    info.Usable = dr["Usable"].ToString();

                    if (dr["Usable"].ToString() == "0")
                    {
                        //미승인
                        info.UsableStr = resMng.GetString("Usable0");
                    }
                    else if (dr["Usable"].ToString() == "1")
                    {
                        //승인
                        info.UsableStr = resMng.GetString("Usable1");
                    }
                    else
                    {
                        //거부
                        info.UsableStr = resMng.GetString("Usable2");
                    }

                    if ((dr["ReplyCount"].ToString() == "0") || (dr["ReplyCount"].ToString() == "") || (dr["ReplyCount"].ToString() == null))
                    {
                        info.ReplyCount = "";
                    }
                    else
                    {
                        info.ReplyCount = "[" + dr["ReplyCount"].ToString() + "]";
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetMyReserveList()", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약현황 리스트
        /// 작성일자 : 2017-06-12
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReserveList(string p)
        {
            SetDecryptString(p);
            string tmpDate = GetDecryptParam("TmpDate", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Ws.GetRM).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string proc = "";

                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_SelReserve");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@Tmpdate", SqlDbType.VarChar, 10)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = tmpDate;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsReserveList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.ReservationID = Convert.ToInt32(dr["ReservationID"].ToString());
                    info.Title = dr["Title"].ToString();
                    info.ReserveDate = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + " ~ " + Convert.ToDateTime(dr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                    info.Subject = dr["Subject"].ToString();

                    #region 이름 다국어 설정
                    string Name = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    string DeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    string TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.ModifyUserInfo = DeptName + " " + Name + " " + TitleName;
                    info.ModifyDateStr = Convert.ToDateTime(dr["ModifyDate"].ToString()).ToString();

                    info.Usable = dr["Usable"].ToString();

                    if (dr["Usable"].ToString() == "0")
                    {
                        //미승인
                        info.UsableStr = resMng.GetString("Usable0");
                    }
                    else if (dr["Usable"].ToString() == "1")
                    {
                        //승인
                        info.UsableStr = resMng.GetString("Usable1");
                    }
                    else
                    {
                        //거부
                        info.UsableStr = resMng.GetString("Usable2");
                    }


                    info.IsGuard = dr["IsGuard"].ToString();

                    //보안예약의 경우 해당 사용자가 내용을 볼 권한이 있는지 체크
                    if (info.IsGuard == "1")
                    {
                        string[] Roles = UserInfo.Roles;

                        bool isRole = false;

                        for (int i = 0; i < Roles.Length; i++)
                        {
                            if (Roles[i] == "admin" || Roles[i] == "resource")
                            {
                                isRole = true;
                                break;
                            }
                        }

                        if (int.Parse(dr["WriteUserID"].ToString()) == UserID || int.Parse(dr["ManagerUserID"].ToString()) == UserID || isRole)
                        {
                            //관리자나 작성자, 담당자의 경우 보안예약을 볼 수 있음                            
                            info.Auth = 1;
                        }
                        else
                        {
                            //관리자나 작성자가 아닌경우에는 공유자 여부 체크
                            info.Auth = CheckAuth(CompanyID, int.Parse(dr["ReservationID"].ToString()), UserID, UserInfo.DeptID);

                            if (info.Auth <= 0)
                            {
                                info.Subject = resMng.GetString("SecurityReservation");
                            }
                        }
                    }
                    else
                    {
                        info.Auth = 1;
                    }

                    if ((dr["ReplyCount"].ToString() == "0") || (dr["ReplyCount"].ToString() == "") || (dr["ReplyCount"].ToString() == null))
                    {
                        info.ReplyCount = "";
                    }
                    else
                    {
                        info.ReplyCount = "[" + dr["ReplyCount"].ToString() + "]";
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetReserveList()", ex);
                throw;
            }
        }




        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약현황 상태별 리스트
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUsalbleList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");
            string usable = GetDecryptParam("Usable", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Ws.GetRM).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string proc = "";

                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_SelUsable");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Usable", SqlDbType.Char, 1)
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = usable;
                parm[3].Value = pageNo;
                parm[4].Value = pageSize;
                parm[5].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsUsalbleList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.ReservationID = Convert.ToInt32(dr["ReservationID"].ToString());
                    info.Title = dr["Title"].ToString();
                    info.ReserveDate = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + " ~ " + Convert.ToDateTime(dr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                    info.Subject = dr["Subject"].ToString();

                    #region 이름 다국어 설정
                    string Name = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    string DeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    string TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.ModifyUserInfo = DeptName + " " + Name + " " + TitleName;
                    info.ModifyDateStr = Convert.ToDateTime(dr["ModifyDate"].ToString()).ToString();

                    info.Usable = dr["Usable"].ToString();

                    if (dr["Usable"].ToString() == "0")
                    {
                        //미승인
                        info.UsableStr = resMng.GetString("Usable0");
                    }
                    else if (dr["Usable"].ToString() == "1")
                    {
                        //승인
                        info.UsableStr = resMng.GetString("Usable1");
                    }
                    else
                    {
                        //거부
                        info.UsableStr = resMng.GetString("Usable2");
                    }

                    info.IsGuard = dr["IsGuard"].ToString();

                    if ((dr["ReplyCount"].ToString() == "0") || (dr["ReplyCount"].ToString() == "") || (dr["ReplyCount"].ToString() == null))
                    {
                        info.ReplyCount = "";
                    }
                    else
                    {
                        info.ReplyCount = "[" + dr["ReplyCount"].ToString() + "]";
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetUsalbleList()", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 권한 체크
        /// 작성일자 : 2017-06-12
        /// </summary>
        /// <returns></returns>      
        private int CheckAuth(int _companyID, int _reservationID, int _userID, int _deptID)
        {
            try
            {
                string proc = "";
                proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_CheckAuth");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@ReservationID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                };

                parm[0].Value = _companyID;
                parm[1].Value = _reservationID;
                parm[2].Value = _userID;
                parm[3].Value = _deptID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("CheckShareUser()", ex);
                return -1;
            }
        }



        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원을 사용할 수 있는지 확인(등록시)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UseResource(string p)
        {
            try
            {
                bool rtnVal = true;

                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");
                string qty = GetDecryptParam("qty", "");
                string usableQty = GetDecryptParam("usableQty", "");
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string sTime = GetDecryptParam("sTime", "");
                string eTime = GetDecryptParam("eTime", "");

                string startDateStr = DateHelpers.GetCultureDate(startDate.Split(',')[0] + " " + sTime, "yyyyMMddHHmm", PreLangReal);
                string endDateStr = DateHelpers.GetCultureDate(endDate.Split(',')[0] + " " + eTime, "yyyyMMddHHmm", PreLangReal);

                Ws.Data.Reserve dal = new Ws.Data.Reserve();
                DataSet ds = dal.GetUseResource(CompanyID, startDateStr, endDateStr, listID);

                int totReservationQty = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    totReservationQty += dr["Qty"].ToString().ToNumber<int>();

                    if (totReservationQty > 0)
                    {
                        int balanceQty = (usableQty.ToNumber<int>() - totReservationQty);

                        if (balanceQty < qty.ToNumber<int>())
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (usableQty.ToNumber<int>() < qty.ToNumber<int>())
                        {
                            return false;
                        }
                    }
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("UseResource()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약내용 가져오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetReserve(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("ReservationID", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Ws.GetRM).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_SelReserve");
            sqlQuery = String.Format(sqlQuery, CompanyID, reservationID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetReserve", null);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
                    DataColumn dc1 = new DataColumn("ReserveDate", typeof(string));
                    DataColumn dc2 = new DataColumn("ModifyUserInfo", typeof(string));
                    DataColumn dc3 = new DataColumn("ModifyDateStr", typeof(string));
                    DataColumn dc4 = new DataColumn("UsableStr", typeof(string));
                    DataColumn dc5 = new DataColumn("IsGuardStr", typeof(string));
                    DataColumn dc6 = new DataColumn("WriteYM", typeof(string));
                    DataColumn dc7 = new DataColumn("StartDateStr", typeof(string));
                    //DataColumn dc8 = new DataColumn("Status", typeof(string));
                    //DataColumn dc9 = new DataColumn("Status", typeof(string));
                    //DataColumn dc10 = new DataColumn("Status", typeof(string));

                    HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                    string content = ds.Tables[0].Rows[0]["Content"].ToString().Trim().ToEditorXSS();

                    hdoc.LoadHtml(content);
                    var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                    if (bodyNode != null)
                    {
                        dc0.DefaultValue = bodyNode.InnerHtml;
                    }
                    else
                    {
                        dc0.DefaultValue = content;
                    }
                    //내용
                    dt.Columns.Add(dc0);

                    //기간
                    dc1.DefaultValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + " ~ " + Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                    dt.Columns.Add(dc1);

                    #region 이름 다국어 설정
                    string Name = this.GetLangName(
                            ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    string DeptName = this.GetLangName(
                            ds.Tables[0].Rows[0]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptKo"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptJa"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptZhcn"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    string TitleName = this.GetLangName(
                            ds.Tables[0].Rows[0]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleKo"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleJa"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleZhcn"].ToString().ToNull(),
                            ds.Tables[0].Rows[0]["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    //신청자 정보
                    dc2.DefaultValue = DeptName + " " + Name + " " + TitleName;
                    dt.Columns.Add(dc2);

                    //신청일
                    dc3.DefaultValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifyDate"].ToString()).ToString();
                    dt.Columns.Add(dc3);

                    //상태 Usable
                    if (ds.Tables[0].Rows[0]["Usable"].ToString() == "0")
                    {
                        //미승인
                        dc4.DefaultValue = resMng.GetString("Usable0");
                    }
                    else if (ds.Tables[0].Rows[0]["Usable"].ToString() == "1")
                    {
                        //승인
                        dc4.DefaultValue = resMng.GetString("Usable1");
                    }
                    else
                    {
                        //거부
                        dc4.DefaultValue = resMng.GetString("Usable2");
                    }
                    dt.Columns.Add(dc4);

                    if (ds.Tables[0].Rows[0]["IsGuard"].ToString() == "0")
                    {
                        //일반
                        dc5.DefaultValue = resMng.GetString("TIsClose1");
                    }
                    else
                    {
                        //보안
                        dc5.DefaultValue = resMng.GetString("TIsClose2");
                    }
                    dt.Columns.Add(dc5);


                    DateTime dtWriteDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["WriteDate"].ToString());
                    string WriteYM = dtWriteDate.Year.ToString() + dtWriteDate.Month.ToString().PadLeft(2, '0');
                    dc6.DefaultValue = WriteYM;
                    dt.Columns.Add(dc6);

                    dc7.DefaultValue = ds.Tables[0].Rows[0]["StartDate"].ToString();
                    dt.Columns.Add(dc7);

                    return StringHelper.GetJSONString(dt);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 반복 일정이 있는지 체크
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetRepeat(string p)
        {
            try
            {
                bool rtnVal = false;

                SetDecryptString(p);
                string reservationID = GetDecryptParam("reservationID", "0");

                Data.Repeat dal = new Data.Repeat();
                DataTableReader dtr = dal.GetRepeat(CompanyID, reservationID.ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnVal = true;
                        }
                    }
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("UseResource()", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약 댓글 정보 가져오기
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReply(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("ReservationID", "0");

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_SelReply");
                sql = string.Format(sql, CompanyID.ToString(), reservationID.ToNumber<int>());
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "GetReply", null);

                List<object> listobj = new List<object>();

                string name = "";
                string deptName = "";
                string titleName = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ReservationID = dr["ReservationID"].ToString().ToNumber<int>();
                    info.ShortReplyID = dr["ShortReplyID"].ToString().ToNumber<int>();
                    info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();


                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    deptName = this.GetLangName(
                       dr["DeptBase"].ToString(),
                       dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                       dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(),
                       PreLang);

                    titleName = this.GetLangName(
                       dr["TitleBase"].ToString(),
                       dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                       dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(),
                       PreLang);

                    info.UserName = deptName + " " + titleName + " " + name;



                    DateTime dt;

                    if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                        info.InputDate = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                    else
                        info.InputDate = string.Empty;

                    #region 날짜 비교(New Img)
                    DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                    info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                    #endregion

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetReply('" + reservationID + "')", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약 거부사유 정보 가져오기
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReject(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("ReservationID", "0");

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_SelReject");
                sql = string.Format(sql, CompanyID.ToString(), reservationID.ToNumber<int>());
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "GetReject", null);

                List<object> listobj = new List<object>();

                string name = "";
                string deptName = "";
                string titleName = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.RejectInfo info = new BE.RejectInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ReservationID = dr["ReservationID"].ToString().ToNumber<int>();
                    info.RejectID = dr["RejectID"].ToString().ToNumber<int>();
                    info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();


                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    deptName = this.GetLangName(
                       dr["DeptBase"].ToString(),
                       dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                       dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(),
                       PreLang);

                    titleName = this.GetLangName(
                       dr["TitleBase"].ToString(),
                       dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                       dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(),
                       PreLang);

                    info.UserName = deptName + " " + titleName + " " + name;



                    DateTime dt;

                    if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                        info.InputDate = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                    else
                        info.InputDate = string.Empty;

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetReply('" + reservationID + "')", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 달력 가지고 오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCalendar(string p)
        {
            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
            int day = Convert.ToInt32(GetDecryptParam("Day", DateTime.Now.Day.ToString()));
            string gubun = GetDecryptParam("Gubun", "");

            int maxPrevDay = 0; // 이전달의 Max 날짜
            int maxDay = 0; //현재달의 Max 날짜
            int startDay = 0; //현재달의 시작 요일
            int realDay = 1; // 실제 날짜
            int nextDay = 1; // 다음날 시작 일자
            int countWeek = 0;

            try
            {
                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월 
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;


                for (int i = 0; i < 42; i++)
                {
                    info = new BE.CalendarInfo();

                    //요일 표시
                    info.Week = countWeek.ToString();
                    countWeek += 1;
                    if (countWeek % 7 == 0) countWeek = 0;

                    if (i < startDay) //지난달 표시
                    {
                        info.Year = dtPrev.Year.ToString();
                        info.Month = dtPrev.Month.ToString();
                        info.Day = (maxPrevDay + 1 - startDay + i).ToString();  //해당월의 날짜가 들어간다
                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " hol";
                        }

                        //오늘 날짜 Style 표시
                        if ((info.Day == DateTime.Now.Day.ToString()) && (info.Month == DateTime.Now.Month.ToString()) && (info.Year == DateTime.Now.Year.ToString()))
                        {
                            info.Class = " busy today";
                        }
                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }

                    info.Month = (Convert.ToInt32(info.Month) < 10) ? "0" + info.Month : info.Month;
                    info.Day = (Convert.ToInt32(info.Day) < 10) ? "0" + info.Day : info.Day;

                    string tmpDate = info.Year + "-" + info.Month + "-" + info.Day;
                    info.Func = "List.getList('" + tmpDate + "')";
                    info.DoCount = "";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }


                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CompanyID] == null)
                {
                    SetCacheHolidayList(CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

                List<string> Holidayobj = new List<string>();
                string tempDate = string.Empty;

                foreach (Pims.Ws.BE.HolidayRInfo _be in listobj2)
                {
                    string SolarLunar = _be.SolarLunar;                               //양음력구분
                    string HOLIDAY = _be.HoliDay;                                     //일자
                    string TITLE = _be.Title;                                         //제목
                    string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                    int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                    string IsReplace = _be.IsReplace;                                 //대체휴가구분
                    int IsReplaceDay = 0;                                             //대체휴가일자
                    string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                    DateTime sDate = DateTime.Parse(HOLIDAY);

                    //양력음력구분
                    if (SolarLunar != "0")
                    {
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    }

                    //대체휴일 확인
                    if (IsReplace == "1")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "1" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    if (i > 0)
                                    {
                                        IsReplaceDay = IsReplaceDay + 1;
                                    }
                                }
                            }

                            if (HolidayBetween == "2" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //대체휴일 + 토 확인
                    if (IsReplace == "2")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "0")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }
                                else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }

                            if (HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //일자 저장
                    Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                    //비교 데이터 저장
                    if (IsHolidayKind == "0")
                    {
                        tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                    }

                    if (HolidayBetween == "1")
                    {
                        //이전일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "2")
                    {
                        //다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "3")
                    {
                        //이전다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }

                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }

                    if (IsReplaceDay > 0)
                    {
                        int IsReplaceDayStart = HolidayBetweenDay + 1;
                        int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                        for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }
                    }
                }

                string Holiday = string.Join(",", Holidayobj);

                string rmMonth = year.ToString() + "-" + month.ToString() + "-01";
                DataTable dtMonth = GetMonthList(gubun, rmMonth);

                //달력에 일정 및 공휴일 표시 
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    // 공휴일 
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    foreach (string hol in Holiday.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol)
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                            {
                                calInfo.Class = " hol";
                            }
                            else
                            {
                                calInfo.Class = " dim_hol";
                            }
                        }
                    }

                    string tmpDate = calInfo.Year + "-" + calInfo.Month + "-" + calInfo.Day;
                    DataRow[] dr = dtMonth.Select("StartDate <= '" + tmpDate + " 23:59:59.000' AND '" + tmpDate + " 00:00:00.000' <= EndDate");

                    if (dr.Length > 0)
                    {
                        if (Convert.ToInt32(calInfo.Month) == month)
                        {
                            if (calInfo.Class.IndexOf("hol") > 0)
                            {
                                calInfo.Class = " busy hol";
                            }
                            else
                            {
                                calInfo.Class = "busy";
                            }

                            //일요일 Style 표시
                            if (calInfo.Week == "0")
                            {
                                calInfo.Class = " busy hol";
                            }
                        }
                        else
                        {
                            if (calInfo.Class.IndexOf("hol") > 0)
                            {
                                calInfo.Class = " busy dim_hol";
                            }
                            else
                            {
                                calInfo.Class = "dim busy";
                            }

                            //일요일 Style 표시
                            if (calInfo.Week == "0")
                            {
                                calInfo.Class = " busy dim_hol";
                            }
                        }

                        calInfo.DoCount = dr.Length.ToString();
                    }


                    //오늘 날짜 Style 표시
                    if ((Convert.ToInt32(calInfo.Day) == DateTime.Now.Day) && (Convert.ToInt32(calInfo.Month) == DateTime.Now.Month) && (Convert.ToInt32(calInfo.Year) == DateTime.Now.Year))
                    {
                        if (calInfo.Class.IndexOf("hol") > 0)
                        {
                            calInfo.Class = " busy holtoday";
                        }
                        else
                        {
                            calInfo.Class = " busy today";
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCalendar()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    SqlParameter[] parm = new SqlParameter[]{
                                new SqlParameter("@CompanyID", SqlDbType.Int)
                            ,new SqlParameter("@SelectYear", SqlDbType.Int)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = _UserSelectedYear.ToNumber<int>();

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_HolidayR_SelByYear");
                    sql = string.Format(sql, _companyID, _UserSelectedYear.ToNumber<int>());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsYear", parm);

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {
                        Pims.Ws.BE.HolidayRInfo _data = new Pims.Ws.BE.HolidayRInfo();

                        _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                        _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                        _data.Title = dtr["TITLE"].ToString();                                         //제목
                        _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                        _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                        _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                        _data.IsReplaceDay = 0;                                                        //대체휴가일자
                        _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                        listobj.Add(_data);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }


        /// <summary>
        ///  요일을 숫자료 변환
        /// </summary>
        /// <param name="_dow"></param>
        /// <returns></returns>
        private int GetCountOfWeek(DayOfWeek _dow)
        {
            int rtn = 0;

            switch (_dow)
            {
                case DayOfWeek.Sunday: rtn = 0; break;
                case DayOfWeek.Monday: rtn = 1; break;
                case DayOfWeek.Tuesday: rtn = 2; break;
                case DayOfWeek.Wednesday: rtn = 3; break;
                case DayOfWeek.Thursday: rtn = 4; break;
                case DayOfWeek.Friday: rtn = 5; break;
                case DayOfWeek.Saturday: rtn = 6; break;
            }

            return rtn;
        }

        /// <summary>
        /// 월별 날짜수 계산
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetDayCountOfMonth(int year, int month)
        {
            int leapYear = 0;
            int rtn = 0;

            //윤년 체크
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) leapYear = 1;

            switch (month)
            {
                case 1: rtn = 31; break;
                case 2: rtn = 28 + leapYear; break;
                case 3: rtn = 31; break;
                case 4: rtn = 30; break;
                case 5: rtn = 31; break;
                case 6: rtn = 30; break;
                case 7: rtn = 31; break;
                case 8: rtn = 31; break;
                case 9: rtn = 30; break;
                case 10: rtn = 31; break;
                case 11: rtn = 30; break;
                case 12: rtn = 31; break;
                default: rtn = -1; break;
            }
            return rtn;
        }

        /// <summary>
        /// 월별일정 가져오기
        /// </summary>
        /// <param name="_gubun"></param>
        /// <param name="_rmMonth"></param>
        /// <returns></returns>
        private DataTable GetMonthList(string _gubun, string _rmMonth)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_SelMonth");

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@RMMonth", SqlDbType.VarChar, 10)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = _gubun;
                parm[3].Value = _rmMonth;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsMonthList", parm);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetMonthList()", ex);
                throw;
            }
        }


    }
}
