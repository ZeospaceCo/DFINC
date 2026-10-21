using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserFamily : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
            cmd.Parameters.Add("@FamilyID", SqlDbType.Int).Value = _info.FamilyID;
            cmd.Parameters.Add("@Relationship", SqlDbType.VarChar,50).Value = _info.Relationship.ToNull().ToXSS();
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar,100).Value = _info.Name.ToNull().ToXSS();
            cmd.Parameters.Add("@Birthday", SqlDbType.VarChar,50).Value = _info.Birthday.ToNull().ToXSS();
            cmd.Parameters.Add("@Job", SqlDbType.NVarChar,100).Value = _info.Job.ToNull().ToXSS();
            cmd.Parameters.Add("@Scholarship", SqlDbType.VarChar,50).Value = _info.Scholarship.ToNull().ToXSS();
            cmd.Parameters.Add("@IsWithLive", SqlDbType.VarChar,50).Value = _info.IsWithLive.ToNull().ToXSS();
            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
            cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
            cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
            cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;
            */

        #region DataReader

        private UserFamilyInfo GetUserFamilyInfo(IDataReader _reader)
        {
            return new UserFamilyInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["FamilyID"] == DBNull.Value) ? -1 : (int)_reader["FamilyID"],
                _reader["Relationship"].ToString().ToNull().ToReverseXSS(),
                _reader["Name"].ToString().ToNull().ToReverseXSS(),
                _reader["Birthday"].ToString().ToNull().ToReverseXSS(),
                _reader["Job"].ToString().ToNull().ToReverseXSS(),
                _reader["Scholarship"].ToString().ToNull().ToReverseXSS(),
                _reader["IsWithLive"].ToString().ToNull().ToReverseXSS(),
                (_reader["Orders"] == DBNull.Value) ? -1 : (int)_reader["Orders"],
                (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
                (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
                );
        }

        protected List<UserFamilyInfo> GetUserFamilyList(IDataReader reader)
        {
            List<UserFamilyInfo> list = new List<UserFamilyInfo>();
            while (reader.Read())
                list.Add(GetUserFamilyInfo(reader));
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
        public bool Add(BE.UserFamilyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserFamily_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Relationship", SqlDbType.VarChar, 50).Value = _info.Relationship.ToNull().ToXSS();
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 500).Value = (_info.Name.ToNull() != "") ? AESManager.Encrypt(_info.Name.ToNull().ToXSS()) : "";
                    cmd.Parameters.Add("@Birthday", SqlDbType.VarChar, 500).Value = (_info.Birthday.ToNull() != "") ? AESManager.Encrypt(_info.Birthday.ToNull().ToXSS()) : "";
                    cmd.Parameters.Add("@Job", SqlDbType.NVarChar, 100).Value = _info.Job.ToNull().ToXSS();
                    cmd.Parameters.Add("@Scholarship", SqlDbType.VarChar, 50).Value = _info.Scholarship.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsWithLive", SqlDbType.VarChar, 50).Value = _info.IsWithLive.ToNull().ToXSS();
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
        /// <param name="_rewardID">The _reward ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _familyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserFamily_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FamilyID", SqlDbType.Int).Value = _familyID;

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
        /// 가족 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<UserFamilyInfo> GetUserFamilyInfo(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserFamily_Sel");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    List<UserFamilyInfo> al = new List<UserFamilyInfo>();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BE.UserFamilyInfo be = new UserFamilyInfo();
                            be.CompanyID = reader["CompanyID"].ToString().ToNull().ToNumber<int>();
                            be.UserID = reader["UserID"].ToString().ToNull().ToNumber<int>();
                            be.FamilyID = reader["FamilyID"].ToString().ToNull().ToNumber<int>();
                            be.Relationship = reader["Relationship"].ToString().ToNull().ToReverseXSS();
                            be.Name = (reader["Name"].ToString().ToNull().ToReverseXSS() != "") ? AESManager.Decrypt(reader["Name"].ToString()).ToNull().ToReverseXSS() : "";
                            be.Birthday = (reader["Birthday"].ToString().ToNull().ToReverseXSS() != "") ? AESManager.Decrypt(reader["Birthday"].ToString()).ToNull().ToReverseXSS() : "";
                            be.Job = reader["Job"].ToString().ToNull().ToReverseXSS();
                            be.Scholarship = reader["Scholarship"].ToString().ToNull().ToReverseXSS();
                            be.IsWithLive = reader["IsWithLive"].ToString().ToNull().ToReverseXSS();
                            be.Orders = reader["Orders"].ToString().ToNull().ToNumber<int>();

                            be.RelNameKo = reader["RelNameKo"].ToString().ToNull().ToReverseXSS();
                            be.RelNameEn = reader["RelNameEn"].ToString().ToNull().ToReverseXSS();
                            be.RelNameJa = reader["RelNameJa"].ToString().ToNull().ToReverseXSS();
                            be.RelNameFr = reader["RelNameFr"].ToString().ToNull().ToReverseXSS();
                            be.RelNameZhcn = reader["RelNameZhcn"].ToString().ToNull().ToReverseXSS();
                            be.RelNameZhtw = reader["RelNameZhtw"].ToString().ToNull().ToReverseXSS();

                            be.ScholarKo = reader["ScholarKo"].ToString().ToNull().ToReverseXSS();
                            be.ScholarEn = reader["ScholarEn"].ToString().ToNull().ToReverseXSS();
                            be.ScholarJa = reader["ScholarJa"].ToString().ToNull().ToReverseXSS();
                            be.ScholarFr = reader["ScholarFr"].ToString().ToNull().ToReverseXSS();
                            be.ScholarZhcn = reader["ScholarZhcn"].ToString().ToNull().ToReverseXSS();
                            be.ScholarZhtw = reader["ScholarZhtw"].ToString().ToNull().ToReverseXSS();

                            be.LiveKo = reader["LiveKo"].ToString().ToNull().ToReverseXSS();
                            be.LiveEn = reader["LiveEn"].ToString().ToNull().ToReverseXSS();
                            be.LiveJa = reader["LiveJa"].ToString().ToNull().ToReverseXSS();
                            be.LiveFr = reader["LiveFr"].ToString().ToNull().ToReverseXSS();
                            be.LiveZhcn = reader["LiveZhcn"].ToString().ToNull().ToReverseXSS();
                            be.LiveZhtw = reader["LiveZhtw"].ToString().ToNull().ToReverseXSS();

                            al.Add(be);
                        }
                    }

                    return al;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}