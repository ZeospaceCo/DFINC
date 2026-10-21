using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;


namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Apps.Board
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 첨부파일 가져오기
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

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.BoaAttach dal = new DAL.BoaAttach();
                List<BE.BoaAttachInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, communityID, menuID, boardID);

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BoaAttachInfo _attachInfo in attachInfo)
                {
                    BE.BoaAttachInfo info = new BoaAttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.CommunityID = _attachInfo.CommunityID;
                    info.MenuID = _attachInfo.MenuID;
                    info.BoardID = _attachInfo.BoardID;

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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 짧은댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.BoaShortReply dal = new DAL.BoaShortReply();
                DataTableReader dtr = dal.GetShortReplyList(CurrentUserInfo.CompanyID, communityID, menuID, boardID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoaShortReplyInfo info = new BoaShortReplyInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                            info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Content = dtr["Content"].ToString().Replace("\n", "<br>");

                            info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            info.UserName = GetLangName(dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(),
                                dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(),
                                dtr["NameZhcn"].ToString(),
                                dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            if (info.UserName.Trim() == "")
                            {
                                info.UserName = dtr["UserName"].ToString();
                            }

                            listobj.Add(info);
                        } 
                    }
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
        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 짧은 댓글 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                DAL.BoaShortReply dal = new DAL.BoaShortReply();

                if (userID == CurrentUserInfo.UserID)
                {
                    return dal.Del(CurrentUserInfo.CompanyID, communityID, menuID, boardID, shortReplyID, CurrentUserInfo.UserID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 이슈지정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddIssue(string p)
        {
            try 
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                DAL.BoaIssue dal = new DAL.BoaIssue();

                return dal.Add(CurrentUserInfo.CompanyID, communityID, menuID, boardID);             
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 추천하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddRecommend(string p)
        {
            try 
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                int grade = GetDecryptParam("grade", "10").ToNumber<int>();                

                DAL.BoaRecom dal = new DAL.BoaRecom();
                int CheckPoint = dal.Add(CurrentUserInfo.CompanyID, communityID, menuID, boardID, CurrentUserInfo.UserID, CurrentUserInfo.Name);

                if ((CheckPoint == 0) && (grade != 10))
                {
                    // 1.글쓰기 2.조회 3.추천 4.짧은답글 5.답변 6.방문
                    BE.PointInfo pInfo = new PointInfo();
                    pInfo.CompanyID = CurrentUserInfo.CompanyID;
                    pInfo.CommunityID = communityID;
                    pInfo.UserID = CurrentUserInfo.UserID;
                    pInfo.Type = 3;
                    pInfo.Point = 0;

                    DAL.Point dalPoint = new DAL.Point();
                    dalPoint.Add(pInfo, menuID);
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
