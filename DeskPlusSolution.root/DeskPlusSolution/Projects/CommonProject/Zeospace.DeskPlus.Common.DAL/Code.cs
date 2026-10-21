using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class Code : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private CodeInfo GetCode(IDataReader _reader)
        {
            return new CodeInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CodeID = (_reader["CodeID"] == DBNull.Value) ? -1 : _reader["CodeID"].ToString().ToNumber<int>(),
                ParentCodeID = (_reader["ParentCodeID"] == DBNull.Value) ? -1 : _reader["ParentCodeID"].ToString().ToNumber<int>(),
                InterfaceCode = _reader["InterfaceCode"].ToString().ToNull(),
                CodeOrder = (_reader["CodeOrder"] == DBNull.Value) ? -1 : _reader["CodeOrder"].ToString().ToNumber<int>(),
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
                Type = _reader["Type"].ToString().ToNull(),
                Scope = _reader["Scope"].ToString().ToNull(),
                Extension1 = _reader["Extension1"].ToString().ToNull(),
                Extension2 = _reader["Extension2"].ToString().ToNull(),
                Description = _reader["Description"].ToString().ToNull(),
                ActiveCode = _reader["ActiveCode"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                AccessID = _reader["AccessID"].ToString().ToNull()
            };
        }

        /// <summary>
        /// Gets the code list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<CodeInfo> GetCodeList(IDataReader reader)
        {
            List<CodeInfo> list = new List<CodeInfo>();
            while (reader.Read())
                list.Add(GetCode(reader));
            return list;
        }

        #region GET

        /// <summary>
        /// 작성자 : 심재운
        /// 직급,직책 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCode">The _active code.</param>
        /// <returns></returns>
        public DataSet GetCode(int _companyID, bool _activeCode)
        {
            string activeCode = (_activeCode) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_Sel");
                query = string.Format(query, activeCode);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetCode");
            }
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_scope">The _scope.</param>
        /// <param name="_activeCode">if set to <c>true</c> [_active code].</param>
        /// <returns></returns>
        public DataSet GetCode(int _companyID, string _scope, bool _activeCode)
        {
            string activeCode = (_activeCode) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelByScope");
                query = string.Format(query, activeCode);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Scope", SqlDbType.Char).Value = _scope;

                cn.Open();

                return ExecuteDataSet(cmd, "GetCode");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 최상위 직급,직책 코드 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_scope">The _scope.</param>
        /// <param name="_type">The _type.</param>
        /// <param name="_activeCode">if set to <c>true</c> [_active code].</param>
        /// <returns></returns>
        public int GetRootCode(int _companyID, string _scope, bool _activeCode)
        {
            string activeCode = (_activeCode) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelGetRootCodeID");
                query = string.Format(query, activeCode);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Scope", SqlDbType.Char, 1).Value = _scope;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자가 입력한 직급,직책 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_scope">The _scope.</param>
        /// <param name="_activeCode">if set to <c>true</c> [_active code].</param>
        /// <returns></returns>
        public DataSet GetCodeByScope(int _companyID, string _scope, bool _activeCode)
        {
            string activeCode = (_activeCode) ? "'1'" : "'0','1'";
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelByTypeScope");
                query = string.Format(query, activeCode);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Scope", SqlDbType.Char, 1).Value = _scope;
                cn.Open();

                return ExecuteDataSet(cmd, "GetCode");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 코드정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_scope">The _scope.</param>
        /// <param name="_activeCode">if set to <c>true</c> [_active code].</param>
        /// <returns></returns>
        public List<BE.CodeInfo> GetCodeByID(int _companyID, int _codeID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelByCodeID");

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _codeID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCodeList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 하위코드로 코드정보들 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <returns></returns>
        public List<BE.CodeInfo> GetCodeByPID(int _companyID, int _parentCodeID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelByPCodeID");

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ParentCodeID", SqlDbType.Int).Value = _parentCodeID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCodeList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// InterfaceCode 로 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_interfaceCodeID">The _interface code ID.</param>
        /// <returns></returns>
        public List<BE.CodeInfo> GetCodeByInterfaceID(int _companyID, string _interfaceCodeID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Code_SelByInterfaceCode");

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 300).Value = _interfaceCodeID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetCodeList(reader);
                }
            }
        }

        #endregion GET

        #region ADD

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 코드 추가
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public int Add(BE.CodeInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Code_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentCodeID", SqlDbType.Int).Value = _info.ParentCodeID;
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 20).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 20).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _info.Type.ToNull().ToXSS();
                    cmd.Parameters.Add("@Scope", SqlDbType.Char, 1).Value = _info.Scope.ToNull().ToXSS();
                    cmd.Parameters.Add("@Extension1", SqlDbType.NVarChar, 100).Value = _info.Extension1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Extension2", SqlDbType.NVarChar, 100).Value = _info.Extension2.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCode", SqlDbType.Char, 1).Value = _info.ActiveCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _info.AccessID;

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

        #endregion ADD

        #region MOD

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 코드 수정
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public bool Mod(BE.CodeInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Code_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _info.CodeID;
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 20).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 20).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _info.Type.ToNull().ToXSS();
                    cmd.Parameters.Add("@Scope", SqlDbType.Char, 1).Value = _info.Scope.ToNull().ToXSS();
                    cmd.Parameters.Add("@Extension1", SqlDbType.NVarChar, 100).Value = _info.Extension1.ToNull().ToXSS();
                    cmd.Parameters.Add("@Extension2", SqlDbType.NVarChar, 100).Value = _info.Extension2.ToNull().ToXSS();
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCode", SqlDbType.Char, 1).Value = _info.ActiveCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@AccessID", SqlDbType.Int).Value = _info.AccessID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
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
        /// 용도 : 순서변경하기
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

                    using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            SqlCommand cmd = new SqlCommand("DP_COM_Code_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _listStr[i];
                            cmd.Parameters.Add("@CodeOrder", SqlDbType.Int).Value = (i + 1);

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

        #endregion MOD

        #region DEL

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 코드 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _codeID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Code_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CodeID", SqlDbType.Int).Value = _codeID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion DEL
    }
}