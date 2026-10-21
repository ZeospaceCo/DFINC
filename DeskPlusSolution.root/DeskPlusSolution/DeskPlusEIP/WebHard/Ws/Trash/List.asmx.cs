using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Trash
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
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTrashPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string uID = GetDecryptParam("uID", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");
                string types = GetDecryptParam("types", "");

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

                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetTrashPageList(CurrentUserInfo.CompanyID, uID.ToNumber<int>(),
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, startDate, endDate, order, types);

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

                            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
                            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Ws.Content.ListImpo).Assembly;	//어셈블리 선언
                            this.GetResourceManager();

                            name = (name == "ROOT") ? resMng.GetString("PrivateFolderName") : name;

                            if (String.IsNullOrEmpty(name))
                            {
                                name = resMng.GetString("SendFile");
                            }

                            info.FolderName = name;

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
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetTrashListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string uID = GetDecryptParam("uID", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string types = GetDecryptParam("types", "");

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

                DAL.File dal = new DAL.File();
                icnt = dal.GetTrashPageListCount(CurrentUserInfo.CompanyID, uID.ToNumber<int>(),
                    searchField, searchValue, startDate, endDate, types);

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

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-13
        /// 작성목적 : 복원
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
                string deleteYN = GetDecryptParam("deleteYN", "N");
                string tmpValue = GetDecryptParam("tmpValue", "");

                tmpValue = tmpValue.TrimEnd(',');

                DAL.File dal = new DAL.File();
                dal.UpdTrash(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, deleteYN, tmpValue);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-13
        /// 작성목적 : 공용폴더 복원
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string UpdPublicTrash(string p)
        {
            try
            {
                SetDecryptString(p);
                string deleteYN = GetDecryptParam("deleteYN", "N");
                string tmpValue = GetDecryptParam("tmpValue", "");

                tmpValue = tmpValue.TrimEnd(',');

                DAL.File dal = new DAL.File();

                DataSet ds = dal.GetPublicUseCountSum(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpValue);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return "2";
                    }
                }

                dal.UpdTrash(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, deleteYN, tmpValue);

                return "1";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "0";
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
        /// 작성일자 : 2011-10-31
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-09
        /// 작성목적 : 파일 전체삭제 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool WDelFile()
        {
            try
            {
                DAL.File dal = new DAL.File();
                DataSet ds = dal.GetTrash(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                List<BE.FileInfo> al = new List<BE.FileInfo>();

                if (ds != null)
                {
                    string tmpFileID = String.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.FileInfo info = new BE.FileInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = CurrentUserInfo.UserID;
                        info.FileID = dr["FileID"].ToString().ToNumber<int>();
                        info.FilePath = dr["FilePath"].ToString() + dr["FileGUID"].ToString();

                        al.Add(info);

                        tmpFileID += dr["FileID"].ToString().ToNumber<int>() + ",";
                    }
                    
                    tmpFileID = tmpFileID.TrimEnd(',');

                    BLL.File bll = new BLL.File();
                    return bll.Del(al, tmpFileID);
                }

                return true;

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
