using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class MBOShareDeptInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MBOShareDeptInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mBOID">The _m BOID.</param>
        /// <param name="_typeTargetNum">The _type target num.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        public MBOShareDeptInfo(
            int	_companyID ,int	_mBOID ,int	_typeTargetNum ,int	_deptID ,string	_deptName ) 
        {
	         this.CompanyID	=	_companyID;
	         this.MBOID	=	_mBOID;
	         this.TypeTargetNum	=	_typeTargetNum;
	         this.DeptID	=	_deptID;
	         this.DeptName	=	_deptName;
        }

        public MBOShareDeptInfo()
        {
        }


        public int CompanyID { get; set; }
        public int MBOID { get; set; }
        public int TypeTargetNum { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
    }
}
