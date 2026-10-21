using System.Data;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class MailZetta52
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 포틀릿 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_listCount">The _list count.</param>
        /// <returns></returns>
        public DataTableReader GetMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            Zeospace.DeskPlus.Common.DAL.MailZetta52 dal = new DAL.MailZetta52();
            return dal.GetMailList(_companyID, _mailID, _mailDomain, _listCount);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 보낸 메일 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_listCount">The _list count.</param>
        /// <returns></returns>
        public DataTableReader GetUserSendMailList(int _companyID, string _mailID, string _mailDomain, int _listCount)
        {
            DAL.MailZetta52 dal = new DAL.MailZetta52();
            return dal.GetUserSendMailList(_companyID, _mailID, _mailDomain, _listCount);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxList(int _companyID, string _mailID, string _mailDomain)
        {
            DAL.MailZetta52 dal = new DAL.MailZetta52();
            return dal.GetUserMailBoxList(_companyID, _mailID, _mailDomain);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 특정 개인별 메일 박스 리스트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_user_sn">The _user_sn.</param>
        /// <param name="_mailboxcode">The _mailboxcode.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetUserMailBoxListByCode(int _companyID, string _mailID, string _mailDomain, string _mailboxcode, int _pageSize)
        {
            DAL.MailZetta52 dal = new DAL.MailZetta52();
            return dal.GetUserMailBoxListByCode(_companyID, _mailID, _mailDomain, _mailboxcode, _pageSize);
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
            DAL.MailZetta52 dal = new DAL.MailZetta52();
            return dal.GetUserMailConfirmList(_companyID, _mail, _pageSize);
        }
    }
}