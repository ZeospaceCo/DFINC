using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Proposal.BLL
{
    public class JudgeUser : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region MOD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 심사자 수정
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _proposalNo, string _userList)
        {
            if (_companyID < 1 || _proposalNo < 1 || _userList.Length < 0)
                return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.JudgeUser dal = new DAL.JudgeUser();
                // 기존의 1차 심사자 모두 삭제
                dal.Del(_companyID, _proposalNo);

                foreach (string userID in _userList.Split(','))
                {
                    dal.Add(_companyID, _proposalNo, userID.ToNumber<int>());
                }
                ts.Complete();
            }

            return true;
        }

        #endregion
    }
}
