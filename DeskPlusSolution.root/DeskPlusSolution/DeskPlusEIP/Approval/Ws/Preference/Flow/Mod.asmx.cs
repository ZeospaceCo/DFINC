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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Flow
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
        private string GetTagInfo(string _approvalTag)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";          //리소스명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Preference.Flow.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            string rtnval = "";
            switch (_approvalTag.ToUpper())
            {
                case"ZD":
                    rtnval = GetResource("SeqTitZD");
                    break;
                case "ZH":
                    rtnval = GetResource("SeqTitZH");
                    break;
                case "ZA":
                    rtnval = GetResource("SeqTitZA");
                    break;
                case "ZP":
                    rtnval = GetResource("SeqTitZP");
                    break;
                case "ZQ":
                    rtnval = GetResource("SeqTitZQ");
                    break;
                case "ZC":
                    rtnval = GetResource("SeqTitZC");
                    break;
                default:
                    rtnval = GetResource("SeqTitZD");
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";          //리소스명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Preference.Flow.Mod).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                int favSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();
                string section = GetDecryptParam("section", "");

                DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
                DataTableReader dtr = fsDal.GetFavSeqLineList(CurrentUserInfo.CompanyID, favSeqID, section);

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
                            info.ApprovalTagStr = GetTagInfo(info.ApprovalTag);
                            info.IsDept = dtr["IsDept"].ToString();
                            info.IsDeptStr = (info.IsDept.Equals("1")) ? GetResource("FavTitIsDept1") : GetResource("FavTitIsDept0");
                            info.IsSub = dtr["IsSub"].ToString();
                            info.IsSubStr = (info.IsSub.Equals("1")) ? GetResource("FavTitIsSub1") : GetResource("FavTitIsSub0");

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

                            info.FileDel = resMng.GetString("Del");

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
        /// 작성자   : 
        /// 작성목적 : 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelItem(string p)
        {
            try
            {
                SetDecryptString(p);

                int favSeqLineID = GetDecryptParam("id", "0").ToNumber<int>();

                DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
                return fsDal.Del(CurrentUserInfo.CompanyID, favSeqLineID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : 
        /// 작성목적 : 항목추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddItem(string p)
        {
            try
            {
                SetDecryptString(p);

                //var response = userID + "" + userName + "" + deptID + "" + types;

                int favSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();
                string isSub = GetDecryptParam("isSub", "0");
                string tag = GetDecryptParam("tag", "");
                string section = GetDecryptParam("section", "0");
                string selvals = GetDecryptParam("selvals", "");       //userID,userID,userID,""name,name,name,""deptid,deptid,deptid,""type,type,type,

                // M : userid""name""deptid""type
                // G : deptid""name""parentdeptid"type

                string [] arruserIDs = selvals.Split('')[0].TrimEnd(',').Split(',');
                string [] arrdeptIDs = selvals.Split('')[2].TrimEnd(',').Split(',');
                string [] arrtypes = selvals.Split('')[3].TrimEnd(',').Split(',');

                DAL.FavSeqLine fsDal = new DAL.FavSeqLine();
                for (int k = 0; k < arruserIDs.Length; k++)
                {
                    FavSeqLineInfo info = new FavSeqLineInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.FavSeqID = favSeqID;
                    info.ApprovalTag = tag;
                    info.Sequence = 0;
                    info.Section = section;

                    if (arrtypes[k].Equals("M"))
                    {
                        //사용자
                        info.DeptID = arrdeptIDs[k].ToNumber<int>();
                        info.UserID = arruserIDs[k].ToNumber<int>();
                        info.IsDept = "0";
                        info.IsSub = "0";
                    }
                    else
                    {
                        //부서
                        info.DeptID = arruserIDs[k].ToNumber<int>();
                        info.UserID = 0;
                        info.IsDept = "1";
                        info.IsSub = isSub;
                    }

                    fsDal.Add(info);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : 
        /// 작성목적 : 수정(이름,요약)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModFav(string p)
        {
            try
            {
                SetDecryptString(p);

                int favSeqID = GetDecryptParam("favSeqID", "0").ToNumber<int>();
                string subject = GetDecryptParam("subject", "");
                string memo = GetDecryptParam("memo", "");

                DAL.FavSeq fDal = new DAL.FavSeq();
                return fDal.Mod(CurrentUserInfo.CompanyID, favSeqID, CurrentUserInfo.UserID, subject, memo);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 순서변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSeq(string p)
        {
            SetDecryptString(p);
            string sortlist = GetDecryptParam("sortlist", "");

            BLL.FavSeqLine fsLine = new BLL.FavSeqLine();
            return fsLine.ModSeq(CurrentUserInfo.CompanyID, sortlist);
        }
    }
}
