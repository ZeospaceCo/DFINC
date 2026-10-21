using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BLL
{
    public class Authority
    {
        /// <summary>
        ///
        /// </summary>
        private string authorityID = "";

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 권한 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_ownerIDs">사용자 및 부서아이디</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataSet GetAuthority(int _companyID, string _categoryID, string _ownerIDs, string _gubun)
        {
            Data.Authority data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Authority();
            return data.GetAuthority(_companyID, _categoryID, _ownerIDs, _gubun);
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 본인 권한이 할당된 카테고리값 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptIDs">The _dept I ds.</param>
        /// <param name="_getSubCategoryID">The _get sub category ID.</param>
        /// <returns></returns>
        public string GetMyAuthCategoryID(int _companyID, int _userID, string _deptIDs, string _getSubCategoryID)
        {
            if (_getSubCategoryID == "") return "";
            string subCategoryID = _getSubCategoryID;
            string myAuthCategoryID = String.Empty;
            DataSet dsMemberCategoryID = new DataSet();
            DataSet dsDeptCategoryID = new DataSet();
            DataSet dsCompanyCategoryID = new DataSet();

            ArrayList sendCategoryIDArr = new ArrayList();
            foreach (string str in _getSubCategoryID.Split(','))
            {
                sendCategoryIDArr.Add(str);
            }

            //1. 사용자 카테고리

            dsMemberCategoryID = GetAuthority(_companyID, subCategoryID, _userID.ToString(), "1");

            for (int i = 0; i < dsMemberCategoryID.Tables[0].Rows.Count; i++)
            {
                sendCategoryIDArr.Remove(dsMemberCategoryID.Tables[0].Rows[i]["CategoryID"].ToString());
            }

            foreach (DataRow dr in dsMemberCategoryID.Tables[0].Rows)
            {
                if (dr["CategoryID"].ToString() != "")
                {
                    myAuthCategoryID += dr["CategoryID"].ToString() + ",";
                }
            }

            subCategoryID = "";

            foreach (string str in sendCategoryIDArr)
            {
                if (str != "")
                {
                    subCategoryID += str + ",";
                }
            }

            //2. 부서 카테고리

            if (subCategoryID.Length > 0)
            {
                dsDeptCategoryID = GetAuthority(_companyID, subCategoryID.TrimEnd(','), _deptIDs, "2");

                for (int i = 0; i < dsDeptCategoryID.Tables[0].Rows.Count; i++)
                {
                    sendCategoryIDArr.Remove(dsDeptCategoryID.Tables[0].Rows[i]["CategoryID"].ToString());
                }

                foreach (DataRow dr in dsDeptCategoryID.Tables[0].Rows)
                {
                    if (dr["CategoryID"].ToString() != "")
                    {
                        myAuthCategoryID += dr["CategoryID"].ToString() + ",";
                    }
                }

                subCategoryID = "";

                foreach (string str in sendCategoryIDArr)
                {
                    if (str != "")
                    {
                        subCategoryID += str + ",";
                    }
                }

                if (subCategoryID.Length > 0)
                {
                    //3. 회사 카테고리
                    dsCompanyCategoryID = GetAuthority(_companyID, subCategoryID.TrimEnd(','), "0", "3");

                    foreach (DataRow dr in dsCompanyCategoryID.Tables[0].Rows)
                    {
                        if (dr["CategoryID"].ToString() != "")
                        {
                            myAuthCategoryID += dr["CategoryID"].ToString() + ",";
                        }
                    }
                }
            }

            myAuthCategoryID = myAuthCategoryID.TrimEnd(',');

            return myAuthCategoryID;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 권한 체크(true 또는 false / 권한여부 및 1~4 (authority)의 값),  1, write : 2
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept identifier.</param>
        /// <param name="_readWrite">read : 1, write : 2</param>
        /// <returns></returns>
        public string IsAutority(int _companyID, int _categoryID, int _userID, int _deptID, string _readWrite)
        {
            DataSet ds = new DataSet();
            Data.Category biz = new Data.Category();
            ds = biz.GetCategory(_companyID, _categoryID);

            string getAuthority = "";
            BLL.Category bizCat = new BLL.Category();

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                //모바일 사용여부에 따라
                if (dbRow["OptionMobile"].ToString() == "1")
                {
                    // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                    // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                    NameValueCollection nvcAuth = new NameValueCollection();

                    if (_readWrite == "2") //쓰기권한
                    {
                        //쓰기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(_companyID, _userID,
                            _deptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, "2,4", true, false);
                        if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                    }
                    else if (_readWrite == "1") //읽기 권한
                    {
                        //읽기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(_companyID, _userID,
                            _deptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, "1,2,3,4", true, false);
                        if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                    }
                }
            }

            return getAuthority;

            #region 미사용

            /*

            //1. 게시판 카테고리에 해당된 회사,부서,개인 아이디값 가져오기
            DataSet ds = new DataSet();
            Data.Authority biz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Authority();
            ds = biz.GetAuthority(_companyID, _categoryID);
            DataView dv = ds.Tables[0].DefaultView;

            string authDeptID = "0";

            if (ds == null || dv.Count == 0)
            {
                return "false/";
            }
            else
            {
                authDeptID = IsAutorityLvl(_companyID, _userID, _deptID, ds);
            }

            string deptMemberIDs;
            string authValue = "";
            string tempReadWrite = "";
            string[] tempReadWriteStr = null;
            bool authcheck = false;
            int order = 0;

            //*1. 개인권한이 1순위
            //* 2. 부서권한이 2순위
            //* 3. 회사권한이 3순위

            for (int i = 0; i < dv.Count; i++)
            {
                authcheck = false;

                //1. 개인아이디값이 권한테이블에 있는지 체크
                if (dv[i]["Type"].ToString().Equals("M"))
                {
                    if (_userID.ToString().Equals(dv[i]["OwnerID"].ToString()))
                    {
                        order = 1;
                        authcheck = true;
                        authValue = dv[i]["Authority"].ToString();
                    }
                }

                //2. 개인아이디가 없으면, 부서아이디로 권한을 체크한다.
                if (authcheck == false)
                {
                    if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString().Equals(authDeptID)) //부서로 권한이 할당되어 있는 경우, 재귀함수로 하위디렉토리 검색해서 개인아이디가 있는지 확인
                    {
                        //해당 부서의 값으로 하위 디렉토리를 재귀함수 돌릴 경우, 부서아이디와 맴버아이이가 콤마로 분류되어 있음(무지하게 많음)
                        //*** 부서아이디를 넣어서 사용자를 추출한다.***
                        deptMemberIDs = GetAuthorityDeptMemeber(_companyID, Convert.ToInt32(dv[i]["OwnerID"])).ToString();

                        if (deptMemberIDs.Length > 0)
                        {
                            deptMemberIDs = "," + deptMemberIDs + ",";

                            //해당 사용자들값에 나의 사용자가 존재한다면?
                            if (deptMemberIDs.IndexOf("," + _userID.ToString() + ",") > -1)
                            {
                                order = 2;
                                //나의 아이디값이 들어 있다면
                                authcheck = true;
                                authValue = dv[i]["Authority"].ToString();
                            }
                        }
                    }
                }

                //3. 둘다 없으면 , 회사아이디로 찾는다.
                if (authcheck == false)
                {
                    //회사 아이디는 0으로 저장되어 있음
                    if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString() == "0")
                    {
                        order = 3;
                        authcheck = true;
                        authValue = dv[i]["Authority"].ToString();
                    }
                }

                //권한 값이 존재하면 순서와 권한값(1,2,3,4) 을 가져온다
                if (authcheck)
                {
                    //임시 저장 = 권한값 + 권한순서
                    tempReadWrite += order + ":" + authValue + ",";
                }
            }

            tempReadWriteStr = tempReadWrite.TrimEnd(',').Split(',');

            string rtn = "";

            //권한값이 있을 경우
            if (tempReadWrite.Trim().Length > 0)
            {
                //hash table 보다 느리지만, 동일한 키값이 포함될 수 있기 때문에 사용함!!!
                NameValueCollection nvc = new NameValueCollection();
                nvc.Clear();

                //1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
                for (int i = 0; i < tempReadWriteStr.Length; i++)
                {
                    // 0 : 권한값, 1 : 권한순서
                    string[] modeOrderStr = tempReadWriteStr[i].ToString().Split(':');
                    nvc.Add(modeOrderStr[0].ToString(), modeOrderStr[1].ToString());
                }

                //1순위 (사람)
                if (nvc["1"] != "" && nvc["1"] != null)
                {
                    if (nvc["1"].IndexOf(",") > 0) //사람이 다중일 경우 콤마로 나옴
                    {
                        string[] nvcStr = nvc["1"].Split(',');
                        rtn = "true/" + nvcStr[0].ToString();
                    }
                    else
                    {
                        rtn = "true/" + nvc["1"];
                    }
                } //2순위 (부서)
                else if (nvc["2"] != "" && nvc["2"] != null)
                {
                    if (nvc["2"].IndexOf(",") > 0)
                    {
                        string[] nvcStr = nvc["2"].Split(',');
                        rtn = "true/" + nvcStr[0].ToString();
                    }
                    else
                    {
                        rtn = "true/" + nvc["2"];
                    }
                } //3순위 (회사)
                else if (nvc["3"] != "" && nvc["3"] != null)
                {
                    if (nvc["3"].IndexOf(",") > 0)
                    {
                        string[] nvcStr = nvc["3"].Split(',');
                        rtn = "true/" + nvcStr[0].ToString();
                    }
                    else
                    {
                        rtn = "true/" + nvc["3"];
                    }
                }
            }

            if (rtn == "")
            {
                rtn = "false/";
            }

            return rtn;

            */

            #endregion 미사용
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 카테고리별로 할당된 부서 및 맴버 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_ownerID">The _owner ID.</param>
        /// <returns></returns>
        private string GetAuthorityDeptMemeber(int _companyID, int _ownerID)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            BLL.Dept dept = new Dept();
            ds = dept.GetDeptUserTree(_companyID);
            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["OwnerID"], ds.Tables[0].Columns["ParentOwnerID"], false);
            //ds.WriteXml("C:/wmpub/love.xml");

            //dataset of tvData 을 foreach 함
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                //추출하고자 하는 최상위 부모 아이디값
                //부서는 G다음에 아이디값, 맴버는 아이디만 표기
                if (dbRow["OwnerID"].ToString() == "G" + _ownerID.ToString())
                {
                    if (!authorityID.Equals("")) authorityID += ",";
                    authorityID += dbRow["OwnerID"].ToString();
                    PopulateSubNode(dbRow);
                }
            }

            return authorityID;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   :  하위 노드의 값들을 추출함.
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-04
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        private void PopulateSubNode(DataRow _dbRow)
        {
            //자식값을 가져오기
            foreach (DataRow childRow in _dbRow.GetChildRows("DeptRelation"))
            {
                authorityID += "," + childRow["OwnerID"].ToString();
                //자식값을 재귀함수 이용해서 추출해 오기
                PopulateSubNode(childRow);
            }
        }

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
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategorys(int _companyID, int _userID, string _deptList,
            string _childCategoryList, string _gubun)
        {
            if (_childCategoryList == "") return null;

            NameValueCollection myAuthCatList = new NameValueCollection(); //순위별로 내려가면서 나의 권한을 수집한다.
            ArrayList existCatIDArr = new ArrayList();//순위별로 카테고리 접근 권한을 찾으면 그 카테고리를 제외하여 다음 순위로 보낸다.
            string existCatID = "";//existCatIDArr 의 콤마 문자열

            foreach (string cat in _childCategoryList.Split(',')) existCatIDArr.Add(cat);

            //################ 1. 사용자 ################
            Data.Authority authBiz = new Data.Authority();
            NameValueCollection mCatList = authBiz.GetMyAuthCategory(_companyID, _childCategoryList, _userID.ToString(), "1");

            foreach (string cat in mCatList.AllKeys) existCatIDArr.Remove(cat);//사용자에 나오는 권한 삭제하기

            foreach (string cat in mCatList)
            {
                if (_gubun.IndexOf(mCatList[cat]) != -1) //요청한 권한값과 맞을 경우
                {
                    myAuthCatList.Add(cat.ToString(), mCatList[cat]);
                }
            }

            existCatID = "";

            foreach (string cat in existCatIDArr) existCatID += cat + ",";

            //################ 2. 부서 ################

            if (existCatID.Length > 0)
            {
                NameValueCollection dCatList = authBiz.GetMyAuthCategory(_companyID, existCatID.TrimEnd(','), _deptList, "2");

                foreach (string cat in dCatList) existCatIDArr.Remove(cat);
                foreach (string cat in dCatList)
                {
                    if (_gubun.IndexOf(dCatList[cat]) != -1) //요청한 권한값과 맞을 경우
                    {
                        myAuthCatList.Add(cat.ToString(), dCatList[cat]);
                    }
                }

                existCatID = "";

                foreach (string cat in existCatIDArr) existCatID += cat + ",";

                if (existCatID.Length > 0)
                {
                    //################ 3. 회사 ################

                    NameValueCollection hCatList = authBiz.GetMyAuthCategory(_companyID, existCatID.TrimEnd(','), "0", "3");
                    foreach (string cat in hCatList)
                    {
                        if (_gubun.IndexOf(hCatList[cat]) != -1) //요청한 권한값과 맞을 경우
                        {
                            myAuthCatList.Add(cat.ToString(), hCatList[cat]);
                        }
                    }
                }
            }

            return myAuthCatList;
        }
    }
}