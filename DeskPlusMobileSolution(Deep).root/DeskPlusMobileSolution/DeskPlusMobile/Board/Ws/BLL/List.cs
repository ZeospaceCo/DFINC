using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using Zeospace.DeskPlusMobile.Web.Board.Ws.Data;
using System.Data;
using System.Xml;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BLL
{
    public class List
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// 특정 언어에 값이 없으면 base 값이 기본값으로 반환
        /// </summary>
        /// <param name="_base">The _base.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public string GetLangName(string _base, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            string lang = "";

            _base = _base.ToNull();
            _ko = _ko.ToNull();
            _en = _en.ToNull();
            _ja = _ja.ToNull();
            _fr = _fr.ToNull();
            _zhcn = _zhcn.ToNull();
            _zhtw = _zhtw.ToNull();

            switch (_preLang.ToLower())
            {
                case "ko":
                    lang = _ko;
                    break;
                case "en":
                    lang = _en;
                    break;
                case "ja":
                    lang = _ja;
                    break;
                case "fr":
                    lang = _fr;
                    break;
                case "zh-cn":
                    lang = _zhcn;
                    break;
                case "zh-tw":
                    lang = _zhtw;
                    break;
            }

            //특정 언어에 값이 없으면 base 값이 기본값으로 반환
            return (lang.Length > 0) ? lang : _base;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 카운트 가져오기
        /// 작성일자 : 2011-07-14
        /// </summary>
        /// <param name="_userComID"></param>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public string GetNoReadList(int _userComID, int _companyID, int _userID, int _deptID, int _categoryID, string _preLang)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo be = bll.GetCategoryOptionEtc(_companyID, _categoryID, _userID, _deptID);
                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                string name = GetLangName(be.NameBase, be.NameKo, be.NameEn, be.NameJa, be.NameFr, be.NameZhcn, be.NameZhtw, _preLang);

                if (be.Auth == "")
                {
                    return name + " <font class='lif'>(0/0)</font>";
                }

                string targetCategory = _categoryID.ToString();
                if (!string.IsNullOrEmpty(be.OptionUseView))
                {
                    if (!be.OptionUseView.Equals("0"))
                    {
                        string parentCategorys = bll.GetChildCategoryAuth(_companyID, _userID, _deptID, _categoryID, "0");

                        if (parentCategorys.Length > 0)
                        {
                            DataSet ds = data.GetUseMobile(_companyID, parentCategorys);

                            if (ds != null)
                            {
                                string tmpCat = "";
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    tmpCat += dr["CategoryID"].ToString() + ",";
                                }

                                targetCategory = tmpCat.TrimEnd(',');
                            }
                        }
                        else
                        {
                            targetCategory = parentCategorys;
                        }
                    }
                }

                //볼 수 있는 권한의 카테고리이지만, 모바일 미사용을 하였음
                targetCategory = (targetCategory.Length > 0) ? targetCategory : "0";

                int cnt = data.GetNoReadListCnt(_userComID, targetCategory, _userID);

                string tmpCnt = (cnt > 0) ? cnt.ToString("#,###") : "0";

                // 총게시물 갯수
                int totalCnt = data.GetTotalCnt(_companyID, targetCategory);
                string tmptotalCnt = (totalCnt > 0) ? totalCnt.ToString("#,###") : "0";

                return name + " <font class='lif'>(" + tmpCnt + "/" + tmptotalCnt + ")</font>";

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetNoReadList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 카운트 가져오기
        /// 작성일자 : 2015-06-23
        /// </summary>
        /// <param name="_userComID"></param>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public int GetNoReadMenu(int _userComID, int _companyID, int _userID, int _deptID, int _categoryID, string _preLang)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo be = bll.GetCategoryOptionEtc(_companyID, _categoryID, _userID, _deptID);
                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                if (be.Auth == "")
                {
                    return 0;
                }

                string targetCategory = _categoryID.ToString();
                if (!string.IsNullOrEmpty(be.OptionUseView))
                {

                    string parentCategorys = bll.GetChildCategoryAuth(_companyID, _userID, _deptID, _categoryID, "0");

                    if (parentCategorys.Length > 0)
                    {
                        DataSet ds = data.GetUseMobile(_companyID, parentCategorys);

                        if (ds != null)
                        {
                            string tmpCat = "";
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                tmpCat += dr["CategoryID"].ToString() + ",";
                            }

                            targetCategory = tmpCat.TrimEnd(',');
                        }
                    }
                    else
                    {
                        targetCategory = parentCategorys;
                    }

                }

                //볼 수 있는 권한의 카테고리이지만, 모바일 미사용을 하였음
                targetCategory = (targetCategory.Length > 0) ? targetCategory : "0";
                int cnt = data.GetNoReadListCnt(_userComID, targetCategory, _userID);

                return cnt;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetNoReadMenu", ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_listInfo"></param>
        /// <returns></returns>
        public bool ModByReadCount(ListInfo _listInfo)
        {
            try
            {
                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List list
                    = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();
                return list.ModByReadCount(_listInfo);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModByReadCount", ex);
                return false;
            }
        }
    }
}