using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.RM.BE;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.RM.BLL
{
    public class ShareUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 공유자 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Add(List<ShareUserInfo> _info, int _reservationID)
        {

            DAL.ShareUser dal = new DAL.ShareUser();
            for (int i = 0; i < _info.Count; i++ )
            {
                ShareUserInfo info = new ShareUserInfo();

                info.CompanyID = _info[i].CompanyID;
                info.ReservationID = _reservationID;
                info.UserID = _info[i].UserID;
                info.DeptID = _info[i].DeptID;
                info.Kind = _info[i].Kind;                

                if (!dal.Add(info))
                {
                    return false;
                }
            }

            return true;            
        }        

        #endregion 

        #region MOD()

        

        #endregion
    }
}
