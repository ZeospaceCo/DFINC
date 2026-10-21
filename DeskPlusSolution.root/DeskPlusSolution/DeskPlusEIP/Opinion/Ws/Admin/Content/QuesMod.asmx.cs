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
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content
{
    /// <summary>
    /// QuesMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class QuesMod : Base.BaseWSOpinion
    {
        #region GET

        /// <summary>
        /// 특정설문의 설문대상자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetTargetList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.QuesMod).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Target tDal = new DAL.Target();
            DataTableReader dtr = tDal.GetTargetList(CurrentUserInfo.CompanyID, quesID);

            List<object> tobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.TargetInfo tInfo = new BE.TargetInfo();
                        tInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        tInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        tInfo.TargetID = dtr["TargetID"].ToString().ToNumber<int>();
                        tInfo.OwnerID = dtr["OwnerID"].ToString().ToNumber<int>();
                        tInfo.Type = dtr["Type"].ToString();
                        tInfo.TypeStr = (tInfo.Type.ToUpper().Equals("M")) ? resMng.GetString("Private") : resMng.GetString("Group");
                        tInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        tInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);
                        tInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);
                        tInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                        tInfo.FileDel = resMng.GetString("BtnDel");

                        tobj.Add(tInfo);
                    }
                }
            }
            return tobj;
        }

        /// <summary>
        /// 특정설문의 설문관리자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetAdminList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.QuesMod).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Admin aDal = new DAL.Admin();
            DataTableReader dtr = aDal.GetAdminList(CurrentUserInfo.CompanyID, quesID);

            List<object> aobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AdminInfo aInfo = new BE.AdminInfo();
                        aInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        aInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        aInfo.AdminID = dtr["AdminID"].ToString().ToNumber<int>(); //modify shimpark
                        aInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        aInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        aInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);
                        aInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);
                        aInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                        aInfo.FileDel = resMng.GetString("BtnDel");

                        aobj.Add(aInfo);
                    }
                }
            }
            return aobj;

        }

        /// <summary>
        /// 특정설문의 질문리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetQuesUnittList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.QuesMod).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.QuesUnit quDal = new DAL.QuesUnit();
            DataTableReader dtr = quDal.GetQuesUnitList(CurrentUserInfo.CompanyID, quesID);

            List<object> quobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                        quInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        quInfo.QuesUnitID = dtr["QuesUnitID"].ToString().ToNumber<int>();
                        quInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        quInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        quInfo.Title = dtr["Title"].ToString();
                        quInfo.IsReq = dtr["IsReq"].ToString();
                        quInfo.IsReqStr = (quInfo.IsReq.Equals("1")) ? resMng.GetString("TitReq") : resMng.GetString("TitNoReq");
                        quInfo.ItemCnt = dtr["ItemCnt"].ToString();
                        quInfo.FileDel = resMng.GetString("BtnDel");

                        quobj.Add(quInfo);
                    }
                }
            }
            return quobj;
        }

        /// <summary>
        /// 특정설문의 파일리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetAttachList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content.QuesMod).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Attach attDal = new DAL.Attach();
            DataTableReader dtr = attDal.GetAttachList(CurrentUserInfo.CompanyID, quesID);

            List<object> attobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AttachInfo attInfo = new BE.AttachInfo();
                        attInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        attInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        attInfo.AttachID = dtr["AttachID"].ToString().ToNumber<int>();
                        attInfo.FilePath = dtr["FilePath"].ToString();
                        attInfo.FileName = dtr["FileName"].ToString();
                        attInfo.FileType = dtr["FileType"].ToString();
                        attInfo.FileLength = dtr["FileLength"].ToString().ToNumber<int>();
                        attInfo.FileGUID = dtr["FileGUID"].ToString();
                        attInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(dtr["FileLength"].ToString().ToNumber<int>()) / 1024) + " KB";
                        attInfo.FileDel = resMng.GetString("BtnDel");

                        attobj.Add(attInfo);
                    }
                }
            }
            return attobj;

        }


        #endregion

        #region SET

        /// <summary>
        /// 첨부파일삭제(건당)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttach(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int attachID = GetDecryptParam("attachID", "0").ToNumber<int>();

            BLL.Attach aBLL = new BLL.Attach();
            return aBLL.Del(CurrentUserInfo.CompanyID, quesID, attachID);
        }

        /// <summary>
        /// 설문-질문추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool InsQuesUnit(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            string arrID = GetDecryptParam("arrID", "").Trim(',');
            BLL.QuesUnit quBLL = new BLL.QuesUnit();
            return quBLL.Add(CurrentUserInfo.CompanyID, quesID, arrID);
        }

        /// <summary>
        /// 설문-질문삭제(건당)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelQuesUnit(string p)
        {
            SetDecryptString(p);
            int quesUnitID = GetDecryptParam("quID", "0").ToNumber<int>();
            BLL.QuesUnit quBLL = new BLL.QuesUnit();
            return quBLL.Del(CurrentUserInfo.CompanyID, quesUnitID);
        }

        /// <summary>
        /// 설문-대상자추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool InsTarget(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            string arrID = GetDecryptParam("arrID", "").Trim(',');
            BLL.Target tBLL = new BLL.Target();
            return tBLL.Add(CurrentUserInfo.CompanyID, quesID, arrID);
        }
        /// <summary>
        /// 설문-대상자삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelTarget(string p)
        {
            SetDecryptString(p);
            int targetID = GetDecryptParam("targetID", "0").ToNumber<int>();
            BLL.Target tBLL = new BLL.Target();
            return tBLL.Del(CurrentUserInfo.CompanyID, targetID);
        }

        /// <summary>
        /// 설문-관리자추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool InsAdmin(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            string arrUnitID = GetDecryptParam("arrID", "").Trim(',');
            BLL.Admin aBLL = new BLL.Admin();
            return aBLL.Add(CurrentUserInfo.CompanyID, quesID, arrUnitID);
        }
        /// <summary>
        /// 설문-관리자삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAdmin(string p)
        {
            SetDecryptString(p);
            int adminID = GetDecryptParam("adminID", "0").ToNumber<int>();
            BLL.Admin aBLL = new BLL.Admin();
            return aBLL.Del(CurrentUserInfo.CompanyID, adminID);
        }
        #endregion

    }
}
