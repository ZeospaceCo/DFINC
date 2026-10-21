using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Pim.BE
{
    [Serializable]
    public class ToDoShareUserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoShareUserInfo"/> class.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_toDoListID">The _to do list ID.</param>
        /// <param name="_typeTargetNum">The _type target num.</param>
        /// <param name="_userName">Name of the _user.</param>
        /// <param name="_userMail">The _user mail.</param>
        public ToDoShareUserInfo(
            int	_companyID ,int	_toDoListID ,int	_typeTargetNum 
            ,string	_userName ,string	_userMail )
        {
	         this.CompanyID	=	_companyID;
	         this.ToDoListID	=	_toDoListID;
	         this.TypeTargetNum	=	_typeTargetNum;
	         this.UserName	=	_userName;
	         this.UserMail	=	_userMail;
        } 

        public int CompanyID { get; set; }
        public int ToDoListID { get; set; }
        public int TypeTargetNum { get; set; }
        public string UserName { get; set; }
        public string UserMail { get; set; }
    }
}
