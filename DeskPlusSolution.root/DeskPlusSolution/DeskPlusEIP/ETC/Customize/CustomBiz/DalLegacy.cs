using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz.Model;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;


namespace Zeospace.DeskPlus.ETC.Web.Customize.CustomBiz
{
    public class DalLegacy : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        private string connErp = ConfigurationManager.ConnectionStrings["SQLConnERP"].ConnectionString;

        #region GET()
        public DataSet GetUserBaseInfo(int _companyID, int _userID)
        {
            string UserCode = string.Empty;

            // 쿠키에 userCode 만 있었어도 안 갔다 와도 되는데...
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "select usercode from dp_acc_user where userid = {0} and activeuser = 1";
                query = string.Format(query, _userID.ToString());

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                UserCode = ExecuteScalar(cmd).ToString();
            }

            // Erp 측 접속 해서 필요한 정보를 가져온다.
            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select top 1 V.[No_] as VendorCode, V.Name as VendorNM , V.[Vendor Posting Group] as VendorPostingGroup,
                                       isnull(BA.Code,'') as BankAccountCode, isnull(BA.Name,'') as BankAccountNM, US.[User ID] as ErpUserNM, US.[Shortcut Dimension 1 Code] as DepartMentDimentionCD,
                                       isnull(BA.[Bank Clearing ID],'') as BankClearingID, V.[PayMent Terms Code] as PaymentTerm, isnull(replace(BA.[Bank Account No_], char(13)+char(10), ''),'') as BankAccountNo
                                      ,(select top 1 Name from [GW_Dimension Value] where [Dimension Code] = 'DEPARTMENT' and Code = US.[Shortcut Dimension 1 Code]) as DepartMentNM
                                      ,(select top 1 Name from [GW_Dimension Value] where [Dimension Code] = 'EMP' and Code = US.[Shortcut Dimension 2 Code]) as EmpNM
                                from [GW_Vendor] V Left outer Join [GW_Vendor Bank Account] BA
                                   on V.[No_] = BA.[Vendor No_] 
                                   inner join [GW_User Setup] US
                                   on V.[No_] = US.[Shortcut Dimension 2 Code]
                                 where V.[No_] = '{0}' and V.Blocked = 0  
                                 order by BA.Code Asc";   // 여러개의 계정이 존재 할 시 Bank Account 를 우선 비용부터 가져온다. 난중에 요구사항이 바뀔지 모르지만. EXP . CARD 두가지라면 Card 가 먼저
                query = string.Format(query, UserCode);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                return ExecuteDataSet(cmd, "GetErpUserInfo");
            }

            
        }


        public DataSet GetUserBatchInfo(int _companyID, int _userID)
        {
            string UserCode = string.Empty;

            // 쿠키에 userCode 만 있었어도 안 갔다 와도 되는데...
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "select usercode from dp_acc_user where userid = {0} and activeuser = 1";
                query = string.Format(query, _userID.ToString());

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                UserCode = ExecuteScalar(cmd).ToString();
            }

            // Erp 측 접속 해서 필요한 정보를 가져온다.
            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select [Journal Template Name] as TempleteName, [Journal Batch Name] as BatchName  from dbo.[GW_Gen_ Journal User Setup]
