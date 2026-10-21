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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Board
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
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

            ObjList listobj = new ObjList();

            DAL.BoardShortReply sr = new DAL.BoardShortReply();

            DataTableReader dtr = sr.GetBoardShortReply(CurrentUserInfo.CompanyID, projectID, menuID, boardID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.PMS.BE.BoardShortReplyInfo info = new BoardShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                        info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                        info.BoardShortReplyID = dtr["BoardShortReplyID"].ToString().ToNumber<int>();

                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        info.Contents = dtr["Contents"].ToString().Replace("\n", "<br>");

                        info.NameBase = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.IsAuth = (info.WriteUserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;


                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            info.WriteDateString = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDateString = string.Empty;

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

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
            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Ws.Board.View).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.BoardAttach dal = new DAL.BoardAttach();
                List<BE.BoardAttachInfo> attachInfo = dal.GetAttachFile(CurrentUserInfo.CompanyID, projectID, menuID, boardID);

                foreach (BE.BoardAttachInfo _attachInfo in attachInfo)
                {
                    BE.BoardAttachInfo info = new BE.BoardAttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ProjectID = _attachInfo.ProjectID;
                    info.MenuID = _attachInfo.MenuID;
                    info.BoardID = _attachInfo.BoardID;
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
                    info.FileDel = resMng.GetString("FileDel");

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

        #region DEL()

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
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            int boardShortReplyID = GetDecryptParam("boardShortReplyID", "0").ToNumber<int>();

            DAL.BoardShortReply biz = new DAL.BoardShortReply();
            List<BoardShortReplyInfo> alList = biz.GetBoardShortReply(CurrentUserInfo.CompanyID, projectID, menuID, boardID, boardShortReplyID);

            foreach(var info in alList)
            {
                if (info.WriteUserID == CurrentUserInfo.UserID)
                {

                    BE.BoardShortReplyInfo be = new BoardShortReplyInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.ProjectID = projectID;
                    be.MenuID = menuID;
                    be.BoardID = boardID;
                    be.BoardShortReplyID = boardShortReplyID;
                    be.WriteUserID = CurrentUserInfo.UserID;

                    return biz.Del(be);
                }
            }

            return false;

        }

        #endregion

        #endregion
    }
}
