using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Address.BE;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.DAL
{
    public class CSV : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_al">The _al.</param>
        /// <returns></returns>
        public int Add(List<BE.CSVInfo> _al)
        {
            if (_al.Count == 0) return 0;

            //삭제 후, 추가한다.
            Del(_al[0].CompanyID, _al[0].UserID);

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_al[0].CompanyID)))
                {
                    cn.Open();                    

                    foreach (BE.CSVInfo info in _al)
                    {
                        var cmd = new SqlCommand("DP_ADD_CSV_Ins", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = info.CompanyID;
                        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = info.UserID;
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = info.Name.ToNull().ToXSS();
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = info.Email.ToNull().ToXSS();
                        cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar, 50).Value = info.MobilePhone.ToNull().ToXSS();
                        cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 50).Value = info.HomePhone.ToNull().ToXSS();
                        cmd.Parameters.Add("@CompanyPhone", SqlDbType.NVarChar, 50).Value = info.CompanyPhone.ToNull().ToXSS();
                        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 50).Value = info.Company.ToNull().ToXSS();

                        ExecuteNonQuery(cmd);
                    }
                }

                return 1;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 선택한 CSV 을 전달받은 주소록에 넣는다.
        /// </summary>
        /// <param name="_al">The _al.</param>
        /// <returns></returns>
        public bool AddReceived(int _companyID, int _userID, string _csvIDs)
        {
            if (_csvIDs.Length == 0) return false;
            
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_ADD_CSV_InsByReceived", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@CSVID", SqlDbType.VarChar, 5000).Value = _csvIDs;

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();
                    var cmd = new SqlCommand("DP_ADD_CSV_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// csv 전체 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetCSV(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_CSV_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// CSV 의 개인주소록과 중복 제외한 리스트 가져오기(차집합)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetCSVExcept(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_CSV_SelByExcept");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// CSV 의 개인주소록과 중복 리스트(교집합) 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetCSVOverlap(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ADDRESS, "QRY_ADD_CSV_SelByOverlap");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// CSV 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetCSVCount(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {                
                var cmd = new SqlCommand("DP_ADD_CSV_SelCount", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 개인주소록 내보내기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPrivateAddress(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ADD_Private_SelGetCSV", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

    }
}
