using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class Return : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string tmpCustomFileName = "";
        public string tmpAccountTag = "";
        public string tmpCustomPath = "";
        public string dID = "", stat = "";
        public string bPage = "";
        public int pageNo = 0;
        public string aTag = "", accTag = "", num2 = "", seq = "", appGun = "";
        public string cDID = "", groupID = "";
        public string uPwd = "";
        public int targetUID = 0;
        public int targetDID = 0;
        public string IsLast = "", IsZQ = "";
        public string searchValue = "";
        public string IsApprovePwd = "N";
        public string alarm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Return).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();

                LoadDocument(dID);
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            dID = GetDecryptParam("DID", "0");
            stat = GetDecryptParam("Stat", "");
            aTag = GetDecryptParam("ATag", "");
            accTag = GetDecryptParam("AccTag", "");
            num2 = GetDecryptParam("Num2", "");
            seq = GetDecryptParam("Seq", "0");
            cDID = GetDecryptParam("CDID", "0");
            groupID = GetDecryptParam("GroupID", "0");
            appGun = GetDecryptParam("AppGun", "S");
            targetUID = GetDecryptParam("TargetUID", "0").ToNumber<int>();
            targetDID = GetDecryptParam("TargetDID", UserInfo.DeptID.ToString()).ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            alarm = GetDecryptParam("Alarm", "");

            uPwd = UserInfo.Password;
            IsApprovePwd = (Session["IsApprovePwd"] == null) ? "N" : Session["IsApprovePwd"].ToString();  // N,Y
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
        /// 작성자   : famo23
        /// 작성일자 : 2012-08-31
        /// 작성목적 : 문서 보기 화면으로 가기 
        /// </summary>
        /// <param name="_dID"></param>
        public void LoadDocument(string _dID)
        {
            if (String.IsNullOrEmpty(_dID)) return;

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, CompanyID.ToString(), dID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocDetailInfo", null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string tmpCustomFileName = ds.Tables[0].Rows[0]["CustomFileName"].ToString();
                    string IsLegacy = ds.Tables[0].Rows[0]["IsLegacy"].ToString();

                    if (!tmpCustomFileName.Equals("") && IsLegacy.Equals("1"))
                    {
                        string[] fileArr = tmpCustomFileName.Split('.');
                        CustomFileName = fileArr[0] + "_Complete." + fileArr[1];
                    }
                    else
                    {
                        CustomFileName = "";
                    }

                    // 최종결재 여부, 반려가능수신(ZQ) 여부
                    if (aTag.IndexOf("ZA") < 0)
                    {
                        string[] arrval = GetNextCnt(dID.ToNumber<int>(), groupID.ToNumber<int>(), seq.ToNumber<int>(), aTag).Split('-');
                        IsLast = arrval[0].Equals("0") ? "Y" : "N";
                        IsZQ = arrval[1].Equals("0") ? "N" : "Y";
                    }
                    else
                    {
                        IsLast = "N";
                        IsZQ = "N";
                    }
                }
                else
                {
                    CustomFileName = "";
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadDocument - Return('" + _dID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성일자 : 2012-12-06
        /// 작성자 : famo23
        /// 작성목적 : 최종결재여부, 반려가능수신(ZQ)여부
        /// </summary>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string GetNextCnt(int _documentID, int _groupID, int _sequence, string _approvalTag)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_NEXTCNT");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@DocumentID", SqlDbType.Int),
                                                            new SqlParameter("@GroupID", SqlDbType.Int),
                                                            new SqlParameter("@Sequence", SqlDbType.Int),
                                                            new SqlParameter("@ApprovalTag", SqlDbType.VarChar,20)
                };

                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _sequence;
                parms[4].Value = _approvalTag;

                return SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms).ToString();
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetNextCnt: " + errMsg);
                throw;
            }
        }


        public string CustomFileName { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-08-31
        /// 작성목적 : 커스텀전체경로        
        /// </summary>
        public string CustomFilePath
        {
            get
            {
                //  /DeskPlusMobile/Legacy/
                string customPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "CustomScriptRoot");
                if (!customPath.Equals("") && !CustomFileName.Equals(""))
                {
                    customPath += CustomFileName;       //  /DeskPlusMobile/Legacy/aaa.aspx

                    if (!File.Exists(Server.MapPath(customPath)))
                    {
                        customPath = "";
                    }
                }
                else
                {
                    customPath = "";
                }
                return customPath;
            }
        }

        public string Today { get; set; }
    }
}