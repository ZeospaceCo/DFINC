using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Proposal.BE;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Proposal.DAL;

namespace Zeospace.DeskPlus.Proposal.BLL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD

        public bool Add(int _companyID, int _proposalNo, string _spoolPath, string _yearMonth)
        {
            string targetPath = "/DeskPlusEIP/FileData/" + _companyID + "/Proposal/" + _yearMonth + "/" + _proposalNo;

            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, targetPath, FolderType.Proposal, _proposalNo, false, _yearMonth);
            DAL.Attach dal = new DAL.Attach();

            try
            {
                if (fileList != null)
                {
                    //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                    fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        BE.AttachInfo fileInfo = new AttachInfo();
                        fileInfo.CompanyID = _companyID;
                        fileInfo.ProposalNo = _proposalNo;
                        fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                        fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                        fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                        fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                        fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                        dal.Add(fileInfo);
                    }
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion

        #region MOD
        #endregion

        #region DEL

        public bool Del(int _companyID, int _proposalNo, int _attachID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;

                    DAL.Attach dalAttach = new DAL.Attach();
                    List<BE.AttachInfo> alAttach = dalAttach.GetAttachFileList(_companyID, _proposalNo, _attachID);

                    foreach (BE.AttachInfo be in alAttach)
                    {
                        dalAttach.Del(be.CompanyID, be.ProposalNo, be.AttachID);
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
