using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Configuration;
using System.Data.Common;
using System.Text;

namespace Zeospace.DeskPlus.Common.Web.Messenger.Ezmaru
{
    public partial class Alarm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LodaData();

        }

        private void LodaData()
        {

            int companyID = 1;
            DataTableReader dtr = GetAlarmList();
            
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        companyID = Convert.ToInt32(dtr["CompanyID"].ToString());                        

                        DataView dv = AppSettingHelper.GetSysCode(companyID, "MessengerDomain");
                        string url = "";

                        for (int i = 0; i < dv.Count; i++)
                        {
                            url = dv[i]["InterfaceCode"].ToString();
                        }

                        if (url.ToString() != "")
                        {
                            string parm = "?dmlUid={0}&asAdmin={1}&ruidList={2}&dmlTitle={3}&dmlContentHtml={4}&noteApi={5}";
                            string ezmaruApiUrl = String.Format(parm, dtr["dmlUid"].ToString(), dtr["asAdmin"].ToString(), dtr["ruidList"].ToString(), HttpUtility.UrlEncode(dtr["dmlTitle"].ToString()), HttpUtility.UrlEncode(dtr["dmlContentHtml"].ToString()), dtr["noteApi"].ToString());
                            //string ezmaruApiUrl = String.Format(parm, "A1.maniaysw", "N", "A1.maniaysw", "제목", "본문", "Y");

                            //string rntVal =  UrlSendHelper.UsePost(url, ezmaruApiUrl);
                            string rntVal = SendPost(url, ezmaruApiUrl);
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메신저알람 : " + url + ezmaruApiUrl + ":" + rntVal, AppSettingType.MESSENGER);
                            AddAlarmHistory(dtr["AlarmID"].ToString());
                        }
                        else
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("메신저도메인없음", AppSettingType.MESSENGER);
                        }
                        

                    }
                }
            }

        }

        /// <summary>
        /// 알람 발송후 history 테이블에 insert
        /// </summary>
        /// <param name="_alarmID"></param>
        /// <returns></returns>
        private bool AddAlarmHistory(string _alarmID)
        {
            try
            {
                using (var cn = new SqlConnection(CONN_ALARM))
                {
                    var cmd = new SqlCommand("EZMARU_ALARM_History_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AlarmID", SqlDbType.Int).Value = Convert.ToInt32(_alarmID); 
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }


        }
        
        /// <summary>
        /// 알람 테이블 리스트 가져오기
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetAlarmList()
        {
            using (var cn = new SqlConnection(CONN_ALARM))
            {
                var cmd = new SqlCommand("EZMARU_ALARM_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

               return ExecuteDataTableReader(cmd);

            }

        }

        private static readonly string CONN_ALARM = ConfigurationManager.ConnectionStrings["SQLConnAlarm"].ConnectionString;

        protected DataTableReader ExecuteDataTableReader(SqlCommand cmd)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.CreateDataReader();
            }
        }

        protected static int ExecuteNonQuery(DbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 :
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mURL">Post Url</param>
        /// <returns></returns>
        public string SendPost(string _mURL, string _parm)
        {
            string getValue = "";
            if (_mURL.Length == 0) return "";

            _mURL = _mURL + _parm;
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_mURL : " + _mURL, AppSettingType.MESSENGER);
            // Send to server
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_mURL);
            httpWebRequest.Method = "Post";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Get response
            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream))
                        {
                            StringBuilder response = new StringBuilder();

                            char[] read = new Char[4096];
                            int count = responseReader.Read(read, 0, 4096);

                            while (count > 0)
                            {
                                response.Append(read, 0, count);
                                count = responseReader.Read(read, 0, 4096);
                            }
                            responseReader.Close();
                            getValue = response.ToString();
                        }
                    }
                }
            }

            getValue = getValue.Replace("\r\n", "|");
            return getValue;
        }
    }
}