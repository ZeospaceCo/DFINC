using System;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.BLL
{
    public class ForwardList : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가

        /// <summary>
        /// Adds the list.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <param name="_sendUserID">The _send user ID.</param>
        /// <param name="_forwardUserID">The _forward user ID.</param>
        /// <returns></returns>
        public int AddList(int _companyID, int _groupID, int _sendUserID, int _forwardUserID)
        {
            try
            {
                DAL.ForwardList dalFWList = new DAL.ForwardList();
                int forwardListID = dalFWList.Add(_companyID, _groupID, _sendUserID, _forwardUserID);

                if (forwardListID == 0) return -1;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ForwardAttach dalFWAtt = new DAL.ForwardAttach();
                    DAL.ForwardListOption dalFWOption = new DAL.ForwardListOption();
                    DAL.ForwardBaseDoc dalFWBaseDoc = new DAL.ForwardBaseDoc();
                    DAL.ForwardBaseCoop dalFWBaseCoop = new DAL.ForwardBaseCoop();

                    var dtrList = dalFWList.GetListByGroup(_companyID, _forwardUserID, forwardListID);

                    if (dtrList != null)
                    {
                        if (dtrList.HasRows)
                        {
                            while (dtrList.Read())
                            {
                                dalFWAtt.Add(_companyID, dtrList["ListID"].ToString().ToNumber<int>(), dtrList["ForwardListID"].ToString().ToNumber<int>());
                                dalFWBaseDoc.Add(_companyID, dtrList["ListID"].ToString().ToNumber<int>(), dtrList["ForwardListID"].ToString().ToNumber<int>());
                                dalFWBaseCoop.Add(_companyID, dtrList["ListID"].ToString().ToNumber<int>(), dtrList["ForwardListID"].ToString().ToNumber<int>());
                            }
                        }
                    }

                    ts.Complete();
                }

                return forwardListID;
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return -1;
            }
        }

        #endregion 추가
    }
}