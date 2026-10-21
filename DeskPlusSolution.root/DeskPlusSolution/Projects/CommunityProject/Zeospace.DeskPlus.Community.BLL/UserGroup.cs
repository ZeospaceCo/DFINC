using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class UserGroup : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 유저그룹 추가 (커뮤니티지기, 운영자..세팅)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <returns></returns>
        public int Add(int _companyID, int _communityID)
        {
            try
            {
                int iUserGroupAdmin = -1;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DataView dvGroup = AppSettingHelper.GetSysCode(_companyID, "CMNGroup");
                    
                    for (int i = 0; i < dvGroup.Count; i++)
                    {
                        if (dvGroup[i]["ActiveCode"].ToString() == "1")
                        {
                            BE.UserGroupInfo info = new BE.UserGroupInfo();
                            info.CompanyID = _companyID;
                            info.CommunityID = _communityID;
                            info.Grade = dvGroup[i]["InterfaceCode"].ToString().ToNumber<int>();
                            info.NameBase = dvGroup[i]["NameBase"].ToString();
                            info.NameKo = dvGroup[i]["NameKo"].ToString();
                            info.NameEn = dvGroup[i]["NameEn"].ToString();
                            info.NameJa = dvGroup[i]["NameJa"].ToString();
                            info.NameFr = dvGroup[i]["NameFr"].ToString();
                            info.NameZhcn = dvGroup[i]["NameZhcn"].ToString();
                            info.NameZhtw = dvGroup[i]["NameZhtw"].ToString();

                            DAL.UserGroup dal = new DAL.UserGroup();

                            int rtnGroup = dal.Add(info);

                            if (rtnGroup < 1)
                            {
                                return -1;
                            }

                            if (info.Grade == 2)
                            {
                                iUserGroupAdmin = rtnGroup;
                            }
                        }
                    }

                    ts.Complete();
                }

                return iUserGroupAdmin;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return -1;
            }
        }

        #endregion 
    }
}
