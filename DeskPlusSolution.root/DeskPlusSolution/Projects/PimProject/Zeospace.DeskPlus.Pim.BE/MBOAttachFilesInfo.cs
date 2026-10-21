using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class MBOAttachFilesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MBOAttachFilesInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mBOID">The _m BOID.</param>
        /// <param name="_attachFileID">The _attach file ID.</param>
        /// <param name="_filePath">The _file path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        /// <param name="_fileType">Type of the _file.</param>
        /// <param name="_fileLength">Length of the _file.</param>
        /// <param name="_fileGUID">The _file GUID.</param>
        public MBOAttachFilesInfo(
            int	_companyID ,int	_mBOID ,int	_attachFileID ,string	_filePath 
            ,string	_fileName ,string	_fileType ,long	_fileLength ,string	_fileGUID ) 
        {
	         this.CompanyID	=	_companyID;
	         this.MBOID	=	_mBOID;
	         this.AttachFileID	=	_attachFileID;
	         this.FilePath	=	_filePath;
	         this.FileName	=	_fileName;
	         this.FileType	=	_fileType;
	         this.FileLength	=	_fileLength;
	         this.FileGUID	=	_fileGUID;
        }

        public MBOAttachFilesInfo()
        { }


        public int CompanyID { get; set; }
        public int MBOID { get; set; }
        public int AttachFileID { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileLength { get; set; }
        public string FileGUID { get; set; }
    }
}
