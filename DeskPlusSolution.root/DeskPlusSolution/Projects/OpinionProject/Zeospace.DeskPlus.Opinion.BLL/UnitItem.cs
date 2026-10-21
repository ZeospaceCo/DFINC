using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Opinion.BE;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class UnitItem : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        #endregion

        #region MOD
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

                    DAL.UnitItem uiDal = new DAL.UnitItem();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        uiDal.ModSeq(_companyID, arr[i].ToNumber<int>(), i + 1);
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.UnitItem[ModSeq] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL
        #endregion
    }
}
