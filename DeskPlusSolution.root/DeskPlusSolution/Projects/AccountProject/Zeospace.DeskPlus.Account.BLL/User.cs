using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.BLL
{
    public class User : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 쿠키

        /// <summary>
        /// 작성자 : 심재운
        /// db 에서 다음 쿠키객체를 문자열로 반환하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_passwordPure">The _password.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public string GetUserCookieStr(int _companyID, string _loginID, string _passwordPure, string _preLang)
        {
            Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
            //활성화 된 사용자만 가져올 수 있다.(deskplus 는 제외)
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth.UserInfo userInfo = user.GetUserCookie(_companyID, _loginID, _preLang, false);

            StringBuilder sb = new StringBuilder();

            /*
             * [CompanyID],[UserID],[LogInID],[Name],[EnglishName],
             * [ManyName],[EmployeeNumber],[MobilePhone],[MailID],[MailDomain],
             * [PrivateQuota],[SecurityLogIn],[DeptID],[CodeTitleID],[CodeDutyID],
             * [DeptName],[DeptManyName],[TitleName],[TitleManyName],[DutyName],
             * [DutyManyName],[PreferredLanguage],[DesignStyle],[BaseMenu],Authrity
             * UserDepts, Roles, GetRootDeptID, GetUpDeptIDInfo, CompanyUser
             */

            sb.Append(userInfo.CompanyID);
            sb.Append("┐");
            sb.Append(userInfo.UserID);
            sb.Append("┐");
            sb.Append(userInfo.LogInID);
            sb.Append("┐");
            sb.Append(userInfo.Name);
            sb.Append("┐");
            sb.Append(userInfo.EmployeeNumber);
            sb.Append("┐");
            sb.Append(userInfo.MobilePhone);
            sb.Append("┐");

            sb.Append(userInfo.MailID);
            sb.Append("┐");
            sb.Append(userInfo.MailDomain);
            sb.Append("┐");
            sb.Append(userInfo.PrivateQuota);
            sb.Append("┐");
            sb.Append(userInfo.SecurityLogIn);
            sb.Append("┐");
            sb.Append(userInfo.DeptID);
            sb.Append("┐");

            sb.Append(userInfo.CodeTitleID);
            sb.Append("┐");
            sb.Append(userInfo.CodeDutyID);
            sb.Append("┐");
            sb.Append(userInfo.DeptName);
            sb.Append("┐");
            sb.Append(userInfo.TitleName);
            sb.Append("┐");
            sb.Append(userInfo.DutyName);
            sb.Append("┐");

            sb.Append(userInfo.PreferredLanguage);
            sb.Append("┐");
            sb.Append(""); //스킨 미사용
            sb.Append("┐");
            sb.Append(userInfo.BaseMenu);
            sb.Append("┐");

            ////메뉴권한
            //if (userInfo.Authrity != null)
            //{
            //    foreach (string auth in userInfo.Authrity)
            //    {
            //        sb.Append(auth);
            //        sb.Append(",");
            //    }
            //}
            //else
            //{
            //    sb.Append("");
            //}

            //sb.Append("┐");

            //사용자 부서및 겸직
            if (userInfo.UserDepts != null)
            {
                foreach (string userDept in userInfo.UserDepts)
                {
                    sb.Append(userDept);
                    sb.Append(",");
                }
            }
            else
            {
                sb.Append("");
            }

            sb.Append("┐");

            //역할 코드값
            if (userInfo.Roles != null)
            {
                foreach (string roles in userInfo.Roles)
                {
                    sb.Append(roles);
                    sb.Append(",");
                }
            }
            else
            {
                sb.Append("");
            }

            sb.Append("┐");

            //역할 아이디값
            if (userInfo.RoleIDs != null)
            {
                foreach (string roles in userInfo.RoleIDs)
                {
                    sb.Append(roles);
                    sb.Append(",");
                }
            }
            else
            {
                sb.Append("");
            }

            sb.Append("┐");

            // RootDeptID 가져오기, 최상위 부서 정보 가져오기
            Account.BLL.Dept dept = new Account.BLL.Dept();
            sb.Append(dept.GetRootDeptID(_companyID, userInfo.DeptID, false));
            sb.Append("┐");

            //상위부서아이디값들 리스트
            sb.Append(dept.GetUpDeptIDInfo(_companyID, userInfo.DeptID, true));
            sb.Append("┐");

            //sysadmin 확인 여부
            sb.Append((userInfo.IsSysAdmin) ? "1" : "0");
            sb.Append("┐");
            //패스워드(순수 패스워드)
            sb.Append(_passwordPure);
            sb.Append("┐");
            //단방향 암호화 패스워드
            sb.Append(userInfo.Password);
            sb.Append("┐");
            //표준시간대
            sb.Append(userInfo.BaseTimeZone);
            sb.Append("┐");
            sb.Append(userInfo.BirthAddress1);
            return sb.ToString();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인별 스킨 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <returns></returns>
        public static string GetSkin(int _companyID, string _loginID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["DeskPlusEIPSkin"] == null)
            {
                DAL.User dal = new DAL.User();
                string skin = dal.GetSkin(_companyID, _loginID);
                skin = (skin == "") ? AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseSkin", "Blue") : skin;

                context.Session["DeskPlusEIPSkin"] = skin;
            }

            return context.Session["DeskPlusEIPSkin"].ToString();
        }

        /// <summary>
        /// 작성자 :
        /// 개인별 메일스킨 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <returns></returns>
        public static string GetSkinMail(int _companyID, string _loginID)
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["DeskPlusEIPSkinMail"] == null)
            {
                DAL.User dal = new DAL.User();
                string skin = dal.GetSkinMail(_companyID, _loginID);
                skin = (skin == "") ? AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseSkinMail", "blue") : skin;

                context.Session["DeskPlusEIPSkinMail"] = skin;
            }

            return context.Session["DeskPlusEIPSkinMail"].ToString();
        }

        #endregion 쿠키

        #region GET

        /// <summary>
        /// 리스트 가져오기
        /// </summary>
        /// <param name="dtr"></param>
        /// <returns></returns>
        public List<object> GetPageList(string _preLang, DataTableReader dtr)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                List<object> listobj = new List<object>();

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new BE.UserInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.LogInID = dtr["LogInID"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                            info.Mail = dtr["Mail"].ToString();
                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.ActiveUser = dtr["ActiveUser"].ToString();
                            info.BaseDept = dtr["BaseDept"].ToString();

                            /*
                            info.PhotoImage = baseFileDirectory + info.CompanyID + "/Users/" + info.UserID + "/Picture/photo.jpg";
                            if (!System.IO.File.Exists(context.Server.MapPath(info.PhotoImage)))
                            {
                                info.PhotoImage = baseImageDirectory + "Blue/Img/Icon/ico_noimg.gif";
                            }*/

                            #region 사진 이미지 설정

                            string photoName = dtr["PhotoImage"].ToString();
                            string imgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            bool response = false;

                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                            {
                                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                                imgPath = string.Format(imgPath, info.CompanyID, info.UserID);

                                if (System.IO.File.Exists(context.Server.MapPath(imgPath))) response = true;
                            }

                            if (response == false) imgPath = baseImageDirectory + "C/Img/Icon/ico_noimg.gif";

                            if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                            {
                                #region 스크린샷

                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", imgPath);
                                encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                                #endregion 스크린샷

                                imgTag = "<img src='{0}' width='52' height='52' />";
                                imgTag = string.Format(imgTag, imgPath);
                            }

                            info.PhotoImage = imgTag;

                            #endregion 사진 이미지 설정

                            info.Name = UtilHelper.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), _preLang);

                            info.DeptName = UtilHelper.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), _preLang);

                            info.TitleName = UtilHelper.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), _preLang);

                            info.DutyName = UtilHelper.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                                dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), _preLang);

                            info.UserDeptOrder = dtr["UserDeptOrder"].ToString().ToNumber<int>();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 해당 부서로 하위부서 가져와서 사용자 추출해 오기(구분자 콤마)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetChildDeptUserList(int _companyID, int _deptID, bool _activeDept)
        {
            BLL.Dept biz = new Dept();
            string deptList = biz.GetChildDeptList(_companyID, _deptID, _activeDept);
            deptList = (deptList.Length == 0) ? _deptID.ToString() : deptList;

            DAL.User dal = new DAL.User();
            List<int> al = dal.GetUserListByDept(_companyID, deptList, _activeDept);
            return String.Join(",", al.Select(x => x.ToString()).ToArray());
        }

        #endregion GET

        #region ADD()

        /// <summary>
        /// 사용자 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.UserInfo _userInfo, BE.UserSNSInfo _snsInfo)
        {
            if (_userInfo == null) return -1;

            int userID = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.User dalUser = new DAL.User();
                userID = dalUser.Add(_userInfo);

                if (userID > 0 && _snsInfo != null)
                {
                    DAL.UserSNS dalSNS = new DAL.UserSNS();
                    _snsInfo.UserID = userID;
                    dalSNS.Add(_snsInfo);
                }

                ts.Complete();
            }

            return userID;
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 사용자 수정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.UserInfo _userInfo, BE.UserSNSInfo _snsInfo)
        {
            if (_userInfo == null) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.User dalUser = new DAL.User();
                dalUser.Mod(_userInfo);

                if (_userInfo.UserID > 0 && _snsInfo != null)
                {
                    DAL.UserSNS dalSNS = new DAL.UserSNS();
                    dalSNS.Mod(_snsInfo);
                }

                ts.Complete();

                return true;
            }
        }

        #endregion MOD()
    }
}