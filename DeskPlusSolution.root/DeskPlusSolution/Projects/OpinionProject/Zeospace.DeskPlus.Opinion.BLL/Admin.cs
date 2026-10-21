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
    public class Admin : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 관리자추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_arrUnitID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _quesID, string _arrID)
        {
            try
            {
                if (_companyID.Equals(0) || _quesID.Equals(0) || _arrID.Equals("")) return false;

                string[] arrID = _arrID.Trim(';').Split(';');

                DAL.Admin aDAL = new DAL.Admin();
                foreach (string id in arrID)
                {
                    AdminInfo aInfo = new AdminInfo();
                    aInfo.CompanyID = _companyID;
                    aInfo.QuesID = _quesID;
                    aInfo.UserID = id.ToNumber<int>();

                    aDAL.Add(aInfo);
                }

                return true;
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
        /// 관리자삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_targetID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _adminID)
        {
            try
            {
                if (_companyID.Equals(0) || _adminID.Equals(0)) return false;
                DAL.Admin aDAL = new DAL.Admin();
                return aDAL.DelByOne(_companyID, _adminID);
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
