using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Task.Web.Ws.Home
{
    /// <summary>
    /// AdminMain의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AdminMain : Base.BaseWSTask
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 지연,진행중인 업무리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetStatusList(string p)
        {
            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string status = GetDecryptParam("status", "6");
                string order = GetDecryptParam("order", "");
                string year = GetDecryptParam("year", "");                

                DAL.List biz = new DAL.List();

                DataTableReader dtr = biz.GetStatusAdminList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo, status, order, year);
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();
                            info.ViewerCount = dtr["ViewerCount"].ToString().ToNumber<int>();
                            //info.ExcessDay = dtr["ExcessDay"].ToString();
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-","+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();

                            #region 요청자 이름 & 부서 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion                                                       

                            #region 수신자 이름 & 부서 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);

                            string[] tmpToDept = dtr["TODeptName"].ToString().Split('|');
                            info.TODeptName = this.GetLangName(
                                    tmpToDept[0], tmpToDept[1], tmpToDept[2], tmpToDept[3],
                                    tmpToDept[4], tmpToDept[5], tmpToDept[6], PreLang);
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

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
        /// 작성자 : famo23
        /// 작성일자 : 2017-02-23
        /// 작성목적 : 최신 업데이트 업무리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRecentList(string p)
        {
            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string year = GetDecryptParam("year", "");

                DAL.List biz = new DAL.List();

                DataTableReader dtr = biz.GetRecentAdminList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo, year);
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();
                            info.ViewerCount = dtr["ViewerCount"].ToString().ToNumber<int>();
                            //info.ExcessDay = dtr["ExcessDay"].ToString();
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-", "+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();
                            info.Status = GetStatus(dtr["StatusLast"].ToString()); //1:대기;2:접수;3:처리중;4:완료;5:반려

                            #region 요청자 이름 & 부서 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 수신자 이름 & 부서 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);

                            string[] tmpToDept = dtr["TODeptName"].ToString().Split('|');
                            info.TODeptName = this.GetLangName(
                                    tmpToDept[0], tmpToDept[1], tmpToDept[2], tmpToDept[3],
                                    tmpToDept[4], tmpToDept[5], tmpToDept[6], PreLang);
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

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
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-24
        /// 작성목적 : 중요표시 업무리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetImpoList(string p)
        {
            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string year = GetDecryptParam("year", "");                

                DAL.List biz = new DAL.List();

                DataTableReader dtr = biz.GetImpoAdminList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo, year);
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();
                            info.ViewerCount = dtr["ViewerCount"].ToString().ToNumber<int>();
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-", "+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();

                            #region 요청자 이름 & 부서 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion                                                        

                            #region 수신자 이름 & 부서 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);

                            string[] tmpToDept = dtr["TODeptName"].ToString().Split('|');
                            info.TODeptName = this.GetLangName(
                                    tmpToDept[0], tmpToDept[1], tmpToDept[2], tmpToDept[3],
                                    tmpToDept[4], tmpToDept[5], tmpToDept[6], PreLang);
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

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
        /// 작성자 : famo23
        /// 작성일자 : 2016-01-12
        /// 작성목적 : 탭 환경 저장
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModEnv(string p)
        {
            try
            {
                SetDecryptString(p);

                string isType = GetDecryptParam("isPage", "Main");

                // 기본탭
                Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
                envdal.TaskHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isType);

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
