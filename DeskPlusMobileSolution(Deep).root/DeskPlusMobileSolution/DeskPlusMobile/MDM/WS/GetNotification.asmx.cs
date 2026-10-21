using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.MDM.WS
{
    /// <summary>
    /// GetWebHard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetNotification : Helper.MobileBaseService
    {

        #region GET()

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 알람 리스트 가져오기
        /// 작성일자 : 2019-08-30
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);
            int pageNo = GetDecryptParam("PageNo", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("PageSize", "0").ToNumber<int>();

            try
            {
                this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.WebHard.Ws.GetWebHard).Assembly;	//어셈블리 선언            
                this.GetResourceManager();

                List<object> listobj = new List<object>();

                SqlParameter[] parms = new SqlParameter[] {
                                                     new SqlParameter("@CompanyID", SqlDbType.Int)
                                                    ,new SqlParameter("@ReceiverID", SqlDbType.Int)
                                                    ,new SqlParameter("@PageNo", SqlDbType.VarChar,10)
                                                    ,new SqlParameter("@PageSize", SqlDbType.VarChar,10)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = pageNo;
                parms[3].Value = pageSize;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_PROC_MDM_NotificationBox");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "NotiList", parms);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.NotificationInfo notificationInfo = new BE.NotificationInfo();

                    notificationInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    notificationInfo.NotificationID = dr["NotificationID"].ToString().ToNumber<int>();
                    notificationInfo.ReceiverID = dr["ReceiverID"].ToString().ToNumber<int>();

                    notificationInfo.Message = GetStringMessage(dr["KindSubject"].ToString(), dr["Message"].ToString());

                    string tmpURL = dr["TargetUrl"].ToString();
                    if (!String.IsNullOrEmpty(tmpURL))
                    {
                        if (tmpURL.IndexOf("tURL=") >= 0)
                        {
                            tmpURL = tmpURL.Substring(tmpURL.IndexOf("tURL="));
                        }
                    }

                    if (dr["KindSubject"].ToString() == "Mail")
                    {
                        tmpURL = "/DeskPlusMobile/Mail/Folder.aspx?";
                    }

                    notificationInfo.TargetUrl = tmpURL.Replace("tURL=", "");
                    notificationInfo.KindSubject = GetStringKind(dr["KindSubject"].ToString());

                    notificationInfo.ReadFlag = dr["ReadFlag"].ToString().ToNumber<int>();
                    notificationInfo.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();
                    notificationInfo.srtWriteDate = dr["WriteDate"].ToString();
                    notificationInfo.MsgSendID = dr["MsgSendID"].ToString().ToNumber<int>();
                    notificationInfo.Content = dr["Content"].ToString().Replace("\n", "<br/>");

                    #region 이름 다국어 설정
                    notificationInfo.WriteUserName = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서 다국어 설정
                    notificationInfo.WriteDeptName = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                    #endregion

                    #region 직급 다국어 설정
                    notificationInfo.WriteTitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                    #endregion

                    DateTime writeDate = Convert.ToDateTime(dr["WriteDate"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();
                    notificationInfo.ToDay = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";

                    listobj.Add(notificationInfo);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList()", ex);
                throw;
            }
        }
        #endregion

        #region 기타

        public string GetStringMessage(string _kindSubject, string _message)
        {
            var resultStr = "";
            switch (_kindSubject)
            {
                case "Mail":
                    resultStr = resMng.GetString("MobileNotiMessege");
                    break;
                default:
                    resultStr = _message;
                    break;
            }

            return resultStr;
        }

        public string GetStringKind(string _kindSubject)
        {
            var resultStr = "";
            switch (_kindSubject)
            {
                case "Mail":
                case "Approval":
                case "Board":
                case "Community":
                case "SNS":
                case "ResourceManager":
                    resultStr = resMng.GetString(_kindSubject);
                    break;
                case "TASK":
                    resultStr = resMng.GetString("Coop");
                    break;
                case "Pim":
                    resultStr = resMng.GetString("Pims");
                    break;
                case "Report":
                    resultStr = resMng.GetString("Work");
                    break;
                case "Notice":
                    resultStr = resMng.GetString("AlarmNotice");
                    break;
                default:
                    resultStr = "";
                    break;
            }

            return resultStr;
        }

        #endregion
    }
}
