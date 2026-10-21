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

namespace Zeospace.DeskPlus.Approval.Web.Process.Afterward
{
    public partial class ConfirmSeqLine : Base.BasePageApproval
    {
        #region 변수설정
        public int DocumentID = 0;
        public int Sequence = 0;
        public string ApprovalTag = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Afterward.ConfirmSeqLine).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                GetSeqLine(DocumentID, Sequence, ApprovalTag);
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParam()
        {
            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            ApprovalTag = GetDecryptParam("approvalTag", "");
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

        private void GetSeqLine(int _documentID, int _sequence, string _approvalTag)
        {
            if (_documentID.Equals(0) || _sequence.Equals(0) || _approvalTag.Equals("")) return;

            DAL.Sequence seqDAl = new DAL.Sequence();
            List<BE.SequenceInfo> list = seqDAl.GetZAUserList(CurrentUserInfo.CompanyID, DocumentID, _sequence, _approvalTag);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='table_typeA'>");
            sb.Append("<colgroup>");
            sb.Append("<col width='35px' />");
            sb.Append("<col width='70px' />");
            sb.Append("<col />");
            sb.Append("<col width='120px' />");
            sb.Append("<col width='90px' />");
            sb.Append("</colgroup>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th></th>");
            sb.Append("<th>");
            sb.Append(this.GetResource("TitSec"));
            sb.Append("</th>");
            sb.Append("<th>");
            sb.Append(this.GetResource("DeptName"));
            sb.Append("</th>");
            sb.Append("<th>");
            sb.Append(this.GetResource("TitleName"));
            sb.Append("/");
            sb.Append(this.GetResource("DutyName"));
            sb.Append("</th>");
            sb.Append("<th>");
            sb.Append(this.GetResource("Name"));
            sb.Append("</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            int i = 0;
            string targetInfo = "";
            foreach (BE.SequenceInfo info in list)
            {
                //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag
                //oDocID, oAccountTag, oNextTag, oNextSeq, oState;
                targetInfo = info.DocumentID.ToString() + "-" + info.Sequence.ToString() + "-" + info.ApprovalTag;

                sb.Append("<tr>");
                sb.Append("<td class='center'><input type='radio' name='rblSeqList' value='");
                sb.Append(targetInfo);
                sb.Append("'");
                if (i.Equals(0))
                {
                    sb.Append(" checked");
                }
                sb.Append("/></td>");
                sb.Append("<td class='center'>[");
                sb.Append(GetTagName(info.ApprovalTag));
                sb.Append("]</td>");
                sb.Append("<td class='center'>");
                sb.Append(info.DeptBase);
                sb.Append("</td>");
                sb.Append("<td class='center'>");
                sb.Append(info.TitleBase);
                sb.Append("/");
                sb.Append(info.DutyBase);
                sb.Append("</td>");
                sb.Append("<td class='center'>");
                sb.Append(info.NameBase);
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
                    default:
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