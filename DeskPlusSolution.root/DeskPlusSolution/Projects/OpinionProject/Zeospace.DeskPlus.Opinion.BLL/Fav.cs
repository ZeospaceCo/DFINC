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
    public class Fav : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 중요표시 추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, int _quesID)
        {
            try
            {
                if (_companyID.Equals(0) || _userID.Equals(0) || _quesID.Equals(0)) return false;

                FavInfo fInfo = new FavInfo();
                fInfo.CompanyID = _companyID;
                fInfo.UserID = _userID;
                fInfo.QuesID = _quesID;

                DAL.Fav fDAL = new DAL.Fav();
                return fDAL.Add(fInfo);
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
        /// <summary>
        /// 중요표시 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_quesID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _quesID)
        {
            try
            {
                if (_companyID.Equals(0) || _userID.Equals(0) || _quesID.Equals(0)) return false;
                DAL.Fav fDAL = new DAL.Fav();
                return fDAL.Del(_companyID, _userID, _quesID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }
        #endregion
    }
}
