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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Common
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
                string documentID = GetDecryptParam("childDocID", "0");

                ObjList listobj = new ObjList();

                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.AttachFileInfo info = new BE.AttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.DocumentID = _attachInfo.DocumentID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = _attachInfo.FileGUID;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;

                    info.ApprovalState = _attachInfo.ApprovalState;

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

                    info.ImageType = fileOk;

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
        /// 첨부파일 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetAttachFileCnt(string p)
        {
            try
            {
                SetDecryptString(p);
                string documentID = GetDecryptParam("childDocID", "0");

                DAL.AttachFile dal = new DAL.AttachFile();
                return dal.GetAttachFileCntByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());
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

                int childDocID = GetDecryptParam("childDocID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, childDocID);

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
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
                string documentID = GetDecryptParam("childDocID", "0");

                ObjList listobj = new ObjList();

                DAL.BaseDocument dal = new DAL.BaseDocument();
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());


                foreach (BE.BaseDocumentInfo _baseDocInfo in baseDocInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.BaseDocumentInfo info = new BE.BaseDocumentInfo();

                    info.CompanyID = _baseDocInfo.CompanyID;
                    info.DocumentID = _baseDocInfo.DocumentID;
                    info.ParentDocumentID = _baseDocInfo.ParentDocumentID;
                    info.Name = _baseDocInfo.Name;
                    info.Subject = _baseDocInfo.Subject.ToReverseXSS();
                    info.ApprovalState = _baseDocInfo.ApprovalState;

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
        /// 근거문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBaseDocCnt(string p)
        {
            try
            {
                SetDecryptString(p);
                string documentID = GetDecryptParam("childDocID", "0");

                DAL.BaseDocument dal = new DAL.BaseDocument();
                return dal.GetBaseDocCntByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());
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
                string documentID = GetDecryptParam("childDocID", "0");

                string rtnval = "";

                DAL.BaseDocument dal = new DAL.BaseDocument();
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

                foreach (BE.BaseDocumentInfo _info in baseDocInfo)
                {
                    rtnval += _info.ParentDocumentID + ":" + _info.Name + ";";
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

        #region 문서내용
        /// <summary>
        /// 문서내용가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDocCont(string p)
        {
            try
            {
                SetDecryptString(p);
                int documentID = GetDecryptParam("childDocID", "0").ToNumber<int>();

                string ApprovalState = "";
                DAL.Document dpDal = new DAL.Document();
                DataTableReader dtr = dpDal.GetDocTot(CurrentUserInfo.CompanyID, documentID);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ApprovalState = dtr["ApprovalState"].ToString();
                    }
                }

                string cont = "";

                if (ApprovalState == "C")
                {
                    List<BE.DocumentInfo> list = dpDal.GetDocList(CurrentUserInfo.CompanyID, documentID);
                    foreach (BE.DocumentInfo info in list)
                    {
                        cont = info.ContentHTML;
                    }
                }
                else
                {
                    DAL.DocumentProcess dal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> list = dal.GetDocument(CurrentUserInfo.CompanyID, documentID);
                    foreach (BE.DocumentProcessInfo info in list)
                    {
                        cont = info.ContentHTML;
                    }
                }

                return cont;
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
