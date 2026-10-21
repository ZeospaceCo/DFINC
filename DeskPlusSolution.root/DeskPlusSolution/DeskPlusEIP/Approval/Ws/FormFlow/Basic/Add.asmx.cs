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

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Basic
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
        #region GET

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetLine(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Basic.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int formid = GetDecryptParam("formid", "0").ToNumber<int>();

            DAL.FavoriteApprovalNet dal = new DAL.FavoriteApprovalNet();
            DataTableReader dtr = dal.GetFavoriteApprovalNet(CurrentUserInfo.CompanyID, formid, CurrentUserInfo.UserID);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Common.SetApproval("ZD0", UserID, UserName, DeptID, DeptName, TitleID, TitleName, DutyID, DutyName, "M", "0", "");
                        //data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, data.d[i].IsGroup, data.d[i].IsSubTree, ""

                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["NetUserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.DeptID = dtr["NetDeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsDept"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.DeptBase = fInfo.NameBase;
                        }
                        else
                        {
                            fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        }
                        fInfo.TitleID = dtr["CodeTitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["CodeDutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsDept"].ToString();
                        fInfo.IsSubTree = dtr["IsSub"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetModfyLine(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Basic.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int formid = GetDecryptParam("formid", "0").ToNumber<int>();

            DAL.FavoriteApprovalNet dal = new DAL.FavoriteApprovalNet();
            DataTableReader dtr = dal.GetFavoriteApprovalNetModfy(CurrentUserInfo.CompanyID, formid, CurrentUserInfo.UserID);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.ApprovalTag = dtr["ApprovalTag"].ToString();
                        fInfo.UserID = dtr["NetUserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.DeptID = dtr["NetDeptID"].ToString().ToNumber<int>();
                        if (dtr["ApprovalTag"].ToString().ToUpper().Equals("ZC") && dtr["IsDept"].ToString().ToUpper().Equals("G"))
                        {
                            fInfo.NameBase = "";
                        }
                        fInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhcn"].ToString(), PreLang);
                        fInfo.TitleID = dtr["CodeTitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["CodeDutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsDept"].ToString();
                        fInfo.IsSubTree = dtr["IsSub"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNetZQZCLine(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Basic.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int formid = GetDecryptParam("formid", "0").ToNumber<int>();
            string appointerKind = GetDecryptParam("appointerKind", "");

            DAL.FormAppointer dal = new DAL.FormAppointer();
            DataTableReader dtr = dal.GetFormAppointerIsActive(CurrentUserInfo.CompanyID, formid, appointerKind);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.SequenceInfo fInfo = new BE.SequenceInfo();

                        fInfo.ApprovalTag = dtr["AppointerKind"].ToString();
                        fInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        fInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhcn"].ToString(), PreLang);
                        fInfo.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        fInfo.DeptBase = dtr["DeptBase"].ToString();
                        fInfo.TitleID = dtr["CodeTitleID"].ToString().ToNumber<int>();
                        fInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhcn"].ToString(), PreLang);
                        fInfo.DutyID = dtr["CodeDutyID"].ToString().ToNumber<int>();
                        fInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhcn"].ToString(), PreLang);
                        fInfo.IsGroup = dtr["IsDept"].ToString();
                        fInfo.IsSubTree = dtr["IsSub"].ToString();
                        uobj.Add(fInfo);
                    }
                }
            }

            return uobj;
        }
        #endregion

        #region SET
        /// <summary>
        /// 결재망저장
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetNetLine(string p)
        {
            try
            {
                SetDecryptString(p);

                int formid = GetDecryptParam("formid", "0").ToNumber<int>();
                string vals = GetDecryptParam("vals", "").TrimEnd('$');

                if (formid.Equals(0)) return "false";

                //  apptag | seq | userid | deptid | isdept | issub $ apptag | seq | userid | deptid | isdept | issub $
                //  0        1     2        3        4        5

                DAL.FavoriteApprovalNet dal = new DAL.FavoriteApprovalNet();
                if (dal.Del(CurrentUserInfo.CompanyID, formid, CurrentUserInfo.UserID))
                {
                    if (!vals.Equals(""))
                    {
                        string[] arr = vals.Split('$');

                        foreach (string s in arr)
                        {
                            if (!s.Split('|')[0].ToUpper().Equals("ZD0"))
                            {
                                BE.FavoriteApprovalNetInfo info = new FavoriteApprovalNetInfo();

                                info.CompanyID = CurrentUserInfo.CompanyID;
                                info.FormID = formid;
                                info.UserID = CurrentUserInfo.UserID;
                                info.ApprovalTag = s.Split('|')[0].ToUpper();
                                info.Sequence = int.Parse(s.Split('|')[1]);
                                info.NetUserID = int.Parse(s.Split('|')[2]);
                                info.NetDeptID = int.Parse(s.Split('|')[3]);
                                if (info.ApprovalTag == "ZP" || info.ApprovalTag == "ZQ")
                                {
                                    info.IsDept = "M";
                                }
                                else
                                {
                                    info.IsDept = s.Split('|')[4];
                                }
                                info.IsSub = s.Split('|')[5];

                                dal.Add(info);
                            }
                        }
                    }
                }

                return "true";
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
