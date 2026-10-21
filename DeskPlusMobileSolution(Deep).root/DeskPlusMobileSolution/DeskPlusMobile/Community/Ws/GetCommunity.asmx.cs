using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws
{
    /// <summary>
    /// GetCommunity의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetCommunity : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 내가 가입한 커뮤니티 가지고 오기
        /// 작성일자 : 2012-07-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCommunityList()
        {
            try
            {
                List<object> listobj = new List<object>();
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_USERLIST");
                proc = string.Format(proc, CompanyID, UserID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetCommunityList", null);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.ComInfo info = new BE.ComInfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.CommunityID = dr["CommunityID"].ToString().ToNumber<int>();
                        info.Name = dr["Name"].ToString().ToReverseXSS();

                        listobj.Add(info);
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCommunityList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판종류 메뉴리스트 가지고 오기
        /// 작성일자 : 2012-07-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBoardMenu(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToString().ToNumber<int>();

                List<object> listobj = new List<object>();

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_BOARDMENULIST");
                proc = string.Format(proc, CompanyID, communityID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetBoardMenu", null);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.MenuInfo info = new BE.MenuInfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.CommunityID = dr["CommunityID"].ToString().ToNumber<int>();
                        info.MenuID = dr["MenuID"].ToString().ToNumber<int>();

                        info.Name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                        info.ReadAuth = dr["ReadAuth"].ToString().ToNumber<int>();

                        listobj.Add(info);
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBoardMenu", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 선택한 게시판 리스트 가져오기
        /// 작성일자 : 2012-07-10
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBoardList(string p)
        {
            SetDecryptString(p);

            int communityID = GetDecryptParam("CommunityID", "0").ToString().ToNumber<int>();
            int menuID = GetDecryptParam("MenuID", "0").ToString().ToNumber<int>();
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("SearchValue", "");

            List<object> listobj = new List<object>();

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SQL_PROC_BOA_LIST");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int),
			                                new SqlParameter("@CommunityID", SqlDbType.Int),
                                            new SqlParameter("@MenuID", SqlDbType.Int),
			                                new SqlParameter("@PageNo", SqlDbType.VarChar,10),
			                                new SqlParameter("@PageSize", SqlDbType.VarChar, 10),
                                            new SqlParameter("@SearchValue", SqlDbType.NVarChar, 100),
                                            new SqlParameter("@UserID", SqlDbType.Int)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = communityID;
                parm[2].Value = menuID;
                parm[3].Value = pageNo;
                parm[4].Value = pageSize;
                parm[5].Value = searchValue;
                parm[6].Value = UserID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetBoardList", parm);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.Boarnfo info = new BE.Boarnfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.CommunityID = dr["CommunityID"].ToString().ToNumber<int>();
                        info.MenuID = dr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dr["BoardID"].ToString().ToNumber<int>();
                        info.ReplyCount = (dr["ReplyCount"].ToString() == "0") ? "" : "[" + dr["ReplyCount"].ToString() + "]";
                        info.ViewCount = Convert.ToInt32(dr["ViewCount"]);
                        info.RowNum = dr["RowNum"].ToString();
                        info.Depth = dr["Depth"].ToString().ToNumber<int>();
                        info.Title = dr["Title"].ToString();
                        info.Title = GetTitle(info, info.Title);
                        info.Content = dr["Content"].ToString();

                        string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                 dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                        string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                        dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                        string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                        dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                        info.WriteUserInfo = deptName + " " + titleName + " " + name;

                        if (info.WriteUserInfo.Trim() == "")
                        {
                            info.WriteUserInfo = dr["WriteUserName"].ToString();
                        }

                        info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteDate = Convert.ToDateTime(dr["WriteDate"].ToString()).ToString();

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dr["WriteDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                        info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                        #endregion

                        listobj.Add(info);
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBoardList('" + communityID + "','" + menuID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        /// <summary>
        /// 게시판 제목 
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <returns></returns>
        private string GetTitle(BE.Boarnfo _info, string _title)
        {
            string title = _title;
            string arrowURl = "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

    }
}
