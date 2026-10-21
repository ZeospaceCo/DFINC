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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status.Common
{
    /// <summary>
    /// DocUserSeq의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class DocUserSeq : Base.BaseWSApproval
    {
        #region GET()

        #region 결재자정보

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 결재자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetApprovalUserInfoList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Status.Common.DocUserSeq).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string childDocID = GetDecryptParam("childDocID", "0");
                string zd = GetDecryptParam("zd", ""); // "발신"
                string zpzq = GetDecryptParam("zpzq", ""); // "수신"

                string accountTag = "";
                int documentID = 0;
                int num = 0;

                ObjList listobj = new ObjList();
                ObjList listobjZR = new ObjList();

                DAL.Sequence dalSeq = new DAL.Sequence();
                List<BE.SequenceInfo> SeqInfo = dalSeq.GetSeqLineUnion(CurrentUserInfo.CompanyID, childDocID.ToNumber<int>());
                    
                // 문서의 결재자수 만큼 반복
                foreach (BE.SequenceInfo _seqInfo in SeqInfo)
                {
                    BE.SequenceInfo Info = new SequenceInfo();

                    // 수/발신 체크 Gubun
                    if (documentID == 0)
                    {
                        accountTag = zd; //발신
                        Info.Gubun = accountTag;
                    }
                    else if (documentID == _seqInfo.DocumentID.ToString().ToNumber<int>())
                        Info.Gubun = accountTag; //이전 상태 따라감
                    else
                    {
                        num += 1;
                        accountTag = zpzq + num; //수신1, 수신2 ....
                        Info.Gubun = accountTag;
                    }

                    documentID = _seqInfo.DocumentID.ToString().ToNumber<int>();

                    //Info.Gubun = accountTag;
                    Info.Sequence = _seqInfo.Sequence;
                    Info.UserID = _seqInfo.UserID;

                    if (_seqInfo.ApprovalTag == "ZD0")
                        Info.ApprovalTag = _seqInfo.ApprovalTag;
                    else
                        Info.ApprovalTag = _seqInfo.ApprovalTag.Substring(0, 2);

                    Info.DeptName = GetLangName(_seqInfo.DeptBase.ToReverseXSS(),
                        _seqInfo.DeptKo.ToReverseXSS(),
                        _seqInfo.DeptEn.ToReverseXSS(),
                        _seqInfo.DeptJa.ToReverseXSS(),
                        _seqInfo.DeptFr.ToReverseXSS(),
                        _seqInfo.DeptZhcn.ToReverseXSS(),
                        _seqInfo.DeptZhtw.ToReverseXSS(), PreLang);

                    Info.TitleName = GetLangName(_seqInfo.TitleBase.ToReverseXSS(),
                        _seqInfo.TitleKo.ToReverseXSS(),
                        _seqInfo.TitleEn.ToReverseXSS(),
                        _seqInfo.TitleJa.ToReverseXSS(),
                        _seqInfo.TitleFr.ToReverseXSS(),
                        _seqInfo.TitleZhcn.ToReverseXSS(),
                        _seqInfo.TitleZhtw.ToReverseXSS(), PreLang);

                    Info.UserName = GetLangName(_seqInfo.NameBase.ToReverseXSS(),
                        _seqInfo.NameKo.ToReverseXSS(),
                        _seqInfo.NameEn.ToReverseXSS(),
                        _seqInfo.NameJa.ToReverseXSS(),
                        _seqInfo.NameFr.ToReverseXSS(),
                        _seqInfo.NameZhcn.ToReverseXSS(),
                        _seqInfo.NameZhtw.ToReverseXSS(), PreLang);

                    Info.Proxy = _seqInfo.Proxy;

                    Info.ProxyUserName = GetLangName(_seqInfo.ProxyNameBase.ToReverseXSS(),
                        _seqInfo.ProxyNameKo.ToReverseXSS(),
                        _seqInfo.ProxyNameEn.ToReverseXSS(),
                        _seqInfo.ProxyNameJa.ToReverseXSS(),
                        _seqInfo.ProxyNameFr.ToReverseXSS(),
                        _seqInfo.ProxyNameZhcn.ToReverseXSS(),
                        _seqInfo.ProxyNameZhtw.ToReverseXSS(), PreLang);

                    Info.ProxyTitleName = GetLangName(_seqInfo.P_TitleBase.ToReverseXSS(),
                        _seqInfo.P_TitleKo.ToReverseXSS(),
                        _seqInfo.P_TitleEn.ToReverseXSS(),
                        _seqInfo.P_TitleJa.ToReverseXSS(),
                        _seqInfo.P_TitleFr.ToReverseXSS(),
                        _seqInfo.P_TitleZhcn.ToReverseXSS(),
                        _seqInfo.P_TitleZhtw.ToReverseXSS(), PreLang);

                    Info.ProxyDeptName = GetLangName(_seqInfo.P_DeptBase.ToReverseXSS(),
                        _seqInfo.P_DeptKo.ToReverseXSS(),
                        _seqInfo.P_DeptEn.ToReverseXSS(),
                        _seqInfo.P_DeptJa.ToReverseXSS(),
                        _seqInfo.P_DeptFr.ToReverseXSS(),
                        _seqInfo.P_DeptZhcn.ToReverseXSS(),
                        _seqInfo.P_DeptZhtw.ToReverseXSS(), PreLang);

                    #region 작성일자
                    DateTime dt;
                    if (DateTime.TryParse(_seqInfo.ApprovalDate.ToNullDT().ToString(), out dt))
                        Info.ApprovalDateStr = _seqInfo.ApprovalDate.ToShortDateString() + " " + _seqInfo.ApprovalDate.ToShortTimeString();
                    else
                        Info.ApprovalDateStr = string.Empty;
                    #endregion

                    Info.ApprovalType = _seqInfo.ApprovalType.ToNull();
                    Info.ApprovalTypeStr = GetApprovalTypeStr(Info.ApprovalType);

                    if (Info.ApprovalTag.IndexOf("ZR") < 0)
                    {
                        listobj.Add(Info);
                    }
                    else
                    {
                        listobjZR.Add(Info);
                    }
                }

                foreach (object obj in listobjZR)
                {
                    listobj.Add(obj);
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
        /// 결재정보
        /// </summary>
        /// <param name="_approvalType"></param>
        /// <returns></returns>
        private string GetApprovalTypeStr(string _approvalType)
        {
            if (_approvalType.Equals("")) return "";

            string rtnval = "";
            switch (_approvalType.ToUpper())
            {
                case "S":
                    rtnval = this.GetResource("ApprovalTypeS");
                    break;
                case "A":
                    rtnval = this.GetResource("ApprovalTypeA");
                    break;
                case "R":
                    rtnval = this.GetResource("ApprovalTypeR");
                    break;
                case "L":
                    rtnval = this.GetResource("ApprovalTypeL");
                    break;
                default:
                    rtnval = "";
                    break;
            }
            return rtnval;
        }

        #endregion


        #region 참조자 정보

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 참조자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetApprovalUserInfoListCA(string p)
        {
            try
            {                
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Status.Common.DocUserSeq).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string documentID = GetDecryptParam("documentID", "0");
                string childDocID = GetDecryptParam("childDocID", "0");

                DAL.DocumentProcess dalDoc = new DAL.DocumentProcess();
                List<DocumentProcessInfo> DocList = dalDoc.GetDocument(CurrentUserInfo.CompanyID, childDocID.ToNumber<int>());
                string accountTag = DocList[0].AccountTag.Substring(0, 2); 

                ObjList listobj = new ObjList();

                DAL.CarbonCopyUser dal = new DAL.CarbonCopyUser();
                DAL.CarbonCopyUserGroup dalGroup = new DAL.CarbonCopyUserGroup();
                List<BE.CarbonCopyUserInfo> CaInfo;
                
                if (accountTag == "ZQ")
                    CaInfo = dal.GetCarbonCopyUserZQ(CurrentUserInfo.CompanyID, childDocID.ToNumber<int>(), "process");
                else
                    CaInfo = dal.GetCarbonCopyUser(CurrentUserInfo.CompanyID, documentID.ToNumber<int>());

                // 문서의 결재자수 만큼 반복
                foreach (BE.CarbonCopyUserInfo _seqInfo in CaInfo)
                {
                    BE.CarbonCopyUserInfo Info = new CarbonCopyUserInfo();

                    Info.UserID = _seqInfo.UserID;

                    #region 부서 다국어 설정
                    string tmDept = GetLangName(_seqInfo.DeptBase.ToReverseXSS(),
                        _seqInfo.DeptKo.ToReverseXSS(),
                        _seqInfo.DeptEn.ToReverseXSS(),
                        _seqInfo.DeptJa.ToReverseXSS(),
                        _seqInfo.DeptFr.ToReverseXSS(),
                        _seqInfo.DeptZhcn.ToReverseXSS(),
                        _seqInfo.DeptZhtw.ToReverseXSS(), PreLang);
                    #endregion

                    #region 직급 다국어 설정
                    string tmpTitle = GetLangName(_seqInfo.TitleBase.ToReverseXSS(),
                        _seqInfo.TitleKo.ToReverseXSS(),
                        _seqInfo.TitleEn.ToReverseXSS(),
                        _seqInfo.TitleJa.ToReverseXSS(),
                        _seqInfo.TitleFr.ToReverseXSS(),
                        _seqInfo.TitleZhcn.ToReverseXSS(),
                        _seqInfo.TitleZhtw.ToReverseXSS(), PreLang);
                    #endregion

                    #region 이름 다국어 설정
                    string tmpName = GetLangName(_seqInfo.NameBase.ToReverseXSS(),
                        _seqInfo.NameKo.ToReverseXSS(),
                        _seqInfo.NameEn.ToReverseXSS(),
                        _seqInfo.NameJa.ToReverseXSS(),
                        _seqInfo.NameFr.ToReverseXSS(),
                        _seqInfo.NameZhcn.ToReverseXSS(),
                        _seqInfo.NameZhtw.ToReverseXSS(), PreLang);
                    #endregion

                    Info.IsGroup = _seqInfo.IsGroup;

                    Info.ApprovalTag = _seqInfo.ApprovalTag;

                    if (_seqInfo.IsGroup == "G")
                    {
                        if (_seqInfo.IsSubTree == "1")
                        {
                            Info.NameBase = tmpName + " " + this.GetResource("TitSub");
                        }
                        else
                        {
                            Info.NameBase = tmpName;
                        }

                        List<CarbonCopyUserGroupInfo> grouplist = dalGroup.GetCarbonCopyUserGroup(_seqInfo.CompanyID, _seqInfo.DocumentID).Where(
                            x => x.ApprovalTag == _seqInfo.ApprovalTag
                        ).ToList<BE.CarbonCopyUserGroupInfo>();

                        if (grouplist.Count > 0)
                        {
                            Info.IsGroupStr = "1"; // 조회한 사용자가 있는지 여부

                            CarbonCopyUserGroupInfo[] CarbonCopyUserGroupInfo = new CarbonCopyUserGroupInfo[grouplist.Count];

                            int index = 0;
                            foreach (BE.CarbonCopyUserGroupInfo _ginfo in grouplist)
                            {
                                BE.CarbonCopyUserGroupInfo groupInfo = new CarbonCopyUserGroupInfo();

                                #region 부서 다국어 설정
                                string tmpGroupDept = GetLangName(_ginfo.DeptBase.ToReverseXSS(),
                                    _ginfo.DeptKo.ToReverseXSS(),
                                    _ginfo.DeptEn.ToReverseXSS(),
                                    _ginfo.DeptJa.ToReverseXSS(),
                                    _ginfo.DeptFr.ToReverseXSS(),
                                    _ginfo.DeptZhcn.ToReverseXSS(),
                                    _ginfo.DeptZhtw.ToReverseXSS(), PreLang);
                                #endregion

                                #region 직급 다국어 설정
                                string tmpGroupTitle = GetLangName(_ginfo.TitleBase.ToReverseXSS(),
                                    _ginfo.TitleKo.ToReverseXSS(),
                                    _ginfo.TitleEn.ToReverseXSS(),
                                    _ginfo.TitleJa.ToReverseXSS(),
                                    _ginfo.TitleFr.ToReverseXSS(),
                                    _ginfo.TitleZhcn.ToReverseXSS(),
                                    _ginfo.TitleZhtw.ToReverseXSS(), PreLang);
                                #endregion

                                #region 이름 다국어 설정
                                string tmpGroupName = GetLangName(_ginfo.NameBase.ToReverseXSS(),
                                    _ginfo.NameKo.ToReverseXSS(),
                                    _ginfo.NameEn.ToReverseXSS(),
                                    _ginfo.NameJa.ToReverseXSS(),
                                    _ginfo.NameFr.ToReverseXSS(),
                                    _ginfo.NameZhcn.ToReverseXSS(),
                                    _ginfo.NameZhtw.ToReverseXSS(), PreLang);
                                #endregion

                                groupInfo.NameBase = "- " + tmpGroupDept + "/" + tmpGroupTitle + "/" + tmpGroupName;

                                #region 작성일자
                                DateTime gdt;
                                if (DateTime.TryParse(_ginfo.ViewingDate.ToNullDT().ToString(), out gdt))
                                {
                                    groupInfo.ViewingDateStr = _ginfo.ViewingDate.ToShortDateString() + " " + _ginfo.ViewingDate.ToShortTimeString();
                                }
                                else
                                {
                                    groupInfo.ViewingDateStr = string.Empty;
                                }
                                #endregion

                                groupInfo.ApprovalTag = _ginfo.ApprovalTag;

                                CarbonCopyUserGroupInfo[index] = groupInfo;
                                index++;
                            }

                            Info.CarbonCopyUserGroupInfo = CarbonCopyUserGroupInfo;
                        }
                        else
                        {
                            Info.IsGroupStr = "0";

                            CarbonCopyUserGroupInfo[] CarbonCopyUserGroupInfo = new CarbonCopyUserGroupInfo[0];
                            Info.CarbonCopyUserGroupInfo = CarbonCopyUserGroupInfo;
                        }
                    }
                    else
                    {
                        Info.IsGroupStr = "0";

                        Info.NameBase = tmDept + "/" + tmpTitle + "/" + tmpName;

                        CarbonCopyUserGroupInfo[] CarbonCopyUserGroupInfo = new CarbonCopyUserGroupInfo[0];
                        Info.CarbonCopyUserGroupInfo = CarbonCopyUserGroupInfo;
                    } 

                    #region 작성일자
                    DateTime dt;
                    if (DateTime.TryParse(_seqInfo.ViewingDate.ToNullDT().ToString(), out dt))
                    {
                        Info.ViewingDateStr = _seqInfo.ViewingDate.ToShortDateString() + " " + _seqInfo.ViewingDate.ToShortTimeString();
                    }
                    else
                    {
                        Info.ViewingDateStr = string.Empty;
                    }
                    #endregion

                    listobj.Add(Info);
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

        #endregion
    }
}
