using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Task.Web.Ws.Home
{
    /// <summary>
    /// Search의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Search : Base.BaseWSTask
    {
        #region GET()

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-15
        /// 작성목적 : 전체 협업 검색
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAllPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchValue = GetDecryptParam("searchValue", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List biz = new DAL.List();

                DataTableReader dtr = biz.GetAllSearchPageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1], searchValue);
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-", "+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();  
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();
                            info.Status = GetStatus(dtr["StatusLast"].ToString()); //1:대기;2:접수;3:처리중;4:완료;5:반려

                            #region 요청자 이름 & 부서 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 수신자 이름 & 부서 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);

                            string[] tmpToDept = dtr["TODeptName"].ToString().Split('|');
                            info.TODeptName = this.GetLangName(
                                    tmpToDept[0], tmpToDept[1], tmpToDept[2], tmpToDept[3],
                                    tmpToDept[4], tmpToDept[5], tmpToDept[6], PreLang);
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
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
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-15
        /// 작성목적 : 본인 협업 검색
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchValue = GetDecryptParam("searchValue", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List biz = new DAL.List();

                DataTableReader dtr = biz.GetSearchPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], searchValue);
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-", "+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();
                            info.Status = GetStatus(dtr["StatusLast"].ToString()); //1:대기;2:접수;3:처리중;4:완료;5:반려

                            #region 요청자 이름 & 부서 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 수신자 이름 & 부서 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);

                            string[] tmpToDept = dtr["TODeptName"].ToString().Split('|');
                            info.TODeptName = this.GetLangName(
                                    tmpToDept[0], tmpToDept[1], tmpToDept[2], tmpToDept[3],
                                    tmpToDept[4], tmpToDept[5], tmpToDept[6], PreLang);
                            #endregion

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
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
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-15
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            SetDecryptString(p);

            int groupID = GetDecryptParam("groupID", "0").ToNumber<Int32>();

            string response = "0";

            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachGroupList(CurrentUserInfo.CompanyID, groupID);

            string totalFileInfo = "";
            string encFilePath = "";

            foreach (var _attachInfo in attachInfo)
            {
                encFilePath = (_attachInfo.FileGUID != "") ? _attachInfo.FilePath + _attachInfo.FileGUID : _attachInfo.FilePath + _attachInfo.FileName;

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", encFilePath);
                paraDic.Add("fileName", _attachInfo.FileName);
                encFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                totalFileInfo += _attachInfo.FileName + ":" + encFilePath + "|"; // Key < 파일경로파일명
            }

            totalFileInfo = totalFileInfo.TrimEnd('|');

            if (totalFileInfo.Length > 0)
            {
                response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
            }

            return response;
        }
        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-15
        /// 작성목적 : 메뉴별 사용자 페이징 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModListUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string isType = GetDecryptParam("isType", "");
                string pageSize = GetDecryptParam("pageSize", "0");

                BE.ListUserInfo info = new BE.ListUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.IsType = isType;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();

                DAL.ListUser dal = new DAL.ListUser();
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
