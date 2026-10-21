using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Community.BE;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class Menu : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 메뉴 정보 추가 (공지사항, 게시판...)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <returns></returns>
        public int Add(int _companyID, int _communityID)
        {
            try
            {
                int rtnVal = -1;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DataView dvMenu = AppSettingHelper.GetSysCode(_companyID, "CMNMenu");

                    int j = 1;
                    for (int i = 0; i < dvMenu.Count; i++)
                    {
                        if (dvMenu[i]["ActiveCode"].ToString() == "1")
                        {
                            // 공지사항 : 1 , 게시판 : 2 , 자료실 : 3, 앨범형 : 4 까지 초기에 생성해 주기
                            if (dvMenu[i]["InterfaceCode"].ToString().ToNumber<int>() < 5)
                            {
                                BE.MenuInfo info = new BE.MenuInfo();
                                info.CompanyID = _companyID;
                                info.CommunityID = _communityID;
                                info.Sequence = j;
                                info.Type = dvMenu[i]["InterfaceCode"].ToString().ToNumber<int>();
                                info.NameBase = dvMenu[i]["NameBase"].ToString();
                                info.NameKo = dvMenu[i]["NameKo"].ToString();
                                info.NameEn = dvMenu[i]["NameEn"].ToString();
                                info.NameJa = dvMenu[i]["NameJa"].ToString();
                                info.NameFr = dvMenu[i]["NameFr"].ToString();
                                info.NameZhcn = dvMenu[i]["NameZhcn"].ToString();
                                info.NameZhtw = dvMenu[i]["NameZhtw"].ToString();
                                info.Weight = 0;

                                DAL.Menu dal = new DAL.Menu();

                                int tmpMenuID = dal.Add(info);
                                if (tmpMenuID < 1)
                                {
                                    return -1;
                                }

                                if (i == 0)
                                {
                                    rtnVal = tmpMenuID;
                                }

                                //  MenuAuth 테이블 추가
                                BE.MenuAuthInfo menuAuthInfo = new BE.MenuAuthInfo();
                                menuAuthInfo.CompanyID = _companyID;
                                menuAuthInfo.CommunityID = _communityID;
                                menuAuthInfo.MenuID = tmpMenuID;
                                menuAuthInfo.WriteAuth = 9;
                                menuAuthInfo.ReadAuth = 9;

                                DAL.MenuAuth dalMenuAuth = new DAL.MenuAuth();
                                if (!dalMenuAuth.Add(menuAuthInfo))
                                {
                                    return -1;
                                }

                                j++;
                            }
                        }
                    }

                    ts.Complete();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 메뉴 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_authInfo"></param>
        /// <returns></returns>
        public int Add(MenuInfo _info, MenuAuthInfo _authInfo)
        {
            try
            {
                int menuID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Menu 테이블 추가
                    DAL.Menu dal = new DAL.Menu();

                    menuID = dal.MenuAdd(_info);

                    if (menuID < 1)
                    {
                        return -1;
                    }

                    // 2. MenuAuth 테이블 추가 
                    DAL.MenuAuth dalAuth = new DAL.MenuAuth();
                    _authInfo.MenuID = menuID;
                    if (!dalAuth.Add(_authInfo))
                    {
                        return -1;
                    }
                    
                   
                    ts.Complete();
                }

                return menuID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return -1;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 메뉴 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_authInfo"></param>
        /// <returns></returns>
        public bool Mod(MenuInfo _info, MenuAuthInfo _authInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Menu 테이블 수정
                    DAL.Menu dal = new DAL.Menu();

                    if (!dal.Mod(_info))
                    {
                        return false;
                    }


                    // 2. MenuAuth 테이블 추가 
                    DAL.MenuAuth dalAuth = new DAL.MenuAuth();
                    if (!dalAuth.Mod(_authInfo))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod()", ex);
                return false;
            }
        }

        #endregion 

        #region DEL()
        #endregion 
    }
}
