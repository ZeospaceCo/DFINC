using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using System.Text;
using System.Data;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class DocumentProcess : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 전역변수

        public string ALARMTARGET = "";     //알람대상정보 (다음결재자,전결재자)
        private readonly Dictionary<string, string> approvalStatusMenuTypeCache = new Dictionary<string, string>();
        /*
         *  상신자만
         *  기결재자 모두 (= 상신자 + 결재자 + 담당자)
         *  참조자
         *  담당자+상신자
         *  결재유무와 상관없는 도은 발신결재라인
         *  다음결재자 리스트 ( > Sequence )
         *  다음결재자 ( = Sequence+1 )
         *  대결자
         */
        public string APPROVALSTATE = "";   //최종 결재상태 (알람발송 시, 해당 최종 결재상태에 따라, 알람대상이 달라짐)
        /*
         * 진행 (+결재대기)
         * 완료
         */
        //public int NextUserID = 0;

        #endregion 전역변수

        #region 문서생성

        #region 발신문서

        /// <summary>
        /// 발신문서 상신
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public int Add(BE.DocumentProcessInfo _dpInfo, string _sealPath, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - 결재선은 반드시, 2개이상 지정.
             *      - @GroupID (발신문서ID) 의 경우, ZP/PS등의 경우는 따로 생성되는게 맞을 듯.
             *
             * 1. 결재라인 처리 (발신결재.수신결재.참조)
             * 2. 근거문서 처리
             * 3. 첨부파일 처리
             * 4. 상신자 승인처리 (=인장/내용처리 포함) - Decide()
                                                            * --> 최종결재여부(결재선은 반드시, 2개이상이므로, 해당 로직은 제외)
             * 5. 알람발송
             */

            #endregion 로직

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

                if (_isLast == "Y" && _isNextZQ == "N")
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("COMPLETE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "C");
                }
                else
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "W");
                }

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[Add] : " + ex);
                return -1;
            }

            #endregion 소스
        }

        /// <summary>
        /// 발신문서 수정후재상신 (ZD)
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_groupID">The _group identifier.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public int EditAdd(BE.DocumentProcessInfo _dpInfo, string _sealPath, int _groupID, UserInfo _sendInfo, string _isLast, string _isNextZQ, bool _isDel)
        {
            #region 로직

            /* 주의 - @GroupID
             *
             * 1. 신규문서생성
             * 2. 경유포함 관련문서 삭제(기존문서삭제(DB/파일)
             *
             */

            #endregion 로직

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
                        DAL.ShortReply srDal = new DAL.ShortReply();
                        //srDal.Mod(_dpInfo.CompanyID, _groupID, docID);
                        //srDal.Copy(_dpInfo.CompanyID, _groupID, docID);

                        if (_isDel)
                        {
                            // 문서삭제(경유포함)
                            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                            List<BE.DocumentProcessInfo> list = dpDal.GetDocuments(_dpInfo.CompanyID, _groupID);

                            DelDocProcess(list, true);
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                #region 알람발송

                if (_isLast == "Y" && _isNextZQ == "N")
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("COMPLETE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "C");
                }
                else
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "W");
                }

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[EditAdd] : " + ex);
                return -1;
            }

            #endregion 소스
        }

        #endregion 발신문서

        #region 수신문서

        /// <summary>
        /// 이관문서 상신
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_pInfo">The _p information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public bool AddPS(BE.DocumentProcessInfo _dpInfo, BE.PassedDocumentInfo _pInfo, string _sealPath, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - @GroupID
             *
             *  1. 신규문서 Insert - Add(BE.DocumentProcessInfo _dpInfo) : 모든 문서의 상태값 처리.('I')
             *  2. 결재상태(이관 관련) 처리
             *      - 수신관련테이블 (DP_APP_PassedDoc update)
             *              - 결재상태/상신일/ .....
             *              - 발신문서의 DocumentID 와 ApprovalTag(AccountTag)를 기준으로 ApprovedDocumentID 업데이트.
             *  3. 수신문서의 상신자는 결재문서함 Insert (ZQ)
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _pInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return false;

            try
            {
                int docID;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 신규문서 Insert (결재상태는 'I')
                    docID = AddUnit(_dpInfo, _sealPath, _sendInfo, _isLast, _isNextZQ);

                    if (docID > 0)
                    {
                        // 2. 결재상태(이관 관련) 처리
                        DAL.PassedDocument pDal = new DAL.PassedDocument();
                        if (!pDal.ModDecide(_pInfo))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "W");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[AddPS] : " + ex);
                return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 수신문서 상신
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_cpInfo">The _CP information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public bool AddZQ(BE.DocumentProcessInfo _dpInfo, BE.CounterPartUserInfo _cpInfo, string _sealPath, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - @GroupID
             *
             *  1. 신규문서 Insert - Add(BE.DocumentProcessInfo _dpInfo) : 모든 문서의 상태값 처리.('I')
             *  2. 결재상태(수신 관련) 처리
             *      - 수신관련테이블 (counterpartusers update)
             *              - 결재상태/상신일/ApprovedDocumentID .....
             *              - 발신문서의 DocumentID 와 ApprovalTag(AccountTag)를 기준으로 ApprovedDocumentID 업데이트.
             *  3. 수신문서의 상신자는 결재문서함 Insert (ZQ)
             *  4. PDF파일생성
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _cpInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return false;

            try
            {
                int docID;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 신규문서 Insert (결재상태는 'I')
                    docID = AddUnit(_dpInfo, _sealPath, _sendInfo, _isLast, _isNextZQ);

                    if (docID > 0)
                    {
                        _cpInfo.ApprovedDocumentID = docID;

                        // 2. 결재상태(수신 관련) 처리
                        DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                        if (!cpDal.ModDecide(_cpInfo))
                        {
                            return false;
                        }

                        // 3. 수신진행처리 (다음 수신자)
                        if (_isLast.Equals("Y") && _isNextZQ.Equals("Y"))
                        {
                            // 수신프로세스처리 - 수신정보 업데이트 (counterpartusers)
                            string ZQInfo = cpDal.ModNextZQ(_dpInfo.CompanyID, _dpInfo.GroupID);    //다음수신자정보 리턴 : ApprovalTag-UserID-DeptCodeID
                            if (ZQInfo.Equals(""))
                            {
                                return false;
                            }
                        }

                        if (_cpInfo.ApprovalTag.IndexOf("ZQ").Equals(0))
                        {
                            // 3. 수신문서의 상신자는 결재문서함 Insert (ZQ)
                            DAL.ApprovalDocument appDal = new DAL.ApprovalDocument();
                            BE.ApprovalDocumentInfo aInfo = new ApprovalDocumentInfo();
                            aInfo.CompanyID = _cpInfo.CompanyID;
                            aInfo.UserID = _dpInfo.UserID;
                            aInfo.DocumentID = docID;
                            aInfo.FolderID = 0;
                            aInfo.Proxy = _cpInfo.Proxy;
                            aInfo.ApprovalTag = _cpInfo.ApprovalTag;
                            aInfo.Sequence = 1;
                            aInfo.ApprovalType = "S";
                            if (!appDal.Add(aInfo))
                            {
                                return false;
                            }

                            //대결자 처리
                            if (_cpInfo.Proxy == "Y")
                            {
                                aInfo.CompanyID = _cpInfo.CompanyID;
                                aInfo.UserID = _cpInfo.ProxyUserID;
                                aInfo.DocumentID = docID;
                                aInfo.FolderID = 0;
                                aInfo.Proxy = _cpInfo.Proxy;
                                aInfo.ApprovalTag = _cpInfo.ApprovalTag;
                                aInfo.Sequence = 1;
                                aInfo.ApprovalType = "S";
                                if (!appDal.Add(aInfo))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }

                #region 알람발송

                if (_isLast.Equals("Y") && _isNextZQ.Equals("N"))
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("COMPLETE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "C");
                }
                else
                {
                    List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "W");
                }



                #endregion 알람발송

                #region PDF생성

                BLL.CreatePDF bll = new BLL.CreatePDF();
                bll.ProcessPDF(_dpInfo.CompanyID, _dpInfo.UserID, docID, _dpInfo.Number2, "1");

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[AddZQ] : " + ex);
                return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 수신문서 수정후재상신 (ZQ) (_groupID 이전과 동일)
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_cpInfo">The _CP information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_beforeDocumentID">The _before document identifier.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public int EditZQ(BE.DocumentProcessInfo _dpInfo, BE.CounterPartUserInfo _cpInfo, string _sealPath, int _beforeDocumentID, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - @GroupID
             *
             * 1. 신규문서생성
             * 2. 수신처리(counterpartusers - approvedDocumentID update) - AddZQ참고
             * 3. 수정후 재상신 예외처리
             *      - 이전의 문서의 상태값 'I' / 내용처리
             *      - 이후의 문서 삭제(기존문서삭제(DB/파일)
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _sealPath.Equals("") || _beforeDocumentID.Equals(0) || _isLast.Equals("") || _isNextZQ.Equals("")) return -1;

            try
            {
                int docID;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 0. 수정후 재상신 예외처리 대상
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> list = dpDal.GetDocuments(_dpInfo.CompanyID, _dpInfo.GroupID);

                    // 이후의 문서 삭제(기존문서삭제(DB)
                    List<BE.DocumentProcessInfo> listA = list.Where(x => x.DocumentID >= _beforeDocumentID).ToList<BE.DocumentProcessInfo>();
                    DelDocProcessZQ(listA, true);

                    // 1. 신규문서생성
                    docID = AddUnit(_dpInfo, _sealPath, _sendInfo, _isLast, _isNextZQ);
                    if (docID > 0)
                    {
                        // 업무톡 변경
                        DAL.ShortReply srDal = new DAL.ShortReply();
                        srDal.Mod(_dpInfo.CompanyID, _beforeDocumentID, docID);

                        // 2. 수신처리 - AddZQ참고
                        _cpInfo.ApprovedDocumentID = docID;
                        DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                        if (!cpDal.ModDecide(_cpInfo))
                        {
                            return -1;
                        }

                        // 3-1. 이전의 문서의 상태값 'I' / 내용처리
                        //var listBefore = list.Where(x => x.DocumentID < _beforeDocumentID);
                        //List<BE.DocumentProcessInfo> listB = list.Where(x => x.DocumentID < _beforeDocumentID).ToList<BE.DocumentProcessInfo>();

                        _dpInfo.ApprovalState = (_isLast.Equals("Y") && _isNextZQ.Equals("Y")) ? "P" : "I";
                        if (!dpDal.ModApprovalState(_dpInfo))
                        {
                            return -1;
                        }

                        // 기존문서의 첨부파일 삭제
                        string targetFolder = "";
                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/
                        foreach (BE.DocumentProcessInfo info in listA)
                        {
                            //트랜잭션에 묶이지 않게 처리함 ByFreeman 20140327
                            using (TransactionScope tsNon = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                targetFolder = baseFileDirectory + info.CompanyID.ToString() + "/Approval/Document/" + info.Number2 + "/" + info.DocumentID.ToString();

                                //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                            }
                        }

                        if (_cpInfo.ApprovalTag.IndexOf("ZQ").Equals(0))
                        {
                            // 3. 수신문서의 상신자는 결재문서함 Insert (ZQ)
                            DAL.ApprovalDocument appDal = new DAL.ApprovalDocument();
                            BE.ApprovalDocumentInfo aInfo = new ApprovalDocumentInfo();
                            aInfo.CompanyID = _cpInfo.CompanyID;
                            aInfo.UserID = _dpInfo.UserID;
                            aInfo.DocumentID = docID;
                            aInfo.FolderID = 0;
                            aInfo.Proxy = _cpInfo.Proxy;
                            aInfo.ApprovalTag = _cpInfo.ApprovalTag;
                            aInfo.Sequence = 1;
                            aInfo.ApprovalType = "S";
                            if (!appDal.Add(aInfo))
                            {
                                return -1;
                            }
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "W");

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[EditZQ] : " + ex);
                return -1;
            }

            #endregion 소스
        }

        /// <summary>
        /// 수신문서 수정후재상신 (ZP)
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_beforeInfo">The _before information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public int EditZP(BE.DocumentProcessInfo _dpInfo, BE.DocumentProcessInfo _beforeInfo, string _sealPath, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - @GroupID
             *
             * 1. 신규문서생성
             * 2. 해당 문서 삭제
             *
             */

            #endregion 로직

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
                        DAL.ShortReply srDal = new DAL.ShortReply();
                        srDal.Mod(_dpInfo.CompanyID, _beforeInfo.DocumentID, docID);

                        // 해당문서삭제
                        List<BE.DocumentProcessInfo> list = new List<BE.DocumentProcessInfo>();
                        list.Add(_beforeInfo);

                        DelDocProcess(list, true);
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 1, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "W");

                #endregion 알람발송

                return docID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[EditZP] : " + ex);
                return -1;
            }

            #endregion 소스
        }

        #endregion 수신문서

        #endregion 문서생성

        #region 본문/결재선 수정

        #region 발신문서

        /// <summary>
        /// 본문결재선 수정
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_hisInfo">The _his information.</param>
        /// <returns></returns>
        public bool Mod(BE.DocumentProcessInfo _dpInfo, BE.HistoryInfo _hisInfo)
        {
            #region 로직

            /*
             *  0. 로그처리(History) : DocProcess => History
             *
             *  1. 문서정보 (DocProcess) 부분적 필드수정
             *
             *  - 삭제후 등록
             *
             *  2. 삭제 (미결재자 & 미결재수신 & 참조자 & 근거문서 & 첨부파일)
             *
             *  3. 추가 (결재자 & 수신 & 참조 & 근거문서 & 첨부파일)
             *
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.History hDal = new DAL.History();
                    if (hDal.Add(_hisInfo) > 0)
                    {
                        //1. 문서정보 (DocProcess) 부분적 필드수정
                        DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                        if (!dpDal.ModEtcs(_dpInfo))
                        {
                            return false;
                        }

                        #region 삭제&추가 (미결재자 & 미결재수신 & 참조자)

                        //2. 삭제 (미결재자 & 미결재수신 & 참조자)
                        DAL.Sequence seqDal = new DAL.Sequence();
                        if (!seqDal.DelSeqEtcs(_dpInfo.CompanyID, _dpInfo.DocumentID))
                        {
                            return false;
                        }

                        //3. 추가 (결재자 & 수신 & 참조자)

                        #region DP_APP_Seq (결재라인)

                        if (_dpInfo.SequenceList != null)
                        {
                            DAL.Sequence sDal = new DAL.Sequence();
                            foreach (BE.SequenceInfo info in _dpInfo.SequenceList)
                            {
                                sDal.Add(info);
                            }
                        }

                        #endregion DP_APP_Seq (결재라인)

                        #region DP_APP_CounterPartUser (수신)

                        if (_dpInfo.CounterPartUserList != null)
                        {
                            DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                            foreach (BE.CounterPartUserInfo info in _dpInfo.CounterPartUserList)
                            {
                                cpDal.Add(info);
                            }
                        }

                        #endregion DP_APP_CounterPartUser (수신)

                        #region DP_APP_CarbonCopyUser (참조)

                        if (_dpInfo.CarbonCopyUserList != null)
                        {
                            DAL.CarbonCopyUser ccDal = new DAL.CarbonCopyUser();
                            foreach (BE.CarbonCopyUserInfo info in _dpInfo.CarbonCopyUserList)
                            {
                                ccDal.Add(info);
                            }
                        }

                        #endregion DP_APP_CarbonCopyUser (참조)

                        #endregion 삭제&추가 (미결재자 & 미결재수신 & 참조자)

                        #region DP_APP_BaseDoc (근거문서-신규)

                        if (_dpInfo.BaseDocumentList != null)
                        {
                            DAL.BaseDocument bDal = new DAL.BaseDocument();
                            foreach (BE.BaseDocumentInfo info in _dpInfo.BaseDocumentList)
                            {
                                bDal.Add(info);
                            }
                        }

                        #endregion DP_APP_BaseDoc (근거문서-신규)

                        #region DP_APP_Attach (첨부파일-신규)

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
                                    attachDal.Add(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.Number2, _dpInfo.HiddenSpoolPath);
                                }
                            }
                        }

                        #endregion DP_APP_Attach (첨부파일-신규)
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[Mod] : " + ex);
                return false;
            }

            #endregion 소스
        }

        #endregion 발신문서

        #region 수신문서

        /// <summary>
        /// 본문결재선 수정
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_hisInfo">The _his information.</param>
        /// <returns></returns>
        public bool ModZQ(BE.DocumentProcessInfo _dpInfo, BE.HistoryInfo _hisInfo)
        {
            #region 로직

            /*
             *  0. 로그처리(History) : DocProcess => History
             *
             *  1. 문서정보 (DocProcess) 부분적 필드수정
             *
             *  - 삭제후 등록
             *
             *  2. 삭제 (미결재자 & 미결재수신 & 참조자 & 근거문서 & 첨부파일)
             *
             *  3. 추가 (결재자 & 수신 & 참조 & 근거문서 & 첨부파일)
             *
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.History hDal = new DAL.History();
                    if (hDal.Add(_hisInfo) > 0)
                    {
                        //1. 문서정보 (DocProcess) 부분적 필드수정
                        DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                        if (!dpDal.ModEtcs(_dpInfo))
                        {
                            return false;
                        }

                        #region 삭제&추가 (미결재자 & 미결재수신 & 참조자)

                        //2. 삭제 (미결재수신 & 참조자)
                        DAL.Sequence seqDal = new DAL.Sequence();
                        if (!seqDal.DelSeqEtcs(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID))
                        {
                            return false;
                        }

                        //3. 추가 (수신 & 참조자)

                        #region DP_APP_CounterPartUser (수신)

                        if (_dpInfo.CounterPartUserList != null)
                        {
                            DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                            foreach (BE.CounterPartUserInfo info in _dpInfo.CounterPartUserList)
                            {
                                cpDal.Add(info);
                            }
                        }

                        #endregion DP_APP_CounterPartUser (수신)

                        #region DP_APP_CarbonCopyUser (참조)

                        if (_dpInfo.CarbonCopyUserList != null)
                        {
                            DAL.CarbonCopyUser ccDal = new DAL.CarbonCopyUser();
                            foreach (BE.CarbonCopyUserInfo info in _dpInfo.CarbonCopyUserList)
                            {
                                ccDal.Add(info);
                            }
                        }

                        #endregion DP_APP_CarbonCopyUser (참조)

                        #endregion 삭제&추가 (미결재자 & 미결재수신 & 참조자)

                        #region DP_APP_BaseDoc (근거문서-신규)

                        if (_dpInfo.BaseDocumentList != null)
                        {
                            DAL.BaseDocument bDal = new DAL.BaseDocument();
                            foreach (BE.BaseDocumentInfo info in _dpInfo.BaseDocumentList)
                            {
                                bDal.Add(info);
                            }
                        }

                        #endregion DP_APP_BaseDoc (근거문서-신규)

                        #region DP_APP_Attach (첨부파일-신규)

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
                                    attachDal.Add(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.Number2, _dpInfo.HiddenSpoolPath);
                                }
                            }
                        }

                        #endregion DP_APP_Attach (첨부파일-신규)
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ModZQ] : " + ex);
                return false;
            }

            #endregion 소스
        }

        #endregion 수신문서

        #endregion 본문/결재선 수정

        #region 문서결재

        /// <summary>
        /// 승인
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_seqInfo">The _seq information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_allTargetInfo">The _all target information.</param>
        /// <param name="_addList">The _add list.</param>
        /// <returns></returns>
        public bool ApproveS(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _sealPath, string _isLast, string _isNextZQ, UserInfo _sendInfo, string _allTargetInfo, List<BE.UserInfo> _addList)
        {
            #region 로직

            // _입력파라미터(대기 수신:ZQ이 있는지 구분정보), 최종결재순서인지 확인
            //                                              - 최종결재자 이면서, 수신이 있는경우에는 doc테이블의 stat를 'P' 로 할것.

            /* 1. 승인로직 호출 (결재상태 주의) - Decide()
             *
             * 2. 최종결재인 경우, 최종결재 호출 - Conclude (수신상태처리였는지, 발신완결이었는지, 수신완결이었는지 know - for.알람)
             * rtnval = (_dpInfo.AccountTag.IndexOf("ZQ") < 0) ? ConcludeZD() : ConcludeZQ();
             *
             * 3. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sealPath.Equals("") || _isLast == null || _isNextZQ == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (_isLast.Equals("Y") && _isNextZQ.Equals("Y"))
                    {
                        _dpInfo.ApprovalState = "P";
                    }

                    // 1. 승인
                    rtnval = Decide(_dpInfo, _seqInfo, _sealPath);
                    if (!rtnval) { return false; }

                    // 2. 최종결재여부(수신여부)
                    if (_isLast.Equals("Y"))
                    {
                        rtnval = Conclude(_dpInfo, _isNextZQ);
                        if (!rtnval) { return false; }
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = null;
                if (_isLast.Equals("Y") && _isNextZQ.Equals("N"))
                {

                    // 최종승인알람
                    targetList = GetAlarmTarget("COMPLETE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);

                    //추가 알람대상자
                    if (_addList != null)
                    {
                        foreach (BE.UserInfo list in _addList)
                        {
                            BE.UserInfo info = new UserInfo();

                            info.AlarmType = "COMPLETE";
                            info.CompanyID = list.CompanyID;
                            info.UserID = list.UserID;
                            info.Name = list.Name;
                            info.Mail = list.Mail;
                            info.MobilePhone = list.MobilePhone;
                            info.PreferredLanguage = list.PreferredLanguage;
                            info.PreLang = list.PreferredLanguage;
                            info.AlarmMessenger = list.AlarmMessenger;
                            info.AlarmMail = list.AlarmMail;
                            info.AlarmSMS = list.AlarmSMS;

                            targetList.Add(info);
                        }
                    }

                    SendAlarm(_dpInfo, _sendInfo, targetList, "C", _addList);
                }
                else
                {
                    // 결재의견 알람
                    if (!_sendInfo.Opinion.Equals(""))
                    {
                        if (_allTargetInfo == "1")
                        {
                            //알람 모두에게
                            targetList = GetAlarmTarget("OPINIONALL", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                        }
                        else
                        {
                            targetList = GetAlarmTarget("OPINION", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                        }

                        //추가 알람대상자
                        if (_addList != null)
                        {
                            foreach (BE.UserInfo list in _addList)
                            {
                                BE.UserInfo info = new UserInfo();

                                info.AlarmType = "OPINION";
                                info.CompanyID = list.CompanyID;
                                info.UserID = list.UserID;
                                info.Name = list.Name;
                                info.Mail = list.Mail;
                                info.MobilePhone = list.MobilePhone;
                                info.PreferredLanguage = list.PreferredLanguage;
                                info.PreLang = list.PreferredLanguage;
                                info.AlarmMessenger = list.AlarmMessenger;
                                info.AlarmMail = list.AlarmMail;
                                info.AlarmSMS = list.AlarmSMS;

                                targetList.Add(info);
                            }
                        }

                        SendAlarm(_dpInfo, _sendInfo, targetList, "O", _addList);
                    }

                    // 다음 결재 알람
                    if (_allTargetInfo == "1")
                    {
                        //알람 모두에게
                        targetList = GetAlarmTarget("APPROVEALL", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                    }
                    else
                    {
                        targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                    }

                    //추가 알람대상자
                    if (_addList != null)
                    {
                        foreach (BE.UserInfo list in _addList)
                        {
                            BE.UserInfo info = new UserInfo();

                            info.AlarmType = "APPROVE";
                            info.CompanyID = list.CompanyID;
                            info.UserID = list.UserID;
                            info.Name = list.Name;
                            info.Mail = list.Mail;
                            info.MobilePhone = list.MobilePhone;
                            info.PreferredLanguage = list.PreferredLanguage;
                            info.PreLang = list.PreferredLanguage;
                            info.AlarmMessenger = list.AlarmMessenger;
                            info.AlarmMail = list.AlarmMail;
                            info.AlarmSMS = list.AlarmSMS;

                            targetList.Add(info);
                        }
                    }

                    SendAlarm(_dpInfo, _sendInfo, targetList, "S", _addList);
                }

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveS] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //전결
        public bool ApproveA(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _sealPath, string _isNextZQ, UserInfo _sendInfo, List<BE.UserInfo> _addList)
        {
            #region 로직

            /* 1. 승인로직 호출(결재상태 주의) - Decide()
             *                          - 포함됨 : 다음결재자 없음처리 (2,3번 묶어도 될듯) (승인로직에서 다음결재자 업데이트 분기로직이 들어가면, 해당 부분 필요없음)
             *
             * 3. 전결문서함 insert (구분값을 통한 다음 결재자없음 처리 포함)
             *
             * 4. 최종결재 호출 - Conclude (수신상태처리였는지, 발신완결이었는지, 수신완결이었는지 know - for.알람)
             *
             * 5. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sealPath.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (_isNextZQ.Equals("Y"))
                    {
                        _dpInfo.ApprovalState = "P";
                    }

                    // 1. 승인
                    rtnval = Decide(_dpInfo, _seqInfo, _sealPath);
                    if (!rtnval) { return false; }

                    // 2. 전결문서함 처리
                    DAL.ArbitraryDocument adDal = new DAL.ArbitraryDocument();
                    rtnval = adDal.Add(_dpInfo.CompanyID, _dpInfo.DocumentID, _seqInfo.Sequence);
                    if (!rtnval) { return false; }

                    // 3. 최종결재(수신여부)
                    rtnval = Conclude(_dpInfo, _isNextZQ);
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("COMPLETE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);

                //추가 알람대상자
                if (_addList != null)
                {
                    foreach (BE.UserInfo list in _addList)
                    {
                        BE.UserInfo info = new UserInfo();

                        info.AlarmType = "COMPLETE";
                        info.CompanyID = list.CompanyID;
                        info.UserID = list.UserID;
                        info.Name = list.Name;
                        info.Mail = list.Mail;
                        info.MobilePhone = list.MobilePhone;
                        info.PreferredLanguage = list.PreferredLanguage;
                        info.PreLang = list.PreferredLanguage;
                        info.AlarmMessenger = list.AlarmMessenger;
                        info.AlarmMail = list.AlarmMail;
                        info.AlarmSMS = list.AlarmSMS;

                        targetList.Add(info);
                    }
                }

                SendAlarm(_dpInfo, _sendInfo, targetList, "C", _addList);

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveA] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //반려
        public bool ApproveR(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _sealPath, string _targetInfo, UserInfo _sendInfo, string _allTargetInfo, List<BE.UserInfo> _addList)
        {
            #region 로직

            /* 1. 승인로직 호출(결재상태 주의) - Decide() - 원본관련문서(결재상태주의)
             *
             * 2. 반려대상자 (결재순서) 처리 - sequence (반려대상자(approvaltag)의 MyTurn='Y')
             *
             * 3. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sealPath.Equals("") || _targetInfo.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    rtnval = Decide(_dpInfo, _seqInfo, _sealPath);
                    if (!rtnval) { return false; }

                    // 반려대상자 처리
                    DAL.Sequence seqDal = new DAL.Sequence();
                    string[] tInfo = _targetInfo.Split('-');   //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID-[4]ApprovalType
                    rtnval = seqDal.ModMyTurnState(_seqInfo.CompanyID, tInfo[0].ToNumber<int>(), tInfo[1].ToNumber<int>(), tInfo[2], "Y", tInfo[4]);
                    if (!rtnval) { return false; }

                    // 반려완결처리 (발신 상신자에게 반려된 경우, 완결처리)
                    // 반려완결처리 : 발신상신자(=최초상신자)에게 반려된 경우, 반려완결처리
                    if (_targetInfo.IndexOf("-ZD0-") > 0 && _dpInfo.GroupID.Equals(tInfo[0].ToNumber<int>()))     //tInfo[2].Equals("ZD0")
                    {
                        DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                        rtnval = dpDal.ModConcludeR(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _dpInfo.AccountTag, _dpInfo.ContentHTML, _dpInfo.ContentTEXT);
                        if (!rtnval) { return false; }
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = new List<BE.UserInfo>();

                if (_allTargetInfo == "1")
                {
                    //알람 모두에게
                    targetList = GetAlarmTarget("REJECTALL", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                }
                else
                {
                    targetList = GetAlarmTarget("REJECT", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                }

                //추가 알람대상자
                if (_addList != null)
                {
                    foreach (BE.UserInfo list in _addList)
                    {
                        BE.UserInfo info = new UserInfo();

                        info.AlarmType = "REJECTALL";
                        info.CompanyID = list.CompanyID;
                        info.UserID = list.UserID;
                        info.Name = list.Name;
                        info.Mail = list.Mail;
                        info.MobilePhone = list.MobilePhone;
                        info.PreferredLanguage = list.PreferredLanguage;
                        info.PreLang = list.PreferredLanguage;
                        info.AlarmMessenger = list.AlarmMessenger;
                        info.AlarmMail = list.AlarmMail;
                        info.AlarmSMS = list.AlarmSMS;

                        targetList.Add(info);
                    }
                }

                if (_targetInfo.IndexOf("ZD0") > 0)
                {
                    SendAlarm(_dpInfo, _sendInfo, targetList, "RW", _addList);    //상신자 반려
                }
                else
                {
                    SendAlarm(_dpInfo, _sendInfo, targetList, "RS", _addList);    //중간결재자 반려
                }

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveR] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //수발자반려
        public bool ApproveRReceive(BE.DocumentProcessInfo _dpInfo, BE.CounterPartUserInfo _cpInfo, string _targetInfo, UserInfo _sendInfo, string _allTargetInfo, List<BE.UserInfo> _addList)
        {
            #region 로직

            /* 1. 승인로직 호출(결재상태 주의) - Decide() - 원본관련문서(결재상태주의)
             *      - counterpartusers 결재상태 주의
             *
             * 2. 반려대상자 (결재순서) 처리 - sequence/counterpartusers
             *
             * 3. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _cpInfo == null || _targetInfo.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 수신처리
                    DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                    if (!cpDal.ModDecide(_cpInfo)) { return false; }

                    // 반려대상자 처리
                    DAL.Sequence seqDal = new DAL.Sequence();
                    string[] tInfo = _targetInfo.Split('-');   //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID
                    rtnval = seqDal.ModMyTurnState(_cpInfo.CompanyID, tInfo[0].ToNumber<int>(), tInfo[1].ToNumber<int>(), tInfo[2], "Y", "S");
                    if (!rtnval) { return false; }

                    // 반려완결처리 : 발신상신자(=최초상신자)에게 반려된 경우, 반려완결처리
                    if (tInfo[0].ToNumber<int>().Equals(_dpInfo.GroupID))
                    {
                        // 수신결재 처리 (반려)
                        BE.ApprovalDocumentInfo info = new ApprovalDocumentInfo();
                        info.CompanyID = _cpInfo.CompanyID;
                        info.UserID = _cpInfo.UserID;
                        info.DocumentID = _cpInfo.GroupID;
                        info.FolderID = 0;
                        info.Proxy = _cpInfo.Proxy;
                        info.ApprovalTag = _cpInfo.ApprovalTag;
                        info.Sequence = _cpInfo.Sequence;
                        info.ApprovalType = _cpInfo.ApprovalType;

                        DAL.ApprovalDocument appdal = new DAL.ApprovalDocument();
                        rtnval = appdal.Add(info);
                        if (!rtnval) { return false; }

                        // 반려완결처리
                        DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                        rtnval = dpDal.ModConcludeR(_dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _dpInfo.AccountTag, _dpInfo.ContentHTML, _dpInfo.ContentTEXT);
                        if (!rtnval) { return false; }
                    }
                    else
                    {
                        // 문서상태변경 (반려)
                        DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                        if (!dpDal.ModApprovalState(_dpInfo)) { return false; }
                    }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = new List<BE.UserInfo>();

                if (_allTargetInfo == "1")
                {
                    //알람 모두에게
                    targetList = GetAlarmTarget("REJECTALL", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 0, _dpInfo.AccountTag);
                }
                else
                {
                    targetList = GetAlarmTarget("REJECT", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, 0, _dpInfo.AccountTag);
                }

                //추가 알람대상자
                if (_addList != null)
                {
                    foreach (BE.UserInfo list in _addList)
                    {
                        BE.UserInfo info = new UserInfo();

                        info.AlarmType = "REJECTALL";
                        info.CompanyID = list.CompanyID;
                        info.UserID = list.UserID;
                        info.Name = list.Name;
                        info.Mail = list.Mail;
                        info.MobilePhone = list.MobilePhone;
                        info.PreferredLanguage = list.PreferredLanguage;
                        info.PreLang = list.PreferredLanguage;
                        info.AlarmMessenger = list.AlarmMessenger;
                        info.AlarmMail = list.AlarmMail;
                        info.AlarmSMS = list.AlarmSMS;

                        targetList.Add(info);
                    }
                }

                if (_targetInfo.IndexOf("ZD0") > 0)
                {
                    SendAlarm(_dpInfo, _sendInfo, targetList, "RW", _addList);    //상신자 반려
                }
                else
                {
                    SendAlarm(_dpInfo, _sendInfo, targetList, "RS", _addList);    //중간결재자 반려
                }

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveR] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //보류
        public bool ApproveD(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. Delay()
             *      - 결재상태처리 (Html/viewing/state)
             *      - 현 결재자 보류처리(DB:MyTurn,ApprovalType) : 결재일 업데이트 안함.
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서상태 처리
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    rtnval = dpDal.ModApprovalState(_dpInfo);
                    if (!rtnval) { return false; }

                    // 결재라인 상태 처리
                    DAL.Sequence seqDal = new DAL.Sequence();
                    rtnval = seqDal.ModMyTurnState(_seqInfo.CompanyID, _seqInfo.DocumentID, _seqInfo.Sequence, _seqInfo.ApprovalTag, "Y", _seqInfo.ApprovalType);
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("APPROVED", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "D");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveD] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //보류취소
        public bool ApproveDCancel(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. Delay()
             *      - 결재상태처리 (Html/viewing/state)
             *      - 현 결재자 보류취소처리(DB:MyTurn,ApprovalType)
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서상태 처리
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    rtnval = dpDal.ModApprovalState(_dpInfo);
                    if (!rtnval) { return false; }

                    // 결재라인 상태 처리
                    DAL.Sequence seqDal = new DAL.Sequence();
                    rtnval = seqDal.ModMyTurnState(_seqInfo.CompanyID, _seqInfo.DocumentID, _seqInfo.Sequence, _seqInfo.ApprovalTag, "Y", _seqInfo.ApprovalType);
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = GetAlarmTarget("APPROVEDC", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "D");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveDCancel] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //상신보류
        public bool ApproveH(int _companyID, int _documentID, int _groupID, string _accountTag, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. Delay()
             *      - 결재상태처리 (Html/viewing/state)
             *      - 현 결재자 보류처리(DB)
             */

            #endregion 로직

            #region 소스

            if (_companyID.Equals(0) || _documentID.Equals(0) || _groupID.Equals(0) || _accountTag.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서상태 처리
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    rtnval = dpDal.ModApprovalState(_companyID, _documentID, _groupID, _accountTag, "H");
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                //List<BE.UserInfo> targetList = GetAlarmTarget("APPROVEH", _companyID, _documentID, _groupID, 1);
                //SendAlarm("APPROVEH", _dpInfo, _sendInfo, targetList);

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveH] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //상신보류취소
        public bool ApproveHCancel(int _companyID, int _documentID, int _groupID, string _accountTag, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. Delay()
             *      - 결재상태처리 (Html/viewing/state)
             *      - 현 결재자 보류처리(DB)
             */

            #endregion 로직

            #region 소스

            if (_companyID.Equals(0) || _documentID.Equals(0) || _groupID.Equals(0) || _accountTag.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서상태 처리
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    rtnval = dpDal.ModApprovalState(_companyID, _documentID, _groupID, _accountTag, "W");
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                //List<BE.UserInfo> targetList = GetAlarmTarget("APPROVEH", _companyID, _documentID, _groupID, 1);
                //SendAlarm("APPROVEHC", _dpInfo, _sendInfo, targetList);

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveHCancel] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //상신취소, 문서폐기(반려)
        public bool DeleteDoc(int _companyID, int _groupID, int _docID, UserInfo _sendInfo, string _accountTag)
        {
            if (_companyID.Equals(0) || _groupID.Equals(0) || _docID.Equals(0)) return false;

            try
            {
                DAL.DocumentProcess dpDal2 = new DAL.DocumentProcess();
                List<BE.DocumentProcessInfo> doclist = dpDal2.GetDocument(_companyID, _docID);

                List<BE.UserInfo> targetList = GetAlarmTarget("CANCEL", doclist[0].CompanyID, doclist[0].DocumentID, doclist[0].GroupID, 1, doclist[0].AccountTag);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서삭제(경유포함)
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[DeleteDoc] : " + ex);
                return false;
            }
        }

        //후열처리
        public bool ApproveReadLaterOn(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 승인로직(DB) 호출 (결재상태 주의) - Decide() + 후열테이블 insert
                    ->최종결재자가 후열일수 없음. 최종결재인 경우, 최종결재 호출 - Conclude (수신상태처리였는지, 발신완결이었는지, 수신완결이었는지 know - for.알람)
             *
             * 2. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sendInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 승인
                    rtnval = Decide(_dpInfo, _seqInfo);
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = null;
                if (!_sendInfo.Opinion.Equals(""))
                {
                    targetList = GetAlarmTarget("OPINION", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                    SendAlarm(_dpInfo, _sendInfo, targetList, "O");
                }

                targetList = GetAlarmTarget("APPROVE", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _seqInfo.Sequence, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "S");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveReadLaterOn] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //후열처리 - 문서확인
        public bool ApproveReadLaterOnConfirm(BE.DocumentInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 후열 문서확인 DB처리 (결재(문서)상태 / HTML / 후열테이블의 관련 필드 업데이트(?????)  )
             *
             * 2. 후열 리스트에서 삭제.
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _sendInfo == null) return false;
            if (_seqInfo.CompanyID.Equals(0) || _seqInfo.DocumentID.Equals(0) || _seqInfo.Sequence.Equals(0) || _seqInfo.ApprovalTag.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // Document 처리(HTML, Viewing)
                    DAL.Document docDal = new DAL.Document();
                    if (!docDal.ModContentHTML(_dpInfo))
                    {
                        return false;
                    }

                    _seqInfo.Opinion = _sendInfo.Opinion;

                    // 결재정보에서 후열 문서확인 시간을 확인할 수 있도록 완료 결재선 시간을 갱신한다.
                    DAL.SequenceBackUp seqBackUpDal = new DAL.SequenceBackUp();
                    rtnval = seqBackUpDal.ModAfterwardConfirm(_seqInfo);
                    if (!rtnval) { return false; }

                    // 후열리스트 삭제
                    DAL.SequenceLater seqDal = new DAL.SequenceLater();
                    rtnval = seqDal.Del(_seqInfo.CompanyID, _seqInfo.DocumentID, _seqInfo.Sequence, _seqInfo.ApprovalTag);
                    if (!rtnval) { return false; }

                    // 결재의견 DB처리 
                    DAL.ShortReply shDal = new DAL.ShortReply();
                    BE.ShortReplyInfo info = new ShortReplyInfo();
                    info.CompanyID = _dpInfo.CompanyID;
                    info.DocumentID = _dpInfo.DocumentID;
                    info.Section = "1";
                    info.Content = _sendInfo.Opinion;
                    info.UserID = _sendInfo.UserID;
                    info.IsOpen = "1";

                    if (!shDal.Add(info)) { return false; }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveReadLaterOnConfirm] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //결재취소
        public bool ApproveSCancel(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 후열문서 처리
             *
             * 2. 결재문서함 처리
             *
             * 3. 현 결재자 결재취소 철치
             *
             * 4. DocProcess 처리 (문서상태,문서내용)
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 문서상태 처리
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    rtnval = dpDal.ModApprovalState(_dpInfo);
                    if (!rtnval) { return false; }

                    // 결재라인 상태 처리
                    DAL.Sequence seqDal = new DAL.Sequence();
                    rtnval = seqDal.ModCancel(_seqInfo.CompanyID, _seqInfo.DocumentID, _seqInfo.Sequence, _seqInfo.ApprovalTag, _seqInfo.Opinion);
                    if (!rtnval) { return false; }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveSCancel] : " + ex);
                return false;
            }

            #endregion 소스
        }

        //결재취소 (관계있는문서-다음결재자이전)
        public bool ApproveSCancelRelated(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _nextInfo, UserInfo _sendInfo, int _sequence, string _approvalTag)
        {
            #region 로직

            /*  1. Seq 결재취소
             *  2. DocProcess 문서내용 업데이트
             *  3. 알람
             */

            #endregion 로직

            if (_dpInfo == null || _nextInfo == null || _sequence.Equals(0) || _approvalTag.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Sequence seqdal = new DAL.Sequence();
                    rtnval = seqdal.ModCancelRelated(_dpInfo.CompanyID, _dpInfo.DocumentID, _sequence, _approvalTag, _nextInfo.MyTurn, _nextInfo.ApprovalTag, _nextInfo.Sequence);
                    if (!rtnval) return false;

                    DAL.DocumentProcess docdal = new DAL.DocumentProcess();
                    rtnval = docdal.ModApprovalState(_dpInfo);
                    if (!rtnval) return false;

                    ts.Complete();
                }

                #region 알람발송

                List<BE.UserInfo> targetList = null;
                targetList = GetAlarmTarget("APPROVEC", _dpInfo.CompanyID, _dpInfo.DocumentID, _dpInfo.GroupID, _sequence, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "I");

                #endregion 알람발송

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveSCancelRelated] : " + ex);
                return false;
            }
        }

        //공람처리 - 문서확인
        public bool ApproveConfirm(BE.DocumentProcessInfo _dpprocinfo, BE.DocumentInfo _dpInfo, BE.SequenceInfo _seqInfo, UserInfo _sendInfo, string _approvalState)
        {
            #region 로직

            /* 1. 공람확인 문서확인 DB처리 (결재(문서)상태 / HTML / 후열테이블의 관련 필드 업데이트(?????)  )
             * 
             * 2. 공람 리스트에서 삭제. (결재문서함 Insert) : 진행중일때는 관계있는 문서에서 조회
             */

            #endregion

            #region 소스

            if (_sendInfo == null) return false;
            if (_seqInfo.CompanyID.Equals(0) || _seqInfo.DocumentID.Equals(0) || _seqInfo.ApprovalTag.Equals("")) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (_approvalState.Equals("C"))
                    {
                        if (_dpInfo.CompanyID.Equals(0) || _dpInfo.DocumentID.Equals(0) || _dpInfo.GroupID.Equals(0) || _dpInfo.AccountTag.Equals("")
                            || _dpInfo.ContentHTML.Equals("") || _dpInfo.ContentTEXT.Equals(""))
                        {
                            return false;
                        }

                        // Document 처리(HTML, Viewing)
                        DAL.Document docDal = new DAL.Document();
                        rtnval = docDal.ModContentHTML(_dpInfo);
                        if (!rtnval) { return false; }

                        // Seq처리 (ApprovalType='C', ApprovalDate)
                        if (rtnval)
                        {
                            DAL.SequenceBackUp seq = new DAL.SequenceBackUp();
                            rtnval = seq.ModConfirm(_seqInfo);
                        }
                        if (!rtnval) { return false; }
                    }
                    else
                    {
                        if (_dpprocinfo.CompanyID.Equals(0) || _dpprocinfo.DocumentID.Equals(0) || _dpprocinfo.GroupID.Equals(0) || _dpprocinfo.AccountTag.Equals("")
                            || _dpprocinfo.ApprovalState.Equals("") || _dpprocinfo.ContentHTML.Equals("") || _dpprocinfo.ContentTEXT.Equals(""))
                        {
                            return false;
                        }

                        DAL.DocumentProcess proc = new DAL.DocumentProcess();
                        rtnval = proc.ModApprovalState(_dpprocinfo);
                        if (!rtnval) { return false; }

                        // Seq처리 (ApprovalType='C', ApprovalDate)
                        if (rtnval)
                        {
                            DAL.Sequence seq = new DAL.Sequence();
                            rtnval = seq.ModConfirm(_seqInfo);
                        }
                        if (!rtnval) { return false; }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveConfirm] : " + ex);
                return false;
            }

            #endregion
        }

        //수신회수
        public bool CancelSusinDoc(int _companyID, int _docID, string _contentHTML)
        {
            if (_companyID.Equals(0) || _docID.Equals(0)) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> list = dpDal.GetDocument(_companyID, _docID);

                    list[0].ContentHTML = _contentHTML;

                    CancelSusinProcess(list[0], true);

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[CancelSusinDoc] : " + ex);
                return false;
            }
        }
        #endregion 문서결재

        #region 회람, 전달, 이관, 의견추가 알람

        /// <summary>
        /// 회람
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_seqInfo">The _seq information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <returns></returns>
        public bool ApproveCir(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _isLast, string _isNextZQ, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                #region 알람발송

                List<BE.UserInfo> targetList = null;

                targetList = GetAlarmTarget("CIRCULATION", _dpInfo.CompanyID, _dpInfo.CirUserList);
                //SendAlarm(_dpInfo, _sendInfo, targetList, "C");     //회람함으로 가는게 아니라, 문서보기로 처리되어 있음. (회람문서보기가 아닌, 결재문서가 계속 보여질수 있음.....음;;)
                SendAlarm(_dpInfo, _sendInfo, targetList, "CIR");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveCir] : " + ex);
                return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 전달
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_seqInfo">The _seq information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <returns></returns>
        public bool ApproveFor(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _isLast, string _isNextZQ, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                #region 알람발송

                List<BE.UserInfo> targetList = null;

                targetList = GetAlarmTarget("FORWARD", _dpInfo.CompanyID, _dpInfo.CirUserList);
                //SendAlarm(_dpInfo, _sendInfo, targetList, "C");
                SendAlarm(_dpInfo, _sendInfo, targetList, "FORWARD");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveFor] : " + ex);
                return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 이관
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_seqInfo">The _seq information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <returns></returns>
        public bool ApprovePass(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo, string _isLast, string _isNextZQ, UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                #region 알람발송

                List<BE.UserInfo> targetList = null;

                targetList = GetAlarmTarget("PASSED", _dpInfo.CompanyID, _dpInfo.CirUserList);
                SendAlarm(_dpInfo, _sendInfo, targetList, "C");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApprovePass] : " + ex);
                return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 담당자 변경
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendInfo"></param>
        /// <returns></returns>
        public bool ModCounterpartuser(BE.DocumentProcessInfo _dpInfo, UserInfo _sendInfo, BE.CounterPartUserInfo _cpInfo)
        {
            if (_dpInfo == null) return false;

            try
            {
                DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                if (cpDal.ModReplace(_cpInfo, _dpInfo.ContentHTML, _dpInfo.ContentTEXT))
                {
                    #region 알람발송
                    List<BE.UserInfo> targetList = GetAlarmTarget("RECEIVEDMOD", _dpInfo.CompanyID, _cpInfo.UserID.ToString());
                    SendAlarm(_dpInfo, _sendInfo, targetList, "SR");

                    #endregion 알람발송
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ModCounterpartuser] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 결재의견
        /// </summary>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <returns></returns>
        public bool ApproveOpinion(BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendInfo)
        {
            #region 로직

            /* 1. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                #region 알람발송

                List<BE.UserInfo> targetList = null;

                //targetList = GetAlarmTarget("OPINION", _dpInfo.CompanyID, _dpInfo.CirUserList);
                DAL.Sequence dal = new DAL.Sequence();
                targetList = dal.GetAlarmOpinionAddAll(_dpInfo.CompanyID, _dpInfo.DocumentID, _sendInfo.UserID, _dpInfo.AccountTag);
                SendAlarm(_dpInfo, _sendInfo, targetList, "C");

                #endregion 알람발송

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApproveOpinion] : " + ex);
                return false;
            }

            #endregion 소스
        }

        #endregion

        #region 단위로직

        /// <summary>
        /// 발신문서 상신
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sealPath">The _seal path.</param>
        /// <param name="_sendInfo">The _send information.</param>
        /// <param name="_isLast">The _is last.</param>
        /// <param name="_isNextZQ">The _is next zq.</param>
        /// <returns></returns>
        public int AddUnit(BE.DocumentProcessInfo _dpInfo, string _sealPath, UserInfo _sendInfo, string _isLast, string _isNextZQ)
        {
            #region 로직

            /* 주의 - 결재선은 반드시, 2개이상 지정.
             *      - @GroupID (발신문서ID) 의 경우, ZP/PS등의 경우는 따로 생성되는게 맞을 듯.
             *
             * 1. 결재라인 처리 (발신결재.수신결재.참조)
             * 2. 근거문서 처리
             * 3. 첨부파일 처리
             * 4. 상신자 승인처리 (=인장/내용처리 포함) - Decide()
                                                            * --> 최종결재여부(결재선은 반드시, 2개이상이므로, 해당 로직은 제외)
             * 5. 알람발송
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _sealPath.Equals("") || _isLast.Equals("") || _isNextZQ.Equals("")) return -1;

            try
            {
                BE.SequenceInfo ZD0Info = null;
                int docID;

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                docID = dpDal.Add(_dpInfo);

                if (docID > 0)
                {
                    _dpInfo.DocumentID = docID;
                    //_dpInfo.GroupID = docID;

                    #region DP_APP_Seq (결재라인)

                    if (_dpInfo.SequenceList != null)
                    {
                        if (_dpInfo.SequenceList.Count > 0)
                        {
                            DAL.Sequence sDal = new DAL.Sequence();
                            foreach (BE.SequenceInfo info in _dpInfo.SequenceList)
                            {
                                info.DocumentID = docID;
                                sDal.Add(info);

                                if (info.ApprovalTag.ToUpper().Equals("ZD0"))
                                {
                                    ZD0Info = info;
                                    //ZD0Info.Proxy = "N";
                                    //ZD0Info.ProxyUserID = 0;
                                    //ZD0Info.ApprovalType = "S";
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
                            DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
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
                            DAL.CarbonCopyUser ccDal = new DAL.CarbonCopyUser();
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
                            DAL.BaseDocument bDal = new DAL.BaseDocument();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[AddUnit] : " + ex);
                throw;  //for, Transaction
                //return -1;
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
            #region 로직

            // doc,seq table의 viewing주의.
            // 인장카피 및 replace(경로) & 상신자 승인처리 & 다음결재자 유무체크
            // - 원본관련문서(결재상태주의) : 진행중인 문서는 어차피 동일상태(state/html)이므로, 해당 값 동시처리.
            //                              단, ZP(수신)/PS(이관)으로 엮여진 문서에는 영향없도록 주의
            // 승인,전결,반려일때, 다음결재자 처리로직이 다름을 주의

            /*
            1. Sequence 승인처리 (대결자 결재포함,viewing) : 본인결재완료 / 다음결재자 확인 : 호출로직에 다음결재자 여부를 알려줌. (수신여부포함??)
            2. Document 내용처리 (내용,결재상태,viewing) : 인장경로/결재상태 주의
            3. ApprovalDocument 처리 (결재문서함) - S인경우만.
            3. 파일처리 (인장 copy)
            */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null || _sealPath == null) return false;

            try
            {
                #region 파일경로처리

                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");                    //  ex) http://
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/
                string serverName = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");                //  ex) localhost

                string targetFolder = baseFileDirectory + _dpInfo.CompanyID.ToString() + "/Approval/Document/" + _dpInfo.Number2 + "/" + _dpInfo.DocumentID.ToString();
                //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                string targetFile = targetFolder + "/" + _dpInfo.AccountTag + "_seal" + _seqInfo.ApprovalTag + ".gif";      //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37/ZD_sealZD0.gif

                _dpInfo.ContentHTML = _dpInfo.ContentHTML.Replace(_sealPath, baseHttp + serverName + targetFile);           //파일처리때문에 도메인정보는 여기서처리.

                #endregion 파일경로처리

                #region DB처리

                DAL.Sequence seqDal = new DAL.Sequence();

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
                DAL.DocumentProcess docDal = new DAL.DocumentProcess();
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
                FileHelper.CopyFile(_sealPath.Replace(baseHttp + serverName, ""), targetFile);      //파일처리때문에 도메인정보는 삭제.

                #endregion 실제 인장복사

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[Decide] : " + ex);
                throw;  //for, Transaction
                //return false;
            }

            #endregion 소스
        }

        /// <summary>
        /// 승인(DB) - 후열
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_seqInfo">The _seq information.</param>
        /// <returns></returns>
        private bool Decide(BE.DocumentProcessInfo _dpInfo, BE.SequenceInfo _seqInfo)
        {
            #region 로직

            /*
            1. Sequence 승인처리 (대결자 결재포함,viewing) : 본인결재완료 / 다음결재자 확인 : 호출로직에 다음결재자 여부를 알려줌. (수신여부포함??)
            2. Document 내용처리 (내용,결재상태,viewing) : 인장경로/결재상태 주의
            3. ApprovalDocument 처리 (결재문서함) - S인경우만.
            */

            #endregion 로직

            #region 소스

            if (_dpInfo == null || _seqInfo == null) return false;

            try
            {
                #region DB처리

                DAL.Sequence seqDal = new DAL.Sequence();

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
                DAL.DocumentProcess docDal = new DAL.DocumentProcess();
                if (!docDal.ModApprovalState(_dpInfo))
                {
                    return false;
                }

                #endregion DB처리

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[Decide-DB] : " + ex);
                throw;  //for, Transaction
                //return false;
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
                    DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[Conclude] : " + ex);
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
            #region 로직

            // 발신완료인경우
            /*
             * 1. 문서번호
             * 2. 결재상태 및 문서번호등의 html처리   = 2,3,4,5를 하나의 완결 프로시저에서 처리.
             * 3. 문서함 (부서별/개인별)
             * 4. Doc / SequenceBakcup Table Insert
             * 5. DocProcess / Sequence Table Delete
             * 6. 외부연동모듈 호출 (EDM)
             */

            #endregion 로직

            #region 소스

            if (_dpInfo == null) return false;

            try
            {
                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();

                #region 문서번호 생성

                string[] tmpInfo = dpDal.GetDocNumber(_dpInfo.CompanyID, _dpInfo.DocumentID).Split('^');
                //  tmpInfo = 문서번호^Number4
                if (tmpInfo == null)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ConcludeZD] : 문서번호생성 오류 (CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ConcludeZD] : 완료SP 호출오류 (CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
                    return false;
                }

                #endregion 완결호출(결재상태/문서내용처리/문서번호/완료일/문서함처리(부서별/개인별)/Doc,Seq Table이동 )

                #region 외부연동모듈 호출 EDM

                // DeskPlusEDM 의 경우 패키지 Approval BLL Trans 호출 ,
                // 타 포듈(사이버다임등)의 경우 별도 개발 (Linked Project 별도로 추가 해서 하세요.) seo
                // 사실 소스 분리 하기 떄문에 별도의 EDM 외부모듈 연동이 필요 하다 하더라도 Transfer.cs 를 수정 하는 형태의 커스터 마이징 하면 됨

                // 문서번호를 리셋 해주고 시작한다.
                _dpInfo.DocNumber = tmpInfo[0];
                Transfer tf = new Transfer();
                int edmRtnVal = tf.ApprovalToEDM(_dpInfo);
                if (edmRtnVal == -1)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ApprovalToEDM] : EDM 호출오류 (CompanyID:" + _dpInfo.CompanyID.ToString() + ",DocumentID:" + _dpInfo.DocumentID.ToString() + ")");
                    return false;
                }

                #endregion 외부연동모듈 호출 EDM

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[ConcludeZD] : " + ex);
                throw;  //for, Transaction
                //return false;
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
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                BLL.CirculationDocument cirDoc = new CirculationDocument();

                foreach (BE.DocumentProcessInfo info in _info)
                {
                    if (dpDal.Del(info.CompanyID, info.DocumentID))
                    {
                        targetFolder = baseFileDirectory + info.CompanyID.ToString() + "/Approval/Document/" + info.Number2 + "/" + info.DocumentID.ToString();

                        //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                        if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);

                        //회람 문서 삭제하기
                        cirDoc.Del(info.CompanyID, info.DocumentID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[DelDocProcess] : " + ex);
                throw;
            }
        }


        /// <summary>
        /// 문서삭제 (특정문서대상)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_number2"></param>
        /// <param name="_isFileDel"></param>
        /// <returns></returns>
        public bool DelDocProcessZQ(List<BE.DocumentProcessInfo> _info, bool _isFileDel)
        {
            if (_info.Count < 1) return false;

            try
            {
                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                BLL.CirculationDocument cirDoc = new CirculationDocument();

                foreach (BE.DocumentProcessInfo info in _info)
                {
                    if (dpDal.DelMod(info.CompanyID, info.DocumentID))
                    {
                        //회람 문서 삭제하기
                        cirDoc.Del(info.CompanyID, info.DocumentID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[DelDocProcessZQ] : ", ex);
                throw;
            }
        }

        /// <summary>
        /// 수신회수
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isFileDel">if set to <c>true</c> [_is file delete].</param>
        /// <returns></returns>
        public bool CancelSusinProcess(BE.DocumentProcessInfo _dpInfo, bool _isFileDel)
        {
            try
            {
                string targetFolder = "";
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                BLL.CirculationDocument cirDoc = new CirculationDocument();
                DAL.CounterPartUser cpDal = new DAL.CounterPartUser();

                BE.CounterPartUserInfo cpInfo = new BE.CounterPartUserInfo();

                DataTableReader dtr = cpDal.GetCounterPartUsers(_dpInfo.CompanyID, _dpInfo.GroupID, _dpInfo.AccountTag);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            cpInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            cpInfo.GroupID = dtr["DocumentID"].ToString().ToNumber<int>();
                            cpInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                            cpInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        }
                    }
                }

                if (cpDal.ModCancel(cpInfo, _dpInfo.ContentHTML))
                {
                    if (dpDal.Del(_dpInfo.CompanyID, _dpInfo.DocumentID))
                    {
                        targetFolder = baseFileDirectory + _dpInfo.CompanyID.ToString() + "/Approval/Document/" + _dpInfo.Number2 + "/" + _dpInfo.DocumentID.ToString();

                        //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                        if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);

                        //회람 문서 삭제하기
                        cirDoc.Del(_dpInfo.CompanyID, _dpInfo.DocumentID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[CancelSusinProcess] : " + ex);
                throw;
            }
        }

        #endregion 단위로직

        #region 외부연동

        // EDM처리
        private bool SendEDM()
        {
            #region 소스

            return false;

            #endregion 소스
        }

        #endregion 외부연동

        #region 알람발송 (메일/메신저/SMS)

        /// <summary>
        /// Sends the alarm.
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_state">The _state.</param>
        private void SendAlarm(BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _state)
        {
            #region 로직

            /*
             */

            #endregion 로직

            #region 소스

            try
            {
                if (_dpInfo == null || _sendUserInfo == null || _receiveUserlist == null) return;
                if (_receiveUserlist.Count.Equals(0)) return;

                // 코드관리로 처리
                DataView dvApp = AppSettingHelper.GetSysCode(_dpInfo.CompanyID, "AlarmApproval");
                if (dvApp.Count.Equals(0)) return;

                string xmlpath = FileHelper.GetServerMapPath("/DeskPlusEIP/Approval/App_Data/Confirm/" + _sendUserInfo.PreLang + ".xml");
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
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                    SendMobile(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, "MOBILE");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[SendAlarm] DocID (" + _dpInfo.DocumentID.ToString() + ") : " + ex);
            }

            #endregion 소스
        }

        /// <summary>
        /// Sends the alarm.
        /// </summary>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_state">The _state.</param>
        private void SendAlarm(BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _state, List<BE.UserInfo> _addList)
        {
            #region 로직

            /*
             */

            #endregion 로직

            #region 소스

            try
            {
                if (_dpInfo == null || _sendUserInfo == null || _receiveUserlist == null) return;
                if (_receiveUserlist.Count.Equals(0)) return;

                // 코드관리로 처리
                DataView dvApp = AppSettingHelper.GetSysCode(_dpInfo.CompanyID, "AlarmApproval");
                if (dvApp.Count.Equals(0)) return;

                string xmlpath = FileHelper.GetServerMapPath("/DeskPlusEIP/Approval/App_Data/Confirm/" + _sendUserInfo.PreLang + ".xml");
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
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");
                    SendMobile(subNode, _dpInfo, _sendUserInfo, _receiveUserlist, _state, "MOBILE");
                }

                // 결재의견 알람                    
                if (!_sendUserInfo.Opinion.Equals("") && _state != "O")
                {
                    List<BE.UserInfo> targetList = null;

                    DAL.Sequence dal = new DAL.Sequence();
                    targetList = dal.GetAlarmOpinionAddAll(_dpInfo.CompanyID, _dpInfo.DocumentID, _sendUserInfo.UserID, _dpInfo.AccountTag);

                    //추가 알람대상자
                    if (_addList != null)
                    {
                        foreach (BE.UserInfo list in _addList)
                        {
                            BE.UserInfo info = new UserInfo();

                            info.AlarmType = "OPINION";
                            info.CompanyID = list.CompanyID;
                            info.UserID = list.UserID;
                            info.Name = list.Name;
                            info.Mail = list.Mail;
                            info.MobilePhone = list.MobilePhone;
                            info.PreferredLanguage = list.PreferredLanguage;
                            info.PreLang = list.PreferredLanguage;
                            info.AlarmMessenger = list.AlarmMessenger;
                            info.AlarmMail = list.AlarmMail;
                            info.AlarmSMS = list.AlarmSMS;

                            targetList.Add(info);
                        }
                    }

                    //메신저알람 활성화일 경우 첨언 메일발송
                    foreach (DataRowView dr in dvApp)
                    {
                        string method = dr["CodeID"].ToString().ToUpper();
                        switch (method)
                        {
                            case "MESSENGER":
                                subNode = rootNode.SelectSingleNode("/ALARM/OPINION");
                                SendMailOpinion(subNode, _dpInfo, _sendUserInfo, targetList, _state, "OPINION");
                                break;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[SendAlarm] DocID (" + _dpInfo.DocumentID.ToString() + ") : " + ex);
            }

            #endregion 소스
        }

        /// <summary>
        /// 메일
        /// </summary>
        /// <param name="_contNode">The _cont node.</param>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_alarmMethod">The _alarm method.</param>
        private void SendMail(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMail(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            // 참조처리
            string[] arrCont = null;
            Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailBll = new MailZettaFactory();

            StringBuilder sbCC = new StringBuilder();
            int cnt = 0; string sendMail = "";
            foreach (UserInfo info in _receiveUserlist)
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
                        arrCont = GetAlarmCont(info.AlarmType.Replace("REJECTALL", "REJECT"), _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);     //수신자의 언어정보
                        if (arrCont == null) return;

                        mailBll.CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, info.Mail, "", "", arrCont[0], arrCont[1], null, "", false);
                    }
                }
            }

            // 참조의 경우, 묶음처리 (참조의 경우, 발송내용이 다름)
            if (!sbCC.ToString().TrimEnd(';').Equals("") || !sendMail.Equals(""))
            {
                // 알람내용 가져오기
                arrCont = GetAlarmCont("CARBON", _contNode, _dpInfo, _sendUserInfo, linkURL, _sendUserInfo.PreLang, _alarmMethod);
                if (arrCont == null) return;

                mailBll.CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, sendMail, sbCC.ToString().TrimEnd(';'), "", arrCont[0], arrCont[1], null, "", false);
            }
        }

        /// <summary>
        /// 메일
        /// </summary>
        /// <param name="_contNode">The _cont node.</param>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_alarmMethod">The _alarm method.</param>
        private void SendMailOpinion(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMail(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            // 참조처리
            string[] arrCont = null;
            Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailBll = new MailZettaFactory();

            StringBuilder sbCC = new StringBuilder();
            foreach (UserInfo info in _receiveUserlist)
            {
                arrCont = GetAlarmCont(info.AlarmType, _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);     //수신자의 언어정보
                if (arrCont == null) return;

                mailBll.CreateMHT(_sendUserInfo.Mail, _sendUserInfo.Name, info.Mail, "", "", arrCont[0], arrCont[1], null, "", false);
            }
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_contNode">The _cont node.</param>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_alarmMethod">The _alarm method.</param>
        private void SendMessenger(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Common.BLL.Alarm();
            foreach (BE.UserInfo info in _receiveUserlist)
            {
                if (info.AlarmMessenger.Equals("1"))
                {
                    linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.UserID);

                    // 결재대상(결재/수신결재)의 경우, 알람URL 재 처리
                    if (_urlSection.ToUpper().Equals("W") || _urlSection.ToUpper().Equals("S"))
                    {
                        if (info.AlarmType.ToUpper().Equals("RECEIVED"))
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR", info.UserID);
                        }
                        else
                        {
                            linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL, info.AlarmType, info.UserID);
                        }
                        if (linkURL.Equals("")) return;
                    }

                    if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                    {
                        linkURL = GetLinkURLMessenger(_dpInfo.CompanyID, _dpInfo.DocumentID, "SR", info.UserID);
                    }

                    // 알람내용 가져오기
                    arrCont = GetAlarmCont(info.AlarmType.Replace("REJECTALL", "REJECT"), _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                    if (arrCont == null) return;

                    AlarmInfo alarmInfo = new AlarmInfo();
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

                    alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                }
            }
        }

        /// <summary>
        /// SMS
        /// </summary>
        /// <param name="_contNode">The _cont node.</param>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_receiveUserlist">The _receive userlist.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_alarmMethod">The _alarm method.</param>
        private void SendSMS(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            if (_sendUserInfo.MobilePhone.Equals("") || _dpInfo.Subject.Equals("")) return;

            Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Zeospace.DeskPlus.Component.SMS.SMSInfo();
            smsInfo.CompanyID = _sendUserInfo.CompanyID;
            smsInfo.LoginID = _sendUserInfo.LogInID;
            smsInfo.UserID = _sendUserInfo.UserID;
            smsInfo.WriteUserName = _sendUserInfo.Name;
            smsInfo.InterfaceID = "";
            smsInfo.Gubun = "";
            //smsInfo.Content = _dpInfo.Subject;  //내용(문서제목)
            //smsInfo.Subject = "";
            smsInfo.Status = "0";               //상태(즉시:0,예약:1)
            smsInfo.SendPhone = _sendUserInfo.MobilePhone;
            smsInfo.TargetUrl = "";

            //##### 중요!!! 회사하나로 SMS 비용한다, 계열사 일경우 부서명 가져와야 한다
            smsInfo.CompanyName = _sendUserInfo.CompanyID.ToString();
            smsInfo.DeptID = _sendUserInfo.DeptID;
            smsInfo.DeptName = _sendUserInfo.DeptName;

            smsInfo.SendDate = System.DateTime.Now.AddMinutes(1); //즉시발송(약간 딜레이가 있어서 1분 추가하였음)
            smsInfo.SendTime = "000000";
            smsInfo.StartDate = System.DateTime.MaxValue; //미사용
            smsInfo.EndDate = System.DateTime.MaxValue;   //미사용
            smsInfo.UseSendAllUsers = false;

            // 회신번호 처리
            _sendUserInfo.MobilePhone = _sendUserInfo.MobilePhone.Trim().Replace(" ", "");
            _sendUserInfo.MobilePhone = System.Text.RegularExpressions.Regex.Replace(_sendUserInfo.MobilePhone, @"[-~.,)(]{1}", "");

            if (_sendUserInfo.MobilePhone.Length.Equals(10))
            {
                smsInfo.ReplyPhone1 = _sendUserInfo.MobilePhone.Substring(0, 3);
                smsInfo.ReplyPhone2 = _sendUserInfo.MobilePhone.Substring(3, 3);
                smsInfo.ReplyPhone3 = _sendUserInfo.MobilePhone.Substring(6, 4);
            }
            else if (_sendUserInfo.MobilePhone.Length.Equals(11))
            {
                smsInfo.ReplyPhone1 = _sendUserInfo.MobilePhone.Substring(0, 3);
                smsInfo.ReplyPhone2 = _sendUserInfo.MobilePhone.Substring(3, 4);
                smsInfo.ReplyPhone3 = _sendUserInfo.MobilePhone.Substring(7, 4);
            }
            else
            {
                if (_sendUserInfo.MobilePhone.Equals(""))
                {
                    smsInfo.ReplyPhone1 = "";
                    smsInfo.ReplyPhone2 = "";
                    smsInfo.ReplyPhone3 = "";
                }
                else
                {
                    smsInfo.ReplyPhone1 = _sendUserInfo.MobilePhone.Substring(0, 3);
                    smsInfo.ReplyPhone2 = _sendUserInfo.MobilePhone.Substring(3, 3);
                    smsInfo.ReplyPhone3 = _sendUserInfo.MobilePhone.Substring(6, _sendUserInfo.MobilePhone.Length - 6);
                }
            }

            // Add 처리
            foreach (BE.UserInfo info in _receiveUserlist)
            {
                if (info.AlarmSMS.Equals("1"))
                {
                    // 알람내용 가져오기
                    string[] arrCont = GetAlarmCont(info.AlarmType.Replace("REJECTALL", "REJECT"), _contNode, _dpInfo, _sendUserInfo, "", info.PreLang, _alarmMethod);
                    smsInfo.Subject = arrCont[0];
                    smsInfo.Content = arrCont[1];

                    info.MobilePhone = info.MobilePhone.Trim().Replace(" ", "");
                    info.MobilePhone = System.Text.RegularExpressions.Regex.Replace(info.MobilePhone, @"[-~.,)(]{1}", "");

                    smsInfo.ReceivePhone = info.MobilePhone;

                    switch (smsInfo.ReceivePhone.Length)
                    {
                        case 10:
                            smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                            smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                            smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, 4);
                            break;

                        case 11:
                            smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                            smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 4);
                            smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(7, 4);
                            break;

                        default:
                            if (smsInfo.ReceivePhone.Equals(""))
                            {
                                smsInfo.ReceivePhone1 = "";
                                smsInfo.ReceivePhone2 = "";
                                smsInfo.ReceivePhone3 = "";
                            }
                            else
                            {
                                smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                                smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, info.MobilePhone.Trim().Length - 6);
                            }
                            break;
                    }

                    Component.SMS.ControlSendSMS sendSMS = new Component.SMS.ControlSendSMS();
                    sendSMS.Transfer(smsInfo);
                }
            }
        }

        /// <summary>
        /// 모바일(Push)
        /// </summary>
        /// <param name="_contNode"></param>
        /// <param name="_dpInfo"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_urlSection"></param>
        /// <param name="_alarmMethod"></param>
        private void SendMobile(XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, List<BE.UserInfo> _receiveUserlist, string _urlSection, string _alarmMethod)
        {
            // 알람링크 가져오기
            string linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);
            if (linkURL.Equals("")) return;

            string[] arrCont = null;
            Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Common.BLL.Alarm();
            foreach (BE.UserInfo info in _receiveUserlist)
            {
                linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection);

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
                        linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL, info.AlarmType);
                    }
                    if (linkURL.Equals("")) return;
                }

                //중간결재자한테 반려
                if (_urlSection.ToUpper().Equals("RS"))
                {
                    linkURL = GetLinkURLMobile(_dpInfo.CompanyID, _dpInfo.DocumentID, _urlSection, info.Sequence, info.ApprovalTag, info.TargetUserID, info.TargetDeptID, linkURL, info.AlarmType);
                }

                if (_urlSection.ToUpper().Equals("C") && info.AlarmType.ToUpper().Equals("RECEIVED"))
                {
                    //_urlSection : SR 
                    linkURL = "";
                }

                // 알람내용 가져오기
                arrCont = GetAlarmCont(info.AlarmType.Replace("REJECTALL", "REJECT"), _contNode, _dpInfo, _sendUserInfo, linkURL, info.PreLang, _alarmMethod);
                if (arrCont == null) return;

                AlarmInfo alarmInfo = new AlarmInfo();
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

                alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
            }
        }

        #region 알람발송 단위로직

        /// <summary>
        /// 알람대상
        /// </summary>
        /// <param name="_section">The _section.</param>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_groupID">The _group identifier.</param>
        /// <param name="_sequence">The _sequence.</param>
        /// <param name="_accountTag">The _account tag.</param>
        /// <returns></returns>
        private List<BE.UserInfo> GetAlarmTarget(string _section, int _companyID, int _documentID, int _groupID, int _sequence, string _accountTag)
        {
            if (_section.Equals("") || _companyID.Equals(0) || _documentID.Equals(0)) return null;

            try
            {
                List<BE.UserInfo> info = null;
                DAL.Sequence seqDal = new DAL.Sequence();

                switch (_section.ToUpper())
                {
                    case "OPINION":                         // 이전 기결재자
                        // 결재의견 알림
                        info = seqDal.GetAlarmOpinion(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "OPINIONALL":                         // 이전 기결재자
                        // 결재의견 알림
                        info = seqDal.GetAlarmOpinionAll(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVE":                         // 다음결재자 (@Sequence + Myturn='Y')
                        // 승인(+상신)
                        info = seqDal.GetAlarmApprove(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVEALL":                         // 다음결재자 (@Sequence + Myturn='Y')
                        // 승인(+상신)
                        info = seqDal.GetAlarmApproveAll(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "REJECT":                          // 반려대상자
                        info = seqDal.GetAlarmReject(_companyID, _documentID, _groupID);
                        // 반려
                        break;

                    case "REJECTALL":                          // 반려대상자
                        info = seqDal.GetAlarmRejectAll(_companyID, _documentID, _groupID);
                        // 반려
                        break;

                    case "COMPLETE":                        // 결재라인+참조+수신(ZP)
                        // 완결
                        info = seqDal.GetAlarmComplete(_companyID, _documentID, _groupID, _accountTag, _sequence);
                        break;

                    case "CANCEL":                          // 다음결재자
                        // 상신취소
                        info = seqDal.GetAlarmCancel(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVED":                        // 이전결재자
                        // 보류
                        info = seqDal.GetAlarmApproveD(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVEDC":                       // 이전결재자
                        // 보류취소
                        info = seqDal.GetAlarmApproveDC(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVEH":                        // 다음결재자
                        // 상신보류
                        // info = seqDal.GetAlarmApprove(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVEHC":                       // 다음결재자
                        // 상신보류취소
                        // info = seqDal.GetAlarmApprove(_companyID, _documentID, _groupID, _sequence);
                        break;

                    case "APPROVEC":                       // 다음결재자
                        // 결재취소
                        info = seqDal.GetAlarmApproveC(_companyID, _documentID, _groupID, _sequence);
                        break;
                }

                return info;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[GetAlarmTarget] : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 알람내용(제목,내용)
        /// </summary>
        /// <param name="_alarmType">Type of the _alarm.</param>
        /// <param name="_contNode">The _cont node.</param>
        /// <param name="_dpInfo">The _DP information.</param>
        /// <param name="_sendUserInfo">The _send user information.</param>
        /// <param name="_linkURL">The _link URL.</param>
        /// <param name="_receiveUserID">The _receive user identifier.</param>
        /// <param name="_alarmMethod">The _alarm method.</param>
        /// <returns></returns>
        private string[] GetAlarmCont(string _alarmType, XmlNode _contNode, BE.DocumentProcessInfo _dpInfo, BE.UserInfo _sendUserInfo, string _linkURL, string _preLang, string _alarmMethod)
        {
            if (_preLang.Equals("")) _preLang = "ko";

            string xmlpath = FileHelper.GetServerMapPath("/DeskPlusEIP/Approval/App_Data/Confirm/" + _preLang + ".xml");
            XmlDocument contXml = new XmlDocument();
            contXml.Load(xmlpath);
            XmlNode rootNode = contXml.DocumentElement;
            XmlNode subNode = null;

            switch (_alarmMethod)
            {
                case "OPINION":
                    subNode = rootNode.SelectSingleNode("/ALARM/OPINION");
                    break;

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

            Zeospace.DeskPlus.Account.DAL.User userDal = new DeskPlus.Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> userlist = userDal.GetUserInfo(_dpInfo.CompanyID, _dpInfo.UserID, true, false);
            foreach (var info in userlist)
            {
                //info.DeptID : 인사부서
                //_dpInfo.DeptID : 상신자 부서
                if (info.DeptID == _dpInfo.DeptID)
                {
                    info.DeptBase = UtilHelper.GetLangName(
                                    info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                    info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                    info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                    info.DeptZhTw.ToNull(), _preLang);

                    info.TitleBase = UtilHelper.GetLangName(
                                    info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                    info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                    info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                    info.TitleZhTw.ToNull(), _preLang);

                    info.NameBase = UtilHelper.GetLangName(
                                    info.NameBase.ToNull(), info.NameKo.ToNull(),
                                    info.NameEn.ToNull(), info.NameJa.ToNull(),
                                    info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                    info.NameZhTw.ToNull(), _preLang);

                    _dpInfo.DeptBase = info.DeptBase.ToNull();
                    _dpInfo.TitleBase = info.TitleBase.ToNull();
                    _dpInfo.NameBase = info.NameBase.ToNull();
                    break;
                }
            }

            userlist = userDal.GetUserInfo(_dpInfo.CompanyID, _sendUserInfo.UserID, true, false);
            foreach (var info in userlist)
            {
                //info.DeptID : 인사부서
                //_sendUserInfo.DeptID : 결재자 부서
                if (_sendUserInfo.DeptID == 0) //상신일때
                {
                    _sendUserInfo.DeptBase = _dpInfo.DeptBase.ToNull();
                    _sendUserInfo.TitleBase = _dpInfo.TitleBase.ToNull();
                    _sendUserInfo.DutyBase = _dpInfo.DutyBase.ToNull();
                    _sendUserInfo.NameBase = _dpInfo.NameBase.ToNull();
                }
                else
                {
                    if (info.DeptID == _sendUserInfo.DeptID)
                    {
                        info.DeptBase = UtilHelper.GetLangName(
                                    info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                    info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                    info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                    info.DeptZhTw.ToNull(), _preLang);

                        info.TitleBase = UtilHelper.GetLangName(
                                        info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                        info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                        info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                        info.TitleZhTw.ToNull(), _preLang);

                        info.DutyBase = UtilHelper.GetLangName(
                                        info.DutyBase.ToNull(), info.DutyKo.ToNull(),
                                        info.DutyEn.ToNull(), info.DutyJa.ToNull(),
                                        info.DutyFr.ToNull(), info.DutyZhCn.ToNull(),
                                        info.DutyZhTw.ToNull(), _preLang);

                        info.NameBase = UtilHelper.GetLangName(
                                        info.NameBase.ToNull(), info.NameKo.ToNull(),
                                        info.NameEn.ToNull(), info.NameJa.ToNull(),
                                        info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                        info.NameZhTw.ToNull(), _preLang);

                        _sendUserInfo.DeptBase = info.DeptBase.ToNull();
                        _sendUserInfo.TitleBase = info.TitleBase.ToNull();
                        _sendUserInfo.DutyBase = info.DutyBase.ToNull();
                        _sendUserInfo.NameBase = info.NameBase.ToNull();
                        break;
                    }
                }
            }

            if (_alarmType.Equals("") || subNode == null || _dpInfo == null || _sendUserInfo == null) return null;

            try
            {
                if (_dpInfo.Subject != "")
                {
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&amp;", "&");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&quot;", "\"");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&#35;", "#");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&#39;", "'");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&#40;", "(");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&#41;", ")");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&lt;", "<");
                    _dpInfo.Subject = _dpInfo.Subject.Replace("&gt;", ">");
                }

                string[] arrRtnval = new string[2];

                string action = subNode.SelectSingleNode("SECTION/" + _alarmType).InnerText;
                arrRtnval[0] = subNode.SelectSingleNode("SUBJ/" + _alarmType).InnerText + _dpInfo.Subject;
                arrRtnval[1] = subNode.SelectSingleNode("CONTENT").InnerText;

                if (_alarmType == "OPINION")
                {
                    arrRtnval[0] = subNode.SelectSingleNode("SUBJ/" + _alarmType).InnerText + " [" + _dpInfo.Number3 + "] " + _dpInfo.Subject + " ( 상신자:" + _dpInfo.NameBase + " ) ";
                }

                arrRtnval[1] = arrRtnval[1].Replace("$ACTION$", action);                    //Message
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", arrRtnval[0]);             //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$NUMBER3$", _dpInfo.Number3);          //양식명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCDEPTNAME$", _dpInfo.DeptBase);     //상신자부서명  $ACCDEPTNAME$ / $ACCTITLENAME$ / $ACCUSERNAME$
                arrRtnval[1] = arrRtnval[1].Replace("$ACCTITLENAME$", _dpInfo.TitleBase);   //상신자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$ACCUSERNAME$", _dpInfo.NameBase);     //상신자이름
                arrRtnval[1] = arrRtnval[1].Replace("$USERDEPTNAME$", _sendUserInfo.DeptBase);      //결재자부서명
                arrRtnval[1] = arrRtnval[1].Replace("$USERTITLENAME$", _sendUserInfo.TitleBase);    //결재자직급명
                arrRtnval[1] = arrRtnval[1].Replace("$USERDUTYNAME$", _sendUserInfo.DutyBase);      //결재자직책명
                arrRtnval[1] = arrRtnval[1].Replace("$USERUSERNAME$", _sendUserInfo.NameBase);          //결재자이름
                arrRtnval[1] = arrRtnval[1].Replace("$OPINION$", _sendUserInfo.Opinion);            //결재자의견
                arrRtnval[1] = arrRtnval[1].Replace("$LINKURL$", _linkURL);                         //링크(문서보기)

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("제목<br>" + arrRtnval[0]);
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("내용<br>" + arrRtnval[1]);

                return arrRtnval;   // [0] : Subj / [1] : Content
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[GetAlarmCont] : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 알람대상(회람,전달,이관)
        /// </summary>
        /// <param name="_section">The _section.</param>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        private List<BE.UserInfo> GetAlarmTarget(string _section, int _companyID, string _userList)
        {
            if (_section.Equals("") || _companyID.Equals(0) || _userList.Equals("")) return null;

            try
            {
                List<BE.UserInfo> info = null;
                DAL.Sequence seqDal = new DAL.Sequence();

                if (_section.ToUpper() == "CIRCULATION")
                {
                    // 회람 대상자
                    info = seqDal.GetAlarmCirculation(_companyID, _userList);
                }
                else if (_section.ToUpper() == "FORWARD")
                {
                    //전달 대상자
                    info = seqDal.GetAlarmForward(_companyID, _userList);
                }
                else if (_section.ToUpper() == "PASSED")
                {
                    //이관 대상자
                    info = seqDal.GetAlarmPassed(_companyID, _userList);
                }
                else if (_section.ToUpper() == "RECEIVEDMOD")
                {
                    //담당자변경 대상자
                    info = seqDal.GetAlarmCounterpartuser(_companyID, _userList);
                }

                return info;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.DocumentProcess[GetAlarmTarget] : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 대상URL정보가져오기(Messenger)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <returns></returns>
        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection, int _receiverUserID = 0)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string targetURL = "";

            #region 상황별 url정보 가져오기

            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerW", "");
                    break;

                case "S":   //승인(결재차례)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerS", "");
                    break;

                case "SR":   //승인(수신차례)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerSR", "");
                    break;

                case "O":   //첨언,첨언추가
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerO", "");
                    break;

                case "C":   //완결
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerC", "");
                    break;

                case "RW":  //반려-상신자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerRW", "");
                    break;

                case "RS":  //반려-중간결재자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerRS", "");
                    break;

                case "D":   //보류,보류취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerD", "");
                    break;

                case "L":   //후열
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerL", "");
                    break;

                case "CIR": //회람
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerCir", "");
                    break;

                case "TRANSFER":    //이관
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerTransfer", "");
                    break;

                case "FORWARD":     //전달
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerForward", "");
                    break;

                case "CA":   //상신취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerW", "");
                    break;

                default:    //기타(문서보기)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerView", "");
                    break;
            }

            #endregion 상황별 url정보 가져오기

            targetURL = string.Format(targetURL, baseHttp, fullDomain);
            targetURL = GetApprovalStatusMenuAlarmUrl(_companyID, _receiverUserID, targetURL);

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

            #endregion 파라미터 처리

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 대상URL정보가져오기(Messenger-결재page)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_sequence">The _sequence.</param>
        /// <param name="_approvalTag">The _approval tag.</param>
        /// <param name="_targetUserID">The _target user identifier.</param>
        /// <param name="_targetDeptID">The _target dept identifier.</param>
        /// <param name="_defaultUrl">The _default URL.</param>
        /// <returns></returns>
        private string GetLinkURLMessenger(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl, string _alarmtype, int _receiverUserID = 0)
        {
            //int 형에 null 이 들어갈 수 없다.(shimpark 수정 2013-09-04 )
            //_sequence = (_sequence == null) ? 0 : _sequence;
            //_targetUserID = (_targetUserID == null) ? 0 : _targetUserID;
            //_targetDeptID = (_targetDeptID == null) ? 0 : _targetDeptID;

            _approvalTag = (_approvalTag == null) ? "" : _approvalTag;

            if (_companyID.Equals(0) || _documentID.Equals(0) || _approvalTag.Equals("") || _targetUserID.Equals(0) || _targetDeptID.Equals(0)) return _defaultUrl;

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string targetURL = "";

            #region 상황별 url정보 가져오기

            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    //targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerW", "");
                    targetURL = (_alarmtype.ToUpper().Equals("CONFIRM")) ? AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerConfirm", "") : AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerW", "");
                    break;

                case "S":   //승인
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerS", "");
                    break;

                case "O":   //첨언,첨언추가
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerO", "");
                    break;

                case "C":   //완결
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerC", "");
                    break;

                case "RW":  //반려-상신자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerRW", "");
                    break;

                case "RS":  //반려-중간결재자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerRS", "");
                    break;

                case "D":   //보류,보류취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerD", "");
                    break;

                case "L":   //후열
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerL", "");
                    break;

                case "CIR": //회람
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerCir", "");
                    break;

                case "TRANSFER":    //이관
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerTransfer", "");
                    break;

                case "FORWARD":     //전달
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerForward", "");
                    break;

                default:    //기타(문서보기)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerView", "");
                    break;
            }

            #endregion 상황별 url정보 가져오기

            targetURL = string.Format(targetURL, baseHttp, fullDomain);
            targetURL = GetApprovalStatusMenuAlarmUrl(_companyID, _receiverUserID, targetURL);

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

            #endregion 파라미터 처리

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 개인 설정에 따른 전자결재 메신저 알람 리스트 URL 보정
        /// </summary>
        private string GetApprovalStatusMenuAlarmUrl(int _companyID, int _receiverUserID, string _targetURL)
        {
            string folderStatusID = GetApprovalStatusMenuFolderStatusID(_targetURL);
            if (folderStatusID.Length == 0) return _targetURL;
            if (!IsApprovalStatusNewMenu(_companyID, _receiverUserID)) return _targetURL;

            string baseUrl = GetApprovalStatusMenuBaseUrl(_targetURL);
            if (baseUrl.Length == 0) return _targetURL;

            return baseUrl + "/Approval/Status/DefaultAll.aspx?folderStatusID=" + folderStatusID;
        }

        private string GetApprovalStatusMenuFolderStatusID(string _targetURL)
        {
            string folderStatusID = GetQueryStringValue(_targetURL, "folderStatusID");
            if (folderStatusID.Length > 0)
            {
                switch (folderStatusID.ToUpper())
                {
                    case "RECEIVE":
                        return "Receive";
                    case "REJECT":
                        return "Reject";
                    case "APPROVE":
                        return "Approve";
                    case "TRANSFER":
                        return "Transfer";
                }
            }

            string cabinetID = GetQueryStringValue(_targetURL, "cabinetID");
            switch (cabinetID.ToUpper())
            {
                case "CARBONCOPY_DOC":
                    return "CarboncopyAll";
                case "CARBONCOPY_CIRRECEIPT":
                    return "CarboncopyCirReceiptAll";
                case "CARBONCOPY_FORWARD":
                    return "CarboncopyForwardReceiveAll";
            }

            return "";
        }

        private bool IsApprovalStatusNewMenu(int _companyID, int _receiverUserID)
        {
            if (_companyID.Equals(0) || _receiverUserID.Equals(0)) return false;

            string cacheKey = _companyID + "_" + _receiverUserID;
            if (!approvalStatusMenuTypeCache.ContainsKey(cacheKey))
            {
                string menuType = "OLD";

                try
                {
                    Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                    List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(_companyID, _receiverUserID);
                    if (list != null && list.Count > 0)
                    {
                        menuType = list[0].ApprovalStatusMenuType.ToNull();
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DocumentProcess[IsApprovalStatusNewMenu] : " + ex);
                }

                approvalStatusMenuTypeCache.Add(cacheKey, menuType);
            }

            return approvalStatusMenuTypeCache[cacheKey].Equals("NEW", StringComparison.OrdinalIgnoreCase);
        }

        private string GetApprovalStatusMenuBaseUrl(string _targetURL)
        {
            string url = _targetURL;
            if (url.IndexOf("?") > 0) url = url.Split('?')[0];

            int approvalIndex = url.IndexOf("/Approval/", StringComparison.OrdinalIgnoreCase);
            if (approvalIndex < 0) return "";

            return url.Substring(0, approvalIndex);
        }

        private string GetQueryStringValue(string _url, string _key)
        {
            if (_url.IndexOf("?") < 0) return "";

            string queryString = _url.Split('?')[1];
            string[] paramList = queryString.Split('&');
            foreach (string param in paramList)
            {
                string[] keyValue = param.Split('=');
                if (keyValue.Length > 1 && keyValue[0].Equals(_key, StringComparison.OrdinalIgnoreCase))
                {
                    return keyValue[1];
                }
            }

            return "";
        }

        /// <summary>
        /// 대상URL정보가져오기(Mobile)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <returns></returns>
        private string GetLinkURLMobile(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string BPage = "";
            string targetURL = "";

            #region 상황별 url정보 가져오기

            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileW", "");
                    break;

                case "S":   //승인(결재차례)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileS", "");
                    break;

                case "SR":   //승인(수신차례)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileSR", "");
                    break;

                case "O":   //첨언,첨언추가
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileO", "");
                    break;

                case "C":   //완결
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileC", "");
                    break;

                case "RW":  //반려-상신자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileRW", "");
                    break;

                case "RS":  //반려-중간결재자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileRS", "");
                    break;

                case "D":   //보류,보류취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileD", "");
                    break;

                case "L":   //후열
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileL", "");
                    break;

                case "CIR": //회람
                    BPage = "CirRecv";
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileCir", "");
                    break;

                case "TRANSFER":    //이관
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileTransfer", "");
                    break;

                case "FORWARD":     //전달
                    BPage = "Forward";
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileForward", "");
                    break;

                case "CA":   //상신취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileW", "");
                    break;

                default:    //기타(문서보기)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileView", "");
                    break;
            }

            #endregion 상황별 url정보 가져오기


            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("DID", _documentID.ToString());
            paraDic.Add("BPage", BPage.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));

            if (!String.IsNullOrEmpty(targetURL)) targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        /// <summary>
        /// 대상URL정보가져오기(Mobile)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <param name="_sequence">The _sequence.</param>
        /// <param name="_approvalTag">The _approval tag.</param>
        /// <param name="_targetUserID">The _target user identifier.</param>
        /// <param name="_targetDeptID">The _target dept identifier.</param>
        /// <param name="_defaultUrl">The _default URL.</param>
        /// <returns></returns>
        private string GetLinkURLMobile(int _companyID, int _documentID, string _urlSection, int _sequence, string _approvalTag, int _targetUserID, int _targetDeptID, string _defaultUrl, string _alarmtype)
        {
            _approvalTag = (_approvalTag == null) ? "" : _approvalTag;

            if (_companyID.Equals(0) || _documentID.Equals(0) || _approvalTag.Equals("") || _targetUserID.Equals(0) || _targetDeptID.Equals(0)) return _defaultUrl;

            string BPage = "";
            string targetURL = "";

            #region 상황별 url정보 가져오기

            switch (_urlSection.ToUpper())
            {
                case "W":   //상신
                    //targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMessengerW", "");
                    if (_alarmtype.ToUpper().Equals("CONFIRM"))
                    {
                        targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileConfirm", "");
                    }
                    else
                    {
                        BPage = "APP";
                        targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileW", "");
                    }
                    break;

                case "S":   //승인
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileS", "");
                    BPage = "APP";
                    break;

                case "O":   //첨언,첨언추가
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileO", "");
                    break;

                case "C":   //완결
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileC", "");
                    break;

                case "RW":  //반려-상신자
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileRW", "");
                    break;

                case "RS":  //반려-중간결재자
                    if (_alarmtype.ToUpper().Equals("REJECTALL"))
                    {
                        targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileRSALL", "");
                    }
                    else
                    {
                        BPage = "APP";
                        targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileRS", "");
                    }
                    break;

                case "D":   //보류,보류취소
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileD", "");
                    break;

                case "L":   //후열
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileL", "");
                    break;

                case "CIR": //회람
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileCir", "");
                    break;

                case "TRANSFER":    //이관
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileTransfer", "");
                    break;

                case "FORWARD":     //전달
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileForward", "");
                    break;

                default:    //기타(문서보기)
                    targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMobileView", "");
                    break;
            }

            #endregion 상황별 url정보 가져오기

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("DID", _documentID.ToString());
            paraDic.Add("Seq", _sequence.ToString());
            paraDic.Add("ATag", _approvalTag);
            paraDic.Add("TargetUID", _targetUserID.ToString());
            paraDic.Add("TargetDID", _targetDeptID.ToString());
            paraDic.Add("BPage", BPage.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));

            if (!String.IsNullOrEmpty(targetURL)) targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }


        /// <summary>
        /// 대상URL정보가져오기(Mail)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_urlSection">The _url section.</param>
        /// <returns></returns>
        private string GetLinkURLMail(int _companyID, int _documentID, string _urlSection)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return "";

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string targetURL = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMail", "");
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

            #endregion 파라미터 처리

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            targetURL = targetURL + "?p=" + encparam;

            return targetURL;
        }

        #endregion 알람발송 단위로직

        #endregion 알람발송 (메일/메신저/SMS)
    }
}
