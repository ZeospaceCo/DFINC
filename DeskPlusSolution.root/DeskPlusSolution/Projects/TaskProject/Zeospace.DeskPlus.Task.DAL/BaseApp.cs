using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class BaseApp : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BaseAppInfo GetListBase(IDataReader _reader)
        {
            return new BaseAppInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                BaseAppID = (_reader["BaseAppID"] == DBNull.Value) ? -1 : _reader["BaseAppID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),                
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),                
                Name = _reader["Name"].ToString().ToNull(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                Path = _reader["Path"].ToString().ToNull(),
                BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>()              
            };
        }

        protected List<BaseAppInfo> GetListBaseList(IDataReader reader)
        {
            List<BaseAppInfo> list = new List<BaseAppInfo>();
            while (reader.Read())
                list.Add(GetListBase(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-15
        /// 작성목적 : 전자결재 근거문서 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetBaseApp(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_BaseApp_Sel");
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-15
        /// 작성목적 : 전자결재 근거문서 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BaseAppInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_BaseApp_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;                    
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _info.Name.ToNull();
                    cmd.Parameters.Add("@IsLegacy", SqlDbType.Char, 1).Value = _info.IsLegacy.ToNull();
                    cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 500).Value = _info.Path.ToNull();
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
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 임시저장 전자결재 근거문서 추가하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, int _draftID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_BaseApp_InsDraft", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;

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

        #region DEL()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 전자결재 근거문서 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_BaseApp_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
