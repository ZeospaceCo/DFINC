<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>

<script language="C#" runat="server">
    //private string connEtc = ConfigurationManager.ConnectionStrings["SQLConnEtc"].ConnectionString;
    private string connErp = ConfigurationManager.ConnectionStrings["SQLConnERP"].ConnectionString;
    private string conn = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;

    // 개별변수값을 세팅 한다. 
    public string TotalAmount = string.Empty; // 합계
    
    // 벤더관련
    public string VenCD1 = " ";
    public string VenNM1 = " ";
    public string VenAmount1 = " ";
    public string VenCD2 = " ";
    public string VenNM2 = " ";
    public string VenAmount2 = " ";
    public string VenCD3 = " ";
    public string VenNM3 = " ";
    public string VenAmount3 = " ";
    public string VenCD4 = " ";
    public string VenNM4 = " ";
    public string VenAmount4 = " ";
    public string VenCD5 = " ";
    public string VenNM5 = " ";
    public string VenAmount5 = " ";
    public string VenCD6 = " ";
    public string VenNM6 = " ";
    public string VenAmount6 = " ";
    public string VenCD7 = " ";
    public string VenNM7 = " ";
    public string VenAmount7 = " ";
    public string VenCD8 = " ";
    public string VenNM8 = " ";
    public string VenAmount8 = " ";
    public string VenCD9 = " ";
    public string VenNM9 = " ";
    public string VenAmount9 = " ";
    public string VenCD10 = " ";
    public string VenNM10 = " ";
    public string VenAmount10 = " ";
    public string VenCD11 = " ";
    public string VenNM11 = " ";
    public string VenAmount11 = " ";
    public string VenCD12 = " ";
    public string VenNM12 = " ";
    public string VenAmount12 = " ";
    public string VenCD13 = " ";
    public string VenNM13 = " ";
    public string VenAmount13 = " ";
    public string VenCD14 = " ";
    public string VenNM14 = " ";
    public string VenAmount14 = " ";
    public string VenCD15 = " ";
    public string VenNM15 = " ";
    public string VenAmount15 = " ";
    public string VenCD16 = " ";
    public string VenNM16 = " ";
    public string VenAmount16 = " ";
    
    // 상단 카테고리 비용관련 
    public string AccNm1 = " ";
    public string AccAmount1 = " ";
    public string AccNm2 = " ";
    public string AccAmount2 = " ";
    public string AccNm3 = " ";
    public string AccAmount3 = " ";
    public string AccNm4 = " ";
    public string AccAmount4 = " ";
    public string AccNm5 = " ";
    public string AccAmount5 = " ";
    public string AccNm6 = " ";
    public string AccAmount6 = " ";
    public string AccNm7 = " ";
    public string AccAmount7 = " ";
    public string AccNm8 = " ";
    public string AccAmount8 = " ";
    public string AccNm9 = " ";
    public string AccAmount9 = " ";
    public string AccNm10 = " ";
    public string AccAmount10 = " ";
    public string AccNm11 = " ";
    public string AccAmount11 = " ";
    public string AccNm12 = " ";
    public string AccAmount12 = " ";
    public string AccNm13 = " ";
    public string AccAmount13 = " ";
    public string AccNm14 = " ";
    public string AccAmount14 = " ";
    public string AccNm15 = " ";
    public string AccAmount15 = " ";
    public string AccNm16 = " ";
    public string AccAmount16 = " ";
    public string AccNm17 = " ";
    public string AccAmount17 = " ";
    public string AccNm18 = " ";
    public string AccAmount18 = " ";
    public string AccNm19 = " ";
    public string AccAmount19 = " ";
    public string AccNm20 = " ";
    public string AccAmount20 = " ";
    public string AccNm21 = " ";
    public string AccAmount21 = " ";
    public string AccNm22 = " ";
    public string AccAmount22 = " ";
    public string AccNm23 = " ";
    public string AccAmount23 = " ";
    public string AccNm24 = " ";
    public string AccAmount24 = " ";
    public string AccNm25 = " ";
    public string AccAmount25 = " ";
    public string AccNm26 = " ";
    public string AccAmount26 = " ";
    public string AccNm27 = " ";
    public string AccAmount27 = " ";
    public string AccNm28 = " ";
    public string AccAmount28 = " ";
    public string AccNm29 = " ";
    public string AccAmount29 = " ";
    public string AccNm30 = " ";
    public string AccAmount30 = " ";
    public string AccNm31 = " ";
    public string AccAmount31 = " ";
    public string AccNm32 = " ";
    public string AccAmount32 = " ";
    public string AccNm33 = " ";
    public string AccAmount33 = " ";
    public string AccNm34 = " ";
    public string AccAmount34 = " ";
    public string AccNm35 = " ";
    public string AccAmount35 = " ";
    public string AccNm36 = " ";
    public string AccAmount36 = " ";

    public string itemCnt1;
    public string itemCnt2;
    public string itemCnt3;
    public string itemCnt4;
    public string itemCnt5;
    public string itemCnt6;
    public string itemCnt7;
    public string itemCnt8;
    public string itemCnt9;
    public string itemCnt10;
    public string itemCnt11;
    public string itemCnt12;
    public string itemCnt13;
    public string itemCnt14;
    public string itemCnt15;
    public string itemCnt16;
    
    public string ExVendorNM1;
    public string ExComNM1;
    public string DocumentDT1;
    public string PostingDT1;
    public string VendorDes1;
    public string ExVendorNM2;
    public string ExComNM2;
    public string DocumentDT2;
    public string PostingDT2;
    public string VendorDes2;
    public string ExVendorNM3;
    public string ExComNM3;
    public string DocumentDT3;
    public string PostingDT3;
    public string VendorDes3;
    public string ExVendorNM4;
    public string ExComNM4;
    public string DocumentDT4;
    public string PostingDT4;
    public string VendorDes4;
    public string ExVendorNM5;
    public string ExComNM5;
    public string DocumentDT5;
    public string PostingDT5;
    public string VendorDes5;
    public string ExVendorNM6;
    public string ExComNM6;
    public string DocumentDT6;
    public string PostingDT6;
    public string VendorDes6;
    public string ExVendorNM7;
    public string ExComNM7;
    public string DocumentDT7;
    public string PostingDT7;
    public string VendorDes7;
    public string ExVendorNM8;
    public string ExComNM8;
    public string DocumentDT8;
    public string PostingDT8;
    public string VendorDes8;
    public string ExVendorNM9;
    public string ExComNM9;
    public string DocumentDT9;
    public string PostingDT9;
    public string VendorDes9;
    public string ExVendorNM10;
    public string ExComNM10;
    public string DocumentDT10;
    public string PostingDT10;
    public string VendorDes10;

    public string CompanyNM;                                 

	private void Page_Load(object sender, System.EventArgs e)
    {
       if (!IsPostBack) 
       {
           
           itemCnt1 = "0";

           //UserInfoHelper user = new UserInfoHelper();
           CompanyNM = "먼디파마";// user.CompanyName;
           
           LoadData();
           
           Request.QueryString["ParamEtc"].ToString();
       }
    }

    private void LoadData()
    {
        if(Request["ParamEtc"] != null)
        {
            string ParamEtc = Request["ParamEtc"].ToString();
            
            //대상 비용 리스트를 가져온다. 
            DataSet dsExpenseList = GetExpenseList(ParamEtc);

            //한번더 가져온다. 피지컬 하게.. 삭제가 같이 영향을 받아서 어쩔 수 없이.. New 해도 그러네.. 
            DataSet dsExpenseListTax = GetExpenseList(ParamEtc);            
            
            //대상 계정과목 리스트를 가져온다. 
            DataSet dsAccountCalList = GetAccountCalList();
            
            //대상 벤더별 금액을 가져온다.
            DataSet deVendorAmountList = GetVendorAmountList(ParamEtc);

            //벤더금액 및 합계 금액을 세팅 해준다. 
            SetVendorTotal(deVendorAmountList);

            DetailInfo(dsExpenseList);

            SetExpenseManager(dsExpenseList, dsAccountCalList, dsExpenseListTax);  
        }
    }

    private void DetailInfo( DataSet _ex)
    {
        itemCnt1 = _ex.Tables[0].Rows.Count.ToString();

        ERPTable1.DataSource = _ex;
        ERPTable1.DataBind();        

        
    }


    private string LotMoney(string Num)
    {
        string mntyNum = string.Format("{0:#,0}", int.Parse(Num));
        string ComplateNum = string.Format("{0}", mntyNum);
        return ComplateNum;

    }
    
    
    private void SetVendorTotal(DataSet _ds)
    {
        int totalVal = 0;
        for(int i = 0 ; i < _ds.Tables[0].Rows.Count; i++)
        {
            // 각각 row 별로 하드코딩 필수 값을 세팅 해준다. 
            // 벤더의 경우 10개의 하드코딩 변수가 존재한다.
            if (i == 0)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD1 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM1 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount1 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 1)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD2 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM2 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount2 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 2)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD3 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM3 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount3 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 3)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD4 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM4 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount4 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 4)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD5 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM5 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount5 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 5)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD6 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM6 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount6 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 6)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD7 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM7 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount7 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 7)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD8 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM8 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount8 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 8)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD9 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM9 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount9 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 9)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD10 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM10 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount10 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 10)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD11 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM11 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount11 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 11)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD12 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM12 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount12 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 12)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD13 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM13 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount13 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 13)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD14 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM14 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount14 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 14)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD15 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM15 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount15 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }

            if (i == 15)
            {
                totalVal += Int32.Parse(_ds.Tables[0].Rows[i]["Amount"].ToString());
                VenCD16 = _ds.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                VenNM16 = _ds.Tables[0].Rows[i]["ExVendorNM"].ToString();
                VenAmount16 = LotMoney(_ds.Tables[0].Rows[i]["Amount"].ToString());
            }                                                                                                                                  
                                                          
        }

        TotalAmount = LotMoney(totalVal.ToString());
    }

    private void SetExpenseManager(DataSet _ex, DataSet _acc, DataSet ex)
    {
        string AccountCondition = string.Empty;
        string GLAccount = string.Empty;
        string Amount = string.Empty;
        string TaxCode = string.Empty;

        //DataSet _ex = new DataSet();
        //_ex = ex;
        
        for (int a = 0; a < _acc.Tables[0].Rows.Count; a++)
        {
            if (a == 0)
            {
                // 카테고리 이름 세팅
                AccNm1 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;
                
                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount1 == " ")
                    {
                        AccAmount1 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount1.Replace(",", "")) + AccountTaxTotal;
                        AccAmount1 = LotMoney(tmp.ToString());
                    }
                }
                                             
                
                // 비용에 대한 사항 
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        
                        AccountTotal += AccountAmount;
                    }
                }
                
                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount1 == " ")
                    {
                        AccAmount1 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount1.Replace(",", "")) + AccountTotal;
                        AccAmount1 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 1)
            {
                // 카테고리 이름 세팅
                AccNm2 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
 

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount2 == " ")
                    {
                        AccAmount2 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount2.Replace(",", "")) + AccountTaxTotal;
                        AccAmount2 = LotMoney(tmp.ToString());
                    }
                }                
                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount2 == " ")
                    {
                        AccAmount2 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount2.Replace(",", "")) + AccountTotal;
                        AccAmount2 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 2)
            {
                // 카테고리 이름 세팅
                AccNm3 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
   

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount3 == " ")
                    {
                        AccAmount3 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount3.Replace(",", "")) + AccountTaxTotal;
                        AccAmount3 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount3 == " ")
                    {
                        AccAmount3 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount3.Replace(",", "")) + AccountTotal;
                        AccAmount3 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 3)
            {
                // 카테고리 이름 세팅
                AccNm4 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount4 == " ")
                    {
                        AccAmount4 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount4.Replace(",", "")) + AccountTaxTotal;
                        AccAmount4 = LotMoney(tmp.ToString());
                    }
                }
                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount4 == " ")
                    {
                        AccAmount4 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount4.Replace(",", "")) + AccountTotal;
                        AccAmount4 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 4)
            {
                // 카테고리 이름 세팅
                AccNm5 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount5 == " ")
                    {
                        AccAmount5 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount5.Replace(",", "")) + AccountTaxTotal;
                        AccAmount5 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount5 == " ")
                    {
                        AccAmount5 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount5.Replace(",", "")) + AccountTotal;
                        AccAmount5 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 5)
            {
                // 카테고리 이름 세팅
                AccNm6 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
 
                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount6 == " ")
                    {
                        AccAmount6 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount6.Replace(",", "")) + AccountTaxTotal;
                        AccAmount6 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount6 == " ")
                    {
                        AccAmount6 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount6.Replace(",", "")) + AccountTotal;
                        AccAmount6 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 6)
            {
                // 카테고리 이름 세팅
                AccNm7 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
   
                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount7 == " ")
                    {
                        AccAmount7 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount7.Replace(",", "")) + AccountTaxTotal;
                        AccAmount7 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount7 == " ")
                    {
                        AccAmount7 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount7.Replace(",", "")) + AccountTotal;
                        AccAmount7 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 7)
            {
                // 카테고리 이름 세팅
                AccNm8 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
 
                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount8 == " ")
                    {
                        AccAmount8 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount8.Replace(",", "")) + AccountTaxTotal;
                        AccAmount8 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount8 == " ")
                    {
                        AccAmount8 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount8.Replace(",", "")) + AccountTotal;
                        AccAmount8 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 8)
            {
                // 카테고리 이름 세팅
                AccNm9 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
    

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount9 == " ")
                    {
                        AccAmount9 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount9.Replace(",", "")) + AccountTaxTotal;
                        AccAmount9 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount9 == " ")
                    {
                        AccAmount9 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount9.Replace(",", "")) + AccountTotal;
                        AccAmount9 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 9)
            {
                // 카테고리 이름 세팅
                AccNm10 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount10 == " ")
                    {
                        AccAmount10 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount10.Replace(",", "")) + AccountTaxTotal;
                        AccAmount10 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount10 == " ")
                    {
                        AccAmount10 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount10.Replace(",", "")) + AccountTotal;
                        AccAmount10 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 10)
            {
                // 카테고리 이름 세팅
                AccNm11 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount11 == " ")
                    {
                        AccAmount11 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount11.Replace(",", "")) + AccountTaxTotal;
                        AccAmount11 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount11 == " ")
                    {
                        AccAmount11 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount11.Replace(",", "")) + AccountTotal;
                        AccAmount11 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 11)
            {
                // 카테고리 이름 세팅
                AccNm12 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
   

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount12 == " ")
                    {
                        AccAmount12 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount12.Replace(",", "")) + AccountTaxTotal;
                        AccAmount12 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount12 == " ")
                    {
                        AccAmount12 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount12.Replace(",", "")) + AccountTotal;
                        AccAmount12 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 12)
            {
                // 카테고리 이름 세팅
                AccNm13 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
    

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount13 == " ")
                    {
                        AccAmount13 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount13.Replace(",", "")) + AccountTaxTotal;
                        AccAmount13 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount13 == " ")
                    {
                        AccAmount13 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount13.Replace(",", "")) + AccountTotal;
                        AccAmount13 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 13)
            {
                // 카테고리 이름 세팅
                AccNm14 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount14 == " ")
                    {
                        AccAmount14 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount14.Replace(",", "")) + AccountTaxTotal;
                        AccAmount14 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount14 == " ")
                    {
                        AccAmount14 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount14.Replace(",", "")) + AccountTotal;
                        AccAmount14 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 14)
            {
                // 카테고리 이름 세팅
                AccNm15 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount15 == " ")
                    {
                        AccAmount15 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount15.Replace(",", "")) + AccountTaxTotal;
                        AccAmount15 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount15 == " ")
                    {
                        AccAmount15 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount15.Replace(",", "")) + AccountTotal;
                        AccAmount15 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 15)
            {
                // 카테고리 이름 세팅
                AccNm16 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount16 == " ")
                    {
                        AccAmount16 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount16.Replace(",", "")) + AccountTaxTotal;
                        AccAmount16 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount16 == " ")
                    {
                        AccAmount16 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount16.Replace(",", "")) + AccountTotal;
                        AccAmount16 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 16)
            {
                // 카테고리 이름 세팅
                AccNm17 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount17 == " ")
                    {
                        AccAmount17 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount17.Replace(",", "")) + AccountTaxTotal;
                        AccAmount17 = LotMoney(tmp.ToString());
                    }
                }                
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount17 == " ")
                    {
                        AccAmount17 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount17.Replace(",", "")) + AccountTotal;
                        AccAmount17 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 17)
            {
                // 카테고리 이름 세팅
                AccNm18 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
         

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount18 == " ")
                    {
                        AccAmount18 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount18.Replace(",", "")) + AccountTaxTotal;
                        AccAmount18 = LotMoney(tmp.ToString());
                    }
                }
                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount18 == " ")
                    {
                        AccAmount18 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount18.Replace(",", "")) + AccountTotal;
                        AccAmount18 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 18)
            {
                // 카테고리 이름 세팅
                AccNm19 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
    

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount19 == " ")
                    {
                        AccAmount19 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount19.Replace(",", "")) + AccountTaxTotal;
                        AccAmount19 = LotMoney(tmp.ToString());
                    }
                }
                
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount19 == " ")
                    {
                        AccAmount19 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount19.Replace(",", "")) + AccountTotal;
                        AccAmount19 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 19)
            {
                // 카테고리 이름 세팅
                AccNm20 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
  
                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount20 == " ")
                    {
                        AccAmount20 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount20.Replace(",", "")) + AccountTaxTotal;
                        AccAmount20 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount20 == " ")
                    {
                        AccAmount20 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount20.Replace(",", "")) + AccountTotal;
                        AccAmount20 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 20)
            {
                // 카테고리 이름 세팅
                AccNm21 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
          

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount21 == " ")
                    {
                        AccAmount21 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount21.Replace(",", "")) + AccountTaxTotal;
                        AccAmount21 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount21 == " ")
                    {
                        AccAmount21 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount21.Replace(",", "")) + AccountTotal;
                        AccAmount21 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 21)
            {
                // 카테고리 이름 세팅
                AccNm22 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
         

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount22 == " ")
                    {
                        AccAmount22 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount22.Replace(",", "")) + AccountTaxTotal;
                        AccAmount22 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    AccAmount22 = LotMoney(AccountTotal.ToString());
                }
            }
            if (a == 22)
            {
                // 카테고리 이름 세팅
                AccNm23 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
  

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount23 == " ")
                    {
                        AccAmount23 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount23.Replace(",", "")) + AccountTaxTotal;
                        AccAmount23 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();
                    
                    if (ConditionAccount(GLAccount, AccountCondition))
                    {                        
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount23 == " ")
                    {
                        AccAmount23 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount23.Replace(",", "")) + AccountTotal;
                        AccAmount23 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 23)
            {
                // 카테고리 이름 세팅
                AccNm24 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
   

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount24 == " ")
                    {
                        AccAmount24 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount24.Replace(",", "")) + AccountTaxTotal;
                        AccAmount24 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount24 == " ")
                    {
                        AccAmount24 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount24.Replace(",", "")) + AccountTotal;
                        AccAmount24 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 24)
            {
                // 카테고리 이름 세팅
                AccNm25 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
        

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount25 == " ")
                    {
                        AccAmount25 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount25.Replace(",", "")) + AccountTaxTotal;
                        AccAmount25 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount25 == " ")
                    {
                        AccAmount25 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount25.Replace(",", "")) + AccountTotal;
                        AccAmount25 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 25)
            {
                // 카테고리 이름 세팅
                AccNm26 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
         

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount26 == " ")
                    {
                        AccAmount26 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount26.Replace(",", "")) + AccountTaxTotal;
                        AccAmount26 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount26 == " ")
                    {
                        AccAmount26 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount26.Replace(",", "")) + AccountTotal;
                        AccAmount26 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 26)
            {
                // 카테고리 이름 세팅
                AccNm27 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount27 == " ")
                    {
                        AccAmount27 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount27.Replace(",", "")) + AccountTaxTotal;
                        AccAmount27 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount27 == " ")
                    {
                        AccAmount27 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount27.Replace(",", "")) + AccountTotal;
                        AccAmount27 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 27)
            {
                // 카테고리 이름 세팅
                AccNm28 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
     

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount28 == " ")
                    {
                        AccAmount28 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount28.Replace(",", "")) + AccountTaxTotal;
                        AccAmount28 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount28 == " ")
                    {
                        AccAmount28 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount28.Replace(",", "")) + AccountTotal;
                        AccAmount28 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 28)
            {
                // 카테고리 이름 세팅
                AccNm29 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
       

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount29 == " ")
                    {
                        AccAmount29 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount29.Replace(",", "")) + AccountTaxTotal;
                        AccAmount29 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount29 == " ")
                    {
                        AccAmount29 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount29.Replace(",", "")) + AccountTotal;
                        AccAmount29 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 29)
            {
                // 카테고리 이름 세팅
                AccNm30 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount30 == " ")
                    {
                        AccAmount30 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount30.Replace(",", "")) + AccountTaxTotal;
                        AccAmount30 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount30 == " ")
                    {
                        AccAmount30 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount30.Replace(",", "")) + AccountTotal;
                        AccAmount30 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 30)
            {
                
                // 카테고리 이름 세팅
                AccNm31 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break


                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount31 == " ")
                    {
                        AccAmount31 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount31.Replace(",", "")) + AccountTaxTotal;
                        AccAmount31 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount31 == " ")
                    {
                        AccAmount31 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount31.Replace(",", "")) + AccountTotal;
                        AccAmount31 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 31)
            {
                // 카테고리 이름 세팅
                AccNm32 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    
                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount32 == " ")
                    {
                        AccAmount32 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount32.Replace(",", "")) + AccountTaxTotal;
                        AccAmount32 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount32 == " ")
                    {
                        AccAmount32 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount32.Replace(",", "")) + AccountTotal;
                        AccAmount32 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 32)
            {
                // 카테고리 이름 세팅
                AccNm33 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount33 == " ")
                    {
                        AccAmount33 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount33.Replace(",", "")) + AccountTaxTotal;
                        AccAmount33 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount33 == " ")
                    {
                        AccAmount33 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount33.Replace(",", "")) + AccountTotal;
                        AccAmount33 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 33)
            {
                // 카테고리 이름 세팅
                AccNm34 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break
                    

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount34 == " ")
                    {
                        AccAmount34 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount34.Replace(",", "")) + AccountTaxTotal;
                        AccAmount34 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount34 == " ")
                    {
                        AccAmount34 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount34.Replace(",", "")) + AccountTotal;
                        AccAmount34 = LotMoney(tmp.ToString());
                    }
                }
            }
            if (a == 34)
            {
                // 카테고리 이름 세팅
                AccNm35 = _acc.Tables[0].Rows[a]["Accounts Name"].ToString();
                AccountCondition = _acc.Tables[0].Rows[a]["Accounts Condition"].ToString();

                int AccountAmount = 0; int AccountTaxAmount = 0; string AccountTaxCode = "00";
                int AccountTotal = 0; int AccountTaxTotal = 0;

                // 비용전 부가세를 먼저 계산해서 가져온다.
                for (int t = 0; t < ex.Tables[0].Rows.Count; t++)
                {
                    AccountTaxAmount = Int32.Parse(ex.Tables[0].Rows[t]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountTaxCode = ex.Tables[0].Rows[t]["TaxCode"].ToString();

                    string TaxGL = "";

                    // 해당 없음의 경우 일단 break

                    if (AccountTaxCode == "01")
                    {
                        TaxGL = "1114100";
                    }

                    if (AccountTaxCode == "05" || AccountTaxCode == "06")
                    {
                        TaxGL = "2109900";
                    }

                    if (ConditionAccount(TaxGL, AccountCondition))
                    {
                        AccountTaxTotal += AccountTaxAmount;
                    }
                }

                // 계정과목에 부가세
                if (AccountTaxTotal != 0)
                {
                    if (AccAmount35 == " ")
                    {
                        AccAmount35 = LotMoney(AccountTaxTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount35.Replace(",", "")) + AccountTaxTotal;
                        AccAmount35 = LotMoney(tmp.ToString());
                    }
                }
                                
                for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
                {
                    GLAccount = _ex.Tables[0].Rows[e]["GLAccount"].ToString(); AccountTaxAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount2"].ToString().Split('.')[0]);
                    AccountAmount = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);AccountTaxCode = _ex.Tables[0].Rows[e]["TaxCode"].ToString();

                    if (ConditionAccount(GLAccount, AccountCondition))
                    {
                        _ex.Tables[0].Rows[e].Delete();_ex.Tables[0].AcceptChanges();e--;
                        AccountTotal += AccountAmount;
                    }
                }

                // 계정과목에 금액 세팅 
                if (AccountTotal != 0)
                {
                    if (AccAmount35 == " ")
                    {
                        AccAmount35 = LotMoney(AccountTotal.ToString());
                    }
                    else
                    {
                        int tmp = Int32.Parse(AccAmount35.Replace(",", "")) + AccountTotal;
                        AccAmount35 = LotMoney(tmp.ToString());
                    }
                }
                
            }
            
        }
        // 36번째는 Etc 로 이용
        // 상단에서 제거 되지 않은 것은 누락 본임으로 무조건 36번째 열에 잡아 준다.
        // 카테고리 이름 세팅
        AccNm36 = "Etc";// _acc.Tables[0].Rows[a]["Accounts Name"].ToString();

        int AccountAmountEtc = 0;
        int AccountTotalEtc = 0;
        for (int e = 0; e < _ex.Tables[0].Rows.Count; e++)
        {
            AccountAmountEtc = Int32.Parse(_ex.Tables[0].Rows[e]["Debit Amount1"].ToString().Split('.')[0]);
            AccountTotalEtc += AccountAmountEtc;
        }

        if (AccountTotalEtc != 0)
        {
            AccAmount36 = LotMoney(AccountTotalEtc.ToString());
        }
      
    }


    private bool ConditionAccount(string _glAccount, string _conditionVal)
    {
        bool rtnValue = false;

        if (_glAccount == "") return false;
        // 둘다 있는 경우 
        if (_conditionVal.IndexOf("..") > -1 && _conditionVal.IndexOf('|') > -1)
        {
            string[] arrValue = _conditionVal.Split('|');
            string contitionTemp = string.Empty;

            for (int ar = 0; ar < arrValue.Length; ar++)
            {
                if (arrValue[ar].IndexOf("..") > -1)
                {
                    contitionTemp = arrValue[ar];

                    string[] arr2Value = contitionTemp.Split('.');

                    int gl = Int32.Parse(_glAccount);
                    int formGL = Int32.Parse(arr2Value[0]);
                    int toGL = Int32.Parse(arr2Value[2]);

                    if (gl >= formGL && gl <= toGL)
                        rtnValue = true;                    
                }
                else if (arrValue[ar] == _glAccount)
                    rtnValue = true;
            }
        }
        else if (_conditionVal.IndexOf("..") > -1)
        {
            string[] arr2Value = _conditionVal.Split('.');

            int gl = Int32.Parse(_glAccount);
            int formGL = Int32.Parse(arr2Value[0]);
            int toGL = Int32.Parse(arr2Value[2]);
            
            if(gl >= formGL && gl <= toGL)
                rtnValue = true;
        }
        else if (_conditionVal.IndexOf('|') > -1)
        {
            string[] arrValue = _conditionVal.Split('|');

            for (int ar = 0; ar < arrValue.Length; ar++)
            {
                if (arrValue[ar] == _glAccount)
                    rtnValue = true;
            }
        }
        else
        {
            if (_conditionVal == _glAccount)
                rtnValue = true;
        }

        return rtnValue;
    }


    private DataSet GetExpenseList(string _seqs)
    {
        string strSql = @"select 
[UserID]  
           ,Seq  
           ,LegacyKey  
           ,VoucherNo  
           ,CardExKind 
           ,[Document Date]
           ,[Posting Date]  
           ,[Due Date]
           ,[VendorDescription]   
           ,[ExDescription]  
           ,case TaxCode when '05' then [Debit Amount2] + [Credit Amount] else [Debit Amount1] end as [Debit Amount1]
           ,case TaxCode when '05' then 0 - [Debit Amount2] else [Debit Amount2] end as [Debit Amount2]
           ,[Credit Amount]
           ,[Posting Group]
           ,[Dimension 1 Code]
           ,[Dimension 2 Code]
           ,[Dimension 3 Code]
           ,[Dimension 4 Code]
           ,ExDimension1Code   
           ,ExDimension2Code   
           ,ExDimension3Code   
           ,ExDimension4Code  
           ,VendorNM   
           ,ExVendorNM   
           ,GLAccount  
           ,GLAccountNM  
           ,TaxCode  
           ,TaxCodeNM  
           ,ExBankAccountCode   
           ,ExBankAccountNo   
           ,ExBankAccountCearing   
           ,ExPostingGroup     
           ,[Bank Acount Code]
           ,[Bank Account No.]  
           ,[Bank Acount Cearing ID]
           ,[PO No]
           ,[CardApprovalNo]  
           ,[DocumentID]  
           ,[Status]  
           ,isnull(etc1,'') as etc1
           ,isnull(etc2,'') as etc2
           ,isnull(etc3,'') as etc3
           ,isnull(etc4,'') as etc4
           ,isnull(etc5,'') as etc5
           ,isnull(etc6,'') as etc6
           ,isnull(etc7,'') as etc7
           ,isnull(etc8,'') as etc8
           ,isnull(etc9,'') as etc9
           ,isnull(etc10,'') as etc10           
           ,[WriteDate]                    
from [GW_Interface TableUser] where seq in (" + _seqs + ") order by voucherNo";

        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetExpenseList query=" + strSql);
        return SQLHelper.ExecuteDataSet(conn, CommandType.Text, strSql, "GetExpenseList", null);
    }
    
    
    private DataSet GetVendorAmountList(string _seqs)
    {
        string strSql = @"select ExDimension3Code, ExVendorNM,Convert(int,sum([Credit Amount])) as Amount
                          from [GW_Interface TableUser] where seq in (" + _seqs + ") group by ExDimension3Code, ExVendorNM ";
                         
        return SQLHelper.ExecuteDataSet(conn, CommandType.Text, strSql, "GetExpenseList", null);
    }    


    private DataSet GetAccountCalList()
    {
        string strSql = @"select * from [GW_Expense Account] where [Expense Type] = 0 order by [Accounts Name] ";
        return SQLHelper.ExecuteDataSet(connErp, CommandType.Text, strSql, "GetAccountCalList", null);
    }    


    [ComponentArtCallbackMethod]
    public string AjaxSetExpenseValue(string _legacyKey, string _ItemSeq)
    {
        string SqlTemp = "update [GW_Interface TableUser] set LegacyKey = '{0}', Status = '{1}' where Seq= '{2}'";
        string Sql = string.Empty;

        string itemSeq = _ItemSeq;
        
        try
        {

            itemSeq += ",";

            string[] arryItem = itemSeq.Split(',');

            for (int i = 0; i < arryItem.Length - 1; i++)
            {
                Sql = string.Format(SqlTemp, _legacyKey, "I", arryItem[i]);

                SQLHelper.ExecuteNonQuery(conn, CommandType.Text, Sql, null);
            }

        }
        catch (Exception ex)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AjaxSetExpenseValue", ex);
        }

        return "1";

    }


    [ComponentArtCallbackMethod]
    public string AjaxSetAftarCall(string _documentid)
    {

        try
        {
            string strSql = @"select top 1 etc1,etc2 from dp_app_docprocess where documentid =" + _documentid;
            DataSet ds = SQLHelper.ExecuteDataSet(conn, CommandType.Text, strSql, "IfTbl", null);

            string legacyKey = ds.Tables[0].Rows[0]["etc1"].ToString();

            string strSql2 = @"update [GW_Interface TableUser] set documentid = {0} where legacykey = '" + legacyKey + "'";
            strSql2 = string.Format(strSql2, _documentid);

            SQLHelper.ExecuteNonQuery(conn, CommandType.Text, strSql2, null);

        }
        catch (Exception ex)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AjaxSetExpenseValue", ex);
        }

        return "1";

    }


    public string ApprovalCardNo(string _cardApprovalNo)
    {
        string rtnValue = string.Empty;
        if (_cardApprovalNo != "" && _cardApprovalNo.IndexOf('_') > -1)
        {
            rtnValue = _cardApprovalNo.Split('_')[0].ToString();
        }
        
        return rtnValue;
    }

    public string CardNo(string _cardApprovalNo)
    {
        string rtnValue = string.Empty;
        if (_cardApprovalNo != "" && _cardApprovalNo.IndexOf('_') > -1)
        {
            rtnValue = _cardApprovalNo.Split('_')[1].ToString();
        }

        if (rtnValue != "")
        {
            rtnValue = rtnValue.Substring(0, 4) + "-****-" + rtnValue.Substring(8, 4) + "-" + rtnValue.Substring(12, 4);
        }

        return rtnValue;
    }


    public string MoneyFormat(string Amount)
    {
        string rtnValue = string.Empty;
        if (Amount.IndexOf('.') > -1)
        {
            rtnValue = LotMoney(Amount.Split('.')[0]);
        }
        return rtnValue;
    }

    public string DisplayFormat(string _kind)
    {
        string rtnValue = string.Empty;
        if (_kind == "V")
        {
            rtnValue = "none;";
        }
        return rtnValue;
    }

    public string DateFormat(string _val)
    {
        string rtnValue = string.Empty;

        if (_val != "")
        {
            rtnValue = _val.Substring(0, 10);
        }
        
        return rtnValue;
    }
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
<script language="javascript">

    var DOC = parent.ifrmForm.document;
    var section = parent.CustomSection;
    var isZD = true; 				// 발신여부 (발신true,수신false)

    var seqValues;
    var legacykey;
    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서로딩시, 초기화작업
    **********************************************************************************************/
    function init() {

        if (parent.documentID) { 
			//alert(parent.documentID); 
			onCompleteAfter(parent.documentID); return; 
		}

        switch (section.toUpperCase()) {
            case "ADD":

                seqValues = parent.ParamEtc;
                legacykey = parent.LegacyKey;

                //DOC.all("ENNAME").value = parent.UserEnName;

                var itemCnt1 = "<%= itemCnt1 %>";
                var itemCnt2 = "<%= itemCnt2 %>";
                var itemCnt3 = "<%= itemCnt3 %>";
                var itemCnt4 = "<%= itemCnt4 %>";
                var itemCnt5 = "<%= itemCnt5 %>";
                var itemCnt6 = "<%= itemCnt6 %>";
                var itemCnt7 = "<%= itemCnt7 %>";
                var itemCnt8 = "<%= itemCnt8 %>";
                var itemCnt9 = "<%= itemCnt9 %>";
                var itemCnt10 = "<%= itemCnt10 %>";
                var itemCnt11 = "<%= itemCnt11 %>";
                var itemCnt12 = "<%= itemCnt12 %>";
                var itemCnt13 = "<%= itemCnt13 %>";
                var itemCnt14 = "<%= itemCnt14 %>";
                var itemCnt15 = "<%= itemCnt15 %>";
                var itemCnt16 = "<%= itemCnt16 %>";
                //개별변수 세팅 
                //Vendor 
                DOC.all("VenCD1").innerText = "<%=VenCD1%>";
                DOC.all("VenNM1").innerText = "<%=VenNM1%>";
                DOC.all("VenAmount1").innerText = "<%=VenAmount1%>";

                DOC.all("VenCD2").innerText = "<%=VenCD2%>";
                DOC.all("VenNM2").innerText = "<%=VenNM2%>";
                DOC.all("VenAmount2").innerText = "<%=VenAmount2%>";

                DOC.all("VenCD3").innerText = "<%=VenCD3%>";
                DOC.all("VenNM3").innerText = "<%=VenNM3%>";
                DOC.all("VenAmount3").innerText = "<%=VenAmount3%>";

                DOC.all("VenCD4").innerText = "<%=VenCD4%>";
                DOC.all("VenNM4").innerText = "<%=VenNM4%>";
                DOC.all("VenAmount4").innerText = "<%=VenAmount4%>";

                DOC.all("VenCD5").innerText = "<%=VenCD5%>";
                DOC.all("VenNM5").innerText = "<%=VenNM5%>";
                DOC.all("VenAmount5").innerText = "<%=VenAmount5%>";

                DOC.all("VenCD6").innerText = "<%=VenCD6%>";
                DOC.all("VenNM6").innerText = "<%=VenNM6%>";
                DOC.all("VenAmount6").innerText = "<%=VenAmount6%>";

                DOC.all("VenCD7").innerText = "<%=VenCD7%>";
                DOC.all("VenNM7").innerText = "<%=VenNM7%>";
                DOC.all("VenAmount7").innerText = "<%=VenAmount7%>";

                DOC.all("VenCD8").innerText = "<%=VenCD8%>";
                DOC.all("VenNM8").innerText = "<%=VenNM8%>";
                DOC.all("VenAmount8").innerText = "<%=VenAmount8%>";

                DOC.all("VenCD9").innerText = "<%=VenCD9%>";
                DOC.all("VenNM9").innerText = "<%=VenNM9%>";
                DOC.all("VenAmount9").innerText = "<%=VenAmount9%>";

                DOC.all("VenCD10").innerText = "<%=VenCD10%>";
                DOC.all("VenNM10").innerText = "<%=VenNM10%>";
                DOC.all("VenAmount10").innerText = "<%=VenAmount10%>";

                DOC.all("VenCD11").innerText = "<%=VenCD11%>";
                DOC.all("VenNM11").innerText = "<%=VenNM11%>";
                DOC.all("VenAmount11").innerText = "<%=VenAmount11%>";

                DOC.all("VenCD12").innerText = "<%=VenCD12%>";
                DOC.all("VenNM12").innerText = "<%=VenNM12%>";
                DOC.all("VenAmount12").innerText = "<%=VenAmount12%>";

                DOC.all("VenCD13").innerText = "<%=VenCD13%>";
                DOC.all("VenNM13").innerText = "<%=VenNM13%>";
                DOC.all("VenAmount13").innerText = "<%=VenAmount13%>";

                DOC.all("VenCD14").innerText = "<%=VenCD14%>";
                DOC.all("VenNM14").innerText = "<%=VenNM14%>";
                DOC.all("VenAmount14").innerText = "<%=VenAmount14%>";

                DOC.all("VenCD15").innerText = "<%=VenCD15%>";
                DOC.all("VenNM15").innerText = "<%=VenNM15%>";
                DOC.all("VenAmount15").innerText = "<%=VenAmount15%>";

                DOC.all("VenCD16").innerText = "<%=VenCD16%>";
                DOC.all("VenNM16").innerText = "<%=VenNM16%>";
                DOC.all("VenAmount16").innerText = "<%=VenAmount16%>";

                // 토탈 금액
                DOC.all("AccTotal").innerText = "<%=TotalAmount%>";
                DOC.all("VenTotal").innerText = "<%=TotalAmount%>";


                // 비용 카테고리 세팅 
                DOC.all("AccNm1").innerText = "<%=AccNm1%>";
                DOC.all("AccAmount1").innerText = "<%=AccAmount1%>";
                DOC.all("AccNm2").innerText = "<%=AccNm2%>";
                DOC.all("AccAmount2").innerText = "<%=AccAmount2%>";
                DOC.all("AccNm3").innerText = "<%=AccNm3%>";
                DOC.all("AccAmount3").innerText = "<%=AccAmount3%>";
                DOC.all("AccNm4").innerText = "<%=AccNm4%>";
                DOC.all("AccAmount4").innerText = "<%=AccAmount4%>";
                DOC.all("AccNm5").innerText = "<%=AccNm5%>";
                DOC.all("AccAmount5").innerText = "<%=AccAmount5%>";
                DOC.all("AccNm6").innerText = "<%=AccNm6%>";
                DOC.all("AccAmount6").innerText = "<%=AccAmount6%>";
                DOC.all("AccNm7").innerText = "<%=AccNm7%>";
                DOC.all("AccAmount7").innerText = "<%=AccAmount7%>";
                DOC.all("AccNm8").innerText = "<%=AccNm8%>";
                DOC.all("AccAmount8").innerText = "<%=AccAmount8%>";
                DOC.all("AccNm9").innerText = "<%=AccNm9%>";
                DOC.all("AccAmount9").innerText = "<%=AccAmount9%>";
                DOC.all("AccNm10").innerText = "<%=AccNm10%>";
                DOC.all("AccAmount10").innerText = "<%=AccAmount10%>";
                DOC.all("AccNm11").innerText = "<%=AccNm11%>";
                DOC.all("AccAmount11").innerText = "<%=AccAmount11%>";
                DOC.all("AccNm12").innerText = "<%=AccNm12%>";
                DOC.all("AccAmount12").innerText = "<%=AccAmount12%>";
                DOC.all("AccNm13").innerText = "<%=AccNm13%>";
                DOC.all("AccAmount13").innerText = "<%=AccAmount13%>";
                DOC.all("AccNm14").innerText = "<%=AccNm14%>";
                DOC.all("AccAmount14").innerText = "<%=AccAmount14%>";
                DOC.all("AccNm15").innerText = "<%=AccNm15%>";
                DOC.all("AccAmount15").innerText = "<%=AccAmount15%>";
                DOC.all("AccNm16").innerText = "<%=AccNm16%>";
                DOC.all("AccAmount16").innerText = "<%=AccAmount16%>";
                DOC.all("AccNm17").innerText = "<%=AccNm17%>";
                DOC.all("AccAmount17").innerText = "<%=AccAmount17%>";
                DOC.all("AccNm18").innerText = "<%=AccNm18%>";
                DOC.all("AccAmount18").innerText = "<%=AccAmount18%>";
                DOC.all("AccNm19").innerText = "<%=AccNm19%>";
                DOC.all("AccAmount19").innerText = "<%=AccAmount19%>";
                DOC.all("AccNm20").innerText = "<%=AccNm20%>";
                DOC.all("AccAmount20").innerText = "<%=AccAmount20%>";
                DOC.all("AccNm21").innerText = "<%=AccNm21%>";
                DOC.all("AccAmount21").innerText = "<%=AccAmount21%>";
                DOC.all("AccNm22").innerText = "<%=AccNm22%>";
                DOC.all("AccAmount22").innerText = "<%=AccAmount22%>";
                DOC.all("AccNm23").innerText = "<%=AccNm23%>";
                DOC.all("AccAmount23").innerText = "<%=AccAmount23%>";
                DOC.all("AccNm24").innerText = "<%=AccNm24%>";
                DOC.all("AccAmount24").innerText = "<%=AccAmount24%>";
                DOC.all("AccNm25").innerText = "<%=AccNm25%>";
                DOC.all("AccAmount25").innerText = "<%=AccAmount25%>";
                DOC.all("AccNm26").innerText = "<%=AccNm26%>";
                DOC.all("AccAmount26").innerText = "<%=AccAmount26%>";
                DOC.all("AccNm27").innerText = "<%=AccNm27%>";
                DOC.all("AccAmount27").innerText = "<%=AccAmount27%>";
                DOC.all("AccNm28").innerText = "<%=AccNm28%>";
                DOC.all("AccAmount28").innerText = "<%=AccAmount28%>";
                DOC.all("AccNm29").innerText = "<%=AccNm29%>";
                DOC.all("AccAmount29").innerText = "<%=AccAmount29%>";
                DOC.all("AccNm30").innerText = "<%=AccNm30%>";
                DOC.all("AccAmount30").innerText = "<%=AccAmount30%>";
                DOC.all("AccNm31").innerText = "<%=AccNm31%>";
                DOC.all("AccAmount31").innerText = "<%=AccAmount31%>";
                DOC.all("AccNm32").innerText = "<%=AccNm32%>";
                DOC.all("AccAmount32").innerText = "<%=AccAmount32%>";
                DOC.all("AccNm33").innerText = "<%=AccNm33%>";
                DOC.all("AccAmount33").innerText = "<%=AccAmount33%>";
                DOC.all("AccNm34").innerText = "<%=AccNm34%>";
                DOC.all("AccAmount34").innerText = "<%=AccAmount34%>";
                DOC.all("AccNm35").innerText = "<%=AccNm35%>";
                DOC.all("AccAmount35").innerText = "<%=AccAmount35%>";
                DOC.all("AccNm36").innerText = "<%=AccNm36%>";
                DOC.all("AccAmount36").innerText = "<%=AccAmount36%>";

                // 합계 금액을 넣어 준다.
                DOC.all("tbHiddenTotal").value = "<%=TotalAmount%>";

                // 디테일 정보 
                var Header = "<div style=\"height:30px;\"></div><TABLE  border=0 cellSpacing=0 cellPadding=10 width=100% border = 0 height=\"20px\"><TBODY><TR><TD style=\"BORDER-BOTTOM: #000000 0px BORDER-TOP: #000000 0px BORDER-RIGHT: #000000 0px BORDER-LEFT: #000000 0px solid;FONT-FAMILY: 맑은 고딕;font-size:20px;font-weight:bold;text-align:center;\">Detail of Expenses<br></TD><TBODY></TABLE><p><p>";

                var itemTable1 = "";
                var itemTable2 = "<div style=\"height:5px;BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 0px solid;\"></div>";

                var itemTable = "";

                var TotalHtml = "";
                if (itemCnt1 > 0) {
                    for (i = 1; i <= itemCnt1; i++) {
                        itemTable1 = document.all("ERPTable1_Item_" + i).outerHTML;
                        itemTable += itemTable1 + itemTable2;
                    }
                    //var footerTable1 = document.all("ERPTable1_FooterTable").outerHTML;

                    TotalHtml += itemTable; // +footerTable1;
                }


                DOC.all("ERPTable1").outerHTML = Header + TotalHtml;


                //문서상신(발신) & 임시저장
                isZD = true;
                break;
            case "ADDRECEIVE":
                // 문서상신(수신)
                isZD = false;
                break;
            case "MOD":
                // 수정후재상신(발신)
                isZD = true;
                break;
            case "MODRECEIVE":
                // 수정후재상신(수신)
                isZD = false;
                break;
            case "MODCONTENT":
                //결재선 및 본문수정 (수발신)
                isZD = (parent.AccountTag.indexOf("ZQ") == 0) ? false : true;
                break;
        }
    }

    //상신전에 반드시 수행되어야 할 부분이 있다면 이부분에서 수행.
    function onComplete() {
        if (isZD = true && (section.toUpperCase() == "ADD" || section.toUpperCase() == "MOD" || section.toUpperCase() == "MODCONTENT")) {
            if (section.toUpperCase() == "ADD") {
                parent.document.getElementById("tbEtc1").value = legacykey;
                parent.document.getElementById("tbEtc2").value = seqValues;




                // 키에 대해서 update 처리 해준다.
                var rtnValue = AjaxSetExpenseValue(legacykey, seqValues);
            }
            //            var rlt = true;
            //            if (section.toUpperCase() == "MODCONTENT") //결재선 및 본문수정(수신)인 경우 제외
            //            {
            //                if (parent.AccountTag.indexOf("ZQ") == 0 || parent.AccountTag.indexOf("ZP") == 0) {
            //                    rlt = false;
            //                }
            //            }

            //            if (rlt) {
            //                if (DOC.getElementById("BODY_etc_EventName").value == "") {
            //                    alert("Event Name을 입력해 주세요.");
            //                    parent.ApprovalType = "";
            //                    DOC.getElementById("BODY_etc_EventName").focus();
            //                    return;
            //                }

            //            }
        }

        parent.AddSubmit(); 		//framework func
    }

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 상신이후 호출함수
    **********************************************************************************************/
    function onCompleteAfter(_docID) {
        var test = AjaxSetAftarCall(_docID);
		// 부모레이어에 대한 리플레쉬 처리 후 나머지 submit
        parent.parent.opener.AddMaster.ExReload();

        parent.SubmitAfterCallOK(); //framework func
    }


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 ClickEvent
    **********************************************************************************************/
    function docClickEvent(_event) {
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }

    }


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 KeyDownEvent
    **********************************************************************************************/
    function docKeyDownEvent(_event) {
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }

    }

    function docKeyUpEvent(_event) {
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }

    }

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 onChangeEvent
    **********************************************************************************************/
    function onChangeEvent(selObj) {

        tagId = selObj.id;
        if (tagId == "") { return; }
    }

