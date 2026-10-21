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
    public class Target : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 대상자추가
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
                
                DAL.Target tDAL = new DAL.Target();
                foreach (string id in arrID)
                {
                    TargetInfo tInfo = new TargetInfo();
                    tInfo.CompanyID = _companyID;
                    tInfo.QuesID = _quesID;
                    tInfo.Type = id.Split('-')[0];
                    tInfo.OwnerID = id.Split('-')[1].ToNumber<int>();

                    tDAL.Add(tInfo);
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
        /// 대상자삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_targetID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _targetID)
        {
            try
            {
                if (_companyID.Equals(0) || _targetID.Equals(0)) return false;
                DAL.Target tDAL = new DAL.Target();
                return tDAL.Del(_companyID, _targetID);
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
