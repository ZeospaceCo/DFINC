using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.BLL
{
    public class FolderAuth : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 특정 폴더의 접속 가능자에게 알람 발송
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_folderID">The _folder identifier.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public string GetFolderAuthUser(int _companyID, int _folderID, bool _activeDept)
        {
            DAL.FolderAuth auth = new DAL.FolderAuth();
            DataTableReader dtr = auth.GetFolderAuth(_companyID, _folderID);
            string sendUserIDs = string.Empty;

            if (dtr != null)
            {
                string type = string.Empty;
                int ownerID;

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        type = dtr["Gubun"].ToString();
                        ownerID = dtr["OwnerID"].ToString().ToNumber<int>();

                        //회사별
                        if (type == "G" && ownerID == 0)
                        {
                            sendUserIDs = "all";
                            break;
                        }
                        else if (type == "G" && ownerID > 0) //회사별
                        {
                            Account.BLL.User bll = new Account.BLL.User();
                            string deptUserList = bll.GetChildDeptUserList(_companyID, ownerID, _activeDept);
                            sendUserIDs += (deptUserList.Length > 0) ? deptUserList + "," : deptUserList;
                        }
                        else if (type == "M" && ownerID > 0) //사용자별
                        {
                            sendUserIDs += ownerID.ToString() + ",";
                        }
                    }
                }
            }

            return sendUserIDs;
        }
    }
}