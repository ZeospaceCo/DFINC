using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class Rate : Base.BasePageBoard
    {
        #region 변수설정

        public int ListID = 0;
        private bool IsVote = false;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.Rate).Assembly;	//어셈블리 선언

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
                CbtnRatingList.Text = this.resMng.GetString("RatingList");
                CbtnConfirm.Text = this.resMng.GetString("Confirm");
                CbtnClose.Text = this.resMng.GetString("Closed");
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            BLL.Category biz = new BLL.Category();
            bool response = biz.GetAccessRating(BoaComID, CategoryID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, AccessAuth);

            if (response == false) CbtnRatingList.Visible = false;

            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["OptionVote"].ToString() == "1")
                    {
                        GetRate();
                    }
                    else
                    {
                        Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    }
                }
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
        /// 작성자  : 심재운
        /// 투표 정보 가져오기
        /// </summary>
        private void GetRate()
        {
            BE.RatingInfo ratingInfo = new RatingInfo();
            ratingInfo.CompanyID = BoaComID;
            ratingInfo.CategoryID = CategoryID;
            ratingInfo.ListID = ListID;
            ratingInfo.WriteUserID = CurrentUserInfo.UserID;

            DAL.Ratings biz = new DAL.Ratings();
            List<BE.RatingInfo> al = biz.GetRating(ratingInfo);

            //값이 존재하면 투표를 이미 한 사용자임
            foreach (BE.RatingInfo info in al)
            {
                IsVote = (info.RatingID > 0) ? true : false;
            }

            pnlVoteCast.Visible = IsVote;
            pnlCastVote.Visible = !IsVote;
            this.CbtnConfirm.Visible = !IsVote;

            int[] ratings = new int[5];
            DataTableReader dtr = biz.GetRatingPoint(ratingInfo);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lblAverageRating.Text = String.Format("<b>{0:0.0}</b> out of 5.", dtr["AvgRating"].ToString().ToNumber<int>());
                        lblRatingCount.Text = String.Concat("<b>", dtr["RatingCount"].ToString(), "</b> " + this.GetResource("Rate_VotedPeople"));

                        ratings[0] = dtr["Rating1Count"].ToString().ToNumber<int>();
                        ratings[1] = dtr["Rating2Count"].ToString().ToNumber<int>();
                        ratings[2] = dtr["Rating3Count"].ToString().ToNumber<int>();
                        ratings[3] = dtr["Rating4Count"].ToString().ToNumber<int>();
                        ratings[4] = dtr["Rating5Count"].ToString().ToNumber<int>();
                    }
                }
            }

            int maxRating = 0;
            for (int i = 0; i < 5; i++)
            {
                if (ratings[i] > maxRating) maxRating = ratings[i];
            }

            if (maxRating > 0)
            {
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                TableRow tr = RatingsGraphTable.Rows[0];
                for (int i = 0; i < 5; i++)
                {
                    System.Web.UI.WebControls.Image bcImg = new System.Web.UI.WebControls.Image();
                    bcImg.ImageUrl = this.BaseImgPath + "/Icon/ico_rating.gif";
                    bcImg.Width = Unit.Pixel(10);
                    bcImg.Height = Unit.Pixel(Convert.ToInt32(200.0F * ((float)ratings[i]) / ((float)maxRating)));
                    tr.Cells[i].Controls.Add(bcImg);
                }
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
    }
}