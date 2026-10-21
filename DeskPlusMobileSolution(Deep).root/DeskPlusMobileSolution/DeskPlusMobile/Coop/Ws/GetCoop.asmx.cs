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
using Zeospace.DeskPlusMobile.Web.Coop.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws
{
    /// <summary>
    /// GetCoop의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetCoop : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 업무협업 카운트 가져오기
        /// 작성일자 : 2015-03-30
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCount(string p)
        {
            SetDecryptString(p);
            string companyID = GetDecryptParam("CompanyID", "0");
            string userID = GetDecryptParam("UserID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_CNT");
            SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
            parm[0].Value = CompanyID;
            parm[1].Value = UserID;


            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsList", parm);

            int tot = 0;
            int today = 0;
            int balance = 0;

            // 요청한 협업 
            DataRow[] dr = ds.Tables["dsList"].Select("CAT = 'REQ'");
            string tmpCount = "(" + dr[0]["CNT"].ToString() + ")^";

            // 요청 받은 협업
            dr = ds.Tables["dsList"].Select("CAT = 'REC'"); // 전체갯수
            tot = dr[0]["CNT"].ToString().ToNumber<int>();

            dr = ds.Tables["dsList"].Select("CAT = 'RECT'"); //읽은 갯수
            today = dr[0]["CNT"].ToString().ToNumber<int>();

            balance = tot - today;
            tmpCount += "(" + balance + "/" + tot + ")^";

            // 참조 받은 협업
            dr = ds.Tables["dsList"].Select("CAT = 'REF'"); // 전체갯수
            tot = dr[0]["CNT"].ToString().ToNumber<int>();

            dr = ds.Tables["dsList"].Select("CAT = 'REFT'"); //읽은 갯수
            today = dr[0]["CNT"].ToString().ToNumber<int>();

            balance = tot - today;
            tmpCount += "(" + balance + "/" + tot + ")^";

            // 전달 받은 협업
            dr = ds.Tables["dsList"].Select("CAT = 'FOR'"); // 전체갯수
            tot = dr[0]["CNT"].ToString().ToNumber<int>();

            dr = ds.Tables["dsList"].Select("CAT = 'FORT'"); //읽은 갯수
            today = dr[0]["CNT"].ToString().ToNumber<int>();

            balance = tot - today;
            tmpCount += "(" + balance + "/" + tot + ")";

            return tmpCount;

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 업무협업 리스트 가져오기
        /// 작성일자 : 2015-03-30
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");            
            string searchValue = GetDecryptParam("Sv", "");


            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.GetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();
            
            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "request": //요청한 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_SelectREQ");
                        break;
                    case "receive": //요청 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_SelectREC");
                        break; 
                    case "reference": //참조 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_SelectREF");
                        break;
                    case "forward": // 전달 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_SelectFOR");
                        break;
                }
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = pageNo;
                parm[3].Value = pageSize;
                parm[4].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ListID = dr["ListID"].ToString().ToNumber<int>();
                    info.GroupID = dr["GroupID"].ToString().ToNumber<int>();

                    info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();                  

                    //1:대기;2:접수;3:처리중;4:완료;5:반려                    
                    info.Status = dr["StatusLast"].ToString();

                    if (bPage == "forward")
                    {
                        info.ForwardListID = dr["ForwardListID"].ToString().ToNumber<int>();
                        info.StatusStr = "";
                    }
                    else
                    {
                        info.ForwardListID = 0;
                        switch (dr["StatusLast"].ToString())
                        {
                            case "1":
                                info.StatusStr = resMng.GetString("CoopStatus1");
                                break;
                            case "2":
                                info.StatusStr = resMng.GetString("CoopStatus2");
                                break;
                            case "3":
                                info.StatusStr = resMng.GetString("CoopStatus3");
                                break;
                            case "4":
                                info.StatusStr = resMng.GetString("CoopStatus4");
                                break;
                            case "5":
                                info.StatusStr = resMng.GetString("CoopStatus5");
                                break;
                        }
                    }


                    info.RequestDate = DateHelpers.GetCultureDateChar8(dr["RequestDate"].ToString(), PreLangReal);
                    info.StartDate = DateHelpers.GetCultureDateChar8(dr["StartDate"].ToString(), PreLangReal);
                    info.EndDate = DateHelpers.GetCultureDateChar8(dr["EndDate"].ToString(), PreLangReal);                    
                    info.CoopNumber = dr["CoopNumber"].ToString();
                    info.Title = dr["Title"].ToString();
                    info.ReplyCnt = dr["ReplyCnt"].ToString().ToNumber<int>();
                    info.ExcessDay = dr["ExcessDay"].ToString();
                    
                    if (bPage == "request")
                    {
                        #region 수신자 이름 다국어 설정
                        string[] tmpName = dr["TONameBase"].ToString().Split('|');
                        info.NameBase = this.GetLangName(tmpName[0], tmpName[1], tmpName[2], tmpName[3], tmpName[4], tmpName[5], tmpName[6], PreLang);
                        #endregion

                        info.DeptBase = "";
                        info.TitleBase = "";

                        info.TOCnt = dr["TOCnt"].ToString().ToNumber<int>();

                        #region 날짜 비교(New Img)
                        DateTime lastDate = Convert.ToDateTime(dr["LastDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(lastDate);                        
                        info.ShowNew = (gap.Days <= 0) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                        #endregion
                    }
                    else
                    {
                        #region 이름 다국어 설정
                        info.NameBase = this.GetLangName(
                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                dr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        /*
                        #region 부서 다국어 설정
                        info.DeptBase = this.GetLangName(
                                dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                dr["DeptZhtw"].ToString().ToNull(), PreLang);
                        #endregion
                        */

                        info.DeptBase = "";
                        #region 직급 다국어 설정
                        info.TitleBase = this.GetLangName(
                                dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                dr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion
                         

                        info.TOCnt = 0;
                        info.ShowNew = "";
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList('" + bPage + "','" + UserInfo.UserID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 협업 기본 정보 가져오기
        /// 작성일자 : 2015-03-31
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetHearder(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(listID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("UserName", typeof(string));
                DataColumn dc1 = new DataColumn("DeptName", typeof(string));
                DataColumn dc2 = new DataColumn("TitleName", typeof(string));
                DataColumn dc3 = new DataColumn("Item", typeof(string));
                DataColumn dc4 = new DataColumn("ItemSub", typeof(string));

                dc0.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc0);

                dc1.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc1);

                dc2.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc2);

                dc3.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["ItemNameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc3);

                dc4.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["ItemSubNameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc4);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetHearder('" + listID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 수신자 정보 가져오기
        /// 작성일자 : 2015-04-01
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetTo(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(listID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["UserName"] = this.GetLangName(
                              ds.Tables[0].Rows[i]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameKo"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameJa"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameZhcn"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["TitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["DeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }
                            
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetTo('" + listID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 참조자 정보 가져오기
        /// 작성일자 : 2015-04-01
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCc(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(listID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCc", null);

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["UserName"] = this.GetLangName(
                              ds.Tables[0].Rows[i]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameKo"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameJa"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameZhcn"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameZhtw"].ToString().ToNull(), PreLang);

                            /*
                            ds.Tables[0].Rows[i]["TitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["DeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptZhtw"].ToString().ToNull(), PreLang);
                            */
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCc('" + listID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내용 앞뒤 가져오기
        /// 작성일자 : 2015-04-01
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBeforeNext(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string listID = GetDecryptParam("ListID", "");
            string gubun = GetDecryptParam("Gubun", "0");
            string searchValue = GetDecryptParam("Sv", "");

            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "request": //요청한 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_Coop_Request");
                        break;
                    case "receive": //요청 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_Coop_Receive");
                        break;
                    case "reference": //참조 받은 협업
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_Coop_Reference");
                        break;
                }

                SqlParameter[] parm = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ListID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = listID;
                parm[2].Value = UserID;
                parm[3].Value = gubun;
                parm[4].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBeforeNext", parm);
                return ds.Tables["GetBeforeNext"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Coop-GetBeforeNext: (" + listID + ")" + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내용 앞뒤 가져오기 (전달받은 협업)
        /// 작성일자 : 2015-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBeforeNextForward(string p)
        {
            SetDecryptString(p);            
            string forwardListID = GetDecryptParam("ForwardListID", "");
            string gubun = GetDecryptParam("Gubun", "0");
            string searchValue = GetDecryptParam("Sv", "");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_PROC_Coop_Forward");

                SqlParameter[] parm = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ForwardListID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = forwardListID;
                parm[2].Value = UserID;
                parm[3].Value = gubun;
                parm[4].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBeforeNextForward", parm);
                return ds.Tables["GetBeforeNextForward"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Coop-GetBeforeNextForward: (" + forwardListID + ")" + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전달 협업 기본 정보 가져오기
        /// 작성일자 : 2015-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetForwardHearder(string p)
        {
            SetDecryptString(p);
            string forwardListID = GetDecryptParam("ForwardListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(forwardListID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_Forward");
                sql = string.Format(sql, CompanyID, UserID, forwardListID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("UserName", typeof(string));
                DataColumn dc1 = new DataColumn("DeptName", typeof(string));
                DataColumn dc2 = new DataColumn("TitleName", typeof(string));
                DataColumn dc3 = new DataColumn("Item", typeof(string));
                DataColumn dc4 = new DataColumn("ItemSub", typeof(string));

                dc0.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc0);

                dc1.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc1);

                dc2.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc2);

                dc3.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["ItemNameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc3);

                dc4.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["ItemSubNameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["ItemSubNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["ItemSubNameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                dt.Columns.Add(dc4);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetForwardHearder('" + forwardListID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 수신자 정보 가져오기
        /// 작성일자 : 2015-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetForwardTo(string p)
        {
            SetDecryptString(p);
            string forwardListID = GetDecryptParam("ForwardListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(forwardListID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_ForwardTO");
                sql = string.Format(sql, CompanyID.ToString(), forwardListID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["UserName"] = this.GetLangName(
                              ds.Tables[0].Rows[i]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameKo"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameJa"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameZhcn"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["TitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["DeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetForwardTo('" + forwardListID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 참조자 정보 가져오기
        /// 작성일자 : 2015-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetForwardCc(string p)
        {
            SetDecryptString(p);
            string forwardListID = GetDecryptParam("ForwardListID", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(forwardListID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_ForwardCC");
                sql = string.Format(sql, CompanyID.ToString(), forwardListID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCc", null);

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["UserName"] = this.GetLangName(
                              ds.Tables[0].Rows[i]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameKo"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameJa"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["NameZhcn"].ToString().ToNull(),
                              ds.Tables[0].Rows[i]["NameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["TitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["TitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["TitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["DeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["DeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["DeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCc('" + forwardListID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 달력 가지고 오기
        /// 작성일자 : 2015-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBasicCalendar(string p)
        {
            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
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
                    info.Func = "setList('" + tmpDate + "')";
                    info.DoCount = "";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                /*
                //공휴일 표시
                string[] arSolar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Solar").Split(','); // 양력
                string[] arLunar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Lunar").Split(','); // 음력  

                string targetDay = String.Empty; //공휴일
                string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
                string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지
                List<string> Holidayobj = new List<string>();
                //양력 표시
                foreach (string _s in arSolar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

                //음력 표시
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));


                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }
                */

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
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBasicCalendar()", ex);
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
    }
}
