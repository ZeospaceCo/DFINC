using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Web;


namespace Zeospace.DeskPlusMobile.Web.Helper.WebComponent
{
    public class DocStream
    {
        //독스트림으로 넘겨줄 파라미터 작성
        private string serverAddr = "http://localhost:9979/"; //독스트림 서버 URL
        private string serviceId = "{aa50c9c987cf00b0-42320217047ce1f0}"; //독스트림 관리자로부터 부여받은 시스템 ID

        //private string hostUrl = "http://" + HttpContext.Current.Request.Url.Authority;
        private string hostUrl = "http://localhost";
        private bool orgFileDelete = false;

        // 업로드 결과에서 리턴코드 조회    
        public String GetResCode(DataSet dataset)
        {
            return dataset.Tables[0].Rows[0]["resCode"].ToString();
        }

        // 업로드 결과에서 리턴메시지(에러메시지) 조회    
        public String GetResMessage(DataSet dataset)
        {
            return dataset.Tables[0].Rows[0]["resMessage"].ToString();
        }

        // 업로드 결과에서 독스트림에서 부여받은 FileId 조회    
        public String GetDocStreamFileId(DataSet dataset)
        {
            if (dataset.Tables["data"].Rows.Count > 0)
            {
                return dataset.Tables["data"].Rows[0]["FILE_ID"].ToString();
            }
            else
            {
                return "";
            }
        }

        // URL 파일 등록
        public DataSet UrlFileToDocStream(String file_name, String file_path, String service_file_id)
        {
            DataSet dsReturn = new DataSet();

            String add_url = serverAddr + "api/file/urlUpload.xml";

            //파라미터 작성
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("?SERVICE_ID={0}", serviceId)); 					// 서비스ID
            sb.Append(string.Format("&FILE_NAME={0}", Uri.EscapeDataString(file_name)));// 파일명 (UTF-8 Encoding)
            sb.Append(string.Format("&FILE_URL={0}", Uri.EscapeDataString(hostUrl + file_path)));// 파일경로 (UTF-8 Encoding)\
            sb.Append(string.Format("&ORG_DELETE_YN={0}", orgFileDelete)); 					// 원본파일 삭제여부
            if (service_file_id != null)
            {
                sb.Append(string.Format("&SERVICE_FILE_ID={0}", Uri.EscapeDataString(service_file_id)));// 서버스 파일ID (UTF-8 Encoding)
            }

            try
            {
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
                byte[] byte1 = encoding.GetBytes(sb.ToString());
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(add_url);
                webrequest.UseDefaultCredentials = true;
                webrequest.Method = "POST";
                webrequest.ContentType = "application/x-www-form-urlencoded";
                //webrequest.ContentType = "text/xml";
                webrequest.ContentLength = byte1.Length;
                //
                Stream requestStream = webrequest.GetRequestStream();
                requestStream.Write(byte1, 0, byte1.Length);
                requestStream.Close();
                //
                HttpWebResponse response = (HttpWebResponse)webrequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                //StreamReader responseStreamReader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                StreamReader responseStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                String result = responseStreamReader.ReadToEnd();

                response = null;
                webrequest = null;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                dsReturn.ReadXml(new XmlNodeReader(xmlDoc));
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DS_UrlFileToDocStream : " + ex.Message);
                throw;
            }

            return dsReturn;
        }

        /// <summary>
        /// 작성자 : 이중호
        /// 추가
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public bool AddDocStream(int companyID, int attachFileID, string attachType)
        {
            try
            {
                DelDocStream(companyID, attachFileID, attachType);
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@AttachFileID", SqlDbType.Int)  
                                               ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)  
                                               ,new SqlParameter("@FileID", SqlDbType.NVarChar, 500)
                };

                parms[0].Value = companyID;
                parms[1].Value = attachFileID;
                parms[2].Value = attachType;
                parms[3].Value = "";

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_DS_Attach_Ins", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DocStream_Add : " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이중호
        /// 추가
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public bool DelDocStream(int companyID, int attachFileID, string attachType)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@AttachFileID", SqlDbType.Int)  
                                               ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)
                };

                parms[0].Value = companyID;
                parms[1].Value = attachFileID;
                parms[2].Value = attachType;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_DS_Attach_Del", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DocStream_Del : " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이중호
        /// 파일 선택
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public DataTable DSFileInfo(int companyID, int attachFileID, string attachType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@AttachFileID", SqlDbType.Int)  
                                               ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)
                };

                parms[0].Value = companyID;
                parms[1].Value = attachFileID;
                parms[2].Value = attachType;

                ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_DS_Attach_Sel", "AttachFileList", parms);

                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                DeskPlusMobile.Web.Helper.AppException.WriteException("DocStream_List : " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이중호
        /// FileID 업데이트
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public bool UpdateFileID(int companyID, int attachFileID, string attachType)
        {
            try
            {
                DataRow dr = DSFileInfo(companyID, attachFileID, attachType).Rows[0];
                string FilePath = dr["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/");
                var fileID = GetDocStreamFileId(UrlFileToDocStream(dr["FileName"].ToString(), FilePath + dr["FileGUID"].ToString(), null));

                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@AttachFileID", SqlDbType.Int)  
                                               ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)
                                               ,new SqlParameter("@FileID", SqlDbType.NVarChar, 500)
                };

                parms[0].Value = companyID;
                parms[1].Value = attachFileID;
                parms[2].Value = attachType;
                parms[3].Value = fileID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_DS_Attach_UpdateFileID", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlusMobile.Web.Helper.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DocStream_UpdateFileID : " + errMsg);
                throw;
            }
        }
    }
}