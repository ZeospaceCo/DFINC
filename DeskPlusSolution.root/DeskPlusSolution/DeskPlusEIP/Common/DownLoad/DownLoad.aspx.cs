using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.DAL;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Common.Web.DownLoad
{
    public partial class DownLoad : Base.BasePageCommon
    {
        private void Page_Load(object sender, System.EventArgs e)
        {

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.DownLoad.DownLoad).Assembly;	//어셈블리 선언
            #endregion

            SetLanguage();

            //	Paremeter ex >
            //	http://localhost/deskpluseip/common/upload/downLoad.aspx?filePath=/DeskPlusEIP/FileData/1/Board/200607/83/1.dat&fileName=테스트.gif
            // hlAttachFile.NavigateUrl = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?filePath=" + Server.UrlEncode(url) + "&fileName=" + Server.UrlEncode(fileName);
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string filePath = GetDecryptParam("filePath", "");	// 파일 경로(경로 + 파일guid + zeo 확장자)
            string fileName = GetDecryptParam("fileName", "");	// 파일 명(filename + 실질 파일 확장자)

            if (filePath.Trim() == "" || 
                    fileName.Trim() == "" ||
                    (filePath.ToLower().IndexOf(baseFileDirectory.ToLower()) == -1 && filePath.ToLower().IndexOf("/deskplusedm/filedata/") == -1) || filePath.IndexOf("../") > 0)                
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                        "file download path is problem  " +
                        " | filepath = " + filePath +
                        " | filename = " + fileName,
                        "/DeskPlusEIP/Data/Logs/FileDownload/", "exception");
                return;
            }
            
            if(!System.IO.File.Exists(Server.MapPath(filePath)))
            {
                string tmpFileExt = fileName.Split('.')[1];//실질 파일 확장자(hwp, doc 등등)
                filePath = filePath.Replace("zeo", tmpFileExt); //확장자 zeo 파일을 실질 파일 확장자로 변환

                if (!System.IO.File.Exists(Server.MapPath(filePath)))
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file is not exist. filePath : " + filePath);
                    return;
                }

                string blockExt = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(
                            AppSettingType.COMMON, "BlockExt", "");

                string[] blockExtStr = null;
                if (blockExt.Length > 0) blockExtStr = blockExt.Split(',');

                bool response = false;                
                if (blockExtStr != null)
                {
                    foreach (string ext in blockExtStr)
                    {
                        if (tmpFileExt.IndexOf(ext.ToLower()) != -1)
                        {
                            response = true;
                            break;
                        }
                    }
                }

                //차단용 확장자일 경우,
                if (response)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                        "file download is problem " +
                        " | filepath = " + filePath,
                        "/DeskPlusEIP/Data/Logs/FileDownload/", "");

                    return;
                }
            }

            #region 파일리스트에서 다운 허용 체크되어있는지 확인

            if (filePath.ToUpper().IndexOf("/BOARD/") > 0 )  //filePath에 board 가 포함되어 있으면 게시판 첨부파일
            {
                int listid = filePath.Split('/')[6].ToNumber<int>();

                Zeospace.DeskPlus.Board.DAL.ListEtc etc = new ListEtc();
                List<Board.BE.ListEtcInfo> al = etc.GetListID(CurrentUserInfo.CompanyID, listid);

                if (al.Count > 0)
                {
                    foreach (Board.BE.ListEtcInfo info in al)
                    {
                        if (info.Etc8.ToString() == "1") //특정사용자들만 첨부파일이 가능하도록 체크가 되어있다면
                        {
                            Application.Lock();
                            Application.Remove("SysCode" + CurrentUserInfo.CompanyID);
                            Application.UnLock();
                            if (!CurrentUserInfo.IsInRole("AllowBoardDownUser") && !CheckIP() ) //게시판 첨부다운 허용role이 없고, IP대역도 다르다면 다운불가 메시지 출력
                            {
                                Response.Write("<script type='text/javascript'>");
                                Response.Write("window.alert('" + resMng.GetString("DownLoadArea") + "');self.opener = self;self.close();");
                                Response.Write("<" + "/script>");
                                Response.End();
                            }
                        }
                    }
                }
            }
            #endregion 
            // 지정된 경로 이외의 파일요청 거부
            try
            {
                if (filePath.Trim() != "" && fileName.Trim() != "")
                {
                    // 다운로드 시킬 파일의 경로
                    string fileFullPath = Server.UrlDecode(filePath);
                    string tempFileName = Path.GetFileNameWithoutExtension(fileName);
                    string tempFileExtention = Path.GetExtension(fileName);

                    //bool isKoreanOrAscii = true;

                    for (int i = 0; i < fileName.Length; i++)
                    {
                        char ch = fileName[i];
                        //ASCII : ~ Ox00FF
                        //특수문자 : OxA1A1 ~ OxACF1
                        //한글 음절 : OxAC00 ~ 0xD7A3
                        //특수문자(원문자)가 들어간 파일을 엑셀에서 다운 받지 못함(현담산업) ByFreeman 20070715
                        //if ((ch > 0x00FF && ch < 0xAC00) || ch > 0xD7A3) 
                        if (ch > 0xD7A3)
                        {
                            //isKoreanOrAscii = false;
                            break;
                        }
                    }


                    if (tempFileName.Length > 64)
                    {
                        tempFileName = tempFileName.Substring(0, 60);
                        tempFileName += "~";
                    }

                    string FileName = String.Empty;

                    ////파일명이 한글이나 Ascii 일때
                    //isKoreanOrAscii = true;
                    /*
                    if (isKoreanOrAscii)
                    {
                        FileName = tempFileName + tempFileExtention;

                        Request.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                        Response.HeaderEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                    }
                    else
                    {
                        Request.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.HeaderEncoding = System.Text.Encoding.UTF8;
                    }
                     * */
                    
                    //크로스 브라우징에 맞게 첨부파일 다운로드 방식 변경
                    String header = Request.Headers["User-Agent"];
                    if (header.IndexOf("MSIE") > -1 || header.IndexOf("Trident") > -1 || header.IndexOf("Edge") > -1)
                    {
                        FileName = System.Web.HttpUtility.UrlEncode(tempFileName + tempFileExtention).Replace(" ", "%20").Replace("+", "%20");
                    }
                    else
                    {
                        FileName = tempFileName + tempFileExtention;
                    }


                    // 헤더에 파일이름 지정하기
                    System.IO.FileInfo fileInfo = null;

                    string tmpFullPath = fileFullPath.Substring(0, fileFullPath.LastIndexOf('/')) + "/" + FileName;
                    if (File.Exists(Server.MapPath(fileFullPath)))
                    {
                        fileInfo = new FileInfo(Server.MapPath(fileFullPath));
                    }
                    else
                    {
                        fileInfo = new FileInfo(Server.MapPath(tmpFullPath));
                    }


                    #region 파일 로그 넣기

                    // /DeskPlusEIP/FileData/1/Board/201202/100793/a29a0a7b-f378-4d4e-b704-774ca7b26a20.jpg


                    Zeospace.DeskPlus.Common.DAL.DownAttach dalDA = new DAL.DownAttach();
                    Zeospace.DeskPlus.Common.BE.DownAttachInfo daInfo = new BE.DownAttachInfo();

                    string[] remoteAddrStr = Request.UserHostAddress.Split('.');
                    string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                    daInfo.CompanyID = CurrentUserInfo.CompanyID;
                    daInfo.UserID = CurrentUserInfo.UserID;
                    daInfo.Gubun = (filePath.IndexOf("/Users/") != -1) ? filePath.Split('/')[6].ToString() : daInfo.Gubun = filePath.Split('/')[4].ToString();                    
                    daInfo.FilePath = filePath;
                    daInfo.FileName = tempFileName + tempFileExtention;
                    daInfo.FileType = tempFileExtention;
                    daInfo.FileLength = fileInfo.Length;
                    daInfo.IPAddress = remoteAddr;

                    dalDA.Add(daInfo);

                    #endregion


                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());

                    ///파일 확장자에 따라서 ContentType,변경
                    string fileType = "file/unknown";

                    if (!String.IsNullOrEmpty(tempFileExtention))
                    {
                        switch (tempFileExtention.TrimStart('.').ToLower())
                        {
                            case "avi":
                                fileType = "video/x-msvideo";
                                break;
                            case "asf":
                                fileType = "video/x-msvideo";
                                break;
                            case "mpg":
                                fileType = "video/mpeg";
                                break;
                            case "mpeg":
                                fileType = "video/mpeg";
                                break;
                            case "jpg":
                                fileType = "image/jpeg";
                                break;
                            case "jpeg":
                                fileType = "image/jpeg";
                                break;
                            case "gif":
                                fileType = "image/gif";
                                break;
                            case "png":
                                fileType = "image/png";
                                break;
                            case "txt":
                                fileType = "text/plain";
                                break;
                            case "zip":
                                fileType = "application/x-zip-compressed";
                                break;
                        }
                    }

                    Response.ContentType = fileType; // "application/x-zip-compressed"; ByFreeman 20080624 한국선재에서 Zip으로 다운받는 현상 관련 패치
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");

                    Response.TransmitFile(fileInfo.FullName);

                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DownLoad[Page_Load]", ex);
                
            }
            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// IP체크
        /// </summary>
        /// <returns></returns>
        private bool CheckIP()
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.IPList allowIPList = new IPList();
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AllowBoardDownLoadIP");

            string ip = "";
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ActiveCode"].ToString() == "1")
                {
                    ip = dv[i]["InterfaceCode"].ToString();
                    string[] ipArr = ip.Split('-');

                    if (ipArr.Length == 1)
                    {
                        allowIPList.Add(ipArr[0]);
                    }
                    else
                    {
                        allowIPList.AddRange(ipArr[0], ipArr[1]);
                    }
                }
            }

            if (allowIPList.CheckNumber(Request.UserHostAddress))
            {
                return true;      // 접속 IP가 사내 IP 대역에 있음. 
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }
        

        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
