using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ViewTotal : System.Web.UI.Page
    {
        #region 변수처리      
        public int ReservationID;
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값
        public string Usable = "";
        #endregion 변수처리

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                PreLangs = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);
            }

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ViewTotal).Assembly;	//어셈블리 선언
            #endregion

            GetResourceManager();

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetData();
        }

        /// <summary>
        /// 작성목적 : 예약 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.Reverse dal = new DAL.Reverse();

            DataTableReader dtr = dal.GetList(GetViewStateString("CompanyID", "0").ToNumber<int>(), ReservationID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbSubject.Text = dtr["Subject"].ToString();                        
                        lbDate.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["StartDate"].ToString()).ToShortTimeString() + " ~ " +
                             DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["EndDate"].ToString()).ToShortTimeString();
                        //RblPattern.SelectedValue = dtr["UserImg"].ToString();

                        if (dtr["UserImg"].ToString() == "FF0000")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c1.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "FFC600")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c2.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "FFFF00")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c3.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "94D652")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c4.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "00B552")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c5.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "00B5F7")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c6.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "0073C6")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c7.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "002163")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c8.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "7331A5")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c9.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "943131")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c10.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "5A4A7B")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c11.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "E76B09")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c12.gif' alt='' />";
                        }
                        else if (dtr["UserImg"].ToString() == "32849C")
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c13.gif' alt='' />";
                        }
                        else
                        {
                            Pattern.InnerHtml = "<img src='/DeskPlusEIP/Design/C/Img/RM/c1.gif' alt='' />";
                        }


                        lbWriteName.Text = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLangs);

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
                                                                   arrManyName[5], arrManyName[6], PreLangs);
                        }

                        //lbManagerName.Text = tmpManagerName;

                        spTitle.InnerText = dtr["Title"].ToString().ToReverseXSS() + " (" + dtr["Qty"].ToString() + ") " + tmpManagerName;

                        ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());

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
                    }
                }
            }

            GetRepeat(ParentReservationID);
        }

        /// <summary>
        /// 작성목적 : 반복 설정 가져오기
        /// </summary>
        private void GetRepeat(int _parentReservationID)
        {
            DAL.Repeat dal = new DAL.Repeat();

            DataTableReader dtr = dal.GetList(GetViewStateString("CompanyID", "0").ToNumber<int>(), ParentReservationID);

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
      

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnList.Text = resMng.GetString("Close");               
            }
        }

        /// <summary>
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            //ViewState["CompanyID"] = GetDecryptParam("companyID", "0").ToNumber<int>();
            //ReservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();
            ViewState["CompanyID"] = 1;
            ReservationID = 342;
        }

        /// <summary>
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

        private void SetDecryptString()
        {
            string param = GetParm("p", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }

        /// <summary>
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        /// <summary>
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        protected string GetLangName(string _name, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            return UtilHelper.GetLangName(_name, _ko, _en, _ja, _fr, _zhcn, _zhtw, _preLang);
        }


        #endregion (함수)

        #region (프로퍼티)

      
        /// <summary>
        /// ParentReservationID
        /// </summary>
        public int ParentReservationID
        {
            get { return GetViewStateInt("ParentReservationID", "0"); }
            set { ViewState["ParentReservationID"] = value; }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE INT 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName">Name of the _view state.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected int GetViewStateInt(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (int)(ViewState[_viewStateName] ?? Convert.ToInt32(_errorReturn));
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToInt32(ViewState[_viewStateName]) : Convert.ToInt32(_errorReturn);
            }
        }


        /// <summary>
        /// 작성목적 : VIEWSTATE STRING 형 가져오기
        /// </summary>
        /// <param name="_viewStateName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetViewStateString(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (string)(ViewState[_viewStateName] ?? _errorReturn);
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToString(ViewState[_viewStateName]) : Convert.ToString(_errorReturn);
            }
        }

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        private string PreLangs
        {
            get { return ViewState["PreLangs"].ToString(); }
            set
            {
                string lang = "";

                if (value == "")
                {
                    lang = Request.UserLanguages[0].ToLower();
                }
                else
                {
                    lang = value.ToLower();
                }

                if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans" || lang == "zh-sg")  //zh-Hans(중국어간체)
                {
                    lang = "zh-cn";
                }
                else if (lang == "zh-tw" || lang == "zh-hant" || lang == "zh-mo")  //번체
                {
                    lang = "zh-tw";
                }
                else if (lang == "ja" || lang == "ja-jp") //일본어
                {
                    lang = "ja";
                }
                else if (lang == "ko" || lang == "ko-kr") //한국어
                {
                    lang = "ko";
                }
                else //나머지는 영어
                {
                    lang = "en";
                }

                ViewState["PreLangs"] = lang;
            }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)
        #endregion (이벤트핸들러)
    }
}

