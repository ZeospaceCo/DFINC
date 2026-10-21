using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Approval.Ws.EDM;
using System.Data;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BLL
{
    public class DocumentProcess
    {
        #region 발신문서

        /// <summary>
        /// 발신문서 상신
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sealPath"></param>
        /// <param name="_sendInfo"></param>
        /// <param name="_isLast"></param>
        /// <param name="_isNextZQ"></param>
        /// <returns></returns>
        public int Add(BE.DocumentProcessInfo _dpInfo, string _sealPath, BE.AccountInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 소스

            if (_dpInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return -1;

            try
            {
                int docID;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    docID = AddUnit(_dpInfo, _sealPath, _sendInfo, _isLast, _isNextZQ);
                    if (docID < 0)
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.AccountInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "W");

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[Add] : ", ex);
                return -1;
            }

            #endregion 소스
        }

        /// <summary>
        /// 발신문서 수정후재상신 (ZD)
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sealPathh"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sendInfo"></param>
        /// <param name="_isLast"></param>
        /// <param name="_isNextZQ"></param>
        /// <returns></returns>
        public int EditAdd(BE.DocumentProcessInfo _dpInfo, string _sealPath, int _groupID, BE.AccountInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 소스

            if (_dpInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return -1;

            try
            {
                int docID;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    docID = AddUnit(_dpInfo, _sealPath, _sendInfo, _isLast, _isNextZQ);
                    if (docID > 0)
                    {
                        // 업무톡 변경
                        Ws.Data.ShortReply srDal = new Ws.Data.ShortReply();
                        srDal.Mod(_dpInfo.CompanyID, _groupID, docID);

                        // 문서삭제(경유포함)
                        Ws.Data.DocumentProcess dpDal = new Ws.Data.DocumentProcess();
                        List<BE.DocumentProcessInfo> list = dpDal.GetDocuments(_dpInfo.CompanyID, _groupID);

                        DelDocProcess(list, true);
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.AccountInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "W");

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[EditAdd] : ", ex);
                return -1;
            }

            #endregion 소스
        }

        #endregion 발신문서

        #region 상신취소, 문서폐기(반려)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_docID"></param>
        /// <param name="_accountTag"></param>
        /// <returns></returns>
        public bool DeleteDoc(int _companyID, int _groupID, int _docID, BE.AccountInfo _sendInfo, string _accountTag)
        {
            if (_companyID.Equals(0) || _groupID.Equals(0) || _docID.Equals(0)) return false;

            try
            {
                Data.DocumentProcess dpDal2 = new Data.DocumentProcess();

                List<BE.DocumentProcessInfo> doclist = dpDal2.GetDocument(_companyID, _docID);

                List<BE.AccountInfo> targetList = GetAlarmTarget("CANCEL", doclist[0].CompanyID, doclist[0].DocumentID, doclist[0].GroupID, 1, doclist[0].AccountTag);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서삭제(경유포함)
                    Data.DocumentProcess dpDal = new Data.DocumentProcess();
                    List<BE.DocumentProcessInfo> list;

                    if ((_accountTag == "ZP") || ((_accountTag == "ZD") && (_groupID != _docID)))
                        list = dpDal.GetDocument(_companyID, _docID);
                    else
                        list = dpDal.GetDocuments(_companyID, _groupID);

                    DelDocProcess(list, true);

                    ts.Complete();
                }

                #region 알람발송

                SendAlarm(doclist[0], _sendInfo, targetList, "CA");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[DeleteDoc] : ", ex);
                return false;
            }
        }

        #endregion

        #region 단위로직

        /// <summary>
        /// 발신문서 상신
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sealPath"></param>
        /// <param name="_sendInfo"></param>
        /// <param name="_isLast"></param>
        /// <param name="_isNextZQ"></param>
        /// <returns></returns>
        public int AddUnit(BE.DocumentProcessInfo _dpInfo, string _sealPath, BE.AccountInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 소스

            if (_dpInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return -1;

            try
            {
                BE.SequenceInfo ZD0Info = null;
                int docID;

                Data.DocumentProcess dpDal = new Data.DocumentProcess();
                docID = dpDal.Add(_dpInfo);

                if (docID > 0)
                {
                    _dpInfo.DocumentID = docID;

                    #region DP_APP_Seq (결재라인)

                    if (_dpInfo.SequenceList != null)
                    {
                        if (_dpInfo.SequenceList.Count > 0)
                        {
                            Data.Sequence sDal = new Data.Sequence();
                            foreach (BE.SequenceInfo info in _dpInfo.SequenceList)
                            {
                                info.DocumentID = docID;
                                sDal.Add(info);

                                if (info.ApprovalTag.ToUpper().Equals("ZD0"))
                                {
                                    ZD0Info = info;
                                }
                            }
                        }
                    }

                    #endregion DP_APP_Seq (결재라인)

                    #region DP_APP_CounterPartUser (수신)

                    if (_dpInfo.CounterPartUserList != null)
                    {
                        if (_dpInfo.CounterPartUserList.Count > 0)
                        {
                            Data.CounterPartUser cpDal = new Data.CounterPartUser();
                            foreach (BE.CounterPartUserInfo info in _dpInfo.CounterPartUserList)
                            {
                                info.DocumentID = docID;
                                cpDal.Add(info);
                            }
                        }
                    }

                    #endregion DP_APP_CounterPartUser (수신)

                    #region DP_APP_CarbonCopyUser (참조)

                    if (_dpInfo.CarbonCopyUserList != null)
                    {
                        if (_dpInfo.CarbonCopyUserList.Count > 0)
                        {
                            Data.CarbonCopyUser ccDal = new Data.CarbonCopyUser();
                            foreach (BE.CarbonCopyUserInfo info in _dpInfo.CarbonCopyUserList)
                            {
                                info.DocumentID = docID;
                                ccDal.Add(info);
                            }
                        }
                    }

                    #endregion DP_APP_CarbonCopyUser (참조)

                    #region DP_APP_BaseDoc (근거문서)

                    if (_dpInfo.BaseDocumentList != null)
                    {
                        if (_dpInfo.BaseDocumentList.Count > 0)
                        {
                            Data.BaseDocument bDal = new Data.BaseDocument();
                            foreach (BE.BaseDocumentInfo info in _dpInfo.BaseDocumentList)
                            {
                                info.DocumentID = docID;
                                bDal.Add(info);
                            }
                        }
                    }

                    #endregion DP_APP_BaseDoc (근거문서)

                    #region DP_APP_Attach (첨부파일)

                    #region 이전 파일생성

                    if (_dpInfo.BeforeHiddenPath != null)
                    {
                        if (!_dpInfo.BeforeHiddenPath.Equals(""))
                        {
                            BLL.AttachFile attachDal = new AttachFile();
                            attachDal.Mod(_dpInfo.CompanyID, docID, _dpInfo.Number2, _dpInfo.BeforeHiddenPath);
                        }
                    }

                    #endregion 이전 파일생성

                    #region 신규 파일생성

                    if (_dpInfo.HiddenSpoolPath != null)
                    {
                        //  FileData/@CompanyID/Approval/Document/상신년월/@DocumentID/Attach
                        if (!_dpInfo.HiddenSpoolPath.Equals(""))
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_dpInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.AttachFile attachDal = new AttachFile();
                                attachDal.Add(_dpInfo.CompanyID, docID, _dpInfo.Number2, _dpInfo.HiddenSpoolPath);
                            }
                        }
                    }

                    #endregion 신규 파일생성

                    #endregion DP_APP_Attach (첨부파일)

                    #region 결재승인로직

                    if (_isLast.Equals("Y") && _isNextZQ.Equals("Y"))
                    {
                        _dpInfo.GroupID = (_dpInfo.GroupID.Equals(0)) ? docID : _dpInfo.GroupID;    // 다음 수신자 순서처리 시, 사용 (CounterPartUser's MyTurn)
                        _dpInfo.ApprovalState = "P";
                    }

                    // 1. 승인
                    if (!Decide(_dpInfo, ZD0Info, _sealPath))
                    {
                        return -1;
                    }

                    // 2. 최종결재여부(수신여부)
                    if (_isLast.Equals("Y"))
                    {
                        if (!Conclude(_dpInfo, _isNextZQ))
                        {
                            return -1;
                        }
                    }

                    #endregion 결재승인로직
                }
                else
                {
                    return -1;
                }

                return docID;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[AddUnit] : ", ex);
                throw;
            }

            #endregion 소스
        }

        /// <summary>
        /// 승인 (발신/승인)
        /// </summary>
        /// <param name="_dpInfo">dpInfo(ContentHTML,AccountTag)</param>
        /// <param name="_seqInfo">seqInfo()</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <returns></returns>
        private bool Decide(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _sealPath)
        {
            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sealPath == null) return false;

            try
            {
                #region 파일경로처리

                string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string serverName = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");                    //  ex) localhost
                string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH") + "/";  //  ex) /DeskPlusMobile/FileData/

                string targetFolder = baseFileDirectory + _dpInfo.CompanyID.ToString() + "/Approval/Document/" + _dpInfo.Number2 + "/" + _dpInfo.DocumentID.ToString();
                string targetFile = targetFolder + "/" + _dpInfo.AccountTag + "_seal" + _seqInfo.ApprovalTag + ".gif";

                string targetEIPFolder = "/DeskPlusEIP/FileData/" + _dpInfo.CompanyID.ToString() + "/Approval/Document/" + _dpInfo.Number2 + "/" + _dpInfo.DocumentID.ToString();
                string targetEIPFile = targetEIPFolder + "/" + _dpInfo.AccountTag + "_seal" + _seqInfo.ApprovalTag + ".gif";

                _dpInfo.ContentHTML = _dpInfo.ContentHTML.Replace(_sealPath, baseHttp + serverName + targetEIPFile);           //파일처리때문에 도메인정보는 여기서처리.

                #endregion 파일경로처리

                #region DB처리

                Data.Sequence seqDal = new Data.Sequence();

                string isNextUpd = "Y";

                if (_seqInfo.ApprovalTag.IndexOf("ZA").Equals(0))
                {
                    // 병렬합의의 경우, 미결재 존재여부 확인에 따라, 다음결재자 처리여부 분기.
                    int zaCnt = seqDal.GetZAUserCnt(_dpInfo.CompanyID, _dpInfo.DocumentID, _seqInfo.Sequence, _seqInfo.ApprovalTag);
                    isNextUpd = (zaCnt.Equals(0)) ? "Y" : "N";
                }

                // 전결이나, 반려일때는 다음결재자 처리 안함.
                if (_seqInfo.ApprovalType.Equals("A") || _seqInfo.ApprovalType.Equals("R"))
                {
                    isNextUpd = "N";
                }

                // Sequence & ApprovalDocument(S) 처리(DB) - 병렬체크
                if (!seqDal.ModDecide(_seqInfo, isNextUpd))
                {
                    return false;
                }

                // Document 처리(HTML, Viewing)
                Data.DocumentProcess docDal = new Data.DocumentProcess();
                if (!docDal.ModApprovalState(_dpInfo))
                {
                    return false;
                }

                #endregion DB처리

                #region 실제 인장복사

                if (!Directory.Exists(FileHelper.GetServerMapPath(targetFolder)))
                {
                    Directory.CreateDirectory(FileHelper.GetServerMapPath(targetFolder));
                }
                FileHelper.CopyFile(_sealPath, targetFile);      //파일처리때문에 도메인정보는 삭제.

                #endregion 실제 인장복사

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[Decide] : ", ex);
                throw;
            }

            #endregion 소스
        }

        /// <summary>
        /// 완결
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        private bool Conclude(BE.DocumentProcessInfo _dpInfo, string _isNextZQ)
        {
            if (_dpInfo == null || _isNextZQ == null) return false;

            try
            {
                bool rtnval = false;

                if (_isNextZQ.Equals("Y"))
                {
                    // 수신프로세스처리 - 수신정보 업데이트 (counterpartusers)
                    Data.CounterPartUser cpDal = new Data.CounterPartUser();
                    string ZQInfo = cpDal.ModNextZQ(_dpInfo.CompanyID, _dpInfo.GroupID);    //다음수신자정보 리턴 : ApprovalTag-UserID-DeptCodeID

                    rtnval = (!ZQInfo.Equals("")) ? true : false;
                }
                else
                {
                    rtnval = ConcludeZD(_dpInfo);
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[Conclude] : ", ex);
                throw;
            }
        }

        /// <summary>
        /// 발신완결
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <returns></returns>
        private bool ConcludeZD(BE.DocumentProcessInfo _dpInfo)
        {
            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                Data.DocumentProcess dpDal = new Data.DocumentProcess();

                #region 문서번호 생성

                string[] tmpInfo = dpDal.GetDocNumber(_dpInfo.CompanyID, _dpInfo.DocumentID).Split('^');
                //  tmpInfo = 문서번호^Number4
                if (tmpInfo == null)
                {
                    Helper.AppException.WriteException("Approval.BLL.DocumentProcess[Conclude] : 문서번호생성 오류 (CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
                    return false;
                }

                _dpInfo.ContentHTML = _dpInfo.ContentHTML.Replace("id=NO", "id=NO value='" + tmpInfo[0] + "'");
                _dpInfo.ContentHTML = _dpInfo.ContentHTML.Replace("id='NO'", "id=NO value='" + tmpInfo[0] + "'");
                _dpInfo.ContentHTML = _dpInfo.ContentHTML.Replace("id=\"NO\"", "id=\"NO\" value='" + tmpInfo[0] + "'");

                #endregion 문서번호 생성

                #region 완결호출(결재상태/문서내용처리/문서번호/완료일/문서함처리(부서별/개인별)/Doc,Seq Table이동 )

                bool rtnval = dpDal.ModConclude(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _dpInfo.AccountTag, "C", _dpInfo.ContentHTML, _dpInfo.ContentTEXT, tmpInfo[1].ToNumber<int>(), tmpInfo[0]);
                if (!rtnval)
                {
                    Helper.AppException.WriteException("Approval.BLL.DocumentProcess[ConcludeZD] : 완료SP 호출오류 (CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
                    return false;
                }

                #endregion 완결호출(결재상태/문서내용처리/문서번호/완료일/문서함처리(부서별/개인별)/Doc,Seq Table이동 )

                #region 외부연동모듈 호출 EDM
                string useLinked = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "USELINKED");
                if (useLinked == "Y")
                {
                    // edm 폴더로 문서 이관처리                                    
                    Transfer TransferEDM = new Transfer();
                    if (!TransferEDM.ApprovalToEDM(_dpInfo.CompanyID, _dpInfo.DocumentID))
                    {
                        Helper.AppException.WriteException("Approval.BLL.DocumentProcess[ApprovalToEDM] : EDM 호출오류(CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
                        return false;
                    }
                }
                #endregion 외부연동모듈 호출 EDM

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[ConcludeZD] : ", ex);
                throw;
            }

            #endregion 소스
        }

        /// <summary>
        /// 문서삭제 (특정문서대상)
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isFileDel">if set to <c>true</c> [_is file delete].</param>
        /// <returns></returns>
        public bool DelDocProcess(List<BE.DocumentProcessInfo> _info, bool _isFileDel)
        {
            if (_info.Count < 1) return false;

            try
            {
                string targetFolder = "";
                string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

                Data.DocumentProcess dpDal = new Data.DocumentProcess();
                BLL.CirculationDocument cirDoc = new CirculationDocument();

                foreach (BE.DocumentProcessInfo info in _info)
                {
                    if (dpDal.Del(info.CompanyID, info.DocumentID))
                    {
                        targetFolder = baseFileDirectory + "/" + info.CompanyID.ToString() + "/Approval/Document/" + info.Number2 + "/" + info.DocumentID.ToString();

                        //  ex) /DeskPlusMobile/FileData/1/Approval/Document/201206/37
                        if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);

                        //회람 문서 삭제하기
                        cirDoc.Del(info.CompanyID, info.DocumentID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[DelDocProcess] : ", ex);
                throw;
            }
        }
        #endregion

        #region 알림

        /// <summary>
        /// 알림대상
        /// </summary>
        /// <param name="_section"></param>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_accountTag"></param>
        /// <returns></returns>
        private List<BE.AccountInfo> GetAlarmTarget(string _section, int _companyID, int _documentID, int _groupID, int _sequence, string _accountTag)
        {
            if (_section.Equals("") || _companyID.Equals(0) || _documentID.Equals(0)) return null;

            try
            {
                List<BE.AccountInfo> info = null;
                Data.Sequence seqDal = new Data.Sequence();

                switch (_section.ToUpper())
                {
                    case "APPROVE":                         // 다음결재자 (@Sequence + Myturn='Y')
                        // 승인(+상신)
                        info = seqDal.GetAlarmApprove(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "CANCEL":                         // 다음결재자 (@Sequence + Myturn='Y')
                        // 승인(+상신)
                        info = seqDal.GetAlarmCancel(_companyID, _documentID, _groupID, _sequence);
                        break;

                }

                return info;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[GetAlarmTarget] : ", ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_state"></param>
        private void SendAlarm(BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _state)
        {
            try
            {
                if (_dpInfo == null || _sendUserInfo == null || _receiveUserlist == null) return;
                if (_receiveUserlist.Count.Equals(0)) return;

                // 코드관리로 처리
                DataView dvApp = XmlDataBaseHelper.GetSysCode(_dpInfo.CompanyID, "AlarmApproval");
                if (dvApp.Count.Equals(0)) return;

                string xmlpath = FileHelper.GetServerMapPath("/DeskPlusMobile/XmlData/Confirm/" + _sendUserInfo.PreLang + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlpath);
                XmlNode rootNode = contXml.DocumentElement;
                XmlNode subNode = null;

                foreach (DataRowView dr in dvApp)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            subNode = rootNode.SelectSingleNode("/ALARM/MAIL");
                            SendMail(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, method);
                            break;

                        case "MESSENGER":
                            subNode = rootNode.SelectSingleNode("/ALARM/MESSENGER");
                            SendMessenger(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, method);
                            break;

                        case "SMS":
                            subNode = rootNode.SelectSingleNode("/ALARM/SMS");
                            SendSMS(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, method);
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                SendMobile(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, "MOBILE");

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.DocumentProcess[SendAlarm] DocID (" + _dpInfo.DocumentID.ToString() + ") : ", ex);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메일
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMail(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMail(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;

            StringBuilder sbCC = new StringBuilder();
            int cnt = 0; string sendMail = "";
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMail.Equals("1"))
                {
                    if (info.AlarmType.Equals("CARBON"))
                    {
                        if (cnt.Equals(0))
                        {
                            sendMail = info.Mail;
                        }
                        else
                        {
                            sbCC.Append(info.Mail + ";");
                        }
                        cnt++;
                    }
                    else
                    {
                        // 참조 외, 메일처리
                        arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);      // 알람내용 가져오기
                        if (arrCont == null) return;

                        // 보낸사람 이름이 받는사람 언어로 나오게
                        string sql2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
                        sql2 = string.Format(sql2, _dpInfo.CompanyID, _sendUserInfo.UserID);
                        DataSet dsSend = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql2, "dsSendUserInfo", null);

                        foreach (DataRow dr in dsSend.Tables[0].Rows)
                        {
                            //dr["DeptID"] : 인사부서
                            //UserInfo.DeptID : 결재자 부서
                            if (dr["DeptID"].ToString().ToNumber<int>() == _sendUserInfo.DeptID)
                            {
                                string NameBase = Utility.GetLangName(
                                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                                dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                                dr["NameZhTw"].ToString().ToNull(), info.PreLang);

                                _sendUserInfo.Name = NameBase.ToNull();
                                break;
                            }
                        }

                        CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, info.Mail, "", "", arrCont[0], arrCont[1]);
                    }
                }
            }

            // 참조의 경우, 묶음처리 (참조의 경우, 발송내용이 다름)
            if (!sbCC.ToString().TrimEnd(';').Equals("") || !sendMail.Equals(""))
            {
                // 알람내용 가져오기
                arrCont = GetAlarmCont("CARBON", _contNode, _dpInfo, _sendUserInfo, linkURL, _sendUserInfo.PreLang, _alarmMethod);
                if (arrCont == null) return;

                CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, sendMail, sbCC.ToString().TrimEnd(';'), "", arrCont[0], arrCont[1]);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대상URL정보가져오기(Mail)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_urlSection"></param>
        /// <returns></returns>
        private string GetLinkURLMail(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMail");
            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 알람내용(제목,내용)
        /// </summary>
        /// <param name="_alarmType"></param>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_linkURL"></param>
        /// <returns></returns>
        private string[] GetAlarmCont(string _alarmType, XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, string _linkURL, string _preLang, string _alarmMethod)
        {
            if (_preLang.Equals("")) _preLang = "ko";

            string xmlPath = FileHelper.GetServerMapPath("/DeskPlusMobile/XmlData/Confirm/" + _preLang + ".xml");
            XmlDocument contXml = new XmlDocument();
            contXml.Load(xmlPath);
            XmlNode rootNode = contXml.DocumentElement;
            XmlNode subNode = null;

            switch (_alarmMethod)
            {
                case "MAIL":
                    subNode = rootNode.SelectSingleNode("/ALARM/MAIL");
                    break;

                case "MESSENGER":
                    subNode = rootNode.SelectSingleNode("/ALARM/MESSENGER");
                    break;

                case "SMS":
                    subNode = rootNode.SelectSingleNode("/ALARM/SMS");
                    break;

                case "MOBILE":
                    subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                    break;
            }

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
            sql = string.Format(sql, _dpInfo.CompanyID, _dpInfo.UserID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsUserInfo", null);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //dr["DeptID"] : 인사부서
                //_dpInfo.DeptID : 상신자 부서
                if (dr["DeptID"].ToString().ToNumber<int>() == _dpInfo.DeptID)
                {
                    string DeptBase = Utility.GetLangName(
                                        dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                        dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                        dr["DeptFr"].ToString().ToNull(), dr["DeptZhCn"].ToString().ToNull(),
                                        dr["DeptZhTw"].ToString().ToNull(), _preLang);

                    string TitleBase = Utility.GetLangName(
                                        dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                        dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                        dr["TitleFr"].ToString().ToNull(), dr["TitleZhCn"].ToString().ToNull(),
                                        dr["TitleZhTw"].ToString().ToNull(), _preLang);

                    string NameBase = Utility.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                        dr["NameZhTw"].ToString().ToNull(), _preLang);

                    _dpInfo.DeptBase = DeptBase.ToNull();
                    _dpInfo.TitleBase = TitleBase.ToNull();
                    _dpInfo.NameBase = NameBase.ToNull();
                }
            }

            string sql2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_ACC_SelByUserID");
            sql2 = string.Format(sql2, _dpInfo.CompanyID, _sendUserInfo.UserID);
            DataSet dsSend = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql2, "dsSendUserInfo", null);

            foreach (DataRow dr in dsSend.Tables[0].Rows)
            {
                //dr["DeptID"] : 인사부서
                //UserInfo.DeptID : 결재자 부서
                if (_sendUserInfo.DeptID == 0) //상신일때
                {
                    _sendUserInfo.DeptName = _dpInfo.DeptBase.ToNull();
                    _sendUserInfo.TitleName = _dpInfo.TitleBase.ToNull();
                    _sendUserInfo.DutyName = _dpInfo.DutyBase.ToNull();
                    _sendUserInfo.Name = _dpInfo.NameBase.ToNull();
                }
                else
                {
                    if (dr["DeptID"].ToString().ToNumber<int>() == _sendUserInfo.DeptID)
                    {
                        string DeptBase = Utility.GetLangName(
                                    dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                    dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                    dr["DeptFr"].ToString().ToNull(), dr["DeptZhCn"].ToString().ToNull(),
                                    dr["DeptZhTw"].ToString().ToNull(), _preLang);

                        string TitleBase = Utility.GetLangName(
                                        dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                        dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                        dr["TitleFr"].ToString().ToNull(), dr["TitleZhCn"].ToString().ToNull(),
                                        dr["TitleZhTw"].ToString().ToNull(), _preLang);

                        string DutyBase = Utility.GetLangName(
                                        dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                                        dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                                        dr["DutyFr"].ToString().ToNull(), dr["DutyZhCn"].ToString().ToNull(),
                                        dr["DutyZhTw"].ToString().ToNull(), _preLang);

                        string NameBase = Utility.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhCn"].ToString().ToNull(),
                                        dr["NameZhTw"].ToString().ToNull(), _preLang);

                        _sendUserInfo.DeptName = DeptBase.ToNull();
                        _sendUserInfo.TitleName = TitleBase.ToNull();
                        _sendUserInfo.DutyName = DutyBase.ToNull();
                        _sendUserInfo.Name = NameBase.ToNull();
                        break;
                    }
                }
            }


            if (_alarmType.Equals("") || subNode == null || _dpInfo == null || _sendUserInfo == null) return null;

            try
            {
                string[] arrRtnval = new string[2];

                string action = subNode.SelectSingleNode("SECTION/" + _alarmType).InnerText;
                arrRtnval[0] = subNode.SelectSingleNode("SUBJ/" + _alarmType).InnerText + _dpInfo.Subject;
                arrRtnval[1] = subNode.SelectSingleNode("CONTENT").InnerText;

                arrRtnval[1] = arrRtnval[1].Replace("$ACTION$", action);                    //Message
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", arrRtnval[0]);             //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$NUMBER3$", _dpInfo.Number3);          //양식명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCDEPTNAME$", _dpInfo.DeptBase);     //상신자부서명  $ACCDEPTNAME$ / $ACCTITLENAME$ / $ACCUSERNAME$
                arrRtnval[1] = arrRtnval[1].Replace("$ACCTITLENAME$", _dpInfo.TitleBase);   //상신자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCUSERNAME$", _dpInfo.NameBase);     //상신자이름
                arrRtnval[1] = arrRtnval[1].Replace("$USERDEPTNAME$", _sendUserInfo.DeptName);      //결재자부서명
                arrRtnval[1] = arrRtnval[1].Replace("$USERTITLENAME$", _sendUserInfo.TitleName);    //결재자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$USERDUTYNAME$", _sendUserInfo.DutyName);      //결재자직책명
                arrRtnval[1] = arrRtnval[1].Replace("$USERUSERNAME$", _sendUserInfo.Name);          //결재자이름
                arrRtnval[1] = arrRtnval[1].Replace("$OPINION$", _sendUserInfo.Opinion);            //결재자의견
                arrRtnval[1] = arrRtnval[1].Replace("$LINKURL$", _linkURL);                         //링크(문서보기)

                return arrRtnval;   // [0] : Subj / [1] : Content
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmCont()", ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메일 발송
        /// </summary>
        /// <param name="_to"></param>
        /// <param name="_from"></param>
        /// <param name="_subject"></param>
        /// <param name="_contentHTML"></param>
        public void CreateMHT(string _sendMail, string _sendName, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            MailDataInfo mailInfo = null;

            mailInfo = new MailDataInfo();
            mailInfo.From = _sendName + "<" + _sendMail + ">";
            //mailInfo.To.Add(_to, _to);

            //받는 사람
            foreach (string tmp in _to.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.To.Contains(tmp)) mailInfo.To.Add(tmp, tmp);
                }
            }

            //참조
            foreach (string tmp in _cc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Cc.Contains(tmp)) mailInfo.Cc.Add(tmp, tmp);
                }
            }

            //숨은 참조
            foreach (string tmp in _bcc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Bcc.Contains(tmp)) mailInfo.Bcc.Add(tmp, tmp);
                }
            }

            mailInfo.Subject = _subject;
            mailInfo.Contents = _body;

            Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "Approval:CreateMHT:" + _to + "," + _sendMail + "," + _subject + "," + _body);

            Zeospace.DeskPlusMobile.Web.Helper.SendMail sendMail = new SendMail();
            sendMail.Send(mailInfo, false, false, "");
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 메신저
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMessenger(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMessenger.Equals("1"))
                {
                    // 결재대상(결재/수신결재)의 경우, 알람URL 재 처리
                    if (_urlSection.ToUpper().Equals("W") || _urlSection.ToUpper().Equals("S"))
                    {
                        if (info.AlarmType.ToUpper().Equals("RECEIVED"))
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR");
                        }
                        else
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                        }
                        if (linkURL.Equals("")) return;
                    }

                    if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                    {
                        linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR");
                    }

                    // 알람내용 가져오기
                    arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                    if (arrCont == null) return;

                    Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                    alarmInfo.CompanyID = _dpInfo.CompanyID;
                    alarmInfo.SenderID = _sendUserInfo.UserID;
                    alarmInfo.SenderName = _sendUserInfo.Name;
                    alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                    alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                    alarmInfo.ReceiverID = info.UserID;
                    alarmInfo.ReceiverName = "";
                    alarmInfo.ReceiverMail = "";
                    alarmInfo.ReceiverPhone = "";
                    alarmInfo.Status = "";
                    alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.Subject = arrCont[0];
                    alarmInfo.Content = arrCont[1];
                    alarmInfo.TargetUrl = linkURL;
                    alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.WriteDate = System.DateTime.Now;
                    alarmInfo.AlarmType = 1;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                    alarmInfo.KindSubject = "Approval";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                    alarm.Add(alarmInfo);
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대상URL정보가져오기(Messenger)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_urlSection"></param>
        /// <returns></returns>
        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                case "CA":   //상신취소
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                default:    //기타(문서보기)
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerView");
                    break;
            }
            #endregion

            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }


        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string targetURL = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerW");
                    break;
                default:    //기타(문서보기)
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "AlarmMessengerView");
                    break;
            }
            #endregion

            targetURL = string.Format(targetURL, baseHttp, fullDomain);

            #region 파라미터 처리

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            if (targetURL.IndexOf("?") > 0)
            {
                string parms = targetURL.Split('?')[1];
                targetURL = targetURL.Split('?')[0];

                if (!parms.Equals("") && parms.IndexOf("=") > 0)
                {
                    if (parms.IndexOf("&") > 0)
                    {
                        string[] arrparms = parms.Split('&');          //xxxx.aspx?a=aaaaaaa&b=bbbbbb
                        foreach (string s in arrparms)
                        {
                            paraDic.Add(s.Split('=')[0], s.Split('=')[0]);
                        }
                    }
                    else
                    {
                        paraDic.Add(parms.Split('=')[0], parms.Split('=')[1]);
                    }
                }
            }

            paraDic.Add("companyID", _companyID.ToString());
            paraDic.Add("documentID", _documentID.ToString());
            paraDic.Add("sequence", _sequence.ToString());
            paraDic.Add("approvalTag", _approvalTag);
            paraDic.Add("targetUserID", _targetUserID.ToString());
            paraDic.Add("targetDeptID", _targetDeptID.ToString());

            #endregion

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 대상URL정보가져오기(Mobile)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_urlSection"></param>
        /// <returns></returns>
        private string GetLinkURLMobile(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("DID", _documentID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string targetURL = "/DeskplusMobile/Approval/AlarmView.aspx?p=" + encparam;

            return targetURL;
        }


        private string GetLinkURLMobile(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return _defaultUrl;

            string BPage = "";

            #region 상황별 url정보 가져오기
            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    BPage = "APP";
                    break;
                default:    //기타(문서보기)
                    BPage = "";
                    break;
            }
            #endregion

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("DID", _documentID.ToString());
            paraDic.Add("Seq", _sequence.ToString());
            paraDic.Add("ATag", _approvalTag);
            paraDic.Add("TargetUID", _targetUserID.ToString());
            paraDic.Add("TargetDID", _targetDeptID.ToString());
            paraDic.Add("BPage", BPage.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string targetURL = "/DeskplusMobile/Approval/AlarmView.aspx?p=" + encparam;

            return targetURL;
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : SMS
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        private void SendSMS(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
        }


        /// <summary>
        /// 모바일(Push)
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        private void SendMobile(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                // 결재대상(결재/수신결재)의 경우, 알람URL 재 처리
                if (_urlSection.ToUpper().Equals("W") || _urlSection.ToUpper().Equals("S"))
                {
                    if (info.AlarmType.ToUpper().Equals("RECEIVED"))
                    {
                        //_urlSection : SR 
                        linkURL = "";
                    }
                    else
                    {
                        linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                    }
                    if (linkURL.Equals("")) return;
                }

                //중간결재자한테 반려
                if (_urlSection.ToUpper().Equals("RS"))
                {
                    linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL);
                }

                if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                {
                    //_urlSection : SR 
                    linkURL = "";
                }

                // 알람내용 가져오기
                arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                if (arrCont == null) return;

                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                alarmInfo.CompanyID = _dpInfo.CompanyID;
                alarmInfo.SenderID = _sendUserInfo.UserID;
                alarmInfo.SenderName = _sendUserInfo.Name;
                alarmInfo.SenderMail = "";  //_sendUserInfo.Mail
                alarmInfo.SenderPhone = ""; //_sendUserInfo.MobilePhone
                alarmInfo.ReceiverID = info.UserID;
                alarmInfo.ReceiverName = "";
                alarmInfo.ReceiverMail = "";
                alarmInfo.ReceiverPhone = "";
                alarmInfo.Status = "";
                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.Subject = arrCont[0];
                alarmInfo.Content = arrCont[1];
                alarmInfo.TargetUrl = "";
                alarmInfo.TargetMobileUrl = linkURL;
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = 4;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                alarmInfo.KindSubject = "Approval";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                alarm.Add(alarmInfo);
            }
        }

        #endregion
    }
}