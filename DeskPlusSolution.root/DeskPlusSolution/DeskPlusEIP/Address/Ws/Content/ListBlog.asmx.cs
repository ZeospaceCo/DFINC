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
namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// ListBlog의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListBlog : Base.BaseWSAddress
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 주소록 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "");
                string types = GetDecryptParam("types", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "");
                string searchOption = GetDecryptParam("searchOption", "");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string orders = GetDecryptParam("order", "");
                string searchFilter = GetDecryptParam("searchFilter", "");

                List<object> listobj = new List<object>();

                BLL.List biz = new BLL.List();

                switch (types)
                {
                    case "GG":
                        listobj = biz.GetCompany(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders, CurrentUserInfo.UserID);
                        break;
                    case "CG":
                        listobj = biz.GetCommon(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders, CurrentUserInfo.UserID);
                        break;
                    case "PG":
                        listobj = biz.GetPrivate(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "RG":
                        listobj = biz.GetTransfer(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "TG":
                        listobj = biz.GetTitle(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders, CurrentUserInfo.UserID);
                        break;
                    case "DG":
                        listobj = biz.GetDuty(CurrentUserInfo.CompanyID, PreLang, categoryID.ToNumber<int>(), totalCount.ToNumber<int>(),
                            pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders, CurrentUserInfo.UserID);
                        break;
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "");
            string types = GetDecryptParam("types", "");
            string searchOption = GetDecryptParam("searchOption", "");
            string searchKey = GetDecryptParam("searchKey", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string searchFilter = GetDecryptParam("searchFilter", "");

            string catList = categoryID.ToString();
            string typeMember = "";
            int userID = 0;


            switch (types)
            {
                case "GG":
                    typeMember = "GM";
                    break;
                case "CG":
                    typeMember = "CM";
                    break;
                case "PG":
                    typeMember = "PM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "RG":
                    typeMember = "RM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "TG":
                    typeMember = "TM";
                    break;
                case "DG":
                    typeMember = "DM";
                    break;
                case "FG":
                    typeMember = "FM"; //가짜 (데이터 안나올거임)
                    userID = CurrentUserInfo.UserID;
                    break;
            }

            BLL.List biz = new BLL.List();
            string catSubList = biz.GetSubCatList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, categoryID, types);

            Address.DAL.Private pBiz = new DAL.Private();
            return pBiz.GetPageCountWithSub(CurrentUserInfo.CompanyID, catSubList, userID, typeMember, searchOption, searchKey, searchValue, BLL.List.FilterString(searchFilter));
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
        /// 용도 : 중요표시 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddImpUser(string p)
        {
            try
            {
                SetDecryptString(p);
                string ownerID = GetDecryptParam("ownerID", "0").TrimEnd(',');//키|구분자,키|구분자
                DAL.ImpoUser dal = new DAL.ImpoUser();

                foreach (string id in ownerID.Split(','))
                {
                    BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.OwnerID = id.Split('|')[0].ToNumber<int>();
                    info.UserID = CurrentUserInfo.UserID;
                    info.Types = id.Split('|')[1];

                    if (info.Types.ToNull().Trim().Length > 0)
                    {
                        dal.Add(info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddImpUser : " + ex.Message);
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

        /// <summary>
        /// 개인주소록 - 간편주소록 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");
                string name = GetDecryptParam("name", "");
                string mail = GetDecryptParam("mail", "");
                string mobile = GetDecryptParam("mobile", "");
                mobile = mobile.Replace("plus", "+");

                //개인주소록만 입력가능하다.
                if (categoryID == 0 || types != "PG") return 0;

                BE.PrivateInfo info = new BE.PrivateInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.LoginID = CurrentUserInfo.LogInID;
                info.CategoryID = categoryID;
                info.PrivateID = 0;
                info.LastName = name;
                info.WholeName = name;
                info.NameBase = name; //기본언어
                info.Mail = mail;
                info.MobilePhone = mobile;
                info.IsReceive = "0";
                info.SendUserID = 0;

                DAL.Private dal = new DAL.Private();
                return dal.Add(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPrivate : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", "");

                if (listID.Length > 0)
                {
                    DAL.Private dal = new DAL.Private();
                    return dal.Del(CurrentUserInfo.CompanyID, categoryID, listID, CurrentUserInfo.UserID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPrivate : " + ex.Message);
                throw;
            }

        }
    }
}
