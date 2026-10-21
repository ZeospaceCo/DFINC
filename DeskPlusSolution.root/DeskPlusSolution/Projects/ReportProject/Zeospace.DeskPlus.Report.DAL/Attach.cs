using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Report.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttach(IDataReader _reader)
        {
            return new AttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<AttachInfo> GetAttachList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttach(reader));
            return list;
        }

        #endregion



        #region 추가/수정/삭제


        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public bool Add(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_Attach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;                    
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _info.FileType.ToNull();
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 50).Value = _info.FileGUID.ToNull();

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
        /// 작성자 : 심재운
        /// 삭제
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
                    var cmd = new SqlCommand("DP_REPORT_Attach_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

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
        /// 작성자 : 심재운
        /// ID 별 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _attachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_REPORT_Attach_DelByID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 첨부 파일 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachLists(int _companyID, int _ListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_AttachFile_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachList(reader);
                }
            }
        }

        /// <summary>
        /// 심재운
        /// 목적 : ID 별 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachLists(int _companyID, int _ListID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_AttachFile_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachList(reader);
                }
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첨부파일 중에 이미지만 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachImageLists(int _companyID, int _ListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.REPORT, "QRY_REPORT_Attach_SelGetImage");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachList(reader);
                }
            }
        }
        

        #endregion
    }
}
