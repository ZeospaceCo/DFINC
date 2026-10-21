using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.BLL
{
    public class Forward : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 전달하기 

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-22
        /// 작성목적 : 전달하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_userList"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _listID, int _userID, string _senderName, string _userList, string _preLang)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Forward dal = new DAL.Forward();

                    foreach (string forwardUserID in _userList.Split(','))
                    {
                        if (forwardUserID.ToNumber<int>() > 0)
                        {
                            dal.Add(_companyID, _listID, _userID, forwardUserID.ToNumber<int>());
                        }
                    }

                    ts.Complete();

                }

                #region 알람
                BLL.Alarm bll = new BLL.Alarm();
                bll.AlarmForward(_companyID, _listID, _userID, _senderName, _userList, _preLang);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return false;
            }
        }

        #endregion 
    }
}
