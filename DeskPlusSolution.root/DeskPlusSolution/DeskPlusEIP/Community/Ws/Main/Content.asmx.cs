using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Main
{
    /// <summary>
    /// Content의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Content : Base.BaseWSCommunity
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
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 커뮤니티 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Ws.Main.Content).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);                
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");               
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');              

                DAL.Com dal = new DAL.Com();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ComInfo info = new BE.ComInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.Name = dtr["Name"].ToString().ToReverseXSS();
                            info.Description = dtr["Description"].ToString();                            
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            if (info.WriteUserName.Trim() == "")
                            {
                                info.WriteUserName = dtr["WriteUserName"].ToString();
                            }

                            info.Type = dtr["Type"].ToString().ToNumber<int>();
                            info.MemberCount = dtr["MemberCount"].ToString();
                            info.BoaCount = dtr["BoaCount"].ToString();

                            if (info.Type == 1)
                            {
                                info.StrType = resMng.GetString("Type1");
                            }
                            else if (info.Type == 2)
                            {
                                info.StrType = resMng.GetString("Type2");
                            }
                            else if (info.Type == 3)
                            {
                                info.StrType = resMng.GetString("Type3");
                            }
                            else
                            {
                                info.StrType = resMng.GetString("Type1");
                            }

                            info.Grade = dtr["Grade"].ToString().ToNumber<int>();

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
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 커뮤니티 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount()
        {
            try
            {          
                int icnt = 0;

                DAL.Com dal = new DAL.Com();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID);

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
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 가입 신청한 커뮤니티 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetJoinPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Ws.Main.Content).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.UserApply dal = new DAL.UserApply();

                DataTableReader dtr = dal.GetJoinPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserApplyInfo info = new BE.UserApplyInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Description = dtr["Description"].ToString();

                            DateTime dt;

                            if (DateTime.TryParse(dtr["ApplyDate"].ToString(), out dt))
                                info.ApplyDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.ApplyDateStr = string.Empty;


                            info.CMNName = dtr["CMNName"].ToString().ToReverseXSS();  
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
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 가입 신청한 커뮤니티 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetJoinListCount()
        {
            try
            {
                int icnt = 0;

                DAL.UserApply dal = new DAL.UserApply();
                icnt = dal.GetJoinPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

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
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 커뮤니티 방문 횟수 & 포인트 넣기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddVisitPoint(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int type = GetDecryptParam("type", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                // 방문 횟수 추가
                DAL.UserVisit dal = new DAL.UserVisit();
                if (!dal.Add(CurrentUserInfo.CompanyID, communityID, CurrentUserInfo.UserID))
                {
                    return false;
                }

                // 포인트 추가 ( 1.글쓰기 2.조회 3.추천 4.짧은답글 5.답변 6.방문 )
                BE.PointInfo pInfo = new BE.PointInfo();
                pInfo.CompanyID = CurrentUserInfo.CompanyID;
                pInfo.CommunityID = communityID;
                pInfo.UserID = CurrentUserInfo.UserID;
                pInfo.Type = type;
                pInfo.Point = 0;

                DAL.Point dalPoint = new DAL.Point();
                if (!dalPoint.Add(pInfo, menuID))
                {
                    return false;
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

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-18
        /// 작성목적 : 리스트별 사용자 정보 설정하기
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

                string communityID = GetDecryptParam("communityID", "0");
                string menuID = GetDecryptParam("menuID", "0");                
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "ML");

                BE.SettingInfo info = new BE.SettingInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID.ToNumber<int>();
                info.MenuID = menuID.ToNumber<int>();
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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-08
        /// 작성목적 : 가입 신청한 커뮤니티 취소하기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUserApply(string p)
        {
            SetDecryptString(p);
            int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();

            string usermail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            BLL.UserApply bll = new BLL.UserApply();
            bll.Del(CurrentUserInfo.CompanyID, communityID, userID, CurrentUserInfo.PreferredLanguage, CurrentUserInfo.LogInID, usermail);
            

            return true;
        }


        #endregion 
    }
}