using System;
using System.ComponentModel;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Common.BLL
{
    /// <summary>
    ///
    /// </summary>
    public class MailDeep55
    {
        private string MailServerIP = "", URL_GateUrl = "";
        private string PARM_ExistUser = "", PARM_AddUser = "", PARM_DelUser = "", PARM_ModUser = "";
        private string PARM_SLO = "";
        private string PARM_ListCnt = "", PARM_List = "", PARM_MailBox = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepBiz" /> class.
        /// </summary>
        public MailDeep55()
        {
            MailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
            URL_GateUrl = MailServerIP + AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_GateUrl", "/zeospace.ds");

            // 계정관리
            PARM_ExistUser = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_ExistUser", "act=isExist&domain={0}&id={1}");
            PARM_AddUser = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_AddUser", "act=add&domain={0}&id={1}&pw={2}&name={3}&status={4}&skin={5}&mailquota={6}&filequota={7}");
            PARM_DelUser = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_DelUser", "act=delete&domain={0}&id={1}&pw={2}");
            PARM_ModUser = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_ModUser", "act=modify&domain={0}&id={1}&pw={2}&newPw={3}&newName={4}&newStatus={5}&newSkin={6}&newMailquota={7}&newFilequota={8}");

            // SLO
            PARM_SLO = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_SLO", "act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}");

            // 리스트관리
            PARM_ListCnt = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_ListCnt", "act=mailCount&domain={0}&id={1}&pw={2}&mbox={3}&type={4}");
            PARM_List = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_List", "act=mailList&domain={0}&id={1}&pw={2}&mbox={3}&type={4}&count={5}&offset={6}");
            PARM_MailBox = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_Folder", "act=mailBoxList&domain={0}&id={1}&pw={2}");
        }

        #region 공통

        /// <summary>
        /// 데이타 전송
        /// </summary>
        /// <param name="_func">The _func.</param>
        /// <param name="_url">The _url.</param>
        /// <param name="_parm">The _parm.</param>
        /// <returns></returns>
        public XmlNode SendPost(string _func, string _url, string _parm)
        {
            if (_func.Equals("") || _url.Equals("") || _parm.Equals("")) return null;
            AppException.WriteAssert("(Before) Func : " + _func + ", URL : " + _url + ", PARM : " + _parm, AppSettingType.MAIL);

            try
            {
                // 전송
                string result = UrlSendHelper.UsePost(_url, _parm);
                AppException.WriteAssert("(After) Func : " + _func + "-Result : " + result, AppSettingType.MAIL);

                XmlNode rootNode = null;
                if (!result.Equals(""))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);

                    rootNode = xmlDoc.DocumentElement;
                    string returnCode = rootNode.SelectSingleNode("/data/returnCode").InnerText;
                    string returnMsg = rootNode.SelectSingleNode("/data/returnMsg").InnerText;

                    if (!returnCode.Equals("0"))
                    {
                        AppException.WriteAssert("(Error) returnCode : " + returnCode + ", returnMsg : " + returnMsg, AppSettingType.MAIL);
                        rootNode = null;
                    }
                }

                return rootNode;
            }
            catch (Exception ex)
            {
                AppException.WriteException("SendPost : " + ex.Message);
                return null; //throw
            }
        }

        #endregion 공통

        #region 계정관리(Add/Mod/Del)

        /// <summary>
        /// 계정 존재여부
        /// true :계정부재 , false :계정존재
        /// </summary>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_id">The _id.</param>
        /// <returns></returns>
        public bool IsNonExistUser(string _domain, string _id)
        {
            bool rtnval = false;

            if (_domain.Equals("") || _id.Equals("")) return rtnval;
            AppException.WriteAssert("IsExistUser (" + _domain + ", " + _id + ")", AppSettingType.MAIL);

            PARM_ExistUser = string.Format(PARM_ExistUser, _domain, _id);   //DEEP_ExistUser : act=isExist&amp;domain={0}&amp;id={1}

            // 전송
            XmlNode rootNode = SendPost("IsExistUser", URL_GateUrl, PARM_ExistUser);
            if (rootNode != null)
            {
                // true :계정부재 , false :계정존재
                rtnval = (rootNode.SelectSingleNode("/data/result").InnerText.Equals("0")) ? true : false;
            }

            return rtnval;
        }

        /// <summary>
        /// 계정 추가
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_userStat">The _user stat.</param>
        /// <param name="_userSkin">The _user skin.</param>
        /// <param name="_mailQuota">The _mail quota.</param>
        /// <param name="_fileQuota">The _file quota.</param>
        /// <returns></returns>
        public bool AddUser(int _companyID, string _domain, string _mailID, string _pw,
            string _name, UserStatus _userStat, UserSkin _userSkin, string _mailQuota, string _fileQuota)
        {
            try
            {
                bool rtnval = false;

                string userStat = AppSettingHelper.GetDescription(_userStat);
                string userSkin = AppSettingHelper.GetDescription(_userSkin);

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("") || _name.Equals("") || userStat.Equals("") || userSkin.Equals("")) return rtnval;

                //true :계정부재
                if (IsNonExistUser(_domain, _mailID))
                {
                    //DEEP_AddUser : act=add&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;name={3}&amp;status={4}&amp;skin={5}&amp;mailquota={6}&amp;filequota={7}
                    PARM_AddUser = string.Format(PARM_AddUser, _domain, _mailID, _pw, _name, userStat, userSkin, _mailQuota, _fileQuota);

                    AppException.WriteAssert("AddUser (사용자 추가) : MailID(" + _mailID + "), Domain(" + _domain + ")", AppSettingType.MAIL);

                    XmlNode rootNode = SendPost("AddUser", URL_GateUrl, PARM_AddUser);
                    rtnval = (rootNode != null) ? true : false;
                }
                else
                {
                    AppException.WriteAssert("AddUser (기존사용자있음) : MailID(" + _mailID + "), Domain(" + _domain + ")", AppSettingType.MAIL);
                    rtnval = true;
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                AppException.WriteException("AddUser : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 정보수정 (변경대상: PW/이름/메일할당량/파일할당량/계정상태/스킨정보)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_newPw">The _new pw.</param>
        /// <param name="_newName">Name of the _new.</param>
        /// <param name="_userStat">0:정상,1:정지,2:신청,3:휴면</param>
        /// <param name="_userSkin">black:검정색,blue:파랑색,bluegray:청회색,bluesilver:남색,green:초록색,orange:주황색,purple:보라색</param>
        /// <param name="_newMailQuota">The _new mail quota.</param>
        /// <param name="_newFileQuota">The _new file quota.</param>
        /// <returns></returns>
        public bool ModUser(int _companyID, string _domain, string _mailID, string _pw, string _newPw,
            string _newName, UserStatus _userStat, UserSkin _userSkin, string _newMailQuota, string _newFileQuota)
        {
            try
            {
                bool rtnval = false;

                string userStat = AppSettingHelper.GetDescription(_userStat);
                string userSkin = AppSettingHelper.GetDescription(_userSkin);

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("") || userStat.Equals("") || userSkin.Equals("")) return rtnval;

                //false :계정존재
                if (!IsNonExistUser(_domain, _mailID))
                {
                    //DEEP_ModUser : act=modify&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;newPw={3}&amp;newName={4}&amp;newStatus={5}&amp;newSkin={6}&amp;newMailquota={7}&amp;newFilequota={8}
                    PARM_ModUser = string.Format(PARM_ModUser, _domain, _mailID, _pw, _newPw, _newName, userStat, userSkin, _newMailQuota, _newFileQuota);

                    XmlNode rootNode = SendPost("ModUser", URL_GateUrl, PARM_ModUser);
                    rtnval = (rootNode != null) ? true : false;
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                AppException.WriteException("ModUser : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 메일 사용자 계정 삭제
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public bool DelUser(int _companyID, string _domain, string _mailID, string _pw)
        {
            try
            {
                bool rtnval = false;

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("")) return rtnval;

                // DEEP_DelUser : act=delete&amp;domain={0}&amp;id={1}&amp;pw={2}
                PARM_DelUser = string.Format(PARM_DelUser, _domain, _mailID, _pw);

                XmlNode rootNode = SendPost("DelUser", URL_GateUrl, PARM_DelUser);
                rtnval = (rootNode != null) ? true : false;

                return rtnval;
            }
            catch (Exception ex)
            {
                AppException.WriteException("DelUser : " + ex.Message);
                throw;
            }
        }

        #endregion 계정관리(Add/Mod/Del)

        #region 딥 메일 화면 경로 제공하기

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
        public string GetMailListUrl(int _companyID, string _domain, string _mailID, string _pw, UserLang _lang, string _mbox)
        {
            try
            {
                string lang = AppSettingHelper.GetDescription(_lang);
                string lp = AppSettingHelper.GetDescription(UserLP.MailList);
                //string mbox = AppSettingHelper.GetDescription(_mbox);

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("") || lang.Equals("") || lp.Equals("") || _mbox.Equals("")) return "";

                //act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}
                PARM_SLO = string.Format(PARM_SLO, _domain, _mailID, _pw, lang, lp, _mbox, "", "", "", "1");

                return URL_GateUrl + "?" + PARM_SLO;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetMailListUrl : " + ex.Message);
                throw;
            }
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
        /// <param name="_msgKey">The _MSG key.</param>
        /// <returns></returns>
        public string GetMailReadUrl(int _companyID, string _domain, string _mailID, string _pw, UserLang _lang, string _mbox, string _msgKey)
        {
            try
            {
                string lang = AppSettingHelper.GetDescription(_lang);
                string lp = AppSettingHelper.GetDescription(UserLP.MailRead);
                //string mbox = AppSettingHelper.GetDescription(_mbox);

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("") || lang.Equals("") || lp.Equals("") || _mbox.Equals("") || _msgKey.Equals("")) return "";

                //act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}
                PARM_SLO = string.Format(PARM_SLO, _domain, _mailID, _pw, lang, lp, _mbox, _msgKey, "", "", "1");

                AppException.WriteAssert("Func : SLORead, URL :" + URL_GateUrl + ", PARM : " + PARM_SLO, AppSettingType.MAIL);

                return URL_GateUrl + "?" + PARM_SLO;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetMailReadUrl : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 메일 쓰기 페이지 경로 값
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_domain">The _domain.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_lang">The _lang.</param>
        /// <param name="_msgKey">The _MSG key.</param>
        /// <param name="_to">The _to.</param>
        /// <param name="_contHtml">The _cont HTML.</param>
        /// <returns></returns>
        public string GetMailWriteUrl(int _companyID, string _domain, string _mailID, string _pw, UserLang _lang, string _msgKey, string _to, string _contHtml)
        {
            try
            {
                string lang = AppSettingHelper.GetDescription(_lang);
                string lp = AppSettingHelper.GetDescription(UserLP.MailWrite);

                if (_domain.Equals("") || _mailID.Equals("") || _pw.Equals("") || lang.Equals("") || lp.Equals("")) return "";

                //act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}
                PARM_SLO = string.Format(PARM_SLO, _domain, _mailID, _pw, lang, lp, "", _msgKey, _to, _contHtml, "1");

                return URL_GateUrl + "?" + PARM_SLO;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetMailWriteUrl : " + ex.Message);
                throw;
            }
        }

        #endregion 딥 메일 화면 경로 제공하기

        #region 리스트관리 (Get)

        #region 건수 가져오기

        /// <summary>
        /// 계정 편지 건수 (ID 기준)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mBoxID">The _m box identifier.</param>
        /// <param name="_mType">Type of the _m.</param>
        /// <returns></returns>
        public int GetListCnt(int _companyID, string _mailID, string _mailDomain, string _pw, MBoxID _mBoxID, MType _mType)
        {
            try
            {
                string mBoxID = AppSettingHelper.GetDescription(_mBoxID);
                string mType = AppSettingHelper.GetDescription(_mType);

                if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("") || mBoxID.Equals("") || mType.Equals("")) return -1;

                int rtnval = 0;

                //DEEP_ListCnt : act=mailCount&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;mbox={3}&amp;type={4}
                PARM_ListCnt = string.Format(PARM_ListCnt, _mailDomain, _mailID, _pw, mBoxID, mType);
                XmlNode rootNode = SendPost("GetListCnt", URL_GateUrl, PARM_ListCnt);
                if (rootNode != null)
                {
                    rtnval = rootNode.SelectSingleNode("/data/result").InnerText.ToNumber<int>();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetListCnt : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 계정 편지 건수 (ID 기준) - 개인메일함(UID)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mBoxUID">The _m box uid.</param>
        /// <param name="_mType">Type of the _m.</param>
        /// <returns></returns>
        public int GetListCnt(int _companyID, string _mailID, string _mailDomain, string _pw, string _mBoxUID, MType _mType)
        {
            try
            {
                string mType = AppSettingHelper.GetDescription(_mType);

                if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("") || _mBoxUID.Equals("") || mType.Equals("")) return -1;

                int rtnval = 0;

                //DEEP_ListCnt : act=mailCount&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;mbox={3}&amp;type={4}
                PARM_ListCnt = string.Format(PARM_ListCnt, _mailDomain, _mailID, _pw, _mBoxUID, mType);
                XmlNode rootNode = SendPost("GetListCnt", URL_GateUrl, PARM_ListCnt);
                if (rootNode != null)
                {
                    rtnval = rootNode.SelectSingleNode("/data/result").InnerText.ToNumber<int>();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetListCnt : " + ex.Message);
                throw;
            }
        }

        #endregion 건수 가져오기

        #region 리스트 가져오기

        /// <summary>
        /// 계정 편지 리스트 (ID 기준)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mBoxID">The _m box identifier.</param>
        /// <param name="_mType">Type of the _m.</param>
        /// <param name="_count">The _count.</param>
        /// <param name="_page">The _page.</param>
        /// <returns></returns>
        public ObjList GetListBoxID(int _companyID, string _mailID, string _mailDomain, string _pw, MBoxID _mBoxID, MType _mType, int _count, int _page)
        {
            try
            {
                string mBoxID = AppSettingHelper.GetDescription(_mBoxID);
                string mType = AppSettingHelper.GetDescription(_mType);

                if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("") || mBoxID.Equals("") || mType.Equals("") || _count < 0 || _page < 0) return null;

                ObjList listobj = new ObjList();

                //DEEP_List : act=mailList&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;mbox={3}&amp;type={4}&amp;count={5}&amp;offset={6}
                PARM_List = string.Format(PARM_List, _mailDomain, _mailID, _pw, mBoxID, mType, _count.ToString(), _page.ToString());
                XmlNode rootNode = SendPost("GetListBoxID", URL_GateUrl, PARM_List);
                if (rootNode != null)
                {
                    XmlNode nodeChk = rootNode.SelectSingleNode("/data/result");
                    if (nodeChk != null)
                    {
                        XmlNodeList nodeList = rootNode.SelectSingleNode("/data/result").ChildNodes;

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            BE.DeepMailInfo info = new BE.DeepMailInfo();
                            info.uid = nodeList.Item(i).SelectSingleNode("uid").InnerText;
                            info.muid = nodeList.Item(i).SelectSingleNode("muid").InnerText;
                            info.subject = nodeList.Item(i).SelectSingleNode("subject").InnerText;
                            info.from = nodeList.Item(i).SelectSingleNode("from").InnerText;
                            info.to = nodeList.Item(i).SelectSingleNode("to").InnerText;
                            info.cc = nodeList.Item(i).SelectSingleNode("cc").InnerText;
                            info.bcc = nodeList.Item(i).SelectSingleNode("bcc").InnerText;
                            info.sendDate = Convert.ToDateTime(nodeList.Item(i).SelectSingleNode("sendDate").InnerText);
                            info.recvDate = Convert.ToDateTime(nodeList.Item(i).SelectSingleNode("recvDate").InnerText);
                            info.flag = nodeList.Item(i).SelectSingleNode("flag").InnerText;
                            info.isAttach = nodeList.Item(i).SelectSingleNode("isAttach").InnerText;
                            info.isRead = nodeList.Item(i).SelectSingleNode("isRead").InnerText;

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetListBoxID : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 계정 편지 리스트 (ID 기준) - 개인메일함(UID)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <param name="_mBoxUID">The _m box uid.</param>
        /// <param name="_mType">Type of the _m.</param>
        /// <param name="_count">The _count.</param>
        /// <param name="_page">The _page.</param>
        /// <returns></returns>
        public ObjList GetListBoxUID(int _companyID, string _mailID, string _mailDomain, string _pw, string _mBoxUID, MType _mType, int _count, int _page)
        {
            try
            {
                string mType = AppSettingHelper.GetDescription(_mType);

                if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("") || _mBoxUID.Equals("") || mType.Equals("") || _count < 0 || _page < 0) return null;

                ObjList listobj = new ObjList();

                //DEEP_List : act=mailList&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;mbox={3}&amp;type={4}&amp;count={5}&amp;offset={6}
                PARM_List = string.Format(PARM_List, _mailDomain, _mailID, _pw, _mBoxUID, mType, _count.ToString(), _page.ToString());
                XmlNode rootNode = SendPost("GetListBoxUID", URL_GateUrl, PARM_List);
                if (rootNode != null)
                {
                    XmlNodeList nodeList = rootNode.SelectSingleNode("/data/result").ChildNodes;

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        DeepMailInfo info = new DeepMailInfo();
                        info.uid = nodeList.Item(i).SelectSingleNode("uid").InnerText;
                        info.subject = nodeList.Item(i).SelectSingleNode("subject").InnerText;
                        info.from = nodeList.Item(i).SelectSingleNode("from").InnerText;
                        info.to = nodeList.Item(i).SelectSingleNode("to").InnerText;
                        info.cc = nodeList.Item(i).SelectSingleNode("cc").InnerText;
                        info.bcc = nodeList.Item(i).SelectSingleNode("bcc").InnerText;
                        info.sendDate = Convert.ToDateTime(nodeList.Item(i).SelectSingleNode("sendDate").InnerText);
                        info.recvDate = Convert.ToDateTime(nodeList.Item(i).SelectSingleNode("recvDate").InnerText);
                        info.flag = nodeList.Item(i).SelectSingleNode("flag").InnerText;
                        info.isAttach = nodeList.Item(i).SelectSingleNode("isAttach").InnerText;
                        info.isRead = nodeList.Item(i).SelectSingleNode("isRead").InnerText;

                        listobj.Add(info);
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetListBoxUID : " + ex.Message);
                throw;
            }
        }

        #endregion 리스트 가져오기

        #region 메일함 리스트가져오기

        /// <summary>
        /// 메일함 리스트가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_mailID">The _mail identifier.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <param name="_pw">The _PW.</param>
        /// <returns></returns>
        public ObjList GetMailBoxList(int _companyID, string _mailID, string _mailDomain, string _pw)
        {
            try
            {
                if (_mailID.Equals("") || _mailDomain.Equals("") || _pw.Equals("")) return null;

                ObjList listobj = new ObjList();

                //DEEP_Folder : act=mailBoxList&amp;domain={0}&amp;id={1}&amp;pw={2}
                PARM_MailBox = string.Format(PARM_MailBox, _mailDomain, _mailID, _pw);
                XmlNode rootNode = SendPost("GetMailBoxList", URL_GateUrl, PARM_MailBox);
                if (rootNode != null)
                {
                    XmlNodeList nodeList = rootNode.SelectSingleNode("/data/result").ChildNodes;

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        DeepMailFolderInfo info = new DeepMailFolderInfo();
                        info.uid = nodeList.Item(i).SelectSingleNode("uid").InnerText;
                        info.puid = nodeList.Item(i).SelectSingleNode("puid").InnerText;
                        info.name = nodeList.Item(i).SelectSingleNode("name").InnerText;
                        info.totalSize = (nodeList.Item(i).SelectSingleNode("totalSize").InnerText.Equals("")) ? 0 : nodeList.Item(i).SelectSingleNode("/totalSize").InnerText.ToNumber<float>();
                        info.totalCount = (nodeList.Item(i).SelectSingleNode("totalCount").InnerText.Equals("")) ? 0 : nodeList.Item(i).SelectSingleNode("/totalCount").InnerText.ToNumber<int>();
                        info.unseenCount = (nodeList.Item(i).SelectSingleNode("unseenCount").InnerText.Equals("")) ? 0 : nodeList.Item(i).SelectSingleNode("/unseenCount").InnerText.ToNumber<int>();

                        listobj.Add(info);
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                AppException.WriteException("GetMailBoxList : " + ex.Message);
                throw;
            }
        }

        #endregion 메일함 리스트가져오기

        #endregion 리스트관리 (Get)

        #region 웹계정상태, 스킨, 다국어, LP, MBOX, MTYPE

        /// <summary>
        /// 웹메일 계정상태
        /// </summary>
        public enum UserStatus
        {
            /// <summary>
            /// 정상
            /// </summary>
            [Description("0")]
            Normal = 0,         //정상

            /// <summary>
            /// 정지
            /// </summary>
            [Description("1")]
            Stop = 1,           //정지

            /// <summary>
            /// 신청
            /// </summary>
            [Description("2")]
            Application = 2,    //신청

            /// <summary>
            /// 휴면
            /// </summary>
            [Description("3")]
            Dormancy = 3        //휴면
        };

        /// <summary>
        /// 사용자 스킨정보
        /// </summary>
        public enum UserSkin
        {
            /// <summary>
            /// 검정색
            /// </summary>
            [Description("black")]
            Black = 0,          //검정색

            /// <summary>
            /// 파랑색
            /// </summary>
            [Description("blue")]
            Blue = 1,           //파랑색

            /// <summary>
            /// 청회색
            /// </summary>
            [Description("bluegray")]
            Bluegray = 2,       //청회색

            /// <summary>
            /// 남색
            /// </summary>
            [Description("bluesilver")]
            Bluesilver = 3,     //남색

            /// <summary>
            /// 초록색
            /// </summary>
            [Description("green")]
            Green = 4,          //초록색

            /// <summary>
            /// 주황색
            /// </summary>
            [Description("orange")]
            Orange = 5,         //주황색

            /// <summary>
            /// 보라색
            /// </summary>
            [Description("purple")]
            Purple = 6          //보라색
        };

        /// <summary>
        /// 웹메일 계정 언어 설정
        /// </summary>
        public enum UserLang
        {
            /// <summary>
            /// 한국어
            /// </summary>
            [Description("kr")]
            KR = 0,             //한국어

            /// <summary>
            /// 일본어
            /// </summary>
            [Description("jp")]
            JP = 1,             //일본어

            /// <summary>
            /// 영어
            /// </summary>
            [Description("en")]
            EN = 2,             //영어

            /// <summary>
            /// 중국어
            /// </summary>
            [Description("cn")]
            CN = 3              //중국어
        };

        /// <summary>
        /// 웹메일 첫페이지
        /// </summary>
        public enum UserLP
        {
            /// <summary>
            /// 메일홈
            /// </summary>
            [Description("home")]
            Home = 0,           //메일홈

            /// <summary>
            /// 메일목록
            /// </summary>
            [Description("mailList")]
            MailList = 1,       //메일목록

            /// <summary>
            /// 메일읽기
            /// </summary>
            [Description("mailRead")]
            MailRead = 2,       //메일읽기

            /// <summary>
            /// 메일쓰기
            /// </summary>
            [Description("mailWrite")]
            MailWrite = 3       //메일쓰기
        }

        /// <summary>
        /// 편지함종류 (웹메일 계정 편지함 이름)
        /// </summary>
        public enum MBoxID
        {
            /// <summary>
            /// entire : 전체
            /// </summary>
            [Description("entire")]
            Entire = 0,         //전체

            /// <summary>
            /// basic : 받은메일함+개인메일함
            /// </summary>
            [Description("basic")]
            Basic = 1,          //받은메일함+개인메일함

            /// <summary>
            /// 1 : 받은편지함
            /// </summary>
            [Description("1")]
            Inbox = 2,          //받은편지함

            /// <summary>
            /// 2 : 보낼편지함
            /// </summary>
            [Description("2")]
            Wait = 3,           //보낼편지함

            /// <summary>
            /// 3 : 보낸편지함
            /// </summary>
            [Description("3")]
            Sent = 4,           //보낸편지함

            /// <summary>
            /// 4 : 임시보관함
            /// </summary>
            [Description("4")]
            Draft = 5,          //임시보관함

            /// <summary>
            /// 5 : 예약편지함
            /// </summary>
            [Description("5")]
            Reserved = 6,       //예약편지함

            /// <summary>
            /// 6 : 지운편지함
            /// </summary>
            [Description("6")]
            Trash = 7           //지운편지함
        };

        /// <summary>
        /// 편지종류 (웹메일 계정 편지 종류)
        /// </summary>
        public enum MType
        {
            /// <summary>
            /// Total : 0
            /// </summary>
            [Description("0")]
            Total = 0,    //전체

            /// <summary>
            /// Attach : 1
            /// </summary>
            [Description("1")]
            Attach = 1,   //첨부파일 메일

            /// <summary>
            /// New : 2
            /// </summary>
            [Description("2")]
            New = 2,      //새편지

            /// <summary>
            /// NoRead : 4
            /// </summary>
            [Description("4")]
            NoRead = 4    //읽지않은 메일
        };

        /// <summary>
        /// Gets the skin.
        /// </summary>
        /// <param name="_userSkin">The _user skin.</param>
        /// <returns></returns>
        public UserSkin GetSkin(string _userSkin)
        {
            UserSkin rtnval = UserSkin.Blue;

            switch (_userSkin.ToLower())
            {
                case "black":
                    rtnval = UserSkin.Black;
                    break;

                case "blue":
                    rtnval = UserSkin.Blue;
                    break;

                case "bluegray":
                    rtnval = UserSkin.Bluegray;
                    break;

                case "bluesilver":
                    rtnval = UserSkin.Bluesilver;
                    break;

                case "green":
                    rtnval = UserSkin.Green;
                    break;

                case "orange":
                    rtnval = UserSkin.Orange;
                    break;

                case "purple":
                    rtnval = UserSkin.Purple;
                    break;

                default:
                    rtnval = UserSkin.Blue;
                    break;
            }

            return rtnval;
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <param name="_lang">The _lang.</param>
        /// <returns></returns>
        public UserLang GetLang(string _lang)
        {
            UserLang rtnval = UserLang.KR;

            switch (_lang.ToLower())
            {
                case "kr":
                    rtnval = UserLang.KR;
                    break;

                case "jp":
                    rtnval = UserLang.JP;
                    break;

                case "en":
                    rtnval = UserLang.EN;
                    break;

                case "cn":
                    rtnval = UserLang.CN;
                    break;

                case "ja":
                    rtnval = UserLang.JP;
                    break;

                case "zh-cn":
                    rtnval = UserLang.CN;
                    break;

                case "zh-tw":
                    rtnval = UserLang.CN;
                    break;

                default:
                    rtnval = UserLang.KR;
                    break;
            }

            return rtnval;
        }

        #endregion 웹계정상태, 스킨, 다국어, LP, MBOX, MTYPE
    }
}