using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.Visit.Web.Ws.Content
{
    /// <summary>
    /// ListDay의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListDay : Base.BaseWSVisit
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
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.ListDay).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string tmpdate = GetDecryptParam("tmpdate", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Reserve dal = new DAL.Reserve();

                DataTableReader dtr = dal.GetDayPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpdate, setPageingStr[0], setPageingStr[1], searchField, searchValue, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ReserveInfo info = new BE.ReserveInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ReserveID = dtr["ReserveID"].ToString().ToNumber<int>();
                            info.ReserveGroupID = dtr["ReserveGroupID"].ToString().ToNumber<int>();
                            info.Subject = dtr["Subject"].ToString();
                            //info.Content = dtr["Content"].ToString();
                            info.VisitType = dtr["VisitType"].ToString();
                            info.Auth = dtr["Auth"].ToString();

                            if (info.VisitType == "0")
                            {
                                info.VisitTypeStr = resMng.GetString("Type0");
                            }
                            else if (info.VisitType == "1")
                            {
                                info.VisitTypeStr = resMng.GetString("Type1");
                            }
                            else if (info.VisitType == "2")
                            {
                                info.VisitTypeStr = resMng.GetString("Type2");
                            }

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            info.WriteUserName = this.GetLangName(
                                       dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                       dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                       dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                       dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;

                            /*
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dt))
                                info.StartDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.StartDateStr = string.Empty;

                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dt))
                                info.EndDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.EndDateStr = string.Empty;
                            */

                            info.StartDateStr = Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.EndDateStr = Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

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
        /// 작성일자 : 2014-02-05
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
                string tmpdate = GetDecryptParam("tmpdate", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
             
                int icnt = 0;

                DAL.Reserve dal = new DAL.Reserve();
                icnt = dal.GetDayPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpdate, searchField, searchValue);
             
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 승인 / 취소 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModVisitType(string p)
        {
            SetDecryptString(p);
            string visitType = GetDecryptParam("visitType", "0");
            string preLang = GetDecryptParam("preLang", "ko");
            int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

            DAL.Reserve dal = new DAL.Reserve();
            if (dal.Mod(CurrentUserInfo.CompanyID, reserveID, visitType))
            {
                // 알람처리
                BLL.Reserve bll = new BLL.Reserve();
                bll.AlarmVisitType(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, reserveID, visitType, preLang);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 사용자 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSetting(string p)
        {
            try
            {
                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "D");

                BE.SettingInfo info = new BE.SettingInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;                
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();                
                info.Types = types;

                DAL.Setting dal = new DAL.Setting();
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
