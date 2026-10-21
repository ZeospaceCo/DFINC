using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.Search
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSAccount
    {
        #region 임직원조회
        /// <summary>
        /// 작성자 : shine
        /// 임직원 조회 중 트리에서 검색 시 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTreePageList(string p)
        {
            try
            {
				#region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Search.List).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string userID = GetDecryptParam("selownerID", "0");
                string deptID = GetDecryptParam("seldeptID", "0");
                string types = GetDecryptParam("types", "");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.User biz = new DAL.User();

                DataTableReader dtr = biz.GetSearchUserDeptPageList(companyID.ToNumber<int>(),
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), setPageingStr[0], setPageingStr[1], types, order);

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new BE.UserInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                               dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

							info.Name = (!dtr["BaseDept"].ToString().Equals("1")) ? info.Name + "(" + this.GetResource("Module_DeptUserTreeSearch_AdditionalJob") + ")" : info.Name;

                            info.DeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                               dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                            info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                            info.DutyName = this.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                                dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), PreLang);

                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.OfficePhone = dtr["OfficePhone"].ToString();
                            info.Mail = dtr["Mail"].ToString();

                            // 오늘의 댓글 카운트
                            info.TodayCount = dtr["TodayCount"].ToString().ToNumber<int>();

                            #region 사진 이미지 설정
                            string photoName = dtr["PhotoImage"].ToString();
                            string BaseImgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            bool response = false;

                            Dictionary<string, string> paraDic = null;                            

                            if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                            {
                                BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                BaseImgPath = string.Format(BaseImgPath, CurrentUserInfo.CompanyID, info.UserID);

                                if (System.IO.File.Exists(Server.MapPath(BaseImgPath))) response = true;
                            }

                            if (response == false) BaseImgPath = baseImageDirectory + "C/Img/Icon/ico_noimg.gif";

                            if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", BaseImgPath);
                                encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                imgTag = "<img src='{0}' width='52' height='52' />";
                                imgTag = string.Format(imgTag, BaseImgPath);                                
                            }

                            info.PhotoImage = imgTag;

                            #endregion

                            listobj.Add(info);
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTreePageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 임직원 조회 중 트리에서 검색 시 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetTreeListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("selownerID", "0");
                string deptID = GetDecryptParam("seldeptID", "0");
                string types = GetDecryptParam("types", "");

                DAL.User biz = new DAL.User();

                int cnt = 0;

                cnt = biz.GetSearchUserDeptPageListCount(companyID.ToNumber<int>(), userID.ToNumber<int>(), deptID.ToNumber<int>(), types);
                return cnt;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSearchPageListCount : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 조건에 따른 임직원 조회시 사용자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSearchUserList(string p)
        {
            try
            {
				#region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Search.List).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");                
                string name = GetDecryptParam("name", "");
                string mobilePhone = GetDecryptParam("mobilePhone", "");
                string deptName = GetDecryptParam("deptName", "");
                string officePhone = GetDecryptParam("officePhone", "");
                string titleName = GetDecryptParam("title", "");
                string mail = GetDecryptParam("mail", "");
                string order = GetDecryptParam("order", "");
                string andOr = GetDecryptParam("andOr", "");

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.User biz = new DAL.User();

                DataTableReader dtr = biz.GetSearchUserPageList(companyID.ToNumber<int>(), name, mail, deptName, officePhone,
                    titleName, mobilePhone, order, andOr, setPageingStr[0], setPageingStr[1]);

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new BE.UserInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.Name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                               dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

							info.Name = (!dtr["BaseDept"].ToString().Equals("1")) ? info.Name + "(" + this.GetResource("Module_DeptUserTreeSearch_AdditionalJob") + ")" : info.Name;

                            info.DeptName = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                               dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                            info.TitleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                            info.DutyName = this.GetLangName(
                                dtr["DutyBase"].ToString(),
                                dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                                dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), PreLang);

                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.OfficePhone = dtr["OfficePhone"].ToString();
                            info.Mail = dtr["Mail"].ToString();

                            // 오늘의 댓글 카운트
                            info.TodayCount = dtr["TodayCount"].ToString().ToNumber<int>();


                            #region 사진 이미지 설정
                            string photoName = dtr["PhotoImage"].ToString();
                            string BaseImgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            bool response = false;

                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                            {
                                BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                BaseImgPath = string.Format(BaseImgPath, CurrentUserInfo.CompanyID, info.UserID);

                                if (System.IO.File.Exists(Server.MapPath(BaseImgPath))) response = true;
                            }

                            if (response == false) BaseImgPath = baseImageDirectory + "C/Img/Icon/ico_noimg.gif";

                            if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", BaseImgPath);
                                encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                imgTag = "<img src='{0}' width='52' height='52' />";
                                imgTag = string.Format(imgTag, BaseImgPath);
                            }

                            info.PhotoImage = imgTag;

                            #endregion

                            listobj.Add(info);
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSearchUserList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 임직원 조건별 검색 시 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSearchUserchListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string name = GetDecryptParam("name", "");
                string mobilePhone = GetDecryptParam("mobilePhone", "");
                string deptName = GetDecryptParam("deptName", "");
                string officePhone = GetDecryptParam("officePhone", "");
                string titleName = GetDecryptParam("title", "");
                string mail = GetDecryptParam("mail", "");
                string andOr = GetDecryptParam("andOr", "");

                int cnt = 0;

                DAL.User biz = new DAL.User();

                cnt = biz.GetSearchUserListCount(companyID.ToNumber<int>(), name, mail, deptName, officePhone, titleName, mobilePhone, andOr);
                return cnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUserSearchListCount : " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
