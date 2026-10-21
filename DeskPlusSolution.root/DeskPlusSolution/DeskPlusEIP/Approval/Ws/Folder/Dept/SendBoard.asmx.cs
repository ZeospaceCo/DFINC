using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Dept
{
    /// <summary>
    /// SendBoard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SendBoard : Base.BaseWSApproval
    {
        #region ADD()

        /// <summary>
        /// 작성자 : 김형석
        /// 게시하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool BoardRegister(string p)
        {
            try
            {

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", ""); //문서id
                string catID = GetDecryptParam("catID", ""); //카테고리id
                string yearMonth = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM");

                DAL.Document dal = new DAL.Document();
                var boaListID = dal.BoardRegister(CurrentUserInfo.CompanyID, docID.ToNumber<int>(), catID.ToNumber<int>(), CurrentUserInfo.DeptID, CurrentUserInfo.UserID, CurrentUserInfo.Name);
                
                var appAttDal = new Zeospace.DeskPlus.Approval.DAL.AttachFile();
                var boaAttDal = new Zeospace.DeskPlus.Board.DAL.AttachFiles();
                var sendMail = new DeskPlus.Base.ApplicationFrameworks.Mail.SendMail();

                #region CREATE FOLDER
                var boaFolder = string.Format("/DeskPlusEIP/FileData/{0}/Board/{1}", CurrentUserInfo.CompanyID, yearMonth);
                if (!Directory.Exists(Server.MapPath(boaFolder))) Directory.CreateDirectory(Server.MapPath(boaFolder));
                boaFolder = string.Format("/DeskPlusEIP/FileData/{0}/Board/{1}/{2}", CurrentUserInfo.CompanyID, yearMonth, boaListID);
                if (!Directory.Exists(Server.MapPath(boaFolder))) Directory.CreateDirectory(Server.MapPath(boaFolder));
                #endregion

                #region CREATE MHT                     
                var docList = dal.GetDocList(CurrentUserInfo.CompanyID, docID.ToNumber<int>());
                string guid = System.Guid.NewGuid().ToString();
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                string emlSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, guid);

                Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new DeskPlus.Common.BLL.MailZettaFactory();
                var getSavefilePath =  biz.CreateMHT(CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, CurrentUserInfo.Name, 
                        CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain,
                        "", "",  docList[0].Subject, docList[0].ContentHTML, null, emlSpoolPath, false);                

                FileInfo emlFI = new FileInfo(getSavefilePath);
                System.IO.File.Copy(getSavefilePath, Server.MapPath(boaFolder + "/" + emlFI.Name));

                var boaAttInfo = new Zeospace.DeskPlus.Board.BE.AttachFileInfo();
                boaAttInfo.CompanyID = CurrentUserInfo.CompanyID;
                boaAttInfo.CategoryID = catID.ToNumber<int>();
                boaAttInfo.ListID = boaListID;

                ///DeskPlusEIP/FileData/1/Board/201203/100883/
                boaAttInfo.FilePath = boaFolder + "/";
                boaAttInfo.FileName = docList[0].Subject + ".mht";
                boaAttInfo.FileType = ".mht";
                boaAttInfo.FileLength = emlFI.Length;
                boaAttInfo.FileGUID = emlFI.Name;
                boaAttDal.Add(boaAttInfo);

                #endregion

                var getAttach = appAttDal.GetAttachFileLists(CurrentUserInfo.CompanyID, docID.ToNumber<int>());

                foreach(var appAttachInfo in getAttach)
                {
                    boaAttInfo = new Zeospace.DeskPlus.Board.BE.AttachFileInfo();
                    boaAttInfo.CompanyID = CurrentUserInfo.CompanyID;
                    boaAttInfo.CategoryID = catID.ToNumber<int>();
                    boaAttInfo.ListID = boaListID;

                    ///DeskPlusEIP/FileData/1/Board/201203/100883/
                    boaAttInfo.FilePath = boaFolder + "/";
                    boaAttInfo.FileName = appAttachInfo.FileName;
                    boaAttInfo.FileType = appAttachInfo.FileType;
                    boaAttInfo.FileLength = appAttachInfo.FileLength;
                    boaAttInfo.FileGUID = appAttachInfo.FileGUID;

                    var respon = boaAttDal.Add(boaAttInfo);
                    if (respon)
                    {
                        var appAttFile = appAttachInfo.FilePath + appAttachInfo.FileGUID;
                        var boaAttFile = boaAttInfo.FilePath  + appAttachInfo.FileGUID;
                        if (System.IO.File.Exists(Server.MapPath(appAttFile))) 
                            System.IO.File.Copy(Server.MapPath(appAttFile), Server.MapPath(boaAttFile), true);

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("BoardRegister : " + ex.Message);
                throw;
            }
        }

        #endregion

        #region MOD()

        #endregion

        #region DEL()

        #endregion
    }
}
