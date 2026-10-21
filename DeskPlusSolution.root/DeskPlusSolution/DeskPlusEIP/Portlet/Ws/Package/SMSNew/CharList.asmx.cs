using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ObjList = System.Collections.Generic.List<System.Object>;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.SMSNew
{
    /// <summary>
    /// CharList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class CharList : Base.BaseWSPortlet
    {

        /// <summary>
        /// Gets the page list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try {

                SetDecryptString(p);

                Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                smsInfo.CompanyID   = CurrentUserInfo.CompanyID;
                smsInfo.WriteUserID = CurrentUserInfo.UserID;
                smsInfo.Gubun       = "0";

                Zeospace.DeskPlus.Component.SMS.SMSHelper dal = new Component.SMS.SMSHelper();
                DataSet ds = dal.GetSMSTemplateNotice(smsInfo);
                ObjList listobj = new ObjList();
                if(ds == null) return listobj;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;

                    for (int i = 0; i < dv.Count; i++)
                    {   
                        BE.CharListInfo info = new BE.CharListInfo();
                        info.CompanyID = dv[i]["CompanyID"].ToString().ToNumber<int>();
                        info.Gubun = dv[i]["Gubun"].ToString();
                        info.SMSNoticeTemplateID = dv[i]["SMSNoticeTemplateID"].ToString().ToNumber<int>();
                        info.WriteUserID = dv[i]["WriteUserID"].ToString().ToNumber<int>();
                        info.Content = dv[i]["Content"].ToString();
                        //info.ContentDecode =  Server.UrlEncode(dv[i]["Content"].ToString());
                        info.ContentDecode = dv[i]["Content"].ToString();

                        if(CharLen(info.Content) > 18)   
                            info.Content = CutCharLen(info.Content,18) + "..";

                        listobj.Add(info);
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
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
        private String CutCharLen(String _value, int limit)
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


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 문자 저장 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try {

                SetDecryptString(p);

                string id = GetDecryptParam("id", "");

                if (id.Length > 0)
                {
                    Component.SMS.SMSHelper dal = new Component.SMS.SMSHelper();

                    foreach (string val in id.Split(','))
                    {
                        if (val.Length > 0)
                        {                            
                            Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                            smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                            smsInfo.WriteUserID = CurrentUserInfo.UserID;
                            smsInfo.SMSNoticeTemplateID = val.ToNumber<int>();

                            dal.DelSMSTemplateNotice(smsInfo);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex.Message);
                return false;
            }
        }
    }
}
