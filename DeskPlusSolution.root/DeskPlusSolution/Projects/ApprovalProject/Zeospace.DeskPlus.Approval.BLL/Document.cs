using System;
using System.Data;
using System.IO;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class Document : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #endregion GET

        #region ADD

        #endregion ADD

        #region MOD

        #endregion MOD

        #region DEL

        /// <summary>
        /// 작성자 : 심재운
        /// 완결 문서 및 회람 문서 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_delFolder">if set to <c>true</c> [_del file].</param>
        /// <returns></returns>
        public bool DelDoc(int _companyID, int _documentID, bool _delFolder)
        {
            if (_companyID == 0 || _documentID == 0) return false;

            bool response = false;
            try
            {
                string targetFolder = "";
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.Approval.DAL.Document dalDoc = new DAL.Document();
                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument cirdal = new DAL.CirculationDocument();

                    DataTableReader dtr = dalDoc.GetDocTotalListByGroupID(_companyID, _documentID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                //결재 문서 삭제하기
                                response = dalDoc.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                if (response && _delFolder)
                                {
                                    //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                    targetFolder = baseFileDirectory + dtr["CompanyID"].ToString().ToNumber<int>() + "/Approval/Document/" + dtr["Number2"].ToString() + "/" + dtr["DocumentID"].ToString().ToNumber<int>();
                                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                }

                                #region 회람 문서 삭제하기

                                var cirList = cirdal.GetDocListByDocID(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                cirdal.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                if (_delFolder)
                                {
                                    foreach (var cirInfo in cirList)
                                    {
                                        // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                        targetFolder = baseFileDirectory + cirInfo.CompanyID + "/Approval/Circulation/" + cirInfo.Number2 + "/" + cirInfo.CirculationDocumentID;
                                        if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                    }
                                }

                                #endregion 회람 문서 삭제하기
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelDoc : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-14
        /// 완결 문서 및 회람 문서 삭제
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_delFolder">if set to <c>true</c> [_del folder].</param>
        /// <returns></returns>
        public bool DelDocID(int _companyID, int _documentID, bool _delFolder)
        {
            if (_companyID == 0 || _documentID == 0) return false;

            bool response = false;
            try
            {
                string targetFolder = "";
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.Approval.DAL.Document dalDoc = new DAL.Document();
                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument cirdal = new DAL.CirculationDocument();

                    DataTableReader dtr = dalDoc.GetDocSubList(_companyID, _documentID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                //결재 문서 삭제하기
                                response = dalDoc.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                if (response && _delFolder)
                                {
                                    //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                    targetFolder = baseFileDirectory + dtr["CompanyID"].ToString().ToNumber<int>() + "/Approval/Document/" + dtr["Number2"].ToString() + "/" + dtr["DocumentID"].ToString().ToNumber<int>();
                                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                }

                                #region 회람 문서 삭제하기

                                var cirList = cirdal.GetDocListByDocID(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                cirdal.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                if (_delFolder)
                                {
                                    foreach (var cirInfo in cirList)
                                    {
                                        // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                        targetFolder = baseFileDirectory + cirInfo.CompanyID + "/Approval/Circulation/" + cirInfo.Number2 + "/" + cirInfo.CirculationDocumentID;
                                        if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                    }
                                }

                                #endregion 회람 문서 삭제하기
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelDoc : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-14
        /// 완결 문서 및 회람 문서 삭제 (리스트)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_delFolder">if set to <c>true</c> [_del folder].</param>
        /// <returns></returns>
        public bool DelDocList(int _companyID, int _documentID, bool _delFolder)
        {
            if (_companyID == 0 || _documentID == 0) return false;

            bool response = false;
            try
            {
                string targetFolder = "";
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.Approval.DAL.Document dalDoc = new DAL.Document();
                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument cirdal = new DAL.CirculationDocument();

                    var docListInfo = dalDoc.GetDocList(_companyID, _documentID);

                    if (docListInfo.Count > 0)
                    {
                        if (docListInfo[0].AccountTag.IndexOf("ZP") == -1)
                        {
                            if ((docListInfo[0].AccountTag.IndexOf("ZQ") == -1) && (docListInfo[0].DocumentID != docListInfo[0].GroupID))
                            {
                                //이관 받은 문서 & 이관받은 문서에서 파생된 문서
                                DataTableReader dtr = dalDoc.GetDocSubList(docListInfo[0].CompanyID, docListInfo[0].DocumentID);
                                if (dtr != null)
                                {
                                    if (dtr.HasRows)
                                    {
                                        while (dtr.Read())
                                        {
                                            //결재 문서 삭제하기
                                            response = dalDoc.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            if (response && _delFolder)
                                            {
                                                //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                                targetFolder = baseFileDirectory + dtr["CompanyID"].ToString().ToNumber<int>() + "/Approval/Document/" + dtr["Number2"].ToString() + "/" + dtr["DocumentID"].ToString().ToNumber<int>();
                                                if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                            }

                                            #region 회람 문서 삭제하기

                                            var cirList = cirdal.GetDocListByDocID(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            cirdal.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            if (_delFolder)
                                            {
                                                foreach (var cirInfo in cirList)
                                                {
                                                    // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                                    targetFolder = baseFileDirectory + cirInfo.CompanyID + "/Approval/Circulation/" + cirInfo.Number2 + "/" + cirInfo.CirculationDocumentID;
                                                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                                }
                                            }

                                            #endregion 회람 문서 삭제하기
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 연관되는 모든 문서 삭제
                                DataTableReader dtr = dalDoc.GetDocTotalListByGroupID(_companyID, docListInfo[0].GroupID);
                                if (dtr != null)
                                {
                                    if (dtr.HasRows)
                                    {
                                        while (dtr.Read())
                                        {
                                            //결재 문서 삭제하기
                                            response = dalDoc.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            if (response && _delFolder)
                                            {
                                                //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                                targetFolder = baseFileDirectory + dtr["CompanyID"].ToString().ToNumber<int>() + "/Approval/Document/" + dtr["Number2"].ToString() + "/" + dtr["DocumentID"].ToString().ToNumber<int>();
                                                if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                            }

                                            #region 회람 문서 삭제하기

                                            var cirList = cirdal.GetDocListByDocID(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            cirdal.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                            if (_delFolder)
                                            {
                                                foreach (var cirInfo in cirList)
                                                {
                                                    // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                                    targetFolder = baseFileDirectory + cirInfo.CompanyID + "/Approval/Circulation/" + cirInfo.Number2 + "/" + cirInfo.CirculationDocumentID;
                                                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                                }
                                            }

                                            #endregion 회람 문서 삭제하기
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // ZP문서 & ZP문서에서 파생된 문서
                            DataTableReader dtr = dalDoc.GetDocSubList(docListInfo[0].CompanyID, docListInfo[0].DocumentID);
                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        //결재 문서 삭제하기
                                        response = dalDoc.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                        if (response && _delFolder)
                                        {
                                            //  ex) /DeskPlusEIP/FileData/1/Approval/Document/201206/37
                                            targetFolder = baseFileDirectory + dtr["CompanyID"].ToString().ToNumber<int>() + "/Approval/Document/" + dtr["Number2"].ToString() + "/" + dtr["DocumentID"].ToString().ToNumber<int>();
                                            if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                        }

                                        #region 회람 문서 삭제하기

                                        var cirList = cirdal.GetDocListByDocID(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                        cirdal.Del(dtr["CompanyID"].ToString().ToNumber<int>(), dtr["DocumentID"].ToString().ToNumber<int>());

                                        if (_delFolder)
                                        {
                                            foreach (var cirInfo in cirList)
                                            {
                                                // 경로 : /DeskPlusEIP/FileData/1/approval/circulation/년월/회람문서id/Attach
                                                targetFolder = baseFileDirectory + cirInfo.CompanyID + "/Approval/Circulation/" + cirInfo.Number2 + "/" + cirInfo.CirculationDocumentID;
                                                if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                                            }
                                        }

                                        #endregion 회람 문서 삭제하기
                                    }
                                }
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelDoc : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2022-11-15
        /// 완결 문서 폐기 (리스트)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_documentID">The _document identifier.</param>
        /// <param name="_delFolder">if set to <c>true</c> [_del folder].</param>
        /// <returns></returns>
        public bool DisuseDocList(int _companyID, int _documentID, int _userID)
        {
            if (_companyID == 0 || _documentID == 0) return false;

            bool response = false;
            int tmpCompanyID = (_companyID.ToString() + "000").ToNumber<int>();

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.Approval.DAL.Document dalDoc = new DAL.Document();

                    var docListInfo = dalDoc.GetDocList(_companyID, _documentID);

                    if (docListInfo.Count > 0)
                    {
                        if (docListInfo[0].AccountTag.IndexOf("ZP") == -1)
                        {
                            if ((docListInfo[0].AccountTag.IndexOf("ZQ") == -1) && (docListInfo[0].DocumentID != docListInfo[0].GroupID))
                            {
                                //이관 받은 문서 & 이관받은 문서에서 파생된 문서
                                DataTableReader dtr = dalDoc.GetDocSubList(docListInfo[0].CompanyID, docListInfo[0].DocumentID);
                                if (dtr != null)
                                {
                                    if (dtr.HasRows)
                                    {
                                        while (dtr.Read())
                                        {
                                            //결재 문서 삭제하기
                                            response = dalDoc.DisuseDoc(dtr["CompanyID"].ToString().ToNumber<int>(), tmpCompanyID, dtr["DocumentID"].ToString().ToNumber<int>(), _userID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 연관되는 모든 문서 삭제
                                DataTableReader dtr = dalDoc.GetDocTotalListByGroupID(_companyID, docListInfo[0].GroupID);
                                if (dtr != null)
                                {
                                    if (dtr.HasRows)
                                    {
                                        while (dtr.Read())
                                        {
                                            //결재 문서 삭제하기
                                            response = dalDoc.DisuseDoc(dtr["CompanyID"].ToString().ToNumber<int>(), tmpCompanyID, dtr["DocumentID"].ToString().ToNumber<int>(), _userID);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // ZP문서 & ZP문서에서 파생된 문서
                            DataTableReader dtr = dalDoc.GetDocSubList(docListInfo[0].CompanyID, docListInfo[0].DocumentID);
                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        //결재 문서 삭제하기
                                        response = dalDoc.DisuseDoc(dtr["CompanyID"].ToString().ToNumber<int>(), tmpCompanyID, dtr["DocumentID"].ToString().ToNumber<int>(), _userID);
                                    }
                                }
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DisuseDocList : " + ex);
                throw;
            }
        }

        #endregion DEL
    }
}