using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;
using System.IO;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Subsidiary
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSEIP
    {

        /// <summary>
        /// 회사 관리자 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSubsideUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string userID = GetDecryptParam("userID", "");
            string roleCode = GetDecryptParam("roleCode", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (userID.Length > 0)
                    {
                        Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();

                        if (userID.IndexOf(',') != -1)
                        {
                            foreach (string user in userID.Split(','))
                            {
                                BE.UserRoleInfo info = new BE.UserRoleInfo();
                                info.CompanyID = companyID;
                                info.RoleCode = roleCode;
                                info.UserID = user.ToNumber<int>();
                                info.Types = "GM"; //사용자
                                dal.Add(info);
                            }
                        }
                        else
                        {
                            BE.UserRoleInfo info = new BE.UserRoleInfo();
                            info.CompanyID = companyID;
                            info.RoleCode = roleCode;
                            info.UserID = userID.ToNumber<int>();
                            info.Types = "GM"; //사용자
                            dal.Add(info);
                        }
                    }

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddSubsideUser : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 회사 관리자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSubsideUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string userID = GetDecryptParam("userID", "");
            string roleCode = GetDecryptParam("roleCode", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (userID.Length > 0)
                    {
                        Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();

                        if (userID.IndexOf(',') != -1)
                        {
                            foreach (string user in userID.Split(','))
                            {
                                BE.UserRoleInfo info = new BE.UserRoleInfo();
                                info.CompanyID = companyID;
                                info.RoleCode = roleCode;
                                info.UserID = user.ToNumber<int>();
                                dal.Del(info);
                            }
                        }
                        else
                        {
                            BE.UserRoleInfo info = new BE.UserRoleInfo();
                            info.CompanyID = companyID;
                            info.RoleCode = roleCode;
                            info.UserID = userID.ToNumber<int>();
                            dal.Del(info);
                        }
                    }

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSubsideUser : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 계열사 관리자 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSubsideUserList(string p)
        {
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();

            try
            {
                DAL.UserRole dal = new DAL.UserRole();
                DataTableReader dtr = dal.GetSubsideUserList(companyID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PageList.SubsideUserPageListInfo info = new BE.PageList.SubsideUserPageListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.RoleID = dtr["RoleID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.RoleCode = dtr["RoleCode"].ToString();

                            info.ComBase = this.GetLangName(
                                dtr["ComBase"].ToString(),
                                dtr["ComKo"].ToString(), dtr["ComEn"].ToString(), dtr["ComJa"].ToString(),
                                dtr["ComFr"].ToString(), dtr["ComZhCn"].ToString(), dtr["ComZhTw"].ToString(), PreLang);

                            info.LogInID = dtr["LogInID"].ToString();

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.DeptBase = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                            info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                            info.Mail = dtr["Mail"].ToString();
                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.OfficePhone = dtr["OfficePhone"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSubsideUserList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 임시폴더의 파일 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string MoveImage(string p)
        {
            SetDecryptString(p);
            string uploadGUID = GetDecryptParam("UploadGUID", "");
            string attachGubun = GetDecryptParam("AttachGubun", "");
            string companyID = GetDecryptParam("CompanyID", "0");
            if (String.IsNullOrEmpty(attachGubun) || String.IsNullOrEmpty(uploadGUID)) return "";

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseCssDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");

            string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
            string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, uploadGUID);

            string gubun = "";
            string rtnVal = "";

            try
            {
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));
                if (di.Exists)
                {
                    FileInfo[] fileList = di.GetFiles();
                    string fileName = "";
                    foreach (FileInfo f in fileList)
                    {
                        fileName = f.Name;
                    }

                    string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                    souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, uploadGUID, fileName);

                    string destFile = baseFileDirectory + companyID + "/";

                    if (attachGubun == "login")
                    {
                        destFile += "lgn_logo.png";
                        gubun = "1";
                    }
                    else if(attachGubun == "Vlogin")
                    {
                        destFile += "Vlgn_logo.png";
                        gubun = "3";
                    }
                    else if(attachGubun == "menu")
                    {
                        destFile += "logo.png";
                        gubun = "3";
                    }
                    else if (attachGubun == "BGlogin")
                    {
                        destFile += "bg.jpg";
                        gubun = "5";
                    }
                    else if (attachGubun == "BGPotal")
                    {
                        destFile += "mainback.jpg";
                        gubun = "6";
                    }
                    else
                    {
                        destFile += "Alogo.gif";
                        gubun = "4";
                    }

                    if (File.Exists(Server.MapPath(destFile)))
                    {
                        System.IO.File.Delete(Server.MapPath(destFile));
                    }

                    System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile));

                    Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
                    dal.Mod(companyID.ToNumber<int>(), gubun);

                    return destFile;

                    /*
                    if ((attachGubun == "BGlogin") || (attachGubun == "BGPotal"))
                    {
                        return destFile;
                    }
                    else
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", destFile);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        return "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                    }
                    */
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveImage : " + ex);
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : freelman
        /// 용도 : 이미지 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelImage(string p)
        {
            try
            {
                SetDecryptString(p);

                string companyID = GetDecryptParam("CompanyID", "");
                string attachGubun = GetDecryptParam("AttachGubun", "");

                if (String.IsNullOrEmpty(attachGubun) || String.IsNullOrEmpty(companyID)) return false;

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string destFile = baseFileDirectory + companyID + "/";

                if (attachGubun == "login")
                {
                    destFile += "lgn_logo.png";
                }
                else if (attachGubun == "Vlogin")
                {
                    destFile += "Vlgn_logo.png";
                }
                else if (attachGubun == "menu")
                {
                    destFile += "logo.png";
                }
                else if (attachGubun == "BGlogin")
                {
                    destFile += "bg.jpg";
                }
                else if (attachGubun == "BGPotal")
                {
                    destFile += "mainback.jpg";
                }
                else
                {
                    destFile += "Alogo.gif";
                }

                if (File.Exists(Server.MapPath(destFile)))
                {
                    System.IO.File.Delete(Server.MapPath(destFile));
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelImage : " + ex.Message);
                throw;
            }
        }
    }
}
