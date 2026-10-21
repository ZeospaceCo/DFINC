using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class WebFax
    {
        private string FaxFileData = "/DeskPlusEIP/FaxFileData/SendData/";

        /// <summary>
        /// 결재
        /// </summary>
        /// <param name="_fileDirPath"></param>
        /// <returns></returns>
        public FaxInfo GetAppDoc(int _companyID, int _documentID, string _guid, string _fileDirPath)
        {
            HttpContext context = HttpContext.Current;

            FaxInfo faxInfo = new FaxInfo();
            List<string> fileNameList = new List<string>();

            Zeospace.DeskPlus.Approval.DAL.Document docDal = new Approval.DAL.Document();
            string docHtml = docDal.GetContentHtml(_companyID, _documentID);

            using (FileStream fs = new FileStream(_fileDirPath, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.GetEncoding("EUC-KR")))
                {
                    w.WriteLine(docHtml);
                }
            }

            //결재 본문 1개
            faxInfo.FileCount = 1;

            File.Copy(_fileDirPath, context.Server.MapPath(FaxFileData + _guid + ".htm"), true);
            fileNameList.Add(_guid + ".htm");

            //결재 첨부파일
            Zeospace.DeskPlus.Approval.DAL.AttachFile attDoc = new Approval.DAL.AttachFile();
            var attDocList = attDoc.GetAttachFileLists(_companyID, _documentID);

            foreach (var att in attDocList)
            {
                File.Copy(context.Server.MapPath(att.FilePath + att.FileGUID), context.Server.MapPath(FaxFileData + att.FileGUID.Replace("-", "")), true);
                fileNameList.Add(att.FileGUID.Replace("-", ""));
                faxInfo.FileCount += 1;
            }

            faxInfo.FileNameList = fileNameList;

            return faxInfo;
        }

        /// <summary>
        /// 회람 결재
        /// </summary>
        /// <param name="_fileDirPath"></param>
        /// <returns></returns>
        public FaxInfo GetAppDocCir(int _companyID, int _cirDocumentID, string _guid, string _fileDirPath)
        {
            HttpContext context = HttpContext.Current;

            FaxInfo faxInfo = new FaxInfo();
            List<string> fileNameList = new List<string>();

            Zeospace.DeskPlus.Approval.DAL.CirculationDocument docDal = new Approval.DAL.CirculationDocument();
            string docHtml = docDal.GetContentHtml(_companyID, _cirDocumentID);

            using (FileStream fs = new FileStream(_fileDirPath, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.GetEncoding("EUC-KR")))
                {
                    w.WriteLine(docHtml);
                }
            }

            //결재 본문 1개
            faxInfo.FileCount = 1;
            string targetPath = "/DeskPlusEIP/FaxFileData/";
            File.Copy(_fileDirPath, context.Server.MapPath(targetPath + _guid + ".htm"), true);
            fileNameList.Add(_guid + ".htm");

            //결재 첨부파일
            Zeospace.DeskPlus.Approval.DAL.CirculationAttachFile attDoc = new Approval.DAL.CirculationAttachFile();
            var attDocList = attDoc.GetAttachFileLists(_companyID, _cirDocumentID);

            foreach (var att in attDocList)
            {
                File.Copy(context.Server.MapPath(att.FilePath + att.FileGUID), context.Server.MapPath(targetPath + att.FileGUID.Replace("-", "")), true);
                fileNameList.Add(att.FileGUID.Replace("-", ""));
                faxInfo.FileCount += 1;
            }

            faxInfo.FileNameList = fileNameList;

            return faxInfo;
        }

        /// <summary>
        /// 수신자정보 할당하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_faxUserList"></param>
        /// <returns></returns>
        public List<FaxRecvInfo> GetRecvList(string _faxUserList)
        {
            if (_faxUserList.Length == 0) return null;

            List<FaxRecvInfo> alRecvList = new List<FaxRecvInfo>();

            string faxNum = "";
            //comName + "^" + userName + "^" + faxNum
            foreach (string userList in _faxUserList.Split('$'))
            {
                faxNum = userList.Split('^')[2];
                //TODO 특수문자 제거
                faxNum = System.Text.RegularExpressions.Regex.Replace(faxNum.Trim(), @"[-~.,)(+]{1}", "");
                //TODO 글자 사이 공백 제거
                faxNum = System.Text.RegularExpressions.Regex.Replace(faxNum.Trim(), " ", "");

                FaxRecvInfo faxRecvInfo = new FaxRecvInfo();
                faxRecvInfo.RecvComp = userList.Split('^')[0]; //회사명
                faxRecvInfo.RecvName = userList.Split('^')[1]; //수신자
                faxRecvInfo.RecvFax = faxNum; //팩스번호 (필수)
                faxRecvInfo.RecvMobile = "";//수신자 전화번호
                faxRecvInfo.Key2 = System.Guid.NewGuid().ToString().Replace("-", "");//GUID(필수)

                alRecvList.Add(faxRecvInfo);
            }

            return alRecvList;
        }

        /// <summary>
        /// 첨부파일 복사 : FROM SPOOLFOLDER TO FAXFOLDER
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_spoolPath"></param>
        /// <param name="_targetPath"></param>
        /// <returns></returns>
        public ArrayList CopyFaxFile(string _spoolPath, string _targetPath)
        {
            HttpContext context = HttpContext.Current;

            ArrayList fileList = new ArrayList();

            string spoolPath = context.Server.MapPath(_spoolPath);
            string targetPath = _targetPath;
            string targetWebPath = targetPath + "/";
            if (targetPath.IndexOf("/") > -1) targetPath = context.Server.MapPath(targetPath);

            if (Directory.Exists(spoolPath))
            {
                DirectoryInfo newDir = Directory.CreateDirectory(targetPath);

                string[] fileArr = Directory.GetFiles(spoolPath);
                string fileName = "";
                string tmpFile = "";
                string sourceFile = "";
                string destinationFile = "";
                string fileGUID = "";
                int tmpfileIdx = -1;

                UploadFileInfo upFileInfo;
                for (int i = 0; i < fileArr.Length; i++)
                {
                    tmpFile = fileArr[i];
                    tmpfileIdx = tmpFile.LastIndexOf("\\");
                    fileName = tmpFile.Substring(tmpfileIdx, tmpFile.Length - tmpfileIdx);

                    sourceFile = spoolPath + fileName;
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);

                    //파일명은 GUID 이며, 확장자는 원본과 동일하다.(.ZEO 사용안함)
                    fileGUID = System.Guid.NewGuid().ToString().Replace("-", "") + sourceFileInfo.Extension;
                    //destinationFile = NewFileName(targetPath, fileName);
                    if (!targetPath.EndsWith(@"\")) targetPath += @"\";
                    destinationFile = targetPath + fileGUID;//특수문자 제거

                    File.Copy(sourceFile, destinationFile, true);

                    upFileInfo = new UploadFileInfo();
                    upFileInfo.fileLength = sourceFileInfo.Length;
                    upFileInfo.fileName = sourceFileInfo.Name;
                    upFileInfo.filePath = targetWebPath;
                    upFileInfo.fileType = sourceFileInfo.Extension;
                    upFileInfo.fileGUID = fileGUID;
                    upFileInfo.fileLastWriteTime = sourceFileInfo.CreationTime;

                    fileList.Add(upFileInfo);
                }
            }

            return fileList;
        }
    }
}