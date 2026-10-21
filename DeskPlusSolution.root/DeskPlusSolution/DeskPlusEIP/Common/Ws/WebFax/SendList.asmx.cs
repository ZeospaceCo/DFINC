using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Common.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;

namespace Zeospace.DeskPlus.Common.Web.Ws.WebFax
{
    /// <summary>
    /// SendList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SendList : Base.BaseWSCommon
    {

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
        /// 작성자 : 심재운
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.FaxSendInfo> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");//pagesize
                string currentPage = GetDecryptParam("currentPage", "0");//pageno
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string branchID = GetDecryptParam("branchID", "0");

                //송신폴더
                string faxFileBackUpData = "";
                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "FaxClientNo");
                for (int i = 0; i < dv.Count; i++)
                {
                    if (dv[i]["CodeID"].ToString() == "FaxFileDirectory")
                    {
                        faxFileBackUpData = dv[i]["NameBase"].ToString() + "BackUpData/";
                        break;
                    }
                }

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

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.FaxManager dal = new DAL.FaxManager();

                DataTableReader dtr = dal.GetSendPageList(CurrentUserInfo.CompanyID,
                    setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchKey, searchValue, order, branchID.ToNumber<int>());

                List<BE.FaxSendInfo> listobj = new List<FaxSendInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FaxSendInfo info = new FaxSendInfo();

                            info.Row = dtr["ROW"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["RequestTime"].ToString(), out dt))
                                info.RequestTime = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.ToString("HH:mm:ss");
                            else
                                info.RequestTime = string.Empty;

                            if (DateTime.TryParse(dtr["ReserveTime"].ToString(), out dt))
                                info.ReserveTime = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.ToString("HH:mm:ss");
                            else
                                info.ReserveTime = string.Empty;

                            if (DateTime.TryParse(dtr["EndTime"].ToString(), out dt))
                                info.EndTime = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.ToString("HH:mm:ss");
                            else
                                info.EndTime = string.Empty;

                            info.SendStatus = dtr["SendStatus"].ToString().ToNumber<int>();
                            info.TotalCnt = dtr["TotalCnt"].ToString().ToNumber<int>();
                            info.SuccessCnt = dtr["SuccessCnt"].ToString().ToNumber<int>();
                            info.FailCnt = dtr["FailCnt"].ToString().ToNumber<int>();
                            info.WaitCnt = dtr["WaitCnt"].ToString().ToNumber<int>();
                            info.UserCode = dtr["UserCode"].ToString();
                            info.Subject = dtr["Subject"].ToString();
                            info.GUID = dtr["GUID"].ToString();
                            

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.LoginID = dtr["LoginID"].ToString();

                            info.NameBase = dtr["NameBase"].ToString();
                            info.NameKo = dtr["NameKo"].ToString();
                            info.NameEn = dtr["NameEn"].ToString();
                            info.NameJa = dtr["NameJa"].ToString();
                            info.NameFr = dtr["NameFr"].ToString();
                            info.NameZhcn = dtr["NameZhcn"].ToString();
                            info.NameZhtw = dtr["NameZhtw"].ToString();

                            info.NameBase = this.GetLangName(
                                    info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                            info.DeptBase = dtr["DeptBase"].ToString();
                            info.DeptKo = dtr["DeptKo"].ToString();
                            info.DeptEn = dtr["DeptEn"].ToString();
                            info.DeptJa = dtr["DeptJa"].ToString();
                            info.DeptFr = dtr["DeptFr"].ToString();
                            info.DeptZhcn = dtr["DeptZhcn"].ToString();
                            info.DeptZhtw = dtr["DeptZhtw"].ToString();

                            info.DeptBase = this.GetLangName(
                                    info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);

                            info.CodeBranchID = dtr["CodeBranchID"].ToString().ToNumber<int>();
                            info.BranchBase = dtr["BranchBase"].ToString();
                            info.BranchKo = dtr["BranchKo"].ToString();
                            info.BranchEn = dtr["BranchEn"].ToString();
                            info.BranchJa = dtr["BranchJa"].ToString();
                            info.BranchFr = dtr["BranchFr"].ToString();
                            info.BranchZhcn = dtr["BranchZhcn"].ToString();
                            info.BranchZhtw = dtr["BranchZhtw"].ToString();

                            info.BranchBase = this.GetLangName(
                                    info.BranchBase, info.BranchKo, info.BranchEn, info.BranchJa, info.BranchFr, info.BranchZhcn, info.BranchZhtw, PreLang);

                            info.Pages = dtr["Pages"].ToString();

                            // /DeskPlusEIP/FaxFileData/BackUpData/20140103/c572dcac50b043509b8c338b031b87ce/c572dcac50b043509b8c338b031b87ce.001.View.gif                            
                            info.FilePath = faxFileBackUpData + Convert.ToDateTime(dtr["RequestTime"]).ToString("yyyyMMdd") + "/" + info.GUID + "/" + info.GUID;

                            info.DownloadFilePath = "";

                            if (File.Exists(Server.MapPath(info.FilePath + ".tif")))
                            {
                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", info.FilePath + ".tif");
                                paraDic.Add("fileName", info.GUID + ".tif");
                                info.DownloadFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
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

        /// <summary>
        /// 작성자 : 심재운
        /// 총개수 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string branchID = GetDecryptParam("branchID", "0");

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

                DAL.FaxManager dal = new DAL.FaxManager();

                int cnt = dal.GetSendPageCount(CurrentUserInfo.CompanyID,
                    startDate, endDate,
                    searchKey, searchValue, branchID.ToNumber<int>());

                return cnt;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

    }
}

