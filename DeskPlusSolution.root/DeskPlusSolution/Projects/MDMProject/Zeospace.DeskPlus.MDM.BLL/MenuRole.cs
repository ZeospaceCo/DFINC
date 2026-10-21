using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;

namespace Zeospace.DeskPlus.MDM.BLL
{
    public class MenuRole : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-01-17
        /// 작성목적 : 권한 추가하기
        /// </summary>
        /// <param name="_al"></param>
        /// <returns></returns>
        public bool AddTx(List<BE.MenuRoleInfo> _al)
        {
            try
            {
                if (_al.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.MenuRole dal = new DAL.MenuRole();
                        foreach (BE.MenuRoleInfo info in _al) dal.Add(info);

                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddTx : " + ex);
                return false;
            }

        }

        #endregion
    }
}
