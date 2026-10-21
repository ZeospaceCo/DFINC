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
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model;

namespace Zeospace.DeskPlus.ETC.Web.Customize.Admin
{
    public partial class ListAdmin : Base.BasePageETC
    {
        #region 변수설정
        private string eventArgs = String.Empty;

        public string TempleteName = string.Empty;
        public string BatchName = string.Empty;

        public int TotalCount = 0; //총카운트
        public string RecordPerPage;// = new string(); //게시판 리스트 개수     
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Customize.Admin.ListAdmin).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                // 비용 데이터 조회 - 카드포함
                LoadExpenseData();

                LoadExpenseDataEnd();

                LoadDatailByVoucherNo("99999");

                // 기초데이터 로딩 Batch , Templete 
                LoadBatch();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadExpenseData()
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "P";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListAdmin(info);
            RepeaterEx1.DataSource = dtr;
            RepeaterEx1.DataBind();

            DataTableReader dtr2 = biz.GetExpenseUserListAdmin(info);
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

        }


        private void LoadExpenseFullData()
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "P";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListFullAdmin(info);
            RepeaterEx1.DataSource = dtr;
            RepeaterEx1.DataBind();

            DataTableReader dtr2 = biz.GetExpenseUserListFullAdmin(info);
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

        }


        private void LoadExpenseDataEnd()
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "C";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListAdmin(info);
            RepeaterEx3.DataSource = dtr;
            RepeaterEx3.DataBind();

            //DataTableReader dtr2 = biz.GetExpenseUserListAdmin(info);
            //int Total = 0;
            //if (dtr2 != null)
            //{
            //    if (dtr2.HasRows)
            //    {
            //        while (dtr2.Read())
            //        {
            //            Total += dtr2["CreditAmount"].ToString().Replace(",", "").ToNumber<int>();
            //        }
            //    }
            //}

            //this.TotalAmount.Text = Total.ToString();

        }


        private void LoadExpenseDataSearch(string _key, string _value)
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "P";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListAdminSearch(info, _key, _value);
            RepeaterEx1.DataSource = dtr;
            RepeaterEx1.DataBind();

            DataTableReader dtr2 = biz.GetExpenseUserListAdminSearch(info, _key, _value);
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

        }



        private void LoadExpenseDataSearchEnd(string _key, string _value)
        {
            // 사용자 입력 비용을 조회 해온다. 
            ExpenseUserInfo info = new ExpenseUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LegacyKey = "";
            info.PONo = "";
            info.CardApprovalNo = "";
            info.Status = "C";

            DalLegacy biz = new DalLegacy();
            DataTableReader dtr = biz.GetExpenseUserListAdminSearch(info, _key, _value);
            RepeaterEx3.DataSource = dtr;
            RepeaterEx3.DataBind();

        }


        private void LoadBatch()
        {
            CustomBiz.DalLegacy Biz = new CustomBiz.DalLegacy();
            DataSet dsUserBaseInfo = Biz.GetUserBatchInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dsUserBaseInfo != null && dsUserBaseInfo.Tables[0].Rows.Count > 0)
            {
                TempleteName = dsUserBaseInfo.Tables[0].Rows[0]["TempleteName"].ToString();
                BatchName = dsUserBaseInfo.Tables[0].Rows[0]["BatchName"].ToString();
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {

        }

        #endregion

        #region (함수)

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
            }

            ddlSearchField.Items.Clear();
            ddlSearchField.Items.Add(new ListItem("통합검색", "all"));
            ddlSearchField.Items.Add(new ListItem("Voucher No", "VoucherNo"));
            ddlSearchField.Items.Add(new ListItem("Vendor Name", "ExVendorNM"));
            ddlSearchField.Items.Add(new ListItem("상신자명", "Name"));
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
            RecordPerPage = GetDecryptParam("recordPerPage", "30");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "30";
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
                    case "Submit":  //비용 한줄 추가
                        Submit();
                        break;
                    case "Reject":  //카드 여러줄 추가 
                        Reject();
                        break;
                    case "LoadDatail":  //카드 여러줄 추가 
                        LoadDatail();
                        break;
                    case "PostingDate":  //posting date 수정 
                        UpdatePostingDate();
                        break;
                    case "DocumentDate":  //document date 수정 
                        UpdateDocumentDate();
                        break;
                    case "DueDate":  //Due date 수정 
                        UpdateDueDate();
                        break;
                    case "GLAccount":  //Due date 수정 
                        UpdateGLAccount();
                        break;
                    case "Vendor":  //Due date 수정 
                        UpdateVendor();
                        break;
                    case "PostingGroup":  
                        UpdatePostingGroup();
                        break;
                    case "Des":
                        UpdateDes();
                        break;
                    case "Bank":
                        UpdateBank();
                        break;
                    case "Department":
                        UpdateDepartment();
                        break;
                    case "Employee":
                        UpdateEmployee();
                        break;
                    case "ProductGroup":
                        UpdateProductGroup();
                        break;
                    case "Amount":
                        UpdateAmount();
                        break;
                    case "Search":
                        Search();
                        break;
                    case "fullDataLoad":
                        LoadExpenseFullData();
                        break;
                //    case "DelItem":  //카드 여러줄 추가 
                //        DelItem();
                //        break;
                //    //case "DelAuth":  //삭제
                //    //    DelAuthTemp();
                //    //    break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        private void Submit()
        {
            string[] ArrCard = TmpData.Text.ToString().Split('|')[0].Split(',');
// 승인 프로세스 - ERP I/F 로 넣어준다. 
            string TempleteNameVal = TmpData.Text.ToString().Split('|')[1];
            string BatchNameVal = TmpData.Text.ToString().Split('|')[2];
            DalLegacy biz;
            for (int i = 0; i < ArrCard.Length; i++)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("승인 VoucherNo=" + ArrCard[i].ToString());
                string rtnValue;
                biz = new DalLegacy();
                rtnValue = biz.CardExpenseIFInsert(1, ArrCard[i], TempleteNameVal, BatchNameVal);

                if (rtnValue == "1")
                {
                    biz.CardExpenseIFInsertbyUserUpdate(1, ArrCard[i],"C");
                }
            }
            LoadExpenseData();
            LoadExpenseDataEnd();
            LoadDatailByVoucherNo("99999");
        }

        private void Reject()
        {
            string[] ArrCard = TmpData.Text.ToString().Split(',');
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz;
            for (int i = 0; i < ArrCard.Length; i++)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("반려 VoucherNo=" + ArrCard[i].ToString());
                // 바우처 단위로 프로세스를 시행한다
                biz = new DalLegacy();
                biz.CardExpenseIFInsertbyUserUpdate(1, ArrCard[i], "T");
                biz.CardExpenseIFInsertbyUserUpdate2(1, ArrCard[i], "FT");

            }
            LoadExpenseData();
            LoadExpenseDataEnd();
            LoadDatailByVoucherNo("99999");
        }

        private void LoadDatail()
        {
            string VoutherNo = TmpData.Text.ToString();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();

            DataTable table = biz.CardExpenseTempSet(1, VoutherNo);

            //this.DocumentAmount.Text = MoneyString(table.Rows[table.Rows.Count - 1]["CreditAmount"].ToString());
            
            // 하단 디테일 뷰를 꾸며준다.
            RepeaterEx2.DataSource = table;
            RepeaterEx2.DataBind();


            
        }


        /// <summary>
        /// PostingData Update
        /// </summary>
        private void UpdatePostingDate()
        {
            string[] ArrData = TmpData.Text.ToString().Split('★');

            //seq_Kind_LineNo
            string [] arrKeys = ArrData[1].Split('_');

            string voucherNo = ArrData[2].ToString();
            string Date = ArrData[0].ToString();

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdatePostingDate(1, voucherNo, arrKeys[0], Date);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(voucherNo);
        }

        /// <summary>
        /// PostingData Update
        /// </summary>
        private void UpdateDocumentDate()
        {
            string[] ArrData = TmpData.Text.ToString().Split('★');

            //seq_Kind_LineNo
            string[] arrKeys = ArrData[1].Split('_');

            string voucherNo = ArrData[2].ToString();
            string Date = ArrData[0].ToString();


            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateDocumentDate(1, voucherNo, arrKeys[0], Date);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(voucherNo);
        }

        /// <summary>
        /// PostingData Update
        /// </summary>
        private void UpdateDueDate()
        {
            string[] ArrData = TmpData.Text.ToString().Split('★');

            //seq_Kind_LineNo
            string[] arrKeys = ArrData[1].Split('_');

            string voucherNo = ArrData[2].ToString();
            string Date = ArrData[0].ToString();

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateDueDate(1, voucherNo, arrKeys[0], Date);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(voucherNo);
        }


        /// <summary>
        /// PostingData Update
        /// </summary>
        private void UpdateGLAccount()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name;
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion 

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateGLAccount(1, valCode, valName, selVoucherNo, keySeq);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void UpdateVendor()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name; ,2=postingGroup
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();
            string postingGroup = arrValData[2].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateVendor(1, valCode, valName, selVoucherNo, keySeq, postingGroup);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void UpdatePostingGroup()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name; ,2=postingGroup
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            //string valName = arrValData[1].ToString();
            //string postingGroup = arrValData[2].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdatePostingGroup(1, valCode, selVoucherNo, keySeq);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void UpdateDes()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name; ,2=postingGroup
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            //string valName = arrValData[1].ToString();
            //string postingGroup = arrValData[2].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateDes(1, valCode, selVoucherNo, keySeq, keyKind);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }



        private void UpdateBank()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name; ,2=postingGroup : _code, _bankAccountName, _bankClearingID
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();
            string bankClearingID = arrValData[2].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateBank(1, valCode,valName,bankClearingID, selVoucherNo, keySeq, keyKind);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void UpdateDepartment()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name;
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateDepartment(1, valCode, valName, selVoucherNo, keySeq);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }

        public string MoneyString(string str)
        {
            string result = str;
            for (int i = str.Length - 3; i > 0; i = i - 3)
            {
                result = result.Insert(i, ",");

            }
            return result.Replace("-,","-");
        }


        private void UpdateAmount()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name;
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            decimal amount = 0;

            amount = Convert.ToDecimal(valCode.Replace(",", ""));



            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateAmount(1, selVoucherNo, keySeq, keyKind, amount);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }



        private void Search()
        {
            string[] arrData = TmpData.Text.ToString().Split('★');

            LoadExpenseDataSearch(arrData[0], arrData[1]);
            LoadExpenseDataSearchEnd(arrData[0], arrData[1]);
        }


        


        private void UpdateEmployee()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name;
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateEmployee(1, valCode, valName, selVoucherNo, keySeq);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void UpdateProductGroup()
        {
            #region 거의 공통 필드
            string[] arrData = TmpData.Text.ToString().Split('★');

            string[] arrValData = arrData[0].Split('|');  //  0 = Code , 1 = Name;
            string[] arrSelData = arrData[1].Split('|');  //  0 = SelVoucher , 1 = SelVendor , 2 = SelCardVendorKind , 3 = SelVal
            //seq_Kind_LineNo
            string[] arrKeyData = arrSelData[3].Split('_');

            string valCode = arrValData[0].ToString();
            string valName = arrValData[1].ToString();

            string selVoucherNo = arrSelData[0].ToString();
            string selCardVendorKind = arrSelData[1].ToString(); // V,C : 벤더,카드

            string keySeq = arrKeyData[0].ToString();
            string keyKind = arrKeyData[1].ToString(); // E , T , V  : 차,차,대 : 공급가,부가세,합계 를 의미 

            #endregion

            //string keys = TmpDatas.Split();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();
            biz.UpdateProductGroup(1, valCode, valName, selVoucherNo, keySeq);

            // 컨트롤에서 들어갔던 속성을 초기화 해준다. 
            // 리셋
            LoadExpenseData();
            LoadDatailByVoucherNo(selVoucherNo);
        }


        private void LoadDatailByVoucherNo(string _voucherNo)
        {
            string VoutherNo = TmpData.Text.ToString();
            // 승인 프로세스 - ERP I/F 로 넣어준다. 
            DalLegacy biz = new DalLegacy();

            DataTable table = biz.CardExpenseTempSet(1, _voucherNo);

            // 하단 디테일 뷰를 꾸며준다.
            RepeaterEx2.DataSource = table;
            RepeaterEx2.DataBind();

            //document amount
            if(_voucherNo != "99999")
                this.DocumentAmount.Text = MoneyString(table.Rows[table.Rows.Count - 1]["CreditAmount"].ToString());
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
        #endregion
    }
}