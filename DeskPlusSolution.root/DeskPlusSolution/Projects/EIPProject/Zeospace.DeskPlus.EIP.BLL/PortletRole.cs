using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;

namespace Zeospace.DeskPlus.EIP.BLL
{
    public class PortletRole : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 추가하기
        /// </summary>
        /// <param name="_al">The _al.</param>
        /// <returns></returns>
        public bool AddTx(List<BE.PortletRoleInfo> _al)
        {
            try
            {
                if (_al.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.PortletRole dal = new DAL.PortletRole();
                        //dal.DelByPortletID(_al[0].CompanyID, _al[0].PortletID);

                        foreach (BE.PortletRoleInfo info in _al) dal.Add(info);

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

        #region 가져오기
        #endregion
    }
}
