using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class AttachInfo
    {
        public AttachInfo(
            int _companyID, int _scheduleID, int _attachID, string _filePath
            , string _fileName, string _fileType, long _fileLength, string _fileGUID
            )
        {
            this.CompanyID = _companyID;
            this.ScheduleID = _scheduleID;
            this.AttachID = _attachID;
            this.FilePath = _filePath;
            this.FileName = _fileName;
            this.FileType = _fileType;
            this.FileLength = _fileLength;
            this.FileGUID = _fileGUID;
        }

        public AttachInfo()
        {
        }



        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public int AttachID { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public long FileLength { get; set; }

        public string FileGUID { get; set; }

        public string FileDel { get; set; }

    }
}
