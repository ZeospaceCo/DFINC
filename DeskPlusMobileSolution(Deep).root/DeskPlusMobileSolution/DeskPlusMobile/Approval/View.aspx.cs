using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class View : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 1;
        public string dID = "", stat = "", aTag = "", accTag = "", num2 = "", seq = "";
        public string cDID = "";
        public int targetUID = 0;
        public int targetDID = 0;
        public string searchValue = "";
        public string PhotoImage = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.View).Assembly;	//어셈블리 선언
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

                LoadData();
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
            targetUID = GetDecryptParam("TargetUID", UserID).ToNumber<int>();
            targetDID = GetDecryptParam("TargetDID", UserInfo.DeptID.ToString()).ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
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
        /// </summary>
        public void LoadData()
        {
            LoadParam();

            GetUserSeal();

            LoadPhotoImage();

            GetShortInfo();

            if (bPage == "APP")
            {
                btnOpinion.Visible = false;

                if (stat == "R")
                {
                    btnApproveA.Visible = false;
                    btnApproveS.Visible = false;
                    btnApproveH.Visible = false;
                    btnApproveHR.Visible = false;
                    btnReturn.Visible = false;
                    btnApproveSC.Visible = true;
                    btnApproveCancel.Visible = false;
                }
                else
                {
                    if (aTag.IndexOf("ZA") > -1)
                    {
                        btnApproveA.Visible = false;
                        btnApproveS.Visible = false;
                        btnApproveH.Visible = true;
                        btnApproveHR.Visible = true;
                        btnReturn.Visible = false;
                        btnApproveSC.Visible = false;
                        btnApproveCancel.Visible = false;
                    }
                    else
                    {
                        btnApproveA.Visible = true;
                        btnApproveS.Visible = true;
                        btnApproveH.Visible = false;
                        btnApproveHR.Visible = false;
                        btnReturn.Visible = true;
                        btnApproveSC.Visible = false;
                        btnApproveCancel.Visible = false;
                    }
                }
            }
            else if (bPage == "REP")
            {
                btnApproveA.Visible = false;
                btnApproveS.Visible = false;
                btnApproveH.Visible = false;
                btnApproveHR.Visible = false;
                btnReturn.Visible = false;
                btnApproveSC.Visible = false;
                btnApproveCancel.Visible = false;
            }
            else
            {
                // bPage == "REL"
                btnApproveA.Visible = false;
                btnApproveS.Visible = false;
                btnApproveH.Visible = false;
                btnApproveHR.Visible = false;
                btnReturn.Visible = false;
                btnApproveSC.Visible = false;
                btnApproveCancel.Visible = false;

                if (GetIsApproveCancel(CompanyID, dID, seq, aTag) == "Y")
                {
                    btnApproveCancel.Visible = true;
                }
            }

            string IsLast = "";

            // 최종결재 여부, 반려가능수신(ZQ) 여부
            if (aTag.IndexOf("ZA") < 0)
            {
                string[] arrval = GetNextCnt(dID.ToNumber<int>(), dID.ToNumber<int>(), seq.ToNumber<int>(), aTag).Split('-');
                IsLast = arrval[0].Equals("0") ? "Y" : "N";
            }
            else
            {
                IsLast = "N";
            }

            //최종결재자인 경우에는 전결버튼 안 보이게 처리
            if (IsLast == "Y")
            {
                btnApproveA.Visible = false;
            }
        }

        /// <summary>
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

 
        private void LoadPhotoImage()
        {
            string BaseImgPath = "";
            string imgTag = "";
            string encParams = "";

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
            sql = string.Format(sql, CompanyID.ToString(), dID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDocInfo", null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                Account.Data.User dal = new Account.Data.User();
                Account.BE.UserInfo info = dal.GetUser(CompanyID.ToNumber<int>(), ds.Tables[0].Rows[0]["UserID"].ToString().ToNumber<int>());

                string photoName = info.PhotoImage;

                string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

                if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                {
                    BaseImgPath = filePath + "/{0}/Users/{1}/Picture/" + photoName;
                    BaseImgPath = string.Format(BaseImgPath, CompanyID, info.UserID);
                }
                else
                {
                    BaseImgPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
                }
            }
            else
            {
                BaseImgPath = "/DeskPlusMobile/Content/ko/Base/Img/DStyle/myimgdefault.gif";
            }

            if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
            {
                #region 스크린샷
                Dictionary<string, string> paraDic = null;
                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", BaseImgPath);
                encParams = GetEncryptParam(Zeospace.DeskPlusMobile.Web.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                BaseImgPath = "/DeskPlusMobile/Common/ImageView.aspx?p=" + encParams;
                #endregion

                imgTag = "<img src='{0}' style='width:70px; height:auto;' />";
                imgTag = string.Format(imgTag, BaseImgPath);
            }

            PhotoImage = imgTag;

        }

        private void GetShortInfo()
        {
            //결재의견 보기용
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_SelectDoc");
            sql = string.Format(sql, CompanyID.ToString(), dID);
            DataSet dsDoc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

            foreach (DataRow dr in dsDoc.Tables[0].Rows)
            {
                cDID = dr["LastDocID"].ToString().ToNull();
            }

        }

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-12-21
        /// 작성목적 : 인장 정보 
        /// </summary>
        public void GetUserSeal()
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER");
            proc = string.Format(proc, CompanyID, UserID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserSeal", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SealPath = dr["SealImage"].ToString();
                }
            }
        }

        /// <summary>
        /// 결재취소 가능여부 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        public string GetIsApproveCancel(string _companyID, string _documentID, string _sequence, string _approvalTag)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_IsCancelApproval");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@DocumentID", SqlDbType.Int),
                                                            new SqlParameter("@Sequence", SqlDbType.Int),
                                                            new SqlParameter("@ApprovalTag", SqlDbType.VarChar, 5)
                };

                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = _documentID;
                parms[2].Value = _sequence;
                parms[3].Value = _approvalTag;

                return SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms).ToString();

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetIsApproveCancel: " + errMsg);
                throw;
            }
        }

        public string Today { get; set; }

        public string SealPath { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }

    }
}