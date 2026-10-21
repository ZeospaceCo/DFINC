using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.BLL
{
    public class UserCompany : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 타회사의 사용자 매칭 시켜주기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_groupGUID">The _group GUID.</param>
        /// <param name="_multiCompanyID">The _multi company ID.</param>
        /// <param name="_newUserDeptList">The _new user dept list.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, int _writeUserID,
            string _groupGUID,
            int _multiCompanyID,
            List<BE.UserDeptInfo> _newUserDeptList)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    BE.UserCompanyInfo infoCom = new UserCompanyInfo();
                    DAL.UserCompany bizCom = new DAL.UserCompany();
                    //DAL.UserDept biz = new DAL.UserDept();

                    //기존 사용자를 전부 삭제한다.
                    //if (_oldUserDeptList.Count > 0)
                    //{
                    //    //기존 사용자를 전부 삭제한다.
                    //    //biz.DelUserDept(_oldUserDeptList[0].CompanyID, _oldUserDeptList[0].UserID);

                    //    //기존 타회사정보를 삭제한다.
                    //    bizCom.Del(_oldUserDeptList[0].CompanyID, _groupGUID);
                    //}

                    //추가할 타회사 사용자가 존재할 경우
                    if (_newUserDeptList.Count > 0)
                    {
                        ////추가할 사용자를 인사정보(UserDept)에 추가한다.
                        //foreach (BE.UserDeptInfo be in _newUserDeptList)
                        //{
                        //    biz.AddUserDept(be);
                        //}

                        //호출한 회사와 함께 userCompany 에 추가한다. (이미 있으면 제외된다.)
                        infoCom.CompanyID = _companyID;
                        infoCom.UserID = _userID;
                        infoCom.GroupGUID = _groupGUID;
                        infoCom.WriteUserID = _writeUserID;
                        bizCom.Add(infoCom);

                        //추가할 타회사 사용자를 userCompany 에 추가한다. (이미 있으면 제외된다.)
                        infoCom.CompanyID = _newUserDeptList[0].CompanyID;
                        infoCom.UserID = _newUserDeptList[0].UserID;
                        infoCom.GroupGUID = _groupGUID;
                        infoCom.WriteUserID = _writeUserID;
                        bizCom.Add(infoCom);
                    }
                    else if (_newUserDeptList.Count == 0) //없을 경우
                    {
                        bizCom.Del(_multiCompanyID, _groupGUID);
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        #endregion ADD
    }
}