using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Upload
{
    /// <summary>
    /// WebHardAttach의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WebHardAttach : Base.BaseWSWebHard
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
        /// 작성일자 : 2012-01-26
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
                string order = GetDecryptParam("order", "");
                string fileType = GetDecryptParam("fileType", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string tmpVal = String.Empty;
                if(fileType.IndexOf("*.*") < 0)
                {
                    if (fileType != "")
                    {
                        string[] arrfileType = fileType.Split(';');

                        tmpVal = "AND (";
                        for (int i = 0; i < arrfileType.Length - 1; i++)
                        {
                            tmpVal += " (FileType Like '%" + arrfileType[i].Replace("*", "") + "%')";
                            if ((arrfileType.Length - 2) != i)
                            {
                                tmpVal += " OR ";
                            }
                        }

                        tmpVal += ")";
                    }
                }

                
                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID,
                    setPageingStr[0], setPageingStr[1], order, tmpVal);

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

                            int tmpFileLength = dtr["FileLength"].ToString().ToNumber<int>();
                            tmpFileLength = tmpFileLength / 1024;

                            info.FileLength = tmpFileLength.ToString("###,#");
                            info.FileGUID = dtr["FileGUID"].ToString();
 
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
        /// 작성일자 : 2012-01-26
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
                string fileType = GetDecryptParam("fileType", "");

                int icnt = 0;

                string tmpVal = String.Empty;
                if (fileType.IndexOf("*.*") < 0)
                {
                    if (fileType != "")
                    {
                        string[] arrfileType = fileType.Split(';');

                        tmpVal = "AND (";
                        for (int i = 0; i < arrfileType.Length - 1; i++)
                        {
                            tmpVal += " (FileType Like '%" + arrfileType[i].Replace("*", "") + "%')";
                            if ((arrfileType.Length - 2) != i)
                            {
                                tmpVal += " OR ";
                            }
                        }

                        tmpVal += ")";
                    }
                }

                
                DAL.File dal = new DAL.File();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>(), tmpVal);

                return icnt;
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
