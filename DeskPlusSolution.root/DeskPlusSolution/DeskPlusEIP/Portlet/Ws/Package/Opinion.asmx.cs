using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Opinion.BE;


namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Opinion : Base.BaseWSPortlet
    {
        /// <summary>
        /// 작성자 : withsuki
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 설문리스트가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetQuesList()
        {
            try
            {
                DAL.Opinion opiDal = new DAL.Opinion();
                DataTableReader dtr = opiDal.GetQuesList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                List<object> uobj = new List<object>();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            QuesInfo qInfo = new QuesInfo();
                            qInfo.Title = dtr["Title"].ToString();
                            //if (qInfo.Title.Length > 20)
                            //{
                            //    qInfo.Title = qInfo.Title.Substring(0, 15) + "..";
                            //}
                            qInfo.QuesID = int.Parse(dtr["QuesID"].ToString());
                            qInfo.VoteCnt = int.Parse(dtr["VoteCnt"].ToString());
                            uobj.Add(qInfo);
                        }
                    }
                }
                return uobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetQuesUnitList(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);
                int quesID = GetDecryptParam("quesID", "").ToNumber<int>();

                Zeospace.DeskPlus.Opinion.DAL.QuesUnit quDal = new DeskPlus.Opinion.DAL.QuesUnit();
                DataTableReader dtr = quDal.GetQuesUnitList(CurrentUserInfo.CompanyID, quesID);
                
                List<object> uobj = new List<object>();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            QuesUnitInfo quInfo = new QuesUnitInfo();
                            quInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            quInfo.QuesUnitID = dtr["QuesUnitID"].ToString().ToNumber<int>();
                            quInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                            quInfo.Title = dtr["Title"].ToString();
                            //if (quInfo.Title.Length > 20)
                            //{
                            //    quInfo.Title = quInfo.Title.Substring(0, 15) + "..";
                            //}
                            uobj.Add(quInfo);
                        }
                    }
                }
                return uobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }




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
        /// 설문 리스트(참여)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetVoteList(string p)
        {
            SetDecryptString(p);

            string totalItemCount = GetDecryptParam("totalItemCount", "0");
            string recordPerPage = GetDecryptParam("recordPerPage", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                List<object> list = new List<object>();

                
                DAL.Opinion dal = new DAL.Opinion();
                DataTableReader dtr = dal.GetVoteList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1]);
                                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {                            
                            QuesInfo info = new QuesInfo();

                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Title = dtr["Title"].ToString();
                            info.QuesID = int.Parse(dtr["QuesID"].ToString());
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());
                            info.StartDateStr = info.StartDate.Year + "." + info.StartDate.Month.ToString().PadLeft(2, '0') + "." + info.StartDate.Day.ToString().PadLeft(2, '0');
                            info.EndDateStr = info.EndDate.Year + "." + info.EndDate.Month.ToString().PadLeft(2, '0') + "." + info.EndDate.Day.ToString().PadLeft(2, '0');

                            list.Add(info);                            
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUsable1List : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 설문리스트(미참여)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetVoteNonList(string p)
        {
            SetDecryptString(p);

            string totalItemCount = GetDecryptParam("totalItemCount", "0");
            string recordPerPage = GetDecryptParam("recordPerPage", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                List<object> list = new List<object>();

                DAL.Opinion dal = new DAL.Opinion();
                DataTableReader dtr = dal.GetVoteNonList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1]);
                                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            QuesInfo info = new QuesInfo();

                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Title = dtr["Title"].ToString();
                            info.QuesID = int.Parse(dtr["QuesID"].ToString());
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());
                            info.StartDateStr = info.StartDate.Year + "." + info.StartDate.Month.ToString().PadLeft(2, '0') + "." + info.StartDate.Day.ToString().PadLeft(2, '0');
                            info.EndDateStr = info.EndDate.Year + "." + info.EndDate.Month.ToString().PadLeft(2, '0') + "." + info.EndDate.Day.ToString().PadLeft(2, '0');

                            list.Add(info);    
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUsable1List : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 설문카운트(참여)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetVoteCnt(string p)
        {
            try
            {
                SetDecryptString(p);

                DAL.Opinion dal = new DAL.Opinion();
                return dal.GetVoteCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return 0;
                //throw;
            }
        }

        /// <summary>
        /// 설문카운트(미참여)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetVoteNonCnt(string p)
        {
            try
            {
                SetDecryptString(p);

                DAL.Opinion dal = new DAL.Opinion();
                return dal.GetVoteNonCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return 0;
                //throw;
            }
        }
    }
}
