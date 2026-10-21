using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ComponentArt.Web.UI;
using System.Configuration;

namespace Zeospace.DeskPlus.ETC.Web.DecisionRule
{
    public partial class decisionRuleUser : Base.BasePageETC
    {
        #region 변수설정

        public string categoryID;
        public string categoryName;
        public string course;
        public string remark;

        public string targetID;
        public string targetName;

        private string CONN = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            //사업부 Load
            this.LoadCategoryData(CurrentUserInfo.CompanyID);
            //전결권자 Load
            this.LoadTargetData(CurrentUserInfo.CompanyID);
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        private void LoadCategoryData(int _companyID)
        {
            //사업부 List
            string strSQL = "";

            try
            {
                strSQL = "SELECT CategoryID, CategoryName, Course, Remark FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID=" + _companyID.ToString() + " AND ParentCategoryID=0 ";
                strSQL += " ORDER BY CategoryOrder ";

                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        categoryID += reader["CategoryID"].ToString() + "∫";
                        categoryName += reader["CategoryName"].ToString() + "∫";
                        course += reader["Course"].ToString() + "∫";
                        remark += reader["Remark"].ToString() + "∫";
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LoadCategoryData(SQL:::" + strSQL + ")", ex);
            }
        }

        private void LoadTargetData(int _companyID)
        {
            //전결권자 List
            string strSQL = "";

            try
            {
                strSQL = "SELECT TargetID, TargetName FROM Site_APP_DecisionRuleTarget WHERE CompanyID=" + _companyID.ToString() + " ORDER BY TargetOrder ";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetID += reader["TargetID"].ToString() + "∫";
                        targetName += reader["TargetName"].ToString() + "∫";
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LoadTargetData(SQL:::" + strSQL + ")", ex);
            }
        }

        #region Ajax Func

        [ComponentArtCallbackMethod]
        public string decisionListLoad(string p_Gubun, string p_ParentCategoryID, string _companyID)
        {
            //전결사항 가져오기
            string strSQL = "";
            string CategoryID = "";
            string CategoryName = "";
            string Course = "";
            string Remark = "";

            try
            {
                strSQL = "SELECT CategoryID, CategoryName, Course, Remark ";
                strSQL += " FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND parentCategoryID = '" + p_ParentCategoryID + "'";
                strSQL += " ORDER BY CategoryOrder";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        CategoryID += reader["CategoryID"].ToString() + "∫";
                        CategoryName += reader["CategoryName"].ToString() + "∫";
                        Course += reader["Course"].ToString() + "∫";
                        Remark += reader["Remark"].ToString() + "∫";
                    }
                }

                return CategoryID + "∬" + CategoryName + "∬" + Course + "∬" + Remark;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListLoad(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        [ComponentArtCallbackMethod]
        public string decisionTargetLoad(string p_CategoryID, string _companyID)
        {
            //전결권자 가져오기
            string strSQL = "";
            string TargetID = "";
            string TargetIDVal = "";
            string TargetNameVal = "";

            try
            {
                strSQL = "SELECT TargetID ";
                strSQL += " FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryID = '" + p_CategoryID + "'";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        TargetID = reader["TargetID"].ToString();
                    }
                }

                strSQL = "SELECT TargetID, TargetName ";
                strSQL += " FROM Site_APP_DecisionRuleTarget ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetID = '" + TargetID + "'";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        TargetIDVal += reader["TargetID"].ToString();
                        TargetNameVal += reader["TargetName"].ToString();
                    }
                }

                if (TargetIDVal == "")
                {
                    return "0∬규정없음";
                }
                else
                {
                    return TargetIDVal + "∬" + TargetNameVal;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetLoad(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        #endregion


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}