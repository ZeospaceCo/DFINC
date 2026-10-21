using System;
using System.Collections.Generic;
using System.Transactions;

namespace Zeospace.DeskPlus.Account.BLL
{
    public class UserDept : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자 겸직 삭제후 ,  추가하기
        /// </summary>
        /// <param name="_al"></param>
        /// <returns></returns>
        public bool AddUserDept(List<BE.UserDeptInfo> _al)
        {
            try
            {
                if (_al.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.UserDept biz = new DAL.UserDept();
                        biz.DelUserDept(_al[0].CompanyID, _al[0].UserID);

                        foreach (BE.UserDeptInfo be in _al)
                        {
                            biz.AddUserDept(be);
                        }

                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddUserDept : " + ex);
                return false;
            }
        }

        #endregion ADD
    }
}