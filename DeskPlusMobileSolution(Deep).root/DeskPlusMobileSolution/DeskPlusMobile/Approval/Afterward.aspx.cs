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
    public partial class Afterward : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string dID = "", stat = "";
        public string bPage = "";
        public int pageNo = 0;
        public string aTag = "", accTag = "", num2 = "", seq = "";
        public int targetUID = 0;
        public int targetDID = 0;
        public string searchValue = "";
        public string NextTag = "";
        public int NextSeq = 0;
        public string IsApprovePwd = "N";
        public string uPwd = "";
        public string groupID = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Afterward).Assembly;	//어셈블리 선언
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
            targetUID = GetDecryptParam("TargetUID", "0").ToNumber<int>();
            targetDID = GetDecryptParam("TargetDID", UserInfo.DeptID.ToString()).ToNumber<int>();
            NextTag = GetDecryptParam("NextTag", "");
            NextSeq = GetDecryptParam("NextSeq", "0").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            groupID = GetDecryptParam("GroupID", "0");

            uPwd = UserInfo.Password;
            IsApprovePwd = (Session["IsApprovePwd"] == null) ? "N" : Session["IsApprovePwd"].ToString();  // N,Y
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-08-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                // 후열사유
                ddlReason.Items.Add(new ListItem(this.resMng.GetString("Msg_LaterOnSelItem"), ""));
                ddlReason.Items.Add(new ListItem("/", "/"));
                ddlReason.Items.Add(new ListItem(this.resMng.GetString("LaterOnItem1"), this.resMng.GetString("LaterOnItem1")));
                ddlReason.Items.Add(new ListItem(this.resMng.GetString("LaterOnItem2"), this.resMng.GetString("LaterOnItem2")));
                ddlReason.Items.Add(new ListItem(this.resMng.GetString("LaterOnItem3"), this.resMng.GetString("LaterOnItem3")));
                ddlReason.Items.Add(new ListItem(this.resMng.GetString("LaterOnItem4"), this.resMng.GetString("LaterOnItem4")));
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-08-07
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
                }
                else
                {
                    CustomFileName = "";
                }

                #region 원결재자정보 (For. 후열처리시, 대결체크)
                string sqlSeq = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ApprovalTag");
                sqlSeq = string.Format(sqlSeq, CompanyID.ToString(), dID, NextSeq, NextTag);
                DataSet dsSeq = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlSeq, "dsSeq", null);

                if (dsSeq.Tables[0].Rows.Count > 0)
                {
                    OrginUserID = dsSeq.Tables[0].Rows[0]["UserID"].ToString().ToNumber<int>();
                    OrginDeptID = dsSeq.Tables[0].Rows[0]["DeptID"].ToString().ToNumber<int>();
                }

                #endregion

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadDocument - Afterward('" + _dID + "')", ex);
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

        /// <summary>
        /// 원결재자UserID
        /// </summary>
        public int OrginUserID
        {
            get { return GetViewStateInt("OrginUserID", "0"); }
            set { ViewState["OrginUserID"] = value; }
        }
        /// <summary>
        /// 원결재자DeptID
        /// </summary>
        public int OrginDeptID
        {
            get { return GetViewStateInt("OrginDeptID", "0"); }
            set { ViewState["OrginDeptID"] = value; }
        }

        public string Today { get; set; }
    }
}