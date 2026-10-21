using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class ToDo : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region basic
        public ToDo()
        {
        }

        #endregion

        #region ADD
        public int AddTodo(BE.ToDoInfo info)
        {
            if (info == null) return -1;
            int iRt = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.ToDo dal = new DAL.ToDo();

                iRt = dal.TodoAdd(info);

                ts.Complete();
            }

            return iRt;
        }

        #endregion

        #region mod
        public void ModTodo(BE.ToDoInfo info)
        {


            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.ToDo dal = new DAL.ToDo();

                dal.ModTodo(info);

                ts.Complete();
            }

        }
        #endregion

        #region del

      
        #endregion

        


    }
}
