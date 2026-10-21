using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class Board : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 추가하기(동적쿼리)
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public int Add(BoardInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Board dal = new DAL.Board();
                    int boardID = dal.Add(_listInfo);

                    if (boardID < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        #region 첨부파일 추가

                        if (_listInfo.HiddenSpoolPath != "" && _listInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;

                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.BoardAttach attachDal = new BoardAttach();
                                attachDal.Add(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, boardID, _listInfo.HiddenSpoolPath, "");
                            }
                        }

                        #endregion 첨부파일 추가

                        #region 3. 조회

                        if (_listInfo.ListViewerList != null)
                        {
                            DAL.BoardViewer iListViewers = new DAL.BoardViewer();
                            for (int i = 0; i < _listInfo.ListViewerList.Count; i++)
                            {
                                BE.BoardViewerInfo tmpListViewerInfo = (BE.BoardViewerInfo)_listInfo.ListViewerList[i];
                                tmpListViewerInfo.CompanyID = _listInfo.CompanyID;
                                tmpListViewerInfo.ProjectID = _listInfo.ProjectID;
                                tmpListViewerInfo.MenuID = _listInfo.MenuID;
                                tmpListViewerInfo.BoardID = boardID;
                                iListViewers.Add(tmpListViewerInfo);
                            }
                        }

                        #endregion 3. 조회

                        ts.Complete();
                        return boardID;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(BoardInfo _listInfo) : " + ex);

                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 첨부파일 수정하기
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Mod(BoardInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Board dal = new DAL.Board();
                    bool success = dal.Mod(_listInfo);
                    int listID = _listInfo.BoardID;
                    if (!success)
                    {
                        return false;
                    }
                    else
                    {
                        #region 2. 첨부파일

                        #region 1. 첨부파일 내역 : 기존 파일을 삭제할 경우 파일삭제 및 DB 삭제 (수정 모드시)

                        if (_listInfo.HiddenFileList != null && _listInfo.HiddenSaveFileList != null)
                        {
                            int attachFileID = 0;
                            string fileFullPath = String.Empty;
                            string delFileList = String.Empty;
                            string[] tmpFileList;
                            string[] fileInfoArr;

                            ArrayList delFileListArr = FileHelper.GetDelFileList(_listInfo.HiddenFileList, _listInfo.HiddenSaveFileList);
                            DAL.BoardAttach iAttachFile = new DAL.BoardAttach();

                            for (int i = 0; i < delFileListArr.Count; i++)
                            {
                                delFileList = delFileListArr[i].ToString();        //삭제된 파일 리스트 정보   AttachFileID | FileFullName
                                tmpFileList = delFileList.Split('|');              //Array로 변환
                                attachFileID = tmpFileList[0].ToNumber<int>();    //AttachFileID
                                fileFullPath = tmpFileList[1].ToString();          //FileFullName "S>23>/DeskPlusApp/FileData/1/Board/200705/108/>chapter11.html>dfef8a4f-d178-4724-bf99-e77868a7d6e7.zeo"
                                fileInfoArr = fileFullPath.Split('>');
                                fileFullPath = fileInfoArr[2] + fileInfoArr[4];     //DeskPlusApp/FileData/1/Board/200705/108/dfef8a4f-d178-4724-bf99-e77868a7d6e7.zeo

                                iAttachFile.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, _listInfo.BoardID, attachFileID);  // DataBase 삭제
                                FileHelper.DelFile(fileFullPath, false);			// 실제 파일 삭제
                            }
                        }

                        #endregion 1. 첨부파일 내역 : 기존 파일을 삭제할 경우 파일삭제 및 DB 삭제 (수정 모드시)

                        #region 첨부파일 추가

                        if (_listInfo.HiddenSpoolPath != "" && _listInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.BoardAttach attachDal = new BoardAttach();
                                attachDal.Add(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, _listInfo.BoardID, _listInfo.HiddenSpoolPath, FileHelper.GetYearMonth());
                            }
                        }

                        #endregion 첨부파일 추가

                        #endregion 2. 첨부파일

                        ts.Complete();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);

                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// Dels the specified _company ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_BoardListID">The _project board list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public bool Del(BoardInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    #region 1. 프로젝트게시판 조회 삭제

                    BE.BoardViewerInfo viewInfo = new BoardViewerInfo();
                    viewInfo.CompanyID = _listInfo.CompanyID;
                    viewInfo.ProjectID = _listInfo.ProjectID;
                    viewInfo.MenuID = _listInfo.MenuID;
                    viewInfo.BoardID = _listInfo.BoardID;

                    DAL.BoardViewer viewer = new DAL.BoardViewer();
                    viewer.Del(viewInfo);

                    #endregion 1. 프로젝트게시판 조회 삭제

                    #region 2. 프로젝트 게시판 짧은답글 삭제

                    DAL.BoardShortReply shortReply = new DAL.BoardShortReply();
                    shortReply.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, _listInfo.BoardID);

                    #endregion 2. 프로젝트 게시판 짧은답글 삭제

                    #region 3. 프로젝트 게시판 첨부파일 삭제

                    DAL.BoardAttach attach = new DAL.BoardAttach();
                    List<BE.BoardAttachInfo> al = attach.GetAttachFile(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, _listInfo.BoardID);

                    string filePath = "";
                    foreach (BE.BoardAttachInfo _be in al)
                    {
                        filePath = _be.FilePath;
                        break;
                    }

                    if (filePath.Length > 0)
                    {
                        filePath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.GetServerMapPath(filePath);

                        if (Directory.Exists(filePath))
                        {
                            Directory.Delete(filePath, true);
                        }

                        attach.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.MenuID, _listInfo.BoardID);
                    }

                    #endregion 3. 프로젝트 게시판 첨부파일 삭제

                    #region 5. 프로젝트 게시판 삭제

                    Zeospace.DeskPlus.PMS.DAL.Board dal = new DAL.Board();
                    dal.Del(_listInfo);

                    #endregion 5. 프로젝트 게시판 삭제

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;
            }
        }

        #endregion DEL()
    }
}