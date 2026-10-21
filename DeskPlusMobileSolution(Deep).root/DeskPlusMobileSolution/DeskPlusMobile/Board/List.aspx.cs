using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlusMobile.Web.Board.Ws;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageNo = 1;
        public string mainCID = "0";
        public string searchValue = "";
        public string pCatID = "";
        public string authority = "";
        public string OptionUseDept = "0";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadParam();

                //통합게시판 회사아이디
                int BoaComID = Ws.BLL.Category.GetCompanyCategoryInfo(Convert.ToInt32(pCatID));

                spAdd.Visible = false;
                NameValueCollection nvc = new NameValueCollection();

                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category biz = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                DataSet ds = biz.GetCategory(UserInfo.CompanyID, Convert.ToInt32(pCatID));

                string targetCategory = "";
                string isAuthority = "";

                if (ds != null)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "OptionMobile = '1'";
                    foreach (DataRow dr in dv.Table.Rows)
                    {
                        Title = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                        if (Title.Length > 15)
                        {
                            Title = Title.Substring(0, 15) + "..";
                        }

                        OptionUseDept = dr["OptionUseDept"].ToString();

                        if (dr["OptionUseView"].ToString() == "0")
                        {
                            //부서별게시판은 권한없이 쓰기 가능                            
                            spAdd.Visible = (OptionUseDept == "1") ? true : false;

                            //2 : 쓰기 , 4 : 쓰기 + 댓글쓰기
                            nvc = biz.GetMyAuthCategory(BoaComID, UserInfo.UserID, UserInfo.DeptID, Convert.ToInt32(pCatID), dr["OptionUseDept"].ToString(), false, "2,4", true, false);

                            foreach (string s in nvc.AllKeys) spAdd.Visible = true;

                            //일반 카테고리라면..
                            isAuthority = biz.GetCategoryAuthority(BoaComID, pCatID.ToNumber<int>(), UserID.ToNumber<int>(), UserInfo.DeptID, "1", false);
                            if (isAuthority != "") targetCategory = pCatID;
                        }
                        else
                        {
                            string childCatList = biz.GetChildCategory(BoaComID, pCatID.ToNumber<int>());

                            if (childCatList.Length > 0)
                            {
                                foreach (string catID in childCatList.Split(','))
                                {
                                    isAuthority = biz.GetCategoryAuthority(BoaComID, catID.ToNumber<int>(), UserID.ToNumber<int>(), UserInfo.DeptID, "1", false);

                                    //해당 카테고리 게시판을 볼 수 있는지 확인
                                    if (isAuthority != "")
                                    {
                                        targetCategory += catID + ",";
                                    }
                                }

                                targetCategory = targetCategory.TrimEnd(',');
                            }
                        }
                    }
                }

                tbSearch.Value = searchValue;

                if (targetCategory != "")
                {
                    GetListCount(BoaComID, targetCategory);
                }
                else
                {
                    TotalNum.InnerText = "0";
                    PageNum.InnerText = "1";
                }
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            pCatID = GetDecryptParam("PCatID", "");
            mainCID = GetDecryptParam("MainCID", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-11-02
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount(int _companyID, string _targetCategory)
        {
            Ws.Data.List dal = new Ws.Data.List();
            int Total = dal.GetListCount(_companyID, _targetCategory, OptionUseDept, UserInfo.DeptID, searchValue);

            int recsInLastPage = Total % 10;
            int TotalPage = 0;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
            {
                TotalPage = (Total / 10);
            }
            else
            {
                TotalPage = (Total / 10) + 1;
            }

            if (TotalPage < pageNo)
            {
                pageNo = 1;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = pageNo.ToString();
        }

        public new string Title { get; set; }

    }
}