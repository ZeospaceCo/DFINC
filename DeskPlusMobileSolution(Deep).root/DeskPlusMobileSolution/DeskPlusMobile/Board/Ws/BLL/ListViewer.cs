using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BLL
{
    public class ListViewer
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시물 조회수 
        /// 작성일자 : 2011-07-13
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _categoryID, int _listID, int _userID)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer biz
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ListViewer();

                return biz.Add(_companyID, _categoryID, _listID, _userID);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Add", ex);
                return false;
            }
        }
    }
}