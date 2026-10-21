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
using System.Text;
using System.Xml;

namespace Zeospace.DeskPlus.Common.Web.Ws.ZipCode
{
    /// <summary>
    /// ZipCode의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ZipCode : Base.BaseWSCommon
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 구우편번호 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetZipCode(string p)
        {
            SetDecryptString(p);
            string siName = GetDecryptParam("siName", "");
            string guName = GetDecryptParam("guName", "");
            string dongName = GetDecryptParam("dongName", "0");
            string gubun = GetDecryptParam("gubun", "1");

            Common.DAL.ZipCode dal = new DAL.ZipCode();
            DataTableReader dtr = dal.GetZipCode(CurrentUserInfo.CompanyID, siName, guName,dongName, gubun);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ZipCodeInfo info = new BE.ZipCodeInfo();
                        info.ZipCode = dtr["ZipCode"].ToString();
                        info.Si = dtr["Si"].ToString();
                        info.Gu = dtr["Gu"].ToString();
                        info.Dong = dtr["Dong"].ToString();
                        info.Ri = dtr["Ri"].ToString();
                        info.Building = dtr["nm_bldg"].ToString().Replace(",","").Replace("'","");
                        info.Bunji = dtr["Bunji"].ToString();
                        info.Num_Bldg = dtr["num_bldg"].ToString();

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 신우편번호 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetZipCodeNew(string p)
        {
            SetDecryptString(p);
            string dongName = GetDecryptParam("dongName", "0");

            Common.DAL.ZipCodeNew dal = new DAL.ZipCodeNew();
            DataTableReader dtr = dal.GetZipCode(dongName);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ZipCodeNewInfo info = new BE.ZipCodeNewInfo();
                        info.ZipCode = dtr["ZipCode"].ToString().Substring(0, 3) + "-" + dtr["ZipCode"].ToString().Substring(3, 3);
                        info.Do = dtr["Do"].ToString();
                        info.Si = dtr["Si"].ToString();
                        info.Street = dtr["Street"].ToString();
                        info.BuildNum1 = dtr["BuildNum1"].ToString();
                        info.BuildNum2 = dtr["BuildNum2"].ToString();

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 시 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSiCode()
        {
            Common.DAL.ZipCode dal = new DAL.ZipCode();
            DataTableReader dtr = dal.GetSiCode(CurrentUserInfo.CompanyID);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ZipCodeInfo info = new BE.ZipCodeInfo();
                        info.Si = dtr["Si"].ToString();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 선택한 시에 따른 구 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetGuCode(string p)
        {
            SetDecryptString(p);
            string siName = GetDecryptParam("siName", "");
            string gubun = GetDecryptParam("gubun", "1");

            Common.DAL.ZipCode dal = new DAL.ZipCode();
            DataTableReader dtr = dal.GetGuCode(CurrentUserInfo.CompanyID, siName, gubun);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ZipCodeInfo info = new BE.ZipCodeInfo();

                        info.Gu = dtr["Gu"].ToString();
                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : shine
        /// 도로명주소에서 ID 값으로 주소 가져오기(시,구,동...)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetNewAddress(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("code", "");

            Common.DAL.ZipCode dal = new DAL.ZipCode();
            DataTableReader dtr = dal.GetNewAddress(CurrentUserInfo.CompanyID, code);

            List<object> listobj = new List<object>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ZipCodeInfo info = new BE.ZipCodeInfo();

                        info.ZipCode = dtr["ZipCode"].ToString();
                        info.Si = dtr["Si"].ToString();
                        info.Gu = dtr["Gu"].ToString();
                        info.Dong = dtr["Dong"].ToString();
                        info.Ri = dtr["Ri"].ToString();
                        info.Building = dtr["nm_bldg"].ToString().Replace(",", "").Replace("'", "");
                        info.Bunji = dtr["Bunji"].ToString();
                        
                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 서울시에서 제공한 도로명주소 체계 (서울시만 제공함)
        /// 작성일자 : 2013-04-17
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ZipCodeNewInfo> GetNewAddressSeoul(string search)
        {
            /* 우체국은 지원하고 있지만 제대로 주소값을 주지 않음.(빈결과값.. 서비스를 제대로 처리 안함) 인증키 (3f13d68788c167fa51366179893419)
             * 우체국 : http://biz.epost.go.kr/KpostPortal/openapi?regkey=3f13d68788c167fa51366179893419&target=post&query=7Iug66a864+Z
             */
            

            //서울시에서 제공한 api : 인증키 (65616da5db12fa0ea35e0c33f7a5efaa)
            var url = "http://juso.seoul.go.kr/OpenApi/Search.aspx?key=65616da5db12fa0ea35e0c33f7a5efaa&target=newaddress&emdcd=11680103&rdnm=%EA%B0%9C%ED%8F%AC%EB%A1%9C&jibun1=&jibun2=&page=1&rpp=10";
            
            XmlDocument xml = new XmlDocument();
            xml.Load(url);
            XmlNodeList xnList = xml.SelectNodes("/BUILDINGS/BUILDING"); //접근할 노드

            var listobj = new List<BE.ZipCodeNewInfo>();

            foreach (XmlNode xn in xnList)
            {
                BE.ZipCodeNewInfo info = new BE.ZipCodeNewInfo();

                string SGG_NM = xn["SGG_NM"].InnerText;
                string BD_NM = xn["BD_NM"].InnerText;
                string BD_MGT_SN = xn["BD_MGT_SN"].InnerText;

            }

            return listobj;
        }

    }
}
