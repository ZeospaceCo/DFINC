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

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Submit
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {
        /// <summary>
        /// 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Submit.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.SequenceBackUp seqDal = new DAL.SequenceBackUp();
            DataTableReader dtr = seqDal.GetSeqLineTot(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.DeptBase = fInfo.NameBase;
                        }
                        else
                        {
                            fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        }
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        fInfo.Section = dtr["Section"].ToString();
                        fInfo.TagOrder = dtr["TagOrder"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }



        /// <summary>
        /// 부서정보가 변경된 결재라인정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetModfySeq(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Submit.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int docID = GetDecryptParam("docID", "0").ToNumber<int>();

            DAL.SequenceBackUp seqDal = new DAL.SequenceBackUp();
            DataTableReader dtr = seqDal.GetSeqModfyLineTot(CurrentUserInfo.CompanyID, docID);

            List<object> uobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {                        
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.ApprovalDate = DateTime.Parse(dtr["ApprovalDate"].ToString());
                        fInfo.ApprovalDateYMD = dtr["ApprovalDateYMD"].ToString();
                        fInfo.ApprovalType = dtr["ApprovalType"].ToString();
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsGroup"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.NameBase = "";
                        }                        
                        fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        fInfo.TitleID = dtr["TitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["DutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsGroup"].ToString();
                        fInfo.IsSubTree = dtr["IsSubTree"].ToString();
                        fInfo.UserList = dtr["UserList"].ToString();

                        fInfo.Section = dtr["Section"].ToString();
                        fInfo.TagOrder = dtr["TagOrder"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }
    }
}
