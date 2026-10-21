using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Zeospace.DeskPlus.Proposal.BLL
{
    public class Judge : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        #endregion

        #region MOD

        /// <summary>
        /// 1차 심사위원 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_alList">The _al list.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, List<BE.JudgeInfo> _alList)
        {
            if (_alList == null) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.Judge dalAuth = new DAL.Judge();
                dalAuth.Del(_companyID);

                foreach (var info in _alList)
                {
                    dalAuth.Add(_companyID, info.UserID, info.Degree, info.Status);
                }

                ts.Complete();
            }

            return true;
        }

        #endregion

        #region DEL
        #endregion
    }
}
