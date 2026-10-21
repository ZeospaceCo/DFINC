using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.DAL
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
                ActiveGroup = _reader["ActiveGroup"].ToString().ToNull(),
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

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 대분류 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddItem(ItemInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_Item_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@ActiveGroup", SqlDbType.Char, 1).Value = _info.ActiveGroup;
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
        /// 작성자   : shine
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 대분류 수정
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ModItem(ItemInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_Item_Upd", cn);

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
                    cmd.Parameters.Add("@ActiveGroup", SqlDbType.Char, 1).Value = _info.ActiveGroup;
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
        /// 작성자   : shine
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 대분류 삭제
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <returns></returns>
        public bool DelItem(int _companyID, int _itemID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_Item_Del", cn);

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

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 대분류 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <returns></returns>
        public bool ModOrderItem(int _companyID, string _itemList)
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
                            var cmd = new SqlCommand("DP_Coop_Item_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemListStr[i];
                            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = (i + 1);

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

        #region 가져오기

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 1단계메뉴가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeGroup">if set to <c>true</c> [_active group].</param>
        /// <returns></returns>
        public List<BE.ItemInfo> GetItem(int _companyID, bool _activeGroup)
        {
            string activeGroup = (_activeGroup) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_Item_Sel");
                query = string.Format(query, activeGroup);

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
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 1단계메뉴가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <param name="_activeGroup">if set to <c>true</c> [_active group].</param>
        /// <returns></returns>
        public List<BE.ItemInfo> GetItem(int _companyID, int _itemID, bool _activeGroup)
        {
            string activeGroup = (_activeGroup) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_Item_SelByID");
                query = string.Format(query, activeGroup);

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
    }
}
