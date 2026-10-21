using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlus.Coop.DAL
{
    public class ItemFavUser : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        

        #endregion

        #region GET


        /// <summary>
        /// 페이지 리스트
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_orders"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _itemSubID, 
            string _pageSize, string _pageSizePageNo, string _orders)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_ItemFavUser_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Orders", SqlDbType.VarChar, 50).Value = _orders;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 페이지 카운트
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public int GetPageCount(int _companyID, int _itemSubID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COOP_ItemFavUser_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();

            }
        }


        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _itemSubID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ItemFavUser_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }



        #endregion

        #region ADD

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_userList"></param>
        /// <returns></returns>
        public bool Add(BE.ItemFavUserInfo _info, string _userList)
        {
            try
            {
                if (_userList.Length > 0)
                {
                    using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                    {
                        cn.Open();

                        foreach(string user in _userList.Split(','))
                        {
                            var cmd = new SqlCommand("DP_COOP_ItemFavUser_Ins", cn);

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                            cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = user.ToNumber<int>();
                            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

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
                throw;
            }
        }

        #endregion

        #region MOD

        /// <summary>
        /// 순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemFavList"></param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _itemFavList)
        {
            try
            {
                if (_itemFavList.Length > 0)
                {
                    string[] _itemFavListStr = _itemFavList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _itemFavListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_COOP_ItemFavUser_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemFavUserID", SqlDbType.Int).Value = _itemFavListStr[i];
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

        #region DEL

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemFavUserID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _itemFavUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ItemFavUser_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemFavUserID", SqlDbType.Int).Value = _itemFavUserID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion
    }
}
