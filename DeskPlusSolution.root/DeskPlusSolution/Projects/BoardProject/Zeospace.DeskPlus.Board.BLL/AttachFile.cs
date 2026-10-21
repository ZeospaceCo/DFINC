using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class AttachFile
    {
        /// <summary>
        /// 첨부파일 추가하기
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
                ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Board, _listID, false, _yearMonth);
                DAL.AttachFiles dalAttach = new DAL.AttachFiles();

                #region 첨부파일 처리하기

                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        AttachFileInfo fileInfo = new AttachFileInfo();
                        fileInfo.CompanyID = _companyID;
                        fileInfo.CategoryID = _categoryID;
                        fileInfo.ListID = _listID;
                        fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                        dalAttach.Add(fileInfo);

                        HttpContext context = HttpContext.Current;

                        if (
                                string.Compare(fileInfo.FileType, ".GIF", true) == 0
                                || string.Compare(fileInfo.FileType, ".JPG", true) == 0
                                || string.Compare(fileInfo.FileType, ".PNG", true) == 0
                                || string.Compare(fileInfo.FileType, ".BMP", true) == 0
                                || string.Compare(fileInfo.FileType, ".EMF", true) == 0
                                )
                        {
                            //모든 이미지 섬네일 만들기 "Small_" 로 표기
                            if (System.IO.File.Exists(
                                context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID)))
                            {
                                //원본 파일
                                System.Drawing.Image image = System.Drawing.Image.FromFile(
                                    context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));

                                #region 600 사이즈 이미지 (미리보기)

                                //최대 가로, 세로 600
                                string imgSizeSmall600 = UtilHelper.GetThumnailSize(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID), 600, 600);
                                Bitmap imgFile600 = new Bitmap(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));
                                Bitmap thumbnail600 = new Bitmap(imgFile600, imgSizeSmall600.Split(',')[0].ToNumber<int>(), imgSizeSmall600.Split(',')[1].ToNumber<int>());
                                //작은 이미지 Small_ 만들기
                                thumbnail600.Save(context.Server.MapPath(fileInfo.FilePath + "Small_" + fileInfo.FileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                imgFile600.Dispose();
                                thumbnail600.Dispose();

                                #endregion 600 사이즈 이미지 (미리보기)

                                #region 350 사이즈 이미지 (스마트폰)

                                //최대 가로, 세로 350
                                string imgSizeSmall350 = UtilHelper.GetThumnailSize(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID), 350, 350);
                                Bitmap imgFile350 = new Bitmap(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));
                                Bitmap thumbnail350 = new Bitmap(imgFile350, imgSizeSmall350.Split(',')[0].ToNumber<int>(), imgSizeSmall350.Split(',')[1].ToNumber<int>());
                                //작은 이미지 Smart_ 만들기
                                thumbnail350.Save(context.Server.MapPath(fileInfo.FilePath + "Smart_" + fileInfo.FileGUID), System.Drawing.Imaging.ImageFormat.Jpeg);
                                imgFile350.Dispose();
                                thumbnail350.Dispose();

                                #endregion 350 사이즈 이미지 (스마트폰)
                            }
                        }

                        //선택한 파일 - 갤러리 첫이미지 생성 "Thum_" 로 표기
                        if (_screenShot.Length > 0)
                        {
                            if (_screenShot.ToUpper() == fileInfo.FileName.ToUpper())
                            {
                                if (System.IO.File.Exists(
                                    context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID)))
                                {
                                    Board.DAL.Lists dal = new DAL.Lists();
                                    dal.ModByScreenShot(_companyID, _categoryID, _listID, fileInfo.FilePath + "Thum_" + fileInfo.FileGUID);

                                    //원본 파일
                                    System.Drawing.Image image = System.Drawing.Image.FromFile(
                                        context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID));

                                    string imgSize = UtilHelper.GetThumnailSize(context.Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID), 230, 160);

                                    System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                                        delegate() { return false; }, (IntPtr)0);

                                    //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                                    thumb.Save(context.Server.MapPath(fileInfo.FilePath + "Thum_" + fileInfo.FileGUID));
                                }
                            }
                        }
                    }
                }

                #endregion 첨부파일 처리하기

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AttachFile[Add] : " + ex);
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
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    string screenShot = "";

                    #region 섬네일 체크

                    DAL.Lists dal = new DAL.Lists();
                    List<ListInfo> alList = dal.GetList(_companyID, _categoryID, _listID);

                    foreach (ListInfo info in alList)
                    {
                        screenShot = info.ScreenShot;
                    }

                    #endregion 섬네일 체크

                    #region 첨부파일 삭제

                    DAL.AttachFiles dalAttach = new DAL.AttachFiles();
                    List<BE.AttachFileInfo> alAttach = dalAttach.GetAttachFileLists(_companyID, _categoryID, _listID, _attachFileID);

                    foreach (BE.AttachFileInfo be in alAttach)
                    {
                        string filePath = context.Server.MapPath(be.FilePath + be.FileGUID);

                        if (System.IO.File.Exists(filePath))
                        {
                            dalAttach.Del(_companyID, _categoryID, _listID, _attachFileID);

                            try
                            {
                                System.IO.File.Delete(filePath);
                            }
                            catch (Exception ex)
                            {
                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                            }

                            //섬네일 삭제 여부
                            if (screenShot.Length > 0)
                            {
                                string fileName = System.IO.Path.GetFileName(context.Server.MapPath(screenShot)).ToLower();
                                if (fileName.ToLower() == "thum_" + be.FileGUID.ToLower() || fileName.ToLower() == be.FileName.ToLower())
                                {
                                    dal.ModByScreenShot(_companyID, _categoryID, _listID, "");
                                    try
                                    {
                                        System.IO.File.Delete(context.Server.MapPath(screenShot));
                                    }
                                    catch (Exception ex)
                                    {
                                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del screenShot : " + ex);
                                    }
                                }
                            }
                        }
                    }

                    #endregion 첨부파일 삭제

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
    }
}