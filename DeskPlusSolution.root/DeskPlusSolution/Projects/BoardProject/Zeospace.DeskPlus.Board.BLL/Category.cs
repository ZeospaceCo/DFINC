using System;
using System.Collections.Specialized;
using System.Data;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class Category
    {
        #region GET()

        #region 상위카테고리 가져오기

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-25
        /// 작성목적 : 부모 카테고리 가져오기 (1, 2, 3, 4, 5...)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_catID">The _dept ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetParentCatList(int _companyID, int _catID, bool _active)
        {
            string parentCatList = "";
            DAL.Category biz = new DAL.Category();
            DataSet ds = biz.GetCategory(_companyID, _catID);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    parentCatList = dbRow["PathID"].ToString();
                    if (parentCatList.Length > 0) parentCatList = parentCatList.Substring(1);
                }
            }

            return parentCatList;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-25
        /// 작성목적 : 상위 카테고리 아이디값 가져오기 (재귀함수)
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="catIDList">The dept ID list.</param>
        private void GetParentCatID(DataRow _dbRow, ref string catIDList)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow parentRow in _dbRow.GetParentRows("CategoryRelation"))
            {
                catIDList += parentRow["CategoryID"].ToString() + ",";
                //재귀함수 호출~
                GetParentCatID(parentRow, ref catIDList);
            }
        }

        #endregion 상위카테고리 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 하위 카테고리 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public string GetChildCategory(int _companyID, int _categoryID)
        {
            try
            {
                string childCategory = "";
                DAL.Category biz = new DAL.Category();
                DataSet ds = biz.GetCategory(_companyID, _categoryID);

                if (ds != null)
                {
                    foreach (DataRow dbRow in ds.Tables[0].Rows) childCategory = dbRow["ChildCategory"].ToString();
                }

                return childCategory.TrimEnd(',');
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetChildCategory : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 본인이 볼수 있는 게시판 카테고리들 정보 가져오기 (겸직 포함 2015.12.16)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <param name="_userID">사용자 아이디</param>
        /// <param name="_deptID">부서 아이디</param>
        /// <param name="_categoryID">카테고리 아이디</param>
        /// <param name="_useDept">부서별게시판 여부(0:아니오;1:예)</param>
        /// <param name="_useChild">하위부서 포함여부 (true : 예, false : 아니오)</param>
        /// <param name="_auth">읽기(read), 쓰기(write)</param>
        /// <param name="_activeDept">부서 - 활성화 (true), 활성화 + 비활성화(false)</param>
        /// <param name="_admin">관리자 여부 (예:true, 아니오 : false)</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategory(int _companyID, int _userID, int _deptID, int _categoryID,
            string _useDept, bool _useChild, BE.BoardAuth _auth, bool _activeDept, bool _admin)
        {
            try
            {
                NameValueCollection getMyAuthCatList = new NameValueCollection();

                if (_categoryID > 0)
                {
                    #region 겸직부서 포함해서 게시판 권한을 가져오기

                    Account.DAL.User userDal = new Account.DAL.User();
                    Account.BLL.Dept dept = new Account.BLL.Dept();

                    //겸직부서 포함해서 게시판 권한을 할당하자 2015.12.16
                    var alList = userDal.GetUserInfo(_companyID, _userID, true, false);

                    string getMyUpDeptList = _deptID.ToString() + ",";
                    foreach (var info in alList)
                    {
                        //나의 상위 부서들 정보 가져오기 (겸직부서까지 포함)
                        //getMyUpDeptList += dept.GetUpDeptIDInfo(_companyID, info.DeptID, _activeDept) + ",";
                        string tmpPathID = dept.GetUpDeptIDInfo(_companyID, info.DeptID, _activeDept);
                        if (tmpPathID != "")
                        {
                            getMyUpDeptList += tmpPathID + ",";
                        }
                    }

                    if (getMyUpDeptList.Length > 0) getMyUpDeptList = getMyUpDeptList.TrimEnd(',');

                    //string getMyPDeptID = dept.GetUpDeptIDInfo(_companyID, _deptID, _activeDept);

                    #endregion 겸직부서 포함해서 게시판 권한을 가져오기

                    Board.BLL.Authority auth = new Authority();
                    Board.BLL.Category category = new Category();

                    //하위카테고리의 권한까지 가져옴(본인이 볼 수 있는...)
                    if (_useChild)
                    {
                        //부서별 게시판일 경우(권한 할당 하지 않음) , 관리자일 경우
                        if (_useDept.Equals("1") || _admin)
                        {
                            //권한 없이 하위 모든 카테고리 가져오기
                            string childCatList = category.GetChildCategory(_companyID, _categoryID);
                            foreach (string cat in childCatList.Split(','))
                            {
                                //권한 없이 그냥 보여줌
                                getMyAuthCatList.Add(cat, "4"); //쓰기 + 댓글쓰기
                            }
                        }
                        else
                        {
                            //본인 권한이 할당된 하위 카테고리값 가져오기
                            getMyAuthCatList = auth.GetMyAuthCategorys(_companyID, _userID, getMyUpDeptList,
                                category.GetChildCategory(_companyID, _categoryID), _auth);
                        }
                    }
                    else
                    {
                        //부서별 게시판일 경우(권한 할당 하지 않음)
                        if (_useDept.Equals("1"))
                        {
                            //권한 없이 그냥 보여줌
                            getMyAuthCatList.Add(_categoryID.ToString(), "4"); //쓰기 + 댓글쓰기
                        }
                        else
                        {
                            //하나의 카테고리 권한 체크
                            getMyAuthCatList = auth.GetMyAuthCategorys(_companyID, _userID, getMyUpDeptList,
                                _categoryID.ToString(), _auth);
                        }
                    }
                }

                return getMyAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyAuthCategory : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 경로 반환하기
        /// </summary>
        /// <param name="_ds">The _DS.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public string GetPathName(DataSet _ds, string _preLang)
        {
            string pathName = "";

            if (_ds != null)
            {
                foreach (DataRow dr in _ds.Tables[0].Rows)
                {
                    string name = UtilHelper.GetLangName(
                        dr["PathNameBase"].ToString(),
                        dr["PathNameKo"].ToString(),
                        dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                        dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                        dr["PathNameZhTw"].ToString(), _preLang);

                    switch (_preLang)
                    {
                        case "ko":
                            name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "en":
                            name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "ja":
                            name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "fr":
                            name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zh-cn":
                            name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zh-tw":
                            name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;
                    }

                    pathName = name.Substring(1, name.Length - 1);
                }
            }

            return pathName;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 투표 접근 가능 여부
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <returns></returns>
        public bool GetAccessRating(int _companyID, int _categoryID, int _deptID, int _userID, bool _admin)
        {
            try
            {
                bool response = true;
                DAL.Category dal = new DAL.Category();
                DataSet ds = dal.GetCategory(_companyID, _categoryID);

                string optionUseDept = ""; //부서별 게시판 여부
                string optionVote = "0"; //투표사용여부
                string optionVoteView = "0"; //사용자 투표현황 보여주기 여부

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        optionUseDept = dr["OptionUseDept"].ToString();
                        optionVote = dr["OptionVote"].ToString();
                        optionVoteView = dr["OptionVoteView"].ToString();
                    }
                }

                if (optionVote == "0") //투표용이 아닐경우
                {
                    response = false;
                }
                else if (optionVote == "1") //투표용일 경우
                {
                    if (_admin == true) //관리자는 권한없이 보여줌
                    {
                        response = true;
                    }
                    else if (_admin == false && optionVoteView == "1") //일반사용자에게 보이도록 할 경우, 권한체크 필요
                    {
                        BLL.Category bizCat = new BLL.Category();
                        NameValueCollection nvcAuth = new NameValueCollection();

                        //읽기 이상 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(_companyID, _userID,
                            _deptID, _categoryID, optionUseDept, false, BE.BoardAuth.Read, true, false);

                        if (nvcAuth.AllKeys.Length == 0) response = false;//############# 거부
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAccessRating : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 전사 게시판 카테고리 정보를 가져오자
        /// </summary>
        /// <param name="_categoryID">The _category identifier.</param>
        /// <returns></returns>
        public static int GetCompanyCategoryInfo(int _categoryID)
        {
            try
            {
                int companyID = 0;
                if (_categoryID == 0) return companyID;

                DataSet ds = null;
                string dqName = "BoardCategoryInfo";//applicaton name
                string qurey = string.Empty;

                if (HttpContext.Current.Application[dqName] == null)
                {
                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL biz =
                    //    new Base.BaseDAL();
                    DAL.Category dal = new DAL.Category();
                    DataSet dsList = dal.GetCategory();

                    if (dsList != null)
                    {
                        HttpContext.Current.Application.Lock();
                        HttpContext.Current.Application[dqName] = dsList;
                        HttpContext.Current.Application.UnLock();
                    }
                }

                //get hash info from application name
                ds = (DataSet)HttpContext.Current.Application[dqName];

                if (ds != null)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "CategoryID='" + _categoryID + "'";

                    foreach (DataRowView dr in dv) companyID = dr["CompanyID"].ToString().ToNumber<int>();
                }

                ds = null;

                return companyID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAutority : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 전사 게시판 카테고리 정보 초기화 하기
        /// </summary>
        public static void ClearCacheCategoryInfo()
        {
            string dqName = "BoardCategoryInfo";//applicaton name
            HttpContext context = HttpContext.Current;
            context.Application.Lock();
            context.Application.Remove(dqName);
            context.Application.UnLock();
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리 정보 수정하기
        /// </summary>
        /// <param name="_categoryInfo">The _category info.</param>
        /// <param name="_subApply">if set to <c>true</c> [_sub apply].</param>
        /// <returns></returns>
        public bool Mod(BE.CategoryInfo _categoryInfo, bool _subApply)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Category biz = new DAL.Category();
                    bool response = biz.Mod(_categoryInfo);

                    //하위 폴더 적용 선택시
                    if (response && _subApply)
                    {
                        string getSubCategory = GetChildCategory(_categoryInfo.CompanyID, _categoryInfo.CategoryID);

                        if (getSubCategory.Trim().Length > 0)
                        {
                            string[] getSubCategoryStr = getSubCategory.Split(',');

                            for (int i = 0; i < getSubCategoryStr.Length; i++)
                            {
                                if (getSubCategoryStr[i].Length > 0 && _categoryInfo.CategoryID != getSubCategoryStr[i].ToNumber<int>())
                                {
                                    _categoryInfo.CategoryID = getSubCategoryStr[i].ToNumber<int>();

                                    //제목값(영문값포함)이 없으면 피해서 UPDATE된다.
                                    _categoryInfo.NameBase = "";
                                    _categoryInfo.NameKo = "";
                                    _categoryInfo.NameEn = "";
                                    _categoryInfo.NameJa = "";
                                    _categoryInfo.NameFr = "";
                                    _categoryInfo.NameZhcn = "";
                                    _categoryInfo.NameZhtw = "";
                                    _categoryInfo.Explan = "";

                                    biz.Mod(_categoryInfo);
                                }
                            }
                        }
                    }

                    if (response)
                    {
                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddAuthCategory", ex);
                throw;
            }
        }

        #endregion MOD()
    }
}