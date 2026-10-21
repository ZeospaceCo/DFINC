using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.BLL
{
    public class Menu : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 사용자 접근 가능한 메뉴 정보 캐쉬
        /// </summary>
        private string CacheNameMenuListUser = "MenuListUser_";

        /// <summary>
        /// 메뉴 캐쉬
        /// </summary>
        private string CacheNameMenuList = "MenuList_";

        #region 가져오기

        #region 메뉴 관련

        /// <summary>
        /// 작성자 : 심재운
        /// 메뉴 캐쉬 초기화 하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        public void ClearCacheMenu(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Cache[CacheNameMenuList + _companyID] != null) Cache.Remove(CacheNameMenuList + _companyID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// Menu 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet SetCacheMenuList(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            DataSet dsMenu = new DataSet();

            if (context.Cache[CacheNameMenuList + _companyID] == null)
            {
                DAL.Menu menuDal = new DAL.Menu();
                dsMenu = menuDal.GetMenu(_companyID);

                HttpContext ctx = HttpContext.Current;
                ctx.Cache.Insert(CacheNameMenuList + _companyID, dsMenu, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else if (context.Cache[CacheNameMenuList + _companyID] != null)
            {
                dsMenu = (DataSet)context.Cache[CacheNameMenuList + _companyID];
            }

            return dsMenu;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전체 메뉴, 데이터연동, IP 초기화 하기
        /// </summary>
        /// <param name="_companyID"></param>
        public void ResetMenuAuth(int _companyID)
        {
            #region 초기화 하기

            Zeospace.DeskPlus.EIP.BLL.MenuDataLink bizDataLink = new Zeospace.DeskPlus.EIP.BLL.MenuDataLink();
            Zeospace.DeskPlus.EIP.BLL.MenuIP bizMenuIP = new Zeospace.DeskPlus.EIP.BLL.MenuIP();

            #region 캐쉬 삭제

            //데이터연동 정보 - 캐쉬 초기화
            bizDataLink.ClearCacheMenuDataLink(_companyID);

            //메뉴별 IP 정보 - 캐쉬 초기화
            bizMenuIP.ClearCacheMenuIP(_companyID);

            //메뉴 - 캐쉬 초기화
            ClearCacheMenu(_companyID);

            //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
            ClearCacheMenuUser(_companyID);

            #endregion 캐쉬 삭제

            #region 캐쉬 설정

            //데이터연동 정보 - 캐쉬설정
            bizDataLink.SetCacheMenuDataLink(_companyID);

            //메뉴별 IP 정보 - 캐쉬설정
            bizMenuIP.SetCacheMenuIP(_companyID);

            //메뉴 - 캐쉬설정
            SetCacheMenuList(_companyID);

            //사용자별 메뉴 정보 - 캐쉬설정
            SetCacheMenuListUser(_companyID);

            #endregion 캐쉬 설정

            #endregion 초기화 하기
        }

        #endregion 메뉴 관련

        #region 사용자별 메뉴 관련

        /// <summary>
        /// 사용자 접근 가능한 메뉴 정보 캐쉬 초기화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        public void ClearCacheMenuUser(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Cache[CacheNameMenuListUser + _companyID] != null) Cache.Remove(CacheNameMenuListUser + _companyID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 사용자들 MenuListUser 캐쉬에 담기
        /// 문제 생기면, 겸직부서(basedept = 1 이 두개 이상 및 환경정보에 2번 이상 저장되었는지 확인)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public Dictionary<int, string> SetCacheMenuListUser(int _companyID)
        {
            Dictionary<int, string> dicMenuList = new Dictionary<int, string>();

            try
            {
                HttpContext context = HttpContext.Current;

                if (context.Cache[CacheNameMenuListUser + _companyID] == null)
                {
                    //모든 사용자 가져오기(활성화 및 겸직 제외)
                    Account.DAL.User dalUser = new Account.DAL.User();
                    List<UserInfo> alUserList = dalUser.GetUserInfo(_companyID, true);

                    string csv = "";

                    DAL.Menu menuDal = new DAL.Menu();
                    foreach (Account.BE.UserInfo userInfo in alUserList)
                    {
                        //ROLE 로 접근 가능한 MENU ID 값을 콤마(,) 1차원 배열로 저장한다.
                        csv = String.Join(",", menuDal.GetMenuByUser(_companyID, userInfo.DeptID, userInfo.UserID).Select(x => x.ToString()).ToArray());

                        dicMenuList.Add(userInfo.UserID, csv);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheNameMenuListUser + _companyID, dicMenuList, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else if (context.Cache[CacheNameMenuListUser + _companyID] != null)
                {
                    dicMenuList = (Dictionary<int, string>)context.Cache[CacheNameMenuListUser + _companyID];
                }

                return dicMenuList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Dictionary<int, string> SetCacheMenuListUser : " + ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 이지원
        /// 회사별 사용자들 MenuListUser 캐쉬에 담기
        /// 문제 생기면, 겸직부서(basedept = 1 이 두개 이상 및 환경정보에 2번 이상 저장되었는지 확인)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public Dictionary<int, string> SetCacheMenuListUserAllOne(int _companyID)
        {
            Dictionary<int, string> dicMenuList = new Dictionary<int, string>();

            try
            {
                HttpContext context = HttpContext.Current;


                if (context.Cache[CacheNameMenuListUser + _companyID] == null)
                {
                    DAL.Menu menuDal = new DAL.Menu();

                    Dictionary<int, string> alMenuList = menuDal.GetMenuByUserAllOne(_companyID);

                    foreach (KeyValuePair<int, string> L in alMenuList)
                    {
                        dicMenuList.Add(L.Key, L.Value);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheNameMenuListUser + _companyID, dicMenuList, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else if (context.Cache[CacheNameMenuListUser + _companyID] != null)
                {
                    dicMenuList = (Dictionary<int, string>)context.Cache[CacheNameMenuListUser + _companyID];
                }

                return dicMenuList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Dictionary<int, string> SetCacheMenuListUserAllOne : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 사용자별 접근 가능한 MENU ID 설정하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_ipAddress">The _ip address.</param>
        /// <returns></returns>
        public List<string> GetMenuListUser(int _companyID, int _userID, double _ipAddress)
        {
            //회사별 전체 ROLE 권한체크한 사용자들의 메뉴 가져오기(IP 는 제외됨)
            Dictionary<int, string> getMenuListUser = SetCacheMenuListUser(_companyID);

            List<string> myAccessMenuList = new List<string>();
            int myUserID = 0;
            string myTotalMenuIDs = "";

            if (getMenuListUser.Count() > 0)
            {
                //특정 사용자의 ROLE 권한체크한 MENU ID 추출하기
                var myMenuListUserDic = (from entry in getMenuListUser
                                         where entry.Key == _userID
                                         select entry);

                if (myMenuListUserDic.Count() > 0)
                {
                    //########## 1. ROLE 권한체크한 해당사용자 MENU ID 값을 가져온다. ####
                    foreach (KeyValuePair<int, string> data in myMenuListUserDic)
                    {
                        myUserID = data.Key;//user id
                        myTotalMenuIDs = data.Value;//접근 가능 menu id 값들

                        //나의 접근 가능한 Menu ID 를 배열에 담는다.
                        List<string> myTotalMenuList = new List<string>(myTotalMenuIDs.Split(','));

                        //########## 2.ROLE 권한체크한 해당사용자 MENU ID 값으로 IP 체크한다. ####

                        DAL.MenuIP menuIP = new DAL.MenuIP();
                        BLL.MenuIP bizMenuIP = new MenuIP();

                        //Menu ID 별로 IP 정보 가져오기
                        List<BE.MenuIPInfo> menuIPList = bizMenuIP.SetCacheMenuIP(_companyID);

                        //if (menuIPList == null)
                        //{
                        //    AppException.WriteAssert("SetMenuListUser : menuIPList null / userid - " + _userID + " / ipAddress - " + _ipAddress, AppSettingType.EIP);
                        //}

                        foreach (string menuID in myTotalMenuList)
                        {
                            if (menuID == "0")
                            {
                                continue;
                            }

                            #region Menu ID 별로 IP 접근 정보 가져오기

                            var ipMenuLists = (from entry in menuIPList
                                               where entry.MenuID == menuID.ToNumber<int>()
                                               select entry);

                            if (ipMenuLists == null)
                            {
                                myAccessMenuList.Add(menuID);
                                continue;
                            }

                            //### IP값이 존재할 경우
                            if (ipMenuLists.Count() > 0)
                            {
                                double stratIP = 0;
                                double endIP = 0;

                                bool access = false; //우선 막아준다.

                                foreach (BE.MenuIPInfo ipInfo in ipMenuLists)
                                {
                                    stratIP = ipInfo.StartIP.ToDouble<double>();
                                    endIP = ipInfo.EndIP.ToDouble<double>();

                                    //[차단] ip 값이 존재하면 접근 불가며, 차단처리하고 빠져나온다.
                                    if (ipInfo.IPAccess == "1") //0:허용; 1:차단
                                    {
                                        if (stratIP <= _ipAddress && endIP >= _ipAddress)
                                        {
                                            access = false; break;
                                        }
                                    }

                                    //[허용] ip값이 존재할 경우, 차단 값이 존재할 수있으니 계속 루프로 확인한다.
                                    if (ipInfo.IPAccess == "0") //0:허용; 1:차단
                                    {
                                        if (stratIP <= _ipAddress && endIP >= _ipAddress) access = true;
                                    }
                                }

                                //허용일 경우, 메뉴값을 저장한다.
                                if (access) myAccessMenuList.Add(menuID);
                            }
                            else
                            {
                                //IP 값이 없을 경우, 허용해 준다.
                                myAccessMenuList.Add(menuID);
                            }

                            #endregion Menu ID 별로 IP 접근 정보 가져오기
                        }
                    }
                }
            }

            return myAccessMenuList;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 특정 사용자가 볼수 있는 메뉴 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_ipAddress">The _ip address.</param>
        /// <returns></returns>
        public DataView GetMenuUser(int _companyID, int _userID, double _ipAddress)
        {
            try
            {
                string getMyMenuList = string.Empty;
                List<string> menuListUser = GetMenuListUser(_companyID, _userID, _ipAddress);
                if (menuListUser.Count() > 0) getMyMenuList = String.Join(",", menuListUser.Select(x => x.ToString()).ToArray());
                if (string.IsNullOrEmpty(getMyMenuList)) return null;

                AppException.WriteAssert("GetMenuUser : userid - " + _userID + " / getMyMenuList - " + getMyMenuList + " / ipAddress - " + _ipAddress, AppSettingType.EIP);

                DataSet dsMenuList = SetCacheMenuList(_companyID);
                if (dsMenuList != null)
                {
                    DataView dv = dsMenuList.Tables[0].DefaultView;
                    dv.RowFilter = "MenuID IN (" + getMyMenuList + ")";
                    return dv;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuUser : " + ex);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_ipAddress"></param>
        /// <returns></returns>
        public DataView GetFaMenuUser(int _companyID, int _userID, double _ipAddress)
        {
            try
            {
                string getMyMenuList = string.Empty;
                List<string> menuListUser = GetMenuListUser(_companyID, _userID, _ipAddress);
                if (menuListUser.Count() > 0) getMyMenuList = String.Join(",", menuListUser.Select(x => x.ToString()).ToArray());
                if (string.IsNullOrEmpty(getMyMenuList)) return null;

                DAL.Menu dal = new DAL.Menu();
                DataSet dsMenuList = dal.GetFaMenuUser(_companyID, _userID);
                if (dsMenuList != null)
                {
                    DataView dv = dsMenuList.Tables[0].DefaultView;
                    dv.RowFilter = "MenuID IN (" + getMyMenuList + ")";
                    return dv;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetFaMenuUser : " + ex);
            }

            return null;
        }

        #endregion 사용자별 메뉴 관련

        #endregion 가져오기
    }
}