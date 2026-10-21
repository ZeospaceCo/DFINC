using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.DAL
{
    public class BaseCoop : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {

        /// <summary>
        /// 협업 근거문서 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <returns></returns>
        public DataTableReader GetBaseCoop(int _companyID, int _listID, int _groupID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_BaseCoop_Sel");
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _groupID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.BaseCoopInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_BaseCoop_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@BaseCoopID", SqlDbType.Int).Value = _info.BaseCoopID;
                    cmd.Parameters.Add("@BaseOrder", SqlDbType.Int).Value = _info.BaseOrder;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
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
        /// 삭제
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Del(BE.BaseCoopInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_BaseCoop_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@BaseCoopID", SqlDbType.Int).Value = _info.BaseCoopID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
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
        /// 삭제
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_BaseCoop_DelByListID", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }
}
