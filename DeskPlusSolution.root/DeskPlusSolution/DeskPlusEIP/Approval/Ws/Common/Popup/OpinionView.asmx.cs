using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Common.Popup
{
    /// <summary>
    /// OpinionView의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class OpinionView : System.Web.Services.WebService
    {
        private string decryptString = string.Empty;//복고화 파리미터값
        private string preLang = "";

        #region GET()

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2022-01-04
        /// 작성목적 : 결재의견 조회
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetOpinionList(string p)
        {
            try
            {
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                string upPathID = GetDecryptParam("upPathID", "0");
                upPathID = upPathID.TrimStart(',');

                ObjList listobj = new ObjList();

                DAL.ShortReply dal = new DAL.ShortReply();
                DataTableReader dtr = dal.GetShortReplyList(companyID, upPathID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ShortReplyInfo info = new ShortReplyInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                            info.Section = dtr["Section"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLang, true);
                            else
                                info.WriteTimeStr = string.Empty;

                            info.Content = dtr["Content"].ToString().Replace("\n", "<br>").ToReverseXSS();//제목

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
        /// 작성자   : kkzz52
        /// 작성일자 : 2022-01-04
        /// 작성목적 : 결재의견 조회 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetOpinionListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int childDocID = GetDecryptParam("childDocID", "0").ToNumber<int>();

                string upPathID = string.Empty;
                string result = "FALSE";

                DAL.DocumentProcess dalDocProcess = new DAL.DocumentProcess();

                List<BE.DocumentProcessInfo> alDoc = dalDocProcess.GetDocumentUnion(companyID, childDocID);

                foreach (var docInfo in alDoc)
                {
                    upPathID = docInfo.UpPathID;
                }

                if (upPathID != "")
                {
                    upPathID = upPathID.TrimStart(',');

                    DAL.ShortReply dal = new DAL.ShortReply();
                    DataTableReader dtr = dal.GetShortReplyList(companyID, upPathID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            result = "TRUE";
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2022-01-04
        /// 작성목적 : 결재의견 조회 카운트
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetOpinionCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int childDocID = GetDecryptParam("childDocID", "0").ToNumber<int>();

                string upPathID = string.Empty;
                int count = 0;

                DAL.DocumentProcess dalDocProcess = new DAL.DocumentProcess();
                List<BE.DocumentProcessInfo> alDoc = dalDocProcess.GetDocumentUnion(companyID, childDocID);

                foreach (var docInfo in alDoc)
                {
                    upPathID = docInfo.UpPathID;
                }

                if (!string.IsNullOrWhiteSpace(upPathID))
                {
                    upPathID = upPathID.TrimStart(',');

                    DAL.ShortReply dal = new DAL.ShortReply();
                    using (DataTableReader dtr = dal.GetShortReplyList(companyID, upPathID))
                    {
                        if (dtr != null)
                        {
                            while (dtr.Read())
                            {
                                count++;
                            }
                        }
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 기타

        /// <summary>
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        /// <param name="_encParam"></param>
        /// <returns></returns>
        protected void SetDecryptString(string _encParam)
        {
            if (!string.IsNullOrEmpty(_encParam))
            {
                _encParam = Server.UrlDecode(_encParam);
                decryptString = Utility.DecryptString(_encParam);
                decryptString = Server.UrlDecode(decryptString);
            }
        }


        /// <summary>
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryptString))
            {
                return GetParmforParameter(_keyName, _errorReturn);
            }

            return _errorReturn;

        }

        /// <summary>
        /// 작성목적 : 파리미터값으로 키의 값 가져오기
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParmforParameter(string _parmName, string _errorReturn)
        {
            try
            {
                if (string.IsNullOrEmpty(decryptString) || decryptString == "!NULL") return _errorReturn;
                string response = String.Empty;

                decryptString = decryptString.Replace("&amp;", "~@~dp~@~");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                decryptString = decryptString.ToReverseXSS();

                string[] pairs = decryptString.Split('&');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string tmpPair = String.Empty;
                foreach (string pair in pairs)
                {
                    if (String.IsNullOrEmpty(pair) || pair.ToNull() == "") continue;

                    tmpPair = pair.Replace("~@~dp~@~", "&");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                    string[] paramvalue = tmpPair.Split('=');

                    if (dic.ContainsKey(paramvalue[0]) == false && paramvalue[0].ToNull() != "")
                    {
                        if (paramvalue.Length > 2) paramvalue[1] = tmpPair.Substring(tmpPair.IndexOf('=') + 1).Trim();    //특수문자 이후 사라지는 문제 ByFreeman 20131031
                        dic.Add(paramvalue[0], paramvalue[1]);
                    }
                }

                if (dic.ContainsKey(_parmName)) response = dic[_parmName];

                return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("decryptString : " + decryptString, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_parmName : " + _parmName, AppSettingType.EIP);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
            }
        }

        /// <summary>
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

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>The pre lang.</value>
        /// <remarks></remarks>
        public string PreLang
        {
            get { return preLang; }
            set
            {
                string lang = "";

                if (value == "")
                {
                    HttpContext context = HttpContext.Current;
                    lang = context.Request.UserLanguages[0].ToLower();
                }
                else
                {
                    lang = value.ToLower();
                }

                if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans" || lang == "zh-sg")  //zh-Hans(중국어간체)
                {
                    lang = "zh-cn";
                }
                else if (lang == "zh-tw" || lang == "zh-hant" || lang == "zh-mo")  //번체
                {
                    lang = "zh-tw";
                }
                else if (lang == "ja" || lang == "ja-jp") //일본어
                {
                    lang = "ja";
                }
                else if (lang == "ko" || lang == "ko-kr") //한국어
                {
                    lang = "ko";
                }
                else //나머지는 영어
                {
                    lang = "en";
                }

                preLang = lang;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// 특정 언어에 값이 없으면 base 값이 기본값으로 반환
        /// </summary>
        /// <param name="_base">The _base.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        private string GetLangName(string _base, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            string lang = "";

            _base = _base.ToNull();
            _ko = _ko.ToNull();
            _en = _en.ToNull();
            _ja = _ja.ToNull();
            _fr = _fr.ToNull();
            _zhcn = _zhcn.ToNull();
            _zhtw = _zhtw.ToNull();

            switch (_preLang.ToLower())
            {
                case "ko":
                    lang = _ko;
                    break;

                case "en":
                    lang = _en;
                    break;

                case "ja":
                    lang = _ja;
                    break;

                case "fr":
                    lang = _fr;
                    break;

                case "zh-cn":
                    lang = _zhcn;
                    break;

                case "zh-tw":
                    lang = _zhtw;
                    break;
            }

            //특정 언어에 값이 없으면 base 값이 기본값으로 반환
            return (lang.Length > 0) ? lang : _base;
        }

        #endregion
    }
}
