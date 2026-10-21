using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Address.BE
{
    [Serializable]
    public class AttachInfo
    {        
        public AttachInfo(
        int	_companyID ,int	_ownerID ,int	_attachFileID ,string	_gubun ,string	_filePath 
        ,string	_fileName ,string	_fileType ,long	_fileLength ,string	_fileGUID )  
         {
	         this.CompanyID	=	_companyID;
	         this.OwnerID	=	_ownerID;
	         this.AttachFileID	=	_attachFileID;
	         this.Gubun	=	_gubun;
	         this.FilePath	=	_filePath;
	         this.FileName	=	_fileName;
	         this.FileType	=	_fileType;
	         this.FileLength	=	_fileLength;
	         this.FileGUID	=	_fileGUID;

        } 

        public int CompanyID { get; set; }
        public int OwnerID { get; set; }
        public int AttachFileID { get; set; }
        public string Gubun { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }

    }
}
