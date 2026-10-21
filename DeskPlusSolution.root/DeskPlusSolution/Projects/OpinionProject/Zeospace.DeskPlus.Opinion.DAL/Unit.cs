using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Opinion.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Opinion.DAL
{
    public class Unit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.UnitInfo GetUnit(IDataReader _idr)
        {
            return new UnitInfo
            {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                UnitID = (_idr["UnitID"] == DBNull.Value) ? -1 : _idr["UnitID"].ToString().ToNumber<int>(),
                Section = _idr["Section"].ToString(),
                Title = _idr["Title"].ToString(),
                Type = _idr["Type"].ToString(),
                IsReq = _idr["IsReq"].ToString(),
                Point = (_idr["Point"] == DBNull.Value) ? -1 : _idr["Point"].ToString().ToNumber<int>(),
                IsActive = _idr["IsActive"].ToString(),
                IsHorizon = _idr["IsHorizon"].ToString(),
                WriteDate = (_idr["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_idr["WriteDate"],
                WriteUserID = (_idr["WriteUserID"] == DBNull.Value) ? -1 : _idr["WriteUserID"].ToString().ToNumber<int>()
            };
        }

        protected List<BE.UnitInfo> GetUnitList(IDataReader reader)
        {
            List<BE.UnitInfo> list = new List<UnitInfo>();
            while (reader.Read())
            {
                list.Add(GetUnit(reader));
            }
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 질문추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public int Add(UnitInfo _unitInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_unitInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _unitInfo.CompanyID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _unitInfo.Section;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 256).Value = _unitInfo.Title.ToXSS();
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _unitInfo.Type;
                    cmd.Parameters.Add("@IsReq", SqlDbType.Char, 1).Value = _unitInfo.IsReq;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _unitInfo.Point;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _unitInfo.IsActive;
                    cmd.Parameters.Add("@IsHorizon", SqlDbType.Char, 1).Value = _unitInfo.IsHorizon;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _unitInfo.WriteUserID;

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
        /// 질문수정
        /// </summary>
        /// <param name="_unitInfo"></param>
        /// <returns></returns>
        public bool Mod(UnitInfo _unitInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_unitInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _unitInfo.CompanyID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitInfo.UnitID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _unitInfo.Section;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 256).Value = _unitInfo.Title.ToXSS();
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _unitInfo.Type;
                    cmd.Parameters.Add("@IsReq", SqlDbType.Char, 1).Value = _unitInfo.IsReq;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _unitInfo.Point;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _unitInfo.IsActive;
                    cmd.Parameters.Add("@IsHorizon", SqlDbType.Char, 1).Value = _unitInfo.IsHorizon;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _unitInfo.WriteUserID;

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
        /// 질문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _unitID)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

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
        /// 활성화/비활성화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _unitID, string _isActive)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_UpdIsActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Char, 1).Value = _isActive;

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

        #endregion

        #region 가져오기

        /// <summary>
        /// 질문 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public List<BE.UnitInfo> GetUnit(int _companyID, int _unitID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Unit_SelByUnitID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUnitList(reader);
                }
            }
        }

        /// <summary>
        /// 질문리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetUnitList(int _companyID, string _section, string _pageSize, string _pageNo, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal, string _order)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                    cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.DAL.Unit", ex);
                return null;
            }
        }

        /// <summary>
        /// 질문리스트 갯수 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        public int GetUnitList(int _companyID, string _section, string _sKey, string _sVal, string _sPeriod, string _sPeriodVal)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Unit_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                cmd.Parameters.Add("@Period", SqlDbType.NVarChar, 50).Value = _sPeriod;
                cmd.Parameters.Add("@PeriodValue", SqlDbType.NVarChar, 50).Value = _sPeriodVal;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }

        /// <summary>
        /// 질문리스트 가져오기(설문수정)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetUnitListByQuesID(int _companyID, int _quesID, int _pageSize, int _pageNo, string _sKey, string _sVal, string _order)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_Unit_SelQuesIDPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageNo", SqlDbType.Int).Value = _pageNo;
                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;
                    cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 50).Value = _order;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.DAL.Unit", ex);
                return null;
            }
        }

        /// <summary>
        /// 질문리스트 갯수 가져오기(설문수정)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sKey"></param>
        /// <param name="_sVal"></param>
        /// <returns></returns>
        public int GetUnitListByQuesID(int _companyID, int _quesID, string _sKey, string _sVal)
        {
            int rtnCnt = 0;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_Unit_SelQuesIDPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@QuesID", SqlDbType.Int).Value = _quesID;
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _sKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 50).Value = _sVal;

                cn.Open();

                rtnCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }
            return rtnCnt;
        }
        #endregion
    }
}
