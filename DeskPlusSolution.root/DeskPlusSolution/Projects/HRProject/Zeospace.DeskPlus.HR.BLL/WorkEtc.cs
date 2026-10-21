using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using System.Web;
using System.Transactions;

namespace Zeospace.DeskPlus.HR.BLL
{
    public class WorkEtc
    {
        #region ADD
        /// <summary>
        /// 유연근무 대상 추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_arrUnitID"></param>
        /// <param name="_WSTime"></param>
        /// <param name="_WCTime"></param>
        /// <returns></returns>
        public bool Add(int _companyID, string _arrID, DateTime _WSTime, DateTime _WCTime, string _searchWeek)
        {
            try
            {
                if (_companyID.Equals(0) || _arrID.Equals("") || _WSTime.Equals("") || _WCTime.Equals("") || _searchWeek.Equals("")) return false;

                string[] arrID = _arrID.Trim(';').Split(';');

                DAL.WorkEtc dal = new DAL.WorkEtc();
                foreach (string id in arrID)
                {
                    //구분$아이디
                    string[] authValStr = id.Split('$');

                    BE.WorkEtcInfo aInfo = new WorkEtcInfo();
                    aInfo.CompanyID = _companyID;
                    aInfo.UserID = authValStr[1].ToNumber<int>();
                    aInfo.WSTime = _WSTime;
                    aInfo.WCTime = _WCTime;
                    aInfo.WeekDays = _searchWeek;
                    aInfo.Types = (authValStr[0] == "G") ? "GG" : "GM";

                    dal.Add(aInfo);
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

        #region DEL
        /// <summary>
        /// 유연근무 대상 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_workEtcID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _workEtcID)
        {
            try
            {
                if (_companyID.Equals(0) || _workEtcID.Equals(0)) return false;
                DAL.WorkEtc dal = new DAL.WorkEtc();
                return dal.Del(_companyID, _workEtcID);
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
