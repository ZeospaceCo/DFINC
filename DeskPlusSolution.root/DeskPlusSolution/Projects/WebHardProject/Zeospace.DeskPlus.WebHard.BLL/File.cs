using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.BLL
{
    public class File : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-31
        /// 작성목적 : 업로드
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_spoolPath"></param>
        /// <param name="_webHardPath"></param>
        /// <returns></returns>
        public string Add(List<BE.FileInfo> _al, string _spoolPath, string _webHardPath)
        {
            try
            {
                var fileID = "";
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    string tmpWebHard = FileHelper.GetServerMapPath(_webHardPath);
                    if (!Directory.Exists(tmpWebHard))
                    {
                        Directory.CreateDirectory(tmpWebHard);
                    }

                    string tmpSpool = FileHelper.GetServerMapPath(_spoolPath);

                    foreach (BE.FileInfo info in _al)
                    {     
                        // DB 처리
                        DAL.File dal = new DAL.File();

                        //if (!dal.Add(info))
                        //{
                        //    return false;
                        //}
                        var rtn = dal.Add(info);
                        if (rtn > 0)
                        {
                            fileID += rtn.ToString() + ",";
                        }

                        // File 처리                       
                        System.IO.File.Move(tmpSpool + info.FileName, tmpWebHard + info.FileGUID);

                        if (info.ScreenShot == "Y")
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromFile(tmpWebHard + info.FileGUID);

                            System.Drawing.Image thumb = image.GetThumbnailImage(150, 150,
                                delegate() { return false; }, (IntPtr)0);

                            //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                            thumb.Save(tmpWebHard + "Thum_" + info.FileGUID);

                            image.Dispose();

                        }
                        
                    }         
         
                    if(Directory.Exists(tmpSpool))
                    {
                        Directory.Delete(tmpSpool);
                    }

                    ts.Complete();
                }

                return fileID.Substring(0, fileID.Length - 1);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return "";
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-08
        /// 작성목적 : 파일 복사 , 전달
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_beforePath"></param>
        /// <returns></returns>
        public bool Copy(List<BE.FileInfo> _al, string _beforePath)
        {
            try
            {
                foreach (BE.FileInfo info in _al)
                {
                    // File 처리                       
                    string tmpbeforePath = FileHelper.GetServerMapPath(_beforePath + info.FileGUID);
                    string tmpWebHard = FileHelper.GetServerMapPath(info.FilePath + info.NewGUID);

                    if (!Directory.Exists(FileHelper.GetServerMapPath(info.FilePath)))
                    {
                        Directory.CreateDirectory(FileHelper.GetServerMapPath(info.FilePath));
                    }

                    System.IO.File.Copy(tmpbeforePath, tmpWebHard);

                    if (info.ScreenShot == "Y")
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(tmpWebHard);

                        System.Drawing.Image thumb = image.GetThumbnailImage(150, 150,
                            delegate() { return false; }, (IntPtr)0);

                        //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                        thumb.Save(FileHelper.GetServerMapPath(info.FilePath + "Thum_" + info.NewGUID));

                        image.Dispose();
                    }

                    // DB 처리
                    DAL.File dal = new DAL.File();

                    if (!dal.Copy(info))
                    {
                        return false;
                    }                    
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Copy", ex);
                return false;
            }
        }



        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-27
        /// 작성목적 : 파일 업로드
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_beforePath"></param>
        /// <returns></returns>
        public bool Attach(List<BE.FileInfo> _al, string _beforePath)
        {
            try
            {
                foreach (BE.FileInfo info in _al)
                {
                    // File 처리                       
                    string tmpbeforePath = FileHelper.GetServerMapPath(_beforePath + info.FileGUID);
                    string tmpWebHard = FileHelper.GetServerMapPath(info.FilePath + info.FileName);

                    if (!Directory.Exists(FileHelper.GetServerMapPath(info.FilePath)))
                    {
                        Directory.CreateDirectory(FileHelper.GetServerMapPath(info.FilePath));
                    }

                    System.IO.File.Copy(tmpbeforePath, tmpWebHard);                                        
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Attach", ex);
                return false;
            }
        }
        

        #endregion 

        #region MOD()
        
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-08
        /// 작성목적 : 파일 이동 
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_beforePath"></param>
        /// <param name="_userID"></param>
        /// <param name="_fileID"></param>
        /// <returns></returns>
        public bool Move(List<BE.FileInfo> _al, string _beforePath, int _userID, string _fileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 중요 표시 변경 
                    DAL.ImpoUser dalImpo = new DAL.ImpoUser();
                    string tmpTypes = "P";
                    if (_al[0].UserID == _userID)
                    {
                        tmpTypes = "P";
                    }
                    else
                    {
                        tmpTypes = "S";
                    }

                    if(!dalImpo.Mod(_al[0].CompanyID, _al[0].UserID, _al[0].FolderID, tmpTypes, _fileID))
                    {
                        return false;
                    }

                    // 2. DB 처리
                    DAL.File dal = new DAL.File();
                    if (!dal.Move(_al[0].CompanyID, _al[0].UserID, _al[0].FolderID, _fileID, _al[0].FilePath))
                    {
                        return false;
                    }

                    foreach (BE.FileInfo info in _al)
                    {  
                        // File 처리                       
                        string tmpbeforePath = FileHelper.GetServerMapPath(_beforePath + info.FileGUID);
                        string tmpWebHard = FileHelper.GetServerMapPath(info.FilePath + info.FileGUID);

                        if (!Directory.Exists(FileHelper.GetServerMapPath(info.FilePath)))
                        {
                            Directory.CreateDirectory(FileHelper.GetServerMapPath(info.FilePath));
                        }

                        System.IO.File.Move(tmpbeforePath, tmpWebHard);

                        if (info.ScreenShot == "Y")
                        {
                            System.IO.File.Move(FileHelper.GetServerMapPath(_beforePath + "Thum_" +  info.FileGUID), FileHelper.GetServerMapPath(info.FilePath + "Thum_" + info.FileGUID));
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Move", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-04
        /// 작성목적 : 휴지통으로 파일 삭제 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deleteYN"></param>
        /// <param name="_tmpValue"></param>
        /// <returns></returns>
        public bool UpdTrash(int _companyID, int _userID, string _deleteYN, string _fileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. 중요 표시 삭제
                    DAL.ImpoUser dal = new DAL.ImpoUser();
                    dal.Del(_companyID, _fileID);

                    // 2. 휴지통으로 삭제 처리 
                    DAL.File dalFile = new DAL.File();
                    if (!dalFile.UpdTrash(_companyID, _userID, _deleteYN, _fileID))
                    {
                        return false;
                    }
                      
                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("UpdTrash", ex);
                return false;
            }
        } 



        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-31
        /// 작성목적 : 파일 삭제 
        /// </summary>
        /// <param name="_al"></param>
        /// <param name="_fileID"></param>
        /// <returns></returns>
        public bool Del(List<BE.FileInfo> _al, string _fileID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // DB 처리
                    DAL.File dal = new DAL.File();

                    if (!dal.Del(_al[0].CompanyID, _al[0].UserID, _fileID))
                    {
                        return false;
                    }

                    ts.Complete();                   
                }

                foreach (BE.FileInfo info in _al)
                {
                    // File 처리            
                    string tmpPath = FileHelper.GetServerMapPath(info.FilePath);
                    string tmpFilePath = System.IO.Path.GetDirectoryName(tmpPath);
                    string tmpFileName = System.IO.Path.GetFileName(tmpPath);

                    string tmpScreenPath = tmpFilePath + "/Thum_" + tmpFileName;

                    System.IO.File.Delete(tmpPath);

                    if (System.IO.File.Exists(tmpScreenPath))
                    {
                        System.IO.File.Delete(tmpScreenPath);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return true;
            }
        } 
        #endregion 
    }
}
