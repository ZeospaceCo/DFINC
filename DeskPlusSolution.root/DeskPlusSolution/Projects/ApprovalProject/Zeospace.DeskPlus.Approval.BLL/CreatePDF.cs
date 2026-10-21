using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class CreatePDF
    {
        /// <summary>
        /// PDF파일 생성
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_number2"></param>
        /// <param name="_isCreate"></param>
        /// <returns></returns>
        public bool ProcessPDF(int _companyID, int _userID, int _documentID, string _number2, string _isCreate)
        {
            try
            {
                //PDF생성유부 체크
                string createPDF = AppSettingHelper.GetAppSetting(AppSettingType.COMMON, "IsCreatePDF", "0");
                if (createPDF.Equals("0"))
                {
                    Common.DAL.CreatePDF cDall = new Common.DAL.CreatePDF();
                    //pdf로그
                    cDall.Add(_companyID, "APPROVAL", _documentID, "N", "", "", "", _userID);
                    return true;
                }

                //1. 첨부파일 리스트 가져오기
                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> list = dal.GetAttachFileListsByPath(_companyID, _documentID);

                //첨부파일이 존재할 경우에만 PDF파일생성
                if (list.Count > 0)
                {
                    //2. Approval 첨부파일을 PDFInfo형식으로 변환
                    List<Common.BE.PDFInfo> listPDF = new List<Common.BE.PDFInfo>();
                    foreach (BE.AttachFileInfo Attachinfo in list)
                    {
                        Common.BE.PDFInfo info = new Common.BE.PDFInfo();
                        info.CompanyID = Attachinfo.CompanyID;
                        info.FileGUID = Attachinfo.FileGUID;
                        info.FilePath = Attachinfo.FilePath;
                        info.FileType = Attachinfo.FileType;
                        info.ListID = Attachinfo.DocumentID;
                        listPDF.Add(info);
                    }

                    //3. PDF파일 생성
                    Common.BLL.CreatePDF bll = new Common.BLL.CreatePDF();
                    bll.PDFProcess("APPROVAL", _documentID, _number2, _isCreate, _companyID, _userID, listPDF);
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ProcessPDF : " + ex);
                throw;
            }

        }
    }
}
