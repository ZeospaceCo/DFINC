using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.MDM.WS
{
    /// <summary>
    /// PictureUpload의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetNotification : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 
        /// 작성일자 : 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ReadNotification(string p)
        {
            try
            {
                SetDecryptString(p);
                int notificationID = GetDecryptParam("NotificationID", "0").ToNumber<int>();
                string mobileLinkUrl = GetDecryptParam("mobileLinkUrl", "");
                if (notificationID == 0) return "false";

                SqlParameter[] parm = new SqlParameter[]{
                                                 new SqlParameter("@notificationID", SqlDbType.Int)
                    };
                parm[0].Value = notificationID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_Read", parm);

                return mobileLinkUrl;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ReadNotification()", ex);
                return "false";
            }

        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 
        /// 작성일자 : 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ReadAllNotification(string p)
        {
            try
            {
                SetDecryptString(p);
                string notiIDList = GetDecryptParam("NotiIDList", "");
                if (notiIDList.Length == 0) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                                 new SqlParameter("@NotiIDList", SqlDbType.NVarChar, 500)
                    };
                parm[0].Value = notiIDList;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_ReadAll", parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ReadAllNotification()", ex);
                return false;
            }

        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 
        /// 작성일자 : 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAllNotification(string p)
        {
            try
            {
                SetDecryptString(p);
                string notiIDList = GetDecryptParam("NotiIDList", "");
                if (notiIDList.Length == 0) return false;

                SqlParameter[] parm = new SqlParameter[]{
                                                 new SqlParameter("@NotiIDList", SqlDbType.NVarChar, 500)
                    };
                parm[0].Value = notiIDList;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_DelAll", parm);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelAllNotification()", ex);
                return false;
            }

        }

    }
}
