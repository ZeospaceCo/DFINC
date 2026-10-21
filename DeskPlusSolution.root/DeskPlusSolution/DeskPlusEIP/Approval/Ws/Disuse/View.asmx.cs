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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Disuse
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
                List<BE.AttachFileInfo> attachInfo = dal.GetDisuseAttachFileListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

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

                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

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
                return dal.GetDisuseAttachFileCntByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());
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

                int documentID = GetDecryptParam("childDocID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> attachInfo = dal.GetDisuseAttachFileListsByPath(CurrentUserInfo.CompanyID, documentID);

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
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetDisuseBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());


                foreach (BE.BaseDocumentInfo _baseDocInfo in baseDocInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.BaseDocumentInfo info = new BE.BaseDocumentInfo();

                    info.CompanyID = _baseDocInfo.CompanyID;
                    info.DocumentID = _baseDocInfo.DocumentID;
                    info.ParentDocumentID = _baseDocInfo.ParentDocumentID;
                    info.Name = _baseDocInfo.Name;
 
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
                return dal.GetDisuseBaseDocCntByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());
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
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetDisuseBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

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

        #region 문서복원

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-26
        /// 작성목적 : 폐기 문서 복원하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool RestoreDoc(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                DAL.Document bizDoc = new DAL.Document();

                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();

                return bizDoc.DisuseDoc(tmpCompanyID, CurrentUserInfo.CompanyID, docID.ToNumber<int>(), CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-26
        /// 작성목적 : 폐기 문서 복원하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool RestoreDocList(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    DAL.Document bizDoc = new DAL.Document();

                    int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();

                    foreach (string doc in docID.Split(','))
                    {
                        bizDoc.DisuseDoc(tmpCompanyID, CurrentUserInfo.CompanyID, doc.ToNumber<int>(), CurrentUserInfo.UserID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-13
        /// 작성목적 : 연관되어 있는 완결 문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDoc(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                BLL.Document bizDoc = new BLL.Document();

                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                bizDoc.DelDoc(tmpCompanyID, docID.ToNumber<int>(), true);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-13
        /// 작성목적 : 해당 완결 문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocID(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                BLL.Document bizDoc = new BLL.Document();

                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                bizDoc.DelDocID(tmpCompanyID, docID.ToNumber<int>(), true);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 완결 문서 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocList(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    BLL.Document bizDoc = new BLL.Document();

                    int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();

                    foreach (string doc in docID.Split(','))
                    {
                        bizDoc.DelDocList(tmpCompanyID, doc.ToNumber<int>(), true);
                    }
                }

                return true;

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
