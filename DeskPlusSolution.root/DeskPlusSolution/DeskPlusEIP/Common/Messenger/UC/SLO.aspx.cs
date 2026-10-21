using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using System.Configuration;

namespace Zeospace.DeskPlus.Common.Web.Messenger.UC
{
    public partial class SLO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //noClass();
            CountGetUser();
        }

        /// <summary>
        /// 사용자 카운트
        /// </summary>
        private void CountGetUser()
        {
            string response = "-2";

            try
            {

                if (Request.Params["USERID"] == null || Request.Params["USERPASSWORD"] == null || Request.Params["DOMAINVALUE"] == null)
                {
                    response = "-2";
                    Response.Write(response);
                    return;
                }

                string loginID = Request.Params["USERID"].ToString().ToNull();
                string pwd = Request.Params["USERPASSWORD"].ToString().ToNull();
                int companyID = Request.Params["DOMAINVALUE"].ToString().ToNumber<int>();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("loginID1 : " + loginID, AppSettingType.MESSENGER);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("pwd1 : " + pwd, AppSettingType.MESSENGER);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("companyID1 : " + companyID, AppSettingType.MESSENGER);

                loginID = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(loginID.Replace(" ", "+")));
                pwd = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(pwd.Replace(" ", "+")));



                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("loginID2 : " + loginID, AppSettingType.MESSENGER);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("pwd2 : " + pwd, AppSettingType.MESSENGER);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("companyID2 : " + companyID, AppSettingType.MESSENGER);
                if (companyID != 0) //메신저에서 넘어온 CompanyID가 있으면
                {

                    if (loginID.Length > 0 && pwd.Length > 0)
                    {
                        Zeospace.DeskPlus.Common.DAL.UCMessenger dal = new DAL.UCMessenger();
                        DataSet ds = dal.GetUserInUserID(companyID, loginID, pwd);
                        string maxCount = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswrodFaultCount", "0");

                        if (ds != null)
                        {
                            if (Convert.ToInt32(maxCount) > 0 && ds.Tables[0].Rows.Count > 0) //틀린횟수 카운트를 설정했고, 사용자도 ID/PW맞으면
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[0]["PwdCount"].ToString()) >= Convert.ToInt32(maxCount)) //사용자의 틀린횟수를 비교해서 넘었으면 인증실패
                                {
                                    response = "0";
                                }
                                else
                                {
                                    response = "1"; //안넘었으면 인증성공
                                } 
                            }
                            else
                            {
                                response = (ds.Tables[0].Rows.Count > 0) ? "1" : "0"; //틀린횟수 카운트를 설정하지 않았으면 ID/PW맞는지만 체크하고 성공여부 리턴
                            }                            
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SLO UC : " + ex);
                response = "-1";
            }

            Response.Write(response);
        }


        /// <summary>
        /// 클래스 사용안하고 X 풀 코딩 결과값 같음
        /// </summary>
        private void noClass()
        {
            string response = "-2";

            if (Request.Params["USERID"] == null || Request.Params["USERPASSWORD"] == null) response = "-2";

            string loginID = Request.Params["USERID"].ToString().ToNull();
            string pwd = Request.Params["USERPASSWORD"].ToString().ToNull();

            if (loginID.Length > 0 && pwd.Length > 0)
            {

                SqlConnection mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString);


                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                string query = @"SELECT UserID FROM DP_ACC_User WHERE LOGINID = @LOGINID AND PASSWORD in (select SubString(master.dbo.fn_varbintohexstr(HashBytes('SHA1', @PASSWORD)), 3, 100))";
                query = String.Format(query, loginID.Replace("!", "").Replace("-", ""), pwd.Replace("!", "").Replace("-", ""));
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@LOGINID", SqlDbType.VarChar, 100).Value = loginID;
                mySqlCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar, 100).Value = pwd;

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                DataSet myDataSet = new DataSet();

                mySqlConnection.Open();

                mySqlDataAdapter.Fill(myDataSet, "Products");

                mySqlConnection.Close();

                if (myDataSet != null)
                {
                    response = (myDataSet.Tables[0].Rows.Count > 0) ? "1" : "0";
                }
            }

            Response.Write(response);
        }


    }
}