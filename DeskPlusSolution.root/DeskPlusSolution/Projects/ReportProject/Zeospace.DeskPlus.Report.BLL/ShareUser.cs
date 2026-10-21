using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Zeospace.DeskPlus.Report.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class ShareUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()
        #endregion

        #region ADD()
        


        #endregion

        #region MOD()


        /// <summary>
        /// Adds the specified _al list.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_alList">The _al list.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _userID, List<BE.ShareUserInfo> _alList)
        {
            if (_alList == null) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.ShareUser dalAuth = new DAL.ShareUser();
                dalAuth.Del(_companyID, _userID);
                
                foreach (var info in _alList)
                {
                    dalAuth.Add(_companyID, _userID, info.TargetUserID, info.UseComplete);
                }

                ts.Complete();
            }

            return true;
        }     
       


        #endregion

        #region DEL()
        #endregion
    }
}
