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
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSApproval
    {
        #region GET()
        /// <summary>
        /// 작성자 : 이지원
        /// 목적 : 직급 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int accessID = GetDecryptParam("accessID", "0").ToNumber<int>();
            
            try
            {
                DAL.AccessGrade dal = new DAL.AccessGrade();
                DataTableReader dtr = dal.GetTitleList(companyID, accessID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AccessGradeInfo info = new BE.AccessGradeInfo ();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.TitleID = dtr["CodeID"].ToString().ToNumber<int>();                         
                            info.TitleName = this.GetLangName(
                                dtr["NameBase"].ToString().ToReverseXSS(),
                                dtr["NameKo"].ToString().ToReverseXSS(), dtr["NameEn"].ToString().ToReverseXSS(), dtr["NameJa"].ToString().ToReverseXSS(),
                                dtr["NameFr"].ToString().ToReverseXSS(), dtr["NameZhCn"].ToString().ToReverseXSS(), dtr["NameZhTw"].ToString().ToReverseXSS(), PreLang);
   
                            if (dtr["ActiveCode"].ToString() == "1")
                            {
                                info.ActiveCode = "Yes";
                            }
                            else
                            {
                                info.ActiveCode = "No";
                            }

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetList : " + ex);
                return null;
            }
        }
        #endregion

        #region ADD()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 작성목적 : 리스트에 직급 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string accessID = GetDecryptParam("accessID", "0");
                string codeID = GetDecryptParam("codeID", "0");

                DAL.AccessGrade biz = new DAL.AccessGrade();

                biz.AddTitle(companyID.ToNumber<int>(), accessID.ToNumber<int>(), codeID.ToNumber<int>());

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;
            }
        }
        #endregion

        #region DEL()
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-06-13
        /// 작성목적 : 리스트에 직급 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string codeIDList = GetDecryptParam("codeIDList", "0");

                DAL.AccessGrade biz = new DAL.AccessGrade();

                foreach (string codeID in codeIDList.Split(','))
                {
                    biz.DelTitle(companyID.ToNumber<int>(), codeID.ToNumber<int>());
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;
            }
        }
        #endregion
    }
}