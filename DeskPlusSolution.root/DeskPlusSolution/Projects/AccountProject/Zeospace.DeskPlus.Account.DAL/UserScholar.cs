using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserScholar : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@ScholarshipID", SqlDbType.Int).Value = _info.ScholarshipID;
        cmd.Parameters.Add("@StartDate", SqlDbType.VarChar,8).Value = _info.StartDate.ToNull().ToXSS();
        cmd.Parameters.Add("@EndDate", SqlDbType.VarChar,8).Value = _info.EndDate.ToNull().ToXSS();
        cmd.Parameters.Add("@SchoolName", SqlDbType.NVarChar,50).Value = _info.SchoolName.ToNull().ToXSS();
        cmd.Parameters.Add("@Degree", SqlDbType.VarChar,50).Value = _info.Degree.ToNull().ToXSS();
        cmd.Parameters.Add("@Major", SqlDbType.NVarChar,50).Value = _info.Major.ToNull().ToXSS();
        cmd.Parameters.Add("@GraduateType", SqlDbType.VarChar,50).Value = _info.GraduateType.ToNull().ToXSS();
        cmd.Parameters.Add("@Etc", SqlDbType.NVarChar,100).Value = _info.Etc.ToNull().ToXSS();
        cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
        cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
        cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
        cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
        cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate
        */

        #region DataReader

        private UserScholarInfo GetUserScholar(IDataReader _reader)
        {
            return new UserScholarInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["ScholarshipID"] == DBNull.Value) ? -1 : (int)_reader["ScholarshipID"],
                _reader["StartDate"].ToString(),
                _reader["EndDate"].ToString(),
                _reader["SchoolName"].ToString().ToReverseXSS(),
                _reader["Degree"].ToString().ToReverseXSS(),
                _reader["Major"].ToString().ToReverseXSS(),
                _reader["GraduateType"].ToString().ToReverseXSS(),
                _reader["Etc"].ToString().ToReverseXSS(),
                (_reader["Orders"] == DBNull.Value) ? -1 : (int)_reader["Orders"],
                (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
                (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]);
        }

        protected List<UserScholarInfo> GetUserScholarList(IDataReader reader)
        {
            List<UserScholarInfo> list = new List<UserScholarInfo>();
            while (reader.Read())
                list.Add(GetUserScholar(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 부서 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.UserScholarInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserScholar_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _info.StartDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 8).Value = _info.EndDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@SchoolName", SqlDbType.VarChar, 500).Value = AESManager.Encrypt(_info.SchoolName.ToNull().ToXSS());
                    cmd.Parameters.Add("@Degree", SqlDbType.VarChar, 50).Value = _info.Degree.ToNull().ToXSS();
                    cmd.Parameters.Add("@Major", SqlDbType.NVarChar, 50).Value = _info.Major.ToNull().ToXSS();
                    cmd.Parameters.Add("@GraduateType", SqlDbType.VarChar, 50).Value = _info.GraduateType.ToNull().ToXSS();
                    cmd.Parameters.Add("@Etc", SqlDbType.NVarChar, 100).Value = _info.Etc.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

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
        /// 용도 : 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scholarshipID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _scholarshipID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserScholar_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ScholarshipID", SqlDbType.Int).Value = _scholarshipID;

                    cn.Open();

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 학력 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserScholarInfo> GetUserScholarInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserScholar_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                List<UserScholarInfo> al = new List<UserScholarInfo>();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BE.UserScholarInfo be = new UserScholarInfo();

                        be.CompanyID = reader["CompanyID"].ToString().ToNull().ToNumber<int>();
                        be.UserID = reader["UserID"].ToString().ToNull().ToNumber<int>();
                        be.ScholarshipID = reader["ScholarshipID"].ToString().ToNull().ToNumber<int>();
                        be.StartDate = reader["StartDate"].ToString().ToNull().ToReverseXSS();
                        be.EndDate = reader["EndDate"].ToString().ToNull().ToReverseXSS();
                        be.SchoolName = AESManager.Decrypt(reader["SchoolName"].ToString()).ToNull().ToReverseXSS();
                        be.Degree = reader["Degree"].ToString().ToNull().ToReverseXSS();

                        be.DegreeKo = reader["DegreeKo"].ToString().ToNull().ToReverseXSS();
                        be.DegreeEn = reader["DegreeEn"].ToString().ToNull().ToReverseXSS();
                        be.DegreeJa = reader["DegreeJa"].ToString().ToNull().ToReverseXSS();
                        be.DegreeFr = reader["DegreeFr"].ToString().ToNull().ToReverseXSS();
                        be.DegreeZhcn = reader["DegreeZhcn"].ToString().ToNull().ToReverseXSS();
                        be.DegreeZhtw = reader["DegreeZhtw"].ToString().ToNull().ToReverseXSS();

                        be.Major = reader["Major"].ToString().ToNull().ToReverseXSS();
                        be.GraduateType = reader["GraduateType"].ToString().ToNull().ToReverseXSS();

                        be.GraduateTypeKo = reader["GraduateTypeKo"].ToString().ToNull().ToReverseXSS();
                        be.GraduateTypeEn = reader["GraduateTypeEn"].ToString().ToNull().ToReverseXSS();
                        be.GraduateTypeJa = reader["GraduateTypeJa"].ToString().ToNull().ToReverseXSS();
                        be.GraduateTypeFr = reader["GraduateTypeFr"].ToString().ToNull().ToReverseXSS();
                        be.GraduateTypeZhcn = reader["GraduateTypeZhcn"].ToString().ToNull().ToReverseXSS();
                        be.GraduateTypeZhtw = reader["GraduateTypeZhtw"].ToString().ToNull().ToReverseXSS();

                        be.Etc = reader["Etc"].ToString().ToNull().ToReverseXSS();
                        be.Orders = reader["Orders"].ToString().ToNull().ToNumber<int>();
                        be.WriteUserID = reader["WriteUserID"].ToString().ToNull().ToNumber<int>();

                        al.Add(be);
                    }
                }

                return al;
            }
        }

        #endregion 가져오기
    }
}