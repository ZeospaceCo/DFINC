using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using BoardData = Zeospace.DeskPlusMobile.Web.Board.Ws.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlusMobile.Web.Attendance.Ws
{
    /// <summary>
    /// GetAttendance의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetAttendance : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : 이지원
        /// 작성목적 : 근태정보 가져오기
        /// 작성일자 : 2018-05-04
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttendanceData(string p)
        {
            string rtnAttendanceData = "|||";

            SetDecryptString(p);
            string selDate = GetDecryptParam("selDate", "").ToString();

            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SelDate", SqlDbType.NVarChar, 10)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = selDate;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAttendance, "SEL_PROC_ATTENDANCEDATA");
                DataTableReader dtr = SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsAttendance", parm);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnAttendanceData = dtr["bWS"].ToString() + "|" + dtr["WSTime"].ToString() + "|" + dtr["bWC"].ToString() + "|" + dtr["WCTime"].ToString();
                        }
                    }
                }

                return rtnAttendanceData;

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetAttendance", ex);
                throw;
            }
        }
    }
}
