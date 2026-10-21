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
    public partial class decisionRuleList : Base.BasePageETC
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
                strSQL += " WHERE CompanyID = " + _companyID.ToString() + " AND ParentCategoryID = 0 ";
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
                strSQL = "SELECT TargetID, TargetName FROM Site_APP_DecisionRuleTarget WHERE CompanyID = " + _companyID.ToString() + " ORDER BY TargetOrder ";

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

        /// <summary>
        /// 하위분류가져오기
        /// </summary>
        /// <param name="p_Gubun"></param>
        /// <param name="p_ParentCategoryID"></param>
        /// <returns></returns>
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
                strSQL += " WHERE CompanyID = " + _companyID + " AND parentCategoryID = '" + p_ParentCategoryID + "'";
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

        /// <summary>
        /// 소분류 & 전결사항 선택시 전결권자 선택 및 비고 표시
        /// </summary>
        /// <param name="p_CategoryID"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionTargetLoad(string p_CategoryID, string _companyID)
        {
            //전결권자 가져오기
            string strSQL = "";
            string TargetID = "";

            try
            {
                strSQL = "SELECT TargetID ";
                strSQL += " FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "'";

                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        TargetID += reader["TargetID"].ToString() + "∫";
                    }
                }

                return TargetID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetLoad(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="p_ParentCategoryID"></param>
        /// <param name="p_CategoryName"></param>
        /// <param name="p_Course"></param>
        /// <param name="p_TargetID"></param>
        /// <param name="p_Remark"></param>
        /// <param name="p_SelIdx"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionListAdd(string p_ParentCategoryID, string p_CategoryName, string p_Course, string p_TargetID, string p_Remark, string p_SelIdx, string _companyID)
        {
            //전결사항 추가
            string strSQL = "";
            int decisionOrder = 0;
            int decisionNextOrder = 0;
            string categoryID = "";
            string categoryName = "";
            string course = "";
            string remark = "";

            try
            {
                if (p_SelIdx == "-1")
                {
                    strSQL = "SELECT count(categoryID) AS decisionOrder FROM Site_APP_DecisionRuleList WHERE CompanyID=" + _companyID + " AND parentCategoryID=" + p_ParentCategoryID;
                    using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                    {
                        while (reader.Read())
                        {
                            decisionOrder = Convert.ToInt32(reader["decisionOrder"]);
                        }
                    }
                }
                else
                {
                    decisionOrder = Convert.ToInt32(p_SelIdx) + 1;
                }
                decisionNextOrder = decisionOrder + 1;

                strSQL = "UPDATE Site_APP_DecisionRuleList SET CategoryOrder=CategoryOrder+1 ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND parentCategoryID=" + p_ParentCategoryID + " And CategoryOrder > " + decisionOrder;
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "INSERT INTO Site_APP_DecisionRuleList (CompanyID, ParentCategoryID, CategoryOrder, CategoryName, TargetID, Course, Remark) VALUES ";
                strSQL += "(" + _companyID + ",'" + p_ParentCategoryID + "', '" + decisionNextOrder + "', '" + p_CategoryName.Replace("'", "''") + "', '" + p_TargetID + "', '" + p_Course.Replace("'", "''") + "', '" + p_Remark.Replace("'", "''") + "')";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT TOP 1 CategoryID, CategoryName, Course, Remark FROM Site_APP_DecisionRuleList WHERE CompanyID = " + _companyID + " ORDER BY CategoryID DESC";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        categoryID = reader["CategoryID"].ToString();
                        categoryName = reader["CategoryName"].ToString();
                        course = reader["Course"].ToString();
                        remark = reader["Remark"].ToString();
                    }
                }

                return categoryID + "∬" + categoryName + "∬" + course + "∬" + remark;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListAdd(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="p_CategoryID"></param>
        /// <param name="p_CategoryName"></param>
        /// <param name="p_Course"></param>
        /// <param name="p_TargetID"></param>
        /// <param name="p_Remark"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionListMod(string p_CategoryID, string p_CategoryName, string p_Course, string p_TargetID, string p_Remark, string _companyID)
        {
            //전결사항 수정
            string strSQL = "";

            try
            {
                strSQL = "UPDATE Site_APP_DecisionRuleList SET ";
                strSQL += " CategoryName = '" + p_CategoryName.Replace("'", "''") + "', ";
                strSQL += " TargetID = '" + p_TargetID + "', ";
                strSQL += " Course = '" + p_Course.Replace("'", "''") + "', ";
                strSQL += " Remark = '" + p_Remark.Replace("'", "''") + "' ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "'";

                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return "true";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListMod(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="p_ParentCategoryID"></param>
        /// <param name="p_CategoryID"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionListDel(string p_ParentCategoryID, string p_CategoryID, string _companyID)
        {
            //전결사항 삭제
            string strSQL = "";
            int categoryOrder = 0;

            try
            {
                strSQL = "SELECT CategoryOrder FROM Site_APP_DecisionRuleList WHERE CompanyID=" + _companyID + " AND CategoryID = '" + p_CategoryID + "'";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        categoryOrder = Convert.ToInt32(reader["CategoryOrder"]);
                    }
                }

                strSQL = "UPDATE Site_APP_DecisionRuleList SET ";
                strSQL += " CategoryOrder = CategoryOrder - 1 ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryOrder > '" + categoryOrder + "'";
                strSQL += " AND ParentCategoryID = '" + p_ParentCategoryID + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "DELETE FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return "true";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListDel(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 전결권자 추가
        /// </summary>
        /// <param name="p_TargetName"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionTargetAdd(string p_TargetName, string _companyID)
        {
            //전결권자 추가
            string strSQL = "";
            string rtnVal = "";
            int targetOrder = 0;

            try
            {
                strSQL = "SELECT Count(TargetID) AS TargetCnt FROM Site_APP_DecisionRuleTarget WHERE CompanyID=" + _companyID;
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetOrder = Convert.ToInt32(reader["TargetCnt"]) + 1;
                    }
                }

                strSQL = "INSERT INTO Site_APP_DecisionRuleTarget (CompanyID, TargetName, TargetOrder) VALUES (" + _companyID + ",'" + p_TargetName.Replace("'", "''") + "', " + targetOrder + ")";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT TOP 1 TargetID, TargetName FROM Site_APP_DecisionRuleTarget WHERE CompanyID = " + _companyID + " ORDER BY targetID Desc";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        rtnVal = reader["TargetID"].ToString() + "∬" + reader["TargetName"].ToString();
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetAdd(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 전결권자 수정
        /// </summary>
        /// <param name="p_TargetID"></param>
        /// <param name="p_TargetName"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionTargetMod(string p_TargetID, string p_TargetName, string _companyID)
        {
            //전결권자 수정
            string strSQL = "";
            try
            {

                strSQL = "UPDATE Site_APP_DecisionRuleTarget SET ";
                strSQL += " TargetName = '" + p_TargetName.Replace("'", "''") + "' ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND TargetID = '" + p_TargetID + "'";

                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return p_TargetName;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetMod(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 전결권자 삭제
        /// </summary>
        /// <param name="p_TargetID"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionTargetDel(string p_TargetID, string _companyID)
        {
            //전결권자 삭제
            string strSQL = "";
            int targetOrder = 0;

            try
            {
                strSQL = "SELECT TargetOrder FROM Site_APP_DecisionRuleTarget WHERE CompanyID = " + _companyID + " AND TargetID = '" + p_TargetID + "'";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetOrder = Convert.ToInt32(reader["TargetOrder"]);
                    }
                }

                strSQL = "UPDATE Site_APP_DecisionRuleTarget SET ";
                strSQL += " TargetOrder = TargetOrder - 1 ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetOrder > '" + targetOrder + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "DELETE FROM Site_APP_DecisionRuleTarget ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetID = '" + p_TargetID + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return "true";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetDel(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 전결권자 위
        /// </summary>
        /// <param name="p_CategoryID"></param>
        /// <param name="p_ParentCategoryID"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionListUp(string p_CategoryID, string p_ParentCategoryID, string _companyID)
        {
            //전결사항 위로
            string strSQL = "";
            string categoryID = "";
            string categoryName = "";
            string course = "";
            string remark = "";
            int categoryOrder = 0;
            int categoryPrvOrder = 0;

            try
            {
                strSQL = "SELECT CategoryOrder FROM Site_APP_DecisionRuleList WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        categoryOrder = Convert.ToInt32(reader["CategoryOrder"]);
                        categoryPrvOrder = Convert.ToInt32(reader["CategoryOrder"]) - 1;
                    }
                }

                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryOrder = '" + categoryOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryOrder = '" + categoryPrvOrder + "' ";
                strSQL += "  AND ParentCategoryID='" + p_ParentCategoryID + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryOrder = '" + categoryPrvOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT CategoryID, CategoryName, Course, Remark FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND ParentCategoryID = '" + p_ParentCategoryID + "' ";
                strSQL += " ORDER BY CategoryOrder";
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

                return categoryID + "∬" + categoryName + "∬" + course + "∬" + remark;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListUp(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        /// <summary>
        /// 전결권자 아래
        /// </summary>
        /// <param name="p_CategoryID"></param>
        /// <param name="p_ParentCategoryID"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string decisionListDown(string p_CategoryID, string p_ParentCategoryID, string _companyID)
        {
            //전결사항 아래로
            string strSQL = "";
            string categoryID = "";
            string categoryName = "";
            string course = "";
            string remark = "";
            int categoryOrder = 0;
            int categoryNextOrder = 0;

            try
            {
                strSQL = "SELECT CategoryOrder FROM Site_APP_DecisionRuleList WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        categoryOrder = Convert.ToInt32(reader["CategoryOrder"]);
                        categoryNextOrder = Convert.ToInt32(reader["CategoryOrder"]) + 1;
                    }
                }

                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryOrder = '" + categoryOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryOrder = '" + categoryNextOrder + "' ";
                strSQL += "  AND ParentCategoryID='" + p_ParentCategoryID + "'";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryOrder = '" + categoryNextOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT CategoryID, CategoryName, Course, Remark FROM Site_APP_DecisionRuleList ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND ParentCategoryID = '" + p_ParentCategoryID + "' ";
                strSQL += " ORDER BY CategoryOrder";
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

                return categoryID + "∬" + categoryName + "∬" + course + "∬" + remark;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListDown(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        [ComponentArtCallbackMethod]
        public string decisionTargetUp(string p_TargetID, string _companyID)
        {
            //전결권자 위로
            string strSQL = "";
            string targetID = "";
            string targetName = "";
            int targetOrder = 0;
            int targetPrvOrder = 0;

            try
            {
                strSQL = "SELECT TargetOrder FROM Site_APP_DecisionRuleTarget WHERE CompanyID=" + _companyID + " AND TargetID = '" + p_TargetID + "' ";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetOrder = Convert.ToInt32(reader["TargetOrder"]);
                        targetPrvOrder = Convert.ToInt32(reader["TargetOrder"]) - 1;
                    }
                }

                strSQL = "Update Site_APP_DecisionRuleTarget Set ";
                strSQL += " TargetOrder = '" + targetOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetOrder = '" + targetPrvOrder + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "Update Site_APP_DecisionRuleTarget Set ";
                strSQL += " TargetOrder = '" + targetPrvOrder + "' ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND TargetID = '" + p_TargetID + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT TargetID, TargetName FROM Site_APP_DecisionRuleTarget WHERE CompanyID=" + _companyID + " ORDER BY TargetOrder";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetID += reader["TargetID"].ToString() + "∫";
                        targetName += reader["TargetName"].ToString() + "∫";
                    }
                }

                return targetID + "∬" + targetName;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetUp(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        [ComponentArtCallbackMethod]
        public string decisionTargetDown(string p_TargetID, string _companyID)
        {
            //전결권자 아래로
            string strSQL = "";
            string targetID = "";
            string targetName = "";
            int targetOrder = 0;
            int targetNextOrder = 0;

            try
            {
                strSQL = "SELECT TargetOrder FROM Site_APP_DecisionRuleTarget WHERE CompanyID = " + _companyID + " AND TargetID = '" + p_TargetID + "' ";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetOrder = Convert.ToInt32(reader["TargetOrder"]);
                        targetNextOrder = Convert.ToInt32(reader["TargetOrder"]) + 1;
                    }
                }

                strSQL = "Update Site_APP_DecisionRuleTarget Set ";
                strSQL += " TargetOrder = '" + targetOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetOrder = '" + targetNextOrder + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "Update Site_APP_DecisionRuleTarget Set ";
                strSQL += " TargetOrder = '" + targetNextOrder + "' ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND TargetID = '" + p_TargetID + "' ";
                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                strSQL = "SELECT TargetID, TargetName FROM Site_APP_DecisionRuleTarget WHERE CompanyID=" + _companyID + " ORDER BY TargetOrder";
                using (SqlDataReader reader = SQLHelper.ExecuteReader(CONN, CommandType.Text, strSQL, null))
                {
                    while (reader.Read())
                    {
                        targetID += reader["TargetID"].ToString() + "∫";
                        targetName += reader["TargetName"].ToString() + "∫";
                    }
                }

                return targetID + "∬" + targetName;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionTargetDown(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        [ComponentArtCallbackMethod]
        public string decisionListLeft(string p_CategoryID, string _companyID)
        {
            //소분류 & 전결사항 공백 제거
            string strSQL = "";

            try
            {
                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryName = Replace(CategoryName, '   ', '') ";
                strSQL += " WHERE CompanyID = " + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";

                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return "true";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListLeft(SQL:::" + strSQL + ")", ex);
                return "false";
            }
        }

        [ComponentArtCallbackMethod]
        public string decisionListRight(string p_CategoryID, string _companyID)
        {
            //소분류 & 전결사항 공백 추가
            string strSQL = "";

            try
            {
                strSQL = "Update Site_APP_DecisionRuleList Set ";
                strSQL += " CategoryName = '   ' + CategoryName ";
                strSQL += " WHERE CompanyID=" + _companyID + " AND CategoryID = '" + p_CategoryID + "' ";

                SQLHelper.ExecuteNonQuery(CONN, CommandType.Text, strSQL, null);

                return "true";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("decisionListRight(SQL:::" + strSQL + ")", ex);
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