using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.WebHard.BE;
using System.Transactions;

namespace Zeospace.DeskPlus.WebHard.BLL
{
    public class ShareFolder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 공유한 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataSet GetShareUser(int _companyID, int _userID, string[] _deptID)
        {                        
            Zeospace.DeskPlus.Account.BLL.Dept bll = new Account.BLL.Dept();
            string deptIDList = "";

            if (_deptID != null)
            {
                foreach (string userDept in _deptID)
                {
                    deptIDList += bll.GetUpDeptIDInfo(_companyID, userDept.ToNumber<int>(), true) + ","; //부모 조직도 가져오기
                    //deptIDList += userDept + ",";
                }
            }

            deptIDList = deptIDList.TrimEnd(',');

            DAL.ShareFolder dal = new DAL.ShareFolder();
            DataSet ds = dal.GetShareUser(_companyID, _userID, deptIDList);      

            return ds;

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 공유한 사용자의 공유 폴더 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_shareUserID"></param>
        /// <returns></returns>
        public DataSet GetShareUser(int _companyID, int _userID, string[] _deptID, int _shareUserID)
        {
            Zeospace.DeskPlus.Account.BLL.Dept bll = new Account.BLL.Dept();

            string deptIDList = "";

            if (_deptID != null)
            {
                foreach (string userDept in _deptID)
                {
                    deptIDList += bll.GetUpDeptIDInfo(_companyID, userDept.ToNumber<int>(), true) + ","; //부모 조직도 가져오기
                    //deptIDList += userDept + ",";
                }
            }

            deptIDList = deptIDList.TrimEnd(',');
            
            DAL.ShareFolder dal = new DAL.ShareFolder();
            DataSet ds = dal.GetShareUser(_companyID, _userID, deptIDList, _shareUserID);

            return ds;

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-20
        /// 작성목적 : 공유 받은 폴더의 권한 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public DataSet GetShareAuthority(int _companyID, int _userID, string[] _deptID, int _folderID)
        {
            Zeospace.DeskPlus.Account.BLL.Dept bll = new Account.BLL.Dept();

            string deptIDList = "";

            if (_deptID != null)
            {
                foreach (string userDept in _deptID)
                {
                    deptIDList += bll.GetUpDeptIDInfo(_companyID, userDept.ToNumber<int>(), true) + ","; //부모 조직도 가져오기
                    //deptIDList += userDept + ",";
                }
            }

            deptIDList = deptIDList.TrimEnd(',');

            DAL.ShareFolder dal = new DAL.ShareFolder();
            DataSet ds = dal.GetShareAuthority(_companyID, _userID, deptIDList, _folderID);

            return ds;

        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-28
        /// 작성목적 : 공유 폴더 저장
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool Add(List<BE.ShareFolderInfo> _al, string _types)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ShareFolder dal = new DAL.ShareFolder();

                    // 1. 해당 폴더 공유 정보 전체 삭제
                    if (!dal.Del(_al[0].CompanyID, _al[0].UserID, _al[0].FolderID))
                    {
                        return false;
                    }

                    // 2. Favorite 테이블 삭제 (즐겨찾기)
                    DAL.Favorite dalFav = new DAL.Favorite();
                    if (!dalFav.Del(_al[0].CompanyID, _al[0].UserID, _types))
                    {
                        return false;
                    }

                    // 3. ImpoUser 테이블 삭제 (중요표시)
                    DAL.ImpoUser dalImpo = new DAL.ImpoUser();
                    if (!dalImpo.Del(_al[0].CompanyID, _al[0].FolderID, _types))
                    {
                        return false;
                    }

                    // 4. FolderUser 테이블 삭제 (폴더별 사용자 정보)
                    DAL.FolderUser dalUser = new DAL.FolderUser();
                    if (!dalUser.Del(_al[0].CompanyID, _al[0].FolderID, _types))
                    {
                        return false;
                    }

                    // 5. 해당 폴더의 공유 정보 입력
                    foreach (BE.ShareFolderInfo info in _al)
                    {
                        if (!dal.Add(info))
                        {
                            return false;
                        }
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

        #endregion

        #region MOD()
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-03
        /// 작성목적 : 폴더 공유 해제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _folderID, string _types)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {                     
                    // 1. Favorite 테이블 삭제 (즐겨찾기)
                    DAL.Favorite dalFav = new DAL.Favorite();
                    if (!dalFav.Del(_companyID, _folderID, _types))
                    {
                        return false;
                    }
                                        
                    // 2. ImpoUser 테이블 삭제 (중요표시)
                    DAL.ImpoUser dalImpo = new DAL.ImpoUser();
                    if (!dalImpo.Del(_companyID, _folderID, _types))
                    {
                        return false;
                    }

                    // 3. FolderUser 테이블 삭제 (폴더별 사용자 정보)
                    DAL.FolderUser dalUser = new DAL.FolderUser();
                    if (!dalUser.Del(_companyID, _folderID, _types))
                    {
                        return false;
                    }

                    // 4. ShareFolder 테이블 삭제 (공유)
                    DAL.ShareFolder dal = new DAL.ShareFolder();
                    if (!dal.Del(_companyID, _userID, _folderID))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return false;
            }
        }

        #endregion
    }
}
