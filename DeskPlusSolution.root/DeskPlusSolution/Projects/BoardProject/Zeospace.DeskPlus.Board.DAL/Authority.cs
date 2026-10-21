using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class Authority : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private AuthorityInfo GetAuthority(IDataReader _reader)
        {
            return new AuthorityInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                Type = _reader["Type"].ToString().ToNull(),
                OwnerID = (_reader["OwnerID"] == DBNull.Value) ? -1 : _reader["OwnerID"].ToString().ToNumber<int>(),
                Authority = _reader["Authority"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<AuthorityInfo> GetAuthorityList(IDataReader reader)
        {
            List<AuthorityInfo> list = new List<AuthorityInfo>();
            while (reader.Read())
                list.Add(GetAuthority(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 추가 및 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.AuthorityInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Auth_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _listinfo.CategoryID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _listinfo.Type;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = _listinfo.OwnerID;
                    cmd.Parameters.Add("@Authority", SqlDbType.Char, 1).Value = _listinfo.Authority;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _listinfo.WriteUserID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
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
        /// 권한 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Auth_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 100).Value = _categoryID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 게시판 권한 중, 부서권한을 사용자로 풀어서 저장함.
        /// 2015.12.16
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_categoryID">The _category identifier.</param>
        /// <returns></returns>
        public bool BoardUser(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_SEARCH_BoardUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 100).Value = _categoryID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 권한 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public DataTableReader GetAuthority(int _companyID, int _categoryID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Authority_SelByID");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 나의 권한 카테고리 아이디 값들
        /// _childOwnerList : 부서들 또는 사용자
        /// gubun : 1(사용자) 2(부서) 3(전체)
        /// 반환값 : CategoryID, Authority
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_childCatList">The _category ID.</param>
        /// <param name="_childOwnerList">The _owner I ds.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategory(int _companyID, string _childCatList, string _childOwnerList, string _gubun)
        {
            //key 는 중복가능함
            NameValueCollection authCatList = new NameValueCollection();

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Auth_SelGetMyAuth", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar, 10).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 5000).Value = _childCatList;
                    cmd.Parameters.Add("@OwnerID", SqlDbType.VarChar, 5000).Value = _childOwnerList;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;

                    cn.Open();

                    DataTableReader dtr = ExecuteDataTableReader(cmd);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                authCatList.Add(dtr["CategoryID"].ToString(), dtr["Authority"].ToString());
                            }
                        }
                    }
                }

                return authCatList;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 가져오기
    }
}