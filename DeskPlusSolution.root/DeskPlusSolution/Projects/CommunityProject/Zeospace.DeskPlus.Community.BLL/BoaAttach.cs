using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Community.BE;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Community.DAL;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class BoaAttach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 게시물 첨부파일 추가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_boardID"></param>
        /// <param name="_screenShot"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_yearMonth"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _communityID, int _menuID, int _boardID, string _screenShot, string _spoolPath, string _yearMonth)
        {

            string targetPath = "/DeskPlusEIP/FileData/" + _companyID + "/Community/" + _communityID + "/Board/" + _menuID + "/" + _yearMonth + "/" + _boardID;
            
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.Community, _boardID, false, _yearMonth);
            DAL.BoaAttach dal = new DAL.BoaAttach();

            try
            {
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        BoaAttachInfo fileInfo = new BoaAttachInfo();

                        fileInfo.CompanyID = _companyID;
                        fileInfo.CommunityID = _communityID;
                        fileInfo.MenuID = _menuID;
                        fileInfo.BoardID = _boardID;
                        fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                        dal.Add(fileInfo);

                        //경로 없음. 파일명만 옴.
                        if (_screenShot.Length > 0)
                        {
                            if (_screenShot.ToUpper() == fileInfo.FileName.ToUpper())
                            {
                                HttpContext context = HttpContext.Current;
                                //파일이 존재하면 섬네일 파일 생성
                                if (System.IO.File.Exists(
                                    context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID)))
                                {
                                    DAL.Boa dalAttach = new DAL.Boa();
                                    dalAttach.ModByScreenShot(_companyID, _communityID, _menuID, _boardID, fileInfo.FilePath + "Thum_" + fileInfo.FileGUID);

                                    //원본 파일
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(
                                        context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));

                                    System.Drawing.Image thumb = image.GetThumbnailImage(150, 150,
                                        delegate() { return false; }, (IntPtr)0);

                                    //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                                    thumb.Save(context.Server.MapPath(fileInfo.FilePath + "Thum_" + fileInfo.FileGUID));
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 게시물 첨부파일 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_menuID"></param>
        /// <param name="_boardID"></param>
        /// <param name="_attachFileID"></param>
        /// <param name="_screenShot"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _menuID, int _boardID, int _attachFileID, string _screenShot)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Boa dal = new DAL.Boa();
                    DAL.BoaAttach dalAttach = new DAL.BoaAttach();
                    List<BE.BoaAttachInfo> alAttach = dalAttach.GetAttachFileLists(_companyID, _communityID, _menuID, _boardID, _attachFileID);

                    foreach (BE.BoaAttachInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dalAttach.Del(_companyID, _communityID, _menuID, _boardID, _attachFileID);
                            System.IO.File.Delete(filePath);

                            //섬네일 삭제
                            if (_screenShot != "")
                            {
                                string fileName = System.IO.Path.GetFileName(context.Server.MapPath(_screenShot)).ToLower();
                                if (fileName.ToLower() == "thum_" + be.FileGUID.ToLower() || fileName.ToLower() == be.FileName.ToLower())
                                {
                                    dal.ModByScreenShot(_companyID, _communityID, _menuID, _boardID, "");
                                    System.IO.File.Delete(context.Server.MapPath(_screenShot));
                                }
                            }
                        }
                    }

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }
        #endregion
    }
}
