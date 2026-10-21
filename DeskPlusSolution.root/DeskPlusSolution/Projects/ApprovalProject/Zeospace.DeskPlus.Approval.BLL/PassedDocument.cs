using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class PassedDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD

        /// <summary>
        /// 작성자 : 심재운
        /// 전달받은 문서
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_docID">The _doc ID.</param>
        /// <param name="_recvUser">The _recv user.</param>
        /// <returns></returns>
        public bool AddPassedByDoc(int _companyID, int _userID, string _docID, string _recvUser)
        {
            try
            {
                if (_docID.Length.Equals(0) || _recvUser.Length.Equals(0)) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.PassedDocument dal = new DAL.PassedDocument();

                    foreach (string tmpRecvUser in _recvUser.Split(','))
                    {
                        foreach (string tmpDoc in _docID.Split(','))
                        {
                            BE.PassedDocumentInfo be = new BE.PassedDocumentInfo();
                            be.CompanyID = _companyID;
                            be.DocumentID = tmpDoc.ToNumber<int>();
                            be.SendUserID = _userID;
                            be.ReceiveUserID = tmpRecvUser.ToNumber<int>();

                            dal.AddByDoc(be);
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPassedByDoc : " + ex);
                return false;
            }
        }


        #endregion

        #region MOD
        #endregion

        #region DEL
        #endregion
    }
}