</script>
</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">

    <asp:datalist id="ERPTable1" runat="server" width="100%">
    <HeaderTemplate>
    <TABLE  id="ERPTable1_HeaderTable" cellSpacing=0 cellPadding=0 width="100%" border=0 height="5px" >
        <tr>
            <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 0px solid;">&nbsp;  </td>
        </tr>  
    </table>
    </HeaderTemplate>
    <ItemTemplate>
    <TABLE class="tabitem" cellpadding="0" cellspacing="0" id="ERPTable1_Item_<%# Container.ItemIndex + 1 %>">
    <colgroup>
	    <col width="70px"></col>
	    <col width="70px"></col>
	    <col width="80px"></col>
	    <col width="135px"></col>
		<col width="60px"></col>
		<col width="60px"></col>
		<col width="60px"></col>
        <col /></col>
    </colgroup>
    <tbody>
    <tr>
	    <th colspan="3" style="BORDER-BOTTOM: #000000 1px solid;">비용/증빙정보 <%# Container.ItemIndex + 1 %>&nbsp;  (<%# DataBinder.Eval(Container.DataItem, "VoucherNo", "")%>)</th>
		<th colspan="3" style="BORDER-BOTTOM: #000000 1px solid;background:#ffffff;"></th>
    </tr>
    <tr height="10px">
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Date</td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Name</td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">G/L</td>
        <td style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">G/L Name</td>
		<td style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Total Amt.</td>
		<td style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Amt.</td>
		<td style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Tax</td>
        <td style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 1px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;BACKGROUND: #efefef;font-size:11px;text-align:center;">Description</td>
    </tr>
    <tr height="10px">
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:center;"><%# DateFormat(DataBinder.Eval(Container.DataItem, "Document Date", ""))%>&nbsp;  </td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:center;"><%# DataBinder.Eval(Container.DataItem, "ExVendorNM", "")%>&nbsp;  </td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:center;"><%# DataBinder.Eval(Container.DataItem, "GLAccount", "")%>&nbsp;  </td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:left;"><%# DataBinder.Eval(Container.DataItem, "GLAccountNM", "")%>&nbsp;  </td>
		<td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:right;"><%# MoneyFormat(DataBinder.Eval(Container.DataItem, "Credit Amount", ""))%>&nbsp;  </td>
		<td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:right;"><%# MoneyFormat(DataBinder.Eval(Container.DataItem, "Debit Amount1", ""))%>&nbsp;  </td>
		<td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:right;"><%# MoneyFormat(DataBinder.Eval(Container.DataItem, "Debit Amount2", ""))%>&nbsp;  </td>
        <td style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 1px solid;  BORDER-BOTTOM: #000000 1px solid; BORDER-LEFT: #000000 1px solid;font-size:11px;text-align:left;"><%# DataBinder.Eval(Container.DataItem, "ExDescription", "")%>&nbsp;  </td>
    </tr>
    <tr style="height:3px">
        <td colspan="8" style="BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 1px solid;  BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 1px solid;"></td>
    </tr>
    <tr style="display:<%# DisplayFormat(DataBinder.Eval(Container.DataItem, "CardExKind", ""))%>">
        <td colspan="8" style="BORDER-TOP: #000000 0px solid; BORDER-RIGHT: #000000 0px solid;  BORDER-BOTTOM: #000000 0px solid; BORDER-LEFT: #000000 0px solid;padding:0px 0px;">
            <table cellSpacing=0 cellPadding=0 width="100%">
            <colgroup>
	            <col width="70px"></col>
	            <col width="70px"></col>
	            <col width="80px"></col>
	            <col width="135px"></col>
                <col width="126px"></col>
	            <col /></col>  
            </colgroup>
            <tbody>
                <tr height="10px">
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;BACKGROUND: #efefef;text-align:center;">카드승인번호</td>
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;text-align:center;"><%# ApprovalCardNo(DataBinder.Eval(Container.DataItem, "CardApprovalNo", ""))%>&nbsp;   </td>
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;BACKGROUND: #efefef;text-align:center;">벤더명</td>
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;text-align:left;" ><%# DataBinder.Eval(Container.DataItem, "Etc2", "")%>&nbsp;   </td>
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;BACKGROUND: #efefef;text-align:center;">증빙일</td>
	                <td style="BORDER-TOP: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid;font-size:11px;text-align:left;" class="rd"><%# DataBinder.Eval(Container.DataItem, "Etc5", "")%>&nbsp; <%# DataBinder.Eval(Container.DataItem, "Etc6", "")%>  </td>
                </tr>
                <tr height="10px">
	                <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;BACKGROUND: #efefef;text-align:center;">사용금액</td>
	                <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;text-align:right;"><%# MoneyFormat(DataBinder.Eval(Container.DataItem, "Credit Amount", ""))%>&nbsp;   </td>
                    <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;BACKGROUND: #efefef;text-align:center;">카드번호</td>
	                <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;text-align:center;"><%# CardNo(DataBinder.Eval(Container.DataItem, "CardApprovalNo", ""))%>&nbsp;   </td>
                    <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;BACKGROUND: #efefef;text-align:center;">사업장주소</td>
	                <td style="BORDER-BOTTOM: #000000 1px solid;BORDER-TOP: #000000 0px;font-size:11px;text-align:left;" class="rd"><%# DataBinder.Eval(Container.DataItem, "Etc4", "")%>&nbsp;   </td>
                </tr>
            </tbody>
            </table>
        </td>
    </tr>
    </tbody>
    </TABLE>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
    </asp:datalist>
    </form>
</body>
</html>
