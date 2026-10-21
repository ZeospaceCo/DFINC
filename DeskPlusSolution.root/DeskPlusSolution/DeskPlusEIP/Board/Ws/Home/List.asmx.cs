using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Home
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSBoard
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string menuTab = GetDecryptParam("menuTab", "");
                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();

                List<BE.ListViewModel> alList = new List<BE.ListViewModel>();
                BLL.HomeList biz = new BLL.HomeList();
                switch (menuTab)
                {
                    case "RecentWrite": //최신글
                        alList = biz.GetRecentWrite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MaxShort": //최다댓글
                        alList = biz.GetMaxShort(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MaxRead": //최다조회
                        alList = biz.GetMaxRead(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MaxRate": //평가우수
                        alList = biz.GetMaxRate(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MyWrite": //등록한 게시글
                        alList = biz.GetMyWrite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MyListByShort": //댓글 단 게시글
                        alList = biz.GetMyListByShort(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MyShort": //등록한 댓글
                        alList = biz.GetMyShort(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    case "MyImpo": //중요표시
                        alList = biz.GetMyImpo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;

                    default:
                        alList = biz.GetRecentWrite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize);
                        break;
                }

                List<BE.ListViewModel> objList = new List<BE.ListViewModel>();
                foreach (var info in alList) objList.Add(TitleCustom(info));
                alList = null;

                return JsonConvert.SerializeObject(objList);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 제목 칼라, 색깔 설정하기
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        private BE.ListViewModel TitleCustom(BE.ListViewModel info)
        {
            string headerName = this.GetLangName(
                                    info.HeaderBase.ToNull(), info.HeaderKo.ToNull(),
                                    info.HeaderEn.ToNull(), info.HeaderJa.ToNull(),
                                    info.HeaderFr.ToNull(), info.HeaderZhcn.ToNull(),
                                    info.HeaderZhtw.ToNull(), PreLang);

            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

            //말머리
            info.Title = (info.HeaderName.Length > 0) ? info.HeaderName + " " + info.Title : info.Title;

            //글자 폰트 크기 및 색깔 설정하기
            if (info.Etc2.ToNull().Length > 0)
            {
                string[] etcStr = info.Etc2.Split(',');
                StringBuilder sb = new StringBuilder();
                if (etcStr[2].ToString() != "0") sb.Append("<b>");
                if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                sb.Append(info.Title);
                if (etcStr[0].ToString() != "") sb.Append("</font>");
                if (etcStr[1].ToString() != "") sb.Append("</font>");
                if (etcStr[2].ToString() != "0") sb.Append("</b>");

                info.Title = sb.ToString();
            }

            if (info.MobileContent.Length > 55)
            {
                info.MobileContent = info.MobileContent.Substring(0, 55) + "....";
            }

            #region 이름 다국어 설정

            info.WriteUserName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

            #endregion 이름 다국어 설정

            if (info.OptionAnonymity == "1") //익명글
            {
                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                info.WriteUserName = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                info.WriteUserID = 0;
            }

            //조회하지 않으면 굵게 처리
            if (info.ReadCount == 0) info.Title = "<b>" + info.Title + "</b>";

            DateTime dt;

            if (DateTime.TryParse(info.WriteDateDT.ToString(), out dt))
                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
            else
                info.WriteDate = string.Empty;

            return info;
        }

        /// <summary>
        /// 사용자별 선택 탭
        /// </summary>
        /// <param name="p"></param>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModUserTab(string p)
        {
            try
            {
                SetDecryptString(p);                
                int startTabID = GetDecryptParam("startTabID", "0").ToNumber<int>();

                DAL.HomeList dal = new DAL.HomeList();

                return (dal.ModUserTab(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, startTabID));                
                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
            


        }

    }

    /// <summary>
    /// 포틀릿 객체
    /// </summary>
    public class HomeeEntity
    {
        public int CompanyID { get; set; }
        public int CategoryID { get; set; }
        public int ListID { get; set; }
        public string Title { get; set; }
        public string MobileContent { get; set; }
        public string WriteDate { get; set; }
        public bool IsNew { get; set; } //최신글여부
    }
}