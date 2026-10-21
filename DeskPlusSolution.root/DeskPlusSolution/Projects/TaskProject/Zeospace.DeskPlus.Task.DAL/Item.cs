using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Task.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class Item : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ItemInfo GetItem(IDataReader _reader)
        {
            return new ItemInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
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
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<ItemInfo> GetItemList(IDataReader reader)
        {
            List<ItemInfo> list = new List<ItemInfo>();
            while (reader.Read())
                list.Add(GetItem(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-07
        /// 작성목적 : 본인이 보고자 하는 대분류메뉴리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public List<BE.ItemInfo> GetItem(int _companyID, int _userID, string _isType)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Item_SelMenu");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@IsType ", SqlDbType.Char, 2).Value = _isType;   
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-08
        /// 작성목적 : 대분류 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public List<BE.ItemInfo> GetItem(int _companyID, bool _isActive)
        {
            string isActive = (_isActive) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Item_Sel");
                query = string.Format(query, isActive);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-13
        /// 작성목적 : 선택한 대분류 정보가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public List<BE.ItemInfo> GetItemID(int _companyID, int _itemID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemID_Sel");
                query = string.Format(query, _itemID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemList(reader);
                }
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-13
        /// 대분류 아이템 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddItem(ItemInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Item_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
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

        #region MOD()

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-13
        /// 대분류 아이템 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModItem(ItemInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Item_Upd", cn);

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

        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 대분류 변경하기
        public bool ItemOrder(int _companyID, string _itemList)
        {
            try
            {
                if (_itemList.Length > 0)
                {
                    string[] _itemListStr = _itemList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _itemListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_TAS_Item_Order", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemListStr[i];
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
        /// 작성일자 : 2015-04-24
        /// 대분류 아이템 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemID"></param>
        /// <returns></returns>
        public bool DelItem(int _companyID, int _itemID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Item_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;


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
