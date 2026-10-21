using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Config.Ws
{
    /// <summary>
    /// GetConfig의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetConfig : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 환경설정 정보 가져오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetEnvironment()
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERENVIRONMENT");

            sqlQuery = String.Format(sqlQuery, CompanyID, UserID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "UserEnv", null);

            return StringHelper.GetJSONString(ds.Tables[0]);
        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 기기별 환경설정 정보 가져오기
        /// 작성일자 : 2019-07-30
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMDMEnvironment()
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERENVIRONMENT");

            sqlQuery = String.Format(sqlQuery, CompanyID, UserID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "UserEnv", null);

            return StringHelper.GetJSONString(ds.Tables[0]);
        }
    }
}
