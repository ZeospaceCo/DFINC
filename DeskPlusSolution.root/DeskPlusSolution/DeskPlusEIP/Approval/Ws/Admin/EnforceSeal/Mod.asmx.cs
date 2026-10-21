using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.EnforceSeal
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSApproval
    {
        #region 직인정보

        /// <summary>
        /// 직인삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelStamp(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");
            string filename = GetDecryptParam("filename", "");
            string beforefile = GetDecryptParam("beforefile", "");
            string enforceStampID = GetDecryptParam("enforceStampID", "0");

            if (!swfUploadGUID.Equals("") && !filename.Equals(""))
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/
                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID, filename);

                File.Delete(Server.MapPath(souceFile));
            }

            if (!beforefile.Equals(""))
            {
                //DB 처리 
                DAL.EnforceStamp dal = new DAL.EnforceStamp();
                if (dal.ModFile(CurrentUserInfo.CompanyID, enforceStampID.ToNumber<int>()))
                {
                    //파일 처리
                    File.Delete(Server.MapPath(beforefile));
                }
            }

            string noImage = this.BaseImgPath + "/Icon/ico_noimg2.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", noImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            noImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            return "<img src='" + noImage + "' width='60' height='50' alt='' title='' />";

        }

        #endregion

        #region 담당자관리

        /// <summary>
        /// 담당자리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";          //리소스명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.EnforceSeal.Mod).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                int enforceStampID = GetDecryptParam("enforceStampID", "0").ToNumber<int>();

                DAL.EnforceAdmin biz = new DAL.EnforceAdmin();
                DataTableReader dtr = biz.GetEnforceAdminList(CurrentUserInfo.CompanyID, enforceStampID);

                ObjList listobj = new ObjList();
                if (dtr != null)
                {
                    HttpContext context = HttpContext.Current;
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.EnforceAdminInfo info = new EnforceAdminInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.EnforceAdminID = dtr["EnforceAdminID"].ToString().ToNumber<int>();
                            info.EnforceStampID = dtr["EnforceStampID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름/부서/직급/직책 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 사진 이미지 설정
                            string PhotoName = dtr["PhotoImage"].ToString();
                            info.PhotoName = "";
                            string imgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(PhotoName) && PhotoName.Length > 0)
                            {
                                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + PhotoName;
                                imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.UserID);

                                if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                {
                                    #region 스크린샷
                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", imgPath);
                                    encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                    imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    #endregion

                                    imgTag = "<img src='{0}' width='52' height='52' />";
                                    imgTag = string.Format(imgTag, imgPath);
                                    info.PhotoName = imgTag;
                                }
                            }

                            if (info.PhotoName.Length == 0)
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", baseImageDirectory + "C/Img/Icon/ico_noimg.gif");
                                encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                info.PhotoName = "<img src='" + imgPath + "' width='52' height='52' alt='' title='' />";
                            }

                            #endregion

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            info.ListDel = resMng.GetString("Del");

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 
        /// 작성일자 : 
        /// 작성목적 : 담당자 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserList(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);

                string userList = GetDecryptParam("userList", "");
                int enforceStampID = GetDecryptParam("enforceStampID", "0").ToNumber<int>();

                DAL.EnforceAdmin dal = new DAL.EnforceAdmin();

                foreach (string userID in userList.Split(','))
                {
                    BE.EnforceAdminInfo info = new EnforceAdminInfo();

                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.EnforceStampID = enforceStampID;
                    info.UserID = userID.ToNumber<int>();

                    dal.Add(info);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddUserList : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 
        /// 작성일자 : 
        /// 작성목적 : 담당자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUser(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);

                int enforceAdminID = GetDecryptParam("enforceAdminID", "0").ToNumber<int>();

                if (enforceAdminID.Equals(0)) return false;

                DAL.EnforceAdmin dal = new DAL.EnforceAdmin();
                return dal.Del(CurrentUserInfo.CompanyID, enforceAdminID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelUser : " + ex);
                throw;
            }
        }
        #endregion

    }
}
