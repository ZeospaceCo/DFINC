using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class ItemUser : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21 
        /// 작성목적 : 대분류 지정자 저장
        public void Add(BE.ItemUser _itemUserInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_itemUserInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _itemUserInfo.CompanyID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemUserInfo.ItemID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _itemUserInfo.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _itemUserInfo.DeptID;
                    cmd.Parameters.Add("@IsGroup", SqlDbType.Char, 1).Value = _itemUserInfo.IsGroup;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _itemUserInfo.IsType;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.NVarChar, 50).Value = _itemUserInfo.WriteUserID;
                    
                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;               
            }
        }


        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 대분류 지정자 가져오기
        public DataTableReader GetItemUser(int _companyID, int _itemID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemUser_Sel");
                query = string.Format(query, _itemID);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
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


        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 :  대분류 지정자 삭제
        public bool DelItemUser(int _companyID, int _itemID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemUser_Del", cn);
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
                throw;
            }
        }


        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 대분류 수신자 순서 가져오기
        public DataTableReader ItemToUserSel(int _companyID, int _itemID)
        {
            try
            {

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemToUser_Sel");
                    query = string.Format(query, _itemID);

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                        cn.Open();

                        return ExecuteDataTableReader(cmd);
                    }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw; ;
            }
        }

        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 대분류 참조자 순서 가져오기
        public DataTableReader ItemCcUserSel(int _companyID, int _itemID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemCcUser_Sel");
                query = string.Format(query, _itemID);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw; ;
            }
        }



        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 대분류 수신,참조자 순서 변경하기
        public bool ItemUserOrder(int _companyID, string _itemList)
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
                            var cmd = new SqlCommand("DP_TAS_ItemUser_Order", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemUserID", SqlDbType.Int).Value = _itemListStr[i];
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

    }
}

