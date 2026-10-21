using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Address.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// ListBlogImpo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListBlogImpo : Base.BaseWSAddress
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 중요 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetImpoListCount(string p)
        {
            SetDecryptString(p);

            string searchKey = GetDecryptParam("searchKey", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string searchFilter = GetDecryptParam("searchFilter", "");

            Address.DAL.ImpoUser dal = new DAL.ImpoUser();
            return dal.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchKey, searchValue, BLL.List.FilterString(searchFilter));
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 중요 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetImpoPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string orders = GetDecryptParam("order", "");
                string searchFilter = GetDecryptParam("searchFilter", "");

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                List<object> listobj = new List<object>();
                DAL.ImpoUser dal = new DAL.ImpoUser();
                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    setPageingStr[0], setPageingStr[1], searchKey, searchValue, BLL.List.FilterString(searchFilter), orders);

                BLL.List biz = new BLL.List();
                return biz.GetListData(PreLang, dtr);                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetImpoPageList", ex);
                throw;
            }
        }


        /// <summary>
        /// 용도 : 카테고리별 사용자정보 설정하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCategoryUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("categoryID", "0");
                string types = GetDecryptParam("types", "");
                string listType = GetDecryptParam("listType", "");
                string pageSize = GetDecryptParam("pageSize", "0");

                BE.CategoryUserInfo info = new BE.CategoryUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID.ToNumber<int>();
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.ListType = listType;
                info.Types = types;

                DAL.CategoryUser dal = new DAL.CategoryUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModCategoryUser : " + ex.Message);
                throw;
            }

        }


        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 중요표시 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string ownerID = GetDecryptParam("ownerID", "");

                if (ownerID.Length > 0)
                {
                    DAL.ImpoUser dal = new DAL.ImpoUser();

                    foreach (string id in ownerID.Split(','))
                    {
                        BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.OwnerID = id.Split('|')[0].ToNumber<int>();
                        info.UserID = CurrentUserInfo.UserID;
                        info.Types = id.Split('|')[1];

                        dal.Del(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelImpUser : " + ex.Message);
                throw;
            }

            return true;
        }
    }
}
