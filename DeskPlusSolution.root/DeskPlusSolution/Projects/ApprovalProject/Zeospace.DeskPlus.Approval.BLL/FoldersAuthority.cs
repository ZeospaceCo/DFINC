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
    public class FoldersAuthority : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 권한추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_folderID"></param>
        /// <param name="_list"></param>
        /// <param name="_isSub"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _folderID, string _list, int _isSub)
        {
            try
            {
                if (_companyID < 0 || _folderID < 0 || _list.Equals("")) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    string[] arr = _list.Split('|');
                    DAL.FolderAuthority faDal = new FolderAuthority();

                    if (_isSub == 1)
                    {
                        DAL.Folder fDal = new Folder();
                        List<BE.FolderInfo> fsList = fDal.GetSubFolderTreeList(_companyID, _folderID, true);

                        if (fsList != null)
                        {
                            foreach (BE.FolderInfo fInfo in fsList)
                            {
                                foreach (string id in arr)
                                {
                                    BE.FolderAuthorityInfo info = new FolderAuthorityInfo();
                                    info.CompanyID = _companyID;
                                    info.FolderID = fInfo.FolderID;
                                    info.DeptID = id.ToNumber<int>();

                                    if (!faDal.Add(info))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string id in arr)
                        {
                            BE.FolderAuthorityInfo info = new FolderAuthorityInfo();
                            info.CompanyID = _companyID;
                            info.FolderID = _folderID;
                            info.DeptID = id.ToNumber<int>();

                            if (!faDal.Add(info))
                            {
                                return false;
                            }
                        }
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.FoldersAuthority[Add] : " + ex);
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
