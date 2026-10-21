using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.SNS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.SNS.Web.Ws.Common
{
    /// <summary>
    /// BoardTalk의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class BoardTalk : Base.BaseWSWorkTalk
    {
        #region ADD
        #region 댓글

        /// <summary>
        /// 댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int Add(string p)
        {

            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string contents = GetDecryptParam("contents", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            BE.BoardTalkInfo info = new BE.BoardTalkInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            BLL.BoardTalk dal = new BLL.BoardTalk();
            dal.Add(info, CurrentUserInfo.PreferredLanguage);

            return 1;
        }

        #endregion
        #endregion

        #region DEL
        #region 댓글
        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("ShortReplyID", "0");

            DAL.BoardTalk biz = new DAL.BoardTalk();
            BE.BoardTalkInfo info = biz.GetShortReplyListID(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(CurrentUserInfo.CompanyID,
                    categoryID.ToNumber<int>(),
                    listID.ToNumber<int>(),
                    shortReplyID.ToNumber<int>());
            }
            else
            {
                return false;
            }

        }
        #endregion
        #endregion


        #region 가져오기
        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 용도 : 투표 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.BoardTalk biz = new DAL.BoardTalk();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), setPageingStr[0], setPageingStr[1]);

            if (dtr != null)
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.SNS.BE.BoardTalkInfo info = new BE.BoardTalkInfo();
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

                        #region 사진 이미지 설정
                        string photoName = dtr["PhotoImage"].ToString();
                        string BaseImgPath = "";
                        string imgTag = "";
                        string encParams = "";
                        bool response = false;

                        Dictionary<string, string> paraDic = null;

                        if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                        {
                            BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                            BaseImgPath = string.Format(BaseImgPath, CurrentUserInfo.CompanyID, info.UserID);

                            if (System.IO.File.Exists(Server.MapPath(BaseImgPath))) response = true;
                        }

                        if (response == false) BaseImgPath = baseImageDirectory + "C/Img/Icon/ico_noimg.gif";

                        if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                        {
                            #region 스크린샷
                            paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", BaseImgPath);
                            paraDic.Add("thumnail", "Y");
                            paraDic.Add("thumWidth", "40");
                            paraDic.Add("thumHeight", "55");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='40' />";
                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

                        #endregion


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


                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 용도 : 투표 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");

            DAL.BoardTalk biz = new DAL.BoardTalk();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
        }
        #endregion
    }
}
