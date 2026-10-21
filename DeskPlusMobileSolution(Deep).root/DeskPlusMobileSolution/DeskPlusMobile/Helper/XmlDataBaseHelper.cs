using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public abstract class XmlDataBaseHelper
    {
        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 캐쉬 초기화 하기
        /// 작성일자 : 2009-10-23
        /// </summary>
        public static void ClearCache()
        {
            HttpContext ctx = HttpContext.Current;
            List<string> keys = new List<string>();

            IDictionaryEnumerator enumerator = ctx.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            for (int i = 0; i < keys.Count; i++)
            {
                ctx.Cache.Remove(keys[i]);
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 캐쉬에서 GetAppSetting 값 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">_type.</param>
        /// <param name="_key">_key.</param>
        /// <returns></returns>
        public static string GetAppSetting(AppSettingType _type, string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                return "";
            }

            string appSettingValue = "";

            bool useXmlCache = Convert.ToBoolean(ConfigurationManager.AppSettings["UseXmlCache"].ToString());

            DataSet ds = new DataSet();

            if (useXmlCache)
            {
                string cacheName = GetCacheName(_type);

                if (HttpContext.Current.Cache[cacheName] == null)
                {
                    CreateCacheAppSetting(_type, _key, cacheName);
                }

                HttpContext context = HttpContext.Current;
                ds = (DataSet)context.Cache[cacheName];
            }
            else
            {
                string filePath = HttpContext.Current.Server.MapPath(GetTargetPath(_type));
                ds.ReadXml(filePath);
            }

            if (ds == null) return "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = ds.Tables[0].Select("ID = '" + _key + "'");

                if (dr.Length > 0)
                {
                    foreach (DataRow tempRow in dr)
                    {
                        appSettingValue = tempRow[1].ToString();
                    }
                }
            }

            ds = null;

            return appSettingValue.Replace("\r\n", " ");
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
            string filePath = HttpContext.Current.Server.MapPath(GetTargetPath(_type));
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            HttpContext ctx = HttpContext.Current;
            ctx.Cache.Insert(_cacheName, ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            ds = null;
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : xml 경로값 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <returns></returns>
        private static string GetTargetPath(AppSettingType _type)
        {
            string targetPath = String.Empty;

            switch (_type)
            {
                case AppSettingType.Base:
                    targetPath = "/DeskPlusMobile/App_Data/Base.xml";
                    break;
                case AppSettingType.SQLApproval:
                    targetPath = "/DeskPlusMobile/App_Data/SQLApproval.xml";
                    break;
                case AppSettingType.SQLBoard:
                    targetPath = "/DeskPlusMobile/App_Data/SQLBoard.xml";
                    break;
                case AppSettingType.SQLMail:
                    targetPath = "/DeskPlusMobile/App_Data/SQLMail.xml";
                    break;
                case AppSettingType.SQLAddress:
                    targetPath = "/DeskPlusMobile/App_Data/SQLAddress.xml";
                    break;
                case AppSettingType.SQLPim:
                    targetPath = "/DeskPlusMobile/App_Data/SQLPim.xml";
                    break;
                case AppSettingType.SQLPortal:
                    targetPath = "/DeskPlusMobile/App_Data/SQLPortal.xml";
                    break;
                case AppSettingType.Approval:
                    targetPath = "/DeskPlusMobile/App_Data/Approval.xml";
                    break;
                case AppSettingType.SQLSMS:
                    targetPath = "/DeskPlusMobile/App_Data/SQLSMS.xml";
                    break;
                case AppSettingType.SQLAccount:
                    targetPath = "/DeskPlusMobile/App_Data/SQLAccount.xml";
                    break;
                case AppSettingType.SQLWebHard:
                    targetPath = "/DeskPlusMobile/App_Data/SQLWebHard.xml";
                    break;
                case AppSettingType.SQLCommunity:
                    targetPath = "/DeskPlusMobile/App_Data/SQLCommunity.xml";
                    break;
                case AppSettingType.SQLMDM:
                    targetPath = "/DeskPlusMobile/App_Data/SQLMDM.xml";
                    break;
                case AppSettingType.SQLSNS:
                    targetPath = "/DeskPlusMobile/App_Data/SQLSNS.xml";
                    break;
                case AppSettingType.Mail:
                    targetPath = "/DeskPlusMobile/App_Data/Mail.xml";
                    break;
                case AppSettingType.SQLCoop:
                    targetPath = "/DeskPlusMobile/App_Data/SQLCoop.xml";
                    break;
                case AppSettingType.Coop:
                    targetPath = "/DeskPlusMobile/App_Data/Coop.xml";
                    break;
                case AppSettingType.SQLTask:
                    targetPath = "/DeskPlusMobile/App_Data/SQLTask.xml";
                    break;
                case AppSettingType.Task:
                    targetPath = "/DeskPlusMobile/App_Data/Task.xml";
                    break;
                case AppSettingType.SQLRM:
                    targetPath = "/DeskPlusMobile/App_Data/SQLRM.xml";
                    break;
                case AppSettingType.RM:
                    targetPath = "/DeskPlusMobile/App_Data/RM.xml";
                    break;
                case AppSettingType.SQLAttendance:
                    targetPath = "/DeskPlusMobile/App_Data/SQLAttendance.xml";
                    break;
                case AppSettingType.SQLHR:
                    targetPath = "/DeskPlusMobile/App_Data/SQLHR.xml";
                    break;
                case AppSettingType.HR:
                    targetPath = "/DeskPlusMobile/App_Data/HR.xml";
                    break;

            }

            return targetPath;
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 캐쉬명 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-21
        /// </summary>
        /// <param name="_type">The _type.</param>
        /// <returns></returns>
        private static string GetCacheName(AppSettingType _type)
        {
            string cacheName = "";

            switch (_type)
            {
                case AppSettingType.Base:
                    cacheName = "Base";
                    break;
                case AppSettingType.SQLApproval:
                    cacheName = "SQLApproval";
                    break;
                case AppSettingType.SQLBoard:
                    cacheName = "SQLBoard";
                    break;
                case AppSettingType.SQLMail:
                    cacheName = "SQLMail";
                    break;
                case AppSettingType.SQLPim:
                    cacheName = "SQLPim";
                    break;
                case AppSettingType.SQLAddress:
                    cacheName = "SQLAddress";
                    break;
                case AppSettingType.SQLPortal:
                    cacheName = "SQLPortal";
                    break;
                case AppSettingType.Approval:
                    cacheName = "Approval";
                    break;
                case AppSettingType.SQLSMS:
                    cacheName = "SQLSMS";
                    break;
                case AppSettingType.SQLAccount:
                    cacheName = "SQLAccount";
                    break;
                case AppSettingType.SQLWebHard:
                    cacheName = "SQLWebHard";
                    break;
                case AppSettingType.SQLCommunity:
                    cacheName = "SQLCommunity";
                    break;
                case AppSettingType.SQLMDM:
                    cacheName = "SQLMDM";
                    break;
                case AppSettingType.SQLSNS:
                    cacheName = "SQLSNS";
                    break;
                case AppSettingType.Mail:
                    cacheName = "Mail";
                    break;
                case AppSettingType.SQLCoop:
                    cacheName = "SQLCoop";
                    break;
                case AppSettingType.Coop:
                    cacheName = "Coop";
                    break;
                case AppSettingType.SQLTask:
                    cacheName = "SQLTask";
                    break;
                case AppSettingType.Task:
                    cacheName = "Task";
                    break;
                case AppSettingType.SQLRM:
                    cacheName = "SQLRM";
                    break;
                case AppSettingType.RM:
                    cacheName = "RM";
                    break;
                case AppSettingType.SQLAttendance:
                    cacheName = "SQLATTENDANCE";
                    break;
                case AppSettingType.SQLHR:
                    cacheName = "SQLHR";
                    break;
                case AppSettingType.HR:
                    cacheName = "HR";
                    break;
            }

            return cacheName;
        }

        /// <summary>
        /// 그룹별 SYSCODE 값 가져오기
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
        /// SysCode 가져오기 (DB처리)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public static DataSet GetSysCode(int _companyID)
        {
            using (SqlConnection cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
            {
                string qry = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SYSCodeAll");
                qry = String.Format(qry, _companyID);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, qry, "dsCode", null);
            }
        }
    }


    /// <summary>
    /// AppSettingType
    /// </summary>
    public enum AppSettingType
    {
        /// <summary>
        /// Base
        /// </summary>
        Base = 0,
        /// <summary>
        /// Approval
        /// </summary>
        SQLApproval = 1,
        /// <summary>
        /// Board
        /// </summary>
        SQLBoard = 2,
        /// <summary>
        /// Mail
        /// </summary>
        SQLMail = 3,
        /// <summary>
        /// Address
        /// </summary>
        SQLAddress = 4,
        /// <summary>
        /// Pim
        /// </summary>
        SQLPim = 5,
        /// <summary>
        /// Portal
        /// </summary>
        SQLPortal = 6,
        /// <summary>
        /// Approval 설정
        /// </summary>
        Approval = 7,
        /// <summary>
        /// SMS
        /// </summary>
        SQLSMS = 8,
        /// <summary>
        /// Account
        /// </summary>
        SQLAccount = 9,
        /// <summary>
        /// WebHard
        /// </summary>
        SQLWebHard = 10,
        /// <summary>
        /// Community
        /// </summary>
        SQLCommunity = 11,
        /// <summary>
        /// MDM
        /// </summary>
        SQLMDM = 12,
        /// <summary>
        /// SNS
        /// </summary>
        SQLSNS = 13, 
        /// <summary>
        /// Mail
        /// </summary>
        Mail = 14,
        /// <summary>
        /// Coop
        /// </summary>
        SQLCoop = 15,
        /// <summary>
        /// Coop
        /// </summary>
        Coop = 16,
         /// <summary>
        /// Task
        /// </summary>
        SQLTask = 17,
        /// <summary>
        /// Task
        /// </summary>
        Task = 18,
        /// <summary>
        /// RM
        /// </summary>
        SQLRM = 19,
        /// <summary>
        /// RM
        /// </summary>
        RM = 20,
        /// <summary>
        /// SQLAttendance
        /// </summary>
        SQLAttendance = 21,
        /// <summary>
        /// SQLHR
        /// </summary>
        SQLHR = 22,
        /// <summary>
        /// HR
        /// </summary>
        HR = 23
    };
}
