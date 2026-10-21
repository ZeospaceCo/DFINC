using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Unit
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSPMS
    {
        /// <summary>
        /// 짧은댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            SetDecryptString(p);

            int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();

            ObjList listobj = new ObjList();

            DAL.UnitShortReply sr = new DAL.UnitShortReply();

            var  alList = sr.GetUnitShortReply(CurrentUserInfo.CompanyID, projectID, unitID);

            foreach (var be in alList)
            {
                Zeospace.DeskPlus.PMS.BE.UnitShortReplyInfo info = new UnitShortReplyInfo();

                info.CompanyID = be.CompanyID;
                info.ProjectID = be.ProjectID;
                info.UnitID = be.UnitID;
                info.UnitShortReplyID = be.UnitShortReplyID;
                info.Contents = be.Contents.Replace("\n", "<br>");
                info.DeptCodeID = be.DeptCodeID;
                info.NameBase = be.NameBase;
                info.NameKo = be.NameKo;
                info.NameEn = be.NameEn;
                info.NameJa = be.NameJa;
                info.NameFr = be.NameFr;
                info.NameZhcn = be.NameZhcn;
                info.NameZhtw = be.NameZhtw;
                info.ManyName = be.ManyName;
                info.WriteUserID = be.WriteUserID;
                info.WriteDate = be.WriteDate;               

                info.NameBase = this.GetLangName(
                    info.NameBase,
                    info.NameKo, info.NameEn, info.NameJa,
                    info.NameFr, info.NameZhcn, info.NameZhtw,
                    PreLang);

                info.IsAuth = (info.WriteUserID == CurrentUserInfo.UserID) ? "Y" : "N";

                DateTime dt;


                if (DateTime.TryParse(info.WriteDate.ToString(), out dt))
                    info.WriteDateString = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                else
                    info.WriteDateString = string.Empty;

                #region 날짜 비교(New Img)                
                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                TimeSpan gap = todayDate.Subtract(info.WriteDate);
                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                #endregion


                listobj.Add(info);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            try
            {

                int projectID = 0;
                int unitID = 0;

                SetDecryptString(p);

                projectID = GetDecryptParam("projectID", "0").ToNumber<Int32>();
                unitID = GetDecryptParam("unitID", "0").ToNumber<Int32>();


                string response = "0";

                DAL.UnitAttach dal = new DAL.UnitAttach();
                List<UnitAttachInfo> attachInfo = dal.GetAttachFile(CurrentUserInfo.CompanyID, projectID, unitID);

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.UnitAttachInfo _attachInfo in attachInfo)
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


        #region 댓글

        /// <summary>
        /// 짧은댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);

            int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            int unitShortReplyID = GetDecryptParam("unitShortReplyID", "0").ToNumber<int>();

            DAL.UnitShortReply biz = new DAL.UnitShortReply();
            var alList = biz.GetUnitShortReply(CurrentUserInfo.CompanyID, projectID, unitID, unitShortReplyID);

            foreach (var info in alList)
            {
                if (info.WriteUserID == CurrentUserInfo.UserID)
                {
                    BE.UnitShortReplyInfo be = new UnitShortReplyInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.ProjectID = projectID;
                    be.UnitID = unitID;
                    be.UnitShortReplyID = unitShortReplyID;
                    be.WriteUserID = CurrentUserInfo.UserID;

                    return biz.Del(be);
                }
            }

            return false;
        }

        #endregion

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
                string projectID = GetDecryptParam("projectID", "0");
                string unitID = GetDecryptParam("unitID", "0");

                ObjList listobj = new ObjList();

                Zeospace.DeskPlus.PMS.DAL.UnitAttach dal = new DAL.UnitAttach();
                List<BE.UnitAttachInfo> attachInfo = dal.GetAttachFile(CurrentUserInfo.CompanyID, projectID.ToNumber<int>(), unitID.ToNumber<int>());                

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.UnitAttachInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.PMS.BE.UnitAttachInfo info = new UnitAttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ProjectID = _attachInfo.ProjectID;
                    info.UnitID = _attachInfo.UnitID;
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
        #endregion
    }
}
