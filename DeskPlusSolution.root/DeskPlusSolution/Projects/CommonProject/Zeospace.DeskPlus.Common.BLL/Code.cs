using System.Data;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class Code
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 하위 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <param name="_activeCode">if set to <c>true</c> [_active code].</param>
        /// <returns></returns>
        public string GetChildList(int _companyID, int _codeID, bool _activeCode)
        {
            string list = "";

            Zeospace.DeskPlus.Common.DAL.Code biz = new DAL.Code();
            DataSet ds = biz.GetCode(_companyID, _activeCode);
            ds.Relations.Add("CodeRelation", ds.Tables[0].Columns["CodeID"], ds.Tables[0].Columns["ParentCodeID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow["CodeID"].ToString() == _codeID.ToString())
                {
                    //하위트리 만들기(datarow, treeviw)
                    GetChildID(dbRow, ref list);
                }
            }

            return list.TrimEnd(',');
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 하위 가져오는 재귀 함수
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="deptIDList">The dept ID list.</param>
        private void GetChildID(DataRow _dbRow, ref string deptIDList)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("CodeRelation"))
            {
                deptIDList += childRow["CodeID"].ToString() + ",";
                GetChildID(childRow, ref deptIDList);
            }
        }
    }
}