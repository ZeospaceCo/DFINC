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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Process.Basic
{
    /// <summary>
    /// Approve의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Approve : Base.BaseWSApproval
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
                string documentID = GetDecryptParam("documentID", "0");

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
                    //info.FileCnt = attachInfo.Count;

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
                string documentID = GetDecryptParam("documentID", "0");

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

                string documentID = GetDecryptParam("documentID", "0");

                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

                string response = "0";

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
                string documentID = GetDecryptParam("documentID", "0");

                ObjList listobj = new ObjList();

                DAL.BaseDocument dal = new DAL.BaseDocument();
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());


                foreach (BE.BaseDocumentInfo _baseDocInfo in baseDocInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.BaseDocumentInfo info = new BE.BaseDocumentInfo();

                    info.CompanyID = _baseDocInfo.CompanyID;
                    info.DocumentID = _baseDocInfo.DocumentID;
                    info.ParentDocumentID = _baseDocInfo.ParentDocumentID;
                    info.Name = _baseDocInfo.Name.ToReverseXSS();
                    info.Subject = _baseDocInfo.Subject.ToReverseXSS();

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
                string documentID = GetDecryptParam("documentID", "0");

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
                string documentID = GetDecryptParam("documentID", "0");

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

        #region 인장
        /// <summary>
        /// 인장경로 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSealPath(string p)
        {
            try
            {
                string sealpath = "";

                Zeospace.DeskPlus.Account.DAL.User uDAL = new Account.DAL.User();
                List<Account.BE.UserInfo> userlist = uDAL.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                foreach (Account.BE.UserInfo info in userlist)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string path = baseFileDirectory + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Picture/" + info.SealImage;
                    if (File.Exists(Server.MapPath(path)))
                    {
                        sealpath = FullDomainPath + path;
                    }
                }
                return sealpath;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region ModViewing
        /// <summary>
        /// 해당 결재 결재페이지 viewing처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetViewing(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
                string approvalTag = GetDecryptParam("approvalTag", "");
                string viewing = GetDecryptParam("viewing", "");

                DAL.Sequence seqDal = new DAL.Sequence();
                return seqDal.ModViewing(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, viewing).ToString();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 해당 결재 결재페이지 접근가능여부 확인
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetViewing(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
                string approvalTag = GetDecryptParam("approvalTag", "");
                int userID = GetDecryptParam("targetUserID", "0").ToNumber<int>();

                BLL.Sequence seqBll = new BLL.Sequence();
                BE.SequenceInfo info = seqBll.GetViewing(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, userID);

                string rtnval = "";
                if (info != null)
                {
                    rtnval = (info.IsViewing.Equals("N")) ? "OK" : GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang) + "/" + GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang) + "/" + GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region 문서결재라인 정보
        /// <summary>
        /// 결재취소 대상 결재라인 가져오길
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeqLine(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();
            int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            string approvalTag = GetDecryptParam("approvalTag", "");

            if (docID.Equals(0) || sequence.Equals(0) || approvalTag.Equals(""))
            {
                return null;
            }

            DAL.Sequence seqDal = new DAL.Sequence();
            List<BE.SequenceInfo> seqLine = seqDal.GetSeqLine(CurrentUserInfo.CompanyID, docID).Where(x => x.Sequence >= sequence).ToList<BE.SequenceInfo>();

            List<object> uobj = new List<object>();
            foreach (BE.SequenceInfo info in seqLine)
            {
                if (!info.Sequence.Equals(sequence))
                {
                    uobj.Add(info); //결재취소 시, 이후 결재자 태그 초기화
                }
                else
                {
                    if (info.ApprovalTag.Equals(approvalTag))
                    {
                        uobj.Add(info); //결재취소 시, 합의의 경우, 본인만 초기화
                    }
                }
            }

            return uobj;
        }
        #endregion

        #region 상신보류 상태 정보
        /// <summary>
        /// 상신보류 상태 체크
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetApprovalState(string p)
        {
            SetDecryptString(p);
            int docID = GetDecryptParam("documentID", "0").ToNumber<int>();

            string result = "0";

            if (docID.Equals(0))
            {
                return result;
            }

            //문서정보가져오기
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, docID);

            foreach (BE.DocumentProcessInfo info in list)
            {
                if (info.ApprovalState == "H")
                {
                    result = "1";
                }
            }

            return result;
        }
        #endregion

        #region 현재 결재순서 여부 확인
        /// <summary>
        /// 현재 결재순서 여부 확인
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetIsMyTurn(string p)
        {
            SetDecryptString(p);
            int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            string approvalTag = GetDecryptParam("approvalTag", "");
            int targetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();

            string result = "N";

            DAL.Sequence seqDal = new DAL.Sequence();
            result = seqDal.GetIsMyTurn(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, targetUserID);

            return result;
        }
        #endregion
    }
}
