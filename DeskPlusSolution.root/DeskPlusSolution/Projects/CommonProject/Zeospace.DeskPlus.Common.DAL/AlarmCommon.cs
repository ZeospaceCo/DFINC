using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class AlarmCommon : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// Gets the login ID PWD.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GetLoginIDPwd(int _companyID, int _userID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string receiverPwd = "", receiverLoginID = "";
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserID");
                query = string.Format(query, "'1'", '1');

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receiverLoginID = reader["LogInID"].ToString();
                        receiverPwd = reader["Password"].ToString();
                    }
                }

                return receiverLoginID + "^" + receiverPwd;
            }
        }

        /// <summary>
        /// Gets the login ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GetLoginID(int _companyID, int _userID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string receiverLoginID = "";
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_User_SelByUserID");
                query = string.Format(query, "'1'", '1');

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receiverLoginID = reader["LogInID"].ToString();
                    }
                }

                return receiverLoginID;
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// Adds the specified _listinfo.
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Add(AlarmInfo _listinfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_listinfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_COMMON_INSERT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@SenderID", SqlDbType.Int).Value = _listinfo.SenderID;
                    cmd.Parameters.Add("@SenderName", SqlDbType.NVarChar, 50).Value = _listinfo.SenderName.ToNull();
                    cmd.Parameters.Add("@SenderMail", SqlDbType.NVarChar, 50).Value = _listinfo.SenderMail.ToNull();
                    cmd.Parameters.Add("@SenderPhone", SqlDbType.VarChar, 15).Value = _listinfo.SenderPhone.ToNull();
                    cmd.Parameters.Add("@ReceiverID", SqlDbType.Int).Value = _listinfo.ReceiverID;
                    cmd.Parameters.Add("@ReceiverName", SqlDbType.NVarChar, 50).Value = _listinfo.ReceiverName.ToNull();
                    cmd.Parameters.Add("@ReceiverMail", SqlDbType.NVarChar, 50).Value = _listinfo.ReceiverMail.ToNull();
                    cmd.Parameters.Add("@ReceiverPhone", SqlDbType.VarChar, 15).Value = _listinfo.ReceiverPhone.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _listinfo.Status.ToNull();
                    cmd.Parameters.Add("@SendDate", SqlDbType.DateTime).Value = _listinfo.SendDate;
                    cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 70).Value = _listinfo.Subject.ToNull();
                    cmd.Parameters.Add("@Content", SqlDbType.NVarChar, 1000).Value = _listinfo.Content.ToNull();
                    cmd.Parameters.Add("@TargetUrl", SqlDbType.NVarChar, 500).Value = _listinfo.TargetUrl;
                    cmd.Parameters.Add("@TargetMobileUrl", SqlDbType.NVarChar, 500).Value = _listinfo.TargetMobileUrl.ToNull();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _listinfo.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _listinfo.EndDate;
                    cmd.Parameters.Add("@AlarmType", SqlDbType.TinyInt).Value = _listinfo.AlarmType;
                    cmd.Parameters.Add("@KindSubject", SqlDbType.NVarChar, 60).Value = _listinfo.KindSubject.ToNull();
                    cmd.Parameters.Add("@ViewOption", SqlDbType.NVarChar, 2).Value = _listinfo.ViewOption.ToNull();
                    cmd.Parameters.Add("@Store", SqlDbType.TinyInt).Value = _listinfo.Store;
                    cmd.Parameters.Add("@UserState", SqlDbType.TinyInt).Value = _listinfo.UserState;
                    cmd.Parameters.Add("@Random", SqlDbType.VarChar,10).Value = _listinfo.RandomStr;
                    cmd.Parameters.Add("@MsgSendID", SqlDbType.Int).Value = _listinfo.MsgSendID;
                    cmd.Parameters.Add("@AlarmUrl", SqlDbType.NVarChar, 500).Value = _listinfo.AlarmUrl;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                    cmd.Parameters.Add("@DisplayID", SqlDbType.Int).Value = _listinfo.DisplayID;


                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion ADD()



        #region DEL()

        /// <summary>
        /// Dels the specified _company ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_senderID">The _sender ID.</param>
        /// <param name="_alarmID">The _alarm ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _senderID, int _alarmID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_COMMON_DELETE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SenderID", SqlDbType.Int).Value = _senderID;
                    cmd.Parameters.Add("@AlarmID", SqlDbType.Int).Value = _alarmID;
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion DEL()
    }
}