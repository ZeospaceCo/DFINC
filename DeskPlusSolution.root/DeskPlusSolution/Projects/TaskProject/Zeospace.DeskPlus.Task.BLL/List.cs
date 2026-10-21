using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Task.BLL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 요청하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-15
        /// 작성목적 : 요청하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public int Add(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return -1;

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_listinfo);

                    if (listID < 0)
                    {
                        return -1;
                    }

                    _listinfo.ListID = listID;

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    _listoptionInfo.ListID = listID;
                    opDal.Add(_listoptionInfo);

                    #region 수신자
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        DAL.ListTO toDal = new DAL.ListTO();
                        toDal.Add(_listinfo.CompanyID, listID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        DAL.ListCC ccDal = new DAL.ListCC();
                        ccDal.Add(_listinfo.CompanyID, listID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, listID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, listID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = listID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = listID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmRequest(_listinfo);
                #endregion

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion

        #region 답변하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-16
        /// 작성목적 : 답변하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public int Reply(BE.ListInfo _listinfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if (_listinfo == null) return -1;

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_listinfo);

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, listID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "REPLY");
                #endregion

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Reply", ex);
                return -1;
            }
        }

        #endregion

        #region 완료취소하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 완료취소하기 
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <returns></returns>
        public int CompCancel(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo)
        {
            if (_listinfo == null) return -1;

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_listinfo);

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.ModCompCancel(_listoptionInfo);

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "CANCEL");
                #endregion

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CompCancel", ex);
                return -1;
            }
        }

        #endregion

        #region 수정하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 수정하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {


                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    dal.Mod(_listinfo);

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.Mod(_listoptionInfo);

                    #region 수신자
                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        toDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    DAL.ListCC ccDal = new DAL.ListCC();
                    ccDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        ccDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, _listinfo.ListID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmRequest(_listinfo);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }

        #endregion

        #region 답변 수정하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-20
        /// 작성목적 : 답변 수정하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _listinfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if (_listinfo == null) return false;

            try
            {

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    dal.Mod(_listinfo);

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, _listinfo.ListID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }

        #endregion

        #region 접수하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-20
        /// 작성목적 : 접수하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Accept(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {
                int listID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블 msg 처리
                    DAL.List dal = new DAL.List();
                    _listinfo.IsMsg = "1";
                    dal.AddMsg(_listinfo);

                    if (_listinfo.ContentText != "")
                    {
                        _listinfo.IsMsg = "0";
                        listID = dal.Add(_listinfo);
                    }

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.ModAccept(_listoptionInfo);

                    #region 수신자
                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        toDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    DAL.ListCC ccDal = new DAL.ListCC();
                    ccDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        ccDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            if (listID == 0)
                            {
                                _listinfo.IsMsg = "0";
                                listID = dal.Add(_listinfo);
                            }

                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, listID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        if (listID == 0)
                        {
                            _listinfo.IsMsg = "0";
                            listID = dal.Add(_listinfo);
                        }

                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        if (listID == 0)
                        {
                            _listinfo.IsMsg = "0";
                            listID = dal.Add(_listinfo);
                        }

                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서


                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "ACCEPT");
                #endregion

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Accept", ex);
                return false;
            }
        }

        #endregion

        #region 완료하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-20
        /// 작성목적 : 완료하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Complete(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {
                int listID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블 msg 처리
                    DAL.List dal = new DAL.List();
                    _listinfo.IsMsg = "1";
                    dal.AddMsg(_listinfo);

                    if (_listinfo.ContentText != "")
                    {
                        _listinfo.IsMsg = "0";
                        listID = dal.Add(_listinfo);
                    }

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.ModComplete(_listoptionInfo);

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            if (listID == 0)
                            {
                                _listinfo.IsMsg = "0";
                                listID = dal.Add(_listinfo);
                            }

                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, listID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        if (listID == 0)
                        {
                            _listinfo.IsMsg = "0";
                            listID = dal.Add(_listinfo);
                        }

                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        if (listID == 0)
                        {
                            _listinfo.IsMsg = "0";
                            listID = dal.Add(_listinfo);
                        }

                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서


                    ts.Complete();

                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "COMPLETE");
                #endregion


                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Complete", ex);
                return false;
            }
        }

        #endregion

        #region 재검토요청

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 재검토요청 
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <returns></returns>
        public bool Reject(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo)
        {
            if (_listinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블 msg 처리
                    DAL.List dal = new DAL.List();
                    _listinfo.IsMsg = "1";
                    dal.AddMsg(_listinfo);

                    if (_listinfo.ContentText != "")
                    {
                        _listinfo.IsMsg = "0";
                        dal.Add(_listinfo);
                    }

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.ModReject(_listoptionInfo);

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "REJECT");
                #endregion


                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Reject", ex);
                return false;
            }
        }

        #endregion

        #region 재요청

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-21
        /// 작성목적 : 재요청
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Recall(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블 msg 처리
                    DAL.List dal = new DAL.List();
                    _listinfo.IsMsg = "1";
                    dal.AddMsg(_listinfo);

                    // List테이블
                    dal.Mod(_listinfo);

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    opDal.ModRecall(_listoptionInfo);

                    #region 수신자
                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        toDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    DAL.ListCC ccDal = new DAL.ListCC();
                    ccDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        ccDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.GroupID, _listinfo.HiddenSpoolPath, "");

                            SetDocStream(_listinfo.CompanyID, _listinfo.ListID, "TAS_ATTACH");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();
                        DAL.BaseApp appDal = new DAL.BaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }
                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();
                        DAL.BaseTask taskDal = new DAL.BaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = _listinfo.ListID;
                            baseInfo.GroupID = _listinfo.GroupID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서


                    ts.Complete();

                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmRequest(_listinfo);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Recall", ex);
                return false;
            }
        }

        #endregion

        #region 임시저장글 요청하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 임시저장글 요청하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public int Add(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList, int _draftID)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return -1;

            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_listinfo);

                    if (listID < 0)
                    {
                        return -1;
                    }

                    _listinfo.ListID = listID;

                    // ListOption 테이블
                    DAL.ListOption opDal = new DAL.ListOption();
                    _listoptionInfo.ListID = listID;
                    opDal.Add(_listoptionInfo);

                    #region 수신자
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        DAL.ListTO toDal = new DAL.ListTO();
                        toDal.Add(_listinfo.CompanyID, listID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        DAL.ListCC ccDal = new DAL.ListCC();
                        ccDal.Add(_listinfo.CompanyID, listID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가

                    //임시저장 첨부파일 추가 
                    if (_listinfo.BeforeHiddenPath != null)
                    {
                        if (!_listinfo.BeforeHiddenPath.Equals(""))
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Mod(_listinfo.CompanyID, listID, listID, _listinfo.BeforeHiddenPath);
                        }
                    }

                    HttpContext context = HttpContext.Current;
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach attachDal = new Attach();
                            attachDal.Add(_listinfo.CompanyID, listID, listID, _listinfo.HiddenSpoolPath, "");
                        }
                    }

                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    DAL.BaseApp appDal = new DAL.BaseApp();

                    //임시저장 전자결재 추가
                    appDal.Add(_listinfo.CompanyID, listID, _draftID);

                    if (_arrAppList != null)
                    {
                        List<BE.BaseAppInfo> baselist = new List<BE.BaseAppInfo>();


                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.BaseAppInfo baseInfo = new BE.BaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = listID;
                            baseInfo.DocumentID = _arrAppList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.Name = _arrAppList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                            baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                            baseInfo.Path = "";
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            appDal.Add(baseInfo);
                        }
                    }

                    #endregion 전자결재 근거문서

                    #region 업무협업 근거문서
                    DAL.BaseTask taskDal = new DAL.BaseTask();

                    //임시저장 업무협업 추가
                    taskDal.Add(_listinfo.CompanyID, listID, _draftID);

                    if (_arrTaskList != null)
                    {
                        List<BE.BaseTaskInfo> baselist = new List<BE.BaseTaskInfo>();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.BaseTaskInfo baseInfo = new BE.BaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.ListID = listID;
                            baseInfo.GroupID = listID;
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }

                    #endregion 업무협업 근거문서

                    ts.Complete();
                }


                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmRequest(_listinfo);
                #endregion

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion

        #region 변경저장

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-28
        /// 작성목적 : 변경저장
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_to"></param>
        /// <param name="_cc"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public bool IngDetail(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string _to, string _cc, string _date)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블 msg 처리
                    DAL.List dal = new DAL.List();
                    int ListID = dal.AddMsg(_listinfo);

                    if (_date == "Y")
                    {
                        DAL.HistoryOption hopDal = new DAL.HistoryOption();
                        hopDal.Add(_listinfo.CompanyID, ListID, _listinfo.GroupID, _listinfo.WriteUserID);

                        // ListOption 테이블
                        DAL.ListOption opDal = new DAL.ListOption();
                        opDal.ModIngDetail(_listoptionInfo);
                    }

                    #region 수신자

                    if (_to == "Y")
                    {
                        DAL.HistoryTO htoDal = new DAL.HistoryTO();
                        htoDal.Add(_listinfo.CompanyID, ListID, _listinfo.GroupID, _listinfo.WriteUserID);

                        DAL.ListTO toDal = new DAL.ListTO();
                        toDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                        if (_listinfo.ToUserList.Length > 0)
                        {
                            toDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.ToUserList, _listinfo.WriteUserID);
                        }
                    }
                    #endregion 수신자

                    #region 참조자
                    if (_cc == "Y")
                    {
                        DAL.HistoryCC hccDal = new DAL.HistoryCC();
                        hccDal.Add(_listinfo.CompanyID, ListID, _listinfo.GroupID, _listinfo.WriteUserID);

                        DAL.ListCC ccDal = new DAL.ListCC();
                        ccDal.Del(_listinfo.CompanyID, _listinfo.ListID);
                        if (_listinfo.CCUserList.Length > 0)
                        {
                            ccDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.CCUserList, _listinfo.WriteUserID);
                        }
                    }
                    #endregion 참조자

                    ts.Complete();
                }

                #region 알람
                BLL.Alarm alarmBiz = new BLL.Alarm();
                alarmBiz.AlarmEtc(_listinfo, "INGDETAIL");
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Complete", ex);
                return false;
            }
        }

        #endregion

        #region 하위글 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-16
        /// 작성목적 : 하위글 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttachList(_companyID, _listID);

                    // List , 전자결재 근거문서, 업무협업 근거문서 , 첨부파일 삭제
                    DAL.List dalList = new DAL.List();
                    bool response = dalList.Del(_companyID, _listID);

                    if (response)
                    {
                        if (alAttach.Count > 0)
                        {
                            Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(alAttach[0].FilePath, false);
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return false;
            }
        }

        #endregion

        #region 전체글 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-16
        /// 작성목적 : 전체글 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public bool DelGroup(int _companyID, int _groupID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttachGroupList(_companyID, _groupID);

                    // 관련 DB 삭제
                    DAL.List dalList = new DAL.List();
                    bool response = dalList.DelGroup(_companyID, _groupID);

                    if (response)
                    {
                        if (alAttach.Count > 0)
                        {
                            Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(alAttach[0].FilePath, false);
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelGroup", ex);
                return false;
            }
        }

        #endregion

        #region DOC_STREAM
        public void SetDocStream(int companyID, int listID, string attachType)
        {
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                DAL.Attach dalFile = new DAL.Attach();
                DocStream ds = new DocStream();
                try
                {
                    var newDSList = dalFile.GetAttachList(companyID, listID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachID, attachType);
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachID, attachType);
                    }
                }
                catch (Exception ex)
                {
                    var DSList = dalFile.GetAttachList(companyID, listID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachID, attachType);
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TAS_ATTACH_Addition_DS : " + ex.Message);
                }

            }
        }            
        #endregion
    }
}