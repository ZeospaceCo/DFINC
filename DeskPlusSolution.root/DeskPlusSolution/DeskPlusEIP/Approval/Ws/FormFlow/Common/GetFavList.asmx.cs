using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common
{
    /// <summary>
    /// GetFavList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetFavList : Base.BaseWSApproval
    {
        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.FavSeqInfo> GetFlowList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.GetFavList).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                //SetDecryptString(p);
                
                DAL.FavSeq biz = new DAL.FavSeq();
                List<BE.FavSeqInfo> list = biz.GetFavSeqList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                
                int i = 1;
                foreach (BE.FavSeqInfo info in list)
                {
                    info.RowStr = i.ToString();
                    i++;
                    info.WriteDateStr = DateHelpers.GetCultureDate(info.WriteDate, PreLangReal).Substring(0, 10);
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 특정 결재선 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetFlowSeqLine(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Common.GetFavList).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                int favSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();

                DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
                DataTableReader dtr = fsDal.GetFavSeqLineList(CurrentUserInfo.CompanyID, favSeqID);

                ObjList listobj = new ObjList();

                int i = 1;
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FavSeqLineInfo info = new FavSeqLineInfo();

                            info.RowStr = i.ToString();
                            i++;

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.FavSeqID = dtr["FavSeqID"].ToString().ToNumber<int>();
                            info.FavSeqLineID = dtr["FavSeqLineID"].ToString().ToNumber<int>();
                            info.ApprovalTag = dtr["ApprovalTag"].ToString();
                            info.IsDept = dtr["IsDept"].ToString();
                            info.IsDept = (info.IsDept.Equals("1")) ? "G" : "M";
                            info.IsSub = dtr["IsSub"].ToString();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                            #endregion

                            info.LoginID = dtr["LoginID"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                            info.TitleID = dtr["CodeTitleID"].ToString().ToNumber<int>();
                            info.DutyID = dtr["CodeDutyID"].ToString().ToNumber<int>();

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
    }
}
