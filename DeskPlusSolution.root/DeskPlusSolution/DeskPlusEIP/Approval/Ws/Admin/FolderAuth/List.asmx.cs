using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.FolderAuth
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        #region GET()

        /// <summary>
        /// 작성자 : 이문호
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
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
        /// 작성자   : shine
        /// 작성일자 : 2012-06-21
        /// 작성목적 : 타부서문서열람 리스트 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            string type = GetDecryptParam("type", "");

            try
            {
                DAL.FolderViewAuthority list = new DAL.FolderViewAuthority();
                return list.GetPageListCount(CurrentUserInfo.CompanyID, deptID, type);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetFavoriteListCnt : " + ex);
                return -1;
            }

        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-06-21
        /// 작성목적 : 수발자, 결재자 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                if (IsAdmin == false) return null;

                SetDecryptString(p);

                int targetDeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                string type = GetDecryptParam("type", "M");
                string pageSize = GetDecryptParam("pageSize", "0"); //페이지에 보여지는 목록수
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0"); //페이지 넘버


                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.FolderViewAuthority biz = new DAL.FolderViewAuthority();
                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, targetDeptID, type, setPageingStr[0], setPageingStr[1]);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FolderViewAuthorityInfo info = new FolderViewAuthorityInfo();

                            info.FolderViewAuthorityID = dtr["FolderViewAuthorityID"].ToString().ToNumber<int>();
                            info.TargetDeptID = dtr["TargetDeptID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.BaseDept = dtr["BaseDept"].ToString().ToNumber<int>();

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
        /// 작성목적 : 전체내역리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTotalList(string p)
        {
            try
            {
                if (IsAdmin == false) return null;

                SetDecryptString(p);
                string type = GetDecryptParam("type", "M");

                DAL.FolderViewAuthority biz = new DAL.FolderViewAuthority();
                DataTableReader dtr = biz.GetTotalList(CurrentUserInfo.CompanyID, type);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FolderViewAuthorityInfo info = new FolderViewAuthorityInfo();

                            info.ROW = dtr["ROW"].ToString().ToNumber<int>();

                            info.TargetPathNameBase = this.GetLangName(
                                dtr["TargetPathNameBase"].ToString().ToNull()
                                , dtr["TargetPathNameKo"].ToString().ToNull()
                                , dtr["TargetPathNameEn"].ToString().ToNull()
                                , dtr["TargetPathNameJa"].ToString().ToNull()
                                , dtr["TargetPathNameFr"].ToString().ToNull()
                                , dtr["TargetPathNameZhcn"].ToString().ToNull()
                                , dtr["TargetPathNameZhtw"].ToString().ToNull(), PreLang);

                            info.TargetDeptBase = this.GetLangName(
                                dtr["TargetDeptBase"].ToString().ToNull()
                                , dtr["TargetDeptKo"].ToString().ToNull()
                                , dtr["TargetDeptEn"].ToString().ToNull()
                                , dtr["TargetDeptJa"].ToString().ToNull()
                                , dtr["TargetDeptFr"].ToString().ToNull()
                                , dtr["TargetDeptZhcn"].ToString().ToNull()
                                , dtr["TargetDeptZhtw"].ToString().ToNull(), PreLang);

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull()
                                , dtr["NameKo"].ToString().ToNull()
                                , dtr["NameEn"].ToString().ToNull()
                                , dtr["NameJa"].ToString().ToNull()
                                , dtr["NameFr"].ToString().ToNull()
                                , dtr["NameZhcn"].ToString().ToNull()
                                , dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull()
                                , dtr["TitleKo"].ToString().ToNull()
                                , dtr["TitleEn"].ToString().ToNull()
                                , dtr["TitleJa"].ToString().ToNull()
                                , dtr["TitleFr"].ToString().ToNull()
                                , dtr["TitleZhcn"].ToString().ToNull()
                                , dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                            info.DutyBase = this.GetLangName(
                                dtr["DutyBase"].ToString().ToNull()
                                , dtr["DutyKo"].ToString().ToNull()
                                , dtr["DutyEn"].ToString().ToNull()
                                , dtr["DutyJa"].ToString().ToNull()
                                , dtr["DutyFr"].ToString().ToNull()
                                , dtr["DutyZhcn"].ToString().ToNull()
                                , dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                            info.DeptBase = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull()
                                , dtr["DeptKo"].ToString().ToNull()
                                , dtr["DeptEn"].ToString().ToNull()
                                , dtr["DeptJa"].ToString().ToNull()
                                , dtr["DeptFr"].ToString().ToNull()
                                , dtr["DeptZhcn"].ToString().ToNull()
                                , dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.TimeOfDay.ToString().Substring(0, 5);
                            else
                                info.WriteDateStr = string.Empty;       

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

        #endregion

        #region DEL()

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUserList(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                //파라미터 가져오기
                SetDecryptString(p);

                string authIDList = GetDecryptParam("authIDList", "0");

                DAL.FolderViewAuthority biz = new DAL.FolderViewAuthority();

                foreach (string authID in authIDList.Split(','))
                {
                    biz.Del(CurrentUserInfo.CompanyID, authID.ToNumber<int>());
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelUserList : " + ex);
                throw;
            }
        }
        #endregion

        #region ADD()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-06-21
        /// 작성목적 : 수발자,결재자 리스트에 사용자 추가
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
                string type = GetDecryptParam("type", "");
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();

                userList = userList.TrimEnd(',');

                DAL.FolderViewAuthority biz = new DAL.FolderViewAuthority();

                foreach (string userID in userList.Split(','))
                {
                    BE.FolderViewAuthorityInfo info = new FolderViewAuthorityInfo();

                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = userID.ToNumber<int>();
                    info.TargetDeptID = deptID;
                    info.Type = type;
                    biz.Add(info);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddUserList : " + ex);
                throw;
            }
        }

        #endregion
    }
}
