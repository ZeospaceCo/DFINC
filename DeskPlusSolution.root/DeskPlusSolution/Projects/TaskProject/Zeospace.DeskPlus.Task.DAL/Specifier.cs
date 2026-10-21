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
    public class Specifier : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region ADD()

        /// <summary>
        /// 작성자   : maniaysw
        /// 작성일자 : 2015-05-19
        /// 작성목적 : 지정자 저장
        /// </summary>
        /// <param name="_be"></param>
        public void AddSpecifier(BE.Specifier _be)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Specifier_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@SpecifierUserID", SqlDbType.Int).Value = _be.SpecifierUserID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _be.ItemSubID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _be.DeptID;
                    cmd.Parameters.Add("@SpecifierOrder", SqlDbType.Int).Value = _be.SpecifierOrder;
                    cmd.Parameters.Add("@IsGroup", SqlDbType.Char, 1).Value = _be.IsGroup;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char,2).Value = _be.IsType;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _be.WriteUserID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
                
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                
            }
        }

        #endregion

        #region DEL()
        /// <summary>
        /// 작성자   : maniaysw
        /// 작성일자 : 2015-05-20
        /// 작성목적 : 지정자 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_specifierUserID"></param>
        /// <param name="_itemSubID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public bool DelSpecifier(int _companyID, int _specifierUserID, int _itemSubID, string _isType)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Specifier_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SpecifierUserID", SqlDbType.Int).Value = _specifierUserID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _isType;

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

        #region GET
        /// <summary>
        /// 작성자   : maniaysw
        /// 작성일자 : 2015-05-20
        /// 작성목적 : 사용자별 지정자 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <param name="_specifierUserID"></param>
        /// <returns></returns>
        public DataTableReader GetSpecifier(int _companyID, int _itemSubID, int _specifierUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Specifier_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                    cmd.Parameters.Add("@SpecifierUserID", SqlDbType.Int).Value = _specifierUserID;

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
    }
}
