using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.BLL
{
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 추가하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.ListInfo _info)
        {
            if (_info == null) return -1;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();
                
                string yearMonth = FileHelper.GetYearMonth();

                // 리스트 추가
                int proposalNo = dalList.Add(_info, yearMonth);
                _info.ProposalNo = proposalNo;

                DAL.Person person = new DAL.Person();
                #region 제안자(본인)

                person.Add(_info.CompanyID, _info.ProposalNo, _info.UserID, "W");

                #endregion

                #region 공동 제안자
                if (_info.CoWorkUserIDList.Length > 0)
                {
                    foreach (string userID in _info.CoWorkUserIDList.Split(','))
                    {
                        person.Add(_info.CompanyID, _info.ProposalNo, userID.ToNumber<int>(), "P");
                    }
                }
                #endregion

                #region 1차 심사자
                if (_info.JudgeUserIDList.Length > 0)
                {
                    DAL.JudgeUser judge = new DAL.JudgeUser();

                    foreach (string userID in _info.JudgeUserIDList.Split(','))
                    {
                        judge.Add(_info.CompanyID, _info.ProposalNo, userID.ToNumber<int>());
                    }
                }
                #endregion

                #region 첨부파일
                if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));

                    BLL.Attach attach = new Attach();
                    
                    attach.Add(_info.CompanyID, _info.ProposalNo, _info.HiddenSpoolPath, "");
                }
                #endregion

                ts.Complete();

                return proposalNo;
            }
        }
        #endregion

        #region DEL

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 취소(삭제)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _proposalNo, string _gubunCode)
        {
            if (_companyID < 1 || _proposalNo < 1 || _gubunCode == "")
                return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                #region List

                DAL.List list = new DAL.List();
                list.Del(_companyID, _proposalNo);

                #endregion

                #region Person (공동제안자)

                DAL.Person person = new DAL.Person();
                person.Del(_companyID, _proposalNo, _gubunCode);

                #endregion

                #region JudgeUser (1차 심사자)

                DAL.JudgeUser user = new DAL.JudgeUser();
                user.Del(_companyID, _proposalNo);
                
                #endregion

                #region 첨부파일

                DAL.Attach attach = new DAL.Attach();
                attach.Del(_companyID, _proposalNo);

                #endregion

                ts.Complete();
            }

            return true;
        }

        #endregion

        #region Mod

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 제안 수정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _info)
        {
            if (_info == null) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                // 리스트 수정
                DAL.List list = new DAL.List();
                list.Mod(_info);

                #region 공동제안자 수정

                DAL.Person person = new DAL.Person();

                // 기존의 공동제안자 삭제 후 다시 추가
                person.Del(_info.CompanyID, _info.ProposalNo, "P");

                if (_info.CoWorkUserIDList.Length > 0)
                {
                    foreach (string userID in _info.CoWorkUserIDList.Split(','))
                    {
                        person.Add(_info.CompanyID, _info.ProposalNo, userID.ToNumber<int>(), "P");
                    }
                }

                #endregion

                #region 1차 심사자 수정
                DAL.JudgeUser judge = new DAL.JudgeUser();

                // 기존 1차 심사자 삭제
                judge.Del(_info.CompanyID, _info.ProposalNo);

                if (_info.JudgeUserIDList.Length > 0)
                {
                    foreach (string userID in _info.JudgeUserIDList.Split(','))
                    {
                        judge.Add(_info.CompanyID, _info.ProposalNo, userID.ToNumber<int>());
                    }
                }

                #endregion

                #region 첨부파일
                if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));

                    BLL.Attach attach = new Attach();

                    attach.Add(_info.CompanyID, _info.ProposalNo, _info.HiddenSpoolPath, "");
                }
                #endregion
                ts.Complete();

                return true;
            }
        }

        #endregion
    }
}
