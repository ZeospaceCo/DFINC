using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.ETC.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.ETC.Web.Ws.Holiday.Popup
{
    /// <summary>
    /// HoliAdminView의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewAdmin : Base.BaseWSETC
    {
        #region 리스트

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
        /// 작성자 : 김형석
        ///  리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.HoliUsed biz = new DAL.HoliUsed();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    userID, startDate, endDate,
                    setPageingStr[0], setPageingStr[1]);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.HoliUsedInfo info = new BE.HoliUsedInfo();

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.ROW = listNum.ToString();

                            info.UsedID = dtr["UsedID"].ToString();
                            info.DocumentID = dtr["DocumentID"].ToString();

                            DateTime dtS;
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dtS))
                                info.StartDate = DateHelpers.GetCultureDate(dtS, PreLangReal).Substring(0, 10);
                            else
                                info.StartDate = string.Empty;

                            DateTime dtE;
                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dtE))
                                info.EndDate = DateHelpers.GetCultureDate(dtE, PreLangReal).Substring(0, 10);
                            else
                                info.EndDate = string.Empty;

                            info.Duration = dtr["Duration"].ToString();
                            info.StartHalf = dtr["StartHalf"].ToString();
                            info.EndHalf = dtr["EndHalf"].ToString();
                            info.ProcessFlag = dtr["ProcessFlag"].ToString();
                            info.Gubun = dtr["Gubun"].ToString();

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
        /// 작성자 : 김형석
        /// 사용자, 년도별 전체 리스트(사용자) 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAllHoliUserList(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");

                DAL.HoliUsed biz = new DAL.HoliUsed();

                DataTableReader dtr = biz.GetAllHoliUserList(CurrentUserInfo.CompanyID, userID, startDate, endDate);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.HoliUsedInfo info = new BE.HoliUsedInfo();

                            DateTime dtS;
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dtS))
                                info.StartDate = DateHelpers.GetCultureDate(dtS, PreLangReal).Substring(0, 10);
                            else
                                info.StartDate = string.Empty;

                            DateTime dtE;
                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dtE))
                                info.EndDate = DateHelpers.GetCultureDate(dtE, PreLangReal).Substring(0, 10);
                            else
                                info.EndDate = string.Empty;

                            info.Duration = dtr["Duration"].ToString();
                            info.StartHalf = dtr["StartHalf"].ToString();
                            info.EndHalf = dtr["EndHalf"].ToString();
                            info.Gubun = dtr["Gubun"].ToString();

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

        #endregion

        #region MOD()

        /// <summary>
        /// 용도 : 사용자별 페이지사이즈 설정하기
        /// 작성자 : 김형석
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "ViewAdmin");

                BE.FolderUserInfo info = new FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
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
        /// 작성자 : 김형석
        /// 휴가일 수 저장하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelHoli(string p)
        {
            try
            {
                SetDecryptString(p);

                int usedID = GetDecryptParam("usedID", "").ToNumber<int>();

                DAL.HoliUsed biz = new DAL.HoliUsed();

                biz.DelHoli(CurrentUserInfo.CompanyID, usedID);

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
