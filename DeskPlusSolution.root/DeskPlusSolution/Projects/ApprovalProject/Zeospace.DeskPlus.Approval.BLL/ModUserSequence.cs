using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class ModUserSequence : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region MOD()
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2016-03-31
        /// 작성목적 : 정렬순서변경
        /// </summary>
        /// <returns></returns>
        public bool ModSeq(int _companyID, string _sortlist)
        {
            try
            {
                if (_companyID < 0 || _sortlist.Equals("")) return false;

                string[] arr = _sortlist.TrimEnd(',').Split(',');

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ModUserSequence dal = new DAL.ModUserSequence();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (!dal.ModSeq(_companyID, arr[i].ToNumber<int>(), i + 1))
                        {
                            return false;
                        }
                    }
                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModSeq : " + ex);
                return false;
            }
        }
        #endregion
    }
}
