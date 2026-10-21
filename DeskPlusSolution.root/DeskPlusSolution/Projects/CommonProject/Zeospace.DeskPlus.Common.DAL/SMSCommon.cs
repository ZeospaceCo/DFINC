using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class SMSCommon : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 최신번호

        /// <summary>
        /// Gets the recent phone.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetRecentPhone(int _companyID, int _userID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetRecentPhone");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// Adds the recent phone.
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddRecentPhone(SuremRecentPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = _info.Name;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _info.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _info.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _info.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 최신전화번호 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ModRecentPhone(SuremRecentPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = _info.Name;
                    cmd.Parameters.Add("@SuremRecentPhoneID", SqlDbType.Int).Value = _info.SuremRecentPhoneID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _info.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _info.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _info.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 최신전화번호 삭제
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool DelRecentPhone(SuremRecentPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremRecentPhone_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@SuremRecentPhoneID", SqlDbType.Int).Value = _info.SuremRecentPhoneID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 최신번호

        #region 회신번호

        /// <summary>
        /// 회신전화번호 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_suremReplyPhoneID">The _surem reply phone ID.</param>
        /// <returns></returns>
        public DataSet GetReplyPhone(int _companyID, int _userID, int _suremReplyPhoneID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _suremReplyPhoneID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetReplyPhone");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회신 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int AddReplyPhone(SuremReplyPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _info.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _info.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _info.PhoneNumber3;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회신 수정
        /// </summary>
        /// <param name="_suremReplyPhoneInfo">The _surem reply phone info.</param>
        /// <returns></returns>
        public bool ModReplyPhone(SuremReplyPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Update", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _info.SuremReplyPhoneID;
                    cmd.Parameters.Add("@PhoneNumber1", SqlDbType.VarChar, 10).Value = _info.PhoneNumber1;
                    cmd.Parameters.Add("@PhoneNumber2", SqlDbType.VarChar, 10).Value = _info.PhoneNumber2;
                    cmd.Parameters.Add("@PhoneNumber3", SqlDbType.VarChar, 10).Value = _info.PhoneNumber3;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool DelReplyPhone(SuremReplyPhoneInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhone_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _info.SuremReplyPhoneID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 회신번호

        #region 회신 기본설정(ReplyPhoneBase)

        /// <summary>
        /// Dels the specified _user ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataSet GetReplyPhoneBase(int _companyID, int _userID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Select", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetReplyPhoneBase");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddReplyPhoneBase(SuremReplyPhoneBaseInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SuremReplyPhoneBaseID", SqlDbType.Int).Value = _info.SuremReplyPhoneBaseID;
                    cmd.Parameters.Add("@ReplyPhoneNumber", SqlDbType.VarChar, 30).Value = _info.ReplyPhoneNumber;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자로 회신 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool DelReplyPhoneBase(int _companyID, int _userID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_DeleteByUserID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool DelReplyPhoneBase(SuremReplyPhoneBaseInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnAlarm(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_ALARM_SuremReplyPhoneBase_Delete", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SuremReplyPhoneID", SqlDbType.Int).Value = _info.SuremReplyPhoneID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 회신 기본설정(ReplyPhoneBase)
    }
}