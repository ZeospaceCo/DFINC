using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;
using Zeospace.DeskPlus.Account.BE;
using System.Web;
using System.Data;

namespace Zeospace.DeskPlus.EIP.BLL
{
    public class MenuDataLink : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// MENU 캐쉬명
        /// </summary>
        private string CacheName = "MenuDataLink_";

        #region 추가/수정/삭제
        #endregion

        #region 가져오기

        /// <summary>
        /// 메뉴아이피 캐쉬 초기화 하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        public void ClearCacheMenuDataLink(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Cache[CacheName + _companyID] != null) Cache.Remove(CacheName + _companyID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 MenuIP 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.MenuDataLinkInfo> SetCacheMenuDataLink(int _companyID)
        {
            HttpContext context = HttpContext.Current;
            List<BE.MenuDataLinkInfo> al = new List<BE.MenuDataLinkInfo>();

            if (context.Cache[CacheName + _companyID] == null)
            {
                //모든 사용자 가져오기(활성화 및 겸직 제외)
                DAL.MenuDataLink dal = new DAL.MenuDataLink();
                al = dal.GetDataLink(_companyID);
                HttpContext ctx = HttpContext.Current;
                ctx.Cache.Insert(CacheName + _companyID, al, null, DateTime.Now.AddDays(7), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else if (context.Cache[CacheName + _companyID] != null)
            {
                al = (List<BE.MenuDataLinkInfo>)context.Cache[CacheName + _companyID];
            }

            return al;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 메뉴의 파라미터 값 가져오기(배열 [0] : 암호화; [1] 비암호화)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_ldapInfo">The _ldap info.</param>
        /// <returns></returns>
        public string[] GetMenuParameter(int _companyID, int _menuID, int _userID, BE.LDAPInfo _ldapInfo)
        {
            try
            {
                List<BE.MenuDataLinkInfo> alMenu = SetCacheMenuDataLink(_companyID);

                var list = from m in alMenu
                           where m.MenuID == _menuID
                           select m;

                StringBuilder sbEnc = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                string linkValue = String.Empty;
                string[] arrRtnVal = new string[2];

                foreach (BE.MenuDataLinkInfo _info in list)
                {
                    //OL : LDAP 쿼리; OU : 사용자 입력값
                    linkValue = (_info.LinkType == "OL") ? GetLinkValue(_ldapInfo, _info.LinkValue) : _info.LinkValue;

                    if (_info.EncriptValue == "1") //파라미터 암호화
                    {                        
                        sbEnc.Append(_info.LinkName);
                        sbEnc.Append("=");
                        sbEnc.Append(linkValue);
                        sbEnc.Append("&");
                    }
                    else
                    {                        
                        sb.Append(_info.LinkName);
                        sb.Append("=");
                        sb.Append(linkValue);
                        sb.Append("&");
                    }
                }
                arrRtnVal[0] = sbEnc.ToString().TrimEnd('&');//암호화 파라미터
                arrRtnVal[1] = sb.ToString().TrimEnd('&');//순수 파라미터
                return arrRtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Menu.cs[GetMenuListUser] : " + ex);
            }

            return null;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// ldap 값 가져오기
        /// </summary>
        /// <param name="ldapInfo">The LDAP info.</param>
        /// <param name="_linkValue">The _link value.</param>
        /// <returns></returns>
        public string GetLinkValue(BE.LDAPInfo ldapInfo, string _linkValue)
        {
            string linkValue = "";

            switch (_linkValue)
            {
                case "CompanyID":
                    linkValue = ldapInfo.CompanyID.ToString();
                    break;
                case "UserID":
                    linkValue = ldapInfo.UserID;
                    break;
                case "DisplayName":
                    linkValue = ldapInfo.DisplayName;
                    break;
                case "EmployeeNumber":
                    linkValue = ldapInfo.EmployeeNumber;
                    break;
                case "DepartmentNumber":
                    linkValue = ldapInfo.DepartmentNumber.ToString();
                    break;
                case "Mail":
                    linkValue = ldapInfo.Mail;
                    break;
                case "UserPassword":
                    linkValue = ldapInfo.UserPassword;
                    break;
                case "SN":
                    linkValue = ldapInfo.SN;
                    break;
                case "GivenName":
                    linkValue = ldapInfo.GivenName;
                    break;
                case "Initials":
                    linkValue = ldapInfo.Initials;
                    break;
                case "Title":
                    linkValue = ldapInfo.Title;
                    break;
                case "Telephone":
                    linkValue = ldapInfo.Telephone;
                    break;
                case "Fax":
                    linkValue = ldapInfo.Fax;
                    break;
                case "Mobile":
                    linkValue = ldapInfo.Mobile;
                    break;
                case "RoomNumber":
                    linkValue = ldapInfo.RoomNumber;
                    break;
                case "O":
                    linkValue = ldapInfo.O;
                    break;
                case "OU":
                    linkValue = ldapInfo.OU;
                    break;
                case "EmployeeType":
                    linkValue = ldapInfo.EmployeeType;
                    break;
                case "PreferredLanguage":
                    linkValue = ldapInfo.PreferredLanguage;
                    break;
                case "LabeledURI":
                    linkValue = ldapInfo.LabeledURI;
                    break;
                case "UserCode":
                    linkValue = ldapInfo.UserCode;
                    break;
                case "Description":
                    linkValue = ldapInfo.Description;
                    break;
                case "MailPassword":
                    linkValue = ldapInfo.MailPassword;
                    break;
            }

            return linkValue;
        }

        #endregion
    }
}
