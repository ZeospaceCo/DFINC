using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class SYSCode : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private SYSCodeInfo GetSysCode(IDataReader _reader)
        {
            return new SYSCodeInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                SYSCodeID = (_reader["SYSCodeID"] == DBNull.Value) ? -1 : _reader["SYSCodeID"].ToString().ToNumber<int>(),
                InterfaceCode = _reader["InterfaceCode"].ToString().ToNull(),
                CodeID = _reader["CodeID"].ToString().ToNull(),
                ParentCodeID = _reader["ParentCodeID"].ToString().ToNull(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ActiveCode = _reader["ActiveCode"].ToString().ToNull(),
                CodeOrder = (_reader["CodeOrder"] == DBNull.Value) ? -1 : _reader["CodeOrder"].ToString().ToNumber<int>(),
                Description = _reader["Description"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the user mail domain list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<SYSCodeInfo> GetSysCodeList(IDataReader reader)
        {
            List<SYSCodeInfo> list = new List<SYSCodeInfo>();
            while (reader.Read())
                list.Add(GetSysCode(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(SYSCodeInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 100).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 20).Value = _info.CodeID.ToNull().ToXSS();
                    cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _info.ParentCodeID.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCode", SqlDbType.Char, 1).Value = _info.ActiveCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToXSS();

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

        /// <summary>
        /// 심재운
        /// 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(SYSCodeInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@InterfaceCode", SqlDbType.VarChar, 100).Value = _info.InterfaceCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 20).Value = _info.CodeID.ToNull().ToXSS();
                    cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _info.ParentCodeID.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@ActiveCode", SqlDbType.Char, 1).Value = _info.ActiveCode.ToNull().ToXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = _info.Description.ToXSS();

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

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_COM_SYSCode_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@SYSCodeID", SqlDbType.Int).Value = _listStr[i];
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

        /// <summary>
        /// 심재운
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_sysCodeIDList">The _sys code ID list.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _sysCodeIDList)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SYSCodeID", SqlDbType.VarChar, 1000).Value = _sysCodeIDList;

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
        /// 심재운
        /// 비활성화 및 활성화
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_sysCodeID">The _sys code ID.</param>
        /// <returns></returns>
        public bool ModActive(int _companyID, int _sysCodeID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_UpdByActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SYSCodeID", SqlDbType.Int).Value = _sysCodeID;

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : sysCodeID 별 자료 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_sysCodeID">The _sys code ID.</param>
        /// <returns></returns>
        public List<BE.SYSCodeInfo> GetSysCode(int _companyID, int _sysCodeID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelBySysCodeID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SysCodeID", SqlDbType.Int).Value = _sysCodeID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSysCodeList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : codeid 별 자료 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <returns></returns>
        public List<BE.SYSCodeInfo> GetSysCodeByCodeID(int _companyID, string _codeID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelByCodeID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 20).Value = _codeID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetSysCodeList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 하위 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <returns></returns>
        public DataSet GetSysCodeByPID(int _companyID, string _parentCodeID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelByPID");

                    if (_parentCodeID == "0")
                    {
                        query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelByPIDNull");
                    }

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    if (_parentCodeID != "0")
                    {
                        cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _parentCodeID;
                    }

                    cn.Open();

                    return ExecuteDataSet(cmd, "SysCodeRelation");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _parentCodeID, string _searchKey, string _searchValue)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_SelPageCount", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _parentCodeID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 페이징 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <param name="_searchKey">The _search key.</param>
        /// <param name="_searchValue">The _search value.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _parentCodeID,
            string _pageSize, string _pageSizePageNo, string _searchKey, string _searchValue, string _order)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_COM_SYSCode_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _parentCodeID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// codeid 로 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCodeID">The _parent code ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <returns></returns>
        public int GetCountByCodeID(int _companyID, string _parentCodeID, string _codeID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelCntByCodeID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ParentCodeID", SqlDbType.VarChar, 20).Value = _parentCodeID;
                cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 20).Value = _codeID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 상위 코드로 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_codeID">The _code ID.</param>
        /// <returns></returns>
        public int GetCountByRootCodeID(int _companyID, string _codeID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_SYSCode_SelCntByRootCodeID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@CodeID", SqlDbType.VarChar, 20).Value = _codeID;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 가져오기
    }
}