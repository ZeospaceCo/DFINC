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
    public class Reply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        public bool Add(BE.ReplyInfo _rInfo)
        {
            try
            {
                if (_rInfo == null) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Reply rDAL = new DAL.Reply();

                    if (!_rInfo.ReplyID.Equals(0))
                    {
                        if (!rDAL.Del(_rInfo.CompanyID, _rInfo.ReplyID))
                        {
                            return false;
                        }
                    }

                    if (rDAL.Add(_rInfo) < 0)
                    {
                        return false;
                    }

                    ts.Complete();
                    
                    return true;
                }
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
