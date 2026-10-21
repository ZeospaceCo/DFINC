using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;
using System.Xml;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws.Data
{
    public class List
    {
        /// <summary>
        /// 작성목적 : 총 게시물 카운트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_mailDomain"></param>
        /// <param name="_mailID"></param>
        /// <param name="_mailPassword"></param>
        /// <param name="_code"></param>
        /// <returns></returns>
        public int GetListCount(string _companyID, string _mailDomain, string _mailID, string _mailPassword, string _code, string _searchValue)
        {
            try
            {
                int tmpCount = 0;

                string paramMailCount = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailCount");                
                string param = string.Format(paramMailCount, _mailDomain, _mailID, _mailPassword, _code, "0", "all", Helper.Security.AES.Base64Convert.Encode(_searchValue));
                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");

                string sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);                

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXmlData);

                string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if (returnCode == "0")
                {
                    //성공                    
                    tmpCount = int.Parse(xml.SelectSingleNode("/data/result").InnerText);
                }
                else
                {
                    //실패
                }

                return tmpCount;

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetListCount: " + errMsg);
                throw;
            }
        }        

    }
}

