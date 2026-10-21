using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class View : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리
        public int CurrentPage = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string TmpDate = String.Empty;
        public string TagGubun = String.Empty;
        public int ReserveID;
        public string VisitType = String.Empty;
        public string ShareCheck = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetData();            
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.Reserve dal = new DAL.Reserve();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, ReserveID);

            int writeUserID = 0;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbSubject.Text = dtr["Subject"].ToString();                        
                        lbDate.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["StartDate"].ToString()).ToShortTimeString() + " ~ " +
                             DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["EndDate"].ToString()).ToShortTimeString();

                        lbWriteName.Text = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);dtr["Content"].ToString();

                        lbContent.Text = dtr["Content"].ToString().ToEditorXSS();                      

                        ReserveGroupID = dtr["ReserveGroupID"].ToString().ToNumber<int>();
                        StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        WriteYM = dtWriteDate.Year.ToString() + dtWriteDate.Month.ToString().PadLeft(2, '0');
                        writeUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        VisitType = dtr["VisitType"].ToString();                      
                    }
                }               
            }

            GetAuth(writeUserID);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 버튼 권한 가져오기
        /// </summary>
        private void GetAuth(int _writeUserID)
        {
            // 관리자 권한 
            if (AccessAuth)
            {
                cbtnDel.Visible = true;
                cbtnMod.Visible = true;       
            }
            else
            {
                if (_writeUserID == CurrentUserInfo.UserID)
                {
                    cbtnDel.Visible = true;
                    cbtnMod.Visible = true;
                }
                else
                {
                    cbtnDel.Visible = false;
                    cbtnMod.Visible = false;                   
                }                
            }

            // 본인이 기본 공유자에 속하는 지 파악 
            DAL.Share dal = new DAL.Share();
            int ShareCount = dal.GetShare(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (ShareCount > 0)
            {
                ShareCheck = "Y";                
            }
            else
            {
                ShareCheck = "N";
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
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
                cbtnList.Text = resMng.GetString("List");
                cbtnAccept.Text = resMng.GetString("Type1");
                cbtnUnaccept.Text = resMng.GetString("Type2");       
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            TagGubun = GetDecryptParam("tagGubun", "");
            ReserveID = GetDecryptParam("reserveID", "").ToNumber<int>();
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
        /// 작성일자 : 2014-02-18
        /// 작성목적 : 방문 예약 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            bool rtnVal = false;
            BLL.Reserve bll = new BLL.Reserve();

            if (hdRepeatSel.Text == "1")
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReserveID, ReserveGroupID, WriteYM);
            }
            else if (hdRepeatSel.Text == "2")
            {
                string tmpStartDate = StartDate.ToShortDateString().Replace("-","");
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReserveID, ReserveGroupID, WriteYM, tmpStartDate);
            }
            else if (hdRepeatSel.Text == "3")
            {
                rtnVal = bll.DelAll(CurrentUserInfo.CompanyID, ReserveGroupID);
            }
            else
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, ReserveID);
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        private void DelList()
        {
            if (Del())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();                
                paraDic.Add("tmpDate", TmpDate.ToString());
                paraDic.Add("searchField", SearchField.ToString());
                paraDic.Add("searchValue", SearchValue.ToString());
                paraDic.Add("tagGubun", TagGubun.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Visit/Content/ListMain.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }



        #endregion

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
        /// ReserveGroupID
        /// </summary>
        public int ReserveGroupID
        {
            get { return GetViewStateInt("ReserveGroupID", "0"); }
            set { ViewState["ReserveGroupID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 삭제 버튼 클릭시 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            DelList();
        }

        #endregion
    }
}