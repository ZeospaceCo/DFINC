using System.Collections.Generic;
using System.Transactions;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class ForwardDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #endregion GET

        #region ADD

        /// <summary>
        /// 작성자 : shimpark
        /// 전달자 전송하기
        /// </summary>
        /// <param name="_docInfo">The _doc info.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public bool Add(List<BE.ForwardDocumentInfo> _fList, List<BE.ForwardUserInfo> _userList)
        {
            if (_fList.Count == 0 || _userList.Count == 0) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                int fDocID = 0;
                int companyID = 0, documentID = 0, sendUserID = 0; ;
                string memo = "";

                foreach (var fDoc in _fList)
                {
                    DAL.ForwardDocument dalFD = new DAL.ForwardDocument();
                    fDocID = dalFD.Add(fDoc);
                    companyID = fDoc.CompanyID;
                    documentID = fDoc.DocumentID;
                    memo = fDoc.Memo;

                    if (fDocID > 0)
                    {
                        DAL.ForwardUser dalUser = new DAL.ForwardUser();
                        foreach (var info in _userList)
                        {
                            info.ForwardDocumentID = fDocID;
                            info.DocumentID = fDoc.DocumentID;
                            dalUser.Add(info);
                        }
                    }
                }

                #region 업무톡연동

                if (companyID > 0 && documentID > 0 && !memo.Equals("") && sendUserID > 0)
                {
                    // 업무톡연동
                    BE.ShortReplyInfo be = new BE.ShortReplyInfo();
                    be.CompanyID = companyID;
                    be.DocumentID = documentID;
                    be.Section = "3";
                    be.Content = memo;
                    be.UserID = sendUserID;
                    be.IsOpen = "1";

                    DAL.ShortReply dal = new DAL.ShortReply();
                    dal.Add(be);
                }

                #endregion 업무톡연동

                ts.Complete();
            }

            return true;
        }

        #endregion ADD

        #region MOD

        #endregion MOD

        #region DEL

        #endregion DEL
    }
}