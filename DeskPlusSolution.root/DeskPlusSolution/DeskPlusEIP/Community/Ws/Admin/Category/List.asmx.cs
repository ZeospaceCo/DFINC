using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Admin.Category
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSCommunity
    {
        #region GET()
        /// <summary>
        /// 작성자 : shine
        /// 대분류 리스트 가져오기(수정용)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList()
        {
            try
            {
                List<object> listobj = new List<object>();

                DAL.Category dal = new DAL.Category();
                DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.CategoryInfo info = new CategoryInfo();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            #region 다국어(콤마로 갖고오기)
                            info.MultiName = GetMuiltiLang(dtr["NameKo"].ToString().ToNull().ToReverseXSS(), 
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS());
                            #endregion
                            #region 다국어언어 간추려서 하나의 언어 반환하기
                            info.GetName = GetLangName(dtr["NameBase"].ToString().ToNull().ToReverseXSS(), 
                                dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS(), CurrentUserInfo.PreferredLanguage);
                            #endregion
                            info.NameBase = dtr["NameBase"].ToString().ToNull().ToReverseXSS();

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
        /// 작성자 : shine
        /// 소분류 리스트 가져오기(수정용)
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> SubList(string p)
        {
            try 
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                int categoryID = GetDecryptParam("parentCategoryID", "0").ToNumber<int>();

                DAL.Category dal = new DAL.Category();
                DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID, categoryID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.CategoryInfo info = new CategoryInfo();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            #region 다국어(콤마로 갖고오기)
                            info.MultiName = GetMuiltiLang(dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS());
                            #endregion
                            #region 다국어언어 간추려서 하나의 언어 반환하기
                            info.GetName = GetLangName(dtr["NameBase"].ToString().ToNull().ToReverseXSS(), 
                                dtr["NameKo"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameEn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameJa"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhcn"].ToString().ToNull().ToReverseXSS(),
                                dtr["NameZhtw"].ToString().ToNull().ToReverseXSS(), CurrentUserInfo.PreferredLanguage);
                            #endregion
                            info.NameBase = dtr["NameBase"].ToString().ToNull().ToReverseXSS();

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

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 대분류,소분류 카테고리 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryName = GetDecryptParam("categoryName", "");
                string multiName = GetDecryptParam("multiName", "");
                int parentCategoryID = GetDecryptParam("parentCategoryID", "0").ToNumber<int>();

                BE.CategoryInfo info = new CategoryInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = categoryName;
                
                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.ParentCategoryID = parentCategoryID;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.WriteUserName = CurrentUserInfo.Name;

                DAL.Category dal = new DAL.Category();
                dal.Add(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 대분류, 소분류 카테고리 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Mod(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryName = GetDecryptParam("categoryName", "");
                string multiName = GetDecryptParam("multiName", "");
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

                BE.CategoryInfo info = new CategoryInfo();
                
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = categoryName;

                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.CategoryID = categoryID;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.Category dal = new DAL.Category();
                dal.Mod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : shine
        /// 대분류 카테고리 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try 
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

                BE.CategoryInfo info = new CategoryInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID;

                DAL.Category dal = new DAL.Category();
                dal.Del(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
