using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class Project : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public int Add(ProjectInfo _listInfo, List<XmlDataInfo> _al)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Project dal = new DAL.Project();
                    int projectID = dal.Add(_listInfo);

                    #region 1. 프로젝트 사용자 추가하기

                    BE.ProjectUserInfo userInfo = new ProjectUserInfo();
                    userInfo.CompanyID = _listInfo.CompanyID;
                    userInfo.ProjectID = projectID;
                    userInfo.ProjectUserID = 0;
                    userInfo.UserID = _listInfo.WriteUserID;
                    userInfo.Status = 1;
                    userInfo.RegisterDate = System.DateTime.Now;
                    userInfo.VisitedDate = System.DateTime.MaxValue;
                    userInfo.WriteUserID = _listInfo.WriteUserID;

                    DAL.ProjectUser bll = new DAL.ProjectUser();
                    bll.Add(userInfo);

                    #endregion 1. 프로젝트 사용자 추가하기

                    #region 2. 프로젝트 메뉴 추가하기

                    for (int i = 1; i < 7; i++)
                    {
                        BE.MenuInfo menuInfo = new MenuInfo();
                        menuInfo.CompanyID = _listInfo.CompanyID;
                        menuInfo.ProjectID = projectID;
                        menuInfo.MenuID = 0;
                        menuInfo.MenuType = i;
                        menuInfo.IsUse = 1;
                        menuInfo.Orders = 0;
                        menuInfo.MenuBase = "No Title";
                        menuInfo.MenuDesc = "";

                        foreach (XmlDataInfo be in _al)
                        {
                            if (be.KeyID == i.ToString())
                            {
                                menuInfo.MenuBase = be.KeyValue;
                                break;
                            }
                        }

                        DAL.Menu menuBiz = new DAL.Menu();
                        menuBiz.Add(menuInfo);
                    }

                    #endregion 2. 프로젝트 메뉴 추가하기

                    ts.Complete();

                    return projectID;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(ProjectInfo _listInfo) : " + ex);
                //ContextUtil.SetAbort();
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Mod(ProjectInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Project dal = new DAL.Project();
                    dal.Mod(_listInfo);

                    #region 4. 프로젝트 단위 책임자 추가

                    if (_listInfo.ChargeDeptID.Length > 0)
                    {
                        string[] chargerStr = _listInfo.ChargeDeptID.Split(',');

                        DAL.ProjectCharger iCharger = new DAL.ProjectCharger();
                        iCharger.Del(_listInfo.CompanyID, _listInfo.ProjectID);

                        for (int i = 0; i < chargerStr.Length; i++)
                        {
                            BE.ProjectChargerInfo projUnitInfo2 = new ProjectChargerInfo();
                            projUnitInfo2.CompanyID = _listInfo.CompanyID;
                            projUnitInfo2.ProjectID = _listInfo.ProjectID;
                            projUnitInfo2.DeptID = 0;
                            projUnitInfo2.UserID = chargerStr[i].ToNumber<int>();
                            projUnitInfo2.WriteUserID = _listInfo.WriteUserID;
                            iCharger.Add(projUnitInfo2);
                        }
                    }

                    #endregion 4. 프로젝트 단위 책임자 추가

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);

                throw;
            }
        }

        #endregion MOD()
    }
}