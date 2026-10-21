using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Task.BLL
{
    public class Draft : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 임시저장하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 임시저장하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public int Add(BE.DraftInfo _listinfo, BE.DraftOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return -1;

            try
            {
                int draftID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // Draft 테이블
                    DAL.Draft dal = new DAL.Draft();
                    draftID = dal.Add(_listinfo);

                    if (draftID < 0)
                    {
                        return -1;
                    }

                    _listinfo.DraftID = draftID;

                    // DraftOption 테이블
                    DAL.DraftOption opDal = new DAL.DraftOption();
                    _listoptionInfo.DraftID = draftID;
                    opDal.Add(_listoptionInfo);

                    #region 수신자
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        DAL.DraftTO toDal = new DAL.DraftTO();
                        toDal.Add(_listinfo.CompanyID, draftID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        DAL.DraftCC ccDal = new DAL.DraftCC();
                        ccDal.Add(_listinfo.CompanyID, draftID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.DraftAttach attachDal = new DraftAttach();
                            attachDal.Add(_listinfo.CompanyID, draftID, _listinfo.HiddenSpoolPath, "");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.DraftBaseAppInfo> baselist = new List<BE.DraftBaseAppInfo>();
                        DAL.DraftBaseApp appDal = new DAL.DraftBaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.DraftBaseAppInfo baseInfo = new BE.DraftBaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.DraftID = draftID;                            
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
                        List<BE.DraftBaseTaskInfo> baselist = new List<BE.DraftBaseTaskInfo>();
                        DAL.DraftBaseTask taskDal = new DAL.DraftBaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.DraftBaseTaskInfo baseInfo = new BE.DraftBaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.DraftID = draftID;                            
                            baseInfo.BaseID = _arrTaskList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                            baseInfo.WriteUserID = _listinfo.WriteUserID;

                            taskDal.Add(baseInfo);
                        }
                    }
                    #endregion 업무협업 근거문서

                    ts.Complete();
                }

                #region 알람
                #endregion

                return draftID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion 

        #region 수정하기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 수정하기
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <param name="_listoptionInfo"></param>
        /// <param name="_arrAppList"></param>
        /// <param name="_arrTaskList"></param>
        /// <returns></returns>
        public bool Mod(BE.DraftInfo _listinfo, BE.DraftOptionInfo _listoptionInfo, string[] _arrAppList, string[] _arrTaskList)
        {
            if ((_listinfo == null) || (_listoptionInfo == null)) return false;

            try
            {


                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // List테이블
                    DAL.Draft dal = new DAL.Draft();
                    dal.Mod(_listinfo);

                    // ListOption 테이블
                    DAL.DraftOption opDal = new DAL.DraftOption();
                    opDal.Mod(_listoptionInfo);

                    #region 수신자
                    DAL.DraftTO toDal = new DAL.DraftTO();
                    toDal.Del(_listinfo.CompanyID, _listinfo.DraftID);
                    if (_listinfo.ToUserList.Length > 0)
                    {
                        toDal.Add(_listinfo.CompanyID, _listinfo.DraftID, _listinfo.ToUserList, _listinfo.WriteUserID);
                    }
                    #endregion 수신자

                    #region 참조자
                    DAL.DraftCC ccDal = new DAL.DraftCC();
                    ccDal.Del(_listinfo.CompanyID, _listinfo.DraftID);
                    if (_listinfo.CCUserList.Length > 0)
                    {
                        ccDal.Add(_listinfo.CompanyID, _listinfo.DraftID, _listinfo.CCUserList, _listinfo.WriteUserID);
                    }
                    #endregion 참조자

                    #region 첨부파일 추가
                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.DraftAttach attachDal = new DraftAttach();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.DraftID, _listinfo.HiddenSpoolPath, "");
                        }
                    }
                    #endregion 첨부파일 추가

                    #region 전자결재 근거문서
                    if (_arrAppList != null)
                    {
                        List<BE.DraftBaseAppInfo> baselist = new List<BE.DraftBaseAppInfo>();
                        DAL.DraftBaseApp appDal = new DAL.DraftBaseApp();

                        for (int i = 0; i < _arrAppList.Length; i++)
                        {
                            BE.DraftBaseAppInfo baseInfo = new BE.DraftBaseAppInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.DraftID = _listinfo.DraftID;                            
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
                        List<BE.DraftBaseTaskInfo> baselist = new List<BE.DraftBaseTaskInfo>();
                        DAL.DraftBaseTask taskDal = new DAL.DraftBaseTask();

                        for (int i = 0; i < _arrTaskList.Length; i++)
                        {
                            BE.DraftBaseTaskInfo baseInfo = new BE.DraftBaseTaskInfo();

                            baseInfo.CompanyID = _listinfo.CompanyID;
                            baseInfo.DraftID = _listinfo.DraftID;                            
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

        #region 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 첨부파일 정보가져오기
                    DAL.DraftAttach dalAttach = new DAL.DraftAttach();
                    List<BE.DraftAttachInfo> alAttach = dalAttach.GetAttachList(_companyID, _draftID);

                    // 관련 DB 삭제
                    DAL.Draft dalList = new DAL.Draft();
                    bool response = dalList.Del(_companyID, _draftID);

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
    }
}
