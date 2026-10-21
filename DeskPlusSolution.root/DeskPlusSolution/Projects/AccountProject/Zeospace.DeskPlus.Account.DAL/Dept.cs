using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class Dept : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        private DeptInfo GetDept(IDataReader _reader)
        {
            return new DeptInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                ParentDeptID = (_reader["ParentDeptID"] == DBNull.Value) ? -1 : _reader["ParentDeptID"].ToString().ToNumber<int>(),
                RootDept = _reader["RootDept"].ToString().ToNull(),
                RealDept = _reader["RealDept"].ToString().ToNull(),
                DeptCode = _reader["DeptCode"].ToString().ToNull(),
                DeptOrder = (_reader["DeptOrder"] == DBNull.Value) ? -1 : _reader["DeptOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameBase = _reader["PathNameBase"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
                ShortName = _reader["ShortName"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull(),
                Root = _reader["Root"].ToString().ToNull(),
                ZipCode = _reader["ZipCode"].ToString().ToNull(),
                Address1 = _reader["Address1"].ToString().ToNull(),
                Address2 = _reader["Address2"].ToString().ToNull(),
                Phone = _reader["Phone"].ToString().ToNull(),
                Fax = _reader["Fax"].ToString().ToNull(),
                HomePage = _reader["HomePage"].ToString().ToNull(),
                ActiveDept = _reader["ActiveDept"].ToString().ToNull(),
                ChildDept = _reader["ChildDept"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        /// <summary>
        /// Gets the dept list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<DeptInfo> GetDeptList(IDataReader reader)
        {
            List<DeptInfo> list = new List<DeptInfo>();
            while (reader.Read())
                list.Add(GetDept(reader));
            return list;
        }

        #region GET()

        /// <summary>
        /// 부서별 아이디별로 데이터 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetDeptLists(int _companyID, int _deptID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByDeptID");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 다중 부서 아이디별로 데이터 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptList">The _dept list.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetDeptLists(int _companyID, string _deptList, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByDeptList");
                query = string.Format(query, activeDept, _deptList);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 상위 부서별 아이디로 데이터 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentDeptID">The _parent dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetParentDeptLists(int _companyID, int _parentDeptID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByPDeptID");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ParentDeptID", SqlDbType.Int).Value = _parentDeptID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 최상위 부서별 정보만 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetTopDeptLists(int _companyID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByPIDNull");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 :
        /// 최상위 부서별 정보만 가져오기 (최상위부서 체크박스)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetRootDeptLists(int _companyID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByRootDept");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-11-16
        /// 작성목적 : 모든 부서정보가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataSet GetDept(int _companyID, string tableName, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_Sel");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataSet(cmd, tableName);
            }
        }

        /// <summary>
        /// 부서별 코드로 데이터 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptCode">The _dept code.</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetDeptListsByDeptCode(int _companyID, string _deptCode)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelByDeptCode");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar, 300).Value = _deptCode;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        /// <summary>
        /// 심재운 (게시판용)
        /// 회사 상관없이 - 부서별 아이디별로 데이터 가져오기
        /// </summary>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_activeDept">if set to <c>true</c> [_active dept].</param>
        /// <returns></returns>
        public List<BE.DeptInfo> GetDeptLists(int _deptID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution()))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_Dept_SelWholeByDeptID");
                query = string.Format(query, activeDept);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDeptList(reader);
                }
            }
        }

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서 추가
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int Add(BE.DeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_Dept_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentDeptID", SqlDbType.Int).Value = _info.ParentDeptID;
                    cmd.Parameters.Add("@RootDept", SqlDbType.Char, 1).Value = _info.RootDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@RealDept", SqlDbType.Char, 1).Value = _info.RealDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar, 20).Value = _info.DeptCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@ShortName", SqlDbType.NVarChar, 100).Value = _info.ShortName.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@Root", SqlDbType.Char, 1).Value = _info.Root.ToNull().ToXSS();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = _info.Address1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = _info.Address2.ToNull().ToXSS();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePage", SqlDbType.NVarChar, 100).Value = _info.HomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveDept", SqlDbType.Char, 1).Value = _info.ActiveDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서 수정
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public bool Mod(BE.DeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_Dept_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@RootDept", SqlDbType.Char, 1).Value = _info.RootDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@RealDept", SqlDbType.Char, 1).Value = _info.RealDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@DeptCode", SqlDbType.VarChar, 20).Value = _info.DeptCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@ShortName", SqlDbType.NVarChar, 100).Value = _info.ShortName.ToNull();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar, 50).Value = _info.ZipCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = _info.Address1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = _info.Address2.ToNull().ToXSS();
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = _info.Phone.ToNull().ToXSS();
                    cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 20).Value = _info.Fax.ToNull().ToXSS();
                    cmd.Parameters.Add("@HomePage", SqlDbType.NVarChar, 100).Value = _info.HomePage.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveDept", SqlDbType.Char, 1).Value = _info.ActiveDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 부서 순서변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _category list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] _listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_ACC_Dept_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _listStr[i];
                            cmd.Parameters.Add("@DeptOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 부서 상위 변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_parentDeptID">The _parent dept ID.</param>
        /// <returns></returns>
        public bool ModParentDeptID(int _companyID, int _deptID, int _parentDeptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_ACC_Dept_UpdPID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@ParentDeptID", SqlDbType.Int).Value = _parentDeptID;

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_Dept_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion DEL()
    }
}