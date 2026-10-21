using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;

using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class HolidayD : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region Add
        public void Add(BE.HolidayDInfo _info)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.HolidayD dal = new DAL.HolidayD();
                dal.Add(_info);
                ts.Complete();
            }
        }
        #endregion

        #region Mod
        public void Mod(BE.HolidayDInfo _info)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.HolidayD dal = new DAL.HolidayD();
                dal.Mod(_info);
                ts.Complete();
            }
        }
        #endregion
    }
}
