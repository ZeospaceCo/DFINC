using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class MyAttachFile
    {
        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장글 첨부파일 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_screenShot">The _screen shot.</param>
        /// <param name="_spoolPath">The _spool path.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _categoryID, int _listID, string _screenShot, string _spoolPath, string _yearMonth)
        {
            try
            {

                ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.MyBoard, _listID, false, _yearMonth);
                DAL.MyAttachFiles dalAttach = new DAL.MyAttachFiles();

                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        MyAttachFileInfo fileInfo = new MyAttachFileInfo();
                        fileInfo.CompanyID = _companyID;
                        fileInfo.CategoryID = _categoryID;
                        fileInfo.MyTemplateID = _listID;
                        fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                        dalAttach.Add(fileInfo);

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
                                    Board.DAL.MyTemplates dal = new DAL.MyTemplates();
                                    dal.ModByScreenShot(_companyID, _categoryID, _listID, fileInfo.FilePath + "Thum_" + fileInfo.FileGUID);

                                    //원본 파일
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(
                                        context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));

                                    string imgSize = UtilHelper.GetThumnailSize(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID), 160, 160);

                                    System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 첨부파일 id별 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_attachFileID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID, int _attachFileID)
        {
            try
            {
                DAL.MyAttachFiles dalAttach = new DAL.MyAttachFiles();
                List<BE.MyAttachFileInfo> al = dalAttach.GetAttachFileLists(_companyID, _categoryID, _listID);

                HttpContext context = HttpContext.Current;

                foreach (BE.MyAttachFileInfo be in al)
                {
                    string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                    if (System.IO.File.Exists(filePath))
                    {
                        dalAttach.Del(_companyID, _categoryID, _listID, _attachFileID);

                        System.IO.File.Delete(filePath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;
            }
        }

        #endregion 추가/수정/삭제
    }
}