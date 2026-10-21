using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Opinion.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Opinion.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader
        private BE.AttachInfo GetAttach(IDataReader _idr)
        {
            return new AttachInfo {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                QuesID = (_idr["QuesID"] == DBNull.Value) ? -1 : _idr["QuesID"].ToString().ToNumber<int>(),
                AttachID = (_idr["AttachID"] == DBNull.Value) ? -1 : _idr["AttachID"].ToString().ToNumber<int>(),
                FilePath = _idr["FilePath"].ToString().ToNull(),
                FileName = _idr["FileName"].ToString().ToNull(),
                FileType = _idr["FileType"].ToString().ToNull(),
                FileLength = (_idr["FileLength"] == DBNull.Value) ? -1 : (long)_idr["FileLength"],
                FileGUID = _idr["FileGUID"].ToString().ToNull()

            };
        }

        protected List<BE.AttachInfo> GetAttachList(IDataReader reader)
        {
            List<BE.AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
            {
                list.Add(GetAttach(reader));
            }
            return list;
        }
        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 파일추가
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public int Add(AttachInfo _attachInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_attachInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Attach_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _attachInfo.CompanyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _attachInfo.QuesID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _attachInfo.FilePath;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _attachInfo.FileName;
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _attachInfo.FileType;
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _attachInfo.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 50).Value = _attachInfo.FileGUID;

                    cn.Open();
                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 파일삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesID, int _attachID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Attach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                    cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;

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

        #region 가져오기

        /// <summary>
        /// 첨부파일 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public DataTableReader GetAttachList(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Attach_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 첨부파일 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _quesID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Attach_SelByQuesID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;

                cn.Open();
                return GetAttachList(ExecuteReader(cmd));
            }
        }

        /// <summary>
        /// 첨부파일정보
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_attachID"></param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttach(int _companyID, int _quesID, int _attachID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Attach_SelByAttachID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;

                cn.Open();
                return GetAttachList(ExecuteReader(cmd));
            }
        }
        #endregion
    }
}
