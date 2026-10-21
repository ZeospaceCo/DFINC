using System.Collections;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.BLL
{
    public class Dept
    {
        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2010-11-15
        /// 작성목적 : 그룹사 ID 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public int GetRootDeptID(int _companyID, int _deptID, bool _activeDept)
        {
            if (_deptID < 0) return -1;
            DAL.Dept biz = new DAL.Dept();
            DataSet ds = biz.GetDept(_companyID, "dsDept", _activeDept);
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
        /// 작성자   : Freeman
        /// 작성일자 : 2010-11-16
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
                //if(parentRow.IsNull("ParentDeptID"))
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

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-25
        /// 작성목적 : 부모 DeptID List 가져오기 (1, 2, 3, 4, 5...)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetUpDeptIDInfo(int _companyID, int _deptID, bool _activeDept)
        {
            string deptIDList = "";

            DAL.Dept biz = new DAL.Dept();
            var alList = biz.GetDeptLists(_companyID, _deptID, _activeDept);

            foreach (var info in alList)
            {
                if (info.PathID.Length > 0) deptIDList = info.PathID.Substring(1);
            }

            return deptIDList;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 하위부서 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetChildDeptList(int _companyID, int _deptID, bool _activeDept)
        {
            string deptIDList = "";

            DAL.Dept biz = new DAL.Dept();
            var alList = biz.GetDeptLists(_companyID, _deptID, _activeDept);

            foreach (var info in alList)
            {
                deptIDList = info.ChildDept;
            }

            return deptIDList;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 나의 최상위 부서들 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-05
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetTopRootDeptID(int _companyID, int _deptID, bool _activeDept)
        {
            string topRootDeptIDs = "";
            ArrayList arr = new ArrayList();

            DAL.Dept biz = new DAL.Dept();
            DataSet ds = biz.GetDept(_companyID, "dsDept", _activeDept);
            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);

            DataRow[] dbRow = ds.Tables[0].Select("DeptID =" + _deptID);

            arr = GetTopRootDeptID(dbRow[0], ref arr);

            if (arr.Count > 0)
            {
                arr.Reverse();

                for (int i = 0; i < arr.Count; i++)
                {
                    topRootDeptIDs += arr[i].ToString() + ",";
                }
            }

            List<BE.DeptInfo> deptList = biz.GetDeptLists(_companyID, _deptID, _activeDept);

            foreach (BE.DeptInfo be in deptList)
            {
                //1이면 최상위노드를 선택했다는 의미
                if (be.RootDept == "1")
                {
                    topRootDeptIDs += _deptID + ",";
                }
            }

            return topRootDeptIDs.TrimEnd(',');
        }

        /// <summary>
        /// Gets the top root dept ID.
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="_arr">The _arr.</param>
        /// <returns></returns>
        private ArrayList GetTopRootDeptID(DataRow _dbRow, ref ArrayList _arr)
        {
            //MenuRelation의 관계에 datarow 의 부모행을 가져오자
            DataRow parentRow = _dbRow.GetParentRow("DeptRelation");

            if (parentRow != null)
            {
                //if(parentRow.IsNull("ParentDeptID"))
                if (parentRow["RootDept"].ToString() == "1")
                {
                    _arr.Add(parentRow["DeptID"]);

                    //return Convert.ToInt32(parentRow["DeptID"]);
                    return GetTopRootDeptID(parentRow, ref _arr);
                }
                else
                {
                    return GetTopRootDeptID(parentRow, ref _arr);
                }
            }
            else
            {
                return _arr;
            }
        }
    }
}