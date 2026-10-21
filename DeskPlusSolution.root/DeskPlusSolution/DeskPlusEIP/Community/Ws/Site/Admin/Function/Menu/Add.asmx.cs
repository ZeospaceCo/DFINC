using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.Function.Menu
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSCommunity
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 메뉴 상세정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMenuInfo(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                DataTableReader dtr = dal.GetMenuInfo(CurrentUserInfo.CompanyID, communityID, menuID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.MenuInfo info = new BE.MenuInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                            info.Type = dtr["Type"].ToString().ToNumber<int>();
                            info.Weight = dtr["Weight"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();


                            info.MultiName = this.GetMuiltiLang(dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString()).ToReverseXSS();

                            info.WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            info.ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();

                            listobj.Add(info);

                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 메뉴 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMenuList(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();                

                DAL.Menu dal = new DAL.Menu();
                DataSet ds = dal.GetMenuList(CurrentUserInfo.CompanyID, communityID);

                List<object> listobj = new List<object>();

                if (ds != null)
                {                    
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.MenuInfo info = new BE.MenuInfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.MenuID = dr["MenuID"].ToString().ToNumber<int>();
                        info.Sequence = dr["Sequence"].ToString().ToNumber<int>();
                        info.NameBase = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                                                            dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        listobj.Add(info);

                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 메뉴 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuAdd(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int type = GetDecryptParam("type", "0").ToNumber<int>();
                string nameBase = GetDecryptParam("nameBase", "");
                string multiName = GetDecryptParam("multiName", "");
                int writeAuth = GetDecryptParam("writeAuth", "9").ToNumber<int>();
                int readAuth = GetDecryptParam("readAuth", "9").ToNumber<int>();
                int weight = GetDecryptParam("weight", "0").ToNumber<int>();

                BE.MenuInfo info = new BE.MenuInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.Type = type;
                info.Weight = weight;
                info.NameBase = nameBase;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                BE.MenuAuthInfo authInfo = new BE.MenuAuthInfo();
                authInfo.CompanyID = CurrentUserInfo.CompanyID;
                authInfo.CommunityID = communityID;
                authInfo.WriteAuth = writeAuth;
                authInfo.ReadAuth = readAuth;

                BLL.Menu bll = new BLL.Menu();
                if (bll.Add(info, authInfo) < 1)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 커뮤니티 메뉴 수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuMod(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                string nameBase = GetDecryptParam("nameBase", "");
                string multiName = GetDecryptParam("multiName", "");
                int writeAuth = GetDecryptParam("writeAuth", "9").ToNumber<int>();
                int readAuth = GetDecryptParam("readAuth", "9").ToNumber<int>();
                int weight = GetDecryptParam("weight", "0").ToNumber<int>();

                BE.MenuInfo info = new BE.MenuInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;
                info.Weight = weight;
                info.NameBase = nameBase;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                BE.MenuAuthInfo authInfo = new BE.MenuAuthInfo();
                authInfo.CompanyID = CurrentUserInfo.CompanyID;
                authInfo.CommunityID = communityID;
                authInfo.MenuID = menuID;
                authInfo.WriteAuth = writeAuth;
                authInfo.ReadAuth = readAuth;

                BLL.Menu bll = new BLL.Menu();

                return bll.Mod(info, authInfo);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 커뮤니티 메뉴 순서 Up
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuUp(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                return dal.MenuUp(CurrentUserInfo.CompanyID, communityID, menuID, sequence);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 커뮤니티 메뉴 순서 Down
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuDown(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                return dal.MenuDown(CurrentUserInfo.CompanyID, communityID, menuID, sequence);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 커뮤니티 메뉴 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MenuDel(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                DAL.Menu dal = new DAL.Menu();
                return dal.Del(CurrentUserInfo.CompanyID, communityID, menuID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        #endregion 
    }
}
