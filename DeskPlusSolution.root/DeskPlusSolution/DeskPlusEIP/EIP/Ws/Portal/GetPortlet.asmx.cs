using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// GetPortlet의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetPortlet : Base.BaseWSEIP
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
        /// 작성자 : shine
        /// 작성일자 :
        /// 작성목적 : 포틀릿 리스트 카운트 가져오기
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

                string categoryID = GetDecryptParam("categoryID", "0");
                string tabID = GetDecryptParam("tabID", "0");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.Portlet biz = new DAL.Portlet();

                return biz.GetPageCount(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), 
                        tabID.ToNumber<int>(), CurrentUserInfo.UserID, 
                        string.Join(",", CurrentUserInfo.RoleIDs), searchKey, searchValue, true);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string categoryID = GetDecryptParam("categoryID", "0");
                string tabID = GetDecryptParam("tabID", "0");

                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string PhotoName = string.Empty;

                DAL.Portlet biz = new DAL.Portlet();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                                         tabID.ToNumber<int>(), 
                                         string.Join(",", CurrentUserInfo.RoleIDs),
                                         setPageingStr[0], setPageingStr[1],
                                         searchKey, searchValue, order, true);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PortletInfo info = new PortletInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.PortletID = dtr["PortletID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();

                            #region 다국어(제목)
                            info.TitleName = this.GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.Description = dtr["Description"].ToString();

                            #region 이미지

                            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "");
                            string thumImgPath = this.ImgPath + "/Icon/rss.gif";

                            info.PhotoName = dtr["PhotoName"].ToString();

                            if (info.PhotoName.Length > 0)
                            {
                                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                                thumImgPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/EIP/Portlet/" + info.PhotoName.Split('.')[0] + "_S." + info.PhotoName.Split('.')[1];
                            }

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", thumImgPath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.PhotoName = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            info.PortletIDCnt = dtr["PortletIDCnt"].ToString().ToNumber<int>();

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
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 탭의 포틀릿 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddTabPortlet(string p)
        {
            try
            {
                SetDecryptString(p);

                string tabID = GetDecryptParam("tabID", "0");
                string columnID = GetDecryptParam("columnID", "");
                string portletID = GetDecryptParam("portletID", "0");

                if (tabID == "" && columnID == "" && portletID == "0") return false;

                DAL.TabPortlet dal = new DAL.TabPortlet();
                return dal.AddTabPortlet(CurrentUserInfo.CompanyID, tabID.ToNumber<int>(),
                    portletID.ToNumber<int>(), columnID, CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
