using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Board.BE;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class LinkURL : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL 
    {
        #region ADD()

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-03-20
        /// 작성목적 : 링크URL 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_listID"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _categoryID, int _listID, List<BE.LinkURLInfo> _info)
        {

            DAL.LinkURL dal = new DAL.LinkURL();
            for (int i = 0; i < _info.Count; i++)
            {
                LinkURLInfo info = new LinkURLInfo();

                info.CompanyID = _companyID;
                info.CategoryID = _categoryID;
                info.ListID = _listID;
                info.URLName = _info[i].URLName;
                info.URL = _info[i].URL;

                if (!dal.Add(info))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion 

        #region DEL()
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-03-20
        /// 작성목적 : 링크URL 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, int _linkURLID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.LinkURL dalLinkURL = new DAL.LinkURL();
                    dalLinkURL.Del(_companyID, _categoryID, _listID, _linkURLID);

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;
            }
        }
        #endregion
    }
}
