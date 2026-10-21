using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Content
{
    /// <summary>
    /// ListImpoPreview 의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListImpoPreview : Base.BaseWSWebHard
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
        /// 작성일자 : 2011-12-30
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetImpoPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");

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
                        getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DAL.ImpoUser dal = new DAL.ImpoUser();

                DataTableReader dtr = dal.GetImpoPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
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
                            info.FileType = dtr["FileType"].ToString().Replace(".", "");
                            info.FileLength = FormatSize(Convert.ToDouble(dtr["FileLength"].ToString()));
                            info.FileGUID = dtr["FileGUID"].ToString();
                            info.ScreenShot = dtr["ScreenShot"].ToString();
                            //info.ScreenShotPath = dtr["FilePath"].ToString() + "Thum_" + dtr["FileGUID"].ToString();

                            if (info.FolderID == -3)
                            {
                                info.SendUserID = dtr["SendUserID"].ToString().ToNumber<int>();
                                info.SendName = this.GetLangName(
                                        dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                        dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                        dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                        dtr["SendNameZhtw"].ToString().ToNull(), PreLang);
                            }
                            else
                            {
                                info.SendUserID = 0;
                                info.SendName = "";
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelper.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.ROW = listNum.ToString();

                            string name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.WriteName = this.GetLangName(
                            dtr["WriteNameBase"].ToString().ToNull(), dtr["WriteNameKo"].ToString().ToNull(),
                            dtr["WriteNameEn"].ToString().ToNull(), dtr["WriteNameJa"].ToString().ToNull(),
                            dtr["WriteNameFr"].ToString().ToNull(), dtr["WriteNameZhcn"].ToString().ToNull(),
                            dtr["WriteNameZhtw"].ToString().ToNull(), PreLang); 

                            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
                            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Ws.Content.ListImpo).Assembly;	//어셈블리 선언
                            this.GetResourceManager();

                            name = (name == "ROOT") ? resMng.GetString("PrivateFolderName") : name;

                            if (String.IsNullOrEmpty(name))
                            {
                                name = resMng.GetString("SendFile");
                            }

                            info.FolderName = name;

                            #region 미리보기 이미지
                            if (dtr["ScreenShot"].ToString() == "Y")
                            {
                                info.ScreenShotImage = dtr["FilePath"].ToString() + "Thum_" + dtr["FileGUID"].ToString();
                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", info.ScreenShotImage);
                                paraDic.Add("thumnail", "Y");
                                paraDic.Add("thumWidth", "160");
                                paraDic.Add("thumHeight", "160");
                                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                info.ScreenShotImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            }
                            else
                            {
                                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                                info.ScreenShotImage = this.BaseImgPath + "/Icon/ExtL/" + dtr["FileType"].ToString().Replace(".", "") + ".gif";
                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", info.ScreenShotImage);
                                paraDic.Add("thumnail", "Y");
                                paraDic.Add("thumWidth", "160");
                                paraDic.Add("thumHeight", "160");
                                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                info.ScreenShotImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            }
                            #endregion

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
        /// 작성일자 : 2011-12-30
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetImpoListCount(string p)
        {
            try
            {
                SetDecryptString(p);
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
                        getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelper.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                int icnt = 0;

                DAL.ImpoUser dal = new DAL.ImpoUser();
                icnt = dal.GetImpoPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
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
        /// 작성일자 : 2012-01-02
        /// 작성목적 : 다운로드 권한 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAuthority(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderID = GetDecryptParam("folderID", "");
                string rtnVal = "N";

                BLL.ShareFolder bll = new BLL.ShareFolder();
                DataSet ds = bll.GetShareAuthority(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.UserDepts, folderID.ToNumber<int>());

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // 개인 권한 우선 
                        DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");
                        if (dr.Length > 0)
                        {
                            if (dr[0]["Authority"].ToString() == "3")
                            {
                                rtnVal = "Y";
                            }
                            else if (dr[0]["Authority"].ToString() == "2")
                            {
                                rtnVal = "N";
                            }
                            else if (dr[0]["Authority"].ToString() == "1")
                            {
                                rtnVal = "Y";
                            }
                            else
                            {
                                rtnVal = "N";
                            }
                        }
                        else
                        {
                            // 부서 권한       
                            foreach (DataRow dbRow in ds.Tables[0].Rows)
                            {
                                if (dbRow["Authority"].ToString() == "3")
                                {
                                    return "Y";
                                }
                                else if (dbRow["Authority"].ToString() == "2")
                                {
                                    rtnVal = "N";
                                }
                                else if (dbRow["Authority"].ToString() == "1")
                                {
                                    return "Y";
                                }
                            }
                        }
                    }
                    else
                    {
                        return "Y";
                    }
                }
                else
                {
                    return "Y";
                }


                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
        
        #region MOD()


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-12
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
                string fileID = GetDecryptParam("tmpValue", "").TrimEnd(',');//fileID,fileID

                if (fileID.Length > 0)
                {
                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    dal.DelImpUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, fileID);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelImpUser : " + ex.Message);
                throw;
            }

            return true;
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
