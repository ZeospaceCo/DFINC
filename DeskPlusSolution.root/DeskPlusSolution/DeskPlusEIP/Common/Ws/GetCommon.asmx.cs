using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Common.Web.Ws
{
    /// <summary>
    /// GetCommon의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetCommon : System.Web.Services.WebService
    {

        #region 함수

        private string decryptString = string.Empty;//복고화 파리미터값

        /// <summary>
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        /// <param name="_encParam"></param>
        /// <returns></returns>
        protected void SetDecryptString(string _encParam)
        {
            if (!string.IsNullOrEmpty(_encParam))
            {
                _encParam = Server.UrlDecode(_encParam);//디코딩
                decryptString = Utility.DecryptString(_encParam);//복고화 하여 복고 전역변수에 할당하기
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryptString))
            {
                return UtilHelper.GetParmforParameter(decryptString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }



        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        #endregion

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : js 에서 파라미터 암호화 하기 - TripleDES 암호화 (168 byte 가능 한글 84자까지)
        /// </summary>
        /// <param name="_data">The _data.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AjaxGetEncrypt(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                string UrlEnc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncryptURLParameter", "N");

                if (UrlEnc == "Y")
                {
                    data = Server.UrlDecode(data);
                    TripleDESManager tds = new TripleDESManager();
                    return tds.Encrypt(data.Trim(), System.Text.Encoding.UTF8);
                }
                else
                {
                    data = Server.UrlDecode(data);
                    data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
                    return data.Replace("+", "~plus~");
                }
            }
            catch
            {
                return "!NULL";
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 비동기로 ascx 호출하여 값 반환하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AjaxGetUC(string p)
        {

            SetDecryptString(p);
            string ucPath = GetDecryptParam("ucPath", "");

            Page page = new Page();

            UserControl userControl = (UserControl)page.LoadControl(ucPath);

            HtmlForm form = new HtmlForm();
            form.Controls.Add(userControl);

            page.Controls.Add(form);

            StringWriter textWriter = new StringWriter();
            HttpContext.Current.Server.Execute(page, textWriter, false);

            return CleanHtml(textWriter.ToString());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : form 태그 제거하기
        /// </summary>
        private string CleanHtml(string html)
        {
            return Regex.Replace(html, @"<[/]?(form)[^>]*?>", "", RegexOptions.IgnoreCase);
        }




    }
}
