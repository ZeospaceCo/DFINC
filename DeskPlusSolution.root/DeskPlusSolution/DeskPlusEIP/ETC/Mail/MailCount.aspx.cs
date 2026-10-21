using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.Mail
{
    public partial class MailCount : System.Web.UI.Page
    {
        #region 변수설정
        public string xmlData;
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
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {

            string postURL = "http://mail.wooree.co.kr/mailGate.ds";
            string MailID = "";
            string MailDomain = "";
            int rtnval = 0;
            MailID = GetParm("MailID", "").ToString();
            MailDomain = GetParm("MailDomain", "").ToString();
            string postParm = "act=mailCount&domain=" + MailDomain + "&id=" + MailID + "&mbox=inbox&type=4";

            Zeospace.DeskPlus.Common.BLL.MailDeep55 bll = new Zeospace.DeskPlus.Common.BLL.MailDeep55();
            XmlNode rootNode = bll.SendPost("MailCount", postURL, postParm);
            if (rootNode != null)
            {
                rtnval = rootNode.SelectSingleNode("/data/result").InnerText.ToNumber<int>();
            }


            xmlData = "<?xml version='1.0' encoding='utf-8' ?><COUNTLIST><SYSTEMNAME>MAIL</SYSTEMNAME><COUNT>" + rtnval + "</COUNT></COUNTLIST>";
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}