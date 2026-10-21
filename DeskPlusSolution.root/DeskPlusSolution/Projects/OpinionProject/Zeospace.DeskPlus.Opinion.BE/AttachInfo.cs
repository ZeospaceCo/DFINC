using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Opinion.BE
{
    [Serializable]
    public class AttachInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public AttachInfo()
        {
        }

        #region 필드
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>
        /// The company ID.
        /// </value>
        public int CompanyID { get; set; }

        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        /// <value>
        /// The category ID.
        /// </value>
        public int QuesID { get; set; }

        /// <summary>
        /// Gets or sets the attach file ID.
        /// </summary>
        /// <value>
        /// The attach file ID.
        /// </value>
        public int AttachID { get; set; }
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        /// FileName
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the length of the file.
        /// </summary>
        /// <value>
        /// The length of the file.
        /// </value>
        public long FileLength { get; set; }

        /// <summary>
        /// Gets or sets the file GUID.
        /// </summary>
        /// <value>
        /// The file GUID.
        /// </value>
        public string FileGUID { get; set; }
        #endregion

        #region 그외 기타
        public string FileLengthKB { get; set; }
        public string FileDel { get; set; }
        #endregion
    }
}