where [Shortcut Dimension 2 Code] = '{0}'";   // 여러개의 계정이 존재 할 시 Bank Account 를 우선 비용부터 가져온다. 난중에 요구사항이 바뀔지 모르지만. EXP . CARD 두가지라면 Card 가 먼저
                query = string.Format(query, UserCode);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                return ExecuteDataSet(cmd, "GetErpBatchInfo");
            }


        }


        /// <summary>
        /// 타인의 카드 정보를 로딩 해준다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_venCD"></param>
        /// <returns></returns>
        public string GetUserAnotherBasicInfo(int _companyID, string _venCD)
        {
            string rtnValue = "!";

            // EP 에서 코드에 맞는 카드 정보를 가져온다.
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"select top 1 cardnumber from BS_GW_CardInfo where userid = (select top 1 userid from dp_acc_user where usercode = '{0}' and activeuser = 1)  order by issec desc";
                //string query = "select * from dbo.BS_GW_CardInfo where UserID = " + _userID.ToString() + " order by isSec ";
                query = string.Format(query, _venCD);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                DataSet ds = ExecuteDataSet(cmd, "Get");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    rtnValue = ds.Tables[0].Rows[0][0].ToString();
                }
            }

            return rtnValue;
        }

        /// <summary>
        /// 밴더 팝업 페이징 가져오기
        /// </summary>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListVendor(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _masterDetailGubun)
        {

//            using (var cn = new SqlConnection(connErp))
//            {
//                string query = @"select top 20 No_ as VendorCode, Name, [Vendor Posting Group] as VendorPostingGroup,isnull([Payment Terms Code],'') as PaymentTerm from [GW_Vendor]     
//                                where Blocked = '0'";

//                if(_masterDetailGubun == "M")
//                    query +=  "and No_ like 'E%'";

//                string searchValue = string.Empty;

//                if (_searchValue != "")
//                {
//                    // 통합검색인경우 
//                    if (_searchKey == "all")
//                    {
//                        searchValue = " and (No_ like '%" + _searchValue + "%' or Name like '%" + _searchValue + "%')";
//                    }
//                    else
//                    {
//                        searchValue = " and " + _searchKey + "like '%" + _searchValue + "%'";
//                    }
//                }

//                query = query + searchValue;

//                var cmd = new SqlCommand(query, cn);
//                cmd.CommandType = CommandType.Text;

//                cn.Open();

//                return ExecuteDataTableReader(cmd);

//            }

            using (var cn = new SqlConnection(connErp))
            {
                var cmd = new SqlCommand("Site_ERP_Vendor_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@MasterDetailGubun", SqlDbType.Char,1).Value = _masterDetailGubun;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        /// <summary>
        /// 벤더팝업 선택 이후 나머지 값을 Ajax 로 가져오는 값
        /// </summary>
        /// <param name="_vendorCD"></param>
        /// <returns></returns>
        public string GetVendorEtc(string _vendorCD)
        {
            string rtnValue = "|||";

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select top 1                     
                             BA.Code as BankAccountCode,  (select top 1 isnull([Shortcut Dimension 1 Code],'') from [GW_User Setup] where [Shortcut Dimension 2 Code] = BA.[Vendor No_]) as DepartMentDimentionCD,  
                             BA.[Bank Clearing ID] as BankClearingID, BA.[Bank Account No_] as BankAccountNo  
                            from [GW_Vendor Bank Account] BA  
                            where BA.[Vendor No_] = '{0}'";


//on BA.[Vendor No_]  = US.[Shortcut Dimension 2 Code]  

                query = string.Format(query, _vendorCD);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                DataSet ds = ExecuteDataSet(cmd, "GetVendorEtcInfo");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    rtnValue = ds.Tables[0].Rows[0]["BankAccountCode"].ToString() + "|" + ds.Tables[0].Rows[0]["DepartMentDimentionCD"].ToString() + "|" + ds.Tables[0].Rows[0]["BankClearingID"].ToString() + "|" + ds.Tables[0].Rows[0]["BankAccountNo"].ToString();
                }
            }

            return rtnValue;
        }


        public string GetSysCode(string _vendorCD)
        {
            string rtnValue = "!";

            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                string query = "select usercode from dp_acc_user where usercode = '" + _vendorCD + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                DataSet ds =  ExecuteDataSet(cmd, "GetErpSysInfo");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    rtnValue = ds.Tables[0].Rows[0][0].ToString();
                }
            }

            return rtnValue;
        }


        public DataSet GetMyCardInfo(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "select * from dbo.BS_GW_CardInfo where UserID = " + _userID.ToString() + " order by isSec ";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                return ExecuteDataSet(cmd, "GetErpUserInfo");
            }
        }


        public DataSet GetProductGroupStand()
        {
            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select No_ from  dbo.[GW_GL Account]
where [Gen_ Journal Account] = 1
  and No_ 
  in 
  (select No_ from dbo.[GW_Default Dimension]
where [dimension code] = 'PRODUCT GROUP'
 and [Value Posting] = 1
 and [Table ID] = 15)";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                return ExecuteDataSet(cmd, "ProductGroupStand");
            }
        }


        /// <summary>
        /// 카드가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListCard(int _companyID, string _cardNum, string _cardDate)
        {

            string CardDate = SetCardDate(_cardDate);

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"select top 30 * from (
SELECT  replace([APPROVE_NUM],' ','') + '_' + replace([CARD_NUM],' ','') + '_' + replace([GUBUN],' ','') as seq
		,substring([TRANS_DATE],1,4) + '-' + substring([TRANS_DATE],5,2) + '-' + substring([TRANS_DATE],7,2) as [TRANS_DATE]
		,[GUBUN]
		,substring([APPROVE_DATE],1,4) + '-' + substring([APPROVE_DATE],5,2) + '-' + substring([APPROVE_DATE],7,2) as [APPROVE_DATE]
		,substring([APPROVE_TIME],1,2) + ':' + substring([APPROVE_TIME],3,2) + ':' + substring([APPROVE_TIME],5,2) as [APPROVE_TIME]
		,[BUY_DATE]
		,[CHARGE_DATE]
		,[CARD_NUM]
		,[APPROVE_NUM]
		,[DEAL_DATE]
		,[CURRENCY]
,CASE CURRENCY WHEN 'KRW' THEN
	REPLACE(CONVERT(VARCHAR, convert(money,([APPROVE_TOTAL] - isnull((select top 1 [APPROVE_TOTAL] from [approval_data] where gubun = '02' and card_num = '{0}' and [APPROVE_NUM] = MM.[APPROVE_NUM]),0))), 1),'.00','') 
ELSE
	REPLACE(CONVERT(VARCHAR, convert(money,(isnull((select top 1 [APPROVE_TOTAL] from acquire_data where  CARD_NUM = MM.CARD_NUM and APPROVE_NUM = MM.APPROVE_NUM),0))), 1),'.00','') 
END as [APPROVE_AMT]
		,[TAX]
		,[SERVICE_CHARGE]
		,[APPROVE_TOTAL]
		,[BUY_COLLECTION_NUM]
		,[BUY_EXCHANGE_RATE]
		,[USD_AMT]
		,[KRW_AMT]
		,[PAY_DATE]
		,[USE_TYPE]
		,[PAY_TOTAL]
		,[PAY_AMT]
		,[PAY_INTEREST]
		,[FEE]
		,[MEMBERSHIP_FEE]
		,[INSTALLMENT_MONTHS]
		,[REMAIN_INSTALLMENT_MONTHS]
		,[VENDOR_NUM]
		,[ABROAD_USE]
		,[ABROAD_CURRENCY]
		,[VENDOR_TAX_NUM]
		,[VENDOR_NAME]
		,[VENDOR_PERSON]
		,[VENDOR_TEL]
		,[VENDOR_ADDRESS1]
		,[VENDOR_ADDRESS2]
		,[VENDOR_CLOSE_INFO]
		,[MMC_CODE]
		,[VENDOR_ZIP_CODE]
		,[MMC_NAME]
		,[ACTUAL_PLACE_AMT]
FROM [approval_data]  MM
where card_num = '{1}'
and isnull(Status,'') <> 'Y'
and gubun = '01'
--and approve_date >= '{2}'
) a where [APPROVE_AMT] <> '0' order by TRANS_DATE";

                query = string.Format(query, _cardNum, _cardNum, CardDate);
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        public DataTableReader GetPageListCardPe(int _companyID, string _cardNum, string _from, string _to)
        {
            DateTime fromDate = DateTime.Parse(_from);

            fromDate = fromDate.AddDays(-1); // between 특성상 당일이 안탐.. 그래서 하루 뺌.. 꼼수

            string fromdateStr = fromDate.ToShortDateString();
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"select top 30 * from (
SELECT  replace([APPROVE_NUM],' ','') + '_' + replace([CARD_NUM],' ','') + '_' + replace([GUBUN],' ','') as seq
		,substring([TRANS_DATE],1,4) + '-' + substring([TRANS_DATE],5,2) + '-' + substring([TRANS_DATE],7,2) as [TRANS_DATE]
		,[GUBUN]
		,substring([APPROVE_DATE],1,4) + '-' + substring([APPROVE_DATE],5,2) + '-' + substring([APPROVE_DATE],7,2) as [APPROVE_DATE]
		,substring([APPROVE_TIME],1,2) + ':' + substring([APPROVE_TIME],3,2) + ':' + substring([APPROVE_TIME],5,2) as [APPROVE_TIME]
		,[BUY_DATE]
		,[CHARGE_DATE]
		,[CARD_NUM]
		,[APPROVE_NUM]
		,[DEAL_DATE]
		,[CURRENCY]
,CASE CURRENCY WHEN 'KRW' THEN
	REPLACE(CONVERT(VARCHAR, convert(money,([APPROVE_TOTAL] - isnull((select top 1 [APPROVE_TOTAL] from [approval_data] where gubun = '02' and card_num = '{0}' and [APPROVE_NUM] = MM.[APPROVE_NUM]),0))), 1),'.00','') 
ELSE
	REPLACE(CONVERT(VARCHAR, convert(money,(isnull((select top 1 [APPROVE_TOTAL] from acquire_data where  CARD_NUM = MM.CARD_NUM and APPROVE_NUM = MM.APPROVE_NUM),0))), 1),'.00','') 
END as [APPROVE_AMT]
		,[TAX]
		,[SERVICE_CHARGE]
		,[APPROVE_TOTAL]
		,[BUY_COLLECTION_NUM]
		,[BUY_EXCHANGE_RATE]
		,[USD_AMT]
		,[KRW_AMT]
		,[PAY_DATE]
		,[USE_TYPE]
		,[PAY_TOTAL]
		,[PAY_AMT]
		,[PAY_INTEREST]
		,[FEE]
		,[MEMBERSHIP_FEE]
		,[INSTALLMENT_MONTHS]
		,[REMAIN_INSTALLMENT_MONTHS]
		,[VENDOR_NUM]
		,[ABROAD_USE]
		,[ABROAD_CURRENCY]
		,[VENDOR_TAX_NUM]
		,[VENDOR_NAME]
		,[VENDOR_PERSON]
		,[VENDOR_TEL]
		,[VENDOR_ADDRESS1]
		,[VENDOR_ADDRESS2]
		,[VENDOR_CLOSE_INFO]
		,[MMC_CODE]
		,[VENDOR_ZIP_CODE]
		,[MMC_NAME]
		,[ACTUAL_PLACE_AMT]
FROM [approval_data]  MM
where card_num = '{1}'
and isnull(Status,'') <> 'Y'
and gubun = '01'
--and (approve_date between '{2}' and '{3}')
) a where [APPROVE_AMT] <> '0' order by TRANS_DATE";

                query = string.Format(query, _cardNum, _cardNum, fromdateStr, _to);
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        public string SetCardDate(string _PaymentTerm)
        {
            string rtnValue = string.Empty;
            DateTime currentDate = DateTime.Now;
            int AddDate = 0;

            if (_PaymentTerm.IndexOf('D') > -1)
            {
                AddDate = AddDate - Int32.Parse(_PaymentTerm.Split('D')[0].ToString()) * 1;

                currentDate = currentDate.AddDays(AddDate);
            }

            if (_PaymentTerm.IndexOf('M') > -1)
            {
                AddDate = AddDate - Int32.Parse(_PaymentTerm.Split('M')[0].ToString()) * 1;

                currentDate = currentDate.AddMonths(AddDate);
            }

            if (_PaymentTerm == "")
            {
                currentDate = currentDate.AddDays(-40);
            }

            rtnValue = currentDate.ToShortDateString().Replace("-","");

            return rtnValue;
        }


        /// <summary>
        /// Bank Account 팝업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListBankAccount(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _vendorCD)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select [Vendor No_] as VendorCD, Code , Name, [Bank Account No_] as BankAccountCD, [Bank Clearing ID] as BankClearingID from [GW_Vendor Bank Account] where [Vendor No_] = '{0}'";

                query = string.Format(query, _vendorCD);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// Bank Account 팝업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListBatch(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"SELECT [Journal Template Name] as TempleteName
      ,[Name] as BatchName
      ,[Description]
  FROM [GW_Gen_ Journal Batch]
