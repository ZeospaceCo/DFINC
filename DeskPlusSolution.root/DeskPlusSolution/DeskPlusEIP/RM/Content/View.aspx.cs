using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class View : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수처리

        public int CategoryID;
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string TmpDate = String.Empty;
        public string TagGubun = String.Empty;
        public string GroupGubun = String.Empty;
        public int ReservationID;
        public int DeptPageNo = 0;
        public int MainPageNo = 0;
        public string Usable = "";
        public string CntYN = "N";
        public int RmCnt = 0;

        #endregion 변수처리

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.View).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetData();

            if (CntYN == "Y")
            {
                GetRMCnt();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 예약 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.Reverse dal = new DAL.Reverse();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, ReservationID);

            int listID = 0;
            int writeUserID = 0;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbSubject.Text = dtr["Subject"].ToString();
                        //lbDate.Text = dtr["StartDate"].ToString() + " ~ " + dtr["EndDate"].ToString();
                        lbDate.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["StartDate"].ToString()).ToShortTimeString() + " ~ " +
                             DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["EndDate"].ToString()).ToShortTimeString();

                        /*
                        if (String.IsNullOrEmpty(dtr["UserImg"].ToString().ToNull()))
                        {
                            RblPattern.SelectedValue = "FF0000";
                        }
                        else
                        {
                            RblPattern.SelectedValue = dtr["UserImg"].ToString();
                        }
                        */

                        if (dtr["UserImg"].ToString() == "FF0000")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c1.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "FFC600")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c2.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "FFFF00")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c3.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "94D652")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c4.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "00B552")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c5.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "00B5F7")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c6.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "0073C6")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c7.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "002163")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c8.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "7331A5")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c9.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "943131")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c10.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "5A4A7B")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c11.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "E76B09")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c12.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "32849C")
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c13.gif' alt='' />";
                        }
                        else
                        {
                            Pattern.InnerHtml = "<img src='" + BaseImgPath + "/RM/c1.gif' alt='' />";
                        }


                        lbWriteName.Text = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        lbModifyDate.Text = DateTime.Parse(dtr["ModifyDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ModifyDate"].ToString()).ToShortTimeString();

                        lbContent.Text = dtr["Content"].ToString().ToEditorXSS();
                        //lbTitle.Text = dtr["Title"].ToString();
                        //lbQty.Text = dtr["Qty"].ToString();
                        lbGuard.Text = (dtr["IsGuard"].ToString() == "0") ? this.resMng.GetString("Normal") : this.resMng.GetString("Security");


                        //자원 담당자 이름
                        string[] arrManyName = dtr["MManyName"].ToString().Split('|');
                        string tmpManagerName = "";
                        if (arrManyName.Length == 7)
                        {
                            tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                   arrManyName[2], arrManyName[3], arrManyName[4],
                                                                   arrManyName[5], arrManyName[6], PreLang);
                        }

                        //lbManagerName.Text = tmpManagerName;

                        spTitle.InnerText = dtr["Title"].ToString().ToReverseXSS() + " (" + dtr["Qty"].ToString() + ") " + tmpManagerName;

                        ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        WriteYM = dtWriteDate.Year.ToString() + dtWriteDate.Month.ToString().PadLeft(2, '0');
                        listID = dtr["ListID"].ToString().ToNumber<int>();
                        writeUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        Usable = dtr["Usable"].ToString();
                        if (Usable == "0")
                        {
                            lbUsable.Text = this.resMng.GetString("Usable0");
                        }
                        else if (Usable == "1")
                        {
                            lbUsable.Text = this.resMng.GetString("Usable1");
                        }
                        else
                        {
                            lbUsable.Text = this.resMng.GetString("Usable2");
                        }

                        #region 종료버튼 처리

                        int DifferMinite = dtr["DifferMinite"].ToString().ToNumber<int>();

                        DateTime dtStart = Convert.ToDateTime(dtr["StartDate"].ToString());
                        DateTime dtEnd = Convert.ToDateTime(dtr["EndDate"].ToString());

                        DateTime dt = new DateTime();
                        dt = DateTime.Now;

                        TimeSpan StartGap = dtStart - dt;
                        double sDifferMinite = StartGap.TotalMinutes;

                        TimeSpan EndGap = dtEnd - dt;
                        double eDifferMinite = EndGap.TotalMinutes;

                        if (sDifferMinite < 0 && eDifferMinite > 0)
                        {
                            cbtnEnd.Visible = true;
                        }
                        else
                        {
                            cbtnEnd.Visible = false;
                        }

                        #endregion
                    }
                }
            }

            GetRepeat(ParentReservationID);
            GetAuth(listID, writeUserID);
        }

        /// <summary>
        /// 작성목적 : 반복 설정 가져오기
        /// </summary>
        private void GetRepeat(int _parentReservationID)
        {
            DAL.Repeat dal = new DAL.Repeat();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, ParentReservationID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.RepeatInfo info = new BE.RepeatInfo();
                        info.CompanyID = dtr["RepeatType"].ToString().ToNumber<int>();
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.RepeatType = dtr["RepeatType"].ToString();
                        info.SelectType = dtr["SelectType"].ToString();
                        info.IntervalCnt = dtr["IntervalCnt"].ToString().ToNumber<int>();
                        info.Option1 = dtr["Option1"].ToString();
                        info.Option2 = dtr["Option2"].ToString();
                        info.Option3 = dtr["Option3"].ToString();
                        info.Option4 = dtr["Option4"].ToString();
                        info.Option5 = dtr["Option5"].ToString();
                        info.Option6 = dtr["Option6"].ToString();
                        info.Option7 = dtr["Option7"].ToString();
                        info.MaxReptCnt = dtr["MaxReptCnt"].ToString().ToNumber<int>();
                        info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());

                        switch (info.RepeatType)
                        {
                            case "D":
                                GetDay(info);
                                break;

                            case "W":
                                GetWeek(info);
                                break;

                            case "M":
                                GetMonth(info);
                                break;
                        }
                    }

                    hdRepeatChk.Text = "Y";
                }
            }
        }

        /// <summary>
        /// 일별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetDay(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;
            if (_info.SelectType == "1")
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat17");
            }
            else
            {
                strVal += resMng.GetString("Repeat3");
            }

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            lbRepeat.Text = strVal;
        }

        /// <summary>
        /// 주별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetWeek(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;

            strVal += _info.IntervalCnt + resMng.GetString("Repeat18") + " ";

            if (_info.Option1 == "1")
                strVal += resMng.GetString("Sun") + " ";
            if (_info.Option2 == "1")
                strVal += resMng.GetString("Mon") + " ";
            if (_info.Option3 == "1")
                strVal += resMng.GetString("Tue") + " ";
            if (_info.Option4 == "1")
                strVal += resMng.GetString("Wed") + " ";
            if (_info.Option5 == "1")
                strVal += resMng.GetString("Thu") + " ";
            if (_info.Option6 == "1")
                strVal += resMng.GetString("Fri") + " ";
            if (_info.Option7 == "1")
                strVal += resMng.GetString("Sat") + " ";

            strVal += resMng.GetString("Repeat19");

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            lbRepeat.Text = strVal;
        }

        /// <summary>
        /// 월별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetMonth(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;
            if (_info.SelectType == "1")
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat5") + " " + _info.Option1 + resMng.GetString("Repeat20");
            }
            else
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat5") + " " + _info.Option1 + _info.Option2 + resMng.GetString("Repeat21");
            }

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            lbRepeat.Text = strVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 삭제 버튼 권한 가져오기
        /// </summary>
        private void GetAuth(int _listID, int _writeUserID)
        {
            // 관리자 권한
            /*
            if (AccessAuth)
            {
                cbtnDel.Visible = true;
                cbtnMod.Visible = true;
            }
            else
            {
            */
            if (_writeUserID == CurrentUserInfo.UserID)
            {
                cbtnDel.Visible = true;
                cbtnMod.Visible = true;
            }
            else
            {
                cbtnDel.Visible = false;
                cbtnMod.Visible = false;
                DAL.ListUser dal = new DAL.ListUser();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, _listID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            if (dtr["ManagerUserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                            {
                                cbtnDel.Visible = true;
                                cbtnMod.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
            //}

            if (System.IO.Path.GetFileName(Request.Path).ToUpper() == "VIEWPOPUP.ASPX")
            {
                cbtnMod.Visible = false;
                cbtnDel.Visible = false;
                cbtnEnd.Visible = false;
            }

            // 자원이 삭제가 되어 있으면 수정이 불가능하게 처리.
            DAL.List dalList = new DAL.List();
            DataTableReader dtrList = dalList.GetListInfo(CurrentUserInfo.CompanyID, _listID);

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        if (dtrList["ActiveUse"].ToString() == "0")
                        {
                            cbtnMod.Visible = false;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-03
        /// 작성목적 : 자원 주 담당자의 대기 예약 카운트 가져오기
        /// </summary>
        public void GetRMCnt()
        {
            DAL.List dal = new DAL.List();
            int tmpCnt = dal.GetListUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (tmpCnt > 0)
            {
                DAL.Reverse redal = new DAL.Reverse();
                RmCnt = redal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");

                if (System.IO.Path.GetFileName(Request.Path).ToUpper() == "VIEWPOPUP.ASPX")
                {
                    cbtnList.Text = resMng.GetString("Close");
                }
                else
                {
                    cbtnList.Text = resMng.GetString("list");
                }

                cbtnEnd.Text = resMng.GetString("End"); //종료
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            TagGubun = GetDecryptParam("tagGubun", "");
            ReservationID = GetDecryptParam("reservationID", "").ToNumber<int>();
            GroupGubun = GetDecryptParam("groupGubun", "1");
            DeptPageNo = GetDecryptParam("deptPageNo", "0").ToNumber<int>();
            MainPageNo = GetDecryptParam("mainPageNo", "0").ToNumber<int>();
            CntYN = GetDecryptParam("cntYN", "N");
        }


        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            bool rtnVal = false;
            BLL.Reserve bll = new BLL.Reserve();

            if (hdRepeatSel.Text == "1")
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReservationID, ParentReservationID, WriteYM);
            }
            else if (hdRepeatSel.Text == "2")
            {
                string tmpStartDate = StartDate.ToShortDateString().Replace("-", "");
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReservationID, ParentReservationID, WriteYM, tmpStartDate);
            }
            else if (hdRepeatSel.Text == "3")
            {
                rtnVal = bll.DelAll(CurrentUserInfo.CompanyID, ParentReservationID);
            }
            else
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReservationID);
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        private void DelList()
        {
            if (Del())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                if (TagGubun == "my")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/MyList.aspx");
                }
                else if (TagGubun == "usable0")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable0List.aspx");
                }
                else if (TagGubun == "usable1")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable1List.aspx");
                }
                else if (TagGubun == "usable2")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable2List.aspx");
                }
                else if (TagGubun == "main")
                {
                    paraDic.Add("cntYN", "Y");
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Home/Main.aspx");
                }
                else
                {
                    paraDic.Add("cntYN", "Y");
                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("tmpDate", TmpDate.ToString());
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("tagGubun", TagGubun.ToString());
                    paraDic.Add("groupGubun", GroupGubun.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/ListMain.aspx");
                }

                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-04-03
        /// 작성목적 : 예약 자원 종료
        /// </summary>
        /// <returns></returns>
        private bool ModifyEnd()
        {
            bool rtnVal = false;

            BE.ReserveInfo info = new BE.ReserveInfo();
            BLL.Reserve bll = new BLL.Reserve();

            DAL.Reverse dal = new DAL.Reverse();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, ReservationID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ParentReservationID = ParentReservationID;
                        info.ReservationID = ReservationID;
                        info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                        info.UserImg = dtr["UserImg"].ToString();
                        info.Content = lbContent.Text;
                        info.IsGuard = dtr["IsGuard"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        info.ModifyUserID = CurrentUserInfo.UserID;
                        info.ModifyUserName = CurrentUserInfo.Name;
                        info.ModifyDate = System.DateTime.Now;
                        info.WriteDateYM = WriteYM;

                        info.PreLang = PreLang;
                        info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";

                        info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());

                        // 자원 리스트
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.Qty = dtr["Qty"].ToString().ToNumber<int>();
                        info.Usable = dtr["Usable"].ToString();
                    }

                }
            }

            // 공유자 리스트
            List<BE.ShareUserInfo> arrSuInfo = new List<BE.ShareUserInfo>();

            DAL.ShareUser Sharedal = new DAL.ShareUser();

            DataTableReader sdtr = Sharedal.GetShareUserList(CurrentUserInfo.CompanyID, ReservationID);
            if (sdtr != null)
            {
                if (sdtr.HasRows)
                {
                    while (sdtr.Read())
                    {
                        BE.ShareUserInfo suInfo = new BE.ShareUserInfo();

                        if (sdtr["Kind"].ToString() == "G")
                        {
                            //부서
                            suInfo.UserID = 0;
                            suInfo.DeptID = sdtr["DeptID"].ToString().ToNumber<int>();
                        }
                        else
                        {
                            //사용자
                            suInfo.UserID = sdtr["UserID"].ToString().ToNumber<int>();
                            suInfo.DeptID = 0;
                        }

                        suInfo.CompanyID = sdtr["CompanyID"].ToString().ToNumber<int>();
                        suInfo.Kind = sdtr["Kind"].ToString();

                        arrSuInfo.Add(suInfo);
                    }
                }
            }

            List<BE.ReserveEtcInfo> al = new List<BE.ReserveEtcInfo>();

            //반복 일정 X
            if (hdRepeatChk.Text != "")
            {
                if (hdRepeatSel.Text == "1") // 1. 이번 일정만
                {
                    #region 반복 일정 미수정시 (해당 일정 하나만 수정 / 반복일정에서 빠진다)

                    TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                    info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                    info.ParentReservationID = ReservationID;

                    #region 30분 이전에 종료 누르면 30분, 30분 이후에 종료 누르면 0분으로

                    DateTime dt = new DateTime();
                    dt = DateTime.Now;

                    TimeSpan StartGap = info.StartDate - dt;
                    double sDifferMinite = StartGap.TotalMinutes;

                    TimeSpan EndGap = info.EndDate - dt;
                    double eDifferMinite = EndGap.TotalMinutes;

                    if (sDifferMinite < 0 && eDifferMinite > 0)
                    {
                        if (dt.Minute < 30)
                        {
                            info.EndDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);

                            info.EndDate = info.EndDate + TimeSpan.FromMinutes(30);
                        }
                        else
                        {
                            info.EndDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0);

                            info.EndDate = info.EndDate + TimeSpan.FromMinutes(30);
                        }

                        gap = info.EndDate.Subtract(info.StartDate);
                        info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();
                    }

                    #endregion

                    rtnVal = bll.Mod(info, ParentReservationID, arrSuInfo);

                    #endregion 반복 일정 미수정시 (해당 일정 하나만 수정 / 반복일정에서 빠진다)
                }
            }
            else
            {
                #region 반복 일정 설정이 없었을 경우

                TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                info.ParentReservationID = ReservationID;

                #region 30분 이전에 종료 누르면 30분, 30분 이후에 종료 누르면 0분으로

                DateTime dt = new DateTime();
                dt = DateTime.Now;

                TimeSpan StartGap = info.StartDate - dt;
                double sDifferMinite = StartGap.TotalMinutes;

                TimeSpan EndGap = info.EndDate - dt;
                double eDifferMinite = EndGap.TotalMinutes;

                if (sDifferMinite < 0 && eDifferMinite > 0)
                {
                    if (dt.Minute < 30)
                    {
                        info.EndDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);

                        info.EndDate = info.EndDate + TimeSpan.FromMinutes(30);
                    }
                    else
                    {
                        info.EndDate = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0);

                        info.EndDate = info.EndDate + TimeSpan.FromMinutes(30);
                    }

                    gap = info.EndDate.Subtract(info.StartDate);
                    info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();
                }

                #endregion

                rtnVal = bll.Mod(info, arrSuInfo);

                #endregion 반복 일정 설정이 없었을 경우
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-02-23
        /// 작성목적 : 예약 자원 종료
        /// </summary>
        private void ModifyEndList()
        {
            if (ModifyEnd())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                if (TagGubun == "my")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/MyList.aspx");
                }
                else if (TagGubun == "usable0")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable0List.aspx");
                }
                else if (TagGubun == "usable1")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable1List.aspx");
                }
                else if (TagGubun == "usable2")
                {
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/Manager/Usable2List.aspx");
                }
                else if (TagGubun == "main")
                {
                    paraDic.Add("cntYN", "Y");
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Home/Main.aspx");
                }
                else
                {
                    paraDic.Add("cntYN", "Y");
                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("tmpDate", TmpDate.ToString());
                    paraDic.Add("searchField", SearchField.ToString());
                    paraDic.Add("searchValue", SearchValue.ToString());
                    paraDic.Add("tagGubun", TagGubun.ToString());
                    paraDic.Add("groupGubun", GroupGubun.ToString());
                    paraDic.Add("goURL", "/DeskPlusEIP/RM/Content/ListMain.aspx");
                }

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        ///
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate
        {
            get { return Convert.ToDateTime(ViewState["StartDate"]); }
            set { ViewState["StartDate"] = value; }
        }

        /// <summary>
        /// WriteYM
        /// </summary>
        public string WriteYM
        {
            get { return GetViewStateString("WriteYM", ""); }
            set { ViewState["WriteYM"] = value; }
        }

        /// <summary>
        /// ParentReservationID
        /// </summary>
        public int ParentReservationID
        {
            get { return GetViewStateInt("ParentReservationID", "0"); }
            set { ViewState["ParentReservationID"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 삭제 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();
            DelList();
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2018-04-11
        /// 작성목적 : 종료 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirmEnd_Click(object sender, EventArgs e)
        {
            LoadParam();
            ModifyEndList();
        }

        #endregion (이벤트핸들러)
    }
}

