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
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {
        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetSeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

            ObjList listobj = new ObjList();
            DAL.DraftSequence seqDal = new DAL.DraftSequence();
            List<BE.DraftSequenceInfo> list = seqDal.GetDraftSequence(CurrentUserInfo.CompanyID, draftID);
            foreach (BE.DraftSequenceInfo info in list)
            {
                info.NameBase = GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                info.DeptBase = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                info.TitleBase = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                info.DutyBase = GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhcn, info.DutyZhtw, PreLang);

                listobj.Add(info);
            }

            return listobj;
        }


        /// <summary>
        /// 부서정보가 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetModfySeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

            ObjList listobj = new ObjList();
            DAL.DraftSequence seqDal = new DAL.DraftSequence();
            List<BE.DraftSequenceInfo> list = seqDal.GetDraftSequenceModfy(CurrentUserInfo.CompanyID, draftID);
            foreach (BE.DraftSequenceInfo info in list)
            {
                info.NameBase = GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                info.DeptBase = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                info.TitleBase = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                info.DutyBase = GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhcn, info.DutyZhtw, PreLang);

                listobj.Add(info);
            }

            return listobj;
        }

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
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary.Add).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.DraftFile dal = new DAL.DraftFile();
                List<BE.DraftFileInfo> list = dal.GetAttachFileList(CurrentUserInfo.CompanyID, draftID);

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.DraftFileInfo info in list)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    info.FileType = info.FileType.Replace(".", "");
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(info.FileLength) / 1024) + " KB";

                    string fileOk = "0";
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (info.FileType.ToLower() == allowedExtensions[i])
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
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary.Add).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.DraftBaseDoc dal = new DAL.DraftBaseDoc();
                List<BE.DraftBaseDocInfo> list = dal.GetBaseDocLists(CurrentUserInfo.CompanyID, draftID);

                foreach (BE.DraftBaseDocInfo info in list)
                {
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
        /// 첨부파일 삭제(임시서장)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFile(string p)
        {
            try
            {
                SetDecryptString(p);
                int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();
                int attachID = GetDecryptParam("attachID", "0").ToNumber<int>();
                
                if (draftID.Equals(0)||attachID.Equals(0)) return false;

                DAL.DraftFile dal = new DAL.DraftFile();

                List<BE.DraftFileInfo> list = dal.GetAttachFileList(CurrentUserInfo.CompanyID, draftID, attachID);
                foreach (BE.DraftFileInfo info in list)
                {
                    if (dal.Del(CurrentUserInfo.CompanyID, attachID))
                    {
                        File.Delete(Server.MapPath(info.FilePath + info.FileGUID));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 근거문서 삭제(임시서장)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseDoc(string p)
        {
            try
            {
                SetDecryptString(p);
                int draftID = GetDecryptParam("draftID", "0").ToNumber<int>();
                int baseID = GetDecryptParam("baseID", "0").ToNumber<int>();

                DAL.DraftBaseDoc dal = new DAL.DraftBaseDoc();
                return dal.Del(CurrentUserInfo.CompanyID, baseID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

    }
}
