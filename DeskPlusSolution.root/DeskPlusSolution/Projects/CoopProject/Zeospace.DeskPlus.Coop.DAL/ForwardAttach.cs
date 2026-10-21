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
    public class ForwardAttach : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ForwardAttachInfo GetForwardAttachFile(IDataReader _reader)
        {
            return new ForwardAttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ForwardListID = (_reader["ForwardListID"] == DBNull.Value) ? -1 : _reader["ForwardListID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                AttachFileID = (_reader["AttachFileID"] == DBNull.Value) ? -1 : _reader["AttachFileID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<ForwardAttachInfo> GetForwardAttachFileList(IDataReader reader)
        {
            List<ForwardAttachInfo> list = new List<ForwardAttachInfo>();
            while (reader.Read())
                list.Add(GetForwardAttachFile(reader));
            return list;
        }

        #endregion

        #region GET

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
        public List<BE.ForwardAttachInfo> GetForwardAttachFileLists(int _companyID, int _forwardListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardAttach_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetForwardAttachFileList(reader);
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
        public List<BE.ForwardAttachInfo> GetForwardAttachFileLists(int _companyID, int _forwardListID, int _attachFileID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardAttach_SelByID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;
                cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _attachFileID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetForwardAttachFileList(reader);
                }
            }
        }

        #endregion

        #region ADD

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, int _forwardListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ForwardAttach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;

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
