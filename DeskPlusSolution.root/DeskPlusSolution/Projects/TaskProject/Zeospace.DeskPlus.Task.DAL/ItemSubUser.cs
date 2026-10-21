using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlus.Task.DAL
{
    public class ItemSubUser : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 소분류에 지정되어 있는 사용자,부서 정보가져오기
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
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemSubUser_Sel");
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

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 소분류 수신 사용자 가져오기        
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public DataTableReader GetSubToUser(int _companyID, int _itemSubID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemSubToUser_Sel");
                query = string.Format(query, _itemSubID);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
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


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 소분류 참조 사용자 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public DataTableReader GetSubCcUser(int _companyID, int _itemSubID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_ItemSubCcUser_Sel");
                query = string.Format(query, _itemSubID);
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    
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

        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 소분류 수신,참조자 순서 변경하기
        public bool ItemSubUserOrder(int _companyID, string _itemSubUserList)
        {
            try
            {
                if (_itemSubUserList.Length > 0)
                {
                    string[] _itemSubListStr = _itemSubUserList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _itemSubListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_TAS_ItemSubUser_Order", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ItemSubUserID", SqlDbType.Int).Value = _itemSubListStr[i];
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

        #region ADD()
        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-29
        /// 작성목적 : 소분류 지정자 저장
        public void SubAdd(BE.ItemUser _itemUserInfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_itemUserInfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemSubUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _itemUserInfo.CompanyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemUserInfo.ItemID;
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
        #endregion

        #region DEL()
        /// 작성자 : maniaysw 
        /// 작성일자 : 2015-04-21
        /// 작성목적 :  소분류 지정자 삭제
        public bool DelItemSubUser(int _companyID, int _itemSubID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ItemSubUser_Del", cn);
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
                throw;
            }
        }
        #endregion
    }
}
