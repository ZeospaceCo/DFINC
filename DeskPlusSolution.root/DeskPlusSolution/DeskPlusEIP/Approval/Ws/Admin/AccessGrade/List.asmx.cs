using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.AccessGrade
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        #region GET
        /// <summary>
        /// 열람등급 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.AccessGrade.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>();
            int CurrentPage = GetDecryptParam("currentPage", "0").ToNumber<int>();
            string SearchField = GetDecryptParam("searchField", "");
            string SearchValue = GetDecryptParam("searchValue", "");
            string Order = GetDecryptParam("order", "");

            DAL.AccessGrade fDal = new DAL.AccessGrade();
            DataTableReader dtr = fDal.GetList(CurrentUserInfo.CompanyID, RecordPerPage, CurrentPage, SearchField, SearchValue, Order);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AccessGradeInfo fInfo = new BE.AccessGradeInfo();

                        fInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        fInfo.AccessID = dtr["AccessID"].ToString().ToNumber<int>();
                        fInfo.AccessGrade = dtr["AccessGrade"].ToString().ToNumber<int>();
                        //fInfo.IsAccess = dtr["IsAccess"].ToString();

                        fInfo.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToReverseXSS().ToNull(), dtr["NameKo"].ToString().ToReverseXSS().ToNull(),
                                dtr["NameEn"].ToString().ToReverseXSS().ToNull(), dtr["NameJa"].ToString().ToReverseXSS().ToNull(),
                                dtr["NameFr"].ToString().ToReverseXSS().ToNull(), dtr["NameZhcn"].ToString().ToReverseXSS().ToNull(),
                                dtr["NameZhtw"].ToString().ToReverseXSS().ToNull(), PreLang);


                        if (dtr["IsAccess"].ToString() == "1")
                        {
                            fInfo.IsAccess = "Y";
                        }
                        else
                        {
                            fInfo.IsAccess = "N";
                        }

                        fInfo.AccessGradeUser = this.GetLangName(
                                dtr["AccessGradeUser"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserKo"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserEn"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserJa"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserFr"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserZhcn"].ToString().ToReverseXSS().ToNull(),
                                dtr["AccessGradeUserZhtw"].ToString().ToReverseXSS().ToNull(),
                                PreLang);

                        fInfo.IsBase = (dtr["IsBase"].ToString().ToNull().Equals("")) ? "0" : dtr["IsBase"].ToString().ToNull();
                        fInfo.IsBaseStr = (fInfo.IsBase.Equals("1")) ? "Y" : "N";

                        uobj.Add(fInfo);
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 열람등급 갯수 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            string SearchField = GetDecryptParam("searchField", "");
            string SearchValue = GetDecryptParam("searchValue", "");

            DAL.AccessGrade fDal = new DAL.AccessGrade();
            int listCnt = fDal.GetList(CurrentUserInfo.CompanyID, SearchField, SearchValue);

            return listCnt;
        }

        /// <summary>
        /// 열람등급 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListIsAccess(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.AccessGrade.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            DAL.AccessGrade fDal = new DAL.AccessGrade();

            DataSet ds = fDal.GetListIsAccess(CurrentUserInfo.CompanyID, "1");
            List<object> uobj = new List<object>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BE.AccessGradeInfo fInfo = new BE.AccessGradeInfo();

                fInfo.NameBase = GetLangName(dr["NameBase"].ToString().ToReverseXSS(), dr["NameKo"].ToString().ToReverseXSS(), dr["NameEn"].ToString().ToReverseXSS(), dr["NameJa"].ToString().ToReverseXSS(), dr["NameFr"].ToString().ToReverseXSS(), dr["NameZhcn"].ToString().ToReverseXSS(), dr["NameZhtw"].ToString().ToReverseXSS(), PreLang);
                fInfo.AccessGradeUser = GetLangName(dr["AccessGradeUser"].ToString(), dr["AccessGradeUserKo"].ToString(), dr["AccessGradeUserEn"].ToString(), dr["AccessGradeUserJa"].ToString(), dr["AccessGradeUserFr"].ToString(), dr["AccessGradeUserZhcn"].ToString(), dr["AccessGradeUserZhtw"].ToString(), PreLang);

                uobj.Add(fInfo);
            }

            return uobj;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-22
        /// 작성목적 : 파일 삭제 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetChkList(string p)
        {
            try
            {
                SetDecryptString(p);
                string tmpValue = GetDecryptParam("id", "");

                tmpValue = tmpValue.TrimEnd(',');

                DAL.AccessGrade dal = new DAL.AccessGrade();
                return dal.GetChkList(CurrentUserInfo.CompanyID, tmpValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region SET

        /// <summary>
        /// 삭제(다중)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.AccessGrade fBll = new BLL.AccessGrade();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    fBll.Del(CurrentUserInfo.CompanyID, id.ToNumber<int>());
                }
            }
            return true;
        }

        /// <summary>
        /// 활성화/비활성화
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UpdActive(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.AccessGrade fBll = new BLL.AccessGrade();
            foreach (string id in arrID)
            {
                if (id.Split('-').Length.Equals(2))
                {
                    fBll.UpdActive(CurrentUserInfo.CompanyID, id.Split('-')[0].ToNumber<int>(), id.Split('-')[1]);
                }
            }
            return true;
        }

        /// <summary>
        /// 기본값설정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetBase(string p)
        {
            SetDecryptString(p);
            int accessID = GetDecryptParam("aid", "0").ToNumber<int>();

            bool rtnval = false;
            if (accessID > 0)
            {
                DAL.AccessGrade dal = new DAL.AccessGrade();
                rtnval = dal.UpdBase(CurrentUserInfo.CompanyID, accessID);
            }

            return rtnval;
        }
        
        #endregion
    }
}
