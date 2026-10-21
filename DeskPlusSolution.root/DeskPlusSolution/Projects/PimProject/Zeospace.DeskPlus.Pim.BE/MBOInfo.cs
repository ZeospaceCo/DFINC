using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class MBOInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MBOInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mBOID">The _m BOID.</param>
        /// <param name="_parentMBOID">The _parent MBOID.</param>
        /// <param name="_mBOGroup">The _m BO group.</param>
        /// <param name="_mBOOrder">The _m BO order.</param>
        /// <param name="_dept">The _dept.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_endDate">The _end date.</param>
        /// <param name="_toDoListYN">The _to do list YN.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_writeDate">The _write date.</param>
        /// <param name="_deptShared">The _dept shared.</param>
        public MBOInfo(
            int	_companyID ,int	_mBOID ,int	_parentMBOID ,int	_mBOGroup ,int	_mBOOrder 
            ,int	_dept ,string	_title ,DateTime	_startDate ,DateTime	_endDate 
            ,string	_toDoListYN ,int	_userID ,DateTime	_writeDate ,int	_deptShared )
        {
	         this.CompanyID	=	_companyID;
	         this.MBOID	=	_mBOID;
	         this.ParentMBOID	=	_parentMBOID;
	         this.MBOGroup	=	_mBOGroup;
	         this.MBOOrder	=	_mBOOrder;
	         this.Dept	=	_dept;
	         this.Title	=	_title;
	         this.StartDate	=	_startDate;
	         this.EndDate	=	_endDate;
	         this.ToDoListYN	=	_toDoListYN;
	         this.UserID	=	_userID;
	         this.WriteDate	=	_writeDate;
	         this.DeptShared	=	_deptShared;
        }


        public MBOInfo()
        {
        }

        public int DeptID { get; set; }
        public int CompanyID { get; set; }
        public int MBOID { get; set; }
        public int ParentMBOID { get; set; }
        public int MBOGroup { get; set; }
        public int MBOOrder { get; set; }
        public int Dept { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ToDoListYN { get; set; }
        public int UserID { get; set; }
        public DateTime WriteDate { get; set; }
        public int DeptShared { get; set; }

        
        public string attachYN { get; set; }

        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public string AuthYN { get; set; }

        public int GroupCnt { get; set; }
    }
}
