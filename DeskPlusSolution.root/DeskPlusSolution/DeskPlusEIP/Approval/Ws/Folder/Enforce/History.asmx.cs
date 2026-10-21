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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Enforce
{
    /// <summary>
    /// History의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class History : Base.BaseWSApproval
    {

        /// <summary>
        /// 시행문 직인리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Folder.Enforce.History).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                int enforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();

                DAL.EnforceHistory dal = new DAL.EnforceHistory();
                List<BE.EnforceHistoryInfo> list = dal.GetEnforceHistory(CurrentUserInfo.CompanyID, enforceID);

                ObjList listobj = new ObjList();
                foreach (BE.EnforceHistoryInfo info in list)
                {
                    info.NameBase= this.GetLangName(info.NameBase, info.NameKo,info.NameEn, info.NameJa,info.NameFr, info.NameZhcn,info.NameZhtw, PreLang);
                    info.DeptBase = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                    info.TitleBase = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                    info.DutyBase = this.GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhcn, info.DutyZhtw, PreLang);

                    info.SendTypeStr = (info.SendType.Equals("0")) ? this.GetResource("EnforceTypeMail") : this.GetResource("EnforceTypePrint");

                    DateTime dt;

                    if (DateTime.TryParse(info.WriteDate.ToString(), out dt))
                        info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                    else
                        info.WriteDateStr = string.Empty;

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
