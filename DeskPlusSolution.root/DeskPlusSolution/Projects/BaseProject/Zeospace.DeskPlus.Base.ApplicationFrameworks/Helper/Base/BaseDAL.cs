using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : BaseDAL
    /// </summary>
    public abstract class BaseDAL
    {
        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnSolution"];
        /// </summary>
        private static readonly string CONN_SOLUTION = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnAlarm"];
        /// </summary>
        private static readonly string CONN_MASTER = ConfigurationManager.ConnectionStrings["SQLConnMaster"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnAlarm"];
        /// </summary>
        private static readonly string CONN_ALARM = ConfigurationManager.ConnectionStrings["SQLConnAlarm"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLEtc1"];
        /// </summary>
        private static readonly string CONN_ETC = ConfigurationManager.ConnectionStrings["SQLConnEtc"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnMail"];
        /// </summary>
        private static readonly string CONN_EDMS = ConfigurationManager.ConnectionStrings["SQLConnEDMS"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnMail"];
        /// </summary>
        private static readonly string CONN_MAIL = ConfigurationManager.ConnectionStrings["SQLConnMail"].ConnectionString;

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnCMP"];
        /// </summary>
        private static readonly string CONN_CMP = ConfigurationManager.ConnectionStrings["SQLConnCMP"].ConnectionString;

        /// <summary>
        /// 회사별 컨넥션(Solution 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnSolution()
        {
            return CONN_SOLUTION;
        }

        /// <summary>
        /// 회사별 컨넥션(Solution 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnSolution(int _companyID)
        {
            return CONN_SOLUTION;

            //return GetCompanyConn(_companyID);//db가 별도의 회사일 경우 사용
        }

        /// <summary>
        /// 회사별 컨넥션(MASTER 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnMaster()
        {
            return CONN_MASTER;
        }

        /// <summary>
        /// 회사별 컨넥션(알람 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnAlarm(int _companyID)
        {
            return CONN_ALARM;
        }

        /// <summary>
        /// 회사별 컨넥션 (ETC 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnEtc(int _companyID)
        {
            return CONN_ETC;
        }

        /// <summary>
        /// 회사별 컨넥션 (EDMS 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnEDMS(int _companyID)
        {
            return CONN_EDMS;
        }

        /// <summary>
        /// 회사별 컨넥션 (MAIL 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnMAIL(int _companyID)
        {
            return CONN_MAIL;
        }

        /// <summary>
        /// CMP 컨넥션(알람 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnCMP()
        {
            return CONN_CMP;
        }

        protected static int ExecuteNonQuery(DbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        protected static IDataReader ExecuteReader(DbCommand cmd)
        {
            return ExecuteReader(cmd, CommandBehavior.Default);
        }

        protected static IDataReader ExecuteReader(DbCommand cmd, CommandBehavior behavior)
        {
            return cmd.ExecuteReader(behavior);
        }

        protected object ExecuteScalar(DbCommand cmd)
        {
            return cmd.ExecuteScalar();
        }

        protected static DataSet ExecuteDataSet(SqlCommand cmd, string tableName)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds, tableName);
                return ds;
            }
        }

        protected static DataTableReader ExecuteDataTableReader(SqlCommand cmd)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.CreateDataReader();
            }
        }

        /// <summary>
        /// 카테고리별 동적쿼리 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTableReader GetQuery(string _categoryID)
        {
            using (SqlConnection cn = new SqlConnection(CONN_SOLUTION))
            {
                string query = "SELECT [qryid], [qryvalue] FROM dbo.DP_QUERY WHERE Category='" + _categoryID.ToInject() + "'";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 환경정보 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTableReader GetEnvQuery(int _companyID, string _gubun)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Env_Sel");
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Gubun", SqlDbType.VarChar, 100).Value = _gubun;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// SysCode 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public static DataSet GetSysCode(int _companyID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_Sel");
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetSysCodeQuery");
            }
        }

        /// <summary>
        /// 멀티 회사별 데이터베이스 별도 시, 접속연결자 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private string GetCompanyConn(int _companyID)
        {
            string conn = "";
            DataTableReader dtr = null;
            using (SqlConnection cn = new SqlConnection(CONN_MASTER))
            {
                SqlCommand cmd = new SqlCommand("DP_MASTER_Conn_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                dtr = ExecuteDataTableReader(cmd);
            }

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        conn = (dtr["ConnectionString"] == DBNull.Value) ? "" : dtr["ConnectionString"].ToString();
                    }
                }
            }

            return conn;
        }
    }
}