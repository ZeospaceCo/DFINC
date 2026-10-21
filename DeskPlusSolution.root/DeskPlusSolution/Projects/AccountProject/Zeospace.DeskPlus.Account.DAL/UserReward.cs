using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserReward : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@RewardID", SqlDbType.Int).Value = _info.RewardID;
        cmd.Parameters.Add("@RewardDate", SqlDbType.VarChar,8).Value = _info.RewardDate.ToNull().ToXSS();
        cmd.Parameters.Add("@Gubun", SqlDbType.VarChar,50).Value = _info.Gubun.ToNull().ToXSS();
        cmd.Parameters.Add("@RewardType", SqlDbType.NVarChar,100).Value = _info.RewardType.ToNull().ToXSS();
        cmd.Parameters.Add("@Contents", SqlDbType.NVarChar,200).Value = _info.Contents.ToNull().ToXSS();
        cmd.Parameters.Add("@Rewarder", SqlDbType.NVarChar,100).Value = _info.Rewarder.ToNull().ToXSS();
        cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = _info.Orders;
        cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
        cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
        cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
        cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;
        */

        #region DataReader

        private UserRewardInfo GetUserReward(IDataReader _reader)
        {
            return new UserRewardInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                (_reader["RewardID"] == DBNull.Value) ? -1 : (int)_reader["RewardID"],
                (_reader["RewardDate"] == DBNull.Value) ? "" : _reader["RewardDate"].ToString(),
                _reader["Gubun"].ToString().ToReverseXSS(),
                _reader["RewardType"].ToString().ToReverseXSS(),
                _reader["Contents"].ToString().ToReverseXSS(),
                _reader["Rewarder"].ToString().ToReverseXSS(),
                (_reader["Orders"] == DBNull.Value) ? -1 : (int)_reader["Orders"],
                (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
                (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
                (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
                );
        }

        protected List<UserRewardInfo> GetUserRewardList(IDataReader reader)
        {
            List<UserRewardInfo> list = new List<UserRewardInfo>();
            while (reader.Read())
                list.Add(GetUserReward(reader));

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
        public bool Add(BE.UserRewardInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserReward_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@RewardDate", SqlDbType.VarChar, 8).Value = _info.RewardDate.ToNull().ToXSS();
                    cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 50).Value = _info.Gubun.ToNull().ToXSS();
                    cmd.Parameters.Add("@RewardType", SqlDbType.VarChar, 500).Value = (_info.RewardType.ToNull() != "") ? AESManager.Encrypt(_info.RewardType.ToNull().ToXSS()) : "";
                    cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 200).Value = _info.Contents.ToNull().ToXSS();
                    cmd.Parameters.Add("@Rewarder", SqlDbType.VarChar, 500).Value = (_info.Rewarder.ToNull() != "") ? AESManager.Encrypt(_info.Rewarder.ToNull().ToXSS()) : "";
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
        public bool Del(int _companyID, int _rewardID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserReward_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@RewardID", SqlDbType.Int).Value = _rewardID;

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
        /// 상벌 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public List<BE.UserRewardInfo> GetUserRewardInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserReward_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                List<UserRewardInfo> al = new List<UserRewardInfo>();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BE.UserRewardInfo be = new UserRewardInfo();

                        be.CompanyID = reader["CompanyID"].ToString().ToNumber<int>();
                        be.UserID = reader["UserID"].ToString().ToNull().ToNumber<int>();
                        be.RewardID = reader["RewardID"].ToString().ToNull().ToNumber<int>();
                        be.RewardDate = reader["RewardDate"].ToString().ToNull().ToReverseXSS();
                        be.Gubun = reader["Gubun"].ToString().ToNull().ToReverseXSS();
                        be.GubunKo = reader["GubunKo"].ToString().ToNull().ToReverseXSS();
                        be.GubunEn = reader["GubunEn"].ToString().ToNull().ToReverseXSS();
                        be.GubunJa = reader["GubunJa"].ToString().ToNull().ToReverseXSS();
                        be.GubunFr = reader["GubunFr"].ToString().ToNull().ToReverseXSS();
                        be.GubunZhcn = reader["GubunZhcn"].ToString().ToNull().ToReverseXSS();
                        be.GubunZhtw = reader["GubunZhtw"].ToString().ToNull().ToReverseXSS();
                        be.RewardType = (reader["RewardType"].ToString().ToNull() != "") ? AESManager.Decrypt(reader["RewardType"].ToString()).ToNull().ToReverseXSS() : "";
                        be.Contents = reader["Contents"].ToString().ToNull().ToReverseXSS();
                        be.Rewarder = (reader["Rewarder"].ToString().ToNull() != "") ? AESManager.Decrypt(reader["Rewarder"].ToString()).ToNull().ToReverseXSS() : "";
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