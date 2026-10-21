using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class ViewShortReplyList : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        public int ListID = 0;

        private string OptionUseShortReply = string.Empty;//댓글을 사용합니다.
        public string OptionShortReplyGood = string.Empty;//댓글 추천/비추천 기능을 사용합니다.
        public string OptionShortReplyTop = string.Empty;//추천 댓글을 최상위에 보여주기
        private string OptionUseDept = string.Empty;//부서별 게시판 여부

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.ViewShortReplyList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = this.resMng.GetString("Closed");
                cbtnShortReply.Text = this.resMng.GetString("View_AddShortReply");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CategoryID == 0 && ListID == 0) return;

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            GetCategory();
            LoadList();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시물 내용
        /// </summary>
        private void LoadList()
        {
            #region 내용

            BLL.Category bizCat = new BLL.Category();
            NameValueCollection nvcAuth = new NameValueCollection();

            //읽기 이상 존재하는지 체크
            nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                CurrentUserInfo.DeptID, CategoryID, OptionUseDept, false, BE.BoardAuth.Read, true, false);

            if (nvcAuth.AllKeys.Length == 0)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            #endregion 내용

            #region 버튼 활성화 여부

            cbtnShortReply.Visible = false;
            //pnReple.Visible = (OptionUseShortReply == "1") ? true : false;
            liReplyList.Visible = (OptionUseShortReply == "1") ? true : false;

            //부서별게시판은 권한없이 쓰기 가능
            if (OptionUseDept == "1")
            {
                cbtnShortReply.Visible = true;//댓글
                if (OptionUseShortReply == "0") cbtnShortReply.Visible = false;//댓글 활성화 확인
            }
            else
            {
                NameValueCollection nvc = new NameValueCollection();

                //2 : 쓰기 , 4 : 쓰기 + 댓글쓰기
                foreach (string s in nvcAuth.AllKeys)
                {
                    //댓글 권한자(3,4 는 댓글 추가 가능)
                    if (nvcAuth[s].IndexOf("3") != -1 || nvcAuth[s].IndexOf("4") != -1)
                    {
                        if (OptionUseShortReply == "1") cbtnShortReply.Visible = true;//댓글추가
                    }
                }
            }

            #endregion 버튼 활성화 여부
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 옵션 적용
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            BLL.Category biz = new BLL.Category();

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseShortReply = dr["OptionUseShortReply"].ToString();//댓글을 사용합니다.
                    OptionShortReplyGood = dr["OptionShortReplyGood"].ToString();//댓글 추천/비추천 기능을 사용합니다.
                    OptionShortReplyTop = dr["OptionShortReplyTop"].ToString();//추천 댓글을 최상위에 보여주기
                    OptionUseDept = dr["OptionUseDept"].ToString();//부서별 게시판 여부
                }
            }
        }

        #endregion (함수)

        #region (프로퍼티)

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        #endregion (이벤트핸들러)
    }
}