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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Carboncopy
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSApproval
    {
        #region 첨부파일

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAttachFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                string cirDocID = GetDecryptParam("cirDocumentID", "0");

                ObjList listobj = new ObjList();

                DAL.CirculationAttachFile dal = new DAL.CirculationAttachFile();
                List<BE.CirculationAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (var _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.CirculationAttachFileInfo info = new BE.CirculationAttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.DocumentID = _attachInfo.DocumentID;
                    info.CirculationAttachFileID = _attachInfo.CirculationAttachFileID;

                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = _attachInfo.FileGUID;
                    info.FileCnt = attachInfo.Count;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;

                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string fileOk = "0";
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (_attachInfo.FileType.ToLower() == allowedExtensions[i])
                        {
                            fileOk = "1";
                            break;
                        }
                    }

                    listobj.Add(info);
                }


                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 첨부파일 정보 가져오기 (ContextMenu)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            try
            {
                SetDecryptString(p);

                string cirDocID = GetDecryptParam("cirDocumentID", "0");

                string response = "0";

                DAL.CirculationAttachFile dal = new DAL.CirculationAttachFile();
                List<BE.CirculationAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.CirculationAttachFileInfo _attachInfo in attachInfo)
                {
                    encFilePath = (_attachInfo.FileGUID != "") ? _attachInfo.FilePath + _attachInfo.FileGUID : _attachInfo.FilePath + _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", encFilePath);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    encFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    totalFileInfo += _attachInfo.FileName + ":" + encFilePath + "|"; // Key < 파일경로파일명
                }

                totalFileInfo = totalFileInfo.TrimEnd('|');

                if (totalFileInfo.Length > 0)
                {
                    response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region 근거문서

        /// <summary>
        /// 근거문서 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetBaseDocList(string p)
        {
            try
            {
                SetDecryptString(p);
                string cirDocID = GetDecryptParam("cirDocumentID", "0");

                ObjList listobj = new ObjList();

                DAL.CirculationBaseDocument dal = new DAL.CirculationBaseDocument();
                List<BE.CirculationBaseDocumentInfo> baseDocInfo = dal.GetBaseDocLists(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());


                foreach (var be in baseDocInfo)
                {
                    var info = new BE.CirculationBaseDocumentInfo();

                    info.CompanyID = be.CompanyID;
                    info.CirculationDocumentID = be.CirculationDocumentID;
                    info.BaseDocumentID = be.BaseDocumentID;
                    info.DocumentID = be.DocumentID;
                    info.Name = be.Name;
                    info.Subject = be.Subject.ToReverseXSS();
                    info.IsLegacy = be.IsLegacy;
                    info.Path = be.Path;
                    info.BaseOrder = be.BaseOrder;

                    listobj.Add(info);
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 근거문서 정보 가져오기 (ContextMenu)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetBaseDocListMenu(string p)
        {
            try
            {
                SetDecryptString(p);
                string cirDocID = GetDecryptParam("cirDocumentID", "0");

                string rtnval = "";

                DAL.CirculationBaseDocument dal = new DAL.CirculationBaseDocument();
                List<BE.CirculationBaseDocumentInfo> baseDocInfo = dal.GetBaseDocLists(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());

                foreach (BE.CirculationBaseDocumentInfo _info in baseDocInfo)
                {
                    rtnval += _info.BaseDocumentID + ":" + _info.Name + ";";
                }
                return rtnval.TrimEnd(';');
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }
        #endregion
    }
}
