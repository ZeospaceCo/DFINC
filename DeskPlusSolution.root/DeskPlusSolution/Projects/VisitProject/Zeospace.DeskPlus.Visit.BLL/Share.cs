using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Zeospace.DeskPlus.Visit.BE;

namespace Zeospace.DeskPlus.Visit.BLL
{
    public class Share
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 기본 공유자 설정
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public bool Add(List<BE.ShareInfo> _al, int _companyID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Share dal = new DAL.Share();
                    dal.Del(_companyID);

                    foreach (BE.ShareInfo shareInfo in _al)
                    {
                        dal.Add(shareInfo);
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
