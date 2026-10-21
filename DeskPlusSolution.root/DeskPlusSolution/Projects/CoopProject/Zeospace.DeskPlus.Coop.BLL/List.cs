using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.BLL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 추가/수정/삭제

        /// <summary>
        /// 요청하기
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_listoptionInfo">The _listoption info.</param>
        /// <param name="_arrBaseDocList">The _arr list.</param>
        /// <param name="_arrCoopList">The _arr coop list.</param>
        /// <returns></returns>
        public int Add(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo,
            string[] _arrBaseDocList, string[] _arrCoopList)
        {
            if (_listinfo == null) return -1;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();
                int listID = dalList.Add(_listinfo);
                _listinfo.ListID = listID;

                #region 옵션 (질문글 및 답글에 대한 최종 상태값)

                if (_listoptionInfo != null)
                {
                    DAL.ListOption listODal = new DAL.ListOption();
                    _listoptionInfo.ListID = listID;
                    listODal.Add(_listoptionInfo);
                }

                #endregion 옵션 (질문글 및 답글에 대한 최종 상태값)

                #region 수신자

                if (_listinfo.ToUserList.Length > 0)
                {
                    DAL.ListTO toDal = new DAL.ListTO();
                    //toDal.Del(_listinfo.CompanyID, listID);
                    toDal.Add(_listinfo.CompanyID, listID, _listinfo.ToUserList, _listinfo.WriteUserID);
                }

                #endregion 수신자

                #region 참조자

                if (_listinfo.CCUserList.Length > 0)
                {
                    DAL.ListCC ccDal = new DAL.ListCC();
                    //ccDal.Del(_listinfo.CompanyID, listID);
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
                        BLL.AttachFile attachDal = new AttachFile();
                        attachDal.Add(_listinfo.CompanyID, listID, _listinfo.HiddenSpoolPath, "");
                    }
                }

                #endregion 첨부파일 추가

                #region 전자결재 근거문서

                if (_arrBaseDocList != null)
                {
                    List<BE.APPBaseDocInfo> baselist = new List<BE.APPBaseDocInfo>();
                    DAL.APPBaseDoc appDal = new DAL.APPBaseDoc();

                    for (int i = 0; i < _arrBaseDocList.Length; i++)
                    {
                        BE.APPBaseDocInfo baseInfo = new BE.APPBaseDocInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.DocumentID = _arrBaseDocList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.ListID = listID;
                        baseInfo.Name = _arrBaseDocList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                        baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                        baseInfo.Path = "";
                        baseInfo.BaseOrder = i + 1;

                        appDal.Add(baseInfo);
                    }
                }

                #endregion 전자결재 근거문서

                #region 업무협업 근거문서

                if (_arrCoopList != null)
                {
                    List<BE.BaseCoopInfo> baselist = new List<BE.BaseCoopInfo>();
                    DAL.BaseCoop baseDal = new DAL.BaseCoop();

                    for (int i = 0; i < _arrCoopList.Length; i++)
                    {
                        BE.BaseCoopInfo baseInfo = new BE.BaseCoopInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.ListID = _listinfo.ListID;
                        baseInfo.GroupID = _listinfo.ListID;
                        baseInfo.BaseCoopID = _arrCoopList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.BaseOrder = i + 1;
                        baseInfo.WriteUserID = _listinfo.WriteUserID;

                        baseDal.Add(baseInfo);
                    }
                }

                #endregion 업무협업 근거문서

                ts.Complete();

                return listID;
            }
        }

        /// <summary>
        /// 답변
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_arrBaseDocList">The _arr list.</param>
        /// <param name="_arrCoopList">The _arr coop list.</param>
        /// <returns></returns>
        public int Reply(BE.ListInfo _listinfo, string[] _arrBaseDocList, string[] _arrCoopList)
        {
            if (_listinfo == null) return -1;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();
                _listinfo.ListID = dalList.Add(_listinfo);

                #region 수신자

                if (_listinfo.ToUserList.Length > 0)
                {
                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Del(_listinfo.CompanyID, _listinfo.GroupID);
                    toDal.Add(_listinfo.CompanyID, _listinfo.GroupID, _listinfo.ToUserList, _listinfo.WriteUserID);
                }

                #endregion 수신자

                #region 참조자

                if (_listinfo.CCUserList.Length > 0)
                {
                    DAL.ListCC ccDal = new DAL.ListCC();
                    ccDal.Del(_listinfo.CompanyID, _listinfo.GroupID);
                    ccDal.Add(_listinfo.CompanyID, _listinfo.GroupID, _listinfo.CCUserList, _listinfo.WriteUserID);
                }

                #endregion 참조자

                #region 첨부파일 추가

                if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                    if (di.Exists)
                    {
                        BLL.AttachFile attachDal = new AttachFile();
                        attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.HiddenSpoolPath, "");
                    }
                }

                #endregion 첨부파일 추가

                #region 전자결재 근거문서

                if (_arrBaseDocList != null)
                {
                    List<BE.APPBaseDocInfo> baselist = new List<BE.APPBaseDocInfo>();
                    DAL.APPBaseDoc appDal = new DAL.APPBaseDoc();

                    for (int i = 0; i < _arrBaseDocList.Length; i++)
                    {
                        BE.APPBaseDocInfo baseInfo = new BE.APPBaseDocInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.DocumentID = _arrBaseDocList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.ListID = _listinfo.ListID;
                        baseInfo.Name = _arrBaseDocList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                        baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                        baseInfo.Path = "";
                        baseInfo.BaseOrder = i + 1;

                        appDal.Add(baseInfo);
                    }
                }

                #endregion 전자결재 근거문서

                #region 업무협업 근거문서

                if (_arrCoopList != null)
                {
                    List<BE.BaseCoopInfo> baselist = new List<BE.BaseCoopInfo>();
                    DAL.BaseCoop baseDal = new DAL.BaseCoop();

                    for (int i = 0; i < _arrCoopList.Length; i++)
                    {
                        BE.BaseCoopInfo baseInfo = new BE.BaseCoopInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.ListID = _listinfo.ListID;
                        baseInfo.GroupID = _listinfo.GroupID;
                        baseInfo.BaseCoopID = _arrCoopList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.BaseOrder = i + 1;
                        baseInfo.WriteUserID = _listinfo.WriteUserID;

                        baseDal.Add(baseInfo);
                    }
                }

                #endregion 업무협업 근거문서

                ts.Complete();

                return _listinfo.ListID;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 수정하기
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_listoptionInfo">The _listoption info.</param>
        /// <param name="_arrBaseDocList">The _arr list.</param>
        /// <param name="_arrBaseCoopList">The _arr base coop list.</param>
        /// <returns></returns>
        public bool Mod(BE.ListInfo _listinfo, BE.ListOptionInfo _listoptionInfo, string[] _arrBaseDocList, string[] _arrCoopList)
        {
            if (_listinfo == null) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();
                dalList.Mod(_listinfo);

                #region 옵션

                if (_listoptionInfo != null)
                {
                    DAL.ListOption loList = new DAL.ListOption();
                    loList.ModStatus(_listoptionInfo);
                }

                #endregion 옵션

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
                        BLL.AttachFile attachDal = new AttachFile();
                        attachDal.Add(_listinfo.CompanyID, _listinfo.ListID, _listinfo.HiddenSpoolPath, "");
                    }
                }

                #endregion 첨부파일 추가

                #region 전자결재 근거문서 추가

                if (_arrBaseDocList != null)
                {
                    List<BE.APPBaseDocInfo> baselist = new List<BE.APPBaseDocInfo>();
                    DAL.APPBaseDoc appDal = new DAL.APPBaseDoc();

                    for (int i = 0; i < _arrBaseDocList.Length; i++)
                    {
                        BE.APPBaseDocInfo baseInfo = new BE.APPBaseDocInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.DocumentID = _arrBaseDocList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.ListID = _listinfo.ListID;
                        baseInfo.Name = _arrBaseDocList[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                        baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                        baseInfo.Path = "";
                        baseInfo.BaseOrder = i + 1;

                        appDal.Add(baseInfo);
                    }
                }

                #endregion 전자결재 근거문서 추가

                #region 업무협업 근거문서 추가

                if (_arrCoopList != null)
                {
                    List<BE.BaseCoopInfo> baselist = new List<BE.BaseCoopInfo>();
                    DAL.BaseCoop baseDal = new DAL.BaseCoop();

                    for (int i = 0; i < _arrCoopList.Length; i++)
                    {
                        BE.BaseCoopInfo baseInfo = new BE.BaseCoopInfo();

                        baseInfo.CompanyID = _listinfo.CompanyID;
                        baseInfo.ListID = _listinfo.ListID;
                        baseInfo.GroupID = _listinfo.GroupID;
                        baseInfo.BaseCoopID = _arrCoopList[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                        baseInfo.BaseOrder = i + 1;
                        baseInfo.WriteUserID = _listinfo.WriteUserID;

                        baseDal.Add(baseInfo);
                    }
                }

                #endregion 업무협업 근거문서 추가

                ts.Complete();

                return true;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 수신자 및 참조자 정보 수정하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_toUserList">The _to user list.</param>
        /// <param name="_ccUserList">The _CC user list.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public bool ModToCc(int _companyID, int _listID, string _toUserList, string _ccUserList, int _writeUserID)
        {
            if (_companyID == 0 || _listID == 0) return false;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                #region 수신자

                DAL.ListTO toDal = new DAL.ListTO();
                toDal.Del(_companyID, _listID);

                if (_toUserList.Length > 0)
                {
                    toDal.Add(_companyID, _listID, _toUserList, _writeUserID);
                }

                #endregion 수신자

                #region 참조자

                DAL.ListCC ccDal = new DAL.ListCC();
                ccDal.Del(_companyID, _listID);

                if (_ccUserList.Length > 0)
                {
                    ccDal.Add(_companyID, _listID, _ccUserList, _writeUserID);
                }

                #endregion 참조자

                ts.Complete();

                return true;
            }
        }

        #region 삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 그룹별 리스트 삭제(완전삭제)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _groupID)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dal = new DAL.List();

                var dtrList = dal.GetListByGroup(_companyID, _groupID);
                if (dtrList != null)
                {
                    if (dtrList.HasRows)
                    {
                        while (dtrList.Read())
                        {
                            DelReply(
                                dtrList["CompanyID"].ToString().ToNumber<int>(),
                                dtrList["ListID"].ToString().ToNumber<int>(),
                                dtrList["GroupID"].ToString().ToNumber<int>());
                        }
                    }
                }

                ts.Complete();
            }

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 리스트 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_groupID">The _group ID.</param>
        /// <returns></returns>
        public bool DelReply(int _companyID, int _listID, int _groupID)
        {
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.List dalList = new DAL.List();

                //질문글 일 경우, 수신자/참조자/옵션을 삭제할 수 있다.
                if (_listID == _groupID)
                {
                    #region 수신자

                    DAL.ListTO toDal = new DAL.ListTO();
                    toDal.Del(_companyID, _listID);

                    #endregion 수신자

                    #region 참조자

                    DAL.ListCC ccDal = new DAL.ListCC();
                    ccDal.Del(_companyID, _listID);

                    #endregion 참조자

                    #region 옵션

                    DAL.ListOption loDal = new DAL.ListOption();
                    loDal.Del(_companyID, _listID);

                    #endregion 옵션
                }

                #region 첨부파일

                DAL.Attach dalAttach = new DAL.Attach();
                List<BE.AttachInfo> alAttach = dalAttach.GetAttachFileLists(_companyID, _listID);

                HttpContext context = HttpContext.Current;
                string filePath = "";

                foreach (BE.AttachInfo be in alAttach)
                {
                    filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                    dalAttach.Del(be.CompanyID, be.ListID, be.AttachFileID);

                    //if (System.IO.File.Exists(filePath))
                    //{
                    //    System.IO.File.Delete(filePath);
                    //}
                }

                #endregion 첨부파일

                #region 전자결재 근거문서

                DAL.APPBaseDoc dalApp = new DAL.APPBaseDoc();
                dalApp.Del(_companyID, _listID);

                #endregion 전자결재 근거문서

                #region 업무협업 근거문서

                DAL.BaseCoop dalCoop = new DAL.BaseCoop();
                dalCoop.Del(_companyID, _listID);

                #endregion 업무협업 근거문서

                dalList.Del(_companyID, _listID);

                ts.Complete();

                return true;
            }
        }

        #endregion 삭제

        #endregion 추가/수정/삭제
    }
}