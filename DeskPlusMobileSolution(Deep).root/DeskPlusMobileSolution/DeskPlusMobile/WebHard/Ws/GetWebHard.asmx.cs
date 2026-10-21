using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.WebHard.Ws
{
    /// <summary>
    /// GetWebHard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetWebHard : Helper.MobileBaseService
    {

        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 파일 리스트 가져오기
        /// 작성일자 : 2012-04-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);
            string folderID = GetDecryptParam("FolderID", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string order = GetDecryptParam("Order", "0");
            string shareGubun = GetDecryptParam("ShareGubun", "N");
            string publicGubun = GetDecryptParam("PublicGubun", "N");

            try
            {
                this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.WebHard.Ws.GetWebHard).Assembly;	//어셈블리 선언            
                this.GetResourceManager();

                List<object> listobj = new List<object>();

                if ((pageNo == "1") && (folderID != "-3") && (shareGubun == "N"))
                {
                    string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_Folder");
                    SqlParameter[] parm1 = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@FolderID", SqlDbType.Int)
                                            ,new SqlParameter("@PublicGubun", SqlDbType.Char, 1)
                };
                    parm1[0].Value = CompanyID;
                    parm1[1].Value = UserID;
                    parm1[2].Value = folderID;
                    parm1[3].Value = publicGubun;

                    DataSet dss = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "dsList", parm1);

                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        BE.FileInfo info = new BE.FileInfo();

                        info.UserID = dr["UserID"].ToString().ToNumber<int>();
                        info.FolderID = dr["FolderID"].ToString().ToNumber<int>();
                        info.FileID = 0;
                        info.FilePath = "";
                        info.FileName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                        info.FileLength = "";
                        info.WriteDate = "";
                        info.FileGUID = "";
                        info.ScreenShot = "";
                        info.SendName = "";
                        info.ImagePath = "/DeskPlusMobile/Content/ko/Base/Img/Ext/folder.gif";
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", info.ImagePath);
                        string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        info.ImagePath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;

                        if (publicGubun == "Y")
                        {
                            SqlParameter[] parmschk = new SqlParameter[] {
                                                   new SqlParameter("@CompanyID", SqlDbType.Int)
                                                  ,new SqlParameter("@UserID", SqlDbType.Int)                                                
                                                  ,new SqlParameter("@FolderID", SqlDbType.Int)  
                        };
                            parmschk[0].Value = CompanyID;
                            parmschk[1].Value = UserID;
                            parmschk[2].Value = dr["FolderID"].ToString().ToNumber<int>();

                            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_PublicFolder_FolderID");
                            DataSet dschk = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsPublic", parmschk);

                            if (dschk.Tables[0].Rows.Count < 1)
                            {
                                info.FileName = resMng.GetString("NoPermission");
                            }
                        }

                        listobj.Add(info);
                    }
                }

                SqlParameter[] parmschk2 = new SqlParameter[] {
                                                   new SqlParameter("@CompanyID", SqlDbType.Int)
                                                  ,new SqlParameter("@UserID", SqlDbType.Int)                                                
                                                  ,new SqlParameter("@FolderID", SqlDbType.Int)  
                };
                parmschk2[0].Value = CompanyID;
                parmschk2[1].Value = UserID;
                parmschk2[2].Value = folderID;

                string sqlchk2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_PublicFolder_FolderID");
                DataSet dschk2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlchk2, "dsPublic", parmschk2);

                if ((publicGubun == "Y" && dschk2.Tables[0].Rows.Count > 0) || publicGubun == "N")
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_File");
                    SqlParameter[] parm = new SqlParameter[]{
                                                 new SqlParameter("@CompanyID", SqlDbType.Int)
                                                ,new SqlParameter("@UserID", SqlDbType.Int)
                                                ,new SqlParameter("@FolderID", SqlDbType.Int)
                                                ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                                ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                                ,new SqlParameter("@Order", SqlDbType.VarChar, 100)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = UserID;
                    parm[2].Value = folderID;
                    parm[3].Value = pageNo;
                    parm[4].Value = pageSize;
                    parm[5].Value = order;

                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.FileInfo info = new BE.FileInfo();

                        info.UserID = dr["UserID"].ToString().ToNumber<int>();
                        info.FolderID = dr["FolderID"].ToString().ToNumber<int>();
                        info.FileID = dr["FileID"].ToString().ToNumber<int>();

                        string tmpPath = Regex.Replace(dr["FilePath"].ToString(), "/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/", RegexOptions.IgnoreCase);

                        info.FilePath = tmpPath + dr["FileGUID"].ToString();
                        info.FileName = dr["FileName"].ToString();
                        info.FileLength = "(" + FormatSize(Convert.ToDouble(dr["FileLength"].ToString())) + ")";

                        info.WriteName = this.GetLangName(
                        dr["WriteNameBase"].ToString().ToNull(), dr["WriteNameKo"].ToString().ToNull(),
                        dr["WriteNameEn"].ToString().ToNull(), dr["WriteNameJa"].ToString().ToNull(),
                        dr["WriteNameFr"].ToString().ToNull(), dr["WriteNameZhcn"].ToString().ToNull(),
                        dr["WriteNameZhtw"].ToString().ToNull(), PreLang); 

                        info.WriteDate = Convert.ToDateTime(dr["WriteDate"].ToString()).ToString();
                        info.FileGUID = dr["FileName"].ToString().Replace("'", "");
                        info.ScreenShot = dr["ScreenShot"].ToString();

                        if (info.FolderID == -3)
                        {
                            info.SendName = resMng.GetString("SendUser") + " : " + this.GetLangName(
                                    dr["SendNameBase"].ToString().ToNull(), dr["SendNameKo"].ToString().ToNull(),
                                    dr["SendNameEn"].ToString().ToNull(), dr["SendNameJa"].ToString().ToNull(),
                                    dr["SendNameFr"].ToString().ToNull(), dr["SendNameZhcn"].ToString().ToNull(),
                                    dr["SendNameZhtw"].ToString().ToNull(), PreLang);
                        }
                        else
                        {
                            info.SendName = "";
                        }

                        if (info.ScreenShot == "Y")
                        {
                            info.ImagePath = tmpPath + "Thum_" + dr["FileGUID"].ToString();
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", info.ImagePath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.ImagePath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                        }
                        else
                        {
                            #region 아이콘 이미지
                            info.ImagePath = "/DeskPlusMobile/Content/ko/Base/Img/Ext/" + dr["FileType"].ToString().Replace(".", "") + ".gif";
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", info.ImagePath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.ImagePath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                            #endregion
                        }

                        listobj.Add(info);
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 문서 스트리밍 정보 가져오기
        /// 작성일자 : 2018-04-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDSFile(string p)
        {
            SetDecryptString(p);
            string fileID = GetDecryptParam("FileID", "0");
            string attachType = GetDecryptParam("TYPE", "");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "dsDS";
            ds.Tables.Add(dt);

            if (CompanyID <= 0 || UserID <= 0) return StringHelper.GetJSONString(ds.Tables[0]);

            if (String.IsNullOrEmpty(fileID)) return StringHelper.GetJSONString(ds.Tables[0]);

            try
            {
                var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "DocStreamYN");

                if (docStreamYN == "Y")
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_DS");
                    SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@FileID", SqlDbType.Int)
                                            ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)

                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = fileID;
                    parm[2].Value = attachType;
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsDS", parm);
                }

                return StringHelper.GetJSONString(ds.Tables[0]);
                
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDSFile('" + fileID + "')", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 공유자 리스트 가져오기
        /// 작성일자 : 2012-04-17
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShare()
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.WebHard.Ws.GetWebHard).Assembly;	//어셈블리 선언            
                this.GetResourceManager();

                List<object> listobj = new List<object>();                

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_ShareFolder");
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)

                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;


                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ShareFolderInfo info = new BE.ShareFolderInfo();

                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                  dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                    info.TitleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                               dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                    info.DeptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                               dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                    info.FolderID = dr["FolderID"].ToString().ToNumber<int>();
                    info.FolderName = this.GetLangName(dr["FNameBase"].ToString(), dr["FNameKo"].ToString(), dr["FNameEn"].ToString(),
                                  dr["FNameJa"].ToString(), dr["FNameFr"].ToString(), dr["FNameZhcn"].ToString(), dr["FNameZhtw"].ToString(), PreLang);

                    if (info.FolderName == "ROOT")
                    {
                        info.FolderName = resMng.GetString("PrivateFolderName");
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetShare()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 임시 저장 메일 가져오기
        /// 작성일자 : 2012-04-18
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDraft(string p)
        {
            SetDecryptString(p);
            string mailID = GetDecryptParam("MailID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_MESSAGE");
            sqlQuery = String.Format(sqlQuery, CompanyID, UserID, mailID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetDraft", null);            

            return StringHelper.GetJSONString(ds.Tables[0]);
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 파일 삭제 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetChk(string p)
        {
            SetDecryptString(p);
            string tmpValue = GetDecryptParam("tmpValue", "");
            tmpValue = tmpValue.TrimEnd(',');

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_PublicFolderFile_SelByUserID");
            SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@FileID", SqlDbType.VarChar, 4000)

                };
            parm[0].Value = CompanyID;
            parm[1].Value = UserID;
            parm[2].Value = tmpValue;

            return SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parm).ToString();
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 첨부파일 가져오기
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSpoolList(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "0");

            try
            {
                //string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + swfUploadGUID + "/");
                string uploadPath = Server.MapPath("/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID + "/");

                List<object> listobj = new List<object>();

                DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                string sourceFile = "";
                string fileTag = "";
                string fileTagStr = "";
                string BasefilePath = "";

                foreach (var item in files)
                {
                    if (item.Name != "Thumbs.db")
                    {

                        sourceFile = uploadPath + item.Name;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);

                        BE.AttachInfo info = new BE.AttachInfo();
                        info.CompanyID = CompanyID;
                        info.FileName = sourceFileInfo.Name;
                        info.FileType = sourceFileInfo.Extension;

                        /*
                        //BasefilePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + swfUploadGUID + "/" + sourceFileInfo.Name;
                        BasefilePath = "/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID + "/" + sourceFileInfo.Name;


                        fileTag = "<img src='{0}' width='30px' height='30px' />";
                        if (sourceFileInfo.Extension.IndexOf("bmp") > 0 || sourceFileInfo.Extension.IndexOf("tif") > 0 || sourceFileInfo.Extension.IndexOf("jpeg") > 0 || sourceFileInfo.Extension.IndexOf("jpg") > 0 ||
                            sourceFileInfo.Extension.IndexOf("gif") > 0 || sourceFileInfo.Extension.IndexOf("png") > 0 || sourceFileInfo.Extension.IndexOf("pjpeg") > 0 || sourceFileInfo.Extension.IndexOf("pjpg") > 0)
                        {                            
                            fileTagStr = string.Format(fileTag, BasefilePath);
                        }
                        else
                        {
                            fileTagStr = string.Format(fileTag, "/DeskPlusMobile/Content/ko/Base/Img/Ext/" + sourceFileInfo.Extension.Replace(".", "") + ".gif");                            
                        }

                        info.FilePath = fileTagStr;
                        */

                        listobj.Add(info);

                    }
                }


                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetSpoolList", ex);
                throw;
            }
        }
        #endregion 

        #region 기타
        /// <summary>
        /// 사이즈 처리
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        protected string FormatSize(double fileSize)
        {
            if (fileSize < 1024)
                return String.Format("{0:N0}B", fileSize);
            else if (fileSize < 1024 * 1024)
                return String.Format("{0:N1}KB", fileSize / 1024);
            else if (fileSize < 1024 * 1024 * 1024)
                return String.Format("{0:N1}MB", fileSize / (1024 * 1024));
            else
                return String.Format("{0:N1}GB", fileSize / (1024 * 1024 * 1024));
        }

        #endregion 
    }
}
