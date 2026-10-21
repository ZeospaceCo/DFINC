using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Account.BE;
using System.Collections;
using System.Data;


namespace Zeospace.DeskPlusMobile.Web.Account.BLL
{
    public class User
    {
        /// <summary>
        /// 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public UserInfo GetUser(int _companyID, int _userID)
        {
            if (_companyID < -1 || _userID < 0)
                return null;

            Zeospace.DeskPlusMobile.Web.Account.Data.User iUser = new Zeospace.DeskPlusMobile.Web.Account.Data.User();
            UserInfo userInfo = iUser.GetUser(_companyID, _userID);

            userInfo.UserDeptList = GetUserDeptArr(_companyID, _userID);

            return userInfo;
        }

        /// <summary>
        /// 사용자 부서 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public ArrayList GetUserDeptArr(int _companyID, int _userID)
        {
            Zeospace.DeskPlusMobile.Web.Account.Data.UserDept iUserDept = new Zeospace.DeskPlusMobile.Web.Account.Data.UserDept();
            return iUserDept.GetUserDept(_companyID, _userID);
        }

        /// <summary>
        /// 해당 부서로 하위부서 가져와서 사용자 추출해 오기(구분자 콤마)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public string GetChildDeptUserList(int _companyID, int _deptID)
        {
            BLL.Dept biz = new Dept();
            string deptList = biz.GetChildDeptList(_companyID, _deptID);
            deptList = (deptList.Length == 0) ? _deptID.ToString() : deptList;

            Data.UserDept dal = new Data.UserDept();
            DataSet ds = dal.GetUserListByDept(_companyID, deptList);

            string userIDList = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userIDList += dr["UserID"].ToString() + ",";
                }
            }

            userIDList = userIDList.TrimEnd(',');

            return userIDList;
        }
    }
}