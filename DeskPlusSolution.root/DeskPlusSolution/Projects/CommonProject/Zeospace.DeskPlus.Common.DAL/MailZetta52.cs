using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class MailZetta52 : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 제타 5 버전 - 받은 편지함 가져오기
        /// (튜닝 : 홍석기 차장)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_listCount">The _list count.</param>
        /// <returns></returns>
        public DataTableReader GetMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            try
            {
                string domain_idx = GetDomainIdx(_companyID, _mailDomain);

                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SelZettaMail52");
                    query = string.Format(query, _listCount);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@domain_idx", SqlDbType.Int).Value = domain_idx;
                    cmd.Parameters.Add("@member_id", SqlDbType.VarChar).Value = _mailID;
                    cmd.Parameters.Add("@mailbox_code", SqlDbType.VarChar).Value = "1100";

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 메일 도메인의 domainIdx 값 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        private string GetDomainIdx(int _companyID, string _mailDomain)
        {
            try
            {
                string domainIdx = "";
                DataTableReader dtr = null;

                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = @"select domain_idx from zt_domain where domain_name='{0}' ";
                    query = string.Format(query, _mailDomain);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    dtr = ExecuteDataTableReader(cmd);
                }

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            domainIdx = dtr["domain_idx"].ToString();
                        }
                    }
                }

                return domainIdx;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 제타 MEMBER_SN 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        private string GetMemberSN(int _companyID, string _mailID, string _mailDomain)
        {
            try
            {
                string memberSN = "";
                DataTableReader dtr = null;

                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = @"SELECT member_sn FROM dbo.zt_userinfo INNER JOIN dbo.zt_domain
                            ON zt_userinfo.domain_idx = zt_domain.domain_idx
                            AND zt_userinfo.member_id = '{0}' AND zt_domain.domain_name = '{1}'	 ";
                    query = string.Format(query, _mailID, _mailDomain);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();

                    dtr = ExecuteDataTableReader(cmd);
                }

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            memberSN = dtr["member_sn"].ToString();
                        }
                    }
                }

                return memberSN;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 보낸편지함 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_listCount">The _list count.</param>
        /// <returns></returns>
        public DataTableReader GetUserSendMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            try
            {
                string domain_idx = GetDomainIdx(_companyID, _mailDomain);

                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SendList52");
                    query = string.Format(query, _listCount);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@domain_idx", SqlDbType.Int).Value = domain_idx;
                    cmd.Parameters.Add("@member_id", SqlDbType.VarChar).Value = _mailID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxList(int _companyID, string _mailID, string _mailDomain)
        {
            try
            {
                string domain_idx = GetDomainIdx(_companyID, _mailDomain);
                string memberSN = GetMemberSN(_companyID, _mailID, _mailDomain);
                using (var cn = new SqlConnection(ConnMAIL(1)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_MailBoxList52");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@domain_idx", SqlDbType.Int).Value = domain_idx;
                    cmd.Parameters.Add("@member_sn", SqlDbType.VarChar).Value = memberSN;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 특정 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_mailboxcode">The _mailboxcode.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxListByCode(int _companyID, string _mailID, string _mailDomain, string _mailboxcode, int _pageSize)
        {
            try
            {
                string domain_idx = GetDomainIdx(_companyID, _mailDomain);

                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Mail_SelZettaMail52");
                    query = string.Format(query, _pageSize);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@domain_idx", SqlDbType.Int).Value = domain_idx;
                    cmd.Parameters.Add("@member_id", SqlDbType.VarChar).Value = _mailID;
                    cmd.Parameters.Add("@mailbox_code", SqlDbType.VarChar).Value = _mailboxcode;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 승인 요청확인 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mail">The _mail.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailConfirmList(int _companyID, string _mail, int _pageSize)
        {
            try
            {
                return null;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }
}