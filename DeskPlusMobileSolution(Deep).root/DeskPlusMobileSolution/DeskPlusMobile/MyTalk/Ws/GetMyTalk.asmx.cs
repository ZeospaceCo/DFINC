using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zeospace.DeskPlusMobile.Web.MyTalk.Ws
{
    /// <summary>
    /// GetMyTalk의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetMyTalk : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 마이톡 리스트 가져오기
        /// 작성일자 : 2012-11-06
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMyTalkList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.MyTalk.Ws.GetMyTalk).Assembly;	//어셈블리 선언            
            this.GetResourceManager();


            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = UserID;
                parm[2].Value = pageNo;
                parm[3].Value = pageSize;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_PROC_MyTalk");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsMyTalkList", parm);

                List<object> listobj = new List<object>();

                string name = "";
                string rname = "";

                int fileNum = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    fileNum++;

                    BE.MyTalkInfo info = new BE.MyTalkInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.MyTalkID = dr["MyTalkID"].ToString().ToNumber<int>();
                    info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.RUserID = dr["RUserID"].ToString().ToNumber<int>();
                    info.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                    info.IsSecurity = dr["IsSecurity"].ToString();

                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    info.UserName = name;

                    if ((info.GroupID == info.MyTalkID) && (info.RUserID != UserID))
                    {
                        rname = this.GetLangName(
                            dr["RNameBase"].ToString(),
                            dr["RNameKo"].ToString(), dr["RNameEn"].ToString(), dr["RNameJa"].ToString(),
                            dr["RNameFr"].ToString(), dr["RNameZhCn"].ToString(), dr["RNameZhTw"].ToString(),
                            PreLang);

                        if (!String.IsNullOrEmpty(rname))
                        {
                            info.RUserName = "[" + rname + "]";
                        }
                        else
                        {
                            info.RUserName = "";
                        }
                    }
                    else
                    {
                        info.RUserName = "";
                    }


                    #region 등록자 프로필 사진 이미지 설정
                    string photoName = dr["PhotoImage"].ToString();
                    string BaseImgPath = "";
                    string imgTag = "";
                    string encParams = "";
                    bool response = false;

                    Dictionary<string, string> paraDic = null;

                    string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

                    if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                    {
                        BaseImgPath = filePath + "/{0}/Users/{1}/Picture/" + photoName;
                        BaseImgPath = string.Format(BaseImgPath, CompanyID, info.UserID);

                        if (System.IO.File.Exists(Server.MapPath(BaseImgPath))) response = true;
                    }

                    if (response == false) BaseImgPath = "/DeskPlusMobile/Content/ko/Base/Img/noimg.gif";

                    if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                    {
                        #region 스크린샷
                        paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", BaseImgPath);
                        encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        BaseImgPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                        #endregion

                        imgTag = "<img src='{0}' class='tphoto' />";
                        imgTag = string.Format(imgTag, BaseImgPath);
                    }

                    info.PhotoImage = imgTag;
                    #endregion

                    #region 등록한 사진 이미지 설정
                    info.FileCnt = dr["FileCnt"].ToString().ToNumber<int>();

                    if ((info.GroupID == info.MyTalkID) && (info.FileCnt > 0))
                    {
                        string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Attach");
                        sqlQuery = String.Format(sqlQuery, CompanyID, info.GroupID);

                        DataSet dsAttach = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "dsMyTalkAttach", null);

                        string fileTag = "";
                        string fileTagStr = "";

                        foreach (DataRow drAttach in dsAttach.Tables[0].Rows)
                        {
                            //string BasefilePath = drAttach["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/") + "Thum_" + drAttach["FileGUID"].ToString();                                                                                  
                            string BasefilePath = drAttach["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/") + drAttach["FileGUID"].ToString();
                            //string encParams1 = "";

                            //Dictionary<string, string> paraDic1 = null;

                            if (System.IO.File.Exists(Server.MapPath(BasefilePath)))
                            {
                                /*
                                #region 스크린샷
                                paraDic1 = new Dictionary<string, string>();
                                paraDic1.Add("filePath", BasefilePath);
                                encParams1 = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic1));
                                BasefilePath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams1;
                                #endregion
                                */

                                fileTag = "<li><img src='{0}' class='imageFile' /></li>";
                                fileTagStr += string.Format(fileTag, BasefilePath);
                            }
                        }

                        //info.FileImage = "<div class='touchslider" + fileCnt + "'><div class='touchslider-viewport' style='width:250px;height:250px;overflow:hidden'><div>";
                        //info.FileImage += fileTagStr + "</div></div></div>";

                        if (info.FileCnt > 1)
                        {
                            info.FileImage = "<div id='touchslider" + fileNum + "' class='blog_contImg'><ul>";
                            info.FileImage += fileTagStr + "</ul></div><div id='touchpaging" + fileNum + "' class='blog_cnt'></div>";
                        }
                        else
                        {
                            info.FileImage = "<div class='blog_contImg'><ul>";
                            info.FileImage += fileTagStr + "</ul></div><div id='touchpaging" + fileNum + "' class='blog_cnt'>1/1</div>";
                        }
                    }
                    else
                    {
                        info.FileImage = "";
                    }

                    #endregion

                    DateTime dt;

                    if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                        info.WriteDateYMD = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                    else
                        info.WriteDateYMD = string.Empty;

                    #region 날짜 비교(New Img)
                    DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                    info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                    #endregion

                    // 원본글 작성자와 댓글러가 같은지
                    if (UserID == info.RUserID)
                    {
                        info.View = "Y";
                    }
                    else if (UserID == info.UserID && info.IsSecurity == "1")
                    {
                        info.View = "Y";
                    }
                    else if (info.IsSecurity == "0")
                    {
                        info.View = "Y";
                    }
                    else
                    {
                        info.View = "N";
                        info.Content = resMng.GetString("SecurityMsg");
                    }

                    if (info.GroupID != info.MyTalkID)
                    {
                        info.Depth = "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_arrow.gif' title='' />";
                    }
                    else
                    {
                        info.Depth = "";
                    }

                    info.IsAuth = (info.UserID == UserID) ? "Y" : "N";

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetMyTalkList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 마이톡 상세내용 가지고 오기
        /// 작성일자 : 2012-11-06
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMyTalkView(string p)
        {

            SetDecryptString(p);
            string groupID = GetDecryptParam("GroupID", "0");
            string dWidth = GetDecryptParam("DWidth", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_MYTALKBYGROUPID");
            sqlQuery = String.Format(sqlQuery, CompanyID, groupID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "dsMyTalk", null);

            List<object> listobj = new List<object>();

            string name = "";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BE.MyTalkInfo info = new BE.MyTalkInfo();

                info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                info.MyTalkID = dr["MyTalkID"].ToString().ToNumber<int>();
                info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                info.UserID = dr["UserID"].ToString().ToNumber<int>();
                info.IsSecurity = dr["IsSecurity"].ToString();
                info.RUserID = dr["RUserID"].ToString().ToNumber<int>();

                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                    dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                    PreLang);

                info.UserName = name;

                info.IsAuth = (info.UserID == UserID) ? "Y" : "N";

                DateTime dt;

                if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                    info.WriteDateYMD = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                else
                    info.WriteDateYMD = string.Empty;

                info.View = "N";

                // 원본글 작성자와 댓글러가 같은지
                if (UserID == info.RUserID)
                {
                    info.View = "Y";
                }
                else if (UserID == info.UserID && info.IsSecurity == "1")
                {
                    info.View = "Y";
                }
                else if (info.IsSecurity == "0")
                {
                    info.View = "Y";
                }

                #region 등록한 사진 이미지 설정
                info.FileCnt = dr["FileCnt"].ToString().ToNumber<int>();

                if ((groupID.ToNumber<int>() == info.MyTalkID) && (info.FileCnt > 0))
                {
                    string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_QRY_Attach");
                    sqlQuery1 = String.Format(sqlQuery1, CompanyID, groupID);

                    DataSet dsAttach = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery1, "dsMyTalkAttach", null);

                    string fileTag = "";
                    string fileTagStr = "";

                    foreach (DataRow drAttach in dsAttach.Tables[0].Rows)
                    {
                        string BasefilePath = drAttach["FilePath"].ToString().Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/") + drAttach["FileGUID"].ToString();
                        //string encParams1 = "";

                        //Dictionary<string, string> paraDic1 = null;

                        if (System.IO.File.Exists(Server.MapPath(BasefilePath)))
                        {
                            /*
                            #region 스크린샷
                            paraDic1 = new Dictionary<string, string>();
                            paraDic1.Add("filePath", BasefilePath);
                            encParams1 = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic1));
                            BasefilePath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams1;
                            #endregion

                            fileTag = "<img src='{0}' /><br/>";
                            fileTagStr += string.Format(fileTag, BasefilePath);

                            Bitmap sealBitmap;
                            sealBitmap = new Bitmap(Server.MapPath(BasefilePath));

                            int realWidth = sealBitmap.Width;

                            if (realWidth < Convert.ToInt32(dWidth))
                            {
                                fileTag = "<img src='{0}' /><br/>";
                                fileTagStr += string.Format(fileTag, BasefilePath);
                            }
                            else
                            {
                                fileTag = "<img src='{0}' width='100%' /><br/>";
                                fileTagStr += string.Format(fileTag, BasefilePath);
                            }
                            */

                            fileTag = "<img src='{0}' /><br/>";
                            fileTagStr += string.Format(fileTag, BasefilePath);
                        }
                    }

                    info.FileImage = fileTagStr;
                }
                else
                {
                    info.FileImage = "";
                }

                #endregion

                listobj.Add(info);
            }

            return listobj;
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 마이톡 리스트 가져오기
        /// 작성일자 : 2015-03-17
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
                string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/");

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
                        BasefilePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MyTalk/Mobile/" + swfUploadGUID + "/" + sourceFileInfo.Name;

                        fileTag = "<img src='{0}' width='30px' height='30px' />";
                        fileTagStr = string.Format(fileTag, BasefilePath);

                        info.FilePath = fileTagStr;

                        listobj.Add(info);
                    }
                }

                /*
                string[] fileArr = Directory.GetFiles(uploadPath);

                string fileName = "";
                string tmpFile = "";
                string sourceFile = "";
                int tmpfileIdx = -1;
                string fileTag = "";
                string fileTagStr = "";
                string BasefilePath = "";

                for (int i = 0; i < fileArr.Length; i++)
                {
                    tmpFile = fileArr[i];
                    tmpfileIdx = tmpFile.LastIndexOf("\\");
                    fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                    sourceFile = uploadPath + fileName;
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);

                    BE.AttachInfo info = new BE.AttachInfo();
                    info.CompanyID = CompanyID;
                    info.FileName = sourceFileInfo.Name;
                    info.FileType = sourceFileInfo.Extension;
                    BasefilePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MyTalk/Mobile/" + swfUploadGUID + "/" + sourceFileInfo.Name;

                    fileTag = "<img src='{0}' width='30px' height='30px' />";                    
                    fileTagStr = string.Format(fileTag, BasefilePath);

                    info.FilePath = fileTagStr;


                    listobj.Add(info);
                }
                */

                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetSpoolList", ex);
                throw;
            }
        }
    }
}
