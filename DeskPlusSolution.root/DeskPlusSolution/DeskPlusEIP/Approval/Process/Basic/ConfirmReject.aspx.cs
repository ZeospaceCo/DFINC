using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Process.Basic
{
    public partial class ConfirmReject : Base.BasePageApproval
    {
        #region 변수설정
        public int DocumentID = 0;
        public int GroupID = 0;
        public string IsType = "";  // 1 : 일반결재시의 반려대상자, 2 : 수신결재의 반려대상자  
        public string IsApprovePwd = "N";
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Basic.ConfirmReject).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                this.tbOpinion.Attributes.Add("maxlength", "500");

                LoadParam();
                SetLanguage();
                GetReject(DocumentID, GroupID, IsType);
                LoadData(); // 설정정보 가져오기
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            // 1. 패스워드사용유무 정보
            Zeospace.DeskPlus.Account.DAL.UserEnvironment uDAL = new Account.DAL.UserEnvironment();
            List<Account.BE.UserEnvironmentInfo> list = uDAL.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (list != null)
            {
                foreach (Account.BE.UserEnvironmentInfo info in list)
                {
                    IsApprovePwd = (info.IsApprovePwd.Equals("")) ? "N" : info.IsApprovePwd;
                }
            }
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParam()
        {
            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            IsType = GetDecryptParam("isType", "");
        }
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        private void GetReject(int _documentID, int _groupID, string _isType)
        {
            if (_documentID.Equals(0) || _groupID.Equals(0) || _isType.Equals("")) return;

            DAL.Sequence seqDAl = new DAL.Sequence();
            List<BE.SequenceInfo> list = seqDAl.GetReject(CurrentUserInfo.CompanyID, DocumentID, GroupID, IsType);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table_typeT'>");
            sb.Append("<colgroup>");
            sb.Append("<col width='35px' />");
            sb.Append("<col width='70px' />");
            sb.Append("<col />");
            sb.Append("<col width='120px' />");
            sb.Append("<col width='90px' />");
            sb.Append("<col width='90px' />");
            sb.Append("</colgroup>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th></th>");
            sb.Append("<th class='center'>");
            sb.Append(this.GetResource("TitSec"));
            sb.Append("</th>");
            sb.Append("<th class='center'>");
            sb.Append(this.GetResource("DeptName"));
            sb.Append("</th>");
            sb.Append("<th class='center'>");
            sb.Append(this.GetResource("TitleName"));
            sb.Append("</th>");
            sb.Append("<th class='center'>");
            sb.Append(this.GetResource("Name"));
            sb.Append("</th>");
            sb.Append("<th class='center'>");
            sb.Append(this.GetResource("ApproveDate"));
            sb.Append("</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            int i = 0; 
            string targetInfo = "";
            foreach (BE.SequenceInfo info in list)
            {
                DateTime dt;

                if (DateTime.TryParse(info.ApprovalDate.ToString(), out dt))
                    info.ApprovalDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                else
                    info.ApprovalDateStr = string.Empty;

                //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID-[4]ApprovalType
                targetInfo = info.DocumentID.ToString() + "-" + info.Sequence.ToString() + "-" + info.ApprovalTag + "-" + info.UserID.ToString() + "-" + info.ApprovalType;
                
                sb.Append("<tr>");
                sb.Append("<td class='center'><input type='radio' name='rblReject' value='");
                sb.Append(targetInfo);
                sb.Append("'");
                /* 반려대상 상신자 초기 체크로직 해제 요청
                if (i.Equals(0)) 
                {
                    sb.Append(" checked");
                }*/

                /*
                if (info.ApprovalTag.Equals("ZA").Equals(0))    //병렬합의자에게는 반려불가.
                {
                    sb.Append(" disable");
                }
                */

                if (info.ApprovalTag.IndexOf("ZA") == 0)
                {
                    sb.Append(" disabled");
                }

                sb.Append("/></td>");
                sb.Append("<td class='center'>[");
                sb.Append(GetTagName(info.ApprovalTag));
                sb.Append("]</td>");
                sb.Append("<td class='center'>");
                sb.Append(GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang));
                sb.Append("</td>");
                sb.Append("<td class='center'>");
                sb.Append(GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang));
                sb.Append("</td>");
                sb.Append("<td class='center'>");
                sb.Append(GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang));
                sb.Append("</td>");
                sb.Append("<td class='center'>");
                sb.Append(info.ApprovalDateStr);
                sb.Append("</td>");
                sb.Append("</tr>");

                i++;
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            lbContent.Text = sb.ToString();

        }
        /// <summary>
        /// 결재태그
        /// </summary>
        /// <param name="_approvalTag"></param>
        /// <returns></returns>
        private string GetTagName(string _approvalTag)
        {
            if (_approvalTag.ToUpper().Equals("ZD0"))
            {
                return this.GetResource("TagDic_ZD0");
            }
            else
            {
                switch (_approvalTag.Substring(0, 2))
                {
                    case "ZD":
                        return this.GetResource("R_ZD");
                    case "ZH":
                        return this.GetResource("R_ZH");
                    case "ZA":
                        return this.GetResource("R_ZA");
                    case "ZR":
                        return this.GetResource("R_ZR");
                    default :
                        return this.GetResource("R_ZD");

                }
            }
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}