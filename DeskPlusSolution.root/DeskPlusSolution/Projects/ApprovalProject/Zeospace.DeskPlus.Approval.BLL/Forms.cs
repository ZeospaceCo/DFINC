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
    public class Forms : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        /// <summary>
        /// 양식추가
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <param name="_listSeq"></param>
        /// <returns></returns>
        public int Add(BE.FormInfo _formInfo, List<BE.FormSeqInfo> _listSeq)
        {
            if (_formInfo == null || _listSeq == null) return -1;

            try
            {
                int formID = -1;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Form fdal = new Form();
                    formID = fdal.Add(_formInfo);
                    if (formID > 0)
                    {
                        DAL.FormSeq fsdal = new DAL.FormSeq();
                        foreach (BE.FormSeqInfo seqInfo in _listSeq)
                        {
                            seqInfo.FormID = formID;
                            fsdal.Add(seqInfo);
                        }
                    }
                    
                    ts.Complete();
                }

                return formID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[Add] : " + ex);
                return -1;
            }
        }
        #endregion

        #region MOD
        /// <summary>
        /// 양식수정(내용 및 기본정보)
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <param name="_listSeq"></param>
        /// <returns></returns>
        public bool Mod(BE.FormInfo _formInfo, List<BE.FormSeqInfo> _listSeq)
        {
            if (_formInfo == null || _listSeq == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Form fdal = new Form();
                    rtnval = fdal.Mod(_formInfo);
                    if (rtnval)
                    {
                        DAL.FormSeq fsdal = new DAL.FormSeq();
                        if (fsdal.Del(_formInfo.CompanyID, _formInfo.FormID))
                        {
                            foreach (BE.FormSeqInfo seqInfo in _listSeq)
                            {
                                seqInfo.FormID = _formInfo.FormID;
                                fsdal.Add(seqInfo);
                            }
                        }
                    }
                    
                    ts.Complete();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[Mod] : " + ex);
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
        public bool UpdActive(int _companyID, int _formID, string _isActive)
        {
            try
            {
                if (_companyID < 0 || _formID < 0 || _isActive.Equals("")) return false;

                DAL.Form f = new Form();
                return f.UpdActive(_companyID, _formID, _isActive);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[UpdActive] : " + ex);
                return false;
            }
        }

        /// <summary>
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

                DAL.Form fDal = new Form();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[ModSeq] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 양식수정(기본정보)
        /// </summary>
        /// <param name="_formInfo"></param>
        /// <param name="_listSeq"></param>
        /// <returns></returns>
        public bool ModEtc(BE.FormInfo _formInfo, string _listAppointerKind, string _listDeptID, string _listUserID, string _listIsDept, string _listIsActive)
        {
            if (_formInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Form fdal = new Form();
                    rtnval = fdal.ModEtc(_formInfo);

                    DAL.FormAppointer dal = new DAL.FormAppointer();
                    BE.FormAppointerInfo info = new BE.FormAppointerInfo();

                    info.CompanyID = _formInfo.CompanyID;
                    info.FormID = _formInfo.FormID;
                    info.WriteUserID = _formInfo.WriteUserID;
                    info.WriteUserName = _formInfo.WriteUserName;
                    info.WriteDate = _formInfo.WriteDate;

                    dal.Del(_formInfo.CompanyID, _formInfo.FormID);

                    if (!_listDeptID.Length.Equals(0))
                    {
                        string[] _AppointerKind = _listAppointerKind.Split(',');                        
                        string[] _DeptID = _listDeptID.Split(',');
                        string[] _UserID = _listUserID.Split(',');
                        string[] _IsDept = _listIsDept.Split(',');
                        string[] _IsActive = _listIsActive.Split(',');

                        for (int i = 0; i < _DeptID.Length; i++)
                        {
                            info.AppointerKind = _AppointerKind[i];
                            info.DeptID = _DeptID[i].ToNumber<int>();
                            info.UserID = _UserID[i].ToNumber<int>();                            
                            info.IsDept = _IsDept[i];
                            info.IsActive = _IsActive[i];

                            //수신참조정보저장
                            dal.Add(info);
                        }
                    }

                    ts.Complete();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[ModEtc] : " + ex);
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
        public bool Del(int _companyID, int _formID)
        {
            try
            {
                if (_companyID.Equals(0) || _formID.Equals(0)) return false;
                DAL.Form fDal = new Form();
                return fDal.Del(_companyID, _formID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Forms[Del] : " + ex);
                return false;
            }
        }
        #endregion
    }
}
