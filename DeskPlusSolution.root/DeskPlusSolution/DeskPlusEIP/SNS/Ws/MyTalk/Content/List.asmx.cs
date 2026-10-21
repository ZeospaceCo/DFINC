using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlus.SNS.Web.Ws.MyTalk.Content
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSMyTalk
    {
        #region GET()

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
        /// 작성일 : 2013-05-13
        /// 작성목적 : 담벼락글 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.Ws.MyTalk.Content.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            int rUserID = 0;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string searchTerm = string.Empty;
            string searchTermValue = string.Empty;
            string searchField = string.Empty;
            string searchValue = string.Empty;
            string order = string.Empty;

            SetDecryptString(p);
            rUserID = GetDecryptParam("rUserID", "0").ToNumber<int>();
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            searchTerm = GetDecryptParam("searchTerm", "");
            searchTermValue = GetDecryptParam("searchTermValue", "");

            searchField = GetDecryptParam("searchField", "");
            searchValue = GetDecryptParam("searchValue", "");
            order = GetDecryptParam("order", "");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            #region 날짜

            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (searchTerm == "week" || searchTerm == "month")
            {
                if (searchTerm == "week")
                    getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                else if (searchTerm == "month")
                    getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (searchTerm == "period")
            {
                startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            DAL.MyTalk biz = new DAL.MyTalk();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, rUserID, setPageingStr[0], setPageingStr[1], startDate, endDate,
                    searchField, searchValue, order, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";
                    string rname = "";

                    while (dtr.Read())
                    {
                        BE.MyTalkInfo info = new BE.MyTalkInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.MyTalkID = dtr["MyTalkID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.IsSecurity = dtr["IsSecurity"].ToString();
                        info.RUserID = dtr["RUserID"].ToString().ToNumber<int>();
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>(); // 추가

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);


                        info.UserName = name;

                        rname = this.GetLangName(
                                dtr["RNameBase"].ToString(),
                                dtr["RNameKo"].ToString(), dtr["RNameEn"].ToString(), dtr["RNameJa"].ToString(),
                                dtr["RNameFr"].ToString(), dtr["RNameZhCn"].ToString(), dtr["RNameZhTw"].ToString(),
                                PreLang);

                        info.RUserName = rname;

                        info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>() - 1;

                        if (info.ReplyCount > 0)
                        {
                            info.ReplyCountStr = this.GetResource("ShortReply") + "(" + info.ReplyCount + ")";
                        }
                        else
                        {
                            info.ReplyCountStr = "";
                        }         

                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.View = "N";

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

                        /*
                        if (info.GroupID != info.MyTalkID)
                        {
                            info.Depth = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";
                        }
                        else
                        {
                            info.Depth = "";
                        }
                        */

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
                            paraDic.Add("thumWidth", "55");
                            paraDic.Add("thumHeight", "72");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='55' />";                          

                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

                        #endregion

                        #region 등록한 사진 이미지 설정
                        info.FileCnt = dtr["FileCnt"].ToString().ToNumber<int>();

                        if ((info.GroupID == info.MyTalkID) && (info.FileCnt > 0))
                        {
                            DAL.Attach dal = new DAL.Attach();
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

                                    fileTag = "<img src='{0}' width='250' />";  
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

                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일 : 2013-05-13
        /// 용도 : 담벼락글 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            SetDecryptString(p);
            int rUserID = GetDecryptParam("rUserID", "0").ToNumber<int>();
            string searchTerm = GetDecryptParam("searchTerm", "");
            string searchTermValue = GetDecryptParam("searchTermValue", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

            #region 날짜

            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (searchTerm == "week" || searchTerm == "month")
            {
                if (searchTerm == "week")
                    getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                else if (searchTerm == "month")
                    getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (searchTerm == "period")
            {
                startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            DAL.MyTalk biz = new DAL.MyTalk();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, rUserID, startDate, endDate, searchField, searchValue, CurrentUserInfo.UserID);
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-06-17
        /// 작성목적 : 담벼락글의 댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReply(string p)
        {
            string groupID = string.Empty;            

            SetDecryptString(p);
            groupID = GetDecryptParam("groupID", "0");
         
            ObjList listobj = new ObjList();
         
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            DAL.MyTalk biz = new DAL.MyTalk();
            DataTableReader dtr = biz.GetShortReply(CurrentUserInfo.CompanyID, groupID.ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";
                    string rname = "";

                    while (dtr.Read())
                    {
                        BE.MyTalkInfo info = new BE.MyTalkInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.MyTalkID = dtr["MyTalkID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.IsSecurity = dtr["IsSecurity"].ToString();
                        info.RUserID = dtr["RUserID"].ToString().ToNumber<int>();
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>(); // 추가

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);


                        info.UserName = name;

                        rname = this.GetLangName(
                                dtr["RNameBase"].ToString(),
                                dtr["RNameKo"].ToString(), dtr["RNameEn"].ToString(), dtr["RNameJa"].ToString(),
                                dtr["RNameFr"].ToString(), dtr["RNameZhCn"].ToString(), dtr["RNameZhTw"].ToString(),
                                PreLang);

                        info.RUserName = rname;

                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.View = "N";

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

                        info.Depth = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";                       

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
                            paraDic.Add("thumWidth", "32");
                            paraDic.Add("thumHeight", "40");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='32' />";

                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

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

                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }


            return listobj;
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일 : 2013-06-10
        /// 작성목적 : 마이톡 쓰기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddToMyTalk(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.Ws.MyTalk.Content.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string jobDate = GetDecryptParam("jobDate", "");
            string contents = GetDecryptParam("contents", "");
            string rUserID = GetDecryptParam("rUserID", "");
            string security = GetDecryptParam("security", "0");
            string groupID = GetDecryptParam("groupID", "0");
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");
            string preTitle = string.Empty;
            bool check = false;

            BE.MyTalkInfo info = new BE.MyTalkInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;
            info.MyDeptName = CurrentUserInfo.DeptName;
            info.RUserID = rUserID.ToNumber<int>();
            info.JobDate = DateHelpers.GetCultureDate(jobDate, "yyyyMMdd", PreLangReal);
            info.IsSecurity = security;
            info.GroupID = groupID.ToNumber<int>();
            info.WriteDateYM = FileHelper.GetYearMonth();

            preTitle = resMng.GetString("ShortReply") + "," + resMng.GetString("NoteAlarm");

            // 자신이 댓글을 추가했을 경우
            if (info.RUserID == CurrentUserInfo.UserID)
                check = true;

            BLL.MyTalk dal = new BLL.MyTalk();
            return dal.Add(info, CurrentUserInfo.PreferredLanguage, preTitle, check, swfUploadGUID);
            
        }      

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 폴더별 사용자 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModFolderUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "M");

                BE.FolderUserInfo info = new BE.FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Types = types;
                info.PageSize = pageSize.ToNumber<int>();

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion 

        #region DEL()

        #region 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 마이톡 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMyTalk(string p)
        {
            try
            {
                SetDecryptString(p);
                string MyTalkID = GetDecryptParam("myTalkID", "0");

                DAL.MyTalk dal = new DAL.MyTalk();
                return dal.Del(CurrentUserInfo.CompanyID, MyTalkID.ToNumber<int>(), CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 마이톡 그룹 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelGroupMyTalk(string p)
        {
            try
            {
                SetDecryptString(p);
                string groupID = GetDecryptParam("groupID", "0");

                // File 삭제 
                DAL.Attach dalAtt = new DAL.Attach();
                DataSet dsAttach = dalAtt.GetList(CurrentUserInfo.CompanyID, groupID.ToNumber<int>());

                string filePath = "";
                foreach (DataRow dr in dsAttach.Tables[0].Rows)
                {
                    filePath = dr["FilePath"].ToString();
                }

                if (filePath != "")
                {
                    if (Directory.Exists(Server.MapPath(filePath))) Directory.Delete(Server.MapPath(filePath), true);
                }

                // DB 삭제
                DAL.MyTalk dal = new DAL.MyTalk();
                return dal.DelGroupID(CurrentUserInfo.CompanyID, groupID.ToNumber<int>());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-03-13
        /// 작성목적 : 마이톡 그룹 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSpoolFile(string p)
        {
            try
            {
                SetDecryptString(p);
                string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

                // File 삭제 
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}";
                filePath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

                if (Directory.Exists(Server.MapPath(filePath))) Directory.Delete(Server.MapPath(filePath), true);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #endregion 
    }
}
