//===============================================================================
// This file is based on the Microsoft Data Access Application Block for .NET
// For more information please go to
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//===============================================================================
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// OdbcHelper에 대한 요약 설명입니다.
    /// </summary>
    public abstract class OdbcHelper
    {
        /// <summary>
        /// 구성파일에서 SQL 컨넥션 정보 가져오기
        /// CONN_EIP : EIP 부분
        /// CONN_SSO : SSO 부분
        /// </summary>
        //public static readonly string CONN_STRING = Security.Protector.Dec(Config.ConfigurationTool.GetConfig().ConnectionString);
        public static readonly string CONN_MASTER = ConfigurationManager.AppSettings["SQLConnMaster"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnAlarm"];
        /// </summary>
        public static readonly string CONN_ALARM = ConfigurationManager.AppSettings["SQLConnAlarm"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnSolution"];
        /// </summary>
        public static readonly string CONN_SOLUTION = ConfigurationManager.AppSettings["SQLConnSolution"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnMail"];
        /// </summary>
        public static readonly string CONN_MAIL = ConfigurationManager.AppSettings["SQLConnMail"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnMessenger"];
        /// </summary>
        public static readonly string CONN_MESSENGER = ConfigurationManager.AppSettings["SQLConnMessenger"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLEtc1"];
        /// </summary>
        public static readonly string CONN_ETC1 = ConfigurationManager.AppSettings["SQLEtc1"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLEtc2"];
        /// </summary>
        public static readonly string CONN_ETC2 = ConfigurationManager.AppSettings["SQLEtc2"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLEtc3"];
        /// </summary>
        public static readonly string CONN_ETC3 = ConfigurationManager.AppSettings["SQLEtc3"];

        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnAccountStatistics"];
        /// </summary>
        public static readonly string CONN_STATISTICS = ConfigurationManager.AppSettings["SQLConnAccountStatistics"];

        /// <summary>
        /// Hashtable to store cached parameters
        /// </summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Hashtable to store cached parameters
        /// </summary>
        private static Hashtable connCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType</param>
        /// <param name="_cmdText">쿼리문</param>
        /// <param name="_rtnValue">out 리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향받은 Row수</returns>
        public static int ExecuteNonQuery(string _connString, CommandType _cmdType, string _cmdText, out int _rtnValue, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();
            int val = 0;
            using (OdbcConnection conn = new OdbcConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
                val = cmd.ExecuteNonQuery();
                _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
                cmd.Parameters.Clear();
                conn.Close();
            }

            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향받은 Row 수</returns>
        public static int ExecuteNonQuery(string _connString, CommandType _cmdType, string _cmdText, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();
            int val = 0;
            using (OdbcConnection conn = new OdbcConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Close();
            }

            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_conn">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_rtnValue">out 리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OdbcConnection _conn, CommandType _cmdType, string _cmdText, out int _rtnValue, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();

            PrepareCommand(cmd, _conn, null, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
            cmd.Parameters.Clear();
            cmd.Dispose();
            _conn.Close();
            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_trans">존재하는 SQL Transaction</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_rtnValue">out 리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OdbcTransaction _trans, CommandType _cmdType, string _cmdText, out int _rtnValue, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();

            PrepareCommand(cmd, _trans.Connection, _trans, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
            cmd.Parameters.Clear();
            cmd.Dispose();
            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_trans">존재하는 SQL Transaction</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OdbcTransaction _trans, CommandType _cmdType, string _cmdText, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();

            PrepareCommand(cmd, _trans.Connection, _trans, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return val;
        }

        /// <summary>
        /// M : ExecuteDataSet
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  DataSet ds = ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", "users", new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_tableName">DataSet에 들어갈 Table명</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string _connString, CommandType _cmdType, string _cmdText, string _tableName, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();

            using (OdbcConnection conn = new OdbcConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);

                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, _tableName);
                    cmd.Parameters.Clear();
                    return ds;
                }
            }
        }

        /// <summary>
        /// M : ExecuteDataSet
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  DataSet ds = ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, commadText, commandTableName, new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_tableName">DataSet에 들어갈 Table명의 배열</param>
        /// <param name="_cmdParms">SQL Parameter</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string _connString, CommandType _cmdType, string[] _cmdText, string[] _tableName, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();

            using (OdbcConnection conn = new OdbcConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, "", ReturnValue.No, null);

                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    for (int i = 0; i < _tableName.Length; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(_cmdParms[i]);
                        cmd.CommandText = _cmdText[i];
                        da.Fill(ds, _tableName[i]);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// M : ExecuteDataSet
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  DataSet ds = ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, commadText, commandTableName, sqlParam);
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_tableName">DataSet에 들어갈 Table명의 배열</param>
        /// <param name="_cmdParmsArr">SQL Parameter Array</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string _connString, CommandType _cmdType, string[] _cmdText, string[] _tableName, ArrayList _cmdParmsArr)
        {
            OdbcCommand cmd = new OdbcCommand();

            using (OdbcConnection conn = new OdbcConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, "", ReturnValue.No, null);

                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    for (int i = 0; i < _tableName.Length; i++)
                    {
                        cmd.Parameters.Clear();

                        OdbcParameter[] tmpSqlParm = (OdbcParameter[])_cmdParmsArr[i];

                        for (int j = 0; j < tmpSqlParm.Length; j++)
                        {
                            cmd.Parameters.Insert(j, tmpSqlParm[j]);
                        }

                        //cmd.Parameters.Add((OdbcParameter)_cmdParmsArr[i]);
                        cmd.CommandText = _cmdText[i];
                        da.Fill(ds, _tableName[i]);
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// M : ExecuteReader
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  OdbcDataReader r = ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", new OdbcParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>OdbcDataReader</returns>
        public static OdbcDataReader ExecuteReader(string _connString, CommandType _cmdType, string _cmdText, params OdbcParameter[] _cmdParms)
        {
            OdbcCommand cmd = new OdbcCommand();
            OdbcConnection conn = new OdbcConnection(_connString);

            try
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                OdbcDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return sdr;
            }
            catch (Exception e)
            {
                conn.Close();
                Exceptions.AppException.WriteException("OdbcHelper.cs[ExecuteReader] : " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// M : ExecuteScalar
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "PublishOrders", new OdbcParameter("@prodid", 24));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>object</returns>
        public static string ExecuteScalar(string _connString, CommandType _cmdType, string _cmdText, params OdbcParameter[] _cmdParms)
        {
            /*            OdbcCommand cmd = new OdbcCommand();

                        using (OdbcConnection conn = new OdbcConnection(_connString))
                        {
                            PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                            object val = cmd.ExecuteScalar();
                            cmd.Parameters.Clear();
                            return val;
                        }
             */
            OdbcCommand cmd = new OdbcCommand();
            OdbcConnection conn = new OdbcConnection(_connString);
            string tmp = "";
            try
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                OdbcDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                while (sdr.Read())
                {
                    tmp = sdr[0].ToString();
                }
                cmd.Dispose();
                sdr.Close();
                sdr.Dispose();
                return tmp;
            }
            catch (Exception e)
            {
                conn.Close();
                Exceptions.AppException.WriteException("OdbcHelper.cs[ExecuteReader] : " + e.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// M : ExecuteScalar
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "PublishOrders", new OdbcParameter("@prodid", 24));
        /// </remarks>
        /// <param name="_conn">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(OdbcConnection _conn, CommandType _cmdType, string _cmdText, params OdbcParameter[] _cmdParms)
        {
            //OdbcCommand cmd = new OdbcCommand();

            //PrepareCommand(cmd, _conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
            //object val = cmd.ExecuteScalar();
            //cmd.Parameters.Clear();
            //return val;

            OdbcCommand cmd = new OdbcCommand();
            string tmp = "";
            try
            {
                PrepareCommand(cmd, _conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                OdbcDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                while (sdr.Read())
                {
                    tmp = sdr[0].ToString();
                }
                cmd.Dispose();
                sdr.Close();
                sdr.Dispose();
                return tmp;
            }
            catch (Exception e)
            {
                _conn.Close();
                Exceptions.AppException.WriteException("OdbcHelper.cs[ExecuteReader] : " + e.Message);
                throw;
            }
            finally
            {
                _conn.Close();
            }
        }

        /// <summary>
        /// M : 캐쉬에 파라미터 어레이를 추가
        /// </summary>
        /// <param name="_cacheKey">캐쉬 Key name</param>
        /// <param name="_cmdParms">캐쉬할 OdbcParameters Array</param>
        public static void CacheParameters(string _cacheKey, params OdbcParameter[] _cmdParms)
        {
            parmCache[_cacheKey] = _cmdParms;
        }

        /// <summary>
        /// M : 캐쉬된 파라미터 가져오기
        /// </summary>
        /// <param name="_cacheKey">캐쉬 Key name</param>
        /// <returns>캐쉬된 Parameters Array</returns>
        public static OdbcParameter[] GetCacheParameters(string _cacheKey)
        {
            OdbcParameter[] cachedParms = (OdbcParameter[])parmCache[_cacheKey];

            if (cachedParms == null)
                return null;

            OdbcParameter[] clonedParms = new OdbcParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OdbcParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 회사별 컨넥션
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        public static string ConnSolution(int _companyID)
        {
            return OdbcHelper.CONN_SOLUTION;

            #region 여러 데이터베이스(회사별)에 연결 구문

            //			string conn = "";
            //
            //			if (_companyID <= 0)
            //			{
            //				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("잘못된 회사 ID입니다 " + _companyID);
            //				return CONN_SOLUTION;
            //			}
            //			conn = (string)connCache[_companyID];
            //
            //			if (conn == null || conn == "")
            //			{
            //				OdbcParameter[] parms = GetCacheParameters("SQL_DP_MASTER_CONNECTION_SELECT_COMPANYID");
            //
            //				if (parms == null)
            //				{
            //					parms = new OdbcParameter[] {
            //												   new OdbcParameter("@CompanyID", SqlDbType.Int32)
            //											   };
            //					OdbcHelper.CacheParameters("SQL_DP_MASTER_CONNECTION_SELECT_COMPANYID", parms);
            //				}
            //
            //
            //				parms[0].Value = _companyID;
            //
            //				string dbName = "";
            //				string userID = "";
            //				string password = "";
            //				string dBServer = ConfigurationManager.AppSettings["DBServer"];
            //				string dBNamePrefix = ConfigurationManager.AppSettings["DBNamePreFix"];
            //
            //				using (SqlDataReader sdr = ExecuteReader(OdbcHelper.CONN_MASTER, CommandType.StoredProcedure, "SQL_DP_MASTER_Connection_SelectByCompanyID", parms))
            //				{
            //					if (sdr.Read())
            //					{
            //						dbName					= (sdr["DataBaseName"]						== DBNull.Value) ? "" : sdr["DataBaseName"].ToString();
            //						userID					= (sdr["UserID"]							== DBNull.Value) ? "" : sdr["UserID"].ToString();
            //						password				= (sdr["Password"]							== DBNull.Value) ? "" : sdr["Password"].ToString();
            //					}
            //				}
            //
            //				if (dBServer != "" && dbName != "" && userID != "" && password != "")
            //				{
            //					conn = "Server=" + dBServer + ";DataBase=" + dBNamePrefix + dbName + ";User ID=" + userID + ";Password=" + password + ";";
            //				}
            //				else
            //				{
            //					conn = ConfigurationManager.AppSettings["ConnSolution"]; // 잘못된 Connection 일때는 기본 컨넥션으로
            //					return conn;
            //				}
            //
            //				connCache[_companyID] = conn;
            //			}
            //
            //			return conn;

            #endregion 여러 데이터베이스(회사별)에 연결 구문
        }

        /// <summary>
        /// hash table 파라미터 캐쉬에 저장
        /// </summary>
        /// <param name="_key">회사아이디</param>
        /// <param name="_value">회사db연결자</param>
        public static void SetConn(int _key, string _value)
        {
            string conn = "";
            conn = (string)connCache[_key];

            if (conn == null || conn == "")
            {
                connCache[_key] = _value;
            }
        }

        /// <summary>
        /// M : Command 명령 만들기
        /// </summary>
        /// <param name="_cmd">OdbcCommand object</param>
        /// <param name="_conn">OdbcConnection object</param>
        /// <param name="_trans">OdbcTransaction object</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="rtnValue">리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        private static void PrepareCommand(OdbcCommand _cmd, OdbcConnection _conn, OdbcTransaction _trans, CommandType _cmdType, string _cmdText, ReturnValue rtnValue, OdbcParameter[] _cmdParms)
        {
            if (_conn.State != ConnectionState.Open) _conn.Open();

            _cmd.Connection = _conn;
            _cmd.CommandText = _cmdText;

            if (_trans != null)
                _cmd.Transaction = _trans;

            _cmd.CommandType = _cmdType;

            if (_cmdParms != null)
            {
                foreach (OdbcParameter parm in _cmdParms)
                    _cmd.Parameters.Add(parm);
            }

            //Return value 받기
            if (rtnValue == ReturnValue.Yes)
            {
                _cmd.Parameters.Add(new OdbcParameter("ReturnValue",
                    OdbcType.Int,
                    4,
                    ParameterDirection.ReturnValue,
                    false,
                    0,
                    0,
                    string.Empty,
                    DataRowVersion.Default,
                    null));
            }
        }
    }
}