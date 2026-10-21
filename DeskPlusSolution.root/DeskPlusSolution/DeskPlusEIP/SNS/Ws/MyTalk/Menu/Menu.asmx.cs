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

namespace Zeospace.DeskPlus.SNS.Web.Ws.MyTalk.Menu
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Menu : Base.BaseWSMyTalk
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
        /// 작성목적 : 나에게 담벼락을 공유한 사람 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;          
            string searchValue = string.Empty;

            SetDecryptString(p);
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            searchValue = GetDecryptParam("searchValue", "");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            DAL.OpenList biz = new DAL.OpenList();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], searchValue);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";
 
                    while (dtr.Read())
                    {
                        BE.OpenListInfo info = new BE.OpenListInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);


                        info.Name = name;
                       
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
                            paraDic.Add("thumWidth", "52");
                            paraDic.Add("thumHeight", "70");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='52' />";  
                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

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
        /// 용도 : 나에게 담벼락을 공유한 사람 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            SetDecryptString(p);
            string searchValue = GetDecryptParam("searchValue", "");

            DAL.OpenList biz = new DAL.OpenList();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일 : 2013-05-13
        /// 작성목적 : 나의 담벼락을 보는 사람 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetOpenPageList(string p)
        {
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string searchValue = string.Empty;

            SetDecryptString(p);
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            searchValue = GetDecryptParam("searchValue", "");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            DAL.OpenList biz = new DAL.OpenList();
            DataTableReader dtr = biz.GetOpenPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], searchValue);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        BE.OpenListInfo info = new BE.OpenListInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);


                        info.Name = name;

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
                            paraDic.Add("thumWidth", "52");
                            paraDic.Add("thumHeight", "70");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='52' />";
                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

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
        /// 용도 : 나의 담벼락을 보는 사람 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetOpenListCount(string p)
        {
            SetDecryptString(p);
            string searchValue = GetDecryptParam("searchValue", "");

            DAL.OpenList biz = new DAL.OpenList();
            return biz.GetOpenPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue);
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
                string types = GetDecryptParam("types", "T");

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
    }
}
