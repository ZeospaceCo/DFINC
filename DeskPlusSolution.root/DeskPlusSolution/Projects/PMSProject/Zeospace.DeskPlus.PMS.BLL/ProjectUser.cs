using System;
using System.Transactions;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class ProjectUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 참여자 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Del(ProjectUserInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.ProjectUser dal = new DAL.ProjectUser();
                    dal.Del(_listInfo);

                    DAL.ProjectCharger charger = new DAL.ProjectCharger();
                    charger.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.ProjectUserID);

                    DAL.UnitCharger unitCharger = new DAL.UnitCharger();
                    unitCharger.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.ProjectUserID);

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del(ProjectUserInfo _listInfo) : " + ex);
                throw;
            }
        }

        #endregion DEL()
    }
}