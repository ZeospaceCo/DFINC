using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class OpenUserCommentInfo
    {
        public OpenUserCommentInfo()
        {
        }

        public OpenUserCommentInfo(
        int _companyID, int _userID, int _openUserID, int _groupID, DateTime _writeDate,string _Name)
        {
            this.CompanyID = _companyID;
            this.UserID = _userID;
            this.OpenUserID = _openUserID;
            this.GroupID = _groupID;
            this.WriteDate = _writeDate;
            this.Name = _Name;
        }


            public int ParentDeptID { get; set; }

            public int DeptID { get; set; }

            public string kind { get; set; }


            // DP_PIMS_OpenUser
            ///<summary>
            ///CompanyID
            ///</summary>
            public 	int	CompanyID	{ get; set; } 

            ///<summary>
            ///UserID
            ///</summary>
            public 	int	UserID	{ get; set; } 

            ///<summary>
            ///OpenUserID
            ///</summary>
            public 	int	OpenUserID	{ get; set; } 

            ///<summary>
            ///GroupID
            ///</summary>
            public 	int	GroupID	{ get; set; } 

            ///<summary>
            ///WriteDate
            ///</summary>
            public 	DateTime	WriteDate	{ get; set; }


            public string Name { get; set; } 



            //부서명 
            public string DeptName { get; set; } 
            //직급
            public string TitleName { get; set; } 
            //직책
            public string DutyName { get; set; } 
            
    }
}
