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

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class Vote : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 투표하기
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool Add(List<BE.VoteInfo> _list)
        {
            try
            {
                if (_list == null) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Vote vDAL = new DAL.Vote();

                    foreach (BE.VoteInfo vInfo in _list)
                    {
                        if (vDAL.Add(vInfo) < 0) return false;
                    }

                    ts.Complete();
                    return true; ;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Vote[Add] : " + ex);
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
