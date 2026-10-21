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

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws
{
    /// <summary>
    /// GetApproval의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetApproval : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 카운트 가져오기
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCount(string p)
        {
            SetDecryptString(p);
            string companyID = GetDecryptParam("CompanyID", "0");
            string userID = GetDecryptParam("UserID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CNT");
            SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
            parm[0].Value = CompanyID;
            parm[1].Value = UserID;


            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsList", parm);

            DataRow[] dr = ds.Tables["dsList"].Select("CAT = 'AL'");
            string tmpCount = "";
            if (dr.Length > 0)
            {
                tmpCount = dr.Length.ToString("#,###") + "^";
            }
            else
            {
                tmpCount = "0^";
            }

            dr = ds.Tables["dsList"].Select("CAT = 'FL'");
            if (dr.Length > 0)
            {
                tmpCount += dr.Length.ToString("#,###") + "^";
            }
            else
            {
                tmpCount += "0^";
            }

            dr = ds.Tables["dsList"].Select("CAT = 'RL'");
            if (dr.Length > 0)
            {
                tmpCount += dr.Length.ToString("#,###") + "^";
            }
            else
            {
                tmpCount += "0^";
            }

            dr = ds.Tables["dsList"].Select("CAT = 'PL'");
            if (dr.Length > 0)
            {
                tmpCount += dr.Length.ToString("#,###") + "^";
            }
            else
            {
                tmpCount += "0^";
            }

            return tmpCount;

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 리스트 가져오기 (진행중인 문서)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string searchValue = GetDecryptParam("Sv", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.GetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();


            try
            {
                string proc = "";
                switch (bPage.ToUpper())
                {
                    case "APP": //미결리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectApprove");
                        break;
                    case "REP": //상신리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectReport");
                        break;
                    case "REL": //관계있는리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectRelated");
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
                parm[1].Value = targetUID;
                parm[2].Value = pageNo;
                parm[3].Value = pageSize;
                parm[4].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.RowNum = Convert.ToInt32(dr["RowNum"].ToString());
                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());
                    info.DeptID = Convert.ToInt32(dr["DeptID"].ToString());

                    #region 이름 다국어 설정
                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.Subject = dr["Subject"].ToString();
                    info.WriteTime = Convert.ToDateTime(dr["WriteTime"].ToString()).ToString();
                    info.ApprovalState = dr["ApprovalState"].ToString();
                    info.ApprovalTag = dr["ApprovalTag"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.Number2 = dr["Number2"].ToString();
                    info.Number3 = dr["Number3"].ToString();
                    info.Sequence = Convert.ToInt32(dr["Sequence"].ToString());

                    if (info.ApprovalState == "D")
                    {
                        info.StateStr = resMng.GetString("State2");
                    }
                    else if (info.ApprovalState == "H")
                    {
                        info.StateStr = resMng.GetString("State3");
                    }
                    else if (info.ApprovalState == "I")
                    {
                        info.StateStr = resMng.GetString("State4");
                    }
                    else if (info.ApprovalState == "R")
                    {
                        info.StateStr = resMng.GetString("State5");
                    }
                    else if (info.ApprovalState == "W")
                    {
                        info.StateStr = resMng.GetString("State6");
                    }
                    else if (info.ApprovalState == "T")
                    {
                        info.StateStr = resMng.GetString("State7");
                    }
                    else if (info.ApprovalState == "P")
                    {
                        info.StateStr = resMng.GetString("State8");
                    }
                    else
                    {
                        info.StateStr = resMng.GetString("State1");
                    }

                    info.IsMobile = dr["IsMobile"].ToString();
                    info.IsGuard = dr["IsGuard"].ToString();

                    #region 날짜 비교(New Img)
                    DateTime writeDate = Convert.ToDateTime(dr["WriteTime"].ToString());
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
                Helper.AppException.WriteException("GetList('" + bPage + "','" + targetUID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재선정보 가져오기 (진행중인 문서)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSequence(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectSeqList");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

                ds.Tables[0].Columns.Add("HGunbun");
                ds.Tables[0].Columns.Add("ConvertTime");

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                ds.Tables[0].Columns.Add("PUserName");
                ds.Tables[0].Columns.Add("PTitleName");
                ds.Tables[0].Columns.Add("PDeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["HGunbun"] = ds.Tables[0].Rows[i]["ApprovalType"].ToString();
                            ds.Tables[0].Rows[i]["ConvertTime"] = ds.Tables[0].Rows[i]["ApprovalDate"].ToString();

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

                            //대결자 정보
                            ds.Tables[0].Rows[i]["PUserName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyNameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PTitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyTitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PDeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyDeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSequence('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서정보 가져오기 (진행중인 문서)
        /// 작성일자 : 2012-09-05
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContent(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string bPage = GetDecryptParam("BPage", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = "";
                if (bPage == "REP")
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcess");
                    sql = string.Format(sql, CompanyID.ToString(), dID);
                }
                else
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                    sql = string.Format(sql, CompanyID.ToString(), dID);
                }

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("ConvertWriteTime", typeof(string));
                DataColumn dc1 = new DataColumn("UserName", typeof(string));

                dc0.DefaultValue = ds.Tables[0].Rows[0]["WriteTime"].ToString();
                dt.Columns.Add(dc0);

                dc1.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);
                dt.Columns.Add(dc1);

                DataColumn dc2 = new DataColumn("bodyNoStyle", typeof(string));

                string approvalContent = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                }

                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                hdoc.LoadHtml(approvalContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    //dc2.DefaultValue = bodyNode.InnerHtml.Replace("ol, li, ul {margin-top: 3pt; margin-bottom: 3pt; }", "").Replace("body { word-break: break-all; font-size: 10pt;}", "");
                    string formContent = bodyNode.InnerHtml;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc2.DefaultValue = formContent;
                }
                else
                {
                    string formContent = approvalContent;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc2.DefaultValue = formContent;
                }
                dt.Columns.Add(dc2);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetContent('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서정보 가져오기 (완결문서)
        /// 작성일자 : 2012-08-31
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentDoc(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");


            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocCompleteInfo");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("ConvertWriteTime", typeof(string));
                DataColumn dc1 = new DataColumn("UserName", typeof(string));
                DataColumn dc2 = new DataColumn("ConvertCompleteDate", typeof(string));

                dc0.DefaultValue = ds.Tables[0].Rows[0]["WriteTime"].ToString();
                dt.Columns.Add(dc0);

                dc1.DefaultValue = this.GetLangName(
                                ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);
                dt.Columns.Add(dc1);

                dc2.DefaultValue = ds.Tables[0].Rows[0]["CompleteDate"].ToString();
                dt.Columns.Add(dc2);

                DataColumn dc3 = new DataColumn("bodyNoStyle", typeof(string));

                string approvalContent = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                }

                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                hdoc.LoadHtml(approvalContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    string formContent = bodyNode.InnerHtml;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc3.DefaultValue = formContent;
                }
                else
                {
                    string formContent = approvalContent;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc3.DefaultValue = formContent;
                }
                dt.Columns.Add(dc3);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetContentDoc('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내용 가져오기 (진행중인 문서)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentHTML(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));

                string approvalContent = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                }

                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                /*
                approvalContent = Regex.Replace(approvalContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<form", "<xform", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                //메일 본문안의 Style 제거
                approvalContent = Regex.Replace(approvalContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                */

                hdoc.LoadHtml(approvalContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    dc0.DefaultValue = bodyNode.InnerHtml;
                }
                else
                {
                    dc0.DefaultValue = approvalContent;
                }
                dt.Columns.Add(dc0);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetContentHTML('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내용 가져오기 (완결문서)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentHTMLDoc(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocTotal");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsCont", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));

                string approvalContent = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                }

                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                /*
                approvalContent = Regex.Replace(approvalContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<form", "<xform", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                //메일 본문안의 Style 제거
                approvalContent = Regex.Replace(approvalContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                 */

                hdoc.LoadHtml(approvalContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    string formContent = bodyNode.InnerHtml;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc0.DefaultValue = formContent;
                }
                else
                {
                    string formContent = approvalContent;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc0.DefaultValue = formContent;
                }
                dt.Columns.Add(dc0);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBaseContentHTML('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일정보 가져오기
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFile(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectAttList");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAtt", parm);
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAttachFile('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 문서 스트리밍 정보 가져오기
        /// 작성일자 : 2018-04-05
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDSFile(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string attachType = GetDecryptParam("TYPE", "");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "dsDS";
            ds.Tables.Add(dt);

            if (CompanyID <= 0 || UserID <= 0) return StringHelper.GetJSONString(ds.Tables[0]);

            if (String.IsNullOrEmpty(dID)) return StringHelper.GetJSONString(ds.Tables[0]);

            var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "DocStreamYN");

            try
            {
                if (docStreamYN == "Y")
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectDSAtList");
                    SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)

                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = dID;
                    parm[2].Value = attachType;
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsDS", parm);
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAttachFile('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 근거문서정보 가져오기
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBaseDoc(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectBaseList");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsBase", parm);
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBaseDoc('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 내용 앞뒤 가져오기 (진행중인 문서)
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBeforeNext(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "0");
            string gubun = GetDecryptParam("Gubun", "0");
            string targetUID = GetDecryptParam("TargetUID", "0");
            string searchValue = GetDecryptParam("Sv", "");

            try
            {
                string proc = "";
                switch (bPage.ToUpper())
                {
                    case "APP": //미결리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DOCUMENTS_APPROVE");
                        break;
                    case "REP": //상신리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DOCUMENTS_REPORT");
                        break;
                    case "REL": //관계있는리스트
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DOCUMENTS_RELATED");
                        break;
                }

                SqlParameter[] parm = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 20)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = targetUID;
                parm[3].Value = aTag;
                parm[4].Value = gubun;
                parm[5].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBeforeNext", parm);
                return ds.Tables["GetBeforeNext"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Approval-GetBeforeNext: (" + dID + ")" + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 반려대상자 정보 가져오기
        /// 작성일자 : 2012-09-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReject(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_REJECT");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@GroupID", SqlDbType.Int)
                                            ,new SqlParameter("@IsType", SqlDbType.Char,1)
                                            
                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = groupID;
                parm[3].Value = "1";

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Reject", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.SequenceInfo info = new BE.SequenceInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());
                    info.Sequence = Convert.ToInt32(dr["Sequence"].ToString());
                    info.ApprovalTag = dr["ApprovalTag"].ToString();
                    info.UserID = Convert.ToInt32(dr["UserID"].ToString());

                    #region 이름 다국어 설정
                    info.UserName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직책 다국어 설정
                    info.DutyName = this.GetLangName(
                            dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                            dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                            dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                            dr["DutyZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    DateTime dt;

                    if (DateTime.TryParse(dr["ApprovalDate"].ToString(), out dt))
                        info.ApprovalDateStr = DateHelpers.GetCultureDate(dt, UserInfo.PreferredLanguage).Substring(0, 10);
                    else
                        info.ApprovalDateStr = string.Empty;

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetReject('" + dID + "','" + groupID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대결자 리스트 가져오기
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetProxy(string p)
        {
            SetDecryptString(p);
            string companyID = GetDecryptParam("CompanyID", "0");
            string userID = GetDecryptParam("UserID", "0");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PROXY_USERID");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = companyID;
                parm[1].Value = userID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsProxy", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ProxyInfo info = new BE.ProxyInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.UserID = Convert.ToInt32(dr["UserID"].ToString());
                    info.DeptID = Convert.ToInt32(dr["DeptID"].ToString());

                    #region 이름 다국어 설정
                    info.UserName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetProxy('" + userID + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 결재 순서 확인
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMyturn(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string aTag = GetDecryptParam("ATag", "0");

            string rtnVal = "";
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SEQUENCES_MYTURN");
                sql = string.Format(sql, CompanyID.ToString(), dID, aTag);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsMyturn", null);

                if (ds != null)
                {
                    rtnVal = ds.Tables["dsMyturn"].Rows[0][0].ToString();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetMyturn('" + dID + "','" + aTag + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 패스워드 정보 가져오기
        /// 작성일자 : 2012-04-02
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetPwd(string p)
        {
            SetDecryptString(p);
            string pwd = GetDecryptParam("Pwd", "0");
            pwd = Server.UrlDecode(pwd);
            return HashManager.GetHash(pwd);

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재선정보 가져오기 (완결문서)
        /// 작성일자 : 2015-10-21
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSeqBackUp(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SeqBackUpList");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

                ds.Tables[0].Columns.Add("HGunbun");
                ds.Tables[0].Columns.Add("ConvertTime");

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                ds.Tables[0].Columns.Add("PUserName");
                ds.Tables[0].Columns.Add("PTitleName");
                ds.Tables[0].Columns.Add("PDeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["HGunbun"] = ds.Tables[0].Rows[i]["ApprovalType"].ToString();
                            ds.Tables[0].Rows[i]["ConvertTime"] = ds.Tables[0].Rows[i]["ApprovalDate"].ToString();

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

                            //대결자 정보
                            ds.Tables[0].Rows[i]["PUserName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyNameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PTitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyTitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PDeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyDeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSequence('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 문서함 리스트 가져오기 (개인별,완결결재,반려,전결,전달문서함)
        /// 작성일자 : 2015-10-21
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDocList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.GetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();


            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "Private": //개인별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocPrivate");
                        break;
                    case "Approval": //완결결재 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocApproval");
                        break;
                    case "Reject": //반려 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocReject");
                        break;
                    case "Arbitrary": //전결 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocArbitrary");
                        break;
                    case "Forward": //전달 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocForward");
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

                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());

                    #region 이름 다국어 설정
                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.Subject = dr["Subject"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                    info.Key2ID = dr["Key2ID"].ToString().ToNumber<int>();

                    string WriteTime = "";
                    string CompleteDate = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    if (DateTime.TryParse(dr["CompleteDate"].ToString(), out dt))
                        CompleteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        CompleteDate = string.Empty;

                    info.WriteTime = WriteTime;
                    info.TotalDate = CompleteDate;
                    info.IsGuard = dr["IsGuard"].ToString();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList('" + bPage + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 부서별 문서함 리스트 가져오기 
        /// 작성일자 : 2015-10-23
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPublicList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");
            string deptID = GetDecryptParam("TargetDeptID", "0");
            string titleID = GetDecryptParam("TargetTitleID", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.GetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();


            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocPublic");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                                            ,new SqlParameter("@TitleID", SqlDbType.Int)
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = deptID;
                parm[3].Value = titleID;
                parm[4].Value = pageNo;
                parm[5].Value = pageSize;
                parm[6].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());

                    #region 이름 다국어 설정
                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.Subject = dr["Subject"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                    info.Key2ID = dr["Key2ID"].ToString().ToNumber<int>();

                    string WriteTime = "";
                    string CompleteDate = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    if (DateTime.TryParse(dr["CompleteDate"].ToString(), out dt))
                        CompleteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        CompleteDate = string.Empty;

                    info.WriteTime = WriteTime;
                    info.TotalDate = CompleteDate;
                    info.IsGuard = dr["IsGuard"].ToString();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetPublicList('" + bPage + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 참조 문서함 리스트 가져오기
        /// 작성일자 : 2015-10-22
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCarboncopyList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");
            string deptID = GetDecryptParam("TargetDeptID", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.GetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocCarboncopy");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = deptID;
                parm[3].Value = pageNo;
                parm[4].Value = pageSize;
                parm[5].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());

                    #region 이름 다국어 설정
                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.Subject = dr["Subject"].ToString();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                    info.Key2ID = dr["Key2ID"].ToString().ToNumber<int>();

                    string WriteTime = "";
                    string CompleteDate = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    if (DateTime.TryParse(dr["CompleteDate"].ToString(), out dt))
                        CompleteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        CompleteDate = string.Empty;

                    info.WriteTime = WriteTime;
                    info.TotalDate = CompleteDate;
                    info.IsGuard = dr["IsGuard"].ToString();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCarboncopyList('" + bPage + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 회람 문서함 리스트 가져오기 (받은 회람함, 보낸 회람함)
        /// 작성일자 : 2015-10-21
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCirDocList(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.GetApproval).Assembly;	//어셈블리 선언            
            this.GetResourceManager();


            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "CirRecv": //받은 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CirRecv");
                        break;
                    case "CirSend": //보낸 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CirSend");
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

                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());

                    #region 이름 다국어 설정
                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.Subject = dr["Subject"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.DocNumber = dr["DocNumber"].ToString();
                    info.CirculationDocumentID = dr["CirculationDocumentID"].ToString().ToNumber<int>();
                    info.CirculationUserID = dr["CirculationUserID"].ToString().ToNumber<int>();

                    string WriteTime = "";
                    string CompleteDate = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    if (DateTime.TryParse(dr["CompleteDate"].ToString(), out dt))
                        CompleteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        CompleteDate = string.Empty;

                    info.WriteTime = WriteTime;
                    info.TotalDate = CompleteDate;
                    info.IsGuard = dr["IsGuard"].ToString();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCirDocList('" + bPage + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 내용 앞뒤 가져오기 (부서별, 개인별, 완결결재, 반려, 전결, 참조, 전달)
        /// 작성일자 : 2015-10-22
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBeforeNextDoc(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string dID = GetDecryptParam("DID", "0");
            string gubun = GetDecryptParam("Gubun", "0");
            string searchValue = GetDecryptParam("Sv", "");
            string deptID = GetDecryptParam("TargetDeptID", "0");
            string titleID = GetDecryptParam("TargetTitleID", "0");
            string keyID = GetDecryptParam("KeyID", "0");
            string key2ID = GetDecryptParam("Key2ID", "0");

            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "Public": //부서별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DOC_Public");
                        break;
                    case "Private": //개인별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Private");
                        break;
                    case "Approval": //완결결재 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Approval");
                        break;
                    case "Reject": //반려 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Reject");
                        break;
                    case "Arbitrary": //전결 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Arbitrary");
                        break;
                    case "Carboncopy": //참조 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Carboncopy");
                        break;
                    case "Forward": //전달 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Doc_Forward");
                        break;
                }

                SqlParameter[] parm = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)
                                            ,new SqlParameter("@TitleID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                                            ,new SqlParameter("@KeyID",  SqlDbType.Int)
                                            ,new SqlParameter("@Key2ID",  SqlDbType.Int)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = UserID;
                parm[3].Value = deptID;
                parm[4].Value = titleID;
                parm[5].Value = gubun;
                parm[6].Value = searchValue;
                parm[7].Value = keyID;
                parm[8].Value = key2ID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBeforeNext", parm);
                return ds.Tables["GetBeforeNext"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Approval-GetBeforeNextDoc: (" + dID + ")" + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내용 가져오기 (회람문서)
        /// 작성일자 : 2015-10-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentCirDoc(string p)
        {
            SetDecryptString(p);
            string cirDocID = GetDecryptParam("CirDocID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(cirDocID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_CirDoc");
                sql = string.Format(sql, CompanyID.ToString(), cirDocID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

                DataTable dt = ds.Tables[0];
                DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
                DataColumn dc1 = new DataColumn("UserName", typeof(string));
                DataColumn dc2 = new DataColumn("ConvertWriteTime", typeof(string));
                DataColumn dc3 = new DataColumn("ConvertCompleteDate", typeof(string));

                string approvalContent = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                }

                HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                /*
                approvalContent = Regex.Replace(approvalContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<form", "<xform", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                //메일 본문안의 Style 제거
                approvalContent = Regex.Replace(approvalContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                approvalContent = Regex.Replace(approvalContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                 */

                hdoc.LoadHtml(approvalContent);
                var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                if (bodyNode != null)
                {
                    string formContent = bodyNode.InnerHtml;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc0.DefaultValue = formContent;
                }
                else
                {
                    string formContent = approvalContent;
                    formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    dc0.DefaultValue = formContent;
                }

                dt.Columns.Add(dc0);

                dc1.DefaultValue = this.GetLangName(
                               ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                               ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                               ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                               ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);
                dt.Columns.Add(dc1);

                dc2.DefaultValue = ds.Tables[0].Rows[0]["WriteTime"].ToString();
                dt.Columns.Add(dc2);

                dc3.DefaultValue = ds.Tables[0].Rows[0]["CompleteDate"].ToString();
                dt.Columns.Add(dc3);

                return StringHelper.GetJSONString(dt);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetContentHTMLCirDoc('" + cirDocID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일정보 가져오기 (회람문서)
        /// 작성일자 : 2015-10-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileCir(string p)
        {
            SetDecryptString(p);
            string cirDocID = GetDecryptParam("CirDocID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(cirDocID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_AttCirList");
                sql = string.Format(sql, CompanyID.ToString(), cirDocID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAtt", null);

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAttachFileCir('" + cirDocID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 근거문서정보 가져오기
        /// 작성일자 : 2012-09-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBaseDocCir(string p)
        {
            SetDecryptString(p);
            string cirDocID = GetDecryptParam("CirDocID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(cirDocID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_BaseCirList");
                sql = string.Format(sql, CompanyID.ToString(), cirDocID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsBase", null);

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBaseDocCir('" + cirDocID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재 내용 앞뒤 가져오기 (회람문서함)
        /// 작성일자 : 2015-10-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBeforeNextCirDoc(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string gubun = GetDecryptParam("Gubun", "0");
            string searchValue = GetDecryptParam("Sv", "");
            string cirDocID = GetDecryptParam("CirDocID", "0");
            string cirUserID = GetDecryptParam("CirUserID", "0");

            try
            {
                string proc = "";
                switch (bPage)
                {
                    case "CirRecv": //받은 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CirDoc_Recv");
                        break;
                    case "CirSend": //보낸 회람함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CirDoc_Send");
                        break;
                }

                SqlParameter[] parm = new SqlParameter[] {
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                                            ,new SqlParameter("@CirDocID",  SqlDbType.Int)
                                            ,new SqlParameter("@CirUserID",  SqlDbType.Int)
                };

                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = gubun;
                parm[3].Value = searchValue;
                parm[4].Value = cirDocID;
                parm[5].Value = cirUserID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBeforeNext", parm);
                return ds.Tables["GetBeforeNext"].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Approval-GetBeforeNextDoc: (" + cirDocID + "," + cirUserID + ")" + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서 보고 있는 지 체크
        /// 작성일자 : 2017-01-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetViewing(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            try
            {
                if (dID.Equals("") || dID.Equals("0")) return "!NULL";

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                };
                parm[0].Value = UserInfo.CompanyID;
                parm[1].Value = dID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ViewingEtc");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsInfo", parm);

                string rtnVal = "OK";

                if (ds != null)
                {
                    if ((ds.Tables[0].Rows[0]["DocViewing"].ToString() == "Y") && !ds.Tables[0].Rows[0]["UserID"].ToString().Equals(UserInfo.UserID))
                    {
                        if (Convert.ToDateTime(ds.Tables[0].Rows[0]["ViewingTime"].ToString()).AddMinutes(30) > DateTime.Now)
                        {
                            string DeptName = this.GetLangName(
                                    ds.Tables[0].Rows[0]["DeptBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptKo"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["DeptEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptJa"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["DeptFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["DeptZhcn"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["DeptZhtw"].ToString().ToNull(), PreLang);

                            string TitleName = this.GetLangName(
                                    ds.Tables[0].Rows[0]["TitleBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleKo"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["TitleEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleJa"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["TitleFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["TitleZhcn"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["TitleZhtw"].ToString().ToNull(), PreLang);

                            string Name = this.GetLangName(
                                    ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                    ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);

                            rtnVal = DeptName + "/" + TitleName + "/" + Name;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetViewing('" + dID + "')", ex);
                return "ERROR";
            }
        }

        /// <summary>
        /// 작성자   : famo23 
        /// 작성목적 : Badge 정보 가져오기
        /// 작성일자 : 2019-03-06
        /// </summary>
        /// <returns></returns> 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBadgeNum(string p)
        {
            try
            {
                string rtnVal = String.Empty;

                #region 결재 건수 (결재대상문서,대결결재대상문서 건수)
                int appCount = 0;
                string sqlAppQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_MENUCNT");
                SqlParameter[] parm = new SqlParameter[]{
                                          new SqlParameter("@CompanyID", SqlDbType.Int)
                                         ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;

                DataSet dsApp = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlAppQuery, "dsList", parm);

                DataRow[] dr = dsApp.Tables["dsList"].Select("CAT = 'AL'");
                appCount = dr.Length;

                dr = dsApp.Tables["dsList"].Select("CAT = 'FL'");
                appCount += dr.Length;
                #endregion

                return appCount.ToString();
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetBadgeNum()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 자주 사용하는 양식 리스트 가져오기 
        /// 작성일자 : 2019-07-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavFormList(string p)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_FavoriteForm");
                sql = string.Format(sql, CompanyID.ToString(), UserID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsFavForm", null);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.FormInfo info = new BE.FormInfo();

                    info.FormID = Convert.ToInt32(dr["FormID"].ToString());
                    info.FName = this.GetLangName(
                            dr["FormBase"].ToString().ToNull(), dr["FormKo"].ToString().ToNull(),
                            dr["FormEn"].ToString().ToNull(), dr["FormJa"].ToString().ToNull(),
                            dr["FormFr"].ToString().ToNull(), dr["FormZhcn"].ToString().ToNull(),
                            dr["FormZhtw"].ToString().ToNull(), PreLang);

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFavFormList('" + UserID + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 양식폴더 , 양식 리스트 가져오기 
        /// 작성일자 : 2019-07-10
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFormList(string p)
        {
            SetDecryptString(p);
            string folderID = GetDecryptParam("FolderID", "0");
            string typeInfo = GetDecryptParam("TypeInfo", "1");

            try
            {
                List<object> listobj = new List<object>();
                string FolderAuth = "";
                bool isauth = true;

                // 양식 폴더 정보 가져오기 
                string sql = "";
                if (folderID == "0")
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_FolderParent");
                    sql = string.Format(sql, CompanyID.ToString());
                }
                else
                {
                    sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_Folder");
                    sql = string.Format(sql, CompanyID.ToString(), folderID.ToString());

                    //자사폴더일때는 권한을 체크한다 
                    if (typeInfo == "2")
                    {
                        string sqlAuth = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_FolderAuth");
                        sqlAuth = string.Format(sqlAuth, CompanyID.ToString(), UserInfo.DeptID);
                        DataSet dsForm = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlAuth, "dsAuth", null);

                        FolderAuth = ",";
                        foreach (DataRow dr in dsForm.Tables[0].Rows)
                        {
                            FolderAuth += dr["FolderID"].ToString() + ",";
                        }
                    }
                }

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsFolder", null);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.FormInfo info = new BE.FormInfo();

                    info.FolderID = Convert.ToInt32(dr["FolderID"].ToString());
                    info.FormID = 0;
                    info.TypeInfo = dr["TypeInfo"].ToString();
                    info.FName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);

                    listobj.Add(info);
                }

                // 폴더안의 양식 가져오기 
                if (folderID != "0")
                {
                    //자사폴더체크
                    if (typeInfo == "2")
                    {
                        isauth = (FolderAuth.IndexOf("," + folderID + ",") < 0) ? false : true;
                    }

                    if (isauth)
                    {
                        string sqlForm = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_Form");

                        sqlForm = string.Format(sqlForm, CompanyID.ToString(), folderID.ToString());
                        DataSet dsForm = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlForm, "dsForm", null);

                        foreach (DataRow dr in dsForm.Tables[0].Rows)
                        {
                            BE.FormInfo info = new BE.FormInfo();

                            info.FolderID = Convert.ToInt32(dr["FolderID"].ToString());
                            info.FormID = Convert.ToInt32(dr["FormID"].ToString());
                            info.TypeInfo = "0";
                            info.FName = this.GetLangName(
                                        dr["FormBase"].ToString().ToNull(), dr["FormKo"].ToString().ToNull(),
                                        dr["FormEn"].ToString().ToNull(), dr["FormJa"].ToString().ToNull(),
                                        dr["FormFr"].ToString().ToNull(), dr["FormZhcn"].ToString().ToNull(),
                                        dr["FormZhtw"].ToString().ToNull(), PreLang);

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFormList('" + UserID + "','" + folderID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 모바일 상신문서함 리스트 가져오기
        /// 작성일자 : 2019-07-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSubmitList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocSubmit");

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

                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());
                    info.Subject = dr["Subject"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.DocNumber = dr["DocNumber"].ToString();

                    string WriteTime = "";
                    string CompleteDate = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    if (DateTime.TryParse(dr["CompleteDate"].ToString(), out dt))
                        CompleteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        CompleteDate = string.Empty;

                    info.WriteTime = WriteTime;
                    info.TotalDate = CompleteDate;

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSubmitList('" + UserID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 모바일 임시보관함 리스트 가져오기
        /// 작성일자 : 2019-07-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDraftList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("Sv", "");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_Draft");

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

                    info.DraftID = Convert.ToInt32(dr["DraftID"].ToString());
                    info.Subject = dr["Subject"].ToString();
                    info.FormBase = this.GetLangName(
                                        dr["FormBase"].ToString().ToNull(), dr["FormKo"].ToString().ToNull(),
                                        dr["FormEn"].ToString().ToNull(), dr["FormJa"].ToString().ToNull(),
                                        dr["FormFr"].ToString().ToNull(), dr["FormZhcn"].ToString().ToNull(),
                                        dr["FormZhtw"].ToString().ToNull(), PreLang);

                    string WriteTime = "";
                    DateTime dt;

                    if (DateTime.TryParse(dr["WriteTime"].ToString(), out dt))
                        WriteTime = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        WriteTime = string.Empty;

                    info.WriteTime = WriteTime;
                    info.WriteTimeYMD = dr["WriteTimeYMD"].ToString().ToNull();

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDraftList('" + UserID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 후열대상자 정보 가져오기
        /// 작성일자 : 2019-08-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAtferTarget(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");
            string nextTag = GetDecryptParam("NextTag", "");
            string nextSeq = GetDecryptParam("NextSeq", "0");

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@Sequence", SqlDbType.Int)
                                            ,new SqlParameter("@ApprovalTag", SqlDbType.VarChar,20)
                                            
                };
                parm[0].Value = CompanyID;
                parm[1].Value = dID;
                parm[2].Value = nextSeq;
                parm[3].Value = nextTag;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelZAUserList", "Reject", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.SequenceInfo info = new BE.SequenceInfo();

                    info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());
                    info.Sequence = Convert.ToInt32(dr["Sequence"].ToString());
                    info.ApprovalTag = dr["ApprovalTag"].ToString();

                    #region 이름 다국어 설정
                    info.UserName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    info.DeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직책 다국어 설정
                    info.DutyName = this.GetLangName(
                            dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                            dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                            dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                            dr["DutyZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAtferTarget('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재 상태 확인
        /// 작성일자 : 2019-08-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCheckTurn(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            string rtnVal = "";
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsturn", null);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rtnVal = dr["ApprovalState"].ToString();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCheckTurn('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장 근거문서정보 가져오기
        /// 작성일자 : 2019-08-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDraftBaseDoc(string p)
        {
            SetDecryptString(p);
            string draftID = GetDecryptParam("DraftID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(draftID)) { return "!NULL"; }

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DraftID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftBaseDoc_Sel", "dsBase", parm);
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDraftBaseDoc('" + draftID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장 첨부파일정보 가져오기
        /// 작성일자 : 2019-08-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDraftAttachFile(string p)
        {
            SetDecryptString(p);
            string draftID = GetDecryptParam("DraftID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(draftID)) { return "!NULL"; }

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DraftID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftFile_Sel", "dsAtt", parm);
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDraftAttachFile('" + draftID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 결재선정보 가져오기 (진행중,완결문서)
        /// 작성일자 : 2019-09-23
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSeqTotal(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_TotalSeq");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

                ds.Tables[0].Columns.Add("HGunbun");
                ds.Tables[0].Columns.Add("ConvertTime");

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");

                ds.Tables[0].Columns.Add("PUserName");
                ds.Tables[0].Columns.Add("PTitleName");
                ds.Tables[0].Columns.Add("PDeptName");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i]["HGunbun"] = ds.Tables[0].Rows[i]["ApprovalType"].ToString();
                            ds.Tables[0].Rows[i]["ConvertTime"] = ds.Tables[0].Rows[i]["ApprovalDate"].ToString();

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

                            //대결자 정보
                            ds.Tables[0].Rows[i]["PUserName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyNameBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyNameZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyNameZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PTitleName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyTitleBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyTitleZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyTitleZhtw"].ToString().ToNull(), PreLang);

                            ds.Tables[0].Rows[i]["PDeptName"] = this.GetLangName(
                                ds.Tables[0].Rows[i]["ProxyDeptBase"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptKo"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptEn"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptJa"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptFr"].ToString().ToNull(), ds.Tables[0].Rows[i]["ProxyDeptZhcn"].ToString().ToNull(),
                                ds.Tables[0].Rows[i]["ProxyDeptZhtw"].ToString().ToNull(), PreLang);
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSeqTotal('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 문서정보 가져오기 (완결문서)
        /// 작성일자 : 2019-09-23
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentTotal(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectDoc");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        DataColumn dc0 = new DataColumn("ConvertWriteTime", typeof(string));
                        DataColumn dc1 = new DataColumn("UserName", typeof(string));
                        DataColumn dc2 = new DataColumn("ConvertCompleteDate", typeof(string));

                        dc0.DefaultValue = ds.Tables[0].Rows[0]["WriteTime"].ToString();
                        dt.Columns.Add(dc0);

                        dc1.DefaultValue = this.GetLangName(
                                        ds.Tables[0].Rows[0]["NameBase"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameKo"].ToString().ToNull(),
                                        ds.Tables[0].Rows[0]["NameEn"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameJa"].ToString().ToNull(),
                                        ds.Tables[0].Rows[0]["NameFr"].ToString().ToNull(), ds.Tables[0].Rows[0]["NameZhcn"].ToString().ToNull(),
                                        ds.Tables[0].Rows[0]["NameZhtw"].ToString().ToNull(), PreLang);
                        dt.Columns.Add(dc1);

                        dc2.DefaultValue = ds.Tables[0].Rows[0]["CompleteDate"].ToString();
                        dt.Columns.Add(dc2);

                        DataColumn dc3 = new DataColumn("bodyNoStyle", typeof(string));

                        string approvalContent = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            approvalContent = ds.Tables[0].Rows[0]["ContentHTML"].ToString().Trim();
                        }

                        HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                        hdoc.LoadHtml(approvalContent);
                        var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                        if (bodyNode != null)
                        {
                            string formContent = bodyNode.InnerHtml;
                            formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                            formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                            formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                            dc3.DefaultValue = formContent;
                        }
                        else
                        {
                            string formContent = approvalContent;
                            formContent = Regex.Replace(formContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                            formContent = Regex.Replace(formContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                            formContent = Regex.Replace(formContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                            dc3.DefaultValue = formContent;
                        }
                        dt.Columns.Add(dc3);

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
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetContentTotal('" + dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 결재의견 가져오기
        /// 작성일자 : 2022-01-18
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetOpinionList(string p)
        {
            SetDecryptString(p);
            string dID = GetDecryptParam("DID", "0");

            if (CompanyID <= 0 || UserID <= 0 || dID == "0") return "!NULL";

            if (String.IsNullOrEmpty(dID)) { return "!NULL"; }

            string upPathID = string.Empty;

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectDoc");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet dsDoc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

                foreach (DataRow dr in dsDoc.Tables[0].Rows)
                {
                    upPathID = dr["UpPathID"].ToString().ToNull();
                }

                upPathID = upPathID.TrimStart(',');

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectShortReply");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UpPathID", SqlDbType.VarChar, 100)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = upPathID;
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsShort", parm);

                ds.Tables[0].Columns.Add("UserName");
                ds.Tables[0].Columns.Add("TitleName");
                ds.Tables[0].Columns.Add("DeptName");
                ds.Tables[0].Columns.Add("Opinion");
                ds.Tables[0].Columns.Add("ConvertTime");

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

                            ds.Tables[0].Rows[i]["Opinion"] = ds.Tables[0].Rows[i]["Content"].ToString().Replace("\n", "<br>").ToReverseXSS();
                            ds.Tables[0].Rows[i]["ConvertTime"] = ds.Tables[0].Rows[i]["WriteTime"].ToString();
                        }
                    }
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetOpinionList('" + dID + "')", ex);
                throw;
            }
        }
    }
}
