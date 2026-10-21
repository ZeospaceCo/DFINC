using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class MailDeepFactory
    {
        private string MailProduct = string.Empty, ServerUrl = string.Empty, IsUserDel = string.Empty;

        public MailDeepFactory()
        {
            //메일 업체
            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            ServerUrl = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://zetta.zeospace.com:4040");
            IsUserDel = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_IsUserDel", "0");     //사용자삭제시, 영구삭제여부 (정지-0/삭제-1/휴면-2)
        }

        #region 메일 계정 추가, 수정, 삭제

        /// <summary>
        /// 메일 사용자 계정 추가
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_mailQuota">The _mail quota.</param>
        /// <param name="_fileQuota">The _file quota.</param>
        /// <returns></returns>
        public bool AddUser(int _companyID, string _domain, string _mailID, string _pw, string _name,
            string _mailQuota, string _fileQuota)
        {
            bool rtnval = false;

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("") || _name.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                rtnval = deep.AddUser(_companyID, _domain, _mailID, _pw, _name, MailDeep55.UserStatus.Normal, MailDeep55.UserSkin.Blue, _mailQuota, _fileQuota);
            }
            return rtnval;
        }

        /// <summary>
        /// 메일 사용자 계정 수정
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <param name="_newName">Name of the _new.</param>
        /// <param name="_newMailQuota">The _new mail quota.</param>
        /// <param name="_newFileQuota">The _new file quota.</param>
        /// <param name="_userSkin">The _user skin.</param>
        /// <param name="_isAcive">if set to <c>true</c> [_is acive].</param>
        /// <returns></returns>
        public bool ModUser(int _companyID, string _domain, string _mailID, string _pw, string _newPw, 
            string _newName, string _newMailQuota, string _newFileQuota, string _userSkin, bool _isAcive)
        {
            bool rtnval = false;

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                MailDeep55.UserStatus userState = (_isAcive) ? MailDeep55.UserStatus.Normal : MailDeep55.UserStatus.Stop;

                rtnval = deep.ModUser(_companyID, _domain, _mailID, _pw, _newPw, _newName, userState, deep.GetSkin(_userSkin), _newMailQuota, _newFileQuota);
            }

            return rtnval;
        }

        /// <summary>
        /// 메일계정삭제
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public bool DelUser(int _companyID, string _domain, string _mailID, string _pw)
        {
            bool rtnval = false;

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("")) return rtnval;

            // 설정에 따라,영구삭제여부 체크
            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                if (IsUserDel.Equals("1"))
                {
                    rtnval = deep.DelUser(_companyID, _domain, _mailID, _pw);
                }
                else
                {
                    MailDeep55.UserStatus userState = (IsUserDel.Equals("0")) ? MailDeep55.UserStatus.Stop : MailDeep55.UserStatus.Dormancy;
                    rtnval = deep.ModUser(_companyID, _domain, _mailID, _pw, "", "", userState, MailDeep55.UserSkin.Blue, "", "");
                }
            }
            return rtnval;
        }

        /// <summary>
        /// 등록여부
        /// true :계정부재 , false :계정존재
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_id">The _id.</param>
        /// <returns></returns>
        public bool IsNonExistUser(int _companyID, string _domain, string _id)
        {
            bool rtnval = false;

            if (_domain.Equals("") || _id.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();
                //true :계정부재 , false :계정존재
                rtnval = deep.IsNonExistUser(_domain, _id);
            }

            return rtnval;
        }

        #endregion 메일 계정 추가, 수정, 삭제

        #region 메일 화면 경로 제공하기

        /// <summary>
        /// 메일 목록 페이지 경로 값
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_lang">The _lang.</param>
        /// <param name="_mbox">The _mbox.</param>
        /// <returns></returns>
        public string GetMailListUrl(int _companyID, string _domain, string _mailID, string _pw, string _lang, string _mbox)
        {
            string rtnval = "";

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("") || _lang.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();
                rtnval = deep.GetMailListUrl(_companyID, _domain, _mailID, _pw, deep.GetLang(_lang), _mbox);
            }

            return rtnval;
        }

        /// <summary>
        /// 메일 읽기 페이지 경로 값
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_lang">The _lang.</param>
        /// <param name="_mbox">The _mbox.</param>
        /// <param name="_msgKey">웹메일 계정 메일 인덱스 키</param>
        /// <returns></returns>
        public string GetMailReadUrl(int _companyID, string _domain, string _mailID, string _pw, string _lang, string _mbox, string _msgKey)
        {
            string rtnval = "";

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("") || _lang.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();
                rtnval = deep.GetMailReadUrl(_companyID, _domain, _mailID, _pw, deep.GetLang(_lang), _mbox, _msgKey);
            }

            return rtnval;
        }

        /// <summary>
        /// 메일 쓰기 페이지 경로 값
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_lang">The _lang.</param>
        /// <param name="_msgKey">전달인경우 uid(mail read 에서 넘어온 값)</param>
        /// <param name="_to">The _to.</param>
        /// <param name="_contHtml">The _cont HTML.</param>
        /// <returns></returns>
        public string GetMailWriteUrl(int _companyID, string _domain, string _mailID, string _pw, string _lang, string _msgKey, string _to, string _contHtml)
        {
            string rtnval = "";

            if (_mailID.Equals("") || _domain.Equals("") || _pw.Equals("") || _lang.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();
                //_msgKey : 메일 글의 고유 키값
                rtnval = deep.GetMailWriteUrl(_companyID, _domain, _mailID, _pw, deep.GetLang(_lang), _msgKey, _to, _contHtml);
            }

            return rtnval;
        }

        #endregion 메일 화면 경로 제공하기

        #region 받은편지함

        /// <summary>
        /// 받은메일함 + 개인메일함의 안읽은 메일 건수(포틀릿)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public int GetBasicNoReadCnt(int _companyID, string _mailID, string _mailDomain, string _pw)
        {
            int rtnval = -1;

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                //Basic : 받은메일함 + 개인메일함이며, 안읽은 기준
                rtnval = deep.GetListCnt(_companyID, _mailID, _mailDomain, _pw, MailDeep55.MBoxID.Basic, MailDeep55.MType.NoRead);
            }
            return rtnval;
        }

        /// <summary>
        /// 받은메일함 + 개인메일함 리스트(포틀릿)
        /// 계정 편지 리스트 (ID 기준)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_cnt">The _CNT.</param>
        /// <param name="_page">The _page.</param>
        /// <returns></returns>
        public ObjList GetBasicTotalList(int _companyID, string _mailID, string _mailDomain, string _pw, int _cnt, int _page)
        {
            ObjList rtnval = new ObjList();

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                //Basic : 받은메일함 + 개인메일함이며, 전체 가져오기
                rtnval = deep.GetListBoxID(_companyID, _mailID, _mailDomain, _pw, MailDeep55.MBoxID.Basic, MailDeep55.MType.Total, _cnt, _page);
            }
            return rtnval;
        }

        #endregion 받은편지함

        #region 보낸편지함

        /// <summary>
        /// 보낸편지함 건수
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public int GetSentTotalCnt(int _companyID, string _mailID, string _mailDomain, string _pw)
        {
            int rtnval = -1;

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                rtnval = deep.GetListCnt(_companyID, _mailID, _mailDomain, _pw, MailDeep55.MBoxID.Sent, MailDeep55.MType.Total);
            }
            return rtnval;
        }

        /// <summary>
        /// 보낸편지함 리스트(포틀릿)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_cnt">The _CNT.</param>
        /// <param name="_page">The _page.</param>
        /// <returns></returns>
        public ObjList GetSentTotalList(int _companyID, string _mailID, string _mailDomain, string _pw, int _cnt, int _page)
        {
            ObjList rtnval = new ObjList();

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                //Sent : 보낸편지함
                rtnval = deep.GetListBoxID(_companyID, _mailID, _mailDomain, _pw, MailDeep55.MBoxID.Sent, MailDeep55.MType.Total, _cnt, _page);
            }
            return rtnval;
        }

        #endregion 보낸편지함

        #region 특정 개인함 메일 (미사용)

        /// <summary>
        /// 특정 개인함 메일 리스트
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mboxUID">The _mbox uid.</param>
        /// <returns></returns>
        public int GetMailBoxTotalCnt(int _companyID, string _mailID, string _mailDomain, string _pw, string _mboxUID)
        {
            int rtnval = -1;

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                rtnval = deep.GetListCnt(_companyID, _mailID, _mailDomain, _pw, _mboxUID, MailDeep55.MType.Total);
            }
            return rtnval;
        }

        /// <summary>
        /// 특정 개인함 메일 건수
        /// 계정 편지 리스트 (ID 기준) - 개인메일함(UID)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mboxUID">The _mbox uid.</param>
        /// <param name="_cnt">The _CNT.</param>
        /// <param name="_page">The _page.</param>
        /// <returns></returns>
        public ObjList GetMailBoxTotalList(int _companyID, string _mailID, string _mailDomain, string _pw, string _mboxUID, int _cnt, int _page)
        {
            ObjList rtnval = new ObjList();

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                rtnval = deep.GetListBoxUID(_companyID, _mailID, _mailDomain, _pw, _mboxUID, MailDeep55.MType.Total, _cnt, _page);
            }
            return rtnval;
        }

        /// <summary>
        /// 개인함 리스트 (미사용임)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public ObjList GetMailBoxList(int _companyID, string _mailID, string _mailDomain, string _pw)
        {
            ObjList rtnval = new ObjList();

            if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return rtnval;

            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                MailDeep55 deep = new MailDeep55();

                rtnval = deep.GetMailBoxList(_companyID, _mailID, _mailDomain, _pw);
            }
            return rtnval;
        }

        #endregion 특정 개인함 메일 (미사용)
    }
}