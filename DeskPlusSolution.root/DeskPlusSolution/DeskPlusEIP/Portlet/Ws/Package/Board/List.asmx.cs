using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Board
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPortlet
    {
        #region 변수

        private int CategoryID;
        private int PageSize;
        private int PageSizePageNo;
        private int TotalCount;

        private string OptionUseDept;//부서별게시판여부
        private string OptionUseView;//보기용 여부
        private string Auth; //관리자 : 1; 일반인 : 0

        #endregion 변수

        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBoardList(string p)
        {
            SetDecryptString(p);
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            PageSize = GetDecryptParam("pageSize", "1").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            //bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            //bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            //Auth = (isRoleAdmin || isRoleBoard) ? "1" : "0"; //관리자 : 1; 일반인 : 0

            Auth = "0"; //관리자 : 1; 일반인 : 0

            try
            {
                List<object> obj = new List<object>();
                Zeospace.DeskPlus.Board.DAL.Category dalCat = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = dalCat.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0) return obj;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OptionUseDept = dr["OptionUseDept"].ToString();
                        OptionUseView = dr["OptionUseView"].ToString();
                    }
                }

                List<Zeospace.DeskPlus.Board.BE.ListInfo> al = GetBoardPageList();
                foreach (Zeospace.DeskPlus.Board.BE.ListInfo info in al)
                {
                    obj.Add(info);
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetBoardList] : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 게시판 카운트 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetBoardListCount()
        {
            try
            {
                int icnt = 0;
                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능
                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    //관리자 일경우
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    "", "",
                    "", "", OptionUseDept, CurrentUserInfo.DeptID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 페이지 리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<Zeospace.DeskPlus.Board.BE.ListInfo> GetBoardPageList()
        {
            try
            {
                int totalCount = TotalCount;// GetBoardListCount();
                //int maxPageSize = totalCount / PageSize + 1;

                //if (PageSizePageNo > maxPageSize) PageSizePageNo = maxPageSize;
                //string pageSizePageNo = "2";

                string rtnSetPaging = SetPaging(totalCount, PageSize, PageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();

                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    //관리자 일경우
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    setPageingStr[0], setPageingStr[1],
                    "", "",
                    "", "", "",
                    OptionUseDept, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                List<Zeospace.DeskPlus.Board.BE.ListInfo> listobj = new List<Zeospace.DeskPlus.Board.BE.ListInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new ListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            //info.Content = dtr["Content"].ToString();
                            //info.MobileContent = dtr["MobileContent"].ToString();

                            //info.BlogContent = dtr["MobileContent"].ToString();

                            //if (info.BlogContent.Length > 200)
                            //    info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();
                            info.OptionModify = dtr["OptionModify"].ToString();
                            info.OptionTop = dtr["OptionTop"].ToString();
                            info.OptionTopDay = dtr["OptionTopDay"].ToString().ToNumber<int>();

                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            //info.DeptName = dtr["DeptName"].ToString();
                            //info.TitleName = dtr["TitleName"].ToString();
                            //info.DutyName = dtr["DutyName"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.ImgCount = dtr["ImgCount"].ToString().ToNumber<int>();
                            info.HeaderID = dtr["HeaderID"].ToString().ToNumber<int>();
                            info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>();

                            #region 말머리

                            string headerName = this.GetLangName(
                                    dtr["HeaderBase"].ToString().ToNull(), dtr["HeaderKo"].ToString().ToNull(),
                                    dtr["HeaderEn"].ToString().ToNull(), dtr["HeaderJa"].ToString().ToNull(),
                                    dtr["HeaderFr"].ToString().ToNull(), dtr["HeaderZhcn"].ToString().ToNull(),
                                    dtr["HeaderZhtw"].ToString().ToNull(), PreLang);

                            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            #endregion 말머리

                            if (dtr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                                info.WriteUserName = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                                info.WriteUserID = 0;
                            }
                            else
                            {
                                #region 이름 다국어 설정

                                info.WriteUserName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

                                #region 부서 다국어 설정

                                info.DeptName = this.GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                                #endregion 부서 다국어 설정

                                #region 직급 다국어 설정

                                info.TitleName = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직급 다국어 설정

                                #region 직책 다국어 설정

                                info.DutyName = this.GetLangName(
                                        dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                        dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                        dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                        dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직책 다국어 설정

                                #region 다국어 경로 다국어 설정

                                info.PathName = this.GetLangName(
                                    dtr["PathNameKo"].ToString(),
                                    dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                    dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);

                                #endregion 다국어 경로 다국어 설정

                                info.WriteUserName = GetWriteUserName(
                                    info.WriteUserName,
                                    info.DeptName,
                                    info.TitleName,
                                    info.DutyName);
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            info.ModifyUserID = dtr["ModifyUserID"].ToString().ToNumber<int>();
                            info.ModifyUserName = dtr["ModifyUserName"].ToString();

                            if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                                info.ModifyDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ModifyDate = string.Empty;

                            info.PassWord = dtr["PassWord"].ToString();
                            int listNum = (totalCount - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.ROW = (dtr["OptionTop"].ToString() == "1") ? "<img src='" + this.BaseImgPath + "/Lang/" + this.PreLang + "/not.gif' alt=''>" : listNum.ToString();

                            #region 다국어 카테고리 이름 설정

                            info.CA_Title = this.GetLangName(
                                dtr["CNameBase"].ToString(),
                                dtr["CNameKo"].ToString(), dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(),
                                dtr["CNameFr"].ToString(), dtr["CNameZhCn"].ToString(), dtr["CNameZhTw"].ToString(), PreLang);

                            #endregion 다국어 카테고리 이름 설정

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();

                            info.Title = dtr["Title"].ToString();//리스트용

                            info.TitleNoHtml = info.Title; //툴팁용
                            info.TitleShort = info.Title;//갤러리용

                            if (info.TitleShort.Length > 15)
                                info.TitleShort = info.TitleShort.Substring(0, 12) + "..";

                            info.TitleShort = GetTitle(info, info.TitleShort, dtr["Etc2"].ToString());
                            info.Title = GetTitle(info, info.Title, dtr["Etc2"].ToString());

                            //조회하지 않으면 굵게 처리
                            if (dtr["ReadCheck"].ToString().ToNumber<int>() == 0) info.Title = "<b>" + info.Title + "</b>";

                            //말머리
                            info.Title = (info.HeaderName.Length > 0) ?
                                info.HeaderName + " " + info.Title : info.Title;

                            info.ScreenShot = dtr["ScreenShot"].ToString();
                            if (info.ScreenShot.ToNull().Length == 0)
                            {
                                info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                            }

                            #region 날짜 비교(New Img)

                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";

                            #endregion 날짜 비교(New Img)

                            Dictionary<string, string> paraDic1 = new Dictionary<string, string>();
                            paraDic1.Add("catID", dtr["CategoryID"].ToString());
                            paraDic1.Add("categoryID", dtr["CategoryID"].ToString());
                            paraDic1.Add("listID", dtr["ListID"].ToString());
                            info.HrefUrl = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptParam(Utility.GetParameterDic(paraDic1));

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 게시글 최다 등록자
        /// 작성자 : 심재운
        /// 수정 : 2015.01.20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<GetBoardTopRegisterEntity> GetBoardTopRegister()
        {
            try
            {
                //CODE에 등록되어진 게시판 카테고리값 가져오기
                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TOP_LIST");
                string categoryID = "";

                for (int i = 0; i < dv.Count; i++) categoryID += dv[i]["CodeID"].ToString() + ",";

                Portlet.DAL.Board dal = new DAL.Board();
                DataTableReader dtr = dal.GetBoardTopRegister(CurrentUserInfo.CompanyID, categoryID, 5);

                List<GetBoardTopRegisterEntity> listobj = new List<GetBoardTopRegisterEntity>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        int row = 1;
                        while (dtr.Read() && row < 6)
                        {
                            GetBoardTopRegisterEntity info = new GetBoardTopRegisterEntity();
                            info.Row = dtr["Row"].ToString().ToNumber<int>();
                            //info.Row = row;
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.Cnt = dtr["Cnt"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정

                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            #endregion 이름 다국어 설정

                            #region 부서 다국어 설정

                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                            #endregion 부서 다국어 설정

                            #region 직급 다국어 설정

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                            #endregion 직급 다국어 설정

                            #region 직책 다국어 설정

                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                            #endregion 직책 다국어 설정

                            listobj.Add(info);

                            row++;
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 게시글 최다 조회자
        /// 작성자 : 심재운
        /// 수정 : 2015.01.20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<GetBoardTopReaderEntity> GetBoardTopReader()
        {
            try
            {
                //CODE에 등록되어진 게시판 카테고리값 가져오기
                DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TOP_LIST");
                string categoryID = "";

                for (int i = 0; i < dv.Count; i++) categoryID += dv[i]["CodeID"].ToString() + ",";

                Portlet.DAL.Board dal = new DAL.Board();
                DataTableReader dtr = dal.GetBoardTopReader(CurrentUserInfo.CompanyID, categoryID, 5);

                List<GetBoardTopReaderEntity> listobj = new List<GetBoardTopReaderEntity>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        int row = 1;
                        while (dtr.Read() && row < 6)  //중복데이타의 경우를 고려하여, 강제적으로 4개까지 처리
                        {
                            GetBoardTopReaderEntity info = new GetBoardTopReaderEntity();

                            //info.Row = row;
                            info.Row = dtr["Row"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();

                            if (dtr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                                info.NameBase = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                                info.WriteUserID = 0;
                            }
                            else
                            {
                                info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                                #region 이름 다국어 설정

                                info.NameBase = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

                                #region 부서 다국어 설정

                                info.DeptBase = this.GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                                #endregion 부서 다국어 설정

                                #region 직급 다국어 설정

                                info.TitleBase = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직급 다국어 설정

                                #region 직책 다국어 설정

                                info.DutyBase = this.GetLangName(
                                        dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                        dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                        dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                        dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직책 다국어 설정
                            }

                            Dictionary<string, string> paraDic1 = new Dictionary<string, string>();
                            paraDic1.Add("catID", dtr["CategoryID"].ToString());
                            paraDic1.Add("categoryID", dtr["CategoryID"].ToString());
                            paraDic1.Add("listID", dtr["ListID"].ToString());
                            info.HrefUrl = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptParam(Utility.GetParameterDic(paraDic1));

                            listobj.Add(info);

                            row++;
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_dutyName">Name of the _duty.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName, string _deptName,
            string _titleName, string _dutyName)
        {
            string nameFieldFormat = _writeUserName;
            string applyCategory = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameFieldCategory", "0");
            nameFieldFormat = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameField", "WriteName");
            nameFieldFormat = (nameFieldFormat.IndexOf("WriteName") != -1) ? nameFieldFormat.Replace("WriteName", _writeUserName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DeptName") != -1) ? nameFieldFormat.Replace("DeptName", _deptName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("TitleName") != -1) ? nameFieldFormat.Replace("TitleName", _titleName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DutyName") != -1) ? nameFieldFormat.Replace("DutyName", _dutyName) : nameFieldFormat;

            //이름 보이기 (이름:WriteName, 부서:Dept, 직급:Title, 직책:Duty)
            return nameFieldFormat;
        }

        /// <summary>
        /// 게시판 제목 스타일 입히기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string GetTitle(Zeospace.DeskPlus.Board.BE.ListInfo _info, string _title, string _etc2)
        {
            string title = SetTitleOptionEtc(_info.Title, _etc2);
            string arrowURl = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //공지글일 경우
            if (_info.OptionTop.ToNumber<int>() > 0) title = "<b>" + title + "</b>";
            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 옵션설정에 맞게 옵션 스타일 적용하기
        /// </summary>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string SetTitleOptionEtc(string _title, string _etc2)
        {
            try
            {
                if (_etc2.ToNull().Length > 0)
                {
                    string[] etcStr = _etc2.Split(',');
                    StringBuilder sb = new StringBuilder();
                    if (etcStr[2].ToString() != "0") sb.Append("<b>");
                    if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                    if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                    sb.Append(_title);
                    if (etcStr[0].ToString() != "") sb.Append("</font>");
                    if (etcStr[1].ToString() != "") sb.Append("</font>");
                    if (etcStr[2].ToString() != "0") sb.Append("</b>");

                    _title = sb.ToString();

                    return _title;
                }

                return _title;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한별 카테고리 리스트 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_useChild">if set to <c>true</c> [_use child].</param>
        /// <returns></returns>
        private string GetMyAuthCatList(int _categoryID, bool _useChild)
        {
            try
            {
                #region 내가 볼 수 있는 하위 카테고리

                NameValueCollection nvc = new NameValueCollection();
                string myAuthCatList = "";// _categoryID.ToString() + ",";
                Zeospace.DeskPlus.Board.BLL.Category biz = new Zeospace.DeskPlus.Board.BLL.Category();

                //보기용은 하위카테고리까지 가져옴
                if (OptionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, true, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, _useChild, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }

                myAuthCatList = myAuthCatList.TrimEnd(',');

                #endregion 내가 볼 수 있는 하위 카테고리

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

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
        /// 전체 게시판 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Zeospace.DeskPlus.Board.BE.ListInfo> GetBoardTotList(string p)
        {
            SetDecryptString(p);

            PageSize = GetDecryptParam("pageSize", "1").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            try
            {
                List<Zeospace.DeskPlus.Board.BE.ListInfo> listobj = new List<Zeospace.DeskPlus.Board.BE.ListInfo>();

                Zeospace.DeskPlus.Portlet.DAL.Board biz = new DAL.Board();
                DataTableReader dtr = biz.GetBoardTotList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, TotalCount, PageSize, PageSizePageNo);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new ListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();
                            info.OptionModify = dtr["OptionModify"].ToString();
                            info.OptionTop = dtr["OptionTop"].ToString();
                            info.OptionTopDay = dtr["OptionTopDay"].ToString().ToNumber<int>();

                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            #region 말머리

                            string headerName = this.GetLangName(
                                    dtr["HeaderBase"].ToString().ToNull(), dtr["HeaderKo"].ToString().ToNull(),
                                    dtr["HeaderEn"].ToString().ToNull(), dtr["HeaderJa"].ToString().ToNull(),
                                    dtr["HeaderFr"].ToString().ToNull(), dtr["HeaderZhcn"].ToString().ToNull(),
                                    dtr["HeaderZhtw"].ToString().ToNull(), PreLang);

                            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            #endregion 말머리

                            if (dtr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                                info.WriteUserName = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                                info.WriteUserID = 0;
                            }
                            else
                            {
                                #region 이름 다국어 설정

                                info.WriteUserName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

                                #region 부서 다국어 설정

                                info.DeptName = this.GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                                #endregion 부서 다국어 설정

                                #region 직급 다국어 설정

                                info.TitleName = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직급 다국어 설정

                                #region 직책 다국어 설정

                                info.DutyName = this.GetLangName(
                                        dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                        dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                        dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                        dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직책 다국어 설정

                                info.WriteUserName = GetWriteUserName(
                                    info.WriteUserName,
                                    info.DeptName,
                                    info.TitleName,
                                    info.DutyName);
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();//리스트용

                            info.TitleNoHtml = info.Title; //툴팁용
                            info.TitleShort = info.Title;//갤러리용

                            if (info.TitleShort.Length > 15)
                                info.TitleShort = info.TitleShort.Substring(0, 12) + "..";

                            info.TitleShort = GetTitle(info, info.TitleShort, dtr["Etc2"].ToString());
                            info.Title = GetTitle(info, info.Title, dtr["Etc2"].ToString());

                            //조회하지 않으면 굵게 처리
                            if (dtr["ReadCheck"].ToString().ToNumber<int>() == 0) info.Title = "<b>" + info.Title + "</b>";

                            //말머리
                            info.Title = (info.HeaderName.Length > 0) ? info.HeaderName + " " + info.Title : info.Title;

                            #region 날짜 비교(New Img)

                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";

                            #endregion 날짜 비교(New Img)

                            Dictionary<string, string> paraDic1 = new Dictionary<string, string>();
                            paraDic1.Add("catID", dtr["CategoryID"].ToString());
                            paraDic1.Add("categoryID", dtr["CategoryID"].ToString());
                            paraDic1.Add("listID", dtr["ListID"].ToString());
                            info.HrefUrl = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptParam(Utility.GetParameterDic(paraDic1));

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetBoardTotList] : " + ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class GetBoardTopRegisterEntity : Common.BE.DeptCodeInfo
    {
        public int Row { get; set; }

        public int CompanyID { get; set; }

        public int WriteUserID { get; set; }

        public int Cnt { get; set; }
    }

    [Serializable]
    public class GetBoardTopReaderEntity : Common.BE.DeptCodeInfo
    {
        public int Row { get; set; }

        public int CompanyID { get; set; }

        public string Title { get; set; }

        public int WriteUserID { get; set; }

        public int ReadCount { get; set; }

        public string HrefUrl { get; set; }
    }
}