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
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {

        /// <summary>
        /// Gets the attach.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.AttachInfo GetAttach(IDataReader _reader)
        {
            return new BE.AttachInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["ScheduleID"] == DBNull.Value) ? -1 : (int)_reader["ScheduleID"],
                (_reader["AttachID"] == DBNull.Value) ? -1 : (int)_reader["AttachID"],
                _reader["FilePath"].ToString(),
                _reader["FileName"].ToString(),
                _reader["FileType"].ToString(),
                Convert.ToInt64(_reader["FileLength"]),
                _reader["FileGUID"].ToString()
            );
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<AttachInfo> GetAttachList(IDataReader reader)
        {
            List<AttachInfo> list = new List<AttachInfo>();
            while (reader.Read())
                list.Add(GetAttach(reader));
            return list;
        }

        #region GET() 

        public List<BE.AttachInfo> GetAttachInfo(BE.AttachInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_AttachFile_Sel");

                
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@ScheduleID ", SqlDbType.Int).Value = para.ScheduleID;

                //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetAttachList(reader);
                }
            }
        }
        

        #endregion 
 
        #region ADD() 
        public void Add(AttachInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Attach_Ins", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                    cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = _listinfo.FilePath;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = _listinfo.FileName;
                    cmd.Parameters.Add("@FileType", SqlDbType.NVarChar,5).Value = _listinfo.FileType;

                    cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = _listinfo.FileLength;
                    cmd.Parameters.Add("@FileGUID", SqlDbType.VarChar,50).Value = _listinfo.FileGUID;
                     

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
               
            }

        }


        public bool AddList(int _companyID , int _scheduleid, string _spoolPath)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Pims, _scheduleid, true, "");
            if (fileList != null)
            {
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;

                    fileInfo.ScheduleID = _scheduleid;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    Add(fileInfo);

                }
            }
            else
                return false;

            return true;
        }

        public Hashtable AddListHash(int _companyID, int _scheduleid, string _spoolPath)
        {
            Hashtable ht = new Hashtable();
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Pims, _scheduleid, true, "");
            if (fileList != null)
            {
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new AttachInfo();
                    fileInfo.CompanyID = _companyID;

                    fileInfo.ScheduleID = _scheduleid;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    Add(fileInfo);
                     
                    ht.Add(                    
                       fileInfo.FileName,
                       fileInfo.FilePath + fileInfo.FileGUID
                    );
                }
            }

            return ht;
        }



      

       
        #endregion 
 
        #region MOD() 

        #endregion 
 
        #region DEL() 

        public bool Del(AttachInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Attach_Del", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AttachID", SqlDbType.Int).Value = _listinfo.AttachID;
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
