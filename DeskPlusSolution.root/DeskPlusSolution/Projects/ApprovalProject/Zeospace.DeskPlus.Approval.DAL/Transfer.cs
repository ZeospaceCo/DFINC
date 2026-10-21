using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Transfer : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public int AddDocument(BE.SmartPlusEDM.DocumentInfo _be)
        {
            using (var cn = new SqlConnection(ConnEDMS(_be.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_DP_DOCUMENT_Document_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _be.FolderID;
                cmd.Parameters.Add("@VersionID", SqlDbType.Int).Value = _be.VersionID;
                cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = _be.ClassID;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = _be.Name;
                cmd.Parameters.Add("@Description", SqlDbType.NText).Value = _be.Description;
                cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 255).Value = _be.Keyword;
                cmd.Parameters.Add("@Version", SqlDbType.VarChar, 20).Value = _be.Version;
                cmd.Parameters.Add("@ACLID", SqlDbType.VarChar, 4000).Value = _be.ACLID;
                cmd.Parameters.Add("@LifeTimeCode", SqlDbType.Char, 6).Value = _be.LifeTimeCode;
                cmd.Parameters.Add("@LifeTimeStart", SqlDbType.DateTime).Value = _be.LifeTimeStart;
                cmd.Parameters.Add("@LifeTimeEnd", SqlDbType.DateTime).Value = _be.LifeTimeEnd;
                cmd.Parameters.Add("@ReadCount", SqlDbType.Int).Value = _be.ReadCount;
                cmd.Parameters.Add("@TotalPoint", SqlDbType.Int).Value = _be.TotalPoint;
                cmd.Parameters.Add("@Recycle", SqlDbType.Char, 1).Value = _be.Recycle;
                cmd.Parameters.Add("@RecycleUserID", SqlDbType.Int).Value = _be.RecycleUserID;
                cmd.Parameters.Add("@CheckOutDate", SqlDbType.DateTime).Value = _be.CheckOutDate;
                cmd.Parameters.Add("@CheckOutUserID", SqlDbType.Int).Value = _be.CheckOutUserID;
                cmd.Parameters.Add("@CheckOutUserInfo", SqlDbType.NVarChar, 200).Value = _be.CheckOutUserInfo;
                cmd.Parameters.Add("@CheckOutName", SqlDbType.NVarChar, 100).Value = _be.CheckOutName;
                cmd.Parameters.Add("@CheckOutDescription", SqlDbType.NText).Value = _be.CheckOutDescription;
                cmd.Parameters.Add("@CheckOutStatus", SqlDbType.Char, 1).Value = _be.CheckOutStatus;
                cmd.Parameters.Add("@CheckOutPath", SqlDbType.NVarChar, 255).Value = _be.CheckOutPath;
                cmd.Parameters.Add("@CheckInPlanDate", SqlDbType.DateTime).Value = _be.CheckInPlanDate;
                cmd.Parameters.Add("@CheckInDate", SqlDbType.DateTime).Value = _be.CheckInDate;
                cmd.Parameters.Add("@CheckInUserID", SqlDbType.Int).Value = _be.CheckInUserID;
                cmd.Parameters.Add("@CheckInUserInfo", SqlDbType.NVarChar, 200).Value = _be.CheckInUserInfo;
                cmd.Parameters.Add("@CheckInDescription", SqlDbType.NText).Value = _be.CheckInDescription;
                cmd.Parameters.Add("@BaseDocPath", SqlDbType.VarChar, 100).Value = _be.BaseDocPath;
                cmd.Parameters.Add("@HistoryEvent", SqlDbType.VarChar, 100).Value = _be.HistoryEvent;
                cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _be.WriteDate;
                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _be.WriteUserID;
                cmd.Parameters.Add("@WriteUserInfo", SqlDbType.NVarChar, 200).Value = _be.WriteUserInfo;
                cmd.Parameters.Add("@WriteName", SqlDbType.NVarChar, 200).Value = _be.WriteName;
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _be.ModifyDate;
                cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _be.ModifyUserID;
                cmd.Parameters.Add("@ModifyUserInfo", SqlDbType.NVarChar, 200).Value = _be.ModifyUserInfo;
                cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _be.OwnerID;
                cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 500).Value = _be.Etc1;
                cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 500).Value = _be.Etc2;
                cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 500).Value = _be.Etc3;
                cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 500).Value = _be.Etc4;
                cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 500).Value = _be.Etc5;
                cmd.Parameters.Add("@Etc6", SqlDbType.NText).Value = _be.Etc6;
                cmd.Parameters.Add("@Etc7", SqlDbType.NText).Value = _be.Etc7;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        public int AddVersion(BE.SmartPlusEDM.VersionInfo _be)
        {
            using (var cn = new SqlConnection(ConnEDMS(_be.CompanyID)))
            {
                SqlCommand cmd = new SqlCommand("SQL_DP_DOCUMENT_Version_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _be.DocumentID;
                cmd.Parameters.Add("@VersionID", SqlDbType.Int).Value = _be.VersionID;
                cmd.Parameters.Add("@Version", SqlDbType.VarChar, 20).Value = _be.Version;
                cmd.Parameters.Add("@VersionType", SqlDbType.Char, 1).Value = _be.VersionType;
                cmd.Parameters.Add("@VersionDescription", SqlDbType.NText).Value = _be.VersionDescription;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 255).Value = _be.Name;
                cmd.Parameters.Add("@Description", SqlDbType.NText).Value = _be.Description;
                cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 255).Value = _be.Keyword;
                cmd.Parameters.Add("@LifeTimeCode", SqlDbType.Char, 6).Value = _be.LifeTimeCode;
                cmd.Parameters.Add("@LifeTimeStart", SqlDbType.DateTime).Value = _be.LifeTimeStart;
                cmd.Parameters.Add("@LifeTimeEnd", SqlDbType.DateTime).Value = _be.LifeTimeEnd;
                cmd.Parameters.Add("@HistoryEvent", SqlDbType.VarChar, 50).Value = _be.HistoryEvent;
                cmd.Parameters.Add("@CheckOutDate", SqlDbType.DateTime).Value = _be.CheckOutDate;
                cmd.Parameters.Add("@CheckOutUserID", SqlDbType.Int).Value = _be.CheckOutUserID;
                cmd.Parameters.Add("@CheckOutUserInfo", SqlDbType.NVarChar, 200).Value = _be.CheckOutUserInfo;
                cmd.Parameters.Add("@CheckOutName", SqlDbType.NVarChar, 100).Value = _be.CheckOutName;
                cmd.Parameters.Add("@CheckOutDescription", SqlDbType.NText).Value = _be.CheckOutDescription;
                cmd.Parameters.Add("@CheckOutStatus", SqlDbType.Char, 1).Value = _be.CheckOutStatus;
                cmd.Parameters.Add("@CheckOutPath", SqlDbType.NVarChar, 255).Value = _be.CheckOutPath;
                cmd.Parameters.Add("@CheckInPlanDate", SqlDbType.DateTime).Value = _be.CheckInPlanDate;
                cmd.Parameters.Add("@CheckInDate", SqlDbType.DateTime).Value = _be.CheckInDate;
                cmd.Parameters.Add("@CheckInUserID", SqlDbType.Int).Value = _be.CheckInUserID;
                cmd.Parameters.Add("@CheckInUserInfo", SqlDbType.NVarChar, 200).Value = _be.CheckInUserInfo;
                cmd.Parameters.Add("@CheckInDescription", SqlDbType.NText).Value = _be.CheckInDescription;
                cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _be.WriteDate;
                cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _be.WriteUserID;
                cmd.Parameters.Add("@WriteUserInfo", SqlDbType.NVarChar, 200).Value = _be.WriteUserInfo;
                cn.Open();

                try
                {
                    ExecuteNonQuery(cmd);
                }
                catch
                {
                    return -1;
                }
                return 0;
            }
        }

        public int AddCoworker(BE.SmartPlusEDM.CoworkerInfo _be)
        {
            using (var cn = new SqlConnection(ConnEDMS(_be.CompanyID)))
            {
                SqlCommand cmd = new SqlCommand("SQL_DP_DOCUMENT_Coworker_Insert", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _be.DocumentID;
                cmd.Parameters.Add("@VersionID", SqlDbType.Int).Value = _be.VersionID;
                cmd.Parameters.Add("@OwnerType", SqlDbType.Char, 1).Value = _be.OwnerType;
                cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _be.OwnerID;
                cmd.Parameters.Add("@Rating", SqlDbType.NVarChar, 5).Value = _be.Rating;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        public int AddFile(List<BE.SmartPlusEDM.FileInfo> _al)
        {
            using (var cn = new SqlConnection(ConnEDMS(_al[0].CompanyID)))
            {
                int result = 0;
                foreach (BE.SmartPlusEDM.FileInfo _be in _al)
                {
                    SqlCommand cmd = new SqlCommand("SQL_DP_DOCUMENT_File_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _be.DocumentID;
                    cmd.Parameters.Add("@VersionID", SqlDbType.Int).Value = _be.VersionID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 255).Value = _be.FilePath;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = _be.FileName;
                    cmd.Parameters.Add("@FileType", SqlDbType.Char, 4).Value = _be.FileType;
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _be.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar, 40).Value = _be.FileGUID;
                    cmd.Parameters.Add("@CheckOutStatus", SqlDbType.Char, 1).Value = _be.CheckOutStatus;
                    cn.Open();

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    cn.Close();

                    if (result < 1)
                    {
                        return -1;
                    }
                }

                return result;
            }
        }

        public int SearchProcess(int _companyID, string _docType, int _documentID,
            int _userID, string _gubun)
        {
            //EXEC SQL_DP_DOCUMENT_Search_InsertDelete 1,'D',3862,0,'C' 일반 문서 (추가,수정)
            //EXEC SQL_DP_DOCUMENT_Search_InsertDelete 1,'D',3862,0,'D' 일반 문서 (삭제)
            //EXEC SQL_DP_DOCUMENT_Search_InsertDelete 1,'P',3862,1,'C' 수신 문서 (추가,수정)
            //EXEC SQL_DP_DOCUMENT_Search_InsertDelete 1,'P',3862,1,'D' 수신 문서 (삭제)
            using (var cn = new SqlConnection(ConnEDMS(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("SQL_DP_DOCUMENT_Search_InsertDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DocType", SqlDbType.Char, 1).Value = _docType;
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                cn.Open();

                try
                {
                    ExecuteNonQuery(cmd);
                }
                catch
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}