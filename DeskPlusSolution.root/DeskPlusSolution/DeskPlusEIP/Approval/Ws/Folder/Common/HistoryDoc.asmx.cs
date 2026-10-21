using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Common
{
    /// <summary>
    /// HistoryDoc의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class HistoryDoc : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetList(string p)
        {
            SetDecryptString(p);

            int childDocID = GetDecryptParam("childDocID", "0").ToNumber<int>();
            if (childDocID == 0) return null;

            ObjList listobj = new ObjList();

            DAL.History dal = new DAL.History();
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, childDocID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Approval.BE.HistoryInfo info = new HistoryInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                        info.Subject = dtr["Subject"].ToString();
                        info.HistoryID = dtr["HistoryID"].ToString().ToNumber<int>();

                        #region 이름 다국어 설정
                        info.ModifyNameBase = this.GetLangName(
                                dtr["ModifyNameBase"].ToString().ToNull(), dtr["ModifyNameKo"].ToString().ToNull(),
                                dtr["ModifyNameEn"].ToString().ToNull(), dtr["ModifyNameJa"].ToString().ToNull(),
                                dtr["ModifyNameFr"].ToString().ToNull(), dtr["ModifyNameZhcn"].ToString().ToNull(),
                                dtr["ModifyNameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 부서 다국어 설정
                        info.DeptBase = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직책 다국어 설정
                        info.DutyBase = this.GetLangName(
                                dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                        #endregion


                        DateTime dt;

                        if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                            info.ModifyDateStr = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.TimeOfDay.ToString().Substring(0,5);
                        else
                            info.ModifyDateStr = string.Empty;             

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }



    }
}
