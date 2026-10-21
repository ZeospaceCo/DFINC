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
    public class AccessGrade : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region MOD
        /// 순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sortlist"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, string _sortlist)
        {
            try
            {
                if (_companyID < 0 || _sortlist.Equals("")) return false;

                string[] arr = _sortlist.TrimEnd(',').Split(',');

                DAL.AccessGrade fDal = new DAL.AccessGrade();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!fDal.ModSeq(_companyID, arr[i].ToNumber<int>(), i + 1))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.AccessGrade[ModSeq] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 활성/비활성화 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _accessID, string _isAccess)
        {
            try
            {
                if (_companyID < 0 || _accessID < 0 || _isAccess.Equals("")) return false;

                DAL.AccessGrade f = new DAL.AccessGrade();
                return f.UpdActive(_companyID, _accessID, _isAccess);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.AccessGrade[UpdActive] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL
        /// <summary>
        /// 양식삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _accessID)
        {
            try
            {
                if (_companyID.Equals(0) || _accessID.Equals(0)) return false;
                DAL.AccessGrade fDal = new DAL.AccessGrade();
                return fDal.Del(_companyID, _accessID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.AccessGrade[Del] : " + ex);
                return false;
            }
        }
        #endregion
    }
}