where [Journal Template Name] in 
(SELECT [Name]
  FROM [GW_Gen_ Journal Template]
   where [form id] = 39)";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// Posting Group
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListPostingGroup(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _vendorCD)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select * from [GW_Vendor Posting Group]";

                query = string.Format(query, _vendorCD);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        /// <summary>
        /// G/L Account 팝업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListGLAccount(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select No_ as GLAccountNo, Name as GLAccountName from (
                                select No_ , Name  , 1 as seq from [GW_GL Account] 
                                where [Gen_ Journal Account] = 1
                                  and No_ like '5%'
                                union all
                                select No_ , Name , 2 as seq from [GW_GL Account] 
                                where [Gen_ Journal Account] = 1
                                  and No_ like '2%'
                                union all
                                select No_ , Name , 3 as seq from [GW_GL Account] 
                                where [Gen_ Journal Account] = 1
                                  and No_ like '1%'
                                union all
                                select No_ , Name , 4 as seq from [GW_GL Account] 
                                where [Gen_ Journal Account] = 1
                                  and No_ like '6%') tmp
                                where 1=1";

                string qOrder = " order by seq, No_";

                string searchValue = string.Empty; 

                if (_searchValue != "")
                {
                    // 통합검색인경우 
                    if (_searchKey == "all")
                    {
                        searchValue = " and (No_ like '%" + _searchValue + "%' or Name like '%" + _searchValue + "%')";
                    }
                    else
                    {
                        searchValue = " and " + _searchKey + "like '%" + _searchValue + "%'";
                    }
                }

                query = query + searchValue + qOrder;

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// Pruduct Group
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListProductGroup(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select * from [GW_Dimension Value] where [Dimension Code]  = 'PRODUCT GROUP' and Blocked = 0 and [Dimension Value Type] =  0";

                string searchValue = string.Empty;

                if (_searchValue != "")
                {
                    // 통합검색인경우 
                    if (_searchKey == "all")
                    {
                        searchValue = " and (Code like '%" + _searchValue + "%' or Name like '%" + _searchValue + "%')";
                    }
                    else
                    {
                        searchValue = " and " + _searchKey + "like '%" + _searchValue + "%'";
                    }
                }

                query = query + searchValue;

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// Pruduct Group
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListDepartMent(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select * from [GW_Dimension Value] where [Dimension Code]  = 'DEPARTMENT' and Blocked = 0 and [Dimension Value Type] =  0 and len(code) = 4";

                string searchValue = string.Empty;

                if (_searchValue != "")
                {
                    // 통합검색인경우 
                    if (_searchKey == "all")
                    {
                        searchValue = " and (Code like '%" + _searchValue + "%' or Name like '%" + _searchValue + "%')";
                    }
                    else
                    {
                        searchValue = " and " + _searchKey + "like '%" + _searchValue + "%'";
                    }
                }

                query = query + searchValue;

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// Pruduct Group
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListEmployee(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select * from [GW_Dimension Value] where [Dimension Code]  = 'EMP' and Blocked = 0 and [Dimension Value Type] =  0";

                string searchValue = string.Empty;

                if (_searchValue != "")
                {
                    // 통합검색인경우 
                    if (_searchKey == "all")
                    {
                        searchValue = " and (Code like '%" + _searchValue + "%' or Name like '%" + _searchValue + "%')";
                    }
                    else
                    {
                        searchValue = " and " + _searchKey + "like '%" + _searchValue + "%'";
                    }
                }

                query = query + searchValue;

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// TaxCode  팝업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListTax(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _cardVendorKind)
        {

            using (var cn = new SqlConnection(connErp))
            {
                string query = @"select * from [GW_Gen Journal Tax Setup]";

                if (_cardVendorKind == "C")
                    query += " where code in ('00','02')";
                else
                    query += " where code <> '02'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 벤더팝업 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListVendorCount(int _companyID, string _searchKey, string _searchValue, string _masterDetailGubun)
        {

            using (var cn = new SqlConnection(connErp))
            {
                var cmd = new SqlCommand("Site_ERP_Vendor_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull();
                cmd.Parameters.Add("@MasterDetailGubun", SqlDbType.Char, 1).Value = _masterDetailGubun;

                cn.Open();

                return Convert.ToInt32(ExecuteScalar(cmd));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public void AddDraft(int _userID, string _checkVal, string _draftName)
        {

            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                // 마스터를 저장 하고 값을 따온다. 
                var cmd = new SqlCommand("Site_ERP_DraftAddM", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;////.ToNull();
                cmd.Parameters.Add("@CheckVal", SqlDbType.NVarChar, 100).Value = _checkVal;////_checkVal.ToNull();
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _draftName;////_draftName.ToNull();

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public void ModDraft(int _DraftID, string _checkVal, string _draftName)
        {

            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                // 마스터를 저장 하고 값을 따온다. 
                var cmd = new SqlCommand("Site_ERP_DraftMod", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _DraftID;////.ToNull();
                cmd.Parameters.Add("@CheckVal", SqlDbType.NVarChar, 100).Value = _checkVal;////_checkVal.ToNull();
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _draftName;////_draftName.ToNull();

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }


        public void DelDraft(int _DraftID)
        {

            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                // 마스터를 저장 하고 값을 따온다. 
                var cmd = new SqlCommand("Site_ERP_DraftDel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _DraftID;////.ToNull();
                

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }


        public void LoadDraft(int _DraftID, int _UserID)
        {

            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                // 마스터를 저장 하고 값을 따온다. 
                var cmd = new SqlCommand("Site_ERP_DraftLoad", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _DraftID;////.ToNull();
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }


        /// <summary>
        /// 비용 RepeatEx 조회용 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetExpenseUserList(ExpenseUserInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_Select", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        /// <summary>
        /// 비용 RepeatEx 조회용 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetExpenseUserListAdmin(ExpenseUserInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 비용 RepeatEx 조회용 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetExpenseUserListFullAdmin(ExpenseUserInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectFullAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }



        /// <summary>
        /// 비용 RepeatEx 검색용
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetExpenseUserListAdminSearch(ExpenseUserInfo _info, string _key, string _val)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectAdminSearch", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = "";
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = "";
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _key;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _val;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 비용 RepeatEx 조회용 
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public DataTableReader GetExpenseUserListUser(ExpenseUserInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectUser", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region SET

        /// <summary>
        /// 비용전표에서 사용자 하단 Row 입력 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ExpenseUserAdd(Model.ExpenseUserInfo _info)
        {
            string LoginID = string.Empty;

            string UserCode = _info.Dimension3Code;

            // 벤더 코드로 loginID 구해오기 
            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                string query = "select isnull(LoginID,'') from dp_acc_user where UserCode = '{0}' and activeuser = 1 order by userid desc";
                query = string.Format(query, UserCode.ToString());

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                LoginID = ExecuteScalar(cmd).ToString();
            }


            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {

                    //VendorDescription 을 확인 해준다. 
                    string strDes = string.Empty;

                    strDes = "Expense Settlement in " + getMonthYear() + " (" + _info.VoucherNo.ToNull() + ") - " + LoginID;



                    var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_Insert", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                    cmd.Parameters.Add("@VoucherNo", SqlDbType.VarChar, 20).Value = _info.VoucherNo.ToNull();
                    cmd.Parameters.Add("@CardExKind", SqlDbType.Char, 1).Value = _info.CardExKind.ToNull();
                    cmd.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = _info.DocumentDate;
                    cmd.Parameters.Add("@PostingDate", SqlDbType.DateTime).Value = _info.PostingDate;
                    cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = _info.DueDate;
                    cmd.Parameters.Add("@VendorDescription", SqlDbType.NVarChar, 100).Value = strDes;// Byte100Return(_info.VendorDescription.ToNull());
                    cmd.Parameters.Add("@ExDescription", SqlDbType.NVarChar, 100).Value = Byte100Return(_info.ExDescription.ToNull());
                    cmd.Parameters.Add("@DebitAmount1", SqlDbType.Decimal).Value = _info.DebitAmount1;
                    cmd.Parameters.Add("@DebitAmount2", SqlDbType.Decimal).Value = _info.DebitAmount2;
                    cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = _info.CreditAmount;
                    cmd.Parameters.Add("@PostingGroup", SqlDbType.NVarChar, 10).Value = _info.PostingGroup.ToNull();
                    cmd.Parameters.Add("@Dimension1Code", SqlDbType.NVarChar, 20).Value = _info.Dimension1Code.ToNull();
                    cmd.Parameters.Add("@Dimension2Code", SqlDbType.NVarChar, 20).Value = _info.Dimension2Code.ToNull();
                    cmd.Parameters.Add("@Dimension3Code", SqlDbType.NVarChar, 20).Value = _info.Dimension3Code.ToNull();
                    cmd.Parameters.Add("@Dimension4Code", SqlDbType.NVarChar, 20).Value = _info.Dimension4Code.ToNull();

                    cmd.Parameters.Add("@ExDimension1Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension1Code.ToNull();
                    cmd.Parameters.Add("@ExDimension2Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension2Code.ToNull();
                    cmd.Parameters.Add("@ExDimension3Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension3Code.ToNull();
                    cmd.Parameters.Add("@ExDimension4Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension4Code.ToNull();
                    cmd.Parameters.Add("@VendorNM", SqlDbType.NVarChar, 20).Value = _info.VendorNM.ToNull();
                    cmd.Parameters.Add("@ExVendorNM", SqlDbType.NVarChar, 20).Value = _info.ExVendorNM.ToNull();
                    cmd.Parameters.Add("@GLAccount", SqlDbType.NVarChar, 30).Value = _info.GLAccount.ToNull();
                    cmd.Parameters.Add("@GLAccountNM", SqlDbType.NVarChar, 50).Value = _info.GLAccountNM.ToNull();
                    cmd.Parameters.Add("@TaxCode", SqlDbType.NVarChar, 50).Value = _info.TaxCode.ToNull();
                    cmd.Parameters.Add("@TaxCodeNM", SqlDbType.NVarChar, 20).Value = _info.TaxCodeNM.ToNull();
                    cmd.Parameters.Add("@ExBankAccountCode", SqlDbType.NVarChar, 20).Value = _info.ExBankAccountCode.ToNull();
                    cmd.Parameters.Add("@ExBankAccountNo", SqlDbType.NVarChar, 20).Value = _info.ExBankAccountNo.ToNull();
                    cmd.Parameters.Add("@ExBankAccountCearing", SqlDbType.NVarChar, 30).Value = _info.ExBankAccountCearing.ToNull();
                    cmd.Parameters.Add("@ExPostingGroup", SqlDbType.NVarChar, 20).Value = _info.ExPostingGroup.ToNull();

                    cmd.Parameters.Add("@BankAcountCode", SqlDbType.NVarChar, 20).Value = _info.BankAcountCode.ToNull();
                    cmd.Parameters.Add("@BankAccountNo", SqlDbType.NVarChar, 20).Value = _info.BankAccountNo.ToNull();
                    cmd.Parameters.Add("@BankAcountCearingID", SqlDbType.NVarChar, 30).Value = _info.BankAcountCearingID.ToNull();
                    cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                    cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;


                    //사업장 코드 - VENDOR_NUM - Ex)759206175                etc1
                    //벤더명 - VENDor_NAme - 투썸플레이스서울스퀘어점        etc2  
                    //대표자명 - vendor_person - 문윤민                      etc3
                    //사업장주소 - Vendor_address1 + " " + Vendor_address2   etc4
                    //증빙일  - Approve_date                                 etc5
                    //사용시간 - approve_time                                etc6
                    // 기타 필드
                    cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 200).Value = _info.Etc1.ToNull();
                    cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 200).Value = _info.Etc2.ToNull();
                    cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 200).Value = _info.Etc3.ToNull();
                    cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 200).Value = _info.Etc4.ToNull();
                    cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 200).Value = _info.Etc5.ToNull();
                    cmd.Parameters.Add("@Etc6", SqlDbType.NVarChar, 200).Value = _info.Etc6.ToNull();
                    cmd.Parameters.Add("@Etc7", SqlDbType.NVarChar, 200).Value = _info.Etc7.ToNull();
                    cmd.Parameters.Add("@Etc8", SqlDbType.NVarChar, 200).Value = _info.Etc8.ToNull();
                    cmd.Parameters.Add("@Etc9", SqlDbType.NVarChar, 200).Value = _info.Etc9.ToNull();
                    cmd.Parameters.Add("@Etc10", SqlDbType.NVarChar, 200).Value = _info.Etc10.ToNull();

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private string getMonthYear()
        {
            string rtnValue = string.Empty;

            int Month = DateTime.Now.Month;

            string MonthYack = string.Empty;

            switch (Month)
            {
                case 1: MonthYack = "Jan."; break;
                case 2: MonthYack = "Feb."; break;
                case 3: MonthYack = "Mar."; break;
                case 4: MonthYack = "Apr."; break;
                case 5: MonthYack = "May."; break;
                case 6: MonthYack = "Jun."; break;
                case 7: MonthYack = "Jul."; break;
                case 8: MonthYack = "Aug."; break;
                case 9: MonthYack = "Sep."; break;
                case 10: MonthYack = "Oct."; break;
                case 11: MonthYack = "Nov."; break;
                case 12: MonthYack = "Dec."; break;
            }

            rtnValue = MonthYack + " " + DateTime.Now.Year.ToString();

            return rtnValue;
        }


        private string Byte100Return(string _val)
        {
            // 디스크립션에 대한 Byte 수 제어 해서 넘어 선 경우 짤라서 입력 
            byte [] byteString = System.Text.Encoding.Default.GetBytes(_val);
            int nLen = byteString.Length;

            string rtnVal = _val;//string.Empty;
            if (nLen > 99)
            {
                rtnVal = string.Empty;
                byte [] byteString1 = new byte[99];
                for (int i = 0; i < 99; i++)
                {
                    byteString1[i] = byteString[i];
                }

                rtnVal = System.Text.Encoding.Default.GetString(byteString1);
                // 끝자리 특수문자 제거
                rtnVal = rtnVal.Substring(0, rtnVal.Length - 1);
            }

            return rtnVal;
        }



        /// <summary>
        /// 비용전표에서 사용자 하단 Row 입력 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ExpenseUserUpdate(Model.ExpenseUserInfo _info)
        {

            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_Update", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Seq", SqlDbType.Int).Value = _info.Seq;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@LegacyKey", SqlDbType.VarChar, 100).Value = _info.LegacyKey.ToNull();
                cmd.Parameters.Add("@VoucherNo", SqlDbType.VarChar, 20).Value = _info.VoucherNo.ToNull();
                cmd.Parameters.Add("@CardExKind", SqlDbType.Char, 1).Value = _info.CardExKind.ToNull();
                cmd.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = _info.DocumentDate;
                cmd.Parameters.Add("@PostingDate", SqlDbType.DateTime).Value = _info.PostingDate;
                cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = _info.DueDate;
                cmd.Parameters.Add("@VendorDescription", SqlDbType.NVarChar, 100).Value = Byte100Return(_info.VendorDescription.ToNull());
                cmd.Parameters.Add("@ExDescription", SqlDbType.NVarChar, 100).Value = Byte100Return(_info.ExDescription.ToNull());
                cmd.Parameters.Add("@DebitAmount1", SqlDbType.Decimal).Value = _info.DebitAmount1;
                cmd.Parameters.Add("@DebitAmount2", SqlDbType.Decimal).Value = _info.DebitAmount2;
                cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = _info.CreditAmount;
                cmd.Parameters.Add("@PostingGroup", SqlDbType.NVarChar, 10).Value = _info.PostingGroup.ToNull();
                cmd.Parameters.Add("@Dimension1Code", SqlDbType.NVarChar, 20).Value = _info.Dimension1Code.ToNull();
                cmd.Parameters.Add("@Dimension2Code", SqlDbType.NVarChar, 20).Value = _info.Dimension2Code.ToNull();
                cmd.Parameters.Add("@Dimension3Code", SqlDbType.NVarChar, 20).Value = _info.Dimension3Code.ToNull();
                cmd.Parameters.Add("@Dimension4Code", SqlDbType.NVarChar, 20).Value = _info.Dimension4Code.ToNull();

                cmd.Parameters.Add("@ExDimension1Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension1Code.ToNull();
                cmd.Parameters.Add("@ExDimension2Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension2Code.ToNull();
                cmd.Parameters.Add("@ExDimension3Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension3Code.ToNull();
                cmd.Parameters.Add("@ExDimension4Code", SqlDbType.NVarChar, 20).Value = _info.ExDimension4Code.ToNull();
                cmd.Parameters.Add("@VendorNM", SqlDbType.NVarChar, 20).Value = _info.VendorNM.ToNull();
                cmd.Parameters.Add("@ExVendorNM", SqlDbType.NVarChar, 20).Value = _info.ExVendorNM.ToNull();
                cmd.Parameters.Add("@GLAccount", SqlDbType.NVarChar, 30).Value = _info.GLAccount.ToNull();
                cmd.Parameters.Add("@GLAccountNM", SqlDbType.NVarChar, 50).Value = _info.GLAccountNM.ToNull();
                cmd.Parameters.Add("@TaxCode", SqlDbType.NVarChar, 50).Value = _info.TaxCode.ToNull();
                cmd.Parameters.Add("@TaxCodeNM", SqlDbType.NVarChar, 20).Value = _info.TaxCodeNM.ToNull();
                cmd.Parameters.Add("@ExBankAccountCode", SqlDbType.NVarChar, 20).Value = _info.ExBankAccountCode.ToNull();
                cmd.Parameters.Add("@ExBankAccountNo", SqlDbType.NVarChar, 20).Value = _info.ExBankAccountNo.ToNull();
                cmd.Parameters.Add("@ExBankAccountCearing", SqlDbType.NVarChar, 30).Value = _info.ExBankAccountCearing.ToNull();
                cmd.Parameters.Add("@ExPostingGroup", SqlDbType.NVarChar, 20).Value = _info.ExPostingGroup.ToNull();

                cmd.Parameters.Add("@BankAcountCode", SqlDbType.NVarChar, 20).Value = _info.BankAcountCode.ToNull();
                cmd.Parameters.Add("@BankAccountNo", SqlDbType.NVarChar, 20).Value = _info.BankAccountNo.ToNull();
                cmd.Parameters.Add("@BankAcountCearingID", SqlDbType.NVarChar, 30).Value = _info.BankAcountCearingID.ToNull();
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = _info.PONo.ToNull();
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = _info.CardApprovalNo.ToNull();
                cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status;


                //사업장 코드 - VENDOR_NUM - Ex)759206175                etc1
                //벤더명 - VENDor_NAme - 투썸플레이스서울스퀘어점        etc2  
                //대표자명 - vendor_person - 문윤민                      etc3
                //사업장주소 - Vendor_address1 + " " + Vendor_address2   etc4
                //증빙일  - Approve_date                                 etc5
                //사용시간 - approve_time                                etc6
                // 기타 필드
                cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 200).Value = _info.Etc1.ToNull();
                cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 200).Value = _info.Etc2.ToNull();
                cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 200).Value = _info.Etc3.ToNull();
                cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 200).Value = _info.Etc4.ToNull();
                cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 200).Value = _info.Etc5.ToNull();
                cmd.Parameters.Add("@Etc6", SqlDbType.NVarChar, 200).Value = _info.Etc6.ToNull();
                cmd.Parameters.Add("@Etc7", SqlDbType.NVarChar, 200).Value = _info.Etc7.ToNull();
                cmd.Parameters.Add("@Etc8", SqlDbType.NVarChar, 200).Value = _info.Etc8.ToNull();
                cmd.Parameters.Add("@Etc9", SqlDbType.NVarChar, 200).Value = _info.Etc9.ToNull();
                cmd.Parameters.Add("@Etc10", SqlDbType.NVarChar, 200).Value = _info.Etc10.ToNull();

                cn.Open();

                ExecuteNonQuery(cmd);
            }

            return true;

        }


        /// <summary>
        /// 최종 Insert 가 성공 혹은 반려시 하고 나면 Update 준다.
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_voucherNO"></param>
        public void CardExpenseIFInsertbyUserUpdate(int _CompanyID, string _voucherNO, string _status)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set Status='" + _status + "' where VoucherNo='" + _voucherNO + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_voucherNO"></param>
        public void CardUserUpdate(int _CompanyID, string DepartMent, string DepartMentCD, string Employee, string EmployeeCD, string VendorDescription, string Seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = @"update [GW_Interface TableUser] set VendorDescription='" + Byte100Return(VendorDescription) + "'  where seq='" + Seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }


        /// <summary>
        /// 파이넨셜 반려시 Pono 를 FT 로 업데이트 한다
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_voucherNO"></param>
        public void CardExpenseIFInsertbyUserUpdate2(int _CompanyID, string _voucherNO, string _status)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set documentid = 0, LegacyKey = '', [PO No]='" + _status + "' where VoucherNo='" + _voucherNO + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }


        /// <summary>
        /// temp -> I/F 로 최종 자료 입력 
        /// </summary>
        /// <param name="_voucherNO"></param>
        public string CardExpenseIFInsert(int _CompanyID, string _voucherNO, string _tVal, string _bVal)
        {
            // UserTable 로 부터 데이터를 끌어 온다
            string rtnValue = "1";
            DataSet dsUser;
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectByIFInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@VoucherNo", SqlDbType.VarChar, 100).Value = _voucherNO;

                cn.Open();

                dsUser = ExecuteDataSet(cmd, "UserTemp");
            }

            
            int LineNo = 0;      // Line No 10000 씩 증가한다.

            string TaxCode = string.Empty;
            IFInfo ifInfo;

            if (dsUser != null && dsUser.Tables[0].Rows.Count > 0)
            {
                //totalAmount = Convert.ToDecimal(dsUser.Tables[1].Rows[0][0].ToString());
                
                // 대변에서 사용할 변수 세팅
                decimal totalAmount = 0;
                DateTime DocumentDate2 = DateTime.Now;   // 벤더에서는 포스팅 D
                DateTime PostingDate2 = DateTime.Now; 
                DateTime DueDate2 = DateTime.Now;
                string AccountNo2 = string.Empty;
                string Description2 = string.Empty;
                string PostingGroup2 = string.Empty;
                string Dimension1Code2 = string.Empty;
                string Dimension2Code2 = string.Empty;
                string Dimension3Code2 = string.Empty;
                string Dimension4Code2 = string.Empty;
                string BankAcountCode2 = string.Empty;
                string BankAccountNo2 = string.Empty;
                string BankAcountCearingID2 = string.Empty;
                string PONo2 = string.Empty;
                string CardApprovalNo2 = string.Empty;
                for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        // 대변에서 사용 할 변수 할당
                        DocumentDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        PostingDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        DueDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                        AccountNo2 = dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        Description2 = dsUser.Tables[0].Rows[i]["VendorDescription"].ToString();
                        PostingGroup2 = dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                        Dimension1Code2 = dsUser.Tables[0].Rows[i]["Dimension1Code"].ToString();
                        Dimension2Code2 = dsUser.Tables[0].Rows[i]["Dimension2Code"].ToString();
                        Dimension3Code2 = dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        Dimension4Code2 = dsUser.Tables[0].Rows[i]["Dimension4Code"].ToString();
                        BankAcountCode2 = dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        BankAccountNo2 = dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                        BankAcountCearingID2 = dsUser.Tables[0].Rows[i]["ExBankAccountCearing"].ToString();
                        PONo2 = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                        CardApprovalNo2 = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                    }

                    // 기본 공급가액 먼저 입력
                    TaxCode = dsUser.Tables[0].Rows[i]["TaxCode"].ToString();

                    // 부가세 없이 공급가액만 입력 
                    LineNo += 10000;
                    ifInfo = new IFInfo();
                            
                    ifInfo.DocumentNo = _voucherNO;
                    ifInfo.LineNo = LineNo;
                    ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                    ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                    ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                    ifInfo.AccountType = 1;
                    ifInfo.AccountNo = dsUser.Tables[0].Rows[i]["GLAccount"].ToString();
                    ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                    if (TaxCode == "05")
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString()) + Convert.ToDecimal(dsUser.Tables[0].Rows[i]["CreditAmount"].ToString());
                    else
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString());
                    ifInfo.CreditAmount = 0;
                    ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                    ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                    ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                    ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                    ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                    ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                    ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                    ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                    ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                    ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                    ifInfo.TempleteName = _tVal;
                    ifInfo.BatchName = _bVal;

                    // 대변 금액을 정해준다.
                    totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString());

                    InsertIFTable(ifInfo);


                    if (TaxCode == "01" || TaxCode == "02" || TaxCode == "04")
                    {
                        // 차변의 부가세 금액을 입력 10% 
                        LineNo += 10000;
                        ifInfo = new IFInfo();

                        ifInfo.DocumentNo = _voucherNO;
                        ifInfo.LineNo = LineNo;
                        ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                        ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                        ifInfo.AccountType = 1;

                        if (TaxCode == "01" || TaxCode == "04")
                            ifInfo.AccountNo = "1114100";
                        else
                            ifInfo.AccountNo = "1114110";

                        ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());
                        ifInfo.CreditAmount = 0;
                        ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                        ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                        ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                        ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                        ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                        ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                        ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                        ifInfo.TempleteName = _tVal;
                        ifInfo.BatchName = _bVal;
                        // 대변 금액을 정해준다.
                        totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());

                        InsertIFTable(ifInfo);
                    }

                    if (TaxCode == "05" || TaxCode == "06")
                    {
                        // 차변의 부가세 금액을 입력

                        if (TaxCode == "06") // 원천세 관련 
                        {
                            LineNo += 10000;
                            ifInfo = new IFInfo();

                            ifInfo.DocumentNo = _voucherNO;
                            ifInfo.LineNo = LineNo;
                            ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                            ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                            ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                            ifInfo.AccountType = 1;
                            ifInfo.AccountNo = "2109900";
                            ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                            ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());
                            ifInfo.CreditAmount = 0;
                            ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                            ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                            ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                            ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                            ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                            ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                            ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                            ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                            ifInfo.TempleteName = _tVal;
                            ifInfo.BatchName = _bVal;
                            // 대변 금액을 정해준다.
                            totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());

                            InsertIFTable(ifInfo);
                        }
                        else // 원천세 관련 대변을 입력 해준다.
                        {
                            LineNo += 10000;
                            ifInfo = new IFInfo();

                            ifInfo.DocumentNo = _voucherNO;
                            ifInfo.LineNo = LineNo;
                            ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                            ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                            ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                            ifInfo.AccountType = 1;
                            ifInfo.AccountNo = "2109900";
                            ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                            ifInfo.DebitAmount = 0;
                            ifInfo.CreditAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());
                            ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                            ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                            ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                            ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                            ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                            ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                            ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                            ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                            ifInfo.TempleteName = _tVal;
                            ifInfo.BatchName = _bVal;
                            // 대변 금액을 정해준다.// 대변이긴 하지만 별도의 비용 분개 라인
                            //totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());
                            totalAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString()) + Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());

                            InsertIFTable(ifInfo);
                        }
                    }
                    
                   
                }

                // 대변 입력
                LineNo += 10000;
                ifInfo = new IFInfo();

                ifInfo.DocumentNo = _voucherNO;
                ifInfo.LineNo = LineNo;
                ifInfo.DocumentDate = DocumentDate2;
                ifInfo.PostingDate = PostingDate2;
                ifInfo.DueDate = DueDate2;
                ifInfo.AccountType = 0;
                ifInfo.AccountNo = AccountNo2;
                ifInfo.Description = Description2;
                ifInfo.DebitAmount = 0;
                ifInfo.CreditAmount = totalAmount;
                ifInfo.PostingGroup = PostingGroup2;
                ifInfo.Dimension1Code = Dimension1Code2;
                ifInfo.Dimension2Code = Dimension2Code2;
                ifInfo.Dimension3Code = Dimension3Code2;
                ifInfo.Dimension4Code = Dimension4Code2;
                ifInfo.BankAcountCode = BankAcountCode2;
                ifInfo.BankAccountNo = BankAccountNo2;
                ifInfo.BankAcountCearingID = BankAcountCearingID2;
                ifInfo.PONo = PONo2;
                ifInfo.CardApprovalNo = "";// CardApprovalNo2;
                ifInfo.TempleteName = _tVal;
                ifInfo.BatchName = _bVal;

                InsertIFTable(ifInfo);
            }

            return rtnValue;
        }



        /// <summary>
        /// 뿌릴 데이터를 레코드셋으로 반환 하여 입력 해준다.
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_voucherNO"></param>
        /// <returns></returns>
        public DataTable CardExpenseTempSet(int _CompanyID, string _voucherNO)
        {
            DataTable table = new DataTable();
            DataRow row = null;

            table.Columns.Add(new DataColumn("DocumentNo", typeof(string)));
            table.Columns.Add(new DataColumn("LineNo", typeof(int)));
            table.Columns.Add(new DataColumn("DocumentDate", typeof(string)));
            table.Columns.Add(new DataColumn("PostingDate", typeof(string)));
            table.Columns.Add(new DataColumn("DueDate", typeof(string)));
            table.Columns.Add(new DataColumn("AccountType", typeof(int)));
            table.Columns.Add(new DataColumn("AccountNo", typeof(string)));
            table.Columns.Add(new DataColumn("Description", typeof(string)));
            table.Columns.Add(new DataColumn("DebitAmount", typeof(decimal)));
            table.Columns.Add(new DataColumn("CreditAmount", typeof(decimal)));
            table.Columns.Add(new DataColumn("PostingGroup", typeof(string)));
            table.Columns.Add(new DataColumn("Dimension1Code", typeof(string)));
            table.Columns.Add(new DataColumn("Dimension2Code", typeof(string)));
            table.Columns.Add(new DataColumn("Dimension3Code", typeof(string)));
            table.Columns.Add(new DataColumn("Dimension4Code", typeof(string)));
            table.Columns.Add(new DataColumn("BankAcountCode", typeof(string)));
            table.Columns.Add(new DataColumn("BankAccountNo", typeof(string)));
            table.Columns.Add(new DataColumn("BankAcountCearingID", typeof(string)));
            table.Columns.Add(new DataColumn("PONo", typeof(string)));
            table.Columns.Add(new DataColumn("CardApprovalNo", typeof(string)));
            table.Columns.Add(new DataColumn("AccountNM", typeof(string)));
            table.Columns.Add(new DataColumn("VendorNM", typeof(string)));
            table.Columns.Add(new DataColumn("Seq", typeof(string)));
            table.Columns.Add(new DataColumn("Kind", typeof(string)));
            
            // UserTable 로 부터 데이터를 끌어 온다
            DataSet dsUser;
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_UserInterFaceTable_SelectByIFInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@VoucherNo", SqlDbType.VarChar, 100).Value = _voucherNO;

                cn.Open();

                dsUser = ExecuteDataSet(cmd, "UserTemp");
            }


            int LineNo = 0;      // Line No 10000 씩 증가한다.

            string TaxCode = string.Empty;
            IFInfo ifInfo;

            if (dsUser != null && dsUser.Tables[0].Rows.Count > 0)
            {
                //totalAmount = Convert.ToDecimal(dsUser.Tables[1].Rows[0][0].ToString());

                // 대변에서 사용할 변수 세팅
                decimal totalAmount = 0;
                DateTime DocumentDate2 = DateTime.Now;
                DateTime PostingDate2 = DateTime.Now;
                DateTime DueDate2 = DateTime.Now;
                string AccountNo2 = string.Empty;
                string Description2 = string.Empty;
                string PostingGroup2 = string.Empty;
                string Dimension1Code2 = string.Empty;
                string Dimension2Code2 = string.Empty;
                string Dimension3Code2 = string.Empty;
                string Dimension4Code2 = string.Empty;
                string BankAcountCode2 = string.Empty;
                string BankAccountNo2 = string.Empty;
                string BankAcountCearingID2 = string.Empty;
                string PONo2 = string.Empty;
                string CardApprovalNo2 = string.Empty;
                string AccountNM = string.Empty;
                string VendorNM = string.Empty;
                string Seq = string.Empty;
                string Kind = string.Empty;
                for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        // 대변에서 사용 할 변수 할당
                        DocumentDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        PostingDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        DueDate2 = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                        AccountNo2 = dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        Description2 = dsUser.Tables[0].Rows[i]["VendorDescription"].ToString();
                        PostingGroup2 = dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                        Dimension1Code2 = dsUser.Tables[0].Rows[i]["Dimension1Code"].ToString();
                        Dimension2Code2 = dsUser.Tables[0].Rows[i]["Dimension2Code"].ToString();
                        Dimension3Code2 = dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        Dimension4Code2 = dsUser.Tables[0].Rows[i]["Dimension4Code"].ToString();
                        BankAcountCode2 = dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        BankAccountNo2 = dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                        BankAcountCearingID2 = dsUser.Tables[0].Rows[i]["ExBankAccountCearing"].ToString();
                        PONo2 = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                        CardApprovalNo2 = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                        AccountNM = dsUser.Tables[0].Rows[i]["ExVendorNM"].ToString();
                        VendorNM = dsUser.Tables[0].Rows[i]["ExVendorNM"].ToString();
                        Seq = dsUser.Tables[0].Rows[i]["Seq"].ToString();
                        Kind = "V";
                    }

                    // 기본 공급가액 먼저 입력
                    TaxCode = dsUser.Tables[0].Rows[i]["TaxCode"].ToString();

                    // 부가세 없이 공급가액만 입력 
                    LineNo += 10000;
                    ifInfo = new IFInfo();

                    ifInfo.DocumentNo = _voucherNO;
                    ifInfo.LineNo = LineNo;
                    ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                    ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                    ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                    ifInfo.AccountType = 1;
                    ifInfo.AccountNo = dsUser.Tables[0].Rows[i]["GLAccount"].ToString();
                    ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                    if(TaxCode == "05")
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-")) + Convert.ToDecimal(dsUser.Tables[0].Rows[i]["CreditAmount"].ToString().Replace("-,", "-"));
                    else
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString().Replace("-,","-"));

                    ifInfo.CreditAmount = 0;
                    ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                    ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                    ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                    ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                    ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                    ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                    ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                    ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                    ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                    ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                    ifInfo.AccountNM = dsUser.Tables[0].Rows[i]["GLAccountNM"].ToString();
                    ifInfo.VendorNM = "";// dsUser.Tables[0].Rows[i]["VendorNM"].ToString();
                    ifInfo.Seq = dsUser.Tables[0].Rows[i]["Seq"].ToString();
                    ifInfo.Kind = "E";
                    

                    // 대변 금액을 정해준다.
                    totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString().Replace("-,", "-"));

                    row = table.NewRow();
                    row.ItemArray = new object[] { ifInfo.DocumentNo, 
                                                   ifInfo.LineNo, 
                                                   ifInfo.DocumentDate.ToShortDateString(), 
                                                   ifInfo.PostingDate.ToShortDateString(),
                                                   ifInfo.DueDate.ToShortDateString(),
                                                   ifInfo.AccountType,
                                                   ifInfo.AccountNo,
                                                   ifInfo.Description,
                                                   ifInfo.DebitAmount,
                                                   ifInfo.CreditAmount,
                                                   ifInfo.PostingGroup,
                                                   ifInfo.Dimension1Code,
                                                   ifInfo.Dimension2Code,
                                                   ifInfo.Dimension3Code,
                                                   ifInfo.Dimension4Code,
                                                   ifInfo.BankAcountCode,
                                                   ifInfo.BankAccountNo,
                                                   ifInfo.BankAcountCearingID,
                                                   ifInfo.PONo,
                                                   ifInfo.CardApprovalNo,
                                                   ifInfo.AccountNM,
                                                   ifInfo.VendorNM,
                                                   ifInfo.Seq,
                                                   ifInfo.Kind
                                                 };
                    table.Rows.Add(row);



                    if (TaxCode == "01" || TaxCode == "02" || TaxCode == "04")
                    {
                        // 차변의 부가세 금액을 입력 10% 
                        LineNo += 10000;
                        ifInfo = new IFInfo();

                        ifInfo.DocumentNo = _voucherNO;
                        ifInfo.LineNo = LineNo;
                        ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                        ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                        ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                        ifInfo.AccountType = 1;

                        if (TaxCode == "01" || TaxCode == "04")
                        {
                            ifInfo.AccountNo = "1114100";
                            ifInfo.AccountNM = "Prepaid VAT ";
                        }
                        else
                        {
                            ifInfo.AccountNo = "1114110";
                            ifInfo.AccountNM = "Prepaid VAT (신용카드)";
                        }

                        ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                        ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));
                        ifInfo.CreditAmount = 0;
                        ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                        ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                        ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                        ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                        ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                        ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                        ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                        ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                        ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                        //ifInfo.AccountNM = dsUser.Tables[0].Rows[i]["Etc8"].ToString();
                        ifInfo.VendorNM = "";// dsUser.Tables[0].Rows[i]["VendorNM"].ToString();
                        ifInfo.Seq = dsUser.Tables[0].Rows[i]["Seq"].ToString();
                        ifInfo.Kind = "T";

                        // 대변 금액을 정해준다.
                        totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));

                        row = table.NewRow();
                        row.ItemArray = new object[] { ifInfo.DocumentNo, 
                                                   ifInfo.LineNo, 
                                                   ifInfo.DocumentDate.ToShortDateString(), 
                                                   ifInfo.PostingDate.ToShortDateString(),
                                                   ifInfo.DueDate.ToShortDateString(),
                                                   ifInfo.AccountType,
                                                   ifInfo.AccountNo,
                                                   ifInfo.Description,
                                                   ifInfo.DebitAmount,
                                                   ifInfo.CreditAmount,
                                                   ifInfo.PostingGroup,
                                                   ifInfo.Dimension1Code,
                                                   ifInfo.Dimension2Code,
                                                   ifInfo.Dimension3Code,
                                                   ifInfo.Dimension4Code,
                                                   ifInfo.BankAcountCode,
                                                   ifInfo.BankAccountNo,
                                                   ifInfo.BankAcountCearingID,
                                                   ifInfo.PONo,
                                                   ifInfo.CardApprovalNo,
                                                   ifInfo.AccountNM,
                                                   ifInfo.VendorNM,
                                                   ifInfo.Seq,
                                                   ifInfo.Kind
                                                 };
                        table.Rows.Add(row);
                    }

                    if (TaxCode == "05" || TaxCode == "06")
                    {
                        // 차변의 부가세 금액을 입력

                        if (TaxCode == "06") // 원천세 관련 
                        {
                            LineNo += 10000;
                            ifInfo = new IFInfo();

                            ifInfo.DocumentNo = _voucherNO;
                            ifInfo.LineNo = LineNo;
                            ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                            ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                            ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                            ifInfo.AccountType = 1;
                            ifInfo.AccountNo = "2109900";
                            ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                            ifInfo.DebitAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));
                            ifInfo.CreditAmount = 0;
                            ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                            ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                            ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                            ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                            ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                            ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                            ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                            ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                            ifInfo.AccountNM = "W. taxes-other ";// dsUser.Tables[0].Rows[i]["GLAccountNM"].ToString();
                            ifInfo.VendorNM = "";// dsUser.Tables[0].Rows[i]["VendorNM"].ToString();
                            ifInfo.Seq = dsUser.Tables[0].Rows[i]["Seq"].ToString();
                            ifInfo.Kind = "T";

                            // 대변 금액을 정해준다.
                            totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));

                            row = table.NewRow();
                            row.ItemArray = new object[] { ifInfo.DocumentNo, 
                                                   ifInfo.LineNo, 
                                                   ifInfo.DocumentDate.ToShortDateString(), 
                                                   ifInfo.PostingDate.ToShortDateString(),
                                                   ifInfo.DueDate.ToShortDateString(),
                                                   ifInfo.AccountType,
                                                   ifInfo.AccountNo,
                                                   ifInfo.Description,
                                                   ifInfo.DebitAmount,
                                                   ifInfo.CreditAmount,
                                                   ifInfo.PostingGroup,
                                                   ifInfo.Dimension1Code,
                                                   ifInfo.Dimension2Code,
                                                   ifInfo.Dimension3Code,
                                                   ifInfo.Dimension4Code,
                                                   ifInfo.BankAcountCode,
                                                   ifInfo.BankAccountNo,
                                                   ifInfo.BankAcountCearingID,
                                                   ifInfo.PONo,
                                                   ifInfo.CardApprovalNo,
                                                   ifInfo.AccountNM,
                                                   ifInfo.VendorNM,
                                                   ifInfo.Seq,
                                                   ifInfo.Kind
                                                 };
                            table.Rows.Add(row);
                        }
                        else // 원천세 관련 대변을 입력 해준다.
                        {
                            LineNo += 10000;
                            ifInfo = new IFInfo();

                            ifInfo.DocumentNo = _voucherNO;
                            ifInfo.LineNo = LineNo;
                            ifInfo.DocumentDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DocumentDate"].ToString());
                            ifInfo.PostingDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["PostingDate"].ToString());
                            ifInfo.DueDate = Convert.ToDateTime(dsUser.Tables[0].Rows[i]["DueDate"].ToString());
                            ifInfo.AccountType = 1;
                            ifInfo.AccountNo = "2109900";
                            ifInfo.Description = dsUser.Tables[0].Rows[i]["ExDescription"].ToString();
                            ifInfo.DebitAmount = 0;
                            ifInfo.CreditAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));
                            ifInfo.PostingGroup = "";// dsUser.Tables[0].Rows[i]["ExPostingGroup"].ToString();
                            ifInfo.Dimension1Code = dsUser.Tables[0].Rows[i]["ExDimension1Code"].ToString();
                            ifInfo.Dimension2Code = dsUser.Tables[0].Rows[i]["ExDimension2Code"].ToString();
                            ifInfo.Dimension3Code = "";// dsUser.Tables[0].Rows[i]["ExDimension3Code"].ToString();
                            ifInfo.Dimension4Code = dsUser.Tables[0].Rows[i]["ExDimension4Code"].ToString();
                            ifInfo.BankAcountCode = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.BankAccountNo = "";// dsUser.Tables[0].Rows[i]["ExBankAccountNo"].ToString();
                            ifInfo.BankAcountCearingID = "";// dsUser.Tables[0].Rows[i]["ExBankAccountCode"].ToString();
                            ifInfo.PONo = dsUser.Tables[0].Rows[i]["PONo"].ToString();
                            ifInfo.CardApprovalNo = dsUser.Tables[0].Rows[i]["CardApprovalNo"].ToString();
                            ifInfo.AccountNM = "W. taxes-other ";// dsUser.Tables[0].Rows[i]["Etc8"].ToString();
                            ifInfo.VendorNM = "";// dsUser.Tables[0].Rows[i]["VendorNM"].ToString();
                            ifInfo.Seq = dsUser.Tables[0].Rows[i]["Seq"].ToString();
                            ifInfo.Kind = "T";

                            // 대변 금액을 정해준다.// 대변이긴 하지만 별도의 비용 분개 라인
                            //totalAmount += Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString());
                            totalAmount = Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount1"].ToString().Replace("-,", "-")) + Convert.ToDecimal(dsUser.Tables[0].Rows[i]["DebitAmount2"].ToString().Replace("-,", "-"));

                            row = table.NewRow();
                            row.ItemArray = new object[] { ifInfo.DocumentNo, 
                                                   ifInfo.LineNo, 
                                                   ifInfo.DocumentDate.ToShortDateString(), 
                                                   ifInfo.PostingDate.ToShortDateString(),
                                                   ifInfo.DueDate.ToShortDateString(),
                                                   ifInfo.AccountType,
                                                   ifInfo.AccountNo,
                                                   ifInfo.Description,
                                                   ifInfo.DebitAmount,
                                                   ifInfo.CreditAmount,
                                                   ifInfo.PostingGroup,
                                                   ifInfo.Dimension1Code,
                                                   ifInfo.Dimension2Code,
                                                   ifInfo.Dimension3Code,
                                                   ifInfo.Dimension4Code,
                                                   ifInfo.BankAcountCode,
                                                   ifInfo.BankAccountNo,
                                                   ifInfo.BankAcountCearingID,
                                                   ifInfo.PONo,
                                                   ifInfo.CardApprovalNo,
                                                   ifInfo.AccountNM,
                                                   ifInfo.VendorNM,
                                                   ifInfo.Seq,
                                                   ifInfo.Kind
                                                 };
                            table.Rows.Add(row);
                        }
                    }


                }

                // 대변 입력
                LineNo += 10000;
                ifInfo = new IFInfo();

                ifInfo.DocumentNo = _voucherNO;
                ifInfo.LineNo = LineNo;
                ifInfo.DocumentDate = DocumentDate2;
                ifInfo.PostingDate = PostingDate2;
                ifInfo.DueDate = DueDate2;
                ifInfo.AccountType = 0;
                ifInfo.AccountNo = AccountNo2;
                ifInfo.Description = Description2;
                ifInfo.DebitAmount = 0;
                ifInfo.CreditAmount = totalAmount;
                ifInfo.PostingGroup = PostingGroup2;
                ifInfo.Dimension1Code = Dimension1Code2;
                ifInfo.Dimension2Code = Dimension2Code2;
                ifInfo.Dimension3Code = Dimension3Code2;
                ifInfo.Dimension4Code = Dimension4Code2;
                ifInfo.BankAcountCode = BankAcountCode2;
                ifInfo.BankAccountNo = BankAccountNo2;
                ifInfo.BankAcountCearingID = BankAcountCearingID2;
                ifInfo.PONo = PONo2;
                ifInfo.CardApprovalNo = "";// CardApprovalNo2;
                ifInfo.AccountNM = AccountNM;
                ifInfo.VendorNM = VendorNM;
                ifInfo.Seq = Seq;
                ifInfo.Kind = Kind;

                row = table.NewRow();
                row.ItemArray = new object[] { ifInfo.DocumentNo, 
                                                   ifInfo.LineNo, 
                                                   ifInfo.DocumentDate.ToShortDateString(), 
                                                   ifInfo.PostingDate.ToShortDateString(),
                                                   ifInfo.DueDate.ToShortDateString(),
                                                   ifInfo.AccountType,
                                                   ifInfo.AccountNo,
                                                   ifInfo.Description,
                                                   ifInfo.DebitAmount,
                                                   ifInfo.CreditAmount,
                                                   ifInfo.PostingGroup,
                                                   ifInfo.Dimension1Code,
                                                   ifInfo.Dimension2Code,
                                                   ifInfo.Dimension3Code,
                                                   ifInfo.Dimension4Code,
                                                   ifInfo.BankAcountCode,
                                                   ifInfo.BankAccountNo,
                                                   ifInfo.BankAcountCearingID,
                                                   ifInfo.PONo,
                                                   ifInfo.CardApprovalNo,
                                                   ifInfo.AccountNM,
                                                   ifInfo.VendorNM,
                                                   ifInfo.Seq,
                                                   ifInfo.Kind
                                                 };
                table.Rows.Add(row);
            }

            return table;
        }

        private void InsertIFTable(IFInfo ifInfo)
        {
            using (var cn = new SqlConnection(connErp))
            {
                var cmd = new SqlCommand("SQL_Site_ERP_IFInsertEnd", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@DocumentNo", SqlDbType.NVarChar, 20).Value = ifInfo.DocumentNo;
                cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = ifInfo.LineNo;
                cmd.Parameters.Add("@DocumentDate", SqlDbType.DateTime).Value = ifInfo.DocumentDate;
                cmd.Parameters.Add("@PostingDate", SqlDbType.DateTime).Value = ifInfo.PostingDate;
                cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = ifInfo.DueDate;
                cmd.Parameters.Add("@AccountType", SqlDbType.Int).Value = ifInfo.AccountType;
                cmd.Parameters.Add("@AccountNo", SqlDbType.NVarChar, 20).Value = ifInfo.AccountNo;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = ifInfo.Description;
                cmd.Parameters.Add("@DebitAmount", SqlDbType.Decimal).Value = ifInfo.DebitAmount;
                cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = ifInfo.CreditAmount;
                cmd.Parameters.Add("@PostingGroup", SqlDbType.NVarChar, 10).Value = ifInfo.PostingGroup;
                cmd.Parameters.Add("@Dimension1Code", SqlDbType.NVarChar, 20).Value = ifInfo.Dimension1Code;
                cmd.Parameters.Add("@Dimension2Code", SqlDbType.NVarChar, 20).Value = ifInfo.Dimension2Code;
                cmd.Parameters.Add("@Dimension3Code", SqlDbType.NVarChar, 20).Value = ifInfo.Dimension3Code;
                cmd.Parameters.Add("@Dimension4Code", SqlDbType.NVarChar, 20).Value = ifInfo.Dimension4Code;
                cmd.Parameters.Add("@BankAcountCode", SqlDbType.NVarChar, 20).Value = ifInfo.BankAcountCode;
                cmd.Parameters.Add("@BankAccountNo", SqlDbType.NVarChar, 20).Value = ifInfo.BankAccountNo;
                cmd.Parameters.Add("@BankAcountCearingID", SqlDbType.NVarChar, 30).Value = ifInfo.BankAcountCearingID;
                cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 20).Value = ifInfo.PONo;
                cmd.Parameters.Add("@CardApprovalNo", SqlDbType.NVarChar, 100).Value = ifInfo.CardApprovalNo;
                cmd.Parameters.Add("@TempleteName", SqlDbType.NVarChar, 30).Value = ifInfo.TempleteName;
                cmd.Parameters.Add("@BatchName", SqlDbType.NVarChar, 30).Value = ifInfo.BatchName;
                

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }

        

        // 아이템 삭제
        public void ItemDel(int _companyID, string _key)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string[] arrKey = _key.Split('_');
                string query = "delete [GW_Interface TableUser] where seq = '" + _key +"'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }


        public void SetKeyData(string legacykey, string CheckVal, string UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(1)))
            {
                string Seqs = CheckVal.TrimEnd(',');

                string query = "update [GW_Interface TableUser] set legacykey = '" + legacykey + "' where userid = " + UserID + " and seq in (" + Seqs + ")";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }


        // 아이템 삭제
        public void AllItemDel(int _companyID, string _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string Seqs = string.Empty;
                string CardKey = string.Empty;
                string selQuery = "select seq, CardApprovalNO from  [GW_Interface TableUser] where userid = '" + _userID + "' and Status = 'T'";

                var cmd = new SqlCommand(selQuery, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                DataSet ds = ExecuteDataSet(cmd, "TableUser");

                if (ds.Tables[0].Rows.Count > 0 && ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Seqs += ds.Tables[0].Rows[i][0].ToString() + ",";

                        if(ds.Tables[0].Rows[i][1].ToString() != "")
                            CardKey += ds.Tables[0].Rows[i][1].ToString() + ",";
                    }
                }

                Seqs = Seqs.TrimEnd(',');

                string delQuery = "delete [GW_Interface TableUser] where seq in (" + Seqs + ")";

                cmd = new SqlCommand(delQuery, cn);
                cmd.CommandType = CommandType.Text;

                ExecuteNonQuery(cmd);

                if (CardKey != "")
                {
                    string[] keys = CardKey.Split(',');
                    string query = "update approval_data set Status='{3}' where approve_num='{0}' and card_num='{1}' and  gubun='{2}'";
                    string queryd = string.Empty;

                    for (int i = 0; i < keys.Length - 1; i++)
                    {
                        string[] arrKey = keys[i].Split('_');

                        queryd = string.Format(query, arrKey[0], arrKey[1], arrKey[2], "N");

                        cmd = new SqlCommand(queryd, cn);
                        cmd.CommandType = CommandType.Text;

                        ExecuteNonQuery(cmd);
                    }

                }                
            }
        }

        public void CardStatusUpdate(int _companyID, string _key, string _status)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string[] arrKey = _key.Split('_');
                string query = "update approval_data set Status='{3}' where approve_num='{0}' and card_num='{1}' and  gubun='{2}'";
                query = string.Format(query, arrKey[0], arrKey[1], arrKey[2], _status);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }
        }

        public string SeqUpdate(int _companyID)
        {
            string SEQ = string.Empty;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "select count(*) from GW_VoucherSeq where YY = {0} ";
                query = string.Format(query, DateTime.Now.Year.ToString().Substring(2,2));

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                SEQ = ExecuteScalar(cmd).ToString();

                if (SEQ != "0")
                {
                    query = "update GW_VoucherSeq set seq = seq + 1 where YY = {0} ";
                    query = string.Format(query, DateTime.Now.Year.ToString().Substring(2, 2));
                    var cmd1 = new SqlCommand(query, cn);
                    cmd1.CommandType = CommandType.Text;

                    ExecuteNonQuery(cmd1);
                }
                else
                {
                    query = "insert into GW_VoucherSeq values('{0}','{1}')";
                    query = string.Format(query, DateTime.Now.Year.ToString().Substring(2, 2),"1");
                    var cmd2 = new SqlCommand(query, cn);
                    cmd2.CommandType = CommandType.Text;

                    ExecuteNonQuery(cmd2);
                }

                // 현재바우처 정보를 로딩 해준다.
                query = "select seq from GW_VoucherSeq where YY = {0} ";
                query = string.Format(query, DateTime.Now.Year.ToString().Substring(2, 2));
                var cmd3 = new SqlCommand(query, cn);
                cmd3.CommandType = CommandType.Text;

                object rtnValue = ExecuteScalar(cmd3);

                return rtnValue.ToString();
            }
           
        }


        public void UpdatePostingDate(int _CompanyID, string _voucherNO, string _Seq, string _val)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Posting Date]='" + _val + "' where VoucherNo='" + _voucherNO + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdateDocumentDate(int _CompanyID, string _voucherNO, string _Seq, string _val)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Document Date]='" + _val + "' where VoucherNo='" + _voucherNO + "' and seq = '" + _Seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdateDueDate(int _CompanyID, string _voucherNO, string _Seq, string _val)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Due Date]='" + _val + "' where VoucherNo='" + _voucherNO + "'";// and seq = '" + _Seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdateGLAccount(int _CompanyID, string _valCode, string _valName, string _voucherNo, string _seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [GLAccount]='" + _valCode + "', [GLAccountNM] = '" + _valName + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdateVendor(int _CompanyID, string _valCode, string _valName, string _voucherNo, string _seq, string _postingGroup)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [ExDimension3Code]='" + _valCode + "', [ExVendorNM] = '" + _valName + "', [ExPostingGroup] = '" + _postingGroup + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdatePostingGroup(int _CompanyID, string _valCode, string _voucherNo, string _seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [ExPostingGroup]='" + _valCode + "', [Posting Group]='" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }


        public void UpdateDes(int _CompanyID, string _valCode, string _voucherNo, string _seq, string _kind)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {

                string query = string.Empty;

                if(_kind == "V")
                {
                    query = "update [GW_Interface TableUser] set [VendorDescription]='" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";
                }
                else
                {
                    query = "update [GW_Interface TableUser] set [ExDescription]='" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";
                }

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }



        public void UpdateDepartment(int _CompanyID, string _valCode, string _valName, string _voucherNo, string _seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Dimension 1 Code]='" + _valCode + "', ExDimension1Code = '" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }


        public void UpdateAmount(int _CompanyID, string _voucherNo, string _seq, string _kind, decimal _amount)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = string.Empty;

                if (_kind == "E") // 공급가액 변경
                {
                    query = "update [GW_Interface TableUser] set [Debit Amount1]=" + _amount.ToString() + ", [Credit Amount] = [Debit Amount2] + " + _amount.ToString() + " where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";
                }
                else if (_kind == "T") // 부가세변경
                {
                    query = "update [GW_Interface TableUser] set [Debit Amount2]=" + _amount.ToString() + ", [Credit Amount] = [Debit Amount1] + " + _amount.ToString() + " where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";
                }
                else // 합계 변경
                {
                    query = "update [GW_Interface TableUser] set [Credit Amount]=" + _amount.ToString() + ", [Debit Amount1] = " + _amount.ToString() +" - [Debit Amount2]  where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";
                }

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }



        public void UpdateEmployee(int _CompanyID, string _valCode, string _valName, string _voucherNo, string _seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Dimension 2 Code]='" + _valCode + "', ExDimension2Code = '" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }

        public void UpdateProductGroup(int _CompanyID, string _valCode, string _valName, string _voucherNo, string _seq)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {
                string query = "update [GW_Interface TableUser] set [Dimension 4 Code]='" + _valCode + "', ExDimension4Code = '" + _valCode + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }



        public void UpdateBank(int _CompanyID, string _valCode, string _valName, string _clearID, string _voucherNo, string _seq, string _kind)
        {
            using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
            {

                string query = string.Empty;

                query = "update [GW_Interface TableUser] set [ExBankAccountCode]='" + _valCode + "',[ExBankAccountNo]='" + _valName + "',[ExBankAccountCearing]='" + _clearID + "',[Bank Acount Code]='" + _valCode + "',[Bank Account No.]='" + _valName + "',[Bank Acount Cearing ID]='" + _clearID + "' where VoucherNo='" + _voucherNo + "' and seq = '" + _seq + "'";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                ExecuteNonQuery(cmd);
            }

        }
        /// <summary>
        /// Pruduct Group
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageListDraft(int _companyID, string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _userID)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"select * from dbo.GW_ExpenseUserDraft where userid = {0}";

                query = string.Format(query, _userID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        public void ExpenseUp(string _currentSeq, string _upSeq)
        {
            //using (var cn = new SqlConnection(ConnSolution(1)))
            //{
            //    string query = @"select * from dbo.GW_ExpenseUserDraft where userid = {0}";

            //    query = string.Format(query, _userID);

            //    var cmd = new SqlCommand(query, cn);
            //    cmd.CommandType = CommandType.Text;

            //    cn.Open();

            //    return ExecuteDataTableReader(cmd);
            //}
        }


        public void ExpenseDown(string _currentSeq, string _downSeq)
        {
            //using (var cn = new SqlConnection(ConnSolution(1)))
            //{
            //    string query = @"select * from dbo.GW_ExpenseUserDraft where userid = {0}";

            //    query = string.Format(query, _userID);

            //    var cmd = new SqlCommand(query, cn);
            //    cmd.CommandType = CommandType.Text;

            //    cn.Open();

            //    return ExecuteDataTableReader(cmd);
            //}
        }


        #endregion

    }
}