using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Opinion.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class Setting : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        /// <summary>
        /// 사용자 메뉴별환경설정 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_menuInfo"></param>
        /// <returns></returns>
        public List<BE.SettingInfo> GetSetting(int _companyID, int _userID, string _menuInfo)
        {
            DAL.Setting sDAL = new DAL.Setting();
            return sDAL.GetSetting(_companyID, _userID, _menuInfo);
        }

        #endregion

        #region ADD
        /// <summary>
        /// 사용자별 환경설정 추가/수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_menuInfo"></param>
        /// <param name="_listType"></param>
        /// <param name="_pageSize"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, string _menuInfo, string _listType, int _pageSize)
        {
            try
            {
                if (_companyID.Equals(0) || _userID.Equals(0) || _menuInfo.Equals("") || _listType.Equals("") || _pageSize.Equals(0) ) return false;

                SettingInfo sInfo = new SettingInfo();
                sInfo.CompanyID = _companyID;
                sInfo.UserID = _userID;
                sInfo.MenuInfo = _menuInfo;
                sInfo.ListType = _listType;
                sInfo.PageSize = _pageSize;

                DAL.Setting sDAL = new DAL.Setting();
                return sDAL.Add(sInfo);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }
        #endregion

        #region MOD
        #endregion

        #region DEL
        #endregion
    }
}
