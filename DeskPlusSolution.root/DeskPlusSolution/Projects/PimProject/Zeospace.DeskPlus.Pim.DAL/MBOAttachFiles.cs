using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class MBOAttachFiles : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        

        /// <summary>
        /// Gets the MBO attach files.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.MBOAttachFilesInfo GetMBOAttachFiles(IDataReader _reader)
        {
            return new BE.MBOAttachFilesInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["MBOID"] == DBNull.Value) ? -1 : (int)_reader["MBOID"],
                (_reader["AttachFileID"] == DBNull.Value) ? -1 : (int)_reader["AttachFileID"],
                _reader["FilePath"].ToString(),
                _reader["FileName"].ToString(),
                _reader["FileType"].ToString(),
                Convert.ToInt64(_reader["FileLength"]),
                _reader["FileGUID"].ToString()
            );
        }

        /// <summary>
        /// Gets the MBO attach files list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<MBOAttachFilesInfo> GetMBOAttachFilesList(IDataReader reader)
        {
            List<MBOAttachFilesInfo> list = new List<MBOAttachFilesInfo>();
            while (reader.Read())
                list.Add(GetMBOAttachFiles(reader));
            return list;
        }

        #region GET()

        public List<BE.MBOAttachFilesInfo> GetMBOFile(BE.MBOAttachFilesInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_MBO_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _be.UserID;
                //cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _be.StartDate;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetMBOAttachFilesList(reader);
                }

            }
        }


        public DataTableReader GetMBOFileID(int _CompanyID, int _MBOID)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_MBOAttach_Sel_ID");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region ADD()

        public void AddMBO(BE.MBOAttachFilesInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOAttach_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _info.MBOID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar,1000).Value = _info.FilePath;

                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar,500).Value = _info.FileName;
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar,5).Value = _info.FileType;
                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _info.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar,50).Value = _info.FileGUID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);

            }
        }



        
       public bool AddList(int _companyID , int _mboid, string _spoolPath)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Pims, _mboid, true,DateTime.Now.ToString("yyyyMM"));
            if (fileList != null)
            {
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.MBOAttachFilesInfo fileInfo = new MBOAttachFilesInfo();
                    fileInfo.CompanyID = _companyID;

                    fileInfo.MBOID = _mboid;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    AddMBO(fileInfo);
                    
                }
            }
            return true;
        }
     


        #endregion

        #region MOD()
        #endregion

        #region DEL()
        public void DelMBOFile(int _CompanyID, int _MBOID,int _AttachFileID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_MBOAttach_DEL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // DP_PIM_OpenUser_Ins
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@MBOID", SqlDbType.Int).Value = _MBOID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = _AttachFileID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);

            }
        }

        #endregion 
    }
}
