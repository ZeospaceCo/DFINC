using System;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class CirculationDocument : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #endregion GET

        #region ADD

        /// <summary>
        /// 회람추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_recvUserList">The _recv user list.</param>
        /// <param name="_memo">The _memo.</param>
        /// <returns></returns>
        public bool Add(int _companyID, string _documentID, int _userID, string _recvUserList, string _memo, string _menuName)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string nowYearMon = FileHelper.GetYearMonth();

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    int cirID = 0;
                    DAL.CirculationDocument dalCD = new DAL.CirculationDocument();
                    DAL.CirculationUser dalCU = new DAL.CirculationUser();
                    DAL.AttachFile dalAttach = new DAL.AttachFile();

                    foreach (string docID in _documentID.Split(','))
                    {
                        if (_menuName == "DocProcess")
                            cirID = dalCD.AddProcess(_companyID, docID.ToNumber<int>(), _userID, _memo);
                        else if (_menuName == "Doc")
                            cirID = dalCD.Add(_companyID, docID.ToNumber<int>(), _userID, _memo);

                        if (cirID > 0)
                        {
                            foreach (string rUserID in _recvUserList.Split(','))
                            {
                                BE.CirculationUserInfo be = new CirculationUserInfo();
                                be.CompanyID = _companyID;
                                be.DocumentID = docID.ToNumber<int>();
                                be.CirculationDocumentID = cirID;
                                be.UserID = rUserID.ToNumber<int>();
                                dalCU.Add(be);
                            }

                            #region 첨부파일 이관 폴더로 복사하기

                            var attList = dalAttach.GetAttachFilePathsByPath(_companyID, docID.ToNumber<int>());

                            if (attList.Count > 0)
                            {
                                //수신문서일경우 연관문서 갯수만큼 반복
                                for (int i = 0; i < attList.Count; i++)
                                {
                                    string oriFileName;
                                    string oriFolderPath = attList[i].FilePath;
                                    string destFolderPath;
                                    string destFile;

                                    //회람 첨부파일 폴더 생성하기
                                    //DeskPlusEIP/FileData/1/approval/circulation/43/1390/attach/
                                    destFolderPath = baseFileDirectory + _companyID + "/Approval/Circulation/" + nowYearMon + "/" + cirID + "/Attach";
                                    FileHelper.CreateSubFolder(destFolderPath);

                                    if (System.IO.Directory.Exists(context.Server.MapPath(oriFolderPath.TrimEnd('/'))))
                                    {
                                        string[] files = System.IO.Directory.GetFiles(context.Server.MapPath(oriFolderPath.TrimEnd('/')));

                                        foreach (string s in files)
                                        {
                                            oriFileName = System.IO.Path.GetFileName(s);
                                            destFile = System.IO.Path.Combine(context.Server.MapPath(destFolderPath), oriFileName);
                                            System.IO.File.Copy(s, destFile, true);
                                        }
                                    }
                                }
                            }

                            #endregion 첨부파일 이관 폴더로 복사하기
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion ADD

        #region MOD

        #endregion MOD

        #region DEL

        /// <summary>
        /// 작성자 : 심재운
        /// 회람 문서 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_cirDocumentID">The _cir document ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _cirDocumentID, int _userID)
        {
            try
            {
                bool response = false;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (string id in _cirDocumentID.Split(','))
                    {
                        DAL.CirculationDocument biz = new DAL.CirculationDocument();
                        var cirList = biz.GetDocList(_companyID, id.ToNumber<int>());

                        response = biz.Del(_companyID, id.ToNumber<int>(), _userID);

                        if (response)
                        {
                            foreach (var info in cirList)
                            {
                                // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                string filePath = baseFileDirectory + _companyID + "/approval/circulation/" + info.Number2 + "/" + info.CirculationDocumentID;
                                HttpContext context = HttpContext.Current;
                                if (System.IO.Directory.Exists(context.Server.MapPath(filePath))) System.IO.Directory.Delete(context.Server.MapPath(filePath), true);
                            }
                        }
                    }

                    ts.Complete();
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.CirculationDocument[Del] : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// documentid 로 회람 문서 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID)
        {
            try
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //데이터 삭제 전에 정보를 담아 첨부파일 삭제할때 사용한다.
                    DAL.CirculationDocument cirdal = new DAL.CirculationDocument();
                    var cirList = cirdal.GetDocListByDocID(_companyID, _documentID);

                    cirdal.Del(_companyID, _documentID);

                    foreach (var info in cirList)
                    {
                        // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                        string filePath = baseFileDirectory + _companyID + "/approval/circulation/" + info.Number2 + "/" + info.CirculationDocumentID;
                        HttpContext context = HttpContext.Current;
                        if (System.IO.Directory.Exists(context.Server.MapPath(filePath))) System.IO.Directory.Delete(context.Server.MapPath(filePath), true);
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.CirculationDocument[Del] : " + ex);
                throw;
            }
        }

        #endregion DEL
    }
}