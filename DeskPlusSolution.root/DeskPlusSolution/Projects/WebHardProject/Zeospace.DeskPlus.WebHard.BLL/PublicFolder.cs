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
    public class PublicFolder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-16
        /// 작성목적 : 공용폴더의 권한 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public DataSet GetPublicAuthority(int _companyID, int _userID, string[] _deptID, int _folderID)
        {
            Zeospace.DeskPlus.Account.BLL.Dept bll = new Account.BLL.Dept();

            string deptIDList = "";

            if (_deptID != null)
            {
                foreach (string userDept in _deptID)
                {
                    deptIDList += bll.GetUpDeptIDInfo(_companyID, userDept.ToNumber<int>(), true) + ","; //부모 조직도 가져오기
                }
            }

            deptIDList = deptIDList.TrimEnd(',');

            DAL.PublicFolder dal = new DAL.PublicFolder();
            DataSet ds = dal.GetPublicAuthority(_companyID, _userID, deptIDList, _folderID);

            return ds;

        }
        #endregion

        #region ADD()
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-21
        /// 작성목적 : 폴더 사용자 저장
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, int _folderID, string _Auth)
        {
            try
            {
                List<BE.PublicFolderInfo> alPublic = new List<BE.PublicFolderInfo>();

                string publicValue = _Auth.Trim();

                if (publicValue.Length > 0)
                {
                    foreach (string _publicValue in publicValue.Split(','))
                    {
                        //구분$부서아이디값or사용자아이디값$다운로드$업로드
                        string[] arrVal = _publicValue.Split('$');

                        BE.PublicFolderInfo info = new BE.PublicFolderInfo();
                        info.CompanyID = _companyID;
                        info.UserID = arrVal[1].ToNumber<int>();
                        info.FolderID = _folderID;
                        info.TargetType = arrVal[0];

                        if (arrVal[2] == "0" && arrVal[3] == "0")
                        {
                            info.Authority = "3";
                        }
                        else if (arrVal[2] == "1" && arrVal[3] == "0")
                        {
                            info.Authority = "2";
                        }
                        else if (arrVal[2] == "0" && arrVal[3] == "1")
                        {
                            info.Authority = "1";
                        }
                        else
                        {
                            info.Authority = "0";
                        }

                        alPublic.Add(info);
                    }

                    BLL.PublicFolder bll = new BLL.PublicFolder();
                    return bll.AddPublic(alPublic, "U");
                }
                else
                {
                    return DelPublic(_companyID, _userID, _folderID, _Auth);
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-21
        /// 작성목적 : 폴더 사용자 저장
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool AddPublic(List<BE.PublicFolderInfo> _al, string _types)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.PublicFolder dal = new DAL.PublicFolder();

                    // 1. 공유폴더 사용자 전체 삭제
                    if (!dal.Del(_al[0].CompanyID, _al[0].FolderID))
                    {
                        return false;
                    }

                    // 2. Favorite 테이블 삭제 (즐겨찾기)
                    DAL.Favorite dalFav = new DAL.Favorite();
                    if (!dalFav.Del(_al[0].CompanyID, _al[0].FolderID, _types))
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
                    foreach (BE.PublicFolderInfo info in _al)
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPublic", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-21
        /// 작성목적 : 폴더 사용자 저장
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_types"></param>
        /// <returns></returns>
        public bool DelPublic(int _companyID, int _userID, int _folderID, string _Auth)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.PublicFolder dal = new DAL.PublicFolder();

                    // 1. 공유폴더 사용자 전체 삭제
                    if (!dal.Del(_companyID, _folderID))
                    {
                        return false;
                    }

                    // 2. Favorite 테이블 삭제 (즐겨찾기)
                    DAL.Favorite dalFav = new DAL.Favorite();
                    if (!dalFav.Del(_companyID, _folderID, "U"))
                    {
                        return false;
                    }

                    // 3. ImpoUser 테이블 삭제 (중요표시)
                    DAL.ImpoUser dalImpo = new DAL.ImpoUser();
                    if (!dalImpo.Del(_companyID, _folderID, "U"))
                    {
                        return false;
                    }

                    // 4. FolderUser 테이블 삭제 (폴더별 사용자 정보)
                    DAL.FolderUser dalUser = new DAL.FolderUser();
                    if (!dalUser.Del(_companyID, _folderID, "U"))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPublic", ex);
                return false;
            }
        }
        #endregion
    }
}
