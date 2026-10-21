using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.WebHard.BE;
using System.Transactions;

namespace Zeospace.DeskPlus.WebHard.BLL
{
    public class Folder : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-13
        /// 작성목적 : 웹하드 트리 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetTree(int _companyID, int _userID)
        {
            DAL.Folder dalFolder = new DAL.Folder();
            DataSet ds = dalFolder.GetTree(_companyID, _userID);

            if (ds.Tables[0].Rows.Count == 0)
            {
                FolderInfo info = new FolderInfo();
                info.CompanyID = _companyID;
                info.UserID = _userID;
                info.ParentFolderID = 0;
                info.NameBase = "ROOT";
                info.NameKo = "ROOT";
                info.NameEn = "ROOT";
                info.NameJa = "ROOT";
                info.NameFr = "ROOT";
                info.NameZhcn = "ROOT";
                info.NameZhtw = "ROOT";

                if (dalFolder.Add(info) > 0)
                {
                    ds = dalFolder.GetTree(_companyID, _userID);
                } 
            }

            return ds;
                
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-16
        /// 작성목적 : 웹하드 공용폴더 트리 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetTreeU(int _companyID, int _userID)
        {
            DAL.Folder dalFolder = new DAL.Folder();
            DataSet ds = dalFolder.GetTreeU(_companyID, _userID);

            if (ds.Tables[0].Rows.Count == 0)
            {
                FolderInfo info = new FolderInfo();
                info.CompanyID = _companyID;
                info.UserID = 0;
                info.ParentFolderID = 0;
                info.NameBase = "ROOT";
                info.NameKo = "ROOT";
                info.NameEn = "ROOT";
                info.NameJa = "ROOT";
                info.NameFr = "ROOT";
                info.NameZhcn = "ROOT";
                info.NameZhtw = "ROOT";

                if (dalFolder.Add(info) > 0)
                {
                    ds = dalFolder.GetTreeU(_companyID, _userID);
                }
            }

            return ds;

        }

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 폴더 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _folderID)
        {
            try
            {                
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //1. Folder DB삭제 
                    DAL.Folder dal = new DAL.Folder();
                    bool response = dal.Del(_companyID, _folderID);
                    
                    //2. 첨부파일 실제 파일 삭제하기
                    if (response)
                    {
                        string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        filePath = filePath + "{0}/Users/{1}/Webhard/{2}/";
                        string webHardPath = string.Format(filePath, _companyID, _userID, _folderID);

                        Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(webHardPath, false);
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }                

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-12
        /// 작성목적 : 공용폴더 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_folderID"></param>
        /// <returns></returns>
        public bool DelPublic(int _companyID, int _userID, int _folderID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //1. Folder DB삭제 
                    DAL.Folder dal = new DAL.Folder();
                    bool response = dal.Del(_companyID, _folderID);

                    //2. 첨부파일 실제 파일 삭제하기
                    if (response)
                    {
                        string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        filePath = filePath + "{0}/WebHard/{1}/";
                        string webHardPath = string.Format(filePath, _companyID, _folderID);

                        Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(webHardPath, false);
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
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
