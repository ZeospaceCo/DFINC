using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class Authority
    {
        public Authority()
        {
        }

        #region GET()

        /// <summary>
        /// 작성목적   : 본인 권한이 할당된 카테고리값 가져오기
        /// 반환값 : (카테고리 , 권한값)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptList">The _dept I ds.</param>
        /// <param name="_childCategoryList">The _get sub category ID.</param>
        /// <param name="_auth">The _auth.</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategorys(int _companyID, int _userID, string _deptList,
            string _childCategoryList, BE.BoardAuth _auth)
        {
            if (_childCategoryList == "") return null;

            try
            {
                NameValueCollection myAuthCatList = new NameValueCollection(); //순위별로 내려가면서 나의 권한을 수집한다.
                ArrayList existCatIDArr = new ArrayList();//순위별로 카테고리 접근 권한을 찾으면 그 카테고리를 제외하여 다음 순위로 보낸다.
                string existCatID = "";//existCatIDArr 의 콤마 문자열
                string authString = AppSettingHelper.GetDescription(_auth); //게시판권한 문자열 변환

                //하위 카테고리 값 가져오기
                foreach (string cat in _childCategoryList.Split(',')) existCatIDArr.Add(cat);

                #region 1순위. 사용자 권한 M USERID

                //################ 1순위. 사용자 권한 가져오기 ################
                DAL.Authority authBiz = new DAL.Authority();
                NameValueCollection myAuthByUser = authBiz.GetMyAuthCategory(_companyID, _childCategoryList, _userID.ToString(), "1");

                foreach (string cat in myAuthByUser.AllKeys) existCatIDArr.Remove(cat);//사용자에 나오는 권한 삭제하기

                foreach (string cat in myAuthByUser)
                {
                    if (authString.IndexOf(myAuthByUser[cat]) != -1) //요청한 권한값과 맞을 경우
                    {
                        //cagegoryid, auth 값 담기
                        myAuthCatList.Add(cat.ToString(), myAuthByUser[cat]);
                    }
                }

                #endregion 1순위. 사용자 권한 M USERID

                existCatID = "";

                foreach (string cat in existCatIDArr) existCatID += cat + ",";

                if (existCatID.Length > 0)
                {
                    #region 2순위. 부서 권한 G DEPTID

                    NameValueCollection myAuthByDept = authBiz.GetMyAuthCategory(_companyID, existCatID.TrimEnd(','), _deptList, "2");

                    foreach (string cat in myAuthByDept) existCatIDArr.Remove(cat);
                    foreach (string cat in myAuthByDept)
                    {
                        if (authString.IndexOf(myAuthByDept[cat]) != -1) //요청한 권한값과 맞을 경우
                        {
                            myAuthCatList.Add(cat.ToString(), myAuthByDept[cat]);
                        }
                    }

                    existCatID = "";

                    foreach (string cat in existCatIDArr) existCatID += cat + ",";

                    #endregion 2순위. 부서 권한 G DEPTID

                    #region 3순위. 전체 권한 G 0

                    if (existCatID.Length > 0)
                    {
                        //################ 3. 회사 ################

                        NameValueCollection myAuthWhole = authBiz.GetMyAuthCategory(_companyID, existCatID.TrimEnd(','), "0", "3");
                        foreach (string cat in myAuthWhole)
                        {
                            if (authString.IndexOf(myAuthWhole[cat]) != -1) //요청한 권한값과 맞을 경우
                            {
                                myAuthCatList.Add(cat.ToString(), myAuthWhole[cat]);
                            }
                        }
                    }

                    #endregion 3순위. 전체 권한 G 0
                }

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyAuthCategorys : " + ex);
                throw;
            }
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 추가
        /// </summary>
        /// <param name="_categoryInfo">The _category info.</param>
        /// <param name="_al">The _al.</param>
        /// <returns></returns>
        public int AddAuthCategory(BE.CategoryInfo _categoryInfo, List<BE.AuthorityInfo> _al)
        {
            try
            {
                int categoryID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Category dalCategory = new DAL.Category();
                    categoryID = dalCategory.Add(_categoryInfo);

                    if (categoryID > 0)
                    {
                        if (_al.Count > 0)
                        {
                            DAL.Authority dalAuth = new DAL.Authority();

                            foreach (BE.AuthorityInfo authInfo in _al)
                            {
                                authInfo.CategoryID = categoryID;
                                dalAuth.Add(authInfo);
                            }

                            //2015.12.16 부서를 사용자로 저장함.
                            dalAuth.BoardUser(_categoryInfo.CompanyID, categoryID);
                        }
                    }

                    ts.Complete();
                }

                return categoryID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddAuthCategory", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 수정 2015.12.16
        /// </summary>
        /// <param name="_al">The _al.</param>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_subApply">if set to <c>true</c> [_sub apply].</param>
        /// <returns></returns>
        public bool Mod(List<BE.AuthorityInfo> _al, int _companyID, int _categoryID, bool _subApply)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //권한이 없으면, 전부 제거
                    if (_al.Count == 0)
                    {
                        DAL.Authority dalAuth = new DAL.Authority();
                        dalAuth.Del(_companyID, _categoryID); //해당 카테고리 제거

                        //하위폴더까지 깔끔히 제거???
                        if (_subApply)
                        {
                            //하위를 훌터라.
                            BLL.Category bizCat = new Category();
                            string getSubCategory = bizCat.GetChildCategory(_companyID, _categoryID);

                            if (getSubCategory.Trim().Length > 0)
                            {
                                //하위 폴더 값 , 값으로 돌리자..
                                string[] getSubCategoryStr = getSubCategory.Split(',');

                                for (int i = 0; i < getSubCategoryStr.Length; i++)
                                {
                                    if (getSubCategoryStr[i].Length > 0)
                                    {
                                        //하위폴더의 권한값을 지우는 구문..
                                        dalAuth.Del(_companyID, getSubCategoryStr[i].ToNumber<int>());
                                        //2015.12.16 부서를 사용자로 저장함. (부서값이 있으면 거기도 삭제...)
                                        dalAuth.BoardUser(_companyID, getSubCategoryStr[i].ToNumber<int>());
                                    }
                                }
                            }
                        }
                    }
                    else if (_al.Count > 0 && _al != null) //권한값이 있네??? 새건지.. 기존껀지 ㅡㅡ;
                    {
                        //우선 해당 카테고리를 다 지우자..ㅡㅡ;
                        DAL.Authority dalAuth = new DAL.Authority();
                        dalAuth.Del(_companyID, _categoryID);

                        foreach (BE.AuthorityInfo authInfo in _al)
                        {
                            //신규인지,,, 헌건지..모르지만..새껄 넣는다.
                            dalAuth.Add(authInfo);
                        }

                        //2015.12.16 부서를 사용자로 저장함.
                        dalAuth.BoardUser(_companyID, _categoryID);

                        //하위 폴더까지 그 짓을 할껴??
                        if (_subApply)
                        {
                            //오냐.~~ 해당 카테고리 하위 폴더 정보 가져오구래이ㅣ..
                            BLL.Category bizCat = new Category();
                            string getSubCategory = bizCat.GetChildCategory(_companyID, _categoryID);

                            if (getSubCategory.Trim().Length > 0)
                            {
                                string[] getSubCategoryStr = getSubCategory.Split(',');

                                for (int i = 0; i < getSubCategoryStr.Length; i++)
                                {
                                    if (getSubCategoryStr[i].Length > 0)
                                    {
                                        //하위 폴더의 권한을 우선 날려~ 버려..
                                        dalAuth.Del(_companyID, getSubCategoryStr[i].ToNumber<int>());

                                        foreach (BE.AuthorityInfo authInfo in _al)
                                        {
                                            authInfo.CategoryID = getSubCategoryStr[i].ToNumber<int>();
                                            //그리고 쌔거 넣자궁..
                                            dalAuth.Add(authInfo);
                                        }

                                        //2015.12.16 부서를 사용자로 저장함.
                                        dalAuth.BoardUser(_companyID, getSubCategoryStr[i].ToNumber<int>());
                                    }
                                }
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddAuthCategory", ex);
                return false;
            }
        }

        #endregion MOD()
    }
}