using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Proposal.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Proposal.DAL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AttachInfo GetAttachFile(IDataReader _reader)
        {
            return new AttachInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProposalNo = (_reader["ProposalNo"] == DBNull.Value) ? -1 : _reader["ProposalNo"].ToString().ToNumber<int>(),
                AttachID = (_reader["AttachID"] == DBNull.Value) ? -1 : _reader["AttachID"].ToString().ToNumber<int>(),
                FilePath = _reader["FilePath"].ToString().ToNull(),
                FileName = _reader["FileName"].ToString().ToNull(),
                FileType = _reader["FileType"].ToString().ToNull(),
                FileLength = (_reader["FileLength"] == DBNull.Value) ? -1 : _reader["FileLength"].ToString().ToNumber<int>(),
                FileGUID = _reader["FileGUID"].ToString().ToNull()
            };
        }

        protected List<AttachInfo> GetAttachFileList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttachFile(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachFileList(int _companyID, int _proposalNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Attach_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 선택한 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public List<BE.AttachInfo> GetAttachFileList(int _companyID, int _proposalNo, int _attachID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Attach_SelByID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _attachID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachFileList(reader);
                }
            }
        }

        #endregion

        #region ADD

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-24
        /// 작성목적 : 첨부파일 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(AttachInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_Attach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _info.ProposalNo;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _info.FilePath.ToNull();
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _info.FileName.ToNull();
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 5).Value = _info.FileType.ToNull();
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 200).Value = _info.FileGUID.ToNull();

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

        #region DEL

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-24
        /// 작성목적 : 첨부파일 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _proposalNo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_Attach_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;

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
        /// 작성자 : shine
        /// 작성일자 : 201-05-24
        /// 작성목적 : 선택한 첨부파일 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <param name="_attachID">The _attach ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _proposalNo, int _attachID)
        { 
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_Attach_DelByID", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                    cmd.Parameters.Add("@AttachID", SqlDbType.NVarChar, 1000).Value = _attachID;
                    
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
