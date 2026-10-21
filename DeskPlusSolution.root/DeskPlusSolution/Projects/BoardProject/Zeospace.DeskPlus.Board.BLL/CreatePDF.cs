using System;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class CreatePDF
    {
        public bool ProcessPDF(int _companyID, int _userID, int _listID, int _catID, string _yearMonth, string _isCreate)
        {
            try
            {
                //PDF생성유부 체크
                string createPDF = AppSettingHelper.GetAppSetting(AppSettingType.COMMON, "IsCreatePDF", "0");

                if (createPDF.Equals("0"))
                {
                    Common.DAL.CreatePDF cDall = new Common.DAL.CreatePDF();
                    //pdf로그
                    cDall.Add(_companyID, "BOARD", _listID, "N", "", "", "", _userID);
                    return true;
                }

                //1. 첨부파일 리스트 가져오기
                DAL.AttachFiles dal = new DAL.AttachFiles();
                //dal.GetAttachFileLists(
                List<BE.AttachFileInfo> list = dal.GetAttachFileLists(_companyID, _catID, _listID);

                //첨부파일이 존재할 경우에만 PDF파일생성
                if (list.Count > 0)
                {
                    //2. Board 첨부파일을 PDFInfo형식으로 변환
                    List<Common.BE.PDFInfo> listPDF = new List<Common.BE.PDFInfo>();
                    foreach (BE.AttachFileInfo Attachinfo in list)
                    {
                        Common.BE.PDFInfo info = new Common.BE.PDFInfo();
                        info.CompanyID = Attachinfo.CompanyID;
                        info.FileGUID = Attachinfo.FileGUID;
                        info.FilePath = Attachinfo.FilePath;
                        info.FileType = Attachinfo.FileType;
                        info.ListID = Attachinfo.ListID;
                        listPDF.Add(info);
                    }

                    //3. PDF파일 생성
                    Common.BLL.CreatePDF bll = new Common.BLL.CreatePDF();
                    bll.PDFProcess("BOARD", _listID, _yearMonth, _isCreate, _companyID, _userID, listPDF);
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