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
    public class QuesUnit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 설문-질문추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesID"></param>
        /// <param name="_arrUnitID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _quesID, string _arrUnitID)
        {
            try
            {
                if (_companyID.Equals(0) || _quesID.Equals(0) || _arrUnitID.Equals("")) return false;

                string[] arrID = _arrUnitID.Trim(',').Split(',');

                DAL.QuesUnit quDAL = new DAL.QuesUnit();
                foreach (string id in arrID)
                {
                    QuesUnitInfo quInfo = new QuesUnitInfo();
                    quInfo.CompanyID = _companyID;
                    quInfo.QuesID = _quesID;
                    quInfo.UnitID = id.ToNumber<int>();

                    quDAL.Add(quInfo);
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
        /// 설문-질문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_quesUnitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesUnitID)
        {
            try
            {
                if (_companyID.Equals(0) || _quesUnitID.Equals(0)) return false;
                DAL.QuesUnit quDAL = new DAL.QuesUnit();
                return quDAL.Del(_companyID, _quesUnitID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }
        /// <summary>
        /// 항목순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sortlist"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, string _sortlist)
        {
            try
            {
                if (_companyID < 0 || _sortlist.Equals("")) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    string[] arr = _sortlist.TrimEnd(',').Split(',');

                    DAL.QuesUnit quDal = new DAL.QuesUnit();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        quDal.ModSeq(_companyID, arr[i].ToNumber<int>(), i + 1);
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.QuesUnit[ModSeq] : " + ex);
                return false;
            }
        }
        #endregion
    }
}
