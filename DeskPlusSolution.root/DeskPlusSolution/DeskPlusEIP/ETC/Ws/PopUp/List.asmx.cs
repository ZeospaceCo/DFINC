using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;

namespace Zeospace.DeskPlus.ETC.Web.Ws.PopUp
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSETC
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
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.PopUp.ManagerList> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.ETC.DAL.Manager biz = new DAL.Manager();
                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    setPageingStr[0], setPageingStr[1], order);

                List<Ws.PopUp.ManagerList> listobj = new List<ManagerList>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.PopUp.ManagerList info = new ManagerList();

                            info.ROW = dtr["ROW"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ManagerID = dtr["ManagerID"].ToString().ToNumber<int>();

                            info.Subject = dtr["Subject"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.UserName = dtr["UserName"].ToString();

                            info.Height = dtr["Height"].ToString().ToNumber<int>();
                            info.Width = dtr["Width"].ToString().ToNumber<int>();

                            info.StartDate = dtr["StartDate"].ToString();
                            info.EndDate = dtr["EndDate"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 리스트 카운트 가져오기
        /// 작성자 : shine
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

                Zeospace.DeskPlus.ETC.DAL.Manager biz = new DAL.Manager();
                return biz.GetPageListCount(CurrentUserInfo.CompanyID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetListCount : " + ex.Message);
                return 0;
            }
        }

    }

    [Serializable]
    public class ManagerList
    {
        public ManagerList() { }

        public int ROW { get; set; }
        public int CompanyID { get; set; }
        public int ManagerID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string WriteDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string LoginYN { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

    }
}
