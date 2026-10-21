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
    /// Common의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Common : Helper.MobileBaseService
    {

        /// <summary>
        /// 서버시간 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetServerTime(string p)
        {
            List<object> uobj = new List<object>();
            DateTime dt = System.DateTime.Now;

            // getToDayByServer
            uobj.Add(dt.ToShortDateString());
            // getTimeByServer
            uobj.Add(dt.ToShortDateString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0'));
            // getMonthDayByServer
            uobj.Add(dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0'));
            // getTimeExByServer
            uobj.Add(dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0'));

            return uobj;
        }

        /// <summary>
        /// 양식별 저장된 기 결재라인 가져오기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetLine(string p)
        {
            SetDecryptString(p);
            int formID = GetDecryptParam("formID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FormID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = formID;
                parm[2].Value = UserID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FavoriteAppNet_Sel", "dsLine", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo fInfo = new BE.SequenceInfo();

                            fInfo.ApprovalTag = dr["ApprovalTag"].ToString();
                            fInfo.UserID = dr["NetUserID"].ToString().ToNumber<int>();
                            fInfo.UserName = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhcn"].ToString(), PreLang);
                            fInfo.DeptID = dr["NetDeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsDept"].ToString().ToUpper().Equals("G"))
                            {
                                fInfo.DeptName = fInfo.UserName;
                            }
                            else
                            {
                                fInfo.DeptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhcn"].ToString(), PreLang);
                            }
                            fInfo.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            fInfo.TitleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhcn"].ToString(), PreLang);
                            fInfo.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();
                            fInfo.DutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhcn"].ToString(), PreLang);
                            fInfo.IsGroup = dr["IsDept"].ToString();
                            fInfo.IsSubTree = dr["IsSub"].ToString();

                            listobj.Add(fInfo);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetNetLine('" + formID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 수신 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetZQZCLine(string p)
        {
            SetDecryptString(p);
            int formID = GetDecryptParam("formID", "0").ToNumber<int>();
            string appointerKind = GetDecryptParam("appointerKind", "");

            try
            {

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FormID", SqlDbType.Int)
                                            ,new SqlParameter("@AppointerKind", SqlDbType.Char,2)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = formID;
                parm[2].Value = appointerKind;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FormAppointer_SelByFormIDIsActive", "dsLine", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo fInfo = new BE.SequenceInfo();

                            fInfo.ApprovalTag = dr["AppointerKind"].ToString();
                            fInfo.UserID = dr["UserID"].ToString().ToNumber<int>();
                            fInfo.UserName = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhcn"].ToString(), PreLang);
                            fInfo.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            fInfo.DeptName = dr["DeptBase"].ToString();
                            fInfo.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            fInfo.TitleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhcn"].ToString(), PreLang);
                            fInfo.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();
                            fInfo.DutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhcn"].ToString(), PreLang);
                            fInfo.IsGroup = dr["IsDept"].ToString();
                            fInfo.IsSubTree = dr["IsSub"].ToString();
                            listobj.Add(fInfo);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetNetZQZCLine('" + formID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 자주지정하는결재자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavoriteApp(string p)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FavoriteApp_SelByUserID", "dsFavorite", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.FavoriteApprovalInfo info = new BE.FavoriteApprovalInfo();

                            info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.ApprovalDeptID = dr["ApprovalDeptID"].ToString().ToNumber<int>();
                            info.FavoriteApprovalID = dr["FavoriteApprovalID"].ToString().ToNumber<int>();

                            info.ApprovalUserID = dr["ApprovalUserID"].ToString().ToNumber<int>();
                            info.ApprovalUserName = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                            info.ApprovalUserDeptID = dr["ApprovalUserDeptID"].ToString().ToNumber<int>();
                            info.ApprovalUserDeptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                            info.ApprovalUserTitleID = dr["ApprovalUserTitleID"].ToString().ToNumber<int>();
                            info.ApprovalUserTitleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);

                            info.ApprovalUserDutyID = dr["ApprovalUserDutyID"].ToString().ToNumber<int>();
                            info.ApprovalUserDutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhtw"].ToString(), PreLang);

                            info.IsDept = dr["IsDept"].ToString().ToNull();        // G, M
                            info.IsSub = dr["IsSub"].ToString().ToNull();          // 1, 0
                            info.BaseDept = dr["BaseDept"].ToString().ToNull();
                            info.BaseDeptStr = (!info.IsDept.Equals("G") && info.BaseDept.Equals("0")) ? " (" + GetResource("OrganAddtionJob").Trim() + ")" : "";

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFavoriteApp()", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정양식의 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFormSeq(string p)
        {
            SetDecryptString(p);
            int formID = GetDecryptParam("formID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FormID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = formID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FormSeq_Sel", "dsFavorite", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.FormSeqInfo info = new BE.FormSeqInfo();

                            info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            info.FormSeqID = dr["FormSeqID"].ToString().ToNumber<int>();
                            info.FormID = dr["FormID"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalInfo = dr["ApprovalInfo"].ToString();
                            info.IsFix = dr["IsFix"].ToString();
                            info.IsFlow = dr["IsFlow"].ToString();
                            info.IsAdmin = dr["IsAdmin"].ToString();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.IsDeptCheck = dr["IsDeptCheck"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFormSeq('" + formID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 양식별 저장된 결재라인 중 부서 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetModfyLine(string p)
        {
            SetDecryptString(p);
            int formID = GetDecryptParam("formID", "0").ToNumber<int>();
            int appointerKind = GetDecryptParam("appointerKind", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FormID", SqlDbType.Int)
                                            ,new SqlParameter("@AppointerKind", SqlDbType.Char,2)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = formID;
                parm[2].Value = appointerKind;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FormAppointer_SelByFormIDIsActive", "dsFavorite", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["NetUserID"].ToString().ToNumber<int>();
                            info.UserName = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhcn"].ToString(), PreLang);
                            info.DeptID = dr["NetDeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsDept"].ToString().ToUpper().Equals("G"))
                            {
                                info.UserName = "";
                            }
                            info.DeptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhcn"].ToString(), PreLang);
                            info.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            info.TitleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhcn"].ToString(), PreLang);
                            info.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();
                            info.DutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhcn"].ToString(), PreLang);
                            info.IsGroup = dr["IsDept"].ToString();
                            info.IsSubTree = dr["IsSub"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetNetModfyLine('" + formID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 사용자 부서 검색
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDeptUser(string p)
        {
            SetDecryptString(p);
            string searchValue = GetDecryptParam("Sv", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Ws.Common).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar,500)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = searchValue;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_ACC_APP_SelName", "dsUser", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            StringBuilder sb = new StringBuilder();
                            BE.UserDeptInfo info = new BE.UserDeptInfo();

                            sb.Append(dr["Type"].ToString());
                            sb.Append("|");
                            sb.Append(dr["OwnerID"].ToString());

                            string name = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhTw"].ToString(), PreLang);
                            string deptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhTw"].ToString(), PreLang);
                            string titleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhTw"].ToString(), PreLang);
                            string dutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhTw"].ToString(), PreLang);

                            //겸직,사용자일 경우 -> basedept : 0 겸직, 1은 지정
                            if (dr["BaseDept"].ToString() == "0" && dr["Type"].ToString() != "G")
                            {
                                string baseDept = "(" + resMng.GetString("OrganAddtionJob") + ")";
                                info.NameText = name + baseDept + "/" + titleName + "/" + deptName;
                            }
                            else
                            {
                                if (dr["Type"].ToString() == "G")
                                {
                                    info.NameText = name;
                                }
                                else
                                {
                                    info.NameText = name + "/" + titleName + "/" + deptName;
                                }
                            }

                            sb.Append("|");
                            sb.Append(name);
                            sb.Append("|");
                            sb.Append(dr["ParentOwnerID"].ToString().Replace("G", ""));
                            sb.Append("|");
                            sb.Append(deptName);
                            sb.Append("|");
                            sb.Append(dr["TitleID"].ToString());
                            sb.Append("|");
                            sb.Append(titleName);
                            sb.Append("|");
                            sb.Append(dr["DutyID"].ToString());
                            sb.Append("|");
                            sb.Append(dutyName);

                            name = GetLangName(dr["CPNameBase"].ToString(), dr["CPNameKo"].ToString(), dr["CPNameEn"].ToString(), dr["CPNameJa"].ToString(), dr["CPNameFr"].ToString(), dr["CPNameZhcn"].ToString(), dr["CPNameZhTw"].ToString(), PreLang);
                            deptName = GetLangName(dr["CPDeptBase"].ToString(), dr["CPDeptKo"].ToString(), dr["CPDeptEn"].ToString(), dr["CPDeptJa"].ToString(), dr["CPDeptFr"].ToString(), dr["CPDeptZhcn"].ToString(), dr["CPDeptZhTw"].ToString(), PreLang);
                            titleName = GetLangName(dr["CPTitleBase"].ToString(), dr["CPTitleKo"].ToString(), dr["CPTitleEn"].ToString(), dr["CPTitleJa"].ToString(), dr["CPTitleFr"].ToString(), dr["CPTitleZhcn"].ToString(), dr["CPTitleZhTw"].ToString(), PreLang);
                            dutyName = GetLangName(dr["CPDutyBase"].ToString(), dr["CPDutyKo"].ToString(), dr["CPDutyEn"].ToString(), dr["CPDutyJa"].ToString(), dr["CPDutyFr"].ToString(), dr["CPDutyZhcn"].ToString(), dr["CPDutyZhTw"].ToString(), PreLang);

                            sb.Append("|");
                            sb.Append(dr["CPUserID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(name);
                            sb.Append("|");
                            sb.Append(dr["CPDeptID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(deptName);
                            sb.Append("|");
                            sb.Append(dr["CPTitleID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(titleName);
                            sb.Append("|");
                            sb.Append(dr["CPDutyID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(dutyName);

                            name = GetLangName(dr["AppNameBase"].ToString(), dr["AppNameKo"].ToString(), dr["AppNameEn"].ToString(), dr["AppNameJa"].ToString(), dr["AppNameFr"].ToString(), dr["AppNameZhcn"].ToString(), dr["AppNameZhTw"].ToString(), PreLang);
                            deptName = GetLangName(dr["AppDeptBase"].ToString(), dr["AppDeptKo"].ToString(), dr["AppDeptEn"].ToString(), dr["AppDeptJa"].ToString(), dr["AppDeptFr"].ToString(), dr["AppDeptZhcn"].ToString(), dr["AppDeptZhTw"].ToString(), PreLang);
                            titleName = GetLangName(dr["AppTitleBase"].ToString(), dr["AppTitleKo"].ToString(), dr["AppTitleEn"].ToString(), dr["AppTitleJa"].ToString(), dr["AppTitleFr"].ToString(), dr["AppTitleZhcn"].ToString(), dr["AppTitleZhTw"].ToString(), PreLang);
                            dutyName = GetLangName(dr["AppDutyBase"].ToString(), dr["AppDutyKo"].ToString(), dr["AppDutyEn"].ToString(), dr["AppDutyJa"].ToString(), dr["AppDutyFr"].ToString(), dr["AppDutyZhcn"].ToString(), dr["AppDutyZhTw"].ToString(), PreLang);

                            sb.Append("|");
                            sb.Append(dr["AppUserID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(name);
                            sb.Append("|");
                            sb.Append(dr["AppDeptID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(deptName);
                            sb.Append("|");
                            sb.Append(dr["AppTitleID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(titleName);
                            sb.Append("|");
                            sb.Append(dr["AppDutyID"].ToString().ToNull());
                            sb.Append("|");
                            sb.Append(dutyName);

                            info.NameValue = sb.ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDeptUser()", ex);
                throw;
            }
        }

        /// <summary>
        /// 개인결재선 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavSeq(string p)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_FavSeq");
                sql = string.Format(sql, CompanyID.ToString(), UserID.ToString());
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsFavSeq", null);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.FavSeqInfo info = new BE.FavSeqInfo();

                            info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            info.FavSeqID = dr["FavSeqID"].ToString().ToNumber<int>();
                            info.Subject = dr["Subject"].ToString();
                            info.Memo = dr["Memo"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFavSeq()", ex);
                throw;
            }
        }

        /// <summary>
        /// 특정 결재선 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFlowSeqLine(string p)
        {
            SetDecryptString(p);
            int favSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FavSeqID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = favSeqID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_FavSeqLine_SelFavSeqID", "dsFlow", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.FavSeqLineInfo info = new BE.FavSeqLineInfo();

                            info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            info.FavSeqID = dr["FavSeqID"].ToString().ToNumber<int>();
                            info.FavSeqLineID = dr["FavSeqLineID"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.IsDept = dr["IsDept"].ToString();
                            info.IsDept = (info.IsDept.Equals("1")) ? "G" : "M";
                            info.IsSub = dr["IsSub"].ToString();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            info.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            info.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFlowSeqLine('" + favSeqID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-22
        /// 작성목적 : 근거문서 카운트 가져오기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBaseCount(string p)
        {
            SetDecryptString(p);
            string FolderName = GetDecryptParam("FolderName", "0");
            string PageNo = GetDecryptParam("PageNo", "0");

            try
            {
                string proc = "";
                switch (FolderName)
                {
                    case "Dept": //부서별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PublicCnt");
                        break;
                    case "Private": //개인별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PrivateCnt");
                        break;
                    case "Approval": //완결 결재 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ApprovalCnt");
                        break;
                    case "Arbitrary": //전결 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ArbitraryCnt");
                        break;
                    case "Forward": //전달 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_ForwardCnt");
                        break;
                    case "Carboncopy": //참조 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_CarboncopyCnt");
                        break;
                    case "Report": //개인별 진행중인문서
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_PrivateProcessCnt");
                        break;
                }

                SqlParameter[] parm = null;

                if (FolderName == "Dept")
                {
                    parm = new SqlParameter[]{
                                                 new SqlParameter("@CompanyID", SqlDbType.Int)
                                                ,new SqlParameter("@UserID", SqlDbType.Int)
                                                ,new SqlParameter("@DeptID", SqlDbType.Int)
                                                ,new SqlParameter("@TitleID", SqlDbType.Int)
                                                ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                    };
                    parm[0].Value = UserInfo.CompanyID;
                    parm[1].Value = UserInfo.UserID;
                    parm[2].Value = UserInfo.DeptID;
                    parm[3].Value = UserInfo.CodeTitleID;
                    parm[4].Value = "";
                }
                else if (FolderName == "Carboncopy")
                {
                    parm = new SqlParameter[]{
                                                 new SqlParameter("@CompanyID", SqlDbType.Int)
                                                ,new SqlParameter("@UserID", SqlDbType.Int)
                                                ,new SqlParameter("@DeptID", SqlDbType.Int)                                            
                                                ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                    };
                    parm[0].Value = UserInfo.CompanyID;
                    parm[1].Value = UserInfo.UserID;
                    parm[2].Value = UserInfo.DeptID;
                    parm[3].Value = "";
                }
                else
                {
                    parm = new SqlParameter[]{
                                                 new SqlParameter("@CompanyID", SqlDbType.Int)
                                                ,new SqlParameter("@UserID", SqlDbType.Int)
                                                ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                    };
                    parm[0].Value = UserInfo.CompanyID;
                    parm[1].Value = UserInfo.UserID;
                    parm[2].Value = "";
                }

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm);

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

                return TotalPage.ToString() + "^" + PageNo.ToString();

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetBaseCount: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 근거문서 문서함 리스트 가져오기 
        /// 작성일자 : 2019-07-22
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBaseList(string p)
        {
            SetDecryptString(p);
            string FolderName = GetDecryptParam("FolderName", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");

            try
            {
                string proc = "";
                switch (FolderName)
                {
                    case "Dept": //부서별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocPublic");
                        break;
                    case "Private": //개인별 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocPrivate");
                        break;
                    case "Approval": //완결결재 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocApproval");
                        break;
                    case "Arbitrary": //전결 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocArbitrary");
                        break;
                    case "Forward": //전달 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocForward");
                        break;
                    case "Carboncopy": //참조 문서함
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocCarboncopy");
                        break;
                    case "Report": //개인별 진행중인문서
                        proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_DocProcessPrivate");
                        break;
                }


                SqlParameter[] parm = null;

                if (FolderName == "Dept")
                {
                    parm = new SqlParameter[]{
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
                    parm[2].Value = UserInfo.DeptID;
                    parm[3].Value = UserInfo.CodeTitleID;
                    parm[4].Value = pageNo;
                    parm[5].Value = pageSize;
                    parm[6].Value = "";

                }
                else if (FolderName == "Carboncopy")
                {
                    parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@DeptID", SqlDbType.Int)                                            
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = UserID;
                    parm[2].Value = UserInfo.DeptID;
                    parm[3].Value = pageNo;
                    parm[4].Value = pageSize;
                    parm[5].Value = "";
                }
                else
                {
                    parm = new SqlParameter[]{
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
                    parm[4].Value = "";
                }

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
                    info.SubjectStr = dr["Subject"].ToString().Replace("&#39;", "zeo~zeo");
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

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDocList('" + FolderName + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일 가져오기
        /// 작성일자 : 2019-07-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSpoolList(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "0");

            try
            {
                string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/");

                List<object> listobj = new List<object>();

                DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                if (DirInfo.Exists)
                {
                    var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                    string sourceFile = "";

                    foreach (var item in files)
                    {
                        if (item.Name != "Thumbs.db")
                        {
                            sourceFile = uploadPath + item.Name;
                            FileInfo sourceFileInfo = new FileInfo(sourceFile);

                            BE.AttachInfo info = new BE.AttachInfo();
                            info.CompanyID = CompanyID;
                            info.FileName = sourceFileInfo.Name;
                            info.FileType = sourceFileInfo.Extension;

                            listobj.Add(info);
                        }
                    }
                }


                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetSpoolList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Spool 폴더 파일삭제
        /// 작성일자 : 2019-07-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSpoolFileList(string p)
        {
            SetDecryptString(p);
            string fileName = GetDecryptParam("fileName", "");
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            try
            {
                // 파일 삭제
                string filePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/" + fileName;

                if (File.Exists(Server.MapPath(filePath)))
                {
                    System.IO.File.Delete(Server.MapPath(filePath));
                }

                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelSpoolFileList()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 상신취소 연동파일 확인
        /// 작성일자 : 2019-08-06
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCustomFileName(string p)
        {
            try
            {
                SetDecryptString(p);

                string customPath = "";
                string fileName = GetDecryptParam("fileName", "");
                if (fileName.Split('.').Length > 1)
                {
                    string[] fileArr = fileName.Split('.');
                    fileName = fileArr[0] + "_Cancel." + fileArr[1];

                    customPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "CustomScriptRoot");
                    if (!customPath.Equals(""))
                    {
                        customPath += fileName;       //  /DeskPlusMobile/Legacy/aaa.aspx

                        if (!File.Exists(Server.MapPath(customPath)))
                        {
                            customPath = "";
                        }
                    }
                    else
                    {
                        customPath = "";
                    }
                }
                return customPath;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCustomFileName()", ex);
                return "";
            }

        }

        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeq(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = docID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelByLine", "dsSeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["TitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["DutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSeq('" + docID + "')", ex);
                throw;
            }
        }

        /// <summary>
        ///  부서정보 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfySeq(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = docID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_Seq_SelModfyByLine", "dsModfySeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["TitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["DutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSeq('" + docID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 근거 문서 삭제
        /// 작성일자 : 2019-08-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseDoc(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();
            int baseDocID = GetDecryptParam("baseDocID", "0").ToNumber<int>();

            try
            {
                if (docID.Equals("") || docID.Equals("0")) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                                            ,new SqlParameter("@BaseDocumentID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = docID;
                parm[2].Value = baseDocID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_BaseDoc_Del", parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelBaseDoc('" + docID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일 삭제
        /// 작성일자 : 2019-08-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

            try
            {
                if (docID.Equals("") || docID.Equals("0")) return false;

                HttpContext context = HttpContext.Current;

                Data.AttachFile attDal = new Data.AttachFile();
                List<Ws.BE.AttachInfo> list = attDal.GetAttachFileLists(CompanyID, docID, attachFileID);
                foreach (BE.AttachInfo info in list)
                {
                    string tmpFilePath = info.FilePath.ToUpper().Replace("/DESKPLUSEIP/FILEDATA/", "/DeskPlusMobile/FileData/");
                    string filePath = context.Server.MapPath(tmpFilePath + info.FileGUID);

                    if (attDal.Del(CompanyID, docID, info.AttachFileID))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelAttachFile('" + docID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDraftSeq(string p)
        {
            SetDecryptString(p);
            int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DraftID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftSequence_Sel", "dsSeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDraftSeq('" + draftID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 부서정보가 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfyDraftSeq(string p)
        {
            SetDecryptString(p);
            int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DraftID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = draftID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftSequence_SelModfy", "dsSeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["CodeTitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["CodeDutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetModfyDraftSeq('" + draftID + "')", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장 근거 문서 삭제
        /// 작성일자 : 2019-08-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDraftBaseDoc(string p)
        {
            SetDecryptString(p);
            int baseID = GetDecryptParam("BaseID", "0").ToNumber<int>();

            try
            {
                if (baseID.Equals("") || baseID.Equals("0")) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@BaseID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = baseID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DraftBaseDoc_Del", parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelDraftBaseDoc('" + baseID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장 첨부파일 삭제
        /// 작성일자 : 2019-08-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDraftAttachFile(string p)
        {
            SetDecryptString(p);
            int draftID = GetDecryptParam("DraftID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("AttachFileID", "0").ToNumber<int>();

            try
            {
                if (draftID.Equals("") || draftID.Equals("0")) return false;

                HttpContext context = HttpContext.Current;

                Data.DraftFile attDal = new Data.DraftFile();
                List<Ws.BE.DraftFileInfo> list = attDal.GetAttachFileList(CompanyID, draftID, attachFileID);

                foreach (BE.DraftFileInfo info in list)
                {
                    string tmpFilePath = info.FilePath.ToUpper().Replace("/DESKPLUSEIP/FILEDATA/", "/DeskPlusMobile/FileData/");
                    string filePath = context.Server.MapPath(tmpFilePath + info.FileGUID);

                    if (attDal.Del(CompanyID, info.AttachFileID))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelDraftAttachFile('" + draftID + "')", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장 삭제
        /// 작성일자 : 2019-08-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDraft(string p)
        {
            SetDecryptString(p);
            string draftID = GetDecryptParam("DraftID", "0");

            draftID = draftID.TrimEnd(',');
            string[] arrID = draftID.Split(',');

            try
            {
                BLL.Draft dBll = new BLL.Draft();
                foreach (string id in arrID)
                {
                    if (!id.Equals(""))
                    {
                        dBll.Del(CompanyID, id.Split('-')[0].ToNumber<int>(), id.Split('-')[1].Trim().Substring(0, 6));
                    }
                }
                return true;

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelDraft()", ex);
                throw;
            }
        }


        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeqBackup(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = docID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_SeqBackup_SelByLine", "dsSeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["TitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["DutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSeq('" + docID + "')", ex);
                throw;
            }
        }

        /// <summary>
        ///  부서정보 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfySeqBackup(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = docID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_SeqBackup_SelModfyByLine", "dsModfySeq", parm);

                List<object> listobj = new List<object>();

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BE.SequenceInfo info = new BE.SequenceInfo();

                            info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dr["ApprovalTag"].ToString();
                            info.UserID = dr["UserID"].ToString().ToNumber<int>();
                            info.UserName = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                            if (dr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dr["IsGroup"].ToString().ToUpper().Equals("G"))
                            {
                                info.DeptName = info.UserName;
                            }
                            else
                            {
                                info.DeptName = this.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                    dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            }
                            info.TitleID = dr["TitleID"].ToString().ToNumber<int>();
                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            info.DutyID = dr["DutyID"].ToString().ToNumber<int>();
                            info.DutyName = this.GetLangName(
                                    dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                    dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                    dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                                    dr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();

                            info.IsGroup = dr["IsGroup"].ToString();
                            info.IsSubTree = dr["IsSubTree"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetSeq('" + docID + "')", ex);
                throw;
            }
        }
    }
}
