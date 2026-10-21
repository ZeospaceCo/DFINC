using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model;
using ComponentArt.Web.UI;


namespace Zeospace.DeskPlus.ETC.Web.Customize.User
{
    public partial class AddMaster : Base.BasePageETC
    {
        #region 변수설정
        private string eventArgs = String.Empty;

        public string ExUseValidation = string.Empty; // 1 = 인사정보미등록 , 2=카드정보 미등록 , 3=카드내역이 없는 경우 

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Customize.User.AddMaster).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                bool basinInfo = LoadData();
                bool cardbool = false;
                // 법인카드 등록 이력 가져오고, 상태값을 체크 해준다.

                if (basinInfo)
                {
                    cardbool = LoadMyGWCardInfo();
                }

                // 비용 데이터 조회 - 카드포함
                LoadExpenseData();

                // 카드쪽 레이어 별도 조회 
                if(cardbool)
                    LoadCardData();

                //Product Group 기준값 로딩 
                LoadProductGroupStand();

                // 
                LegacyKey = DateTime.Now.ToFileTime().ToString();
            }    
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadProductGroupStand()
        {
            DalLegacy biz = new DalLegacy();
            DataSet ds = biz.GetProductGroupStand();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ProductGroupStandard = ProductGroupStandard + ds.Tables[0].Rows[i][0].ToString() + "|";
                }
            }
        }

        private void LoadExpenseData()
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "T";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserList(info);

            RepeaterEx1.DataSource = dtr;
            RepeaterEx1.DataBind();


            DataTableReader dtr2 = biz.GetExpenseUserList(info);
            int Total = 0;
            if (dtr2 != null)
            {
                if (dtr2.HasRows)
                {
                    while (dtr2.Read())
                    {
                        Total += dtr2["CreditAmount"].ToString().Replace(",", "").ToNumber<int>();
                    }
                }
            }

            this.TotalAmount.Text = Total.ToString();

            // script Call 해준다. 
            //string scripts = @"<script type=""text/javascript"">AddMaster.ColorLayer();" + "<" + "/script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ExColor", scripts);
            
        }

        private bool LoadMyGWCardInfo()
        {
            // 법인카드 등록 이력 가져온다.
            DalLegacy biz = new DalLegacy();

            DataSet dsMyCard = biz.GetMyCardInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dsMyCard != null && dsMyCard.Tables[0].Rows.Count > 0)
            {
                //if (dsMyCard.Tables[0].Rows.Count == 1)
                //{
                //    TargetCardNo = dsMyCard.Tables[0].Rows[0]["CardNumber"].ToString();
                //    TargetCardKind = dsMyCard.Tables[0].Rows[0]["IsSec"].ToString();
                //}
                //else
                //{
                //    TargetCardNo = dsMyCard.Tables[0].Rows[1]["CardNumber"].ToString();
                //    TargetEtcCardNo = dsMyCard.Tables[0].Rows[0]["CardNumber"].ToString();
                //    TargetCardKind = "P";
                //}
                for (int i = 0; i < dsMyCard.Tables[0].Rows.Count; i++)
                {
                    TargetEtcCardNo += dsMyCard.Tables[0].Rows[i]["CardNumber"].ToString() + "|";

                    if (dsMyCard.Tables[0].Rows[i]["IsSec"].ToString() == "P")
                    {
                        TargetCardNo = dsMyCard.Tables[0].Rows[i]["CardNumber"].ToString();
                    }
                }
            }
            else
            {
                ExUseValidation = "2";
                return false;
            }

            return true;
        }


        private void LoadCardDataEtc()
        {
            // 사용자 카드 내역을 조회 해온다.
            DalLegacy biz2 = new DalLegacy();
            DataTableReader dtr2 = biz2.GetPageListCard(CurrentUserInfo.CompanyID, TargetCardNo, CardDate); // 개인법인 카드번호로 가져오고 (관리자가 등록해주더라도 1개만 있는 경우는 개인 법인임)
            //if (dtr2.HasRows)
            //{
                RepeaterEx2.DataSource = dtr2;
                RepeaterEx2.DataBind();
            //}

        }

        private void LoadCardData()
        {
            // 사용자 카드 내역을 조회 해온다.
            DalLegacy biz2 = new DalLegacy();
            DataTableReader dtr2 = biz2.GetPageListCard(CurrentUserInfo.CompanyID, TargetCardNo, CardDate); // 개인법인 카드번호로 가져오고 (관리자가 등록해주더라도 1개만 있는 경우는 개인 법인임)
            if (dtr2.HasRows)
            {
                RepeaterEx2.DataSource = dtr2;
                RepeaterEx2.DataBind();
            }
            else
            {
                //dtr2 = biz2.GetPageListCard(CurrentUserInfo.CompanyID, TargetEtcCardNo); // 없으면 일반법인 카드(관리자등록) 으로 다시 가져온다.

                RepeaterEx2.DataSource = dtr2;
                RepeaterEx2.DataBind();

                //if (!dtr2.HasRows) // 카드내역이 없는 경우 
                //{
                ExUseValidation = "3";
                //}
            }


        }



        private void LoadCardDataPe(string _from, string _to)
        {
            // 사용자 카드 내역을 조회 해온다.
            DalLegacy biz2 = new DalLegacy();
            DataTableReader dtr2 = biz2.GetPageListCardPe(CurrentUserInfo.CompanyID, TargetCardNo, _from, _to); // 개인법인 카드번호로 가져오고 (관리자가 등록해주더라도 1개만 있는 경우는 개인 법인임)
            if (dtr2.HasRows)
            {
                RepeaterEx2.DataSource = dtr2;
                RepeaterEx2.DataBind();
            }
            else
            {
                //dtr2 = biz2.GetPageListCard(CurrentUserInfo.CompanyID, TargetEtcCardNo); // 없으면 일반법인 카드(관리자등록) 으로 다시 가져온다.

                RepeaterEx2.DataSource = dtr2;
                RepeaterEx2.DataBind();

                //if (!dtr2.HasRows) // 카드내역이 없는 경우 
                //{
                ExUseValidation = "3";
                //}
            }


        }
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private bool LoadData()
        {
            // 사용자 정보로 기본 정보를 가져온다. 쿠키정보에 UserCode 있으면 좋을텐데.
            CustomBiz.DalLegacy Biz = new CustomBiz.DalLegacy();
            DataSet dsUserBaseInfo = Biz.GetUserBaseInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dsUserBaseInfo != null && dsUserBaseInfo.Tables[0].Rows.Count > 0)
            {
                this.ctbVendorCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorCode"].ToString();
                this.ctbVendorNM.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorNM"].ToString();
                //this.ctbVendorCDEx.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorCode"].ToString();
                //this.ctbVendorNMEx.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorNM"].ToString();
                this.ctbPostingDate.Text = DateTime.Now.ToShortDateString();
                //this.ctbDocumentDateAdd.Text = DateTime.Now.ToShortDateString();
                this.ctvBankAccountCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["BankAccountCode"].ToString();
                //this.ctvBankAccountCDEx.Text = dsUserBaseInfo.Tables[0].Rows[0]["BankAccountCode"].ToString();

                VendorCD = dsUserBaseInfo.Tables[0].Rows[0]["VendorCode"].ToString();
                VendorNM = dsUserBaseInfo.Tables[0].Rows[0]["VendorNM"].ToString();
                
                VendorPostingGroup = dsUserBaseInfo.Tables[0].Rows[0]["VendorPostingGroup"].ToString();
                // Postring Group 을 세팅 해준다. 
                this.ctvPostingGroup.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorPostingGroup"].ToString();
                //this.ctvPostingGroupAdd.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorPostingGroup"].ToString();

                BankAccountCD = dsUserBaseInfo.Tables[0].Rows[0]["BankAccountCode"].ToString();
                ErpUserNM = dsUserBaseInfo.Tables[0].Rows[0]["ErpUserNM"].ToString();
                DepartMentDimensionCD = dsUserBaseInfo.Tables[0].Rows[0]["DepartMentDimentionCD"].ToString();
                BankClearingID = dsUserBaseInfo.Tables[0].Rows[0]["BankClearingID"].ToString();
                BankAccountNo = dsUserBaseInfo.Tables[0].Rows[0]["BankAccountNo"].ToString();

                //DueDate 도 결정 해준다.
                PaymentTerm = dsUserBaseInfo.Tables[0].Rows[0]["PaymentTerm"].ToString();

                this.ctvDepartMentCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["DepartMentDimentionCD"].ToString();
                this.ctvDepartMent.Text = dsUserBaseInfo.Tables[0].Rows[0]["DepartMentNM"].ToString();
                this.ctbDepartMentOld.Text = dsUserBaseInfo.Tables[0].Rows[0]["DepartMentNM"].ToString();
                this.ctbDepartMentOldCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["DepartMentDimentionCD"].ToString();

                this.ctvEmployee.Text = dsUserBaseInfo.Tables[0].Rows[0]["EmpNM"].ToString();
                this.ctvEmployeeCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorCode"].ToString();
                this.ctbEmployeeOld.Text = dsUserBaseInfo.Tables[0].Rows[0]["EmpNM"].ToString();
                this.ctbEmployeeOldCD.Text = dsUserBaseInfo.Tables[0].Rows[0]["VendorCode"].ToString();



                //this.ctbDueDateAdd.Text = SetDueDate(PaymentTerm, this.ctbPostingDate.Text.ToString());
                DueDate = SetDueDate(PaymentTerm, this.ctbPostingDate.Text.ToString());
            }
            else
            {
                ExUseValidation = "1";
                return false;
            }

            return true;
            
        }

        #endregion

        #region (함수)

        public string SetDueDate(string _PaymentTerm, string _date)
        {
            string rtnValue = string.Empty;
            DateTime currentDate = DateTime.Parse(_date);
            DateTime rtnDate = DateTime.Parse(_date);
            int AddDate = 0;

            if (_date != "")
            {
                if (_PaymentTerm.IndexOf('D') > -1)
                {
                    AddDate = Int32.Parse(_PaymentTerm.Split('D')[0].ToString()) * 1;
                }

                if (_PaymentTerm.IndexOf('W') > -1)
                {
                    AddDate = Int32.Parse(_PaymentTerm.Split('W')[0].ToString()) * 7;
                }

                rtnDate = currentDate.AddDays(AddDate);

                rtnValue = rtnDate.ToShortDateString();
            }
            else
            {
                return "";
            }

            

            return rtnValue;
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            //    cbtnSearch.Text = "검색"; // "검색";
            }

            //ddlSearchField.Items.Clear();
            //ddlSearchField.Items.Add(new ListItem("통합검색", "all"));
            //ddlSearchField.Items.Add(new ListItem("Voucher No", "VoucherNo"));
            //ddlSearchField.Items.Add(new ListItem("Vendor Name", "ExVendorNM"));
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            // ListID = GetDecryptParam("listID", "").ToString();
            // DocID = GetDecryptParam("docID", "0").ToNumber<int>();
        }


        /// <summary>
        /// 바우처 넘버링
        /// </summary>
        /// <param name="_param">The _param.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string GetVoucherNo()
        {
            //회사이이디,사용자아이디,키값
            DalLegacy biz = new DalLegacy();
            string newVoucherNo = biz.SeqUpdate(1);

            int newVoucherNoTmp = 10000000 + Int32.Parse(newVoucherNo);

            string VoucherNo = "EV" + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + newVoucherNoTmp.ToString().Substring(1, 7);

            return VoucherNo;

        }


        public string SubjectNotLong(string _subject)
        {
            if (_subject.Length > 15)
            {
                return _subject.Substring(0,14) + ". . .";
            }
            else
            {
                return _subject;
            }
        }

        public string FormatExVendorNM(string _subject)
        {
            if (_subject.Length > 4)
            {
                return _subject.Substring(0, 4) + ". . .";
            }
            else
            {
                return _subject;
            }
        }


        /// <summary>
        /// 벤더팝업 선택 이후 나머지 값을 Ajax 로 가져오는 값
        /// </summary>
        /// <param name="_vendorCD"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxGetVendorEtc(string _vendorCD)
        {
            //회사이이디,사용자아이디,키값
            DalLegacy biz = new DalLegacy();
            string etcVal = biz.GetVendorEtc(_vendorCD);

            return etcVal;

        }



        /// <summary>
        /// Vendor 텝에서 시스템 코드가 사용가능한지를 가져온다.
        /// </summary>
        /// <param name="_vendorCD"></param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxGetSysCode(string _vendorCD)
        {
            //회사이이디,사용자아이디,키값
            DalLegacy biz = new DalLegacy();
            string etcVal = biz.GetSysCode(_vendorCD);

            return etcVal;

        }


        [ComponentArtCallbackMethod]
        public string AjaxGetDueDate(string _PaymentTerm, string _date)
        {
            //회사이이디,사용자아이디,키값
            return SetDueDate(_PaymentTerm, _date);

        }


        /// <summary>
        /// 바우처 넘버링
        /// </summary>
        /// <param name="_param">The _param.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxAnotherCardInfo(string _venCD)
        {
            //회사이이디,사용자아이디,키값
            CustomBiz.DalLegacy Biz = new CustomBiz.DalLegacy();
            string dsAnotherCardInfo = Biz.GetUserAnotherBasicInfo(1, _venCD);

            return dsAnotherCardInfo;


        }



        /// <summary>
        /// 바우처 넘버링
        /// </summary>
        /// <param name="_param">The _param.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxSetKeyData(string legacykey, string CheckVal, string UserID)
        {
            //회사이이디,사용자아이디,키값
            try
            {
                CustomBiz.DalLegacy Biz = new CustomBiz.DalLegacy();
                Biz.SetKeyData(legacykey, CheckVal, UserID);

                return "1";
            }
            catch
            {
                return "9";
            }
            //return "1";
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-14
        /// 작성목적 : 핸들러
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;
                switch (args)
                {
                    case "AddExpense":  //비용 한줄 추가
                        AddExpense();
                        break;
                    case "AddCard":  //카드 여러줄 추가 
                        AddCard();
                        break;
                    case "AnotherCard":  //다른 카드 추가
                        AnotherCard();
                        break;
                    case "DelItem":  //카드 여러줄 추가 
                        DelItem();
                        break;
                    case "AddExpenseUpdate":  //카드 여러줄 추가 
                        AddExpenseUpdate();
                        break;
                    case "CardLoad":  //카드 여러줄 추가 
                        CardLoad();
                        break;
                    case "AllDelItem":  //카드 여러줄 추가 
                        AllDelItem();
                        break;
                    case "ExReload":  //카드 여러줄 추가 
                        ExReload();
                        break;
                    case "CardPeriod":  //카드 여러줄 추가 
                        CardPeriod();
                        break;
                    case "ExpenseUp":  //카드 여러줄 추가 
                        ExpenseUp();
                        break;
                    case "AddCardUpdate":  //카드 여러줄 추가 
                        AddCardUpdate();
                        break;
                    //case "DelAuth":  //삭제
                    //    DelAuthTemp();
                    //    break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        private void ExReload()
        {
            LoadExpenseData();
        }


        private void AnotherCard()
        {
            string Temp = TmpData.Text.ToString();
            TargetCardNo = Temp;
            //TargetEtcCardNo = Temp;
            LoadCardDataEtc();
        }


        private void CardLoad()
        {
            CardDate = TmpData.Text.ToString();
            //TargetEtcCardNo = Temp;
            LoadCardData();
        }


        private void CardPeriod()
        {
            string[] tmpDate =  TmpData.Text.ToString().Split('★');
            //TargetEtcCardNo = Temp;
            LoadCardDataPe(tmpDate[0], tmpDate[1]);
        }

        private void ExpenseUp()
        {
            string[] tmpDate = TmpData.Text.ToString().Split('★');
            //TargetEtcCardNo = Temp;
            DalLegacy biz = new DalLegacy();

            biz.ExpenseUp(tmpDate[0], tmpDate[1]);
        }

        private void ExpenseDown()
        {
            string[] tmpDate = TmpData.Text.ToString().Split('★');
            //TargetEtcCardNo = Temp;
            LoadCardDataPe(tmpDate[0], tmpDate[1]);
        }


        /// <summary>
        /// 카드관련
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LBTNProcessCard_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;
                switch (args)
                {
                    case "AddCard":  //비용 한줄 추가
                        AddCard();
                        break;
                    //case "DelAuth":  //삭제
                    //    DelAuthTemp();
                    //    break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 법인카드 사용자 한줄 추가
        /// </summary>
        private void AddCard()
        {
            string [] ArrCard = TmpData.Text.ToString().Split('★');
            string[] arrDetail;
            bool voucherGubun = false;

            ExpenseUserInfo info;
            DalLegacy biz;
            for (int i = 0; i < ArrCard.Length - 1; i++)
            {
                arrDetail = ArrCard[i].Split('|');
                info = new ExpenseUserInfo();

                // VoucherNo0 + "|" + CardExKind1 + "|" + vendorCD2 + "|" + DocumentDate3 + "|" + PostingDate4 + "|" + DueDate5 + "|" + VendorDescription6 + "|"
                // ExDescription7 | 공급가액(계산된값)8 | 부가세값9 | 합계금액(요청금액)10 | Dimension1Code(부서코드)11 | Dimension2Code(사번)12 | Dimension3Code(벤더)13 
                //   | Dimension4Code(Product Group)14 | ExDimension1Code(부서코드)15 | ExDimension2Code(사번)16 | ExDimension3Code(벤더)17 | ExDimension4Code(Product Group)18
                //  | VendorNM19 | ExVendorNM20 | GLAccount21 | GLAccountNM22 | TaxCode23 | TaxCodeNM24 | ExBankAccountCode25 | ExBankAccountNo26 | ExBankAccountCearing27
                //  | ExPostingGroup28 | BankAcountCode29 | BankAccountNo30 | BankAcountCearingID31 | PONo32 | CardApprovalNo33 | DocumentID34 | Status35

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.LegacyKey = "";  // 비용초기 입력때는 발생하지 않음 , 전자결재 호출전 update 해준다.
                info.VoucherNo = arrDetail[0];
                info.CardExKind = arrDetail[1];
                info.DocumentDate = Convert.ToDateTime(arrDetail[3].ToString());
                info.PostingDate = Convert.ToDateTime(arrDetail[4].ToString());  // 임시
                info.DueDate = Convert.ToDateTime(arrDetail[5].ToString());      // 임시
                info.VendorDescription = arrDetail[6];
                info.ExDescription = arrDetail[7]; // 일단 VendorDescription 으로 같이 넣고 추후 해당 부분 요청 사항 나올 시 진행 
                if (arrDetail[8] != "")
                    info.DebitAmount1 = decimal.Parse(arrDetail[8].ToString().Replace(",", ""));// 임시 
                else
                    info.DebitAmount1 = 0;

                if(arrDetail[9] != "")
                    info.DebitAmount2 = decimal.Parse(arrDetail[9].ToString().Replace(",", "")); // 임시 
                else
                   info.DebitAmount2 = 0;

                if(arrDetail[10] != "")
                    info.CreditAmount = decimal.Parse(arrDetail[10].ToString().Replace(",", ""));  // 임시 
                else
                    info.CreditAmount = 0;

                info.PostingGroup = arrDetail[28]; // 일단 Vendor Posting Group 를 넣어준다.
                info.Dimension1Code = arrDetail[11]; // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.Dimension2Code = arrDetail[12];              // 임시 - 사용자의 사번이여야 한다. 초기값으로 땡겨 와야 하는데 안 떙겨 오고 있음 벤더가 바뀌더라도 바뀌지 않음
                info.Dimension3Code = arrDetail[13];              // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.Dimension4Code = arrDetail[14];

                // 추가
                info.ExDimension1Code = arrDetail[15];
                info.ExDimension2Code = arrDetail[16];
                info.ExDimension3Code = arrDetail[17];
                info.ExDimension4Code = arrDetail[18];
                info.VendorNM = arrDetail[19];
                info.ExVendorNM = arrDetail[20];
                info.GLAccount = arrDetail[21];
                info.GLAccountNM = arrDetail[22];
                info.TaxCode = arrDetail[23];
                info.TaxCodeNM = arrDetail[24];
                info.ExBankAccountCode = arrDetail[25];
                info.ExBankAccountNo = arrDetail[26];
                info.ExBankAccountCearing = arrDetail[27];
                info.ExPostingGroup = arrDetail[28];

                info.BankAcountCode = arrDetail[29];         // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.BankAccountNo = arrDetail[30];                    // 임시 - 초기 혹은 바뀔떄 땡겨와야 하는데.. 안가져 오고 있음 
                info.BankAcountCearingID = arrDetail[31];   // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.PONo = arrDetail[32];                           // 구매관련 스펙 확인시 진행
                info.CardApprovalNo = arrDetail[33];                   // 카드에서 진행
                info.DocumentID = Int32.Parse(arrDetail[34].ToString());                        // 초기 입력시는 0 결재상신후 update 대상 필드임
                info.Status = arrDetail[35];                        // Temp 로 초기 입력시 T

//사업장 코드 - VENDOR_NUM - Ex)759206175                etc1
//벤더명 - VENDor_NAme - 투썸플레이스서울스퀘어점        etc2  
//대표자명 - vendor_person - 문윤민                      etc3
//사업장주소 - Vendor_address1 + " " + Vendor_address2   etc4
//증빙일  - Approve_date                                 etc5
//사용시간 - approve_time                                etc6
                // 기타 필드
                info.Etc1 = arrDetail[36];
                info.Etc2 = arrDetail[37];
                info.Etc3 = arrDetail[38];
                info.Etc4 = arrDetail[39];
                info.Etc5 = arrDetail[40];
                info.Etc6 = arrDetail[41];
                info.Etc7 = arrDetail[42];
                info.Etc8 = arrDetail[42];
                info.Etc9 = arrDetail[44];
                info.Etc10 = arrDetail[45];

                biz = new DalLegacy();
                biz.ExpenseUserAdd(info);

                // 카드 상태값 변경 
                biz.CardStatusUpdate(1, arrDetail[33], "Y");

                if(arrDetail[0] == "")
                    voucherGubun = true;



            }
            
            // 상기로직 이후 VoucherNo Seq 를 Update 해준다.
            // 기존 바우처가 아닌 바우처 넘버가 새로 따진경우에만 update 해준다.
            if (voucherGubun)
            {

            }

            LoadExpenseData();
            LoadCardData();

        }



        /// <summary>
        /// 아이템삭제
        /// </summary>
        private void DelItem()
        {
            string[] ArrCard = TmpData.Text.ToString().Split('★');
 
            DalLegacy biz = new DalLegacy();

            biz.ItemDel(1, ArrCard[0]);


            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelItem=" + ArrCard[0]);

            // 카드에서 삭제 인경우 
            if(ArrCard[1] != "")
                biz.CardStatusUpdate(1, ArrCard[1], "N");


            LoadExpenseData();
            LoadCardData();

        }


        /// <summary>
        /// 아이템 전체 삭제
        /// </summary>
        private void AllDelItem()
        {

            string[] ArrCard = TmpData.Text.ToString().Split('★');

            DalLegacy biz = new DalLegacy();


            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AllDelItem=" + CurrentUserInfo.UserID.ToString());

            biz.AllItemDel(1, CurrentUserInfo.UserID.ToString());


            LoadExpenseData();
            LoadCardData();

        }


        /// <summary>
        /// 비용전표에서 사용자 하단 Row 입력 저장
        /// </summary>
        private void AddExpense()
        {
            try
            {
                string[] ArrCard = TmpData.Text.ToString().Split('★');
                string[] arrDetail;
                bool voucherGubun = false;

                ExpenseUserInfo info;
                DalLegacy biz;
                for (int i = 0; i < ArrCard.Length - 1; i++)
                {
                    arrDetail = ArrCard[i].Split('|');
                    info = new ExpenseUserInfo();

                    // VoucherNo0 + "|" + CardExKind1 + "|" + vendorCD2 + "|" + DocumentDate3 + "|" + PostingDate4 + "|" + DueDate5 + "|" + VendorDescription6 + "|"
                    // ExDescription7 | 공급가액(계산된값)8 | 부가세값9 | 합계금액(요청금액)10 | Dimension1Code(부서코드)11 | Dimension2Code(사번)12 | Dimension3Code(벤더)13 
                    //   | Dimension4Code(Product Group)14 | ExDimension1Code(부서코드)15 | ExDimension2Code(사번)16 | ExDimension3Code(벤더)17 | ExDimension4Code(Product Group)18
                    //  | VendorNM19 | ExVendorNM20 | GLAccount21 | GLAccountNM22 | TaxCode23 | TaxCodeNM24 | ExBankAccountCode25 | ExBankAccountNo26 | ExBankAccountCearing27
                    //  | ExPostingGroup28 | BankAcountCode29 | BankAccountNo30 | BankAcountCearingID31 | PONo32 | CardApprovalNo33 | DocumentID34 | Status35

                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = CurrentUserInfo.UserID;
                    info.LegacyKey = "";  // 비용초기 입력때는 발생하지 않음 , 전자결재 호출전 update 해준다.
                    info.VoucherNo = arrDetail[0];
                    info.CardExKind = arrDetail[1];
                    info.DocumentDate = Convert.ToDateTime(arrDetail[3].ToString());
                    info.PostingDate = Convert.ToDateTime(arrDetail[4].ToString());  // 임시
                    info.DueDate = Convert.ToDateTime(arrDetail[5].ToString());      // 임시
                    info.VendorDescription = arrDetail[6];
                    info.ExDescription = arrDetail[7]; // 일단 VendorDescription 으로 같이 넣고 추후 해당 부분 요청 사항 나올 시 진행 
                    if (arrDetail[8] != "")
                        info.DebitAmount1 = decimal.Parse(arrDetail[8].ToString().Replace(",", ""));// 임시 
                    else
                        info.DebitAmount1 = 0;

                    if (arrDetail[9] != "")
                        info.DebitAmount2 = decimal.Parse(arrDetail[9].ToString().Replace(",", "")); // 임시 
                    else
                        info.DebitAmount2 = 0;

                    if (arrDetail[10] != "")
                        info.CreditAmount = decimal.Parse(arrDetail[10].ToString().Replace(",", ""));  // 임시 
                    else
                        info.CreditAmount = 0;

                    info.PostingGroup = arrDetail[28]; // 임시
                    info.Dimension1Code = arrDetail[11]; // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                    info.Dimension2Code = arrDetail[12];              // 임시 - 사용자의 사번이여야 한다. 초기값으로 땡겨 와야 하는데 안 떙겨 오고 있음 벤더가 바뀌더라도 바뀌지 않음
                    info.Dimension3Code = arrDetail[13];              // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                    info.Dimension4Code = arrDetail[14];

                    // 추가
                    info.ExDimension1Code = arrDetail[15];
                    info.ExDimension2Code = arrDetail[16];
                    info.ExDimension3Code = arrDetail[17];
                    info.ExDimension4Code = arrDetail[18];
                    info.VendorNM = arrDetail[19];
                    info.ExVendorNM = arrDetail[20];
                    info.GLAccount = arrDetail[21];
                    info.GLAccountNM = arrDetail[22];
                    info.TaxCode = arrDetail[23];
                    info.TaxCodeNM = arrDetail[24];
                    info.ExBankAccountCode = arrDetail[25];
                    info.ExBankAccountNo = arrDetail[26];
                    info.ExBankAccountCearing = arrDetail[27];
                    info.ExPostingGroup = arrDetail[28];

                    info.BankAcountCode = arrDetail[29];         // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                    info.BankAccountNo = arrDetail[30];                    // 임시 - 초기 혹은 바뀔떄 땡겨와야 하는데.. 안가져 오고 있음 
                    info.BankAcountCearingID = arrDetail[31];   // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                    info.PONo = arrDetail[32];                           // 구매관련 스펙 확인시 진행
                    info.CardApprovalNo = arrDetail[33];                   // 카드에서 진행
                    info.DocumentID = Int32.Parse(arrDetail[34].ToString());                        // 초기 입력시는 0 결재상신후 update 대상 필드임
                    info.Status = arrDetail[35];                        // Temp 로 초기 입력시 T

                    //사업장 코드 - VENDOR_NUM - Ex)759206175                etc1
                    //벤더명 - VENDor_NAme - 투썸플레이스서울스퀘어점        etc2  
                    //대표자명 - vendor_person - 문윤민                      etc3
                    //사업장주소 - Vendor_address1 + " " + Vendor_address2   etc4
                    //증빙일  - Approve_date                                 etc5
                    //사용시간 - approve_time                                etc6
                    // 기타 필드
                    info.Etc1 = arrDetail[36];
                    info.Etc2 = arrDetail[37];
                    info.Etc3 = arrDetail[38];
                    info.Etc4 = arrDetail[39];
                    info.Etc5 = arrDetail[40];
                    info.Etc6 = arrDetail[41];
                    info.Etc7 = arrDetail[42];
                    info.Etc8 = arrDetail[42];
                    info.Etc9 = arrDetail[44];
                    info.Etc10 = arrDetail[45];

                    biz = new DalLegacy();
                    biz.ExpenseUserAdd(info);


                    if (arrDetail[0] == "")
                        voucherGubun = true;

                }

                // 상기로직 이후 VoucherNo Seq 를 Update 해준다.
                // 기존 바우처가 아닌 바우처 넘버가 새로 따진경우에만 update 해준다.
                if (voucherGubun)
                {

                }

                LoadExpenseData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        public string ImageType(string _val)
        {
            if (_val == "Card")
                return "/DeskPlusEIP/Design/C/Img/Icon/card.gif";
            else
                return "/DeskPlusEIP/Design/C/Img/Icon/cash.gif";
        }



        /// <summary>
        /// 비용전표에서 사용자 하단 Row 입력 저장
        /// </summary>
        private void AddExpenseUpdate()
        {
            string[] ArrCard = TmpData.Text.ToString().Split('★');
            string[] arrDetail;
            bool voucherGubun = false;

            string seq = ArrCard[1].ToString();

            ExpenseUserInfo info;
            DalLegacy biz;
            for (int i = 0; i < ArrCard.Length - 1; i++)
            {
                arrDetail = ArrCard[i].Split('|');
                info = new ExpenseUserInfo();

                // VoucherNo0 + "|" + CardExKind1 + "|" + vendorCD2 + "|" + DocumentDate3 + "|" + PostingDate4 + "|" + DueDate5 + "|" + VendorDescription6 + "|"
                // ExDescription7 | 공급가액(계산된값)8 | 부가세값9 | 합계금액(요청금액)10 | Dimension1Code(부서코드)11 | Dimension2Code(사번)12 | Dimension3Code(벤더)13 
                //   | Dimension4Code(Product Group)14 | ExDimension1Code(부서코드)15 | ExDimension2Code(사번)16 | ExDimension3Code(벤더)17 | ExDimension4Code(Product Group)18
                //  | VendorNM19 | ExVendorNM20 | GLAccount21 | GLAccountNM22 | TaxCode23 | TaxCodeNM24 | ExBankAccountCode25 | ExBankAccountNo26 | ExBankAccountCearing27
                //  | ExPostingGroup28 | BankAcountCode29 | BankAccountNo30 | BankAcountCearingID31 | PONo32 | CardApprovalNo33 | DocumentID34 | Status35
                info.Seq = Int32.Parse(seq);
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.LegacyKey = "";  // 비용초기 입력때는 발생하지 않음 , 전자결재 호출전 update 해준다.
                info.VoucherNo = arrDetail[0];
                info.CardExKind = arrDetail[1];
                info.DocumentDate = Convert.ToDateTime(arrDetail[3].ToString());
                info.PostingDate = Convert.ToDateTime(arrDetail[4].ToString());  // 임시
                info.DueDate = Convert.ToDateTime(arrDetail[5].ToString());      // 임시
                info.VendorDescription = arrDetail[6];
                info.ExDescription = arrDetail[7]; // 일단 VendorDescription 으로 같이 넣고 추후 해당 부분 요청 사항 나올 시 진행 
                if (arrDetail[8] != "")
                    info.DebitAmount1 = decimal.Parse(arrDetail[8].ToString().Replace(",", ""));// 임시 
                else
                    info.DebitAmount1 = 0;

                if (arrDetail[9] != "")
                    info.DebitAmount2 = decimal.Parse(arrDetail[9].ToString().Replace(",", "")); // 임시 
                else
                    info.DebitAmount2 = 0;

                if (arrDetail[10] != "")
                    info.CreditAmount = decimal.Parse(arrDetail[10].ToString().Replace(",", ""));  // 임시 
                else
                    info.CreditAmount = 0;

                info.PostingGroup = arrDetail[28]; // 임시
                info.Dimension1Code = arrDetail[11]; // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.Dimension2Code = arrDetail[12];              // 임시 - 사용자의 사번이여야 한다. 초기값으로 땡겨 와야 하는데 안 떙겨 오고 있음 벤더가 바뀌더라도 바뀌지 않음
                info.Dimension3Code = arrDetail[13];              // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.Dimension4Code = arrDetail[14];

                // 추가
                info.ExDimension1Code = arrDetail[15];
                info.ExDimension2Code = arrDetail[16];
                info.ExDimension3Code = arrDetail[17];
                info.ExDimension4Code = arrDetail[18];
                info.VendorNM = arrDetail[19];
                info.ExVendorNM = arrDetail[20];
                info.GLAccount = arrDetail[21];
                info.GLAccountNM = arrDetail[22];
                info.TaxCode = arrDetail[23];
                info.TaxCodeNM = arrDetail[24];
                info.ExBankAccountCode = arrDetail[25];
                info.ExBankAccountNo = arrDetail[26];
                info.ExBankAccountCearing = arrDetail[27];
                info.ExPostingGroup = arrDetail[28];

                info.BankAcountCode = arrDetail[29];         // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.BankAccountNo = arrDetail[30];                    // 임시 - 초기 혹은 바뀔떄 땡겨와야 하는데.. 안가져 오고 있음 
                info.BankAcountCearingID = arrDetail[31];   // 임시 - 벤더가 바뀌면 바뀌는 값이기 떄문에 바뀐 값이여야 한다. 우선 초기값 임시
                info.PONo = arrDetail[32];                           // 구매관련 스펙 확인시 진행
                info.CardApprovalNo = arrDetail[33];                   // 카드에서 진행
                info.DocumentID = Int32.Parse(arrDetail[34].ToString());                        // 초기 입력시는 0 결재상신후 update 대상 필드임
                info.Status = arrDetail[35];                        // Temp 로 초기 입력시 T

                //사업장 코드 - VENDOR_NUM - Ex)759206175                etc1
                //벤더명 - VENDor_NAme - 투썸플레이스서울스퀘어점        etc2  
                //대표자명 - vendor_person - 문윤민                      etc3
                //사업장주소 - Vendor_address1 + " " + Vendor_address2   etc4
                //증빙일  - Approve_date                                 etc5
                //사용시간 - approve_time                                etc6
                // 기타 필드
                info.Etc1 = arrDetail[36];
                info.Etc2 = arrDetail[37];
                info.Etc3 = arrDetail[38];
                info.Etc4 = arrDetail[39];
                info.Etc5 = arrDetail[40];
                info.Etc6 = arrDetail[41];
                info.Etc7 = arrDetail[42];
                info.Etc8 = arrDetail[42];
                info.Etc9 = arrDetail[44];
                info.Etc10 = arrDetail[45];

                biz = new DalLegacy();
                biz.ExpenseUserUpdate(info);


                if (arrDetail[0] == "")
                    voucherGubun = true;

            }

            // 상기로직 이후 VoucherNo Seq 를 Update 해준다.
            // 기존 바우처가 아닌 바우처 넘버가 새로 따진경우에만 update 해준다.
            if (voucherGubun)
            {

            }

            LoadExpenseData();
        }


        /// <summary>
        /// 비용전표에서 사용자 하단 Row 입력 저장
        /// </summary>
        private void AddCardUpdate()
        {
            string[] ArrCard = TmpData.Text.ToString().Split('★');
            string[] arrDetail = ArrCard[0].ToString().Split('|'); 

            string seq = ArrCard[1].ToString();

            DalLegacy biz = new DalLegacy();

            biz.CardUserUpdate(1, arrDetail[0].ToString(), arrDetail[1].ToString(), arrDetail[2].ToString(), arrDetail[3].ToString(), arrDetail[4].ToString(), seq);

            LoadExpenseData();
        }

        /// <summary>
        /// 작성자   : seo
        /// 작성일자 : 2011-01-09
        /// 작성목적 : 리스트 바인딩시 설정
        /// </summary>
        /// <param name="Sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void RepeaterEx_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //string Debugger;
                //Debugger = "";
                //System.Web.UI.HtmlControls.HtmlTableRow
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("DdlAuthManage"));
                //HtmlInputHidden hdnAuthValue = (HtmlInputHidden)e.Item.FindControl("HdnAuthValue") as HtmlInputHidden;
                //string appDataPath = AppSettingHelper.GetAppSetting(AppSettingType.Config, "AppDataPath", "");
                //appDataPath = Server.MapPath(appDataPath + "/FolderAuthType/" + CurrentUserInfo.PreferredLanguage + ".xml");
                //List<XmlDataInfo> al = Helpers.UtilHelper.XmlLoadData(appDataPath);

                //foreach (XmlDataInfo be in al)
                //{
                //    ddl.Items.Add(new ListItem(be.KeyText, be.KeyValue));
                //}

                //ddl.SelectedValue = hdnAuthValue.Value;

            }
        }


        /// <summary>
        /// 작성자   : seo
        /// 작성일자 : 2011-01-09
        /// 작성목적 : 리스트 바인딩시 설정
        /// </summary>
        /// <param name="Sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        protected void RepeaterEx2_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("DdlAuthManage"));
                //HtmlInputHidden hdnAuthValue = (HtmlInputHidden)e.Item.FindControl("HdnAuthValue") as HtmlInputHidden;
                //string appDataPath = AppSettingHelper.GetAppSetting(AppSettingType.Config, "AppDataPath", "");
                //appDataPath = Server.MapPath(appDataPath + "/FolderAuthType/" + CurrentUserInfo.PreferredLanguage + ".xml");
                //List<XmlDataInfo> al = Helpers.UtilHelper.XmlLoadData(appDataPath);

                //foreach (XmlDataInfo be in al)
                //{
                //    ddl.Items.Add(new ListItem(be.KeyText, be.KeyValue));
                //}

                //ddl.SelectedValue = hdnAuthValue.Value;
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        
        /// <summary>
        /// vouhcerNo 를 받아서 색상을 리턴한다.
        /// </summary>
        /// <param name="_voucherNo"></param>
        /// <returns></returns>
        public string VoucherColor(string _voucherNo)
        {

            if (proVoucher == _voucherNo)
            {
            }
            else
            {
                proVoucher = _voucherNo;

                if (proColor == "")
                {
                    proColor = "#eeeeee;";
                }
                else
                {
                    proColor = "";
                }
            }

            return proColor;
        }
        #endregion

        public string proColor
        {
            get { return GetViewStateString("proColor", ""); }
            set { ViewState["proColor"] = value; }
        }

        public string proVoucher
        {
            get { return GetViewStateString("proVoucher", ""); }
            set { ViewState["proVoucher"] = value; }
        }

        /// <summary>
        /// VoucherNo
        /// </summary>
        public string VoucherNo
        {
            get { return GetViewStateString("VoucherNo", ""); }
            set { ViewState["VoucherNo"] = value; }
        }

        public string TargetCardNo
        {
            get { return GetViewStateString("TargetCardNo", ""); }
            set { ViewState["TargetCardNo"] = value; }
        }

        public string TargetEtcCardNo
        {
            get { return GetViewStateString("TargetEtcCardNo", ""); }
            set { ViewState["TargetEtcCardNo"] = value; }
        }

        public string TargetCardKind
        {
            get { return GetViewStateString("TargetCardKind", ""); }
            set { ViewState["TargetCardKind"] = value; }
        }

        public string BankClearingID
        {
            get { return GetViewStateString("BankClearingID", ""); }
            set { ViewState["BankClearingID"] = value; }
        }

        public string BankAccountNo
        {
            get { return GetViewStateString("BankAccountNo", ""); }
            set { ViewState["BankAccountNo"] = value; }
        }

        public string DepartMentDimensionCD
        {
            get { return GetViewStateString("DepartMentDimensionCD", ""); }
            set { ViewState["DepartMentDimensionCD"] = value; }
        }

        public string ErpUserNM
        {
            get { return GetViewStateString("ErpUserNM", ""); }
            set { ViewState["ErpUserNM"] = value; }
        }

        public string BankAccountCD
        {
            get { return GetViewStateString("BankAccountCD", ""); }
            set { ViewState["BankAccountCD"] = value; }
        }

        public string VendorPostingGroup
        {
            get { return GetViewStateString("VendorPostingGroup", ""); }
            set { ViewState["VendorPostingGroup"] = value; }
        }

        public string VendorNM
        {
            get { return GetViewStateString("VendorNM", ""); }
            set { ViewState["VendorNM"] = value; }
        }

        public string VendorCD
        {
            get { return GetViewStateString("VendorCD", ""); }
            set { ViewState["VendorCD"] = value; }
        }

        public string PaymentTerm
        {
            get { return GetViewStateString("PaymentTerm", ""); }
            set { ViewState["PaymentTerm"] = value; }
        }

        public string DueDate
        {
            get { return GetViewStateString("DueDate", ""); }
            set { ViewState["DueDate"] = value; }
        }

        public string LegacyKey
        {
            get { return GetViewStateString("LegacyKey", ""); }
            set { ViewState["LegacyKey"] = value; }
        }

        public string ProductGroupStandard
        {
            get { return GetViewStateString("ProductGroupStandard", ""); }
            set { ViewState["ProductGroupStandard"] = value; }
        }


        public string CardDate
        {
            get { return GetViewStateString("CardDate", ""); }
            set { ViewState["CardDate"] = value; }
        }



    }


}