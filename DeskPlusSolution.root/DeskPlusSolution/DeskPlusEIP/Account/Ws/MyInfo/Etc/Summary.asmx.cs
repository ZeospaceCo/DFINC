using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Etc
{
    /// <summary>
    /// Summary의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Summary : Base.BaseWSAccount
    {
        public string GetTitle(string _cat)
        {
            string rtnval = "";
            switch (_cat.ToUpper())
            {
                case "PRI":
                    rtnval = this.GetResource("Summary01");      //개인문서함
                    break;
                case "APP":
                    rtnval = this.GetResource("Summary02");      //결재문서함
                    break;
                case "REJ":
                    rtnval = this.GetResource("Summary03");      //반려문서함
                    break;
                case "AAA":
                    rtnval = this.GetResource("Summary04");      //전결문서함
                    break;
                case "FOR":
                    rtnval = this.GetResource("Summary05");      //전달문서함
                    break;
                case "CIRR":
                    rtnval = this.GetResource("Summary06");      //받은회람
                    break;
                case "CIRS":
                    rtnval = this.GetResource("Summary07");      //보낸회람
                    break;

                case "SUB":
                    rtnval = this.GetResource("Summary08");      //진행중인문서(상신)
                    break;
                case "REL":
                    rtnval = this.GetResource("Summary09");      //진행중인문서(관계있는문서)
                    break;
                case "TAR":
                    rtnval = this.GetResource("Summary10");      //진행중인문서(결재대상)
                    break;
                case "CPP":
                    rtnval = this.GetResource("Summary11");      //진행중인문서(수신)
                    break;

                case "CO-REQ":
                    rtnval = this.GetResource("Summary12");      //요청한협업
                    break;
                case "CO-TAR":
                    rtnval = this.GetResource("Summary13");      //요청받은협업
                    break;
                case "CO-CC":
                    rtnval = this.GetResource("Summary14");      //참조받은협업
                    break;
                case "CO-FOR":
                    rtnval = this.GetResource("Summary15");      //전달받은협업
                    break;

                default:
                    rtnval = "";
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 진행중인문서
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<string> GetApp(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";           //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Etc.Summary).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                if (userID.Equals(0) || deptID.Equals(0)) return null;

                List<string> obj = new List<string>();
                string tmping = "", tmpcomp = "";

                DAL.User dal = new DAL.User();

                // 진행중인문서
                DataTableReader dtr = dal.GetSummaryAppIng(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmping += " " + GetTitle(dtr["CAT"].ToString()) + "(" + dtr["Cnt"].ToString() + GetResource("SummaryEA") + "),";
                        }
                    }
                }
                obj.Add(tmping.TrimEnd(','));

                // 완료된 문서
                dtr = dal.GetSummaryAppCom(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpcomp += " " + GetTitle(dtr["CAT"].ToString()) + "(" + dtr["Cnt"].ToString() + GetResource("SummaryEA") + "),";
                        }
                    }
                }
                obj.Add(tmpcomp.TrimEnd(','));

                return obj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 게시판정보
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<string> GetBoa(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";           //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Etc.Summary).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                if (userID.Equals(0) || deptID.Equals(0)) return null;

                List<string> obj = new List<string>();
                string tmpcnt = "", tmpnm="";

                DAL.User dal = new DAL.User();

                // 게시글 등록건수 
                DataTableReader dtr = dal.GetSummaryBoaList(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpcnt += dtr["Cnt"].ToString() + GetResource("SummaryEA");
                        }
                    }
                }
                obj.Add(tmpcnt.TrimEnd(','));

                // 게시판 폴더 관리목록
                dtr = dal.GetSummaryBoaCategory(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpnm += " " + dtr["PathNameBase"].ToString() + ",";
                        }
                    }
                    else
                    {
                        tmpnm = " " + this.GetResource("NoContent");
                    }
                }
                obj.Add(tmpnm.TrimEnd(','));

                return obj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 커뮤니티
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<string> GetComm(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";           //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Etc.Summary).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                if (userID.Equals(0) || deptID.Equals(0)) return null;

                List<string> obj = new List<string>();
                string tmpcnt = "", tmpnm = "";

                DAL.User dal = new DAL.User();

                // 커뮤니티 게시글 등록건수
                DataTableReader dtr = dal.GetSummaryCmnBoa(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpcnt += " " + dtr["Name"].ToString().ToReverseXSS() + "(" + dtr["WriteCnt"].ToString() + GetResource("SummaryEA") + "),";
                        }
                    }
                    else
                    {
                        tmpcnt = " " + this.GetResource("NoContent");
                    }
                }
                obj.Add(tmpcnt.TrimEnd(','));

                // 커뮤니티 관리목록
                dtr = dal.GetSummaryCmnMgr(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpnm += " " + dtr["Name"].ToString().ToReverseXSS() + ",";
                        }
                    }
                    else
                    {
                        tmpnm = " " + this.GetResource("NoContent");
                    }
                }
                obj.Add(tmpnm.TrimEnd(','));

                return obj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 업무협업
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<string> GetTask(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";           //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Etc.Summary).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                if (userID.Equals(0) || deptID.Equals(0)) return null;

                List<string> obj = new List<string>();
                string tmpvals = "";

                DAL.User dal = new DAL.User();

                // 업무협업 등록건수
                DataTableReader dtr = dal.GetSummaryTask(companyID, deptID, userID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            tmpvals += " " + GetTitle(dtr["CAT"].ToString()) + "(" + dtr["Cnt"].ToString() + GetResource("SummaryEA") + "),";
                        }
                    }
                }

                obj.Add(tmpvals.TrimEnd(','));
                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
