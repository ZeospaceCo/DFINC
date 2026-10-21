using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserEducation : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
            cmd.Parameters.Add("@EducationID", SqlDbType.Int).Value = _info.EducationID;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar,100).Value = _info.Name.ToNull().ToXSS();
            cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _info.Point;
            cmd.Parameters.Add("@Grade", SqlDbType.NVarChar,100).Value = _info.Grade.ToNull().ToXSS();
            cmd.Parameters.Add("@Place", SqlDbType.NVarChar,100).Value = _info.Place.ToNull().ToXSS();
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
            cmd.Parameters.Add("@IsFinish", SqlDbType.VarChar,50).Value = _info.IsFinish.ToNull().ToXSS();
            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
            cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
            cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
            cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;
         * */

        #region DataReader

        private UserEducationInfo GetUserEducation(IDataReader _reader)
        {
            return new UserEducationInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["EducationID"] == DBNull.Value) ? -1 : (int)_reader["EducationID"],
                _reader["Name"].ToString().ToReverseXSS(),
                (_reader["Point"] == DBNull.Value) ? -1 : (int)_reader["Point"],
                _reader["Grade"].ToString().ToReverseXSS(),
                _reader["Place"].ToString().ToReverseXSS(),
                (_reader["StartDate"] == DBNull.Value) ? "" : _reader["StartDate"].ToString(),
                (_reader["EndDate"] == DBNull.Value) ? "" : _reader["EndDate"].ToString(),
                _reader["IsFinish"].ToString(),
                (_reader["Orders"] == DBNull.Value) ? -1 : (int)_reader["Orders"],
                (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
                (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
                );
        }

        protected List<UserEducationInfo> GetUserEducationList(IDataReader reader)
        {
            List<UserEducationInfo> list = new List<UserEducationInfo>();
            while (reader.Read())
                list.Add(GetUserEducation(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 교육정보 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.UserEducationInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEdu_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 500).Value = (_info.Name != "") ? AESManager.Encrypt(_info.Name.ToNull().ToXSS()) : "";
                    cmd.Parameters.Add("@Point", SqlDbType.VarChar, 500).Value = AESManager.Encrypt(_info.Point.ToString().ToXSS());
                    cmd.Parameters.Add("@Grade", SqlDbType.NVarChar, 100).Value = _info.Grade.ToNull().ToXSS();
                    cmd.Parameters.Add("@Place", SqlDbType.NVarChar, 100).Value = _info.Place.ToNull().ToXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _info.StartDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@EndDate", SqlDbType.VarChar, 8).Value = _info.EndDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsFinish", SqlDbType.VarChar, 50).Value = _info.IsFinish.ToNull().ToXSS();
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
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_educationID">The _education ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _educationID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserEdu_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@EducationID", SqlDbType.Int).Value = _educationID;

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
        /// 교육 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserEducationInfo> GetUserEducationInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserEdu_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                List<UserEducationInfo> al = new List<UserEducationInfo>();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BE.UserEducationInfo be = new UserEducationInfo();
                        be.CompanyID = reader["CompanyID"].ToString().ToNull().ToNumber<int>();
                        be.UserID = reader["UserID"].ToString().ToNull().ToNumber<int>();
                        be.EducationID = reader["EducationID"].ToString().ToNull().ToNumber<int>();
                        be.Name = (reader["Name"].ToString().ToNull() != "") ? AESManager.Decrypt(reader["Name"].ToString()).ToNull().ToReverseXSS() : "";
                        be.Point = AESManager.Decrypt(reader["Point"].ToString()).ToNull().ToNumber<int>();
                        be.Grade = reader["Grade"].ToString().ToNull().ToReverseXSS();
                        be.Place = reader["Place"].ToString().ToNull().ToReverseXSS();
                        be.StartDate = reader["StartDate"].ToString().ToNull().ToReverseXSS();
                        be.EndDate = reader["EndDate"].ToString().ToNull().ToReverseXSS();
                        be.IsFinish = reader["IsFinish"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishKo = reader["IsFinishKo"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishEn = reader["IsFinishEn"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishJa = reader["IsFinishJa"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishFr = reader["IsFinishFr"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishZhcn = reader["IsFinishZhcn"].ToString().ToNull().ToReverseXSS();
                        be.IsFinishZhtw = reader["IsFinishZhtw"].ToString().ToNull().ToReverseXSS();
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