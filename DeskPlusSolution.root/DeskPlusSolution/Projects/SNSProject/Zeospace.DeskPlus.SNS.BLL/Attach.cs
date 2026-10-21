using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.SNS.BE;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.SNS.BLL
{
    public class Attach : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-03-12
        /// 작성목적 : 파일 등록 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_groupID"></param>
        /// <param name="_hiddenSpoolPath"></param>
        /// <returns></returns>
        public bool Add(MyTalkInfo _info, int _groupID, string _hiddenSpoolPath)
        {
            ArrayList fileList = FileHelper.GetUploadFile(_hiddenSpoolPath, "", FolderType.SNS, _groupID, false, _info.WriteDateYM);
            DAL.Attach dal = new DAL.Attach();

            if (fileList != null)
            {
                //파일 생성일자로 오름차순 (ASC) 정렬하여 저장
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));

                for (int i = 0; i < fileList.Count; i++)
                {
                    AttachInfo Info = new AttachInfo();
                    Info.CompanyID = _info.CompanyID;
                    Info.GroupID = _groupID;
                    Info.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    Info.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    Info.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    Info.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    Info.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;
                    Info.FileOrder = i + 1;

                    if (dal.Add(Info) < 1)
                    {
                        return false;
                    }

                    string tmpPath = FileHelper.GetServerMapPath(Info.FilePath);
                    System.Drawing.Image image = System.Drawing.Image.FromFile(tmpPath + Info.FileGUID);

                    System.Drawing.Image thumb = image.GetThumbnailImage(250, 250,
                        delegate() { return false; }, (IntPtr)0);

                    //스크린샷 만들기 파일명 앞에 Thum_ 붙이기
                    thumb.Save(tmpPath + "Thum_" + Info.FileGUID);

                    image.Dispose();


                }
            }

            return true;
        }

      

        #endregion 

        #region DEL()       
        #endregion 
    }
}
