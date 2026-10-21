using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Content
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSWebHard
    {

        #region GET()

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
        /// 작성일자 : 2011-10-20
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderID = GetDecryptParam("folderID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");
                string downCheck = GetDecryptParam("downCheck", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

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

                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID,
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, startDate, endDate, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileType = dtr["FileType"].ToString().Replace(".","");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();
                            info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>();
                            info.DownCheck = downCheck;

                            if (info.FolderID == -3)
                            {
                                info.SendUserID = dtr["SendUserID"].ToString().ToNumber<int>();
                                info.SendName = this.GetLangName(
                                        dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                        dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                        dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                        dtr["SendNameZhtw"].ToString().ToNull(), PreLang);
                            }

                            info.WriteName = this.GetLangName(
                            dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                            dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                            dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                            dtr["WriteNameZhtw"].ToString().ToNull(), PreLang);

                            string encFilePath = (info.FileGUID != "") ? dtr["FilePath"].ToString() + info.FileGUID : dtr["FilePath"].ToString() + info.FileName;
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", encFilePath);
                            paraDic.Add("fileName", info.FileName);
                            info.DownPath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.ROW = listNum.ToString();


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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-20
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderID = GetDecryptParam("folderID", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

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

                int icnt = 0;

                DAL.File dal = new DAL.File();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>(),
                    searchField, searchValue, startDate, endDate);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-01
        /// 작성목적 : 개인이 사용한 용량
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetUseCount()
        {
            try
            {
                DAL.File dal = new DAL.File();
                return dal.GetUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-04
        /// 작성목적 : 개인이 올릴 수 있는 남아 있는 사용량 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetShareQuota(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("uID", "0");

                int icnt = 0;
                int FileLength = 0;
                int PrivateQuota = 0;

                DAL.File dal = new DAL.File();
                DataTableReader dtr = dal.GetShareQuota(CurrentUserInfo.CompanyID, userID.ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {   
                            FileLength = dtr["FileLength"].ToString().ToNumber<int>();
                            PrivateQuota = dtr["PrivateQuota"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (PrivateQuota == 0)
                {
                    icnt = 0;
                }
                else
                {
                    icnt = (PrivateQuota * 1048576) - FileLength;
                }

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-23
        /// 작성목적 : 공용폴더 용량정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetPublicUseCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderID = GetDecryptParam("folderID", "");
                string types = GetDecryptParam("types", "");
                string rtn = string.Empty;

                DAL.File dal = new DAL.File();
                DataSet ds = new DataSet();
                ds = dal.GetPublicUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID ,folderID.ToNumber<int>(), types);

                if (ds.Tables["GetPublicUseCount"] != null)
                {
                    foreach (DataRow dbRow in ds.Tables["GetPublicUseCount"].Rows)
                    {
                        rtn = dbRow["FolderLength"].ToString() + "|" + dbRow["FileLengthSum"].ToString();
                    }
                }

                return rtn;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 파일 삭제 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetChkTrash(string p)
        {
            try
            {
                SetDecryptString(p);
                string tmpValue = GetDecryptParam("tmpValue", "");

                tmpValue = tmpValue.TrimEnd(',');

                DAL.File dal = new DAL.File();
                return dal.GetChkTrash(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 공용폴더 최신 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNewListPublic(string p)
        {
            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
              
                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetNewListPublic(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.ListInfo info = new BE.ListInfo();

                            info.FolderName = this.GetLangName(
                            dtr["FolderNameBase"].ToString().ToNull(), dtr["FolderNameKo"].ToString().ToNull(),
                            dtr["FolderNameEn"].ToString().ToNull(), dtr["FolderNameJa"].ToString().ToNull(),
                            dtr["FolderNameFr"].ToString().ToNull(), dtr["FolderNameZhcn"].ToString().ToNull(),
                            dtr["FolderNameZhtw"].ToString().ToNull(), PreLang);

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileType = dtr["FileType"].ToString().Replace(".", "");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.WriteName = this.GetLangName(
                                dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                                dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                                dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                                dtr["WriteNameZhtw"].ToString().ToNull(), PreLang);

                            string encFilePath = (info.FileGUID != "") ? dtr["FilePath"].ToString() + info.FileGUID : dtr["FilePath"].ToString() + info.FileName;
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", encFilePath);
                            paraDic.Add("fileName", info.FileName);
                            info.DownPath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

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

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 전달받은 파일 최신 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNewListSend(string p)
        {
            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetNewListSend(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.ListInfo info = new BE.ListInfo();

                            info.SendName = this.GetLangName(
                                    dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                    dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                    dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                    dtr["SendNameZhtw"].ToString().ToNull(), PreLang); 

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileType = dtr["FileType"].ToString().Replace(".", "");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.WriteName = this.GetLangName(
                            dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                            dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                            dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                            dtr["WriteNameZhtw"].ToString().ToNull(), PreLang);

                            string encFilePath = (info.FileGUID != "") ? dtr["FilePath"].ToString() + info.FileGUID : dtr["FilePath"].ToString() + info.FileName;
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", encFilePath);
                            paraDic.Add("fileName", info.FileName);
                            info.DownPath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

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

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 공유받은폴더 최신 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNewListShare(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Ws.Content.List).Assembly;	        //어셈블리 선언

                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetNewListShare(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.ListInfo info = new BE.ListInfo();

                            info.SendName = this.GetLangName(
                                dtr["ShareNameBase"].ToString().ToNull(), dtr["ShareNameKo"].ToString().ToNull(),
                                dtr["ShareNameEn"].ToString().ToNull(), dtr["ShareNameJa"].ToString().ToNull(),
                                dtr["ShareNameFr"].ToString().ToNull(), dtr["ShareNameZhcn"].ToString().ToNull(),
                                dtr["ShareNameZhtw"].ToString().ToNull(), PreLang); 

                            info.FolderName = this.GetLangName(
                            dtr["FolderNameBase"].ToString().ToNull(), dtr["FolderNameKo"].ToString().ToNull(),
                            dtr["FolderNameEn"].ToString().ToNull(), dtr["FolderNameJa"].ToString().ToNull(),
                            dtr["FolderNameFr"].ToString().ToNull(), dtr["FolderNameZhcn"].ToString().ToNull(),
                            dtr["FolderNameZhtw"].ToString().ToNull(), PreLang);

                            if (info.FolderName == "ROOT")
                            {
                                info.FolderName = resMng.GetString("PrivateFolderName");
                            }

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileType = dtr["FileType"].ToString().Replace(".", "");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.WriteName = this.GetLangName(
                            dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                            dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                            dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                            dtr["WriteNameZhtw"].ToString().ToNull(), PreLang);

                            string encFilePath = (info.FileGUID != "") ? dtr["FilePath"].ToString() + info.FileGUID : dtr["FilePath"].ToString() + info.FileName;
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", encFilePath);
                            paraDic.Add("fileName", info.FileName);
                            info.DownPath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

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

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 검색 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSearchList(string p)
        {
            try
            {

                #region 다국어 리소스 로드/Title 정보                
                this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Ws.Content.List).Assembly;	        //어셈블리 선언

                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchValue = GetDecryptParam("searchValue", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.File dal = new DAL.File();

                List<object> listobj = new List<object>();

                DataTableReader dtr = dal.SearchList(CurrentUserInfo.CompanyID, searchValue, setPageingStr[0], setPageingStr[1], CurrentUserInfo.UserID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.ListInfo info = new BE.ListInfo();

                            info.FolderType = dtr["FolderType"].ToString();                            

                            info.SendName = this.GetLangName(
                                dtr["ShareNameBase"].ToString().ToNull(), dtr["ShareNameKo"].ToString().ToNull(),
                                dtr["ShareNameEn"].ToString().ToNull(), dtr["ShareNameJa"].ToString().ToNull(),
                                dtr["ShareNameFr"].ToString().ToNull(), dtr["ShareNameZhcn"].ToString().ToNull(),
                                dtr["ShareNameZhtw"].ToString().ToNull(), PreLang);

                            info.FolderName = this.GetLangName(
                            dtr["FolderNameBase"].ToString().ToNull(), dtr["FolderNameKo"].ToString().ToNull(),
                            dtr["FolderNameEn"].ToString().ToNull(), dtr["FolderNameJa"].ToString().ToNull(),
                            dtr["FolderNameFr"].ToString().ToNull(), dtr["FolderNameZhcn"].ToString().ToNull(),
                            dtr["FolderNameZhtw"].ToString().ToNull(), PreLang);

                            if (info.FolderName == "ROOT")
                            {
                                info.FolderName = resMng.GetString("PrivateFolderName");
                            }

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileType = dtr["FileType"].ToString().Replace(".", "");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.WriteName = this.GetLangName(
                            dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                            dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                            dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                            dtr["WriteNameZhtw"].ToString().ToNull(), PreLang);

                            string encFilePath = (info.FileGUID != "") ? dtr["FilePath"].ToString() + info.FileGUID : dtr["FilePath"].ToString() + info.FileName;
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", encFilePath);
                            paraDic.Add("fileName", info.FileName);
                            info.DownPath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

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


        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 검색 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSearchCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                DAL.File dal = new DAL.File();
                icnt = dal.SearchCount(CurrentUserInfo.CompanyID, searchValue, CurrentUserInfo.UserID);


                return icnt;
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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-28
        /// 작성목적 : 파일 업로드
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetSpoolUpload(string p)
        {
            try
            {
                SetDecryptString(p);
                string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");
                string folderID = GetDecryptParam("folderID", "0");
                string uID = GetDecryptParam("uID", "0");
                string types = GetDecryptParam("types", "");
                
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string tmpSpoolPath = filePath + "{0}/Users/{1}/Spool/{2}/";
                string spoolPath = string.Format(tmpSpoolPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

                string tmpPath = string.Empty;
                string webHardPath = string.Empty;

                if (types == "U" || types == "L")
                {
                    tmpPath = filePath + "{0}/WebHard/{1}/";
                    webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, folderID);
                }
                else
                {
                    tmpPath = filePath + "{0}/Users/{1}/WebHard/{2}/";
                    webHardPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, uID, folderID);
                }

                List<BE.FileInfo> al = new List<BE.FileInfo>();

                DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));
                if (di.Exists)
                {
                    FileInfo[] sortList = di.GetFiles();
                    Array.Sort(sortList,
                        new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.FileListSort());

                    foreach (FileInfo f in sortList)
                    {  
                        BE.FileInfo info = new BE.FileInfo();

                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = uID.ToNumber<int>();
                        info.FolderID = folderID.ToNumber<int>();
                        info.FilePath = webHardPath;
                        info.FileName = f.Name;
                        info.FileType = f.Extension;
                        info.FileLength = f.Length;
                        info.FileGUID = System.Guid.NewGuid().ToString() + f.Extension;
                        info.DeleteYN = "N";

                        if (HasScreenShot(spoolPath + f.Name))
                        {
                            info.ScreenShot = "Y";
                        }
                        else
                        {
                            info.ScreenShot = "N";
                        }

                        al.Add(info);
                    }
                }

                BLL.File bll = new BLL.File();
                //if (!bll.Add(al, spoolPath, webHardPath))
                //{
                //    return false;
                //}

                var rtnList = bll.Add(al, spoolPath, webHardPath);

                if (rtnList.Length == 0)
	            {
                    return false;
	            }

                #region DOC_STREAM
                var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.WEBHARD, "DocStreamYN", "N");

                if (docStreamYN == "Y")
                {   
                    DocStream ds = new DocStream();

                    try
                    {
                        var newDSList = rtnList.Split(',');
                        for (int i = 0; i < newDSList.Length; i++)
                        {
                            ds.AddDocStream(CurrentUserInfo.CompanyID, newDSList[i].ToNumber<int>(), "WHD_FILE");
                            ds.UpdateFileID(CurrentUserInfo.CompanyID, newDSList[i].ToNumber<int>(), "WHD_FILE");
                        }
                    }
                    catch (Exception ex)
                    {
                        var DSList = rtnList.Split(',');
                        for (int i = 0; i < DSList.Length; i++)
                        {
                            ds.DelDocStream(CurrentUserInfo.CompanyID, DSList[i].ToNumber<int>(), "WHD_FILE");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD_File_Add_DS : " + ex.Message);
                        return false;
                    }

                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-10
        /// 작성목적 : 이미지 파일인지 구분
        /// </summary>
        /// <returns></returns>
        private bool HasScreenShot(string _path)
        {
            bool fileOK = false;

            string fileExtension = System.IO.Path.GetExtension(Server.MapPath(_path)).ToLower();
            //string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };
            string[] allowedExtensions = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ImgFileExtension", "").Split(',');

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-27
        /// 작성목적 : 중요표시 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string fileID = GetDecryptParam("fileID", "");
                string folderID = GetDecryptParam("folderID", "0");
                string types = GetDecryptParam("types", "");

                if (!String.IsNullOrEmpty(fileID))
                {
                    BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.FileID = fileID.ToNumber<int>();
                    info.UserID = CurrentUserInfo.UserID;
                    info.FolderID = folderID.ToNumber<int>();
                    info.Types = types;

                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    dal.Add(info);
                }                   
                
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddImpUser : " + ex.Message);
                throw;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 휴지통으로 삭제 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UpdTrash(string p)
        {
            try
            {
                SetDecryptString(p);
                string deleteYN = GetDecryptParam("deleteYN", "Y");
                string tmpValue = GetDecryptParam("tmpValue", "");

                tmpValue = tmpValue.TrimEnd(',');

                BLL.File bll = new BLL.File();
                bll.UpdTrash(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, deleteYN, tmpValue);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-11
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

                string folderID = GetDecryptParam("folderID", "0");
                string listType = GetDecryptParam("listType", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "P"); 

                BE.FolderUserInfo info = new BE.FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID.ToNumber<int>();
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.ListType = listType;
                info.Types = types;

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-30
        /// 작성목적 : 중요표시 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string fileID = GetDecryptParam("fileID", "");
                string folderID = GetDecryptParam("folderID", "0");
                string types = GetDecryptParam("types", "");

                if (!String.IsNullOrEmpty(fileID))
                {
                    BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.FileID = fileID.ToNumber<int>();
                    info.UserID = CurrentUserInfo.UserID;
                    info.FolderID = folderID.ToNumber<int>();
                    info.Types = types;

                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    dal.Del(info);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelImpUser : " + ex.Message);
                throw;
            }

            return true;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-09
        /// 작성목적 : 파일 삭제 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFile(string p)
        {
            try
            {
                SetDecryptString(p);
                string fileList = GetDecryptParam("fileList", "");
                string order = GetDecryptParam("order", "WriteDate desc");

                fileList = fileList.TrimEnd(',');
                string tmpFileID = String.Empty;

                DAL.File dal = new DAL.File();
                DataTableReader dtr = dal.GetFileList(CurrentUserInfo.CompanyID, fileList, order);

                List<BE.FileInfo> al = new List<BE.FileInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FileInfo info = new BE.FileInfo();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.UserID = CurrentUserInfo.UserID;
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                           
                            al.Add(info);

                            tmpFileID += dtr["FileID"].ToString().ToNumber<int>() + ",";
                        }
                    }
                }

                tmpFileID = tmpFileID.TrimEnd(',');

                BLL.File bll = new BLL.File();
                return bll.Del(al, tmpFileID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        #endregion 

        #region 기타
        /// <summary>
        /// 사이즈 처리
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        protected string FormatSize(double fileSize)
        {
            if (fileSize < 1024)
                return String.Format("{0:N0}B", fileSize);
            else if (fileSize < 1024 * 1024)
                return String.Format("{0:N1}KB", fileSize / 1024);
            else if (fileSize < 1024 * 1024 * 1024)
                return String.Format("{0:N1}MB", fileSize / (1024 * 1024));
            else
                return String.Format("{0:N1}GB", fileSize / (1024 * 1024 * 1024));
        }

        #endregion 
    }
}
