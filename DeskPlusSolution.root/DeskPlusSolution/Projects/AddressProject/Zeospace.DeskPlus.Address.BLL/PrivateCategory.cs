using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Address.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.BLL
{
    public class PrivateCategory
    {
        #region GET() 

        /// <summary>
        /// 작성자 : 심재운
        /// 하위 카테고리 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public string GetChildList(int _companyID, int _categoryID, int _userID)
        {
            string catIDList = "";
            DAL.PrivateCategory biz = new DAL.PrivateCategory();
            DataSet ds = biz.GetPrivateCategory(_companyID, _userID);
            ds.Relations.Add("PrivateRelation", ds.Tables[0].Columns["CategoryID"], ds.Tables[0].Columns["ParentCategoryID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow["CategoryID"].ToString() == _categoryID.ToString())
                {
                    //하위트리 만들기(datarow, treeviw)
                    GetChildID(dbRow, ref catIDList);
                }
            }

            return catIDList.TrimEnd(',');
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 하위 카테고리 가져오는 재귀 함수
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="deptIDList">The dept ID list.</param>
        private void GetChildID(DataRow _dbRow, ref string deptIDList)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("PrivateRelation"))
            {
                deptIDList += childRow["CategoryID"].ToString() + ",";
                GetChildID(childRow, ref deptIDList);
            }
        }

        #endregion 
 
        #region ADD() 

        
        #endregion 
 
        #region MOD() 
        
        #endregion 
 
        #region DEL() 
        
        #endregion     
    }
}
