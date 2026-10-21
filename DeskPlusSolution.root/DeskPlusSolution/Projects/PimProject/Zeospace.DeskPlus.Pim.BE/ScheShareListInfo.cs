using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    public class ScheShareListInfo
    {

        public ScheShareListInfo()
        {
        }

        public ScheShareListInfo(
        int _companyID, int _scheduleID, int _scheShareListID, string _userName, string _userMail
            , int _userID, int _deptID)
        {
            this.CompanyID = _companyID;
            this.ScheduleID = _scheduleID;
            this.ScheShareListID = _scheShareListID;
            this.UserName = _userName;
            this.UserMail = _userMail;
            this.UserID = _userID;
            this.DeptID = _deptID;
            //this.ShareDeptID = _ShareDeptID;

        }

            // DP_PIM_ScheShareList
            ///<summary>
            ///CompanyID
            ///</summary>
            public 	int	CompanyID	{ get; set; } 

            ///<summary>
            ///ScheduleID
            ///</summary>
            public 	int	ScheduleID	{ get; set; } 

            ///<summary>
            ///ScheShareListID
            ///</summary>
            public 	int	ScheShareListID	{ get; set; } 

            ///<summary>
            ///UserName
            ///</summary>
            public 	string	UserName	{ get; set; } 

            ///<summary>
            ///UserMail
            ///</summary>
            public 	string	UserMail	{ get; set; } 

            ///<summary>
            ///UserID
            ///</summary>
            public 	int	UserID	{ get; set; } 

            ///<summary>
            ///DeptID
            ///</summary>
            public 	int	DeptID	{ get; set; }


          
    }
}
