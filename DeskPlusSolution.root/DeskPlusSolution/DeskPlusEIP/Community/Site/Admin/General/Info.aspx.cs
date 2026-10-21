using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.General
{
    public partial class Info : Base.BasePageCommunitySysop
    {
        #region 변수설정
        public int CommunityID;
        public string ParentID;
        public string ChildID;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.General.Info).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            string tmpJoinType = "1";

            DAL.Com dal = new DAL.Com();
            DataTableReader dtr = dal.GetComInfo(CurrentUserInfo.CompanyID, CommunityID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ctbName.Text = dtr["Name"].ToString().ToReverseXSS();
                        ctbDesc.Text = dtr["Description"].ToString().ToReverseXSS();
                        ParentID = dtr["ParentID"].ToString();
                        ChildID = dtr["ChildID"].ToString();

                        rblType.SelectedValue = dtr["Type"].ToString();
                        tmpJoinType = dtr["JoinType"].ToString();
                        ctbQuestion.Text = dtr["Question"].ToString();
                        ctbAnswer.Text = dtr["Answer"].ToString();
                    }
                }
            }

            if (rblType.SelectedValue == "1")
            {
                rblJoinType.SelectedValue = tmpJoinType;
            }
            else
            {
                rblJoinType.SelectedValue = "1";
            }


            LoadCategory();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 분류 정보 가져오기
        /// </summary>
        private void LoadCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID);

            ddlParent.Items.Clear();
            ddlParent.Items.Add(new ListItem(resMng.GetString("SelParentID"), "0"));

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                                        dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ddlParent.Items.Add(new ListItem(name, dtr["CategoryID"].ToString()));
                    }
                }
            }

            ddlParent.SelectedValue = ParentID;

            DataTableReader dtr1 = dal.GetCategory(CurrentUserInfo.CompanyID, ParentID.ToNumber<int>());

            ddlChild.Items.Clear();
            ddlChild.Items.Add(new ListItem(resMng.GetString("SelChildID"), "0"));

            if (dtr1 != null)
            {
                if (dtr1.HasRows)
                {
                    while (dtr1.Read())
                    {
                        string name = this.GetLangName(dtr1["NameBase"].ToString(), dtr1["NameKo"].ToString(), dtr1["NameEn"].ToString(), dtr1["NameJa"].ToString(),
                                                        dtr1["NameFr"].ToString(), dtr1["NameZhcn"].ToString(), dtr1["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ddlChild.Items.Add(new ListItem(name, dtr1["CategoryID"].ToString()));
                    }
                }
            }

            ddlChild.SelectedValue = ChildID;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";                

                rblType.Items.Clear();
                rblType.Items.Add(new ListItem(resMng.GetString("Type1"), "1"));
                rblType.Items.Add(new ListItem(resMng.GetString("Type2") + "<span class='cm_subtxt'> * " + resMng.GetString("Type2Des") + "</span>", "2"));
                rblType.Items.Add(new ListItem(resMng.GetString("Type3") + "<span class='cm_subtxt'> * " + resMng.GetString("Type3Des") + "</span>", "3"));

                rblJoinType.Items.Clear();
                rblJoinType.Items.Add(new ListItem(resMng.GetString("JoinType1"), "1"));
                rblJoinType.Items.Add(new ListItem(resMng.GetString("JoinType2"), "2"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}