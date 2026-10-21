using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BLL
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
                string createPDF = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "IsCreatePDF");
                if (createPDF.Equals("0"))
                {
                    Zeospace.DeskPlusMobile.Web.Common.Ws.Data.CreatePDF cDall = new Zeospace.DeskPlusMobile.Web.Common.Ws.Data.CreatePDF();
                    //pdf로그
                    cDall.Add(_companyID, "APPROVAL", _documentID, "N", "", "", "", _userID);
                    return true;
                }

                //1. 첨부파일 리스트 가져오기
                Data.AttachFile dal = new Data.AttachFile();
                List<BE.AttachInfo> list = dal.GetAttachFileListsByPath(_companyID, _documentID);

                //첨부파일이 존재할 경우에만 PDF파일생성
                if (list.Count > 0)
                {
                    //2. Approval 첨부파일을 PDFInfo형식으로 변환
                    List<Zeospace.DeskPlusMobile.Web.Common.Ws.BE.PDFInfo> listPDF = new List<Zeospace.DeskPlusMobile.Web.Common.Ws.BE.PDFInfo>();
                    foreach (BE.AttachInfo Attachinfo in list)
                    {
                        Zeospace.DeskPlusMobile.Web.Common.Ws.BE.PDFInfo info = new Zeospace.DeskPlusMobile.Web.Common.Ws.BE.PDFInfo();
                        info.CompanyID = Attachinfo.CompanyID;
                        info.FileGUID = Attachinfo.FileGUID;
                        info.FilePath = Attachinfo.FilePath.Replace("/DeskPlusEIP", "/DeskPlusMobile");
                        info.FileType = Attachinfo.FileType;
                        info.ListID = Attachinfo.DocumentID;
                        listPDF.Add(info);
                    }

                    //3. PDF파일 생성
                    Zeospace.DeskPlusMobile.Web.Common.Ws.BLL.CreatePDF bll = new Zeospace.DeskPlusMobile.Web.Common.Ws.BLL.CreatePDF();
                    bll.PDFProcess("APPROVAL", _documentID, _number2, _isCreate, _companyID, _userID, listPDF);
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ProcessPDF : ", ex);
                throw;
            }

        }
    }
}