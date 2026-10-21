using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class ItemSub : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ItemSubInfo GetItemSub(IDataReader _reader)
        {
            return new ItemSubInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ItemSubID = (_reader["ItemSubID"] == DBNull.Value) ? -1 : _reader["ItemSubID"].ToString().ToNumber<int>(),
                ItemID = (_reader["ItemID"] == DBNull.Value) ? -1 : _reader["ItemID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),                
                IsActive = _reader["IsActive"].ToString().ToNull(),
                ItemOrder = (_reader["ItemOrder"] == DBNull.Value) ? -1 : _reader["ItemOrder"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                TempCnt = (_reader["TempCnt"] == DBNull.Value) ? -1 : _reader["TempCnt"].ToString().ToNumber<int>()
            };
        }

        protected List<ItemSubInfo> GetItemSubList(IDataReader reader)
        {
            List<ItemSubInfo> list = new List<ItemSubInfo>();
            while (reader.Read())
                list.Add(GetItemSub(reader));
            return list;
        }

        #endregion
            
        #region GET

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-08
        /// 작성목적 : 소분류 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.ItemSubInfo> GetItemSub(int _companyID, int _itemID, bool _isActive)
        {
            string isActive = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemSub_Sel");
                query = string.Format(query, isActive);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemSubList(reader);
                }
            }
        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 소분류ID 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.ItemSubInfo> GetItemSubID(int _companyID, int _itemID, int _itemSubID, bool _isActive)
        {
            string isActive = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemSubID_Sel");
                query = string.Format(query, isActive);
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemSubList(reader);
                }
            }
        }


        #endregion
        
        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 소분류 저장하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddItemSub(ItemSubInfo _info)
        {

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemSub_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _info.IsActive;
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
        #endregion

        #region MOD

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 소분류 아이템 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModItemSub(ItemSubInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemSub_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _info.IsActive;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;

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
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 소분류 아이템 순서 변경
        public bool ItemSubUpdown(int _companyID, string _itemSubList)
        {
            try
            {
                if (_itemSubList.Length > 0)
                {
                    string[] _itemSubListStr = _itemSubList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _itemSubListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_TAS_ItemSub_Order", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubListStr[i];
                            cmd.Parameters.Add("@ItemOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #endregion

        #region DEL()
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-30
        /// 작성목적 : 소분류 아이템 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public bool DelItemSub(int _companyID, int _itemSubID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemSub_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
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
        #endregion
    }
}
