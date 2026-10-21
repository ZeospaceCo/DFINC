using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MyComment : Base.BaseWSPortlet
    {
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
        /// 댓글 리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMyComment(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.MyComment).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            //파라미터 가져오기
            SetDecryptString(p);
            int PageSize;
            int PageSizePageNo;
            int TotalCount;

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            try
            {
                List<object> list = new List<object>();

                string rtnSetPaging = SetPaging(TotalCount, PageSize, PageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                                
                Zeospace.DeskPlus.SNS.DAL.MyTalk biz = new SNS.DAL.MyTalk();
                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],"", "", "", "", "", CurrentUserInfo.UserID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string tmpContent = String.Empty;
                        string rname = String.Empty;
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.SNS.BE.MyTalkInfo info = new SNS.BE.MyTalkInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.MyTalkID = dtr["MyTalkID"].ToString().ToNumber<int>();

                            tmpContent = dtr["Content"].ToString();
                            if (tmpContent.Length > 20) tmpContent = Utility.CutString(tmpContent, 50) + "...";
                            info.Content = tmpContent.Replace("\n", "<br>");
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.RUserID = dtr["RUserID"].ToString().ToNumber<int>(); //추가
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>(); //추가
                            info.IsSecurity = dtr["IsSecurity"].ToString();//추가

                            info.UserName = this.GetLangName(
                                                    dtr["NameBase"].ToString(),
                                                    dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                                    dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                                    PreLang);

                            info.RUserName = this.GetLangName(
                                    dtr["RNameBase"].ToString(),
                                    dtr["RNameKo"].ToString(), dtr["RNameEn"].ToString(), dtr["RNameJa"].ToString(),
                                    dtr["RNameFr"].ToString(), dtr["RNameZhCn"].ToString(), dtr["RNameZhTw"].ToString(),
                                    PreLang);

                            /*
                            //추가
                            if ((info.GroupID == info.ShortReplyID) && (info.RUserID != CurrentUserInfo.UserID))
                            {
                                rname = this.GetLangName(
                                    dtr["RNameBase"].ToString(),
                                    dtr["RNameKo"].ToString(), dtr["RNameEn"].ToString(), dtr["RNameJa"].ToString(),
                                    dtr["RNameFr"].ToString(), dtr["RNameZhCn"].ToString(), dtr["RNameZhTw"].ToString(),
                                    PreLang);

                                if (!String.IsNullOrEmpty(rname))
                                {
                                    info.RUserName = "[" + rname + "]";
                                }
                                else
                                {
                                    info.RUserName = "";
                                }
                            }
                            else
                            {
                                info.RUserName = "";
                            }
                            */

                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>() - 1;

                            // 원본글 작성자와 댓글러가 같은지
                            if (CurrentUserInfo.UserID == info.RUserID)
                            {
                                info.View = "Y";
                            }
                            else if (CurrentUserInfo.UserID == info.UserID && info.IsSecurity == "1")
                            {
                                info.View = "Y";
                            }
                            else if (info.IsSecurity == "0")
                            {
                                info.View = "Y";
                            }
                            else
                            {
                                info.View = "N";
                                info.Content = resMng.GetString("SecurityMsg");
                            }

                            /*
                            if (info.GroupID != info.ShortReplyID)
                            {                                
                                info.Depth = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";
                            }
                            else
                            {
                                info.Depth = "";
                            }
                            */

                            info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                            #region 등록자 프로필 사진 이미지 설정
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
                                encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                imgTag = "<img src='{0}' width='40' />";
                                imgTag = string.Format(imgTag, BaseImgPath);
                            }

                            info.PhotoImage = imgTag;

                            #endregion

                            #region 등록한 사진 이미지 설정
                            info.FileCnt = dtr["FileCnt"].ToString().ToNumber<int>();

                            if ((info.GroupID == info.MyTalkID) && (info.FileCnt > 0))
                            {
                                Zeospace.DeskPlus.SNS.DAL.Attach dal = new Zeospace.DeskPlus.SNS.DAL.Attach();
                                DataSet dsAttach = dal.GetList(CurrentUserInfo.CompanyID, info.GroupID);

                                string fileTag = "";
                                string fileTagStr = "";

                                foreach (DataRow drAttach in dsAttach.Tables[0].Rows)
                                {
                                    string BasefilePath = drAttach["FilePath"].ToString() + "Thum_" + drAttach["FileGUID"].ToString();
                                    string encParams1 = "";

                                    Dictionary<string, string> paraDic1 = null;

                                    if (System.IO.File.Exists(Server.MapPath(BasefilePath)))
                                    {
                                        #region 스크린샷
                                        paraDic1 = new Dictionary<string, string>();
                                        paraDic1.Add("filePath", BasefilePath);
                                        encParams1 = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic1)));
                                        BasefilePath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams1;
                                        #endregion

                                        fileTag = "<img src='{0}' width='200' />";
                                        fileTagStr += string.Format(fileTag, BasefilePath);
                                    }
                                }

                                info.FileImage = fileTagStr;
                            }
                            else
                            {
                                info.FileImage = "";
                            }

                            #endregion

                            DateTime dt;

                            if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                                info.WriteDateYMD = Convert.ToDateTime(dtr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dtr["InputDate"]).ToShortTimeString();
                            else
                                info.WriteDateYMD = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion                            

                            list.Add(info);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyComment : " + ex);
                throw;
            }
        }
    }

}
