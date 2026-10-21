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

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    public class DocStream : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {   
        //독스트림으로 넘겨줄 파라미터 작성
        private string serverAddr = "http://localhost:9979/"; //독스트림 서버 URL
        //private string serviceId = "{3456fc425a8bce14-d44312d538823791}"; //독스트림 관리자로부터 부여받은 시스템 ID
        private string serviceId = "{aa50c9c987cf00b0-42320217047ce1f0}"; //독스트림 관리자로부터 부여받은 시스템 ID

        private string hostUrl = "http://" + HttpContext.Current.Request.Url.Authority;
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

        //파일 업로드.
        //지정된 파일경로에서 파일을 읽어들여 독스트림으로 업로드 시킨후 결과 메시지를 받는다.
        public DataSet UploadFileToDocStream(String file_name, String file_path,
                                            String service_file_id)
        {
            DataSet dsReturn = new DataSet();
            String result = "";

            try
            {
                String upload_url = serverAddr + "api/file/upload.xml";

                //파라미터 작성
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("?SERVICE_ID={0}", serviceId)); 					// 서비스ID
                sb.Append(string.Format("&FILE_NAME={0}", Uri.EscapeDataString(file_name)));// 파일명 (UTF-8 Encoding)
                if (service_file_id != null)
                {
                    sb.Append(string.Format("&SERVICE_FILE_ID={0}", Uri.EscapeDataString(service_file_id)));// 서버스 파일ID (UTF-8 Encoding)
                }

                sb.Append(string.Format("&ORG_DELETE_YN={0}", orgFileDelete)); 					// 원본파일 삭제여부

                //업로드 Uri 생성
                Uri uri = new Uri(upload_url + sb.ToString());

                //파일 오픈
                //FileStream fileStream = new FileStream("//192.168.1.20" + file_path + file_name, FileMode.Open, FileAccess.Read);
                FileStream fileStream = new FileStream(file_path + file_name, FileMode.Open, FileAccess.Read);
                fileStream.Position = 0;

                //파일전송용 헤더 작성
                String boundary = "----------" + DateTime.Now.Ticks.ToString("x");
                // Build up the post message header
                sb = new StringBuilder();
                sb.Append("--");
                sb.Append(boundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append("file");
                sb.Append("\"; filename=\"");
                sb.Append(file_name);
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: ");
                sb.Append("application/octet-stream");
                sb.Append("\r\n");
                sb.Append("\r\n");

                String postHeader = sb.ToString();
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
                byte[] endBoundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri);
                webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
                webrequest.Method = "POST";
                //webrequest.AllowWriteStreamBuffering = false;
                //webrequest.SendChunked = true;
                //webrequest.KeepAlive = true;
                webrequest.ContentLength = postHeaderBytes.Length + fileStream.Length + endBoundaryBytes.Length;

                Stream requestStream = webrequest.GetRequestStream();

                // Write out our post header
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                // Write out the file contents
                byte[] buffer = new Byte[checked((uint)Math.Min(4096, fileStream.Length))];
                int read_len = fileStream.Read(buffer, 0, buffer.Length);
                int send_len = 0;

                while (read_len != 0)
                {
                    requestStream.Write(buffer, 0, read_len);

                    send_len += read_len;

                    read_len = fileStream.Read(buffer, 0, buffer.Length);
                }

                fileStream.Close();

                // Write out the trailing boundary
                requestStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                //requestStream.Flush();
                //requestStream.Close(); //very important
                //requestStream.Dispose();

                WebResponse response = webrequest.GetResponse();
                if (webrequest.HaveResponse)
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader responseStreamReader = new StreamReader(responseStream);
                    result = responseStreamReader.ReadToEnd();

                    responseStreamReader.Close();
                    responseStream.Close();
                    response.Close();
                }

                response = null;
                webrequest = null;
                buffer = null;
                fileStream = null;
                uri = null;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);

                dsReturn.ReadXml(new XmlNodeReader(xmlDoc));
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DS_UploadFileToDocStream : " + ex.Message);
                throw;
            }

            return dsReturn;
        }


        //파일 등록.
        //네트웍드라이브로 파일이 공유되고 있을경우 독스트림서버에서 인지할수있는 네트웍드라이브 파일경로를 넘겨준다.
        //독스트림서버는 해당 경로에서 파일으로 읽어서 등록시킨다.
        //원본파일삭제 파라미터 사용시 해당위치의 공유되는 파일자체가 삭제되는데 유의한다.
        public DataSet AddFileToDocStream(String file_name, String file_path,
                                            String service_file_id)
        {
            DataSet dsReturn = new DataSet();

            String add_url = serverAddr + "api/file/add.xml";

            //파라미터 작성
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("?SERVICE_ID={0}", serviceId)); 					// 서비스ID
            sb.Append(string.Format("&FILE_NAME={0}", Uri.EscapeDataString(file_name)));// 파일명 (UTF-8 Encoding)
            //sb.Append(string.Format("&FILE_PATH={0}", Uri.EscapeDataString("http://localhost" + file_path + file_name)));// 파일경로 (UTF-8 Encoding)
            sb.Append(string.Format("&FILE_PATH={0}", Uri.EscapeDataString(file_path + file_name)));// 파일경로 (UTF-8 Encoding)

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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DS_AddFileToDocStream : " + ex.Message);
                throw;
            }

            return dsReturn;
        }

        // URL 파일 등록
        public DataSet UrlFileToDocStream(String file_name, String file_path,
                                            String service_file_id)
        {
            DataSet dsReturn = new DataSet();

            String add_url = serverAddr + "api/file/urlUpload.xml";

            //파라미터 작성
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("?SERVICE_ID={0}", serviceId)); 					// 서비스ID
            sb.Append(string.Format("&FILE_NAME={0}", Uri.EscapeDataString(file_name)));// 파일명 (UTF-8 Encoding)
            //sb.Append(string.Format("&FILE_PATH={0}", Uri.EscapeDataString("http://localhost" + file_path + file_name)));// 파일경로 (UTF-8 Encoding)
            sb.Append(string.Format("&FILE_URL={0}", Uri.EscapeDataString(hostUrl + file_path)));// 파일경로 (UTF-8 Encoding)
            
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DS_UrlFileToDocStream : " + ex.Message);
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
                using (var cn = new SqlConnection(ConnSolution(companyID)))
                {
                    DelDocStream(companyID, attachFileID, attachType);
                    var cmd = new SqlCommand("DP_DS_Attach_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = attachFileID;
                    cmd.Parameters.Add("@AttachType", SqlDbType.NVarChar, 100).Value = attachType;
                    cmd.Parameters.Add("@FileID", SqlDbType.NVarChar, 500).Value = "";

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DocStream_Add : " + errMsg);
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
                using (var cn = new SqlConnection(ConnSolution(companyID)))
                {
                    var cmd = new SqlCommand("DP_DS_Attach_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = attachFileID;
                    cmd.Parameters.Add("@AttachType", SqlDbType.NVarChar, 100).Value = attachType;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DocStream_Add : " + errMsg);
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
                using (var cn = new SqlConnection(ConnSolution(companyID)))
                {
                    var cmd = new SqlCommand("DP_DS_Attach_Sel", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = attachFileID;
                    cmd.Parameters.Add("@AttachType", SqlDbType.NVarChar, 100).Value = attachType;

                    cn.Open();
                    ds = ExecuteDataSet(cmd, "AttachFileList");
                }

                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DocStream_List : " + errMsg);
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
                //var FileID = GetDocStreamFileId(AddFileToDocStream(dr["FileGUID"].ToString(), FileHelper.GetServerMapPath(dr["FilePath"].ToString()), null));
                //var FileID = GetDocStreamFileId(UploadFileToDocStream(dr["FileGUID"].ToString(), FileHelper.GetServerMapPath(dr["FilePath"].ToString()), null));
                var FileID = GetDocStreamFileId(UrlFileToDocStream(dr["FileName"].ToString(), dr["FilePath"].ToString() + dr["FileGUID"].ToString(), null));
                
                using (var cn = new SqlConnection(ConnSolution(companyID)))
                {
                    var cmd = new SqlCommand("DP_DS_Attach_UpdateFileID", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                    cmd.Parameters.Add("@AttachFileID", SqlDbType.Int).Value = attachFileID;
                    cmd.Parameters.Add("@AttachType", SqlDbType.NVarChar, 100).Value = attachType;
                    cmd.Parameters.Add("@FileID", SqlDbType.NVarChar, 500).Value = FileID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DocStream_List : " + errMsg);
                throw;
            }
        }

    }
}
