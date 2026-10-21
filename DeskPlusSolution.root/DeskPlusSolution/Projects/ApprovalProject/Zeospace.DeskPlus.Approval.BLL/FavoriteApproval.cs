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
    public class FavoriteApproval : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
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

                string[] arr = _sortlist.TrimEnd(',').Split(',');

                DAL.FavoriteApproval fDal = new DAL.FavoriteApproval();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!fDal.Mod(_companyID, arr[i].ToNumber<int>(), i + 1))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.FavoriteApproval[ModSeq] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL
        #endregion
    }
}
