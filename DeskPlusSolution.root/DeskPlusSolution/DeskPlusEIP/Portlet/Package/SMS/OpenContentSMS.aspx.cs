using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class OpenContentSMS : Base.BasePagePortlet
    {
        public string htmlString = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMS.OpenContentSMS).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 저장된 문자 가져오기
		/// 작성자     : 심재운
		/// 최초작성일 : 2008-02-05
        /// </summary>
        private void LoadData()
        {
            Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new SMSInfo();            
            smsInfo.CompanyID   = CurrentUserInfo.CompanyID;
            smsInfo.WriteUserID = CurrentUserInfo.UserID;
            smsInfo.Gubun       = "0";            

            StringBuilder sb = new StringBuilder();

            Zeospace.DeskPlus.Component.SMS.SMSHelper dal = new SMSHelper();

            DataSet ds = dal.GetSMSTemplateNotice(smsInfo);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                string smsNoticeTemplateID = String.Empty;
                string content          = String.Empty;
                string contentDecode    = String.Empty;                
                int num;


                for (int i = 0; i < dv.Count; i++)
                {
                    num = i + 1;
                    smsNoticeTemplateID = dv[i]["SMSNoticeTemplateID"].ToString();                    
                    
                    if(CharLen(dv[i]["Content"].ToString()) > 18)
                    {
                        content = CutCharLen(dv[i]["Content"].ToString(),18);
                        content += "..";
                    }
                    else
                    {
                        content = dv[i]["Content"].ToString();
                    }
                    
                    
                    contentDecode       = Server.UrlEncode(dv[i]["Content"].ToString());

                    sb.Append("<tr id='opencontent_tr_");
                    sb.Append(smsNoticeTemplateID);
                    sb.Append("' onmouseover=inCell(this,'#E8EDFF'); onmouseout=outCell(this,'#FFFFFF'); ");
                    sb.Append("onclick=ifr_inputCell(this,'#bbbbbb','");
                    sb.Append(smsNoticeTemplateID);
                    sb.Append("','");
                    sb.Append(num);
                    sb.Append("','@");
                    sb.Append(contentDecode);
                    sb.Append("'); ");
                    sb.Append("ondblclick=ifr_openContent_sel_dblclk('");
                    sb.Append(contentDecode);
                    sb.Append("',''); style=cursor:hand;>");
                    sb.Append("<td width=160 style=font-size:8pt;padding-left:3px;>");
                    sb.Append(content);
                    sb.Append("</td>");
                    sb.Append("<td width=57 style=font-size:8pt;padding-left:3px;></td>");
                    sb.Append("</tr>");
                }
            }
            else
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }


            htmlString = sb.ToString();
        }

        /// <summary>
        /// 메서드명   : CutCharLen
        /// 작성목적   : 한글(2byte),숫자(1byte),영어(1byte) 에 맞게 잘라내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_value">글내용</param>
        /// <param name="limit">제한 글자수</param>
        /// <returns></returns>
        public String CutCharLen(String _value, int limit)
        {
            int strlen = 0;
            char c;
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < _value.Length; j++)
            {
                c = _value[j];

                //한글 코드의 시작과 끝 값이다
                if (c < 0xac00 || 0xd7a3 < c)
                {
                    strlen++;
                }
                else
                {
                    strlen += 2;  //한글이다..
                }

                if (strlen > limit)
                {
                    break;
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        } 


        /// <summary>
        /// 메서드명   : CharLen
        /// 작성목적   : 한글(2byte),숫자(1byte),영어(1byte) 글자수
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_value">글내용</param>
        /// <param name="limit">제한 글자수</param>
        /// <returns></returns>
        private int CharLen(String _value)
        {
            int strlen = 0;
            char c;
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < _value.Length; j++)
            {
                c = _value[j];

                //한글 코드의 시작과 끝 값이다
                if (c < 0xac00 || 0xd7a3 < c)
                {
                    strlen++;
                }
                else
                {
                    strlen += 2;  //한글이다..
                }
            }

            return strlen;
        }


        [ComponentArt.Web.UI.ComponentArtCallbackMethod]
        public string AjaxCallDecode(string _value)
        {
            return Server.UrlDecode(_value);
        }
    }
}
