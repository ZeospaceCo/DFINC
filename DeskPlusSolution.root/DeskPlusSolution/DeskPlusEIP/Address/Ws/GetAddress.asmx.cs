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


namespace Zeospace.DeskPlus.Address.Web.Ws
{
    /// <summary>
    /// GetAddress의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetAddress : Base.BaseWSAddress
    {

        #region 함수

        

        #endregion


        #region 가져오기


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
                string searchOption = GetDecryptParam("searchOption", "List");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string orders = GetDecryptParam("order", "");
                string searchFilter = GetDecryptParam("searchFilter", "");

                List<object> listobj = new List<object>();

                switch (types)
                {
                    case "GG":
                        listobj = GetCompany(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "CG":
                        listobj = GetCommon(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "PG":
                        listobj = GetPrivate(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchOption, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "RG":
                        listobj = GetTransfer(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "TG":
                        listobj = GetTitle(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders);
                        break;
                    case "DG":
                        listobj = GetDuty(categoryID.ToNumber<int>(), totalCount.ToNumber<int>(), pageSize, pageSizePageNo, searchKey, searchValue, searchFilter, orders);
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
        /// 회사별 주소록 가져오기
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetCompany(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string deptList = categoryID.ToString();

            if (categoryID == 0)
            {
                Account.BLL.Dept deptBiz = new Account.BLL.Dept();
                deptList = deptBiz.GetChildDeptList(CurrentUserInfo.CompanyID, categoryID, true);
                deptList = (deptList.Length == 0) ? categoryID.ToString() : deptList;
            }
            
            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(CurrentUserInfo.CompanyID,
                deptList, 0, "GM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 공용주소록 가져오기
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetCommon(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string comList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.CommonCategory comCat = new BLL.CommonCategory();
                comList = comCat.GetChildList(CurrentUserInfo.CompanyID, categoryID);
                comList = (comList.Length == 0) ? categoryID.ToString() : comList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(CurrentUserInfo.CompanyID,
                comList, 0, "CM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 가져오기
        /// 주의 : 개인주소록, 전달주소록은 나의것만 봐야 한다.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetPrivate(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchOption, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string priList = categoryID.ToString();

            if (categoryID == 0)
            {
                BLL.PrivateCategory priCat = new BLL.PrivateCategory();
                priList = priCat.GetChildList(CurrentUserInfo.CompanyID, categoryID, CurrentUserInfo.UserID);
                priList = (priList.Length == 0) ? categoryID.ToString() : priList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageListWithSub(CurrentUserInfo.CompanyID,
                priList, CurrentUserInfo.UserID, "PM", setPageingStr[0], setPageingStr[1], _searchOption, _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 전달받은주소록
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetTransfer(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(CurrentUserInfo.CompanyID,
                "0", CurrentUserInfo.UserID, "RM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 직급 리스트
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetTitle(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string titleList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootTitleID = codeDal.GetRootCode(CurrentUserInfo.CompanyID, "T", true);

            if (categoryID == rootTitleID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                titleList = codeBiz.GetChildList(CurrentUserInfo.CompanyID, categoryID, true);
                titleList = (titleList.Length == 0) ? categoryID.ToString() : titleList;
            }            

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(CurrentUserInfo.CompanyID,
                titleList, 0, "TM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 직책 리스트
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageSizePageNo">The page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_idx">The _idx.</param>
        /// <param name="_orders">The _orders.</param>
        /// <returns></returns>
        public List<object> GetDuty(int categoryID, int totalCount, string pageSize,
            string pageSizePageNo, string _searchKey, string _searchValue, string _searchFilter, string _orders)
        {
            string dutyList = categoryID.ToString();
            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            int rootDutyID = codeDal.GetRootCode(CurrentUserInfo.CompanyID, "D", true);

            if (categoryID == rootDutyID)
            {
                Zeospace.DeskPlus.Common.BLL.Code codeBiz = new DeskPlus.Common.BLL.Code();
                dutyList = codeBiz.GetChildList(CurrentUserInfo.CompanyID, categoryID, true);
                dutyList = (dutyList.Length == 0) ? categoryID.ToString() : dutyList;
            }

            Address.DAL.Private pBiz = new DAL.Private();
            //int totalCount = pBiz.GetPageCount(CurrentUserInfo.CompanyID, deptList, 0, "GM", _searchKey, _searchValue);

            string rtnSetPaging = Utility.SetPaging(totalCount, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DataTableReader dtr = pBiz.GetPageList(CurrentUserInfo.CompanyID,
                dutyList, 0, "DM", setPageingStr[0], setPageingStr[1], _searchKey, _searchValue, BLL.List.FilterString(_searchFilter), _orders);

            return GetListData(dtr);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 주소록 객체에 담아서 반환하기
        /// </summary>
        /// <param name="dtr">The DTR.</param>
        /// <returns></returns>
        private List<object> GetListData(DataTableReader dtr)
        {
            List<object> listobj = new List<object>();
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Address.BE.PageListInfo info = new BE.PageListInfo();
                        info.ROW = dtr["ROW"].ToString().ToNumber<int>();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.KeyID = dtr["KeyID"].ToString().ToNumber<int>();
                        info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>(); //중요표시여부

                        string name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                        info.Name = name;
                        //info.OwnerID = dtr["OwnerID"].ToString();
                        //info.ParentOwnerID = dtr["ParentOwnerID"].ToString();
                        info.Type = dtr["Type"].ToString();

                        string titleName = this.GetLangName(
                            dtr["TitleBase"].ToString(), 
                            dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                            dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);
                        info.TitleName = titleName;
                        
                        //info.DutyID = dtr["DutyID"].ToString();
                        info.Mail = dtr["Mail"].ToString();
                        info.MobilePhone = dtr["MobilePhone"].ToString();

                        #region 다국어 회사 설정
                        string comName = this.GetLangName(
                            dtr["CompanyBase"].ToString(),
                            dtr["CompanyKo"].ToString(), dtr["CompanyEn"].ToString(), dtr["CompanyJa"].ToString(),
                            dtr["CompanyFr"].ToString(), dtr["CompanyZhCn"].ToString(), dtr["CompanyZhTw"].ToString(), PreLang);
                        #endregion 

                        info.Company = comName;
                        info.OfficePhone = dtr["OfficePhone"].ToString();
                        info.BaseDept = dtr["BaseDept"].ToString();

                        #region 다국어 회사 설정
                        string deptName = this.GetLangName(
                            dtr["DeptBase"].ToString(),
                            dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                            dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);
                        #endregion 
                        info.DeptName = deptName;

                        #region 사진 이미지 설정
                        string photoName = dtr["PhotoName"].ToString();
                        info.PhotoName = "";
                        if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                        {
                            string imgPath = "";
                            string imgTag = "";
                            switch (info.Type)
                            {
                                case "GM":
                                    imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/photo.jpg";
                                    imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.UserID);                                    

                                    if(System.IO.File.Exists(Server.MapPath(imgPath)))
                                    {
                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }
                                    
                                    break;
                                default:
                                    imgPath = baseFileDirectory + "{0}/Address/{1}";
                                    string[] fileStr = photoName.Split('.');
                                    imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, fileStr[0] + "_S." + fileStr[1]);                                    

                                    if(System.IO.File.Exists(Server.MapPath(imgPath)))
                                    {
                                        imgTag = "<img src='{0}' width='52' height='52' />";
                                        imgTag = string.Format(imgTag, imgPath);
                                        info.PhotoName = imgTag;
                                    }
                                    break;
                            }
                        }

                        if (info.PhotoName.Length == 0)
                            info.PhotoName = "<img src='" + this.BaseImgPath + "/Icon/ico_noimg.gif' width='52' height='52' />";
                        #endregion

                        switch (info.Type)
                        {
                            case "GM" :
                                info.TypeName = "<div class='r_gubun_gm'></div>";
                                break;
                            case "CM" :
                                info.TypeName = "<div class='r_gubun_cm'></div>";
                                break;
                            case "PM" :
                                info.TypeName = "<div class='r_gubun_pm'></div>";
                                break;
                            case "RM" :
                                info.TypeName = "<div class='r_gubun_rm'></div>";
                                break;
                            case "TM" :
                                info.TypeName = "<div class='r_gubun_tm'></div>";
                                break;
                            case "DM" :
                                info.TypeName = "<div class='r_gubun_dm'></div>";
                                break;
                            case "FM" :
                                info.TypeName = "<div class='gubun_fm'></div>";
                                break;
                            default:
                                info.TypeName = "";
                                break;
                        }

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }


        #region CSV 가져오기

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCSVPageList(string p)
        {
            try
            {
                SetDecryptString(p);                
                string types = GetDecryptParam("types", "");

                List<object> listobj = new List<object>();
                DAL.CSV dal = new DAL.CSV();
                DataTableReader dtr = null;

                switch (types)
                {
                    case "ALL":
                        dtr = dal.GetCSV(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;
                    case "EXCEPT":
                        dtr = dal.GetCSVExcept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;
                    case "OVERLAP":
                        dtr = dal.GetCSVOverlap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;                    
                }

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.CSVInfo info = new BE.CSVInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CSVID = dtr["CSVID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.Name = dtr["Name"].ToString();
                            info.Email = dtr["Email"].ToString();
                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.HomePhone = dtr["HomePhone"].ToString();
                            info.CompanyPhone = dtr["CompanyPhone"].ToString();
                            info.Company = dtr["Company"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList", ex);
                throw;
            }
        }

        #endregion

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
        /// 작성자 : shine
        /// 개인주소록의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPrivateCategoryCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.PrivateCategory category = new DAL.PrivateCategory();
            return category.GetPrivateCategoryPCount(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 작성자 : shine
        /// 즐겨찾기의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetFavorCategoryCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.FavorCategory category = new DAL.FavorCategory();
            return category.GetFavoCategoryPCount(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCommonCategoryPCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.CommonCategory category = new DAL.CommonCategory();
            return category.GetCommonCategoryPCount(CurrentUserInfo.CompanyID, CategoryID);
        }


        #region 사용자 카테고리 정보

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 카테고리 사용자별 정보 가져오기(ListType:PageSize)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCategoryUser(string p)
        {
            SetDecryptString(p);

            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<Int32>();
            string types = GetDecryptParam("types", "");

            BE.CategoryUserInfo info = new CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.CategoryID = categoryID;
            info.Types = types;

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                return userInfo.ListType + ":" + userInfo.PageSize;
            }

            return "1:10";
        }

        #endregion


        #region 중요 리스트

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

                return GetListData(dtr);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetImpoPageList", ex);
                throw;
            }
        }

        #endregion


        #endregion
    }
}
