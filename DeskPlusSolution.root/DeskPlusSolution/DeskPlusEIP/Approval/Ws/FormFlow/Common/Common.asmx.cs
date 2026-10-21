using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common
{
    /// <summary>
    /// Common의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Common : Base.BaseWSApproval
    {
        #region GET

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
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			    //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
                this.GetResourceManager();

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
                    info.FileDel = resMng.GetString("Del");

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
        /// 특정파일삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelAttachFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int docID = GetDecryptParam("docID", "0").ToNumber<int>();
                int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

                BLL.AttachFile attBll = new BLL.AttachFile();
                if (attBll.Del(CurrentUserInfo.CompanyID, docID, attachFileID))
                {
                    return docID;
                }
                else
                {
                    return -1;
                }
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
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			    //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string documentID = GetDecryptParam("documentID", "0");

                ObjList listobj = new ObjList();

                DAL.BaseDocument dal = new DAL.BaseDocument();
                List<BE.BaseDocumentInfo> baseDocInfo = dal.GetBaseDocListsByPath(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());


                foreach (BE.BaseDocumentInfo _baseDocInfo in baseDocInfo)
                {
                    Zeospace.DeskPlus.Approval.BE.BaseDocumentInfo info = new BE.BaseDocumentInfo();

                    info.CompanyID = _baseDocInfo.CompanyID;
                    info.BaseDocumentID = _baseDocInfo.BaseDocumentID;
                    info.DocumentID = _baseDocInfo.DocumentID;
                    info.ParentDocumentID = _baseDocInfo.ParentDocumentID;
                    info.Name = _baseDocInfo.Name.ToReverseXSS();
                    info.ApprovalState = _baseDocInfo.ApprovalState;

                    info.DocNumber = _baseDocInfo.DocNumber;
                    info.Subject = _baseDocInfo.Subject.ToReverseXSS();
                    info.FormBase = GetLangName(_baseDocInfo.FormBase, _baseDocInfo.FormKo, _baseDocInfo.FormEn, _baseDocInfo.FormJa, _baseDocInfo.FormFr, _baseDocInfo.FormZhcn, _baseDocInfo.FormZhtw, PreLang);
                    info.NameBase = GetLangName(_baseDocInfo.NameBase, _baseDocInfo.NameKo, _baseDocInfo.NameEn, _baseDocInfo.NameJa, _baseDocInfo.NameFr, _baseDocInfo.NameZhcn, _baseDocInfo.NameZhtw, PreLang);
                    info.DeptBase = GetLangName(_baseDocInfo.DeptBase, _baseDocInfo.DeptKo, _baseDocInfo.DeptEn, _baseDocInfo.DeptJa, _baseDocInfo.DeptFr, _baseDocInfo.DeptZhcn, _baseDocInfo.DeptZhtw, PreLang);
                    info.TitleBase = GetLangName(_baseDocInfo.TitleBase, _baseDocInfo.TitleKo, _baseDocInfo.TitleEn, _baseDocInfo.TitleJa, _baseDocInfo.TitleFr, _baseDocInfo.TitleZhcn, _baseDocInfo.TitleZhtw, PreLang);
                    info.DutyBase = GetLangName(_baseDocInfo.DutyBase, _baseDocInfo.DutyKo, _baseDocInfo.DutyEn, _baseDocInfo.DutyJa, _baseDocInfo.DutyFr, _baseDocInfo.DutyZhcn, _baseDocInfo.DutyZhtw, PreLang);

                    info.DocDel = resMng.GetString("Del");

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
        /// 특정근거문서삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelBaseDoc(string p)
        {
            try
            {
                SetDecryptString(p);
                int docID = GetDecryptParam("docID", "0").ToNumber<int>();
                int baseDocID = GetDecryptParam("baseDocID", "0").ToNumber<int>();

                DAL.BaseDocument bDal = new DAL.BaseDocument();
                if (bDal.Del(CurrentUserInfo.CompanyID, docID, baseDocID))
                {
                    return docID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 자주지정하는결재자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavoriteApp(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            DAL.FavoriteApproval fDal = new DAL.FavoriteApproval();
            DataTableReader dtr = fDal.GetFavoriteList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.FavoriteApprovalInfo info = new BE.FavoriteApprovalInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.ApprovalDeptID = dtr["ApprovalDeptID"].ToString().ToNumber<int>();
                        info.FavoriteApprovalID = dtr["FavoriteApprovalID"].ToString().ToNumber<int>();

                        info.ApprovalUserID = dtr["ApprovalUserID"].ToString().ToNumber<int>();
                        info.ApprovalUserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        info.ApprovalUserDeptID = dtr["ApprovalUserDeptID"].ToString().ToNumber<int>();
                        info.ApprovalUserDeptName = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);

                        info.ApprovalUserTitleID = dtr["ApprovalUserTitleID"].ToString().ToNumber<int>();
                        info.ApprovalUserTitleName = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);

                        info.ApprovalUserDutyID = dtr["ApprovalUserDutyID"].ToString().ToNumber<int>();
                        info.ApprovalUserDutyName = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);

                        info.IsDept = dtr["IsDept"].ToString().ToNull();        // G, M
                        info.IsSub = dtr["IsSub"].ToString().ToNull();          // 1, 0
                        info.BaseDept = dtr["BaseDept"].ToString().ToNull();
                        info.BaseDeptStr = (!info.IsDept.Equals("G") && info.BaseDept.Equals("0")) ? " (" + GetResource("OrganAddtionJob").Trim() + ")" : "";

                        uobj.Add(info);
                    }
                }
            }

            return uobj;
        }

        /// <summary>
        /// 특정양식의 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFormSeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int formID = GetDecryptParam("formID", "0").ToNumber<int>();

            DAL.FormSeq fsDal = new DAL.FormSeq();
            List<BE.FormSeqInfo> list = fsDal.GetFormSeq(CurrentUserInfo.CompanyID, formID);

            List<object> uobj = new List<object>();
            if (list != null)
            {
                foreach (BE.FormSeqInfo info in list)
                {
                    BE.FormSeqInfo fInfo = info;
                    uobj.Add(fInfo);
                }
            }

            return uobj;
        }

        /// <summary>
        /// 서버시간 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetServerTime(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);

            List<object> uobj = new List<object>();
            DateTime dt = System.DateTime.Now;

            // getToDayByServer
            uobj.Add(dt.ToShortDateString());
            // getTimeByServer
            uobj.Add(dt.ToShortDateString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0'));
            // getMonthDayByServer
            uobj.Add(dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0'));
            // getTimeExByServer
            uobj.Add(dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0'));

            return uobj;
        }

        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.Sequence seqDal = new DAL.Sequence();
            DataTableReader dtr = seqDal.GetSeqLineTot(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.DeptBase = fInfo.NameBase;
                        }
                        else
                        {
                            fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        }
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }


        /// <summary>
        /// 부서정보 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfySeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.Sequence seqDal = new DAL.Sequence();
            DataTableReader dtr = seqDal.GetSeqModfyLineTot(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.NameBase = "";
                        }                        
                        fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }

        /// <summary>
        /// 미결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeqNonApp(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.Sequence seqDal = new DAL.Sequence();
            DataTableReader dtr = seqDal.GetSeqLineTotNonApp(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.DeptName = fInfo.NameBase;
                        }
                        else
                        {
                            fInfo.DeptName = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        }
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleName = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }


        /// <summary>
        /// 부서정보가 변경된 미결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfySeqNonApp(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.Common).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.Sequence seqDal = new DAL.Sequence();
            DataTableReader dtr = seqDal.GetSeqModfyLineTotNonApp(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.DeptName = fInfo.NameBase;
                        }
                        else
                        {
                            fInfo.DeptName = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        }
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleName = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }
        #endregion
    }
}
