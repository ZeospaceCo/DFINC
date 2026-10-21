using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// AppSettingHelper에 대한 요약 설명입니다.
    /// </summary>
    public abstract class AppSettingHelper : Helper.Base.BaseDAL
    {
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : Appsetting 의 xml 직접 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_key">The _key.</param>
        /// <param name="_exceptionString">The _exception string.</param>
        /// <returns></returns>
        public static string GetXmlAppSetting(AppSettingType _type, string _key, string _exceptionString)
        {
            if (string.IsNullOrEmpty(_key))
            {
                return "";
            }

            string appSettingValue = "";
            HttpContext context = HttpContext.Current;
            string targetPath = "";

            try
            {
                targetPath = "/DeskPlusEIP/App_Data/" + GetDescription(_type) + ".xml"; //xml 경로 가져오기

                if (targetPath == "") return appSettingValue; // Configuration XML 파일이 존재하지 않는다면.

                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(context.Server.MapPath(targetPath));

                    while (reader.Read())
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.GetAttribute("ID") != "")
                            {
                                if (reader.GetAttribute("ID") == _key)
                                {
                                    appSettingValue = reader.GetAttribute("Value").ToString().Trim();
                                    break;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                appSettingValue = (appSettingValue == "") ? _exceptionString : appSettingValue;
            }
            catch
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AppSettingHelper[GetAppSetting] - _type : " + _type + " : _key :" + _key);
                return _exceptionString;
            }

            return appSettingValue;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : DataSet 형 캐쉬 생성하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_key">The _key.</param>
        /// <param name="_cacheName">Name of the _cache.</param>
        private static void CreateCacheAppSetting(AppSettingType _type, string _key, string _cacheName)
        {
            string filePath = HttpContext.Current.Server.MapPath("/DeskPlusEIP/App_Data/" + GetDescription(_type) + ".xml");
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            HttpContext ctx = HttpContext.Current;
            ctx.Cache.Insert(_cacheName, ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            ds = null;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 캐쉬에서 GetAppSetting 값 가져오기, Exception 발생여부
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_key">The _key.</param>
        /// <param name="_exceptionString">xml id 값이 없을 경우 기본값</param>
        /// <returns></returns>
        public static string GetAppSetting(AppSettingType _type, string _key, string _exceptionString)
        {
            if (string.IsNullOrEmpty(_key)) return "";
            string appSettingValue = "";

            try
            {
                string cacheName = GetDescription(_type);

                if (HttpContext.Current.Cache[cacheName] == null) CreateCacheAppSetting(_type, _key, cacheName);

                HttpContext context = HttpContext.Current;
                DataSet ds = (DataSet)context.Cache[cacheName];

                if (ds != null)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "ID = '" + _key + "'";

                    try
                    {
                        foreach (DataRowView dr in dv) appSettingValue = dr["Value"].ToString();
                    }
                    catch (Exception ex)
                    {
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AppSettingHelper GetAppSetting : ", ex);
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AppSettingHelper GetAppSetting : _type : " + _type + " ,_key : " + _key + " , _string : " + _exceptionString, ex);

                        appSettingValue = GetXmlAppSetting(_type, _key, _exceptionString);
                    }

                    if (appSettingValue.Length == 0) appSettingValue = GetXmlAppSetting(_type, _key, _exceptionString);
                }

                if (appSettingValue == "") appSettingValue = _exceptionString;
                return appSettingValue;
            }
            catch
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AppSettingHelper[GetAppSetting] - _type : " + _type + " : _key :" + _key);
                return _exceptionString;
            }
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : ENUM 의 DESCRIPTION 값 가져오기
        /// 작성일자 : 2009-09-23
        /// </summary>
        /// <param name="en">The en.</param>
        /// <returns></returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 text 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-13
        /// 작성목적 : application cache deleted
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <remarks></remarks>
        public static void ClearApplication(AppSettingType _type)
        {
            HttpContext context = HttpContext.Current;
            context.Application.Lock();
            context.Application.Remove("DQ_" + GetDescription(_type));
            context.Application.UnLock();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 환경정보 application session 삭제
        /// </summary>
        /// <param name="_type">The _type.</param>
        public static void ClearConfigApplication(AppSettingType _type)
        {
            HttpContext context = HttpContext.Current;
            context.Application.Lock();
            context.Application.Remove("COM_ENV_" + GetDescription(_type));
            context.Application.UnLock();
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 환경정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_keyID">The _key ID.</param>
        /// <returns></returns>
        public static string GetEnvKeyValue(int _companyID, AppSettingType _type, string _keyID)
        {
            Hashtable hash = new Hashtable();
            string dqName = "COM_ENV_" + GetDescription(_type);//applicaton name
            string qurey = string.Empty;

            if (HttpContext.Current.Application[dqName] != null)
            {
                //get hash info from application name
                hash = (Hashtable)HttpContext.Current.Application[dqName];
            }
            else
            {
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL biz =
                //    new Base.BaseDAL();

                DataTableReader dtr = GetEnvQuery(_companyID, GetDescription(_type));

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            hash.Add(dtr["KeyID"].ToString(), dtr["KeyValue"].ToString());
                        }

                        HttpContext.Current.Application.Lock();
                        HttpContext.Current.Application[dqName] = hash;
                        HttpContext.Current.Application.UnLock();
                    }
                }
            }

            if (HttpContext.Current.Application[dqName] != null)
            {
                qurey = hash[_keyID].ToString();
            }

            return (qurey.Length == 0) ? "" : qurey;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2008-08-13
        /// 작성목적 : dynamic query 값 가져오기
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <param name="_qryID">The _qry ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDynamicQuery(AppSettingType _type, string _qryID)
        {
            if (_qryID.Length == 0) return "";

            Hashtable hash = new Hashtable();
            string dqName = "DQ_" + GetDescription(_type);//applicaton name
            string qurey = string.Empty;

            if (HttpContext.Current.Application[dqName] != null)
            {
                //get hash info from application name
                hash = (Hashtable)HttpContext.Current.Application[dqName];
            }
            else
            {
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL biz =
                //    new Base.BaseDAL();
                DataTableReader dtr = GetQuery(GetDescription(_type));

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            hash.Add(dtr["qryid"].ToString(), dtr["qryvalue"].ToString().Replace("\n\t", " ").Replace("\n", " "));
                        }
                    }

                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dqName] = hash;
                    HttpContext.Current.Application.UnLock();
                }
            }

            if (HttpContext.Current.Application[dqName] != null)
            {
                qurey = hash[_qryID].ToString();
            }

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("qry : " + _qryID + "=> " + qurey, AppSettingType.BASE);

            return (qurey.Length == 0) ? "" : qurey;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 그룹별 SYSCODE 값 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <returns></returns>
        public static DataView GetSysCode(int _companyID, string _parentCodeID)
        {
            DataSet ds = null;
            string dqName = "SysCode" + _companyID.ToString();//applicaton name
            string qurey = string.Empty;

            if (HttpContext.Current.Application[dqName] == null)
            {
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL biz =
                //    new Base.BaseDAL();
                DataSet dsList = GetSysCode(_companyID);

                if (dsList != null)
                {
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dqName] = dsList;
                    HttpContext.Current.Application.UnLock();
                }
            }

            //get hash info from application name
            ds = (DataSet)HttpContext.Current.Application[dqName];

            if (ds != null)
            {
                if (_parentCodeID.ToNull().Length > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "ParentCodeID='" + _parentCodeID + "'";
                    dv.Sort = "CodeOrder ASC";
                    return dv;
                }
            }

            return null;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : SYSCODE 에서 키값으로 키명(다국어) 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <param name="_prelang">The _prelang.</param>
        /// <returns></returns>
        public static string GetSysCode(int _companyID, string _codeID, string _parentCodeID, string _prelang)
        {
            if (_companyID == 0 || _codeID.ToNull().Length == 0 || _parentCodeID.ToNull().Length == 0 || _prelang.ToNull().Length == 0)
                return "";

            DataSet ds = null;
            string dqName = "SysCode" + _companyID.ToString();//applicaton name
            string qurey = string.Empty;
            string rtnVal = String.Empty;

            if (HttpContext.Current.Application[dqName] == null)
            {
                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL biz =
                //    new Base.BaseDAL();
                DataSet dsList = GetSysCode(_companyID);

                if (dsList != null)
                {
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dqName] = dsList;
                    HttpContext.Current.Application.UnLock();
                }
            }

            //get hash info from application name
            ds = (DataSet)HttpContext.Current.Application[dqName];

            if (ds != null)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "CodeID='" + _codeID + "' AND ParentCodeID='" + _parentCodeID + "'";

                for (int j = 0; j < dv.Count; j++)
                {
                    rtnVal = UtilHelper.GetLangName(
                        dv[j]["NameBase"].ToString(),
                        dv[j]["NameKo"].ToString(),
                        dv[j]["NameEn"].ToString(), dv[j]["NameJa"].ToString(),
                        dv[j]["NameFr"].ToString(), dv[j]["NameZhCn"].ToString(),
                        dv[j]["NameZhTw"].ToString(), _prelang);
                }
            }

            return rtnVal;
        }
    }

    /// <summary>
    /// AppSettingType
    /// </summary>
    public enum AppSettingType
    {
        /// <summary>
        /// Account
        /// </summary>
        [Description("ACCOUNT")]
        ACCOUNT = 0,

        /// <summary>
        /// Address
        /// </summary>
        [Description("ADDRESS")]
        ADDRESS = 1,

        /// <summary>
        /// Admin
        /// </summary>
        [Description("ADMIN")]
        ADMIN = 2,

        /// <summary>
        /// Approval
        /// </summary>
        [Description("APPROVAL")]
        APPROVAL = 3,

        /// <summary>
        /// Base
        /// </summary>
        [Description("BASE")]
        BASE = 4,

        /// <summary>
        /// Board
        /// </summary>
        [Description("BOARD")]
        BOARD = 5,

        /// <summary>
        /// Common
        /// </summary>
        [Description("COMMON")]
        COMMON = 6,

        /// <summary>
        /// Community
        /// </summary>
        [Description("COMMUNITY")]
        COMMUNITY = 7,

        /// <summary>
        /// Component
        /// </summary>
        [Description("COMPONENT")]
        COMPONENT = 8,

        /// <summary>
        /// EIP
        /// </summary>
        [Description("EIP")]
        EIP = 9,

        /// <summary>
        /// Mail
        /// </summary>
        [Description("MAIL")]
        MAIL = 10,

        /// <summary>
        /// Messenger
        /// </summary>
        [Description("MESSENGER")]
        MESSENGER = 11,

        /// <summary>
        /// Pims
        /// </summary>
        [Description("PIM")]
        PIM = 12,

        /// <summary>
        /// Poll
        /// </summary>
        [Description("POLL")]
        POLL = 13,

        /// <summary>
        /// ResourceManager
        /// </summary>
        [Description("RESOURCEMANAGER")]
        RESOURCEMANAGER = 14,

        /// <summary>
        /// WebHard
        /// </summary>
        [Description("WEBHARD")]
        WEBHARD = 15,

        /// <summary>
        /// Work
        /// </summary>
        [Description("WORK")]
        WORK = 16,

        /// <summary>
        /// Opinion
        /// </summary>
        [Description("OPINION")]
        OPINION = 17,

        /// <summary>
        /// SMS
        /// </summary>
        [Description("SMS")]
        SMS = 18,

        /// <summary>
        /// PMS
        /// </summary>
        [Description("PMS")]
        PMS = 19,

        /// <summary>
        ///
        /// </summary>
        [Description("CONFIG")]
        CONFIG = 20,

        /// <summary>
        /// ALARM
        /// </summary>
        [Description("ALARM")]
        ALARM = 21,

        /// <summary>
        /// PORTLET
        /// </summary>
        [Description("PORTLET")]
        PORTLET = 22,

        /// <summary>
        /// REPORT
        /// </summary>
        [Description("REPORT")]
        REPORT = 23,

        /// <summary>
        /// COOP
        /// </summary>
        [Description("COOP")]
        COOP = 24,

        /// <summary>
        /// MDM
        /// </summary>
        [Description("MDM")]
        MDM = 25,

        /// <summary>
        /// ETC
        /// </summary>
        [Description("ETC")]
        ETC = 26,

        /// <summary>
        /// SNS
        /// </summary>
        [Description("SNS")]
        SNS = 27,

        /// <summary>
        /// SEARCH
        /// </summary>
        [Description("SEARCH")]
        SEARCH = 28,

        /// <summary>
        /// VISIT
        /// </summary>
        [Description("VISIT")]
        VISIT = 29,

        /// <summary>
        /// PROPOSAL
        /// </summary>
        [Description("PROPOSAL")]
        PROPOSAL = 30,

        /// <summary>
        /// TASK
        /// </summary>
        [Description("TASK")]
        TASK = 31,

        /// <summary>
        /// HR
        /// </summary>
        [Description("HR")]
        HR = 32
    };

    /// <summary>
    /// AppSettingType
    /// </summary>
    public enum AlertMsgEnum
    {
        Alert_Success = 0,
        Alert_Register = 1,
        Alert_Modify = 2,
        Alert_Delete = 3,
        Alert_Error = 4
    };
}