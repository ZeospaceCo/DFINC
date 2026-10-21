using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.MyTalk.Ws
{
    /// <summary>
    /// SetMyTalk의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetMyTalk : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글 등록
        /// 작성일자 : 2012-11-06
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMyTalk(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.MyTalk.Ws.SetMyTalk).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            SetDecryptString(p);
            string rUserID = GetDecryptParam("RUserID", "0");
            string content = GetDecryptParam("Content", "");
            string isSecurity = GetDecryptParam("IsSecurity", "0");
            string groupID = GetDecryptParam("GroupID", "0");
            string fileYN = GetDecryptParam("FileYN", "N");
            string swfUploadGUID = GetDecryptParam("SwfUploadGUID", "N");

            try
            {
                if (CompanyID <= 0 || UserID <= 0) return false;

                BE.MyTalkInfo info = new BE.MyTalkInfo();

                info.CompanyID = CompanyID;
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.UserID = UserID;
                info.UserName = UserInfo.Name;
                info.RUserID = rUserID.ToNumber<int>();
                info.JobDate = "";
                info.IsSecurity = isSecurity;
                info.GroupID = groupID.ToNumber<int>();

                int rGroupID = AddMyTalk(info);

                if (rGroupID > 0)
                {
                    if (fileYN == "Y")
                    {
                        if (!AddFile(rGroupID, swfUploadGUID))
                        {
                            return false;
                        }

                        if (rGroupID > 0)
                        {

                            #region DOC_STREAM
                            var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "DocStreamYN");

                            if (docStreamYN == "Y")
                            {
                                DocStream ds = new DocStream();
                                try
                                {
                                    string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Attach");
                                    sqlQuery1 = String.Format(sqlQuery1, CompanyID, rGroupID);

                                    DataSet dsAttach = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery1, "dsMyTalkAttach", null);
                                    var newDSList = dsAttach.Tables[0].Rows;
                                    for (int i = 0; i < newDSList.Count; i++)
                                    {
                                        ds.AddDocStream(CompanyID, Convert.ToInt32(newDSList[i]["AttachID"].ToString()), "SNS_MYTALK");
                                        ds.UpdateFileID(CompanyID, Convert.ToInt32(newDSList[i]["AttachID"].ToString()), "SNS_MYTALK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Attach");
                                    sqlQuery1 = String.Format(sqlQuery1, CompanyID, rGroupID);

                                    DataSet dsAttach = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery1, "dsMyTalkAttach", null);
                                    var DSList = dsAttach.Tables[0].Rows;
                                    for (int i = 0; i < DSList.Count; i++)
                                    {
                                        ds.DelDocStream(CompanyID, Convert.ToInt32(DSList[i]["AttachID"].ToString()), "SNS_MYTALK");
                                    }
                                    AppException.WriteException("SNS_MYTALK_Addition_DS : " + ex.Message);
                                }

                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    return false;
                }

                // 알람발송 - 다른 사람한테 댓글을 추가했을 경우
                if (info.RUserID != UserID)
                {
                    string subject = "[" + resMng.GetString("Comment") + "]" + UserInfo.DeptName + " " + UserInfo.Name + resMng.GetString("NoteAlarm");

                    SendAlarmUser(info, rUserID.ToNumber<int>(), subject, 1);

                    SendAlarmUser(info, rUserID.ToNumber<int>(), subject, 4);
                }

                return true;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddMyTalk", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일추가
        /// 작성일자 : 2015-03-04
        /// </summary>
        /// <param name="_groupID"></param>
        /// <param name="_swfUploadGUID"></param>
        /// <returns></returns>
        public bool AddFile(int _groupID, string _swfUploadGUID)
        {
            string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + _swfUploadGUID + "/");
            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
            string targetPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/MyTalk/" + yearMonth + "/" + _groupID + "/");
            string targetWebPath = "/DeskPlusEIP/FileData/" + CompanyID + "/MyTalk/" + yearMonth + "/" + _groupID + "/";

            if (Directory.Exists(uploadPath))
            {
                if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

                DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                string sourceFile = "";
                string destinationFile = "";
                string fileGUID = "";
                int cnt = 1;

                foreach (var item in files)
                {
                    if (item.Name != "Thumbs.db")
                    {
                        sourceFile = uploadPath + item.Name;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);

                        //파일명은 GUID 이며, 확장자는 원본과 동일하다.(.ZEO 사용안함)
                        fileGUID = System.Guid.NewGuid().ToString() + sourceFileInfo.Extension;
                        if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                        destinationFile = targetPath + fileGUID;

                        File.Copy(sourceFile, destinationFile, true);

                        System.Drawing.Image image = System.Drawing.Image.FromFile(destinationFile);

                        int convWidth = 500;
                        int convHeight = Convert.ToInt32((image.Height * convWidth) / image.Width);

                        System.Drawing.Image thumb = image.GetThumbnailImage(convWidth, convHeight,
                            delegate() { return false; }, (IntPtr)0);

                        //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                        thumb.Save(targetPath + "Thum_" + fileGUID);
                        image.Dispose();

                        BE.AttachInfo info = new BE.AttachInfo();
                        info.CompanyID = CompanyID;
                        info.GroupID = _groupID;
                        info.FilePath = targetWebPath;
                        info.FileName = sourceFileInfo.Name;
                        info.FileType = sourceFileInfo.Extension;
                        info.FileLength = sourceFileInfo.Length;
                        info.FileGUID = fileGUID;
                        info.FileOrder = cnt;
                        AddFile(info);

                        cnt++;
                    }
                }

                /*
                string[] fileArr = Directory.GetFiles(uploadPath);

                string fileName = "";
                string tmpFile = "";
                string sourceFile = "";
                string destinationFile = "";
                string fileGUID = "";
                int tmpfileIdx = -1;

                for (int i = 0; i < fileArr.Length; i++)
                {
                    tmpFile = fileArr[i];
                    tmpfileIdx = tmpFile.LastIndexOf("\\");
                    fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                    sourceFile = uploadPath + fileName;
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);

                    //파일명은 GUID 이며, 확장자는 원본과 동일하다.(.ZEO 사용안함)
                    fileGUID = System.Guid.NewGuid().ToString() + sourceFileInfo.Extension;                    
                    if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                    destinationFile = targetPath + fileGUID;

                    File.Copy(sourceFile, destinationFile, true);

                    System.Drawing.Image image = System.Drawing.Image.FromFile(destinationFile);

                    System.Drawing.Image thumb = image.GetThumbnailImage(300, 300,
                        delegate() { return false; }, (IntPtr)0);

                    //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                    thumb.Save(targetPath + "Thum_" + fileGUID);
                    image.Dispose();

                    BE.AttachInfo info = new BE.AttachInfo();
                    info.CompanyID = CompanyID;
                    info.GroupID = _groupID;
                    info.FilePath = targetWebPath;
                    info.FileName = sourceFileInfo.Name;
                    info.FileType = sourceFileInfo.Extension;
                    info.FileLength = sourceFileInfo.Length;
                    info.FileGUID = fileGUID;
                    info.FileOrder = i + 1;
                    AddFile(info);
                }
                */

            }

            return true;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일추가
        /// 작성일자 : 2015-03-04
        /// </summary>
        /// <param name="_attachInfo"></param>
        /// <returns></returns>
        public bool AddFile(BE.AttachInfo _attachInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)
                                               ,new SqlParameter("@FilePath", SqlDbType.NVarChar,1000)                                                 
                                               ,new SqlParameter("@FileName", SqlDbType.NVarChar,500)
                                               ,new SqlParameter("@FileType", SqlDbType.NVarChar,5)
                                               ,new SqlParameter("@FileLength", SqlDbType.BigInt)                                             
                                               ,new SqlParameter("@FileGUID", SqlDbType.VarChar,50)  
                                               ,new SqlParameter("@FileOrder", SqlDbType.Int)  
                };

                parms[0].Value = _attachInfo.CompanyID;
                parms[1].Value = _attachInfo.GroupID;
                parms[2].Value = _attachInfo.FilePath;
                parms[3].Value = _attachInfo.FileName;
                parms[4].Value = _attachInfo.FileType;
                parms[5].Value = _attachInfo.FileLength;
                parms[6].Value = _attachInfo.FileGUID;
                parms[7].Value = _attachInfo.FileOrder;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "INS_PROC_Attach");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAddFile", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddFile(" + _attachInfo.GroupID.ToString() + "," + _attachInfo.FileName + ")", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글 추가
        /// 작성일자 : 2012-11-06
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        public int AddMyTalk(BE.MyTalkInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@Content", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@RUserID", SqlDbType.Int)
                                               ,new SqlParameter("@JobDate", SqlDbType.Char,8)
                                               ,new SqlParameter("@IsSecurity", SqlDbType.Char, 1)                                             
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)  
                };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.Content;
                parms[2].Value = _info.UserID;
                parms[3].Value = _info.RUserID;
                parms[4].Value = _info.JobDate;
                parms[5].Value = _info.IsSecurity;
                parms[6].Value = _info.GroupID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "INS_PROC_MyTalk");

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);
                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddMyTalk(" + _info.CompanyID.ToString() + "," + _info.UserID + ")", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글 삭제 - 그룹
        /// 작성일자 : 2012-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelGroupMyTalk(string p)
        {
            SetDecryptString(p);
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                // 파일 삭제
                string sqlAttachQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Attach");
                sqlAttachQuery = String.Format(sqlAttachQuery, CompanyID, groupID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlAttachQuery, "dsMyTalkAttach", null);
                string filePath = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    filePath = dr["FilePath"].ToString();
                }

                if (filePath != "")
                {
                    try
                    {
                        if (Directory.Exists(Server.MapPath(filePath))) Directory.Delete(Server.MapPath(filePath), true);
                    }
                    catch
                    {
                    }
                }

                // DB 삭제 
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "DEL_PROC_GROUPID");

                SqlParameter[] parm = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@GroupID", SqlDbType.Int),
 									       };
                parm[0].Value = CompanyID;
                parm[1].Value = groupID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);
                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelMyTalk()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글 삭제 - 댓글의 댓글
        /// 작성일자 : 2012-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMyTalk(string p)
        {
            SetDecryptString(p);
            string myTalkID = GetDecryptParam("myTalkID", "0");

            try
            {
                // DB 삭제 
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "DEL_PROC_MyTalk");

                SqlParameter[] parm = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@MyTalkID", SqlDbType.Int),
                                               new SqlParameter("@UserID", SqlDbType.Int),
									       };
                parm[0].Value = CompanyID;
                parm[1].Value = myTalkID;
                parm[2].Value = UserID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelMyTalk()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-11-06
        /// 작성목적 : 댓글 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <param name="_alarmType"></param>
        /// <returns></returns>
        public bool SendAlarmUser(BE.MyTalkInfo _info, int _receiverID, string _subject, int _alarmType)
        {

            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _info.Content.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AlarmMyTalkURL");

            alarmTargetURL = string.Format(alarmTargetURL, httpType, fullDomain);

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/MyTalk/List.aspx?";
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = _alarmType;
            alarmInfo.KindSubject = "SNS";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            return biz.Add(alarmInfo);
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Spool 폴더 파일삭제
        /// 작성일자 : 2015-03-17
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSpoolFileList(string p)
        {
            SetDecryptString(p);
            string fileName = GetDecryptParam("fileName", "");
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            try
            {
                // 파일 삭제
                string filePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/" + fileName;

                if (File.Exists(Server.MapPath(filePath)))
                {
                    System.IO.File.Delete(Server.MapPath(filePath));
                }

                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelSpoolFileList()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장
        /// 작성일자 : 2015-11-10
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddDraft(string p)
        {
            SetDecryptString(p);
            string rUserID = GetDecryptParam("RUserID", "0");
            string content = GetDecryptParam("Content", "");
            string isSecurity = GetDecryptParam("IsSecurity", "0");
            string groupID = GetDecryptParam("GroupID", "0");

            try
            {
                if (CompanyID <= 0 || UserID <= 0) return "";

                BE.MyTalkInfo info = new BE.MyTalkInfo();

                info.CompanyID = CompanyID;
                info.RUserID = rUserID.ToNumber<int>();
                info.GroupID = groupID.ToNumber<int>();
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.IsSecurity = isSecurity;
                info.WriteUserID = UserID;

                int DraftTalkID = AddDraft(info);
                return DraftTalkID.ToString();

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddDraft", ex);
                return "";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시저장
        /// 작성일자 : 2015-11-10
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int AddDraft(BE.MyTalkInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@RUserID", SqlDbType.Int)
                                               ,new SqlParameter("@GroupID", SqlDbType.Int)  
                                               ,new SqlParameter("@Content", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@IsSecurity", SqlDbType.Char, 1)                                             
                                               ,new SqlParameter("@WriteUserID", SqlDbType.Int)     
                                               
                };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.RUserID;
                parms[2].Value = _info.GroupID;
                parms[3].Value = _info.Content;
                parms[4].Value = _info.IsSecurity;
                parms[5].Value = _info.WriteUserID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "INS_PROC_MyTalkDraft");

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);
                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddDraft()", ex);
                return -1;
            }
        }
    }
}
