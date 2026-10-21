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

namespace Zeospace.DeskPlus.EIP.Web.Ws
{
    /// <summary>
    /// GetEIP의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetEIP : Base.BaseWSEIP
    {
        #region GET

        #region 페이징 함수

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        #endregion

        #region 관리자 메뉴
        /// <summary>
        /// 관리자 메인 메뉴 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminMenu()
        {
            List<object> listobj = new List<object>();

            DataRow[] drArr = GetDSAdminMenu().Tables[0].Select("ParentID = ''");

            try
            {
                foreach (DataRow dr in drArr)
                {
                    Zeospace.DeskPlus.EIP.BE.AdminMenuInfo info = new Zeospace.DeskPlus.EIP.BE.AdminMenuInfo();
                    info.ID = dr["ID"].ToString();
                    info.Text = dr["Text"].ToString();
                    info.ParentID = dr["ParentID"].ToString();
                    info.ClientSideCommand = dr["ClientSideCommand"].ToString();

                    if (info.ID == "COM") //시스템 관리자만 회사 관리를 할수 있다
                    {
                        if (CurrentUserInfo.IsSysAdmin == false) continue;
                    }

                    listobj.Add(info);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAdminSubMenu[_id] : " + ex);
            }

            return listobj;
        }
        

        /// <summary>
        ///  서브 메뉴 정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAdminSubMenu(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);
            string parentID = GetDecryptParam("ID", "");

            List<object> listobj = new List<object>();

            if (String.IsNullOrEmpty(parentID)) return listobj;
            
            
            DataRow[] drArr = GetDSAdminMenu().Tables[0].Select("ParentID = '" + parentID + "'");

            try
            {
                //서브메뉴 만들기
                foreach (DataRow dr in drArr)
                {
                    Zeospace.DeskPlus.EIP.BE.AdminMenuInfo info = new Zeospace.DeskPlus.EIP.BE.AdminMenuInfo();
                    info.ID = dr["ID"].ToString();
                    info.Text = dr["Text"].ToString();
                    info.ParentID = dr["ParentID"].ToString();
                    info.ClientSideCommand = dr["ClientSideCommand"].ToString();

                    listobj.Add(info);
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAdminSubMenu[_id] : " + ex);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-08-01 
        /// 작성목적 : 다국어에 맞게 XML 가져오기
        /// </summary>
        private DataSet GetDSAdminMenu()
        {
            DataSet dsAdminMenu = (DataSet)Session["dsAdminMenu"];

            if (dsAdminMenu == null)
            {
                dsAdminMenu = new DataSet();

                try
                {
                    string xmlPath = "";
                    try
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/EIP/App_Data/AdminMenu/" + PreLang + ".xml");
                    }
                    catch
                    {
                        xmlPath = Server.MapPath("/DeskPlusEIP/EIP/App_Data/AdminMenu/en.xml");
                    }
                    dsAdminMenu.ReadXml(xmlPath);
                    Session["dsAdminMenu"] = dsAdminMenu;
                }
                finally
                {
                }
            }

            return dsAdminMenu;
        }
        #endregion

        #region 라이센스관리
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetLicense()
        {
            List<object> listobj = new List<object>();

            Interop.ZeoLicEp.LicenseManager zeoLicMng = null;
            Interop.ZeoLicEp.License zeoLic = null;

            try
            {
                zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
                zeoLic = new Interop.ZeoLicEp.LicenseClass();

                string strLicense = zeoLicMng.GetAllLicenses();
                string[] arrLicense = strLicense.Split(';');

				if (arrLicense.Length > 0)
				{
					for (int i = 0; i < arrLicense.Length; i++) 
					{
                        Zeospace.DeskPlus.EIP.BE.LicenseInfo info = new Zeospace.DeskPlus.EIP.BE.LicenseInfo();
						
                        zeoLic.LicenseKey = arrLicense[i];

						int isValid = zeoLic.LicenseIsValid();
						if (isValid > 0)
						{

							info.LicenseNumber = arrLicense[i];
                            info.Description = GetProductCode(zeoLic.ProductCode.ToString());
							info.Count = zeoLic.Users.ToString();
                            info.Status = GetStatus(zeoLicMng.GetStatus(arrLicense[i]).ToString());
                            info.Expire = GetExpire(zeoLic.Expire);

                            listobj.Add(info);
						}
					}
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLicense[] : ", ex);
            }

            return listobj;
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 숫자 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLicenseCount()
        {
            int rtnVal = 0;

            Interop.ZeoLicEp.LicenseManager zeoLicMng = null;

            try
            {
                zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
                rtnVal = zeoLicMng.GetUsers(4);	// 라이센스 사용자(10명 보너스)  GetUsers(4) <- 4 가 DeskPlusEP 제품번호
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLicenseCount[] : " + ex.ToString());
            }

            return rtnVal;
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 상태 가져오기
        /// </summary>
        /// <param name="_status"></param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            string message = "";

            switch (_status)
            {
                case "0":
                    message = "비활성";
                    break;
                case "1":
                    message = "활성";
                    break;
                case "2":
                    message = "기간만료";
                    break;
                default:
                    message = "오류";
                    break;
            }

            return message;
        }

        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 만료정보 가져오기
        public string GetExpire(string _expire)
        {
            string message = "";

            switch (_expire)
            {
                case "never":
                    message = "제한 없음";
                    break;
                default:
                    message = _expire;
                    break;
            }

            return message;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 정보
        /// </summary>
        /// <returns></returns>
        public string GetProductCode(string _code)
        {
            string message = "";

            switch (_code)
            {
                case "1":
                    message = "데스크플러스 웹";
                    break;
                case "2":
                    message = "데스크플러스 메신저";
                    break;
                case "3":
                    message = "데스크플러스 모바일";
                    break;
                case "4":
                    message = "DeskPlusEP";
                    break;
                default:
                    message = "";
                    break;
            }

            return message;
        }

        #endregion

        #region COMMON 관련

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 회사 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCompanyPageList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string activeCompany = GetDecryptParam("activeCompany", "0");
            string totalCount = GetDecryptParam("totalCount", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "WriteDate asc");            

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<Zeospace.DeskPlus.Common.BE.CompanyInfo>();
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();

            DataTableReader dtr = dal.GetCompanyPageList(companyID,
                    (activeCompany == "1") ? true : false, setPageingStr[0], setPageingStr[1], order);
            
            try
            {
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Common.BE.CompanyInfo info = new Zeospace.DeskPlus.Common.BE.CompanyInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();                                                   

                            info.CompanyName = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.EstablishDate = dtr["EstablishDate"].ToString();
                            info.InstitutorName = dtr["InstitutorName"].ToString();
                            info.InstitutorEnglishName = dtr["InstitutorEnglishName"].ToString();
                            info.Phone = dtr["Phone"].ToString();
                            info.Fax = dtr["Fax"].ToString();
                            info.ActiveCompany = dtr["ActiveCompany"].ToString();
                            info.UserCnt = dtr["UserCnt"].ToString().ToNumber<int>();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCompanyList : " + ex);
                return null;
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
        /// 작성자 : 심재운
        /// sys code 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSysCodePageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string parentCodeID = GetDecryptParam("parentCodeID", "");                
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");                

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                DataTableReader dtr = biz.GetPageList(companyID.ToNumber<int>(),
                    parentCodeID, setPageingStr[0], setPageingStr[1], searchField, searchValue, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Common.BE.SYSCodeInfo info = new DeskPlus.Common.BE.SYSCodeInfo();
                            
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.SYSCodeID = dtr["SYSCodeID"].ToString().ToNumber<int>();
                            info.CodeID = dtr["CodeID"].ToString();
                            info.ParentCodeID = dtr["ParentCodeID"].ToString();
                            info.ActiveCode = dtr["ActiveCode"].ToString();
                            info.InterfaceCode = dtr["InterfaceCode"].ToString();

                            info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);
                            

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodePageList : " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// sys code 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSysCodeCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string parentCodeID = GetDecryptParam("parentCodeID", "");                
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                return biz.GetPageListCount(companyID.ToNumber<int>(), parentCodeID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodeCount : " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// codeid 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSysCodeCountByCodeID(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                string parentCodeID = GetDecryptParam("parentCodeID", "0");
                string codeID = GetDecryptParam("codeID", "");

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                return biz.GetCountByCodeID(companyID, parentCodeID, codeID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodeCountByCodeID : " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// 상위 codeid 로 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSysCodeCountByRootCodeID(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();                
                string codeID = GetDecryptParam("codeID", "");

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                return biz.GetCountByRootCodeID(companyID, codeID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodeCountByRootCodeID : " + ex.Message);
                return 0;
            }
        }
        

        #endregion

        #region EIP 관련

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 하위메뉴 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.MenuInfo> GetSubMenuList(string p)
        {
            SetDecryptString(p);            
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            List<BE.MenuInfo> al = new List<MenuInfo>();
            if (menuID == 0) return al;

            HttpContext context = HttpContext.Current;
            string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            
            BLL.Menu biz = new BLL.Menu();
            DataView dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            if (dv == null) return al;

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ParentMenuID"].ToString().ToNull().ToNumber<int>() == menuID)
                {
                    #region 다국어 이름 설정
                    string name = this.GetLangName(
                        dv[i]["NameBase"].ToString(),
                        dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang);
                    #endregion

                    BE.MenuInfo info = new MenuInfo();
                    info.CompanyID = dv[i]["CompanyID"].ToString().ToNumber<int>();
                    info.MenuID = dv[i]["MenuID"].ToString().ToNumber<int>();
                    info.MenuOrder = dv[i]["MenuOrder"].ToString().ToNumber<int>();
                    info.MenuName = name;
                    info.TargetURL = dv[i]["TargetURL"].ToString();
                    info.DirectCall = dv[i]["DirectCall"].ToString();

                    al.Add(info);
                }
            }

            return al;
        }

        /// <summary>
        /// 권한 코드 카운트
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRoleCountByRoleCode(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                string roleCode = GetDecryptParam("roleCode", "");
                string gubun = GetDecryptParam("gubun", "");

                Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                List<RoleInfo> al = dal.GetRole(companyID, roleCode, gubun);

                return al.Count();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleCountByRoleCode : " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : Role User 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRoleUserPageList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            string totalCount = GetDecryptParam("totalCount", "0");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');


            DAL.UserRole dal = new DAL.UserRole();
            DataTableReader dtr = dal.GetUserRolePageList(companyID, roleID, setPageingStr[0], setPageingStr[1], 
                searchField, searchValue, order);

            try
            {
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.EIP.BE.UserRoleInfo info = new UserRoleInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.RoleID = dtr["RoleID"].ToString().ToNumber<int>();                            
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Types = dtr["Types"].ToString();
                            info.RoleName = dtr["RoleName"].ToString();
                            info.RoleCode = dtr["RoleCode"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleUserPageList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// role user 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRoleUserPageCount(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();                
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.UserRole dal = new DAL.UserRole();
                return dal.GetUserRolePageCount(companyID, roleID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleUserPageCount : " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 데이터연동 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuDataLinkInfo> GetMenuDataLinkList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuDataLink dal = new DAL.MenuDataLink();
                List<BE.MenuDataLinkInfo> al = dal.GetDataLink(companyID, menuID);

                foreach (BE.MenuDataLinkInfo info in al)
                {
                    //암호화 (1 : md5, 2: ASE(Rindal, 3:기타입력)
                    if (info.EncriptType != "3") info.EncriptValue = "";
                    info.EncriptType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.EncriptType, "ENCRIPTION", CurrentUserInfo.PreferredLanguage);
                    if (info.EncriptType == "") info.EncriptType = "사용안함";
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuDataLinkList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 권한접근제어 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuRoleInfo> GetMenuRoleList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuRole dal = new DAL.MenuRole();
                return dal.GetMenuRole(companyID, menuID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuRoleList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : IP 접근제어 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuIPInfo> GetMenuIPList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuIP dal = new DAL.MenuIP();
                List<BE.MenuIPInfo> al = dal.GetMenuIP(companyID, menuID);

                foreach (MenuIPInfo info in al)
                {
                    if (info.StartIP.Trim().Length > 0)
                    {
                        info.StartIP1 = info.StartIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.StartIP2 = info.StartIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.StartIP3 = info.StartIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.StartIP4 = info.StartIP.Substring(9, 3).ToNumber<int>().ToString();
                    }

                    if (info.EndIP.Trim().Length > 0)
                    {
                        info.EndIP1 = info.EndIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.EndIP2 = info.EndIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.EndIP3 = info.EndIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.EndIP4 = info.EndIP.Substring(9, 3).ToNumber<int>().ToString();
                    }

                    if (info.IPAccess == "0")
                    {
                        info.IPAccess = "접속허용";
                    }
                    else if (info.IPAccess == "1")
                    {
                        info.IPAccess = "접속차단";
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuIPList : " + ex);
                return null;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : Portlet 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPortletPageList(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string totalCount = GetDecryptParam("totalCount", "0");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');


            DAL.Portlet dal = new DAL.Portlet();
            DataTableReader dtr = dal.GetPageList(companyID, categoryID, setPageingStr[0], setPageingStr[1],
                searchField, searchValue, order);

            try
            {
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.EIP.BE.PortletInfo info = new PortletInfo();

                            info.CatNameBase = this.GetLangName(
                                        dtr["CatNameBase"].ToString().ToNull(), dtr["CatNameKo"].ToString().ToNull(),
                                        dtr["CatNameEn"].ToString().ToNull(), dtr["CatNameJa"].ToString().ToNull(),
                                        dtr["CatNameFr"].ToString().ToNull(), dtr["CatNameZhcn"].ToString().ToNull(),
                                        dtr["CatNameZhtw"].ToString().ToNull(), PreLang);

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>(); ;
                            info.PortletID = dtr["PortletID"].ToString().ToNumber<int>(); ;
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>(); ;
                            info.NameBase = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
                            info.PortletType = dtr["PortletType"].ToString();
                            info.PortletTypeName = AppSettingHelper.GetSysCode(companyID, info.PortletType, "PortletType", CurrentUserInfo.PreferredLanguage);                            

                            //UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)                            
                            info.UIType = AppSettingHelper.GetSysCode(companyID, dtr["UIType"].ToString(), "UIType", CurrentUserInfo.PreferredLanguage);                            

                            //UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
                            info.UITemplate = AppSettingHelper.GetSysCode(companyID, dtr["UITemplate"].ToString(), "UIType", CurrentUserInfo.PreferredLanguage);
                            info.UIURL = dtr["UIURL"].ToString();

                            //Data 호출 형식 (R : RSS 호출 / S : SQL Storeprocedure Name)                            
                            info.DataType = AppSettingHelper.GetSysCode(companyID, dtr["DataType"].ToString(), "TargetDataType", CurrentUserInfo.PreferredLanguage);

                            info.DataURL = dtr["DataURL"].ToString();

                            //More Link 형식 (P : 팝업창 /  N : 새창 / C : 하단 프레임)
                            info.LinkType = AppSettingHelper.GetSysCode(companyID, dtr["LinkType"].ToString(), "LinkType", CurrentUserInfo.PreferredLanguage);                             

                            info.LinkPopupSize = dtr["LinkPopupSize"].ToString();
                            info.LinkURL = dtr["LinkURL"].ToString();
                            //개인화허용유무(1 : 허용 / 0 : 불가)
                            info.IsPesonalization = dtr["IsPesonalization"].ToString();
                            switch (info.IsPesonalization)
                            {
                                case "0":
                                    info.IsPesonalization = "아니오";
                                    break;
                                case "1":
                                    info.IsPesonalization = "예";
                                    break;
                            }

                            //활성화여부(1 : 허용 / 0 : 불가)
                            info.ActivePortlet = dtr["ActivePortlet"].ToString();
                            switch (info.ActivePortlet)
                            {
                                case "0":
                                    info.ActivePortlet = "아니오";
                                    break;
                                case "1":
                                    info.ActivePortlet = "예";
                                    break;
                            }

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleUserPageList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// Portlet 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPortletPageCount(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.Portlet dal = new DAL.Portlet();
                return dal.GetPageListCount(companyID, categoryID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletPageCount : " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 데이터연동 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<PortletDataLinkInfo> GetPortletDataLinkList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletDataLink dal = new DAL.PortletDataLink();
                List<BE.PortletDataLinkInfo> al = dal.GetDataLink(companyID, portletID);

                foreach (BE.PortletDataLinkInfo info in al)
                {
                    //암호화 (1 : md5, 2: ASE(Rindal, 3:기타입력)
                    if (info.EncriptType != "3") info.EncriptValue = "";
                    info.EncriptType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.EncriptType, "ENCRIPTION", CurrentUserInfo.PreferredLanguage);
                    if (info.EncriptType == "") info.EncriptType = "사용안함";


                    //파라미터 연결 대상( U : UI / D : Data / L : Link)
                    if (info.TargetType == "D")
                    {
                        info.TargetDataType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.TargetDataType, "TargetDataType", CurrentUserInfo.PreferredLanguage);
                    }
                    else
                    {
                        info.TargetDataType = "";
                    }

                    info.TargetType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.TargetType, "TargetType", CurrentUserInfo.PreferredLanguage);
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletDataLinkList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 권한접근제어 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<PortletRoleInfo> GetPortletRoleList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletRole dal = new DAL.PortletRole();
                return dal.GetPortletRole(companyID, portletID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletRoleList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 탭 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<PortletTabInfo> GetPortletTabList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletTab dal = new DAL.PortletTab();
                List<PortletTabInfo> al = dal.GetPortletTab(companyID, portletID);

                foreach(PortletTabInfo info in al)
                {
                    info.Name = this.GetLangName(
                        info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletTabList : " + ex);
                return null;
            }
        }


        #endregion

        #endregion

        #region 통계
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 공유자 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetStaticAttach(string p)
        {
            SetDecryptString(p);
            List<object> listobj = new List<object>();
            return listobj;
        }


        #endregion
    }
}


