using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Zeospace.DeskPlus.MDM.BE;

namespace Zeospace.DeskPlus.MDM.BLL
{
    public class AuthGroup
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 사용불가 설정
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public bool Add(List<BE.AuthGroupInfo> _al, int _companyID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.AuthGroup dal = new DAL.AuthGroup();
                    dal.Del(_companyID);

                    foreach (BE.AuthGroupInfo Info in _al)
                    {
                        dal.Add(Info);
                    }       

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return false;
            }
        }
    }
}
