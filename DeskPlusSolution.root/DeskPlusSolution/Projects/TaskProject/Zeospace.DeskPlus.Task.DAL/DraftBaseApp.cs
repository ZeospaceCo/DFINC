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
    public class DraftBaseApp : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private DraftBaseAppInfo GetDraftBase(IDataReader _reader)
        {
            return new DraftBaseAppInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                BaseAppID = (_reader["BaseAppID"] == DBNull.Value) ? -1 : _reader["BaseAppID"].ToString().ToNumber<int>(),
                DraftID = (_reader["DraftID"] == DBNull.Value) ? -1 : _reader["DraftID"].ToString().ToNumber<int>(),                            
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),                
                Name = _reader["Name"].ToString().ToNull(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                Path = _reader["Path"].ToString().ToNull(),
                BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>()              
            };
        }

        protected List<DraftBaseAppInfo> GetListBaseList(IDataReader reader)
        {
            List<DraftBaseAppInfo> list = new List<DraftBaseAppInfo>();
            while (reader.Read())
                list.Add(GetDraftBase(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 전자결재 근거문서 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public DataTableReader GetBaseApp(int _companyID, string _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_DraftBaseApp_Sel");
                query = string.Format(query, _draftID);

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
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 전자결재 근거문서 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(DraftBaseAppInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftBaseApp_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;                    
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

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 전자결재 근거문서 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftBaseApp_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
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
