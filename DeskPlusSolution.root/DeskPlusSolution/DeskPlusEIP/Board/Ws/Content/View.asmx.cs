using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
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
            string optionShortReplyGood = string.Empty;
            string optionShortReplyTop = string.Empty;
            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            optionShortReplyGood = GetDecryptParam("optionShortReplyGood", "0");
            optionShortReplyTop = GetDecryptParam("optionShortReplyTop", "0");

            ObjList listobj = new ObjList();

            DAL.ShortReply sr = new DAL.ShortReply();

            Hashtable ht = new Hashtable();

            int rowCount = 0;
            ArrayList bestList = new ArrayList();

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            DataTable dtList = sr.GetShortReplyDTList(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

            if (optionShortReplyGood == "1") // 댓글 추천/비추천 사용유무
            {
                #region 베스트댓글 관련

                DataTableReader dtrGood = dtList.CreateDataReader();
                if (dtrGood != null)
                {
                    if (dtrGood.HasRows)
                    {
                        while (dtrGood.Read())
                        {
                            int goodCount = dtrGood["GoodCount"].ToString().ToNumber<int>() - dtrGood["BadCount"].ToString().ToNumber<int>();
                            ht.Add(dtrGood["ShortReplyID"].ToString().ToNumber<int>(), goodCount);

                            rowCount++;
                        }
                    }
                }

                // 추천갯수-비추갯수 값이 가장 높은것이 베스트댓글
                // 동률일 경우 모두 베스트댓글, 정렬 순서는 작성시간 순
                // 댓글의 댓글이 베스트댓글인 경우 BEST 표시만
                int MaxValue = 0;
                foreach (DictionaryEntry de in ht)
                {
                    if (MaxValue <= (Int32)de.Value)
                    {
                        MaxValue = (Int32)de.Value;
                    }
                }

                // 추천/비추값이 아무것도 없을 경우 베스트댓글은 없음
                // 베스트댓글은 2건 이상의 댓글이 달렸을 경우에만
                if (MaxValue > 0 && rowCount >= 2)
                {
                    foreach (DictionaryEntry de in ht)
                    {
                        if (MaxValue == (Int32)de.Value)
                        {
                            bestList.Add((Int32)de.Key);
                        }
                    }
                }

                if (optionShortReplyTop == "1") // 추천 댓글을 최상위에 보여주기 사용유무
                {
                    // 최상위 베스트댓글 추가
                    DataTableReader dtrBest = dtList.CreateDataReader();
                    if (dtrBest != null)
                    {
                        string name = "";

                        if (dtrBest.HasRows)
                        {
                            while (dtrBest.Read())
                            {
                                foreach (Int32 id in bestList)
                                {
                                    if (dtrBest["ShortReplyID"].ToString().ToNumber<int>() == id && dtrBest["Active"].ToString() == "1")
                                    {
                                        Zeospace.DeskPlus.Board.BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                                        // Depth가 0인 댓글만 최상위로 노출
                                        if (dtrBest["Depth"].ToString().ToNumber<int>() == 0)
                                        {
                                            info.CompanyID = dtrBest["CompanyID"].ToString().ToNumber<int>();
                                            info.CategoryID = dtrBest["CategoryID"].ToString().ToNumber<int>();
                                            info.ListID = dtrBest["ListID"].ToString().ToNumber<int>();
                                            info.ShortReplyID = dtrBest["ShortReplyID"].ToString().ToNumber<int>();
                                            info.Content = dtrBest["Content"].ToString().ToReverseXSS().Replace("\n", "<br>");
                                            info.UserID = dtrBest["UserID"].ToString().ToNumber<int>();

                                            name = this.GetLangName(
                                                dtrBest["NameBase"].ToString(),
                                                dtrBest["NameKo"].ToString(), dtrBest["NameEn"].ToString(), dtrBest["NameJa"].ToString(),
                                                dtrBest["NameFr"].ToString(), dtrBest["NameZhCn"].ToString(), dtrBest["NameZhTw"].ToString(),
                                                PreLang);


                                            info.UserName = name;
                                            info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                                            DateTime dt;


                                            if (DateTime.TryParse(dtrBest["InputDate"].ToString(), out dt))
                                                info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                                            else
                                                info.InputDate = string.Empty;

                                            #region 날짜 비교(New Img)
                                            DateTime writeDate = Convert.ToDateTime(dtrBest["InputDate"].ToString());
                                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                            TimeSpan gap = todayDate.Subtract(writeDate);
                                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                            #endregion


                                            info.GroupID = dtrBest["GroupID"].ToString().ToNumber<int>();
                                            info.Depth = dtrBest["Depth"].ToString().ToNumber<int>();

                                            info.DepthDivStart = GetDivStart(info);

                                            info.Active = dtrBest["Active"].ToString();

                                            info.ParentCnt = sr.GetParentCount(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>(), info.GroupID, info.ShortReplyID);
                                            info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                                            info.GoodCount = dtrBest["GoodCount"].ToString().ToNumber<int>();
                                            info.BadCount = dtrBest["BadCount"].ToString().ToNumber<int>();
                                            info.IsBest = "1";
                                            info.IsTop = "1";

                                            listobj.Add(info);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            // 전체 댓글 리스트
            DataTableReader dtr = dtList.CreateDataReader();
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
                        info.Content = dtr["Content"].ToString().ToReverseXSS().Replace("\n", "<br>");
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

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                        #endregion


                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                        info.Depth = dtr["Depth"].ToString().ToNumber<int>();

                        info.DepthDivStart = GetDivStart(info);

                        info.Active = dtr["Active"].ToString();

                        info.ParentCnt = sr.GetParentCount(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>(), info.GroupID, info.ShortReplyID);
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.GoodCount = dtr["GoodCount"].ToString().ToNumber<int>();
                        info.BadCount = dtr["BadCount"].ToString().ToNumber<int>();

                        foreach (Int32 id in bestList)
                        {
                            if (dtr["ShortReplyID"].ToString().ToNumber<int>() == id)
                            {
                                info.IsBest = "1";
                            }
                        }

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 게시판 제목 스타일 입히기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string GetDivStart(BE.ShortReplyInfo _info)
        {
            int gap = 0;
            for (int i = 0; i <= _info.Depth; i++)
            {
                if (i > 1)
                {
                    gap = gap + 24;
                }
            }

            string space = "<div style=\"padding: 0 0 0 " + gap + "px\">";

            return space;
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

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

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

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            DAL.ShortReply biz = new DAL.ShortReply();
            BE.ShortReplyInfo info = biz.GetShortReplyListID(boaComID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(boaComID,
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