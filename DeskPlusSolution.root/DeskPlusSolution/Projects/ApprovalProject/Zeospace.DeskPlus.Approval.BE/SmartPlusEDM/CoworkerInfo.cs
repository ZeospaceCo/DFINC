using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Approval.BE.SmartPlusEDM
{
    public class CoworkerInfo
    {
        #region 생성자
        public CoworkerInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoworkerInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <param name="_coworkerID">The _coworker ID.</param>
        /// <param name="_versionID">The _version ID.</param>
        /// <param name="_ownerType">Type of the _owner.</param>
        /// <param name="_ownerID">The _owner ID.</param>
        /// <param name="_rating">The _rating.</param>
        public CoworkerInfo(int _companyID, int _documentID, int _coworkerID, int _versionID, string _ownerType, int _ownerID, string _rating)
        {
            this.CompanyID = _companyID;
            this.DocumentID = _documentID;
            this.CoworkerID = _coworkerID;
            this.VersionID = _versionID;
            this.OwnerType = _ownerType;
            this.OwnerID = _ownerID;
            this.Rating = _rating;
        }
        #endregion

        #region 필드/프로퍼티 (초기화)
        /// <summary>
        /// 회사ID
        /// </summary> 
        private int companyID = 0;
        public int CompanyID { get { return companyID; } set { companyID = value; } }

        /// <summary>
        /// 문서 ID
        /// </summary>
        private int documentID = 0;
        public int DocumentID { get { return documentID; } set { documentID = value; } }

        /// <summary>
        /// 공동작업ID
        /// </summary>
        private int coworkerID = 0;
        public int CoworkerID { get { return coworkerID; } set { coworkerID = value; } }

        /// <summary>
        /// 버전 ID
        /// </summary>
        private int versionID = 0;
        public int VersionID { get { return versionID; } set { versionID = value; } }

        /// <summary>
        /// 그룹사용자구분
        /// </summary>
        private string ownerType = String.Empty;
        public string OwnerType { get { return ownerType; } set { ownerType = value; } }

        /// <summary>
        /// 그룹사용자ID
        /// </summary>
        private int ownerID = 0;
        public int OwnerID { get { return ownerID; } set { ownerID = value; } }

        /// <summary>
        /// 참여도
        /// </summary>
        private string rating = String.Empty;
        public string Rating { get { return rating; } set { rating = value; } }
        #endregion 프로퍼티
    }
}
