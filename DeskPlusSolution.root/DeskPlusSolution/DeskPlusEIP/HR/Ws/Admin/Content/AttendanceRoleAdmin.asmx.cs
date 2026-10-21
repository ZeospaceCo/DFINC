using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.HR.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.HR.Web.Ws.Admin.Content
{
    /// <summary>
    /// AttendanceRoleAdmin의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AttendanceRoleAdmin : Base.BaseWSHR
    {
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 페이징
        /// </summary>
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

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 조회권한관리 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어

                int icnt = 0;

                DAL.AttendanceRole biz = new DAL.AttendanceRole();
                icnt = biz.GetStatusRoleListCount(CurrentUserInfo.CompanyID, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 조회권한관리 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			                //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Admin.Content.AttendanceRoleAdmin).Assembly;	    //어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string searchField = GetDecryptParam("searchField", "");        //검색필드
                string searchValue = GetDecryptParam("searchValue", "");        //검색어
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.AttendanceRole biz = new DAL.AttendanceRole();

                DataTableReader dtr = biz.GetStatusRoleList(CurrentUserInfo.CompanyID, searchField, searchValue, setPageingStr[0], setPageingStr[1]);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.HR.BE.RoleInfo info = new BE.RoleInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.RoleID = dtr["RoleID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Types = "GM";

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyName = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 적용부서 다국어 설정

                            string ViewDeptName = this.GetLangName(
                                    dtr["ViewDeptBase"].ToString().ToNull(), dtr["ViewDeptKo"].ToString().ToNull(),
                                    dtr["ViewDeptEn"].ToString().ToNull(), dtr["ViewDeptJa"].ToString().ToNull(),
                                    dtr["ViewDeptFr"].ToString().ToNull(), dtr["ViewDeptZhcn"].ToString().ToNull(),
                                    dtr["ViewDeptZhtw"].ToString().ToNull(), PreLang);

                            info.ViewDeptName = ViewDeptName.Replace("Low", resMng.GetString("List_SubDept"));
                            #endregion

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
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 권한부서 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRoleList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			                //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Admin.Content.AttendanceRoleAdmin).Assembly;	    //어셈블리 선언
            this.GetResourceManager();

            try
            {
                List<object> listobj = new List<object>();
                int _CompanyID = 0;
                int _UserID = 0;
                SetDecryptString(p);
                _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
                _UserID = GetDecryptParam("UserID", "").ToString().ToNumber<int>();

                DAL.AttendanceRole dal = new DAL.AttendanceRole();

                DataTableReader dtr = dal.GetRoleListByUserID(_CompanyID, _UserID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.RoleInfo _info = new RoleInfo();

                            string _Deptname = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang
                                );

                            _info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            _info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            _info.DeptName = _Deptname.Replace("Low", resMng.GetString("List_SubDept"));
                            listobj.Add(_info);
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

        #region ADD
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 권한부서추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddUserList(string p)
        {
            int adduser = 0;
            string userList = string.Empty;
            string isRowRank = string.Empty;

            SetDecryptString(p);

            adduser = GetDecryptParam("addUserID", "0").ToNumber<int>();
            userList = GetDecryptParam("userList", "");
            isRowRank = GetDecryptParam("isRowRank", "0");

            string[] arUser = userList.Split('|');

            DAL.AttendanceRole dal = new DAL.AttendanceRole();

            foreach (string _s in arUser)
            {
                string[] arVal = _s.Split('_');

                if (arVal.Length > 1)
                {
                    if (arVal[1].ToNumber<int>() > 0)
                    {
                        BE.RoleInfo info = new RoleInfo();

                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = adduser;
                        info.GroupID = arVal[1].ToNumber<int>();
                        info.IsRowRank = isRowRank;
                        info.WriteUserID = CurrentUserInfo.UserID;

                        dal.AddRole(info);
                    }
                }
            }
            return 1;
        }
        #endregion

        #region MOD()
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 권한부서삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelUserList(string p)
        {
            int CompanyID = 0;
            int UserID = 0;
            int GroupID = 0;
            SetDecryptString(p);

            DAL.AttendanceRole dal = new DAL.AttendanceRole();

            CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("GroupID", "0").ToNumber<int>();

            if (GroupID > 0)
                dal.DelRole(CompanyID, UserID, GroupID);

            return 1;
        }
        #endregion

        #region MOD()
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-29
        /// 작성목적 : 페이지 사이즈 저장
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "Admin");

                BE.FolderUserInfo info = new FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }
        #endregion
    }
}
