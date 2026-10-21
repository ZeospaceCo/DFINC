using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BLL
{
    public class Category
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetCategory(int _companyID)
        {
            Data.Category data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Category();
            return data.GetCategory(_companyID);
        }

        /// <summary>
        /// Gets the child category.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_anonymity">The _anonymity.</param>
        /// <returns></returns>
        public string GetChildCategory(int _companyID, int _categoryID, string _anonymity)
        {
            string categoryIDList = "";
            DataSet ds = GetCategory(_companyID);
            ds.Relations.Add("CategoryRelation", ds.Tables[0].Columns["CategoryID"], ds.Tables[0].Columns["ParentCategoryID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow["CategoryID"].ToString() == _categoryID.ToString())
                {
                    //익명게시판일 경우 해당 카테고리 제외
                    if (dbRow["OptionAnonymity"].ToString().Equals("0"))
                    {
                        categoryIDList += dbRow["CategoryID"].ToString();
                    }

                    //하위트리 만들기(datarow, treeviw)
                    GetSubChildCategory(dbRow, ref _anonymity, ref categoryIDList);
                }
            }

            return categoryIDList.TrimEnd(',');
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 익명게시판 제외한 하위 게시판 카테고리 가져오기-2
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="_anonymity">The _anonymity.</param>
        /// <param name="categoryIDList">The category ID list.</param>
        private void GetSubChildCategory(DataRow _dbRow, ref string _anonymity, ref string categoryIDList)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CategoryRelation"))
            {
                if (childRow["OptionAnonymity"].ToString().Equals("0"))
                {
                    if (categoryIDList.Length > 0)
                    {
                        categoryIDList += "," + childRow["CategoryID"].ToString();
                    }
                    else
                    {
                        categoryIDList += childRow["CategoryID"].ToString();
                    }
                }

                //재귀함수 호출~
                GetSubChildCategory(childRow, ref _anonymity, ref categoryIDList);
            }
        }

        /// <summary>
        /// Gets the child category.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public string GetChildCategory(int _companyID, int _categoryID)
        {
            string childCategory = "";
            Data.Category biz = new Data.Category();
            DataSet ds = biz.GetCategory(_companyID, _categoryID);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows) childCategory = dbRow["ChildCategory"].ToString();
            }

            return childCategory.TrimEnd(',');
        }

        /// <summary>
        /// 하위 게시판 카테고리 가져오기-2
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="categoryIDList">The category ID list.</param>
        private void GetSubChildCategory(DataRow _dbRow, ref string categoryIDList)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CategoryRelation"))
            {
                if (categoryIDList.Length > 0)
                {
                    categoryIDList += "," + childRow["CategoryID"].ToString();
                }
                else
                {
                    categoryIDList += childRow["CategoryID"].ToString();
                }

                //재귀함수 호출~
                GetSubChildCategory(childRow, ref categoryIDList);
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 익명여부의 나의 권한에 따른 하위게시판 카테고리 가져오기 (_anonymity =&gt; 0:일반,1:익명)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_anonymity">The _anonymity.</param>
        /// <returns></returns>
        public string GetChildCategoryAuth(int _companyID, int _userID, int _deptID, int _categoryID, string _anonymity)
        {
            string getChildCategoryAuth = _categoryID.ToString();

            if (_categoryID != 0)
            {
                BLL.Dept dept = new Dept();
                string getParentDeptID = dept.GetParentDeptID(_companyID, _deptID);
                if (getParentDeptID.Length > 0)
                {
                    getParentDeptID += "," + _deptID;
                }
                else
                {
                    getParentDeptID = _deptID.ToString();
                }

                //하위카테고리값 알아내기            
                BLL.Authority auth = new Authority();
                BLL.Category category = new Category();

                //익명일 경우
                if (_anonymity.Equals("1"))
                {
                    //익명을 제외한 본인 권한이 할당된 카테고리값 가져오기
                    getChildCategoryAuth = auth.GetMyAuthCategoryID(_companyID, _userID, getParentDeptID, category.GetChildCategory(_companyID, _categoryID, _anonymity));
                }
                else
                {
                    //본인 권한이 할당된 카테고리값 가져오기
                    getChildCategoryAuth = auth.GetMyAuthCategoryID(_companyID, _userID, getParentDeptID, category.GetChildCategory(_companyID, _categoryID));
                }
            }

            return getChildCategoryAuth;
        }

        /// <summary>
        /// Gets the category option etc.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept identifier.</param>
        /// <returns></returns>
        public BE.CategoryInfo GetCategoryOptionEtc(int _companyID, int _categoryID, int _userID, int _deptID)
        {
            DataSet ds = GetCategory(_companyID, _categoryID);
            BE.CategoryInfo be = new Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo();

            //0. optionShowImage, 1. optionUseReply, 2. optionUseShortReply, 3 : optionShowViewer, 4: optionUseDept
            //5. 권한 , 6. optionCustomView, 7. optionVote, 8. optionUseView, 9. optionAnonymity
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                be.OptionShowImage = dbRow["optionShowImage"].ToString();
                be.OptionUseReply = dbRow["optionUseReply"].ToString();
                be.OptionUseShortReply = dbRow["optionUseShortReply"].ToString();
                be.OptionShowViewer = dbRow["optionShowViewer"].ToString();
                be.OptionUseDept = dbRow["optionUseDept"].ToString();
                be.Auth = GetCategoryAuthority(_companyID, _categoryID, _userID, _deptID, "1", false);//읽기권한자 (1)
                be.OptionCustomView = dbRow["optionCustomView"].ToString();
                be.OptionVote = dbRow["optionVote"].ToString();
                be.OptionUseView = dbRow["optionUseView"].ToString();
                be.OptionAnonymity = dbRow["optionAnonymity"].ToString();
                be.NameBase = dbRow["NameBase"].ToString();
                be.NameKo = dbRow["NameKo"].ToString();
                be.NameEn = dbRow["NameEn"].ToString();
                be.NameJa = dbRow["NameJa"].ToString();
                be.NameFr = dbRow["NameFr"].ToString();
                be.NameZhcn = dbRow["NameZhcn"].ToString();
                be.NameZhtw = dbRow["NameZhtw"].ToString();
            }

            return be;
        }

        /// <summary>
        /// 해당 카테고리 권한이 있는지 체크 : 1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept identifier.</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <returns></returns>
        public string GetCategoryAuthority(int _companyID, int _categoryID, int _userID, int _deptID, string _readWrite, bool _admin)
        {
            string authValue = "";

            if (!_admin)
            {
                //해당 카테고리 권한 사용여부값 할당하기
                BLL.Authority auth = new Authority();
                string isAutority = auth.IsAutority(_companyID, _categoryID, _userID, _deptID, _readWrite); //권한이 할당된 카테고리값들
                if (isAutority.Length > 0)
                {
                    //0:true,false ; 1: 권한값
                    string[] authValueStr = isAutority.Split('/');
                    if (authValueStr[0].ToString().Equals("true"))
                    {
                        authValue = authValueStr[1].ToString();
                    }
                }
            }
            else //관리자일경우
            {
                //1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
                authValue = "4"; //관리자는 full 권한 준다.
            }

            return authValue;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public DataSet GetCategory(int _companyID, int _categoryID)
        {
            Data.Category data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Category();
            return data.GetCategory(_companyID, _categoryID);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 전사 게시판 카테고리 정보를 가져오자
        /// </summary>
        /// <param name="_categoryID">The _category identifier.</param>
        /// <returns></returns>
        public static int GetCompanyCategoryInfo(int _categoryID)
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
                Data.Category dal = new Data.Category();
                DataSet dsList = dal.GetCategoryCompany(1); //1 은 db connection 을 위한 companyid 임.. modify shimpark

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


        /// <summary>
        /// 작성자 : 심재운
        /// 본인이 볼수 있는 게시판 카테고리들 정보 가져오기
        /// 익명여부의 나의 권한에 따른 하위게시판 카테고리 가져오기 (_anonymity  0:일반,1:익명)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_useDept">_useDept</param>
        /// <param name="_useChild">if set to <c>true</c> [_use child].</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <param name="_admin">if set to <c>true</c> [_admin].</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategory(int _companyID, int _userID, int _deptID, int _categoryID,
            string _useDept, bool _useChild, string _gubun, bool _activeDept, bool _admin)
        {
            NameValueCollection getMyAuthCatList = new NameValueCollection();

            if (_categoryID > 0)
            {
                string getMyPDeptID = "";

                //나의 상위 부서들 정보 가져오기
                Account.Data.Dept dal = new Account.Data.Dept();
                DataSet myDeptDs = dal.GetDept(_deptID);

                if (myDeptDs != null)
                {
                    foreach (DataRow dr in myDeptDs.Tables[0].Rows)
                    {
                        getMyPDeptID = dr["PathID"].ToString();
                    }
                }

                if (getMyPDeptID.Length > 0) getMyPDeptID = getMyPDeptID.Substring(1);

                BLL.Authority auth = new Authority();
                BLL.Category category = new Category();

                //하위카테고리의 권한까지 가져옴(본인이 볼 수 있는...)
                if (_useChild)
                {
                    //부서별 게시판일 경우(권한 할당 하지 않음) , 관리자일 경우
                    if (_useDept.Equals("1") || _admin)
                    {
                        //권한 없이 하위 모든 카테고리 가져오기
                        string childCatID = category.GetChildCategory(_companyID, _categoryID);
                        foreach (string cat in childCatID.Split(','))
                        {
                            //권한 없이 그냥 보여줌
                            getMyAuthCatList.Add(cat, "4");
                        }
                    }
                    else
                    {
                        //본인 권한이 할당된 하위 카테고리값 가져오기
                        getMyAuthCatList = auth.GetMyAuthCategorys(_companyID, _userID, getMyPDeptID,
                            category.GetChildCategory(_companyID, _categoryID), _gubun);
                    }
                }
                else
                {
                    //부서별 게시판일 경우(권한 할당 하지 않음)
                    if (_useDept.Equals("1"))
                    {
                        //권한 없이 그냥 보여줌
                        getMyAuthCatList.Add(_categoryID.ToString(), "4");
                    }
                    else
                    {
                        //하나의 카테고리 권한 체크
                        getMyAuthCatList = auth.GetMyAuthCategorys(_companyID, _userID, getMyPDeptID,
                            _categoryID.ToString(), _gubun);
                    }
                }
            }

            return getMyAuthCatList;
        }

    }
}