using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// C : MS Data Access Application Block 을 우리에 맞게 수정하여 사용
    /// </summary>
    ///
    public abstract class OleDbHelper
    {
        /// <summary>
        /// 구성파일에서 SQL 컨넥션 정보 가져오기
        /// CONN : OLE 정보 가져오기
        /// </summary>
        //public static readonly string CONN_STRING = Security.Protector.Dec(Config.ConfigurationTool.GetConfig().ConnectionString);

        public static readonly string CONN = ConfigurationManager.AppSettings["OleDbConn"];

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ExecuteNonQuery : int ExecuteNonQuery(string _connString, CommandType _cmdType, string _cmdText, out int _rtnValue, params OleDbParameter[] _cmdParms)
        /// </summary>
        /// <param name="_connString"></param>
        /// <param name="_cmdType"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_rtnValue"></param>
        /// <param name="_cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string _connString, CommandType _cmdType, string _cmdText, out int _rtnValue, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
                int val = cmd.ExecuteNonQuery();
                _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향받은 Row 수</returns>
        public static int ExecuteNonQuery(string _connString, CommandType _cmdType, string _cmdText, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_conn">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_rtnValue">out 리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OleDbConnection _conn, CommandType _cmdType, string _cmdText, out int _rtnValue, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(cmd, _conn, null, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", rtnValue, new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_trans">존재하는 SQL Transaction</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_rtnValue">out 리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OleDbTransaction _trans, CommandType _cmdType, string _cmdText, out int _rtnValue, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(cmd, _trans.Connection, _trans, _cmdType, _cmdText, ReturnValue.Yes, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            _rtnValue = (int)cmd.Parameters["ReturnValue"].Value;
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// M : ExecuteNonQuery
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_trans">존재하는 SQL Transaction</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>영향 받은 Row 수</returns>
        public static int ExecuteNonQuery(OleDbTransaction _trans, CommandType _cmdType, string _cmdText, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(cmd, _trans.Connection, _trans, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// M : ExecuteDataSet
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  DataSet ds = ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", "users", new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...</param>
        /// <param name="_tableName">DataSet에 들어갈 Table명</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string _connString, CommandType _cmdType, string _cmdText, string _tableName, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);

                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
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
        ///  DataSet ds = ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, commadText, commandTableName, new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_tableName">DataSet에 들어갈 Table명의 배열</param>
        /// <param name="_cmdParms">SQL Parameter</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string _connString, CommandType _cmdType, string[] _cmdText, string[] _tableName, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, "", ReturnValue.No, null);

                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
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
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, "", ReturnValue.No, null);

                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    for (int i = 0; i < _tableName.Length; i++)
                    {
                        cmd.Parameters.Clear();

                        OleDbParameter[] tmpSqlParm = (OleDbParameter[])_cmdParmsArr[i];

                        for (int j = 0; j < tmpSqlParm.Length; j++)
                        {
                            cmd.Parameters.Insert(j, tmpSqlParm[j]);
                        }

                        //cmd.Parameters.Add((OleDbParameter)_cmdParmsArr[i]);
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
        ///  OleDbDataReader r = ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_SSO_GETUSERPASSWORDBYEMAIL", new OleDbParameter("@userid", 100));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string _connString, CommandType _cmdType, string _cmdText, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(_connString);

            try
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                OleDbDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return sdr;
            }
            catch (Exception e)
            {
                conn.Close();
                Exceptions.AppException.WriteException("OleDbHelper.cs[ExecuteReader] : " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// M : ExecuteScalar
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="_connString">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string _connString, CommandType _cmdType, string _cmdText, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(_connString))
            {
                PrepareCommand(cmd, conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// M : ExecuteScalar
        /// </summary>
        /// <remarks>
        /// e.g.:
        ///  Object obj = ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="_conn">연결스트링</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(OleDbConnection _conn, CommandType _cmdType, string _cmdText, params OleDbParameter[] _cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            PrepareCommand(cmd, _conn, null, _cmdType, _cmdText, ReturnValue.No, _cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// M : 캐쉬에 파라미터 어레이를 추가
        /// </summary>
        /// <param name="_cacheKey">캐쉬 Key name</param>
        /// <param name="_cmdParms">캐쉬할 OleDbParameters Array</param>
        public static void CacheParameters(string _cacheKey, params OleDbParameter[] _cmdParms)
        {
            parmCache[_cacheKey] = _cmdParms;
        }

        /// <summary>
        /// M : 캐쉬된 파라미터 가져오기
        /// </summary>
        /// <param name="_cacheKey">캐쉬 Key name</param>
        /// <returns>캐쉬된 Parameters Array</returns>
        public static OleDbParameter[] GetCacheParameters(string _cacheKey)
        {
            OleDbParameter[] cachedParms = (OleDbParameter[])parmCache[_cacheKey];

            if (cachedParms == null)
                return null;

            OleDbParameter[] clonedParms = new OleDbParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OleDbParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// M : Command 명령 만들기
        /// </summary>
        /// <param name="_cmd">OleDbCommand object</param>
        /// <param name="_conn">OleDbConnection object</param>
        /// <param name="_trans">OleDbTransaction object</param>
        /// <param name="_cmdType">CommandType (stored procedure, text, etc.)</param>
        /// <param name="_cmdText">쿼리문,SP이름...의 배열</param>
        /// <param name="rtnValue">리턴값</param>
        /// <param name="_cmdParms">SQL Parameter Array</param>
        private static void PrepareCommand(OleDbCommand _cmd, OleDbConnection _conn, OleDbTransaction _trans, CommandType _cmdType, string _cmdText, ReturnValue rtnValue, OleDbParameter[] _cmdParms)
        {
            if (_conn.State != ConnectionState.Open) _conn.Open();

            _cmd.Connection = _conn;
            _cmd.CommandText = _cmdText;

            if (_trans != null)
                _cmd.Transaction = _trans;

            _cmd.CommandType = _cmdType;

            if (_cmdParms != null)
            {
                foreach (OleDbParameter parm in _cmdParms)
                    _cmd.Parameters.Add(parm);
            }

            //Return value 받기
            if (rtnValue == ReturnValue.Yes)
            {
                _cmd.Parameters.Add(new OleDbParameter("ReturnValue", OleDbType.Integer,
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