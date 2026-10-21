using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Category
{
    /// <summary>
    /// Headers의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Headers : Base.BaseWSBoard
    {
        #region 말머리

        /// <summary>
        /// 말머리 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddHeader(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int headerID = GetDecryptParam("headerID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");

                int companyID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                HeaderInfo be = new HeaderInfo();
                be.CompanyID = companyID;
                be.CategoryID = categoryID;
                be.HeaderID = headerID;
                be.NameBase = name;
                be.Orders = 0;
                be.UseActive = "1";
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.Header dal = new DAL.Header();
                dal.AddMod(be);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddHeader : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 말머리 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelHeader(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int headerID = GetDecryptParam("headerID", "0").ToNumber<int>();

                int companyID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                HeaderInfo be = new HeaderInfo();
                be.CompanyID = companyID;
                be.CategoryID = categoryID;
                be.HeaderID = headerID;

                DAL.Header dal = new DAL.Header();
                dal.Del(be);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelHeader : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 말머리 리스트 가져오기 (json 으로 가져오자)
        /// shimpark
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetHeaderList(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                if (categoryID == 0) return "";

                int companyID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                DAL.Header dal = new DAL.Header();
                List<BE.HeaderInfo> al = dal.GetHeaderLists(companyID, categoryID);
                ObjList listobj = new ObjList();

                foreach (var info in al)
                {
                    BE.HeaderInfo be = new HeaderInfo();
                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.HeaderID = info.HeaderID;
                    be.CategoryID = info.CategoryID;
                    be.Orders = info.Orders;

                    be.NameBase = this.GetLangName(
                            info.NameBase.ToReverseXSS(), info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(),
                            info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS(), PreLang);

                    be.UseActive = info.UseActive;
                    be.WriteUserID = info.WriteUserID;
                    be.WriteDate = info.WriteDate;

                    listobj.Add(be);
                }

                return JsonConvert.SerializeObject(listobj);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetHeaderList : " + ex.Message);
                throw;
            }
        }

        #endregion 말머리
    }
}