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
    public class ListViewer : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListViewerInfo GetListViewer(IDataReader _reader)
        {
            return new ListViewerInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                IsType = _reader["IsType"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),                
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),   
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
              
            };
        }

        protected List<ListViewerInfo> GetListViewerList(IDataReader reader)
        {
            List<ListViewerInfo> list = new List<ListViewerInfo>();
            while (reader.Read())
                list.Add(GetListViewer(reader));
            return list;
        }

        #endregion

        #region GET
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 사용자 리스트 가져오기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetPageReadList(int _companyID, int _listID, int _pageSize, int pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {                
                var cmd = new SqlCommand("DP_TAS_ListViewer_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = pageSizePageNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 사용자 리스트 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _listID)
        {
                     
           using (var cn = new SqlConnection(ConnSolution(_companyID)))
           {
               var cmd = new SqlCommand("DP_TAS_ListViewer_SelCnt", cn);
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
               cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;               

               cn.Open();
               return Convert.ToInt32(ExecuteScalar(cmd));

           }
          
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-13
        /// 작성목적 : 전달자 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public int GetForwardListCount(int _companyID, int _listID)
        {            
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_ListViewer_SelForwardCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

                cn.Open();
                return Convert.ToInt32(ExecuteScalar(cmd));

            }

        }


        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-13
        /// 작성목적 : 전달자 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        public DataTableReader GetForwardList(int _companyID, int _listID, int _pageSize, int pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_ListViewer_SelPageForwardList", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = pageSizePageNo;

                cn.Open();
                return ExecuteDataTableReader(cmd);

            }

        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-15
        /// 작성목적 : 조회 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(ListViewerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_ListViewer_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@IsType", SqlDbType.Char, 2).Value = _info.IsType;

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
