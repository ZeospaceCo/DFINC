using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Statistic
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSBoard
    {
        #region 댓글

        /// <summary>
        /// 댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");

            ObjList listobj = new ObjList();

            DAL.ShortReply sr = new DAL.ShortReply();

            DataTableReader dtr = sr.GetShortReplyList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
            if (dtr != null)
            {
                string name = "";

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Board.BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.UserName = name;
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;

                        if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                            info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.InputDate = string.Empty;

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        #endregion 댓글

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
                string categoryID = GetDecryptParam("categoryID", "0");
                string listID = GetDecryptParam("listID", "0");
                string screenShot = GetDecryptParam("screenShot", "");

                ObjList listobj = new ObjList();

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

                string fileName = (screenShot.Length > 0) ? System.IO.Path.GetFileName(Server.MapPath(screenShot)).ToLower().Replace("thum_", "") : ""; //파일명

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Board.BE.AttachFileInfo info = new BE.AttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.CategoryID = _attachInfo.CategoryID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    //guid 가 없다는 것은 마이그래이션을 해서 그러함으로, filename 을 참조할 수도 있다.
                    info.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : info.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    //스크린샷 파일과 동일 여부 찾기
                    info.ScreenShot = (fileName == info.FileName.ToLower() || fileName == info.FileGUID.ToLower()) ? "1" : "0";

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

        #endregion 첨부파일

        #region DEL()

        #region 댓글

        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("ShortReplyID", "0");
            string groupID = GetDecryptParam("groupID", "0");

            DAL.ShortReply biz = new DAL.ShortReply();
            BE.ShortReplyInfo info = biz.GetShortReplyListID(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(CurrentUserInfo.CompanyID,
                    categoryID.ToNumber<int>(),
                    listID.ToNumber<int>(),
                    shortReplyID.ToNumber<int>(),
                    groupID.ToNumber<int>());
            }
            else
            {
                return false;
            }
        }

        #endregion 댓글

        #endregion DEL()
    }
}