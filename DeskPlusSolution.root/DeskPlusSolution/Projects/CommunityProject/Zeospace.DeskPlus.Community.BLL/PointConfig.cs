using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Community.BE;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class PointConfig : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 포인트 정보 추가 (글쓰기, 조회, 추천, 짧인 답글..세팅)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <returns></returns>
        public bool Add(ComInfo _info, int _communityID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DataView dvPoint = AppSettingHelper.GetSysCode(_info.CompanyID, "CMNPoint");

                    for (int i = 0; i < dvPoint.Count; i++)
                    {
                        if (dvPoint[i]["ActiveCode"].ToString() == "1")
                        {
                            BE.PointConfigInfo info = new BE.PointConfigInfo();
                            info.CompanyID = _info.CompanyID;
                            info.CommunityID = _communityID;
                            info.Type = dvPoint[i]["InterfaceCode"].ToString().ToNumber<int>();
                            info.Point = 1;
                            info.NameBase = dvPoint[i]["NameBase"].ToString();
                            info.NameKo = dvPoint[i]["NameKo"].ToString();
                            info.NameEn = dvPoint[i]["NameEn"].ToString();
                            info.NameJa = dvPoint[i]["NameJa"].ToString();
                            info.NameFr = dvPoint[i]["NameFr"].ToString();
                            info.NameZhcn = dvPoint[i]["NameZhcn"].ToString();
                            info.NameZhtw = dvPoint[i]["NameZhtw"].ToString();
                            info.WriteUserID = _info.WriteUserID;
                            info.WriteUserName = _info.WriteUserName;

                            DAL.PointConfig dal = new DAL.PointConfig();
                            if(!dal.Add(info))
                            {
                                return false;
                            }                           
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return false;
            }
        }

        #endregion 
    }
}
