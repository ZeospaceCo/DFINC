using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// MyMemo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MyMemo : Base.BaseWSPortlet
    {
        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 메모 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MyMemoAdd(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            string myMemo = GetDecryptParam("myMemo", "").ToString();  

            try
            {
                DAL.MyMemo biz = new DAL.MyMemo();
                HttpContext context = HttpContext.Current;
                myMemo = context.Server.UrlEncode(myMemo);
                biz.MemoAdd(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, myMemo);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MyMemoAdd : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 환경설정 저장
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool EnvSettingAdd(string p)
        {
            try 
            { 
                //파라미터 가져오기
                SetDecryptString(p);

                string myMemoTitle = GetDecryptParam("myMemoTitle", "").ToString();
                string fontColor = GetDecryptParam("fontColor", "").ToString();
                string backColor = GetDecryptParam("backColor", "").ToString();
                
                DAL.MyMemo biz = new DAL.MyMemo();

                HttpContext context = HttpContext.Current;
                myMemoTitle = context.Server.UrlEncode(myMemoTitle);
                biz.EnvAdd(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, myMemoTitle, fontColor, backColor);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EnvSettingAdd : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 메모 정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.MyMemoInfo GetMemoInfo()
        {
            try
            {
                DAL.MyMemo biz = new DAL.MyMemo();
                BE.MyMemoInfo info = new BE.MyMemoInfo();

                DataTableReader dtr = biz.GetMyMemoInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            HttpContext context = HttpContext.Current;

                            info.MemoID = dtr["MemoID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString().ToReverseXSS();           // 제목(상단바 제목)
                            info.Title = context.Server.UrlDecode(info.Title);
                            info.Memo = dtr["Memo"].ToString().ToReverseXSS();             // 메모
                            info.Memo = context.Server.UrlDecode(info.Memo);
                            info.Color = dtr["Color"].ToString();           // 글자색
                            info.BackColor = dtr["BackColor"].ToString();   // 배경색
                        }
                    }
                }

                return info;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        
    }
}
