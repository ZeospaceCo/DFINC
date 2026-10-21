using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.BLL
{
    public class Dept
    {
        /// <summary>
        /// 조직도 위,아래 재귀값 가져오기 (1: UP, 2:DOWN)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_upDown">The _up down.</param>
        /// <returns></returns>
        public DataSet GetDeptTree(int _companyID, int _userID, int _upDown)
        {
            Data.Dept data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Dept();
            return data.GetDeptTree(_companyID, _userID, _upDown);
        }

        /// <summary>
        /// 부모 DeptID List 가져오기 (1, 2, 3, 4, 5...)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public string GetParentDeptID(int _companyID, int _deptID)
        {
            string deptIDList = "";
            Board.Ws.BLL.Dept dept = new Dept();
            DataSet ds = dept.GetDeptTree(_companyID);
            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                //하위메뉴가 없을 경우
                if (dbRow["deptID"].ToString() == _deptID.ToString())
                {
                    //하위트리 만들기(datarow, treeviw)
                    GetParentDeptID(dbRow, ref deptIDList);
                }
            }

            return deptIDList.TrimEnd(',');
        }

        /// <summary>
        /// Gets the parent dept ID.
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="deptIDList">The dept ID list.</param>
        private void GetParentDeptID(DataRow _dbRow, ref string deptIDList)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow parentRow in _dbRow.GetParentRows("DeptRelation"))
            {
                deptIDList += parentRow["deptID"].ToString() + ",";
                //재귀함수 호출~
                GetParentDeptID(parentRow, ref deptIDList);
            }
        }


        /// <summary>
        /// 조직도 (사용자 + 부서) DataSet생성
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetDeptUserTree(int _companyID)
        {
            Data.Dept data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Dept();
            return data.GetDeptUserTree(_companyID);
        }

        /// <summary>
        /// Gets the dept tree.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetDeptTree(int _companyID)
        {
            Data.Dept dept = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Dept();
            return dept.GetDeptTree(_companyID);
        }
    }
}