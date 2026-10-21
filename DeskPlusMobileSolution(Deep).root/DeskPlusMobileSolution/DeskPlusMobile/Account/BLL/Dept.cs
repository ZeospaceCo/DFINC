using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Account.BLL
{
    public class Dept
    {
        /// <summary>
        /// 하위부서 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetChildDeptList(int _companyID, int _deptID)
        {
            string deptIDList = "";

            Data.Dept biz = new Data.Dept();
            DataSet ds = biz.GetDept(_companyID, _deptID);

            if (ds != null)
            {
                foreach (DataRow drDept in ds.Tables[0].Rows)
                {
                    deptIDList = drDept["ChildDept"].ToString();
                }
            }


            return deptIDList;
        }

        /// <summary>
        /// 작성목적 : 그룹사 ID 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_activeDept"></param>
        /// <returns></returns>
        public int GetRootDeptID(int _companyID, int _deptID, bool _activeDept)
        {
            if (_deptID < 0) return -1;
            Data.Dept biz = new Data.Dept();
            DataSet ds = biz.GetDept(_companyID, _activeDept);
            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);
            DataRow[] dbRow = ds.Tables[0].Select("DeptID =" + _deptID);

            if (dbRow != null)
            {
                if (dbRow[0]["RootDept"].ToString() == "1")
                {
                    return dbRow[0]["DeptID"].ToString().ToNumber<int>();
                }
                else
                {
                    return GetParentDeptID(dbRow[0]);
                }
            }

            return -1;
        }

        /// <summary>
        /// 작성목적 : 부모 ID 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <returns></returns>
        private int GetParentDeptID(DataRow _dbRow)
        {
            //MenuRelation의 관계에 datarow 의 부모행을 가져오자
            DataRow parentRow = _dbRow.GetParentRow("DeptRelation");

            if (parentRow != null)
            {
                if (parentRow["RootDept"].ToString() == "1")
                {
                    return parentRow["DeptID"].ToString().ToNumber<int>();
                }
                else
                {
                    return GetParentDeptID(parentRow);
                }
            }
            else
            {
                return _dbRow["DeptID"].ToString().ToNumber<int>();
            }
        }
    }
}