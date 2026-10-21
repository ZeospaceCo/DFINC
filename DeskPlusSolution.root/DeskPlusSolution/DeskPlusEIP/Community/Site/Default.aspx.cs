using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;
using System.Collections;

namespace Zeospace.DeskPlus.Community.Web.Site
{
    public partial class Default : Base.BasePageCommunity
    {
        #region 변수처리
        public int CommunityID;
        public int Grade = 0;
        public int TmpType = 0;
        public string AdminCheck = "N";
        public string CommunityGuid = String.Empty;

        public string LeftURL = "";
        public string ContentURL = ""; 
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {  
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-08
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CommunityGuid != "")
            {
                DAL.User dal = new DAL.User();
                DataTableReader dtr = dal.GetGubunGUID(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CommunityGuid);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                            Grade = dtr["Grade"].ToString().ToNumber<int>();
                            TmpType = dtr["Type"].ToString().ToNumber<int>();
                            AdminCheck = "N";
                        }
                    }
                }
            }

            GetURL();
        }
       

        #endregion

        #region (함수)

        private void GetURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            //paraDic.Add("dcparm", Dcparm.ToString());
            //paraDic.Add("catID", CatID.ToString());
            //paraDic.Add("menuCatID", MenuCatID.ToString());

            string encParams = string.Empty;            

            if (CommunityID > 0)
            {
                if (Grade == 10)
                {
                    if ((TmpType == 2) || (TmpType == 3))
                    {
                        paraDic.Add("communityID", CommunityID.ToString());
                        encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));            
                        LeftURL = "/DeskPlusEIP/Community/Main/Menu.aspx";
                        ContentURL = "/DeskPlusEIP/Community/Main/Join.aspx?p=" + encParams;
                    }
                    else
                    {
                        paraDic.Add("communityID", CommunityID.ToString());
                        paraDic.Add("grade", Grade.ToString());
                        paraDic.Add("adminCheck", AdminCheck);
                        encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        LeftURL = "./Main/Menu.aspx?p=" + encParams;
                        ContentURL = "./Main/Content.aspx?p=" + encParams;
                    }
                }
                else
                {
                    paraDic.Add("communityID", CommunityID.ToString());
                    paraDic.Add("grade", Grade.ToString());
                    paraDic.Add("adminCheck", AdminCheck);
                    encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));            
                    LeftURL = "./Main/Menu.aspx?p=" + encParams;
                    ContentURL = "./Main/Content.aspx?p=" + encParams;
                }                
            }
            else
            {
                LeftURL = "/DeskPlusEIP/Community/Main/Menu.aspx";
                ContentURL = "/DeskPlusEIP/Community/Main/Content.aspx";
            }
        }

        

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-08
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
            AdminCheck = GetDecryptParam("adminCheck", "N");
            CommunityGuid = GetDecryptParam("gubun", "");
        }
             
        #endregion


        #region (프로퍼티
        #endregion 

        #region (이벤트핸들러)   
        #endregion
    }
}