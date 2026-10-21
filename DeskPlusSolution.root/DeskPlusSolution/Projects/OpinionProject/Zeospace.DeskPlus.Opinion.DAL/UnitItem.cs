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
    public class UnitItem : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.UnitItemInfo GetUnitItem(IDataReader _idr)
        {
            return new UnitItemInfo
            {
                CompanyID = (_idr["CompanyID"] == DBNull.Value) ? -1 : _idr["CompanyID"].ToString().ToNumber<int>(),
                UnitID = (_idr["UnitID"] == DBNull.Value) ? -1 : _idr["UnitID"].ToString().ToNumber<int>(),
                UnitItemID = (_idr["UnitItemID"] == DBNull.Value) ? -1 : _idr["UnitItemID"].ToString().ToNumber<int>(),
                Type =_idr["Type"].ToString(),
                Cont =_idr["Cont"].ToString(),
                ImgUrl = _idr["ImgUrl"].ToString(),
                ImgName = _idr["ImgName"].ToString(),
                Seq = (_idr["Seq"] == DBNull.Value) ? -1 : _idr["Seq"].ToString().ToNumber<int>()
            };
        }

        protected List<BE.UnitItemInfo> GetUnitItemList(IDataReader reader)
        {
            List<BE.UnitItemInfo> list = new List<UnitItemInfo>();
            while (reader.Read())
            {
                list.Add(GetUnitItem(reader));
            }
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 질문항목추가
        /// </summary>
        /// <param name="_unitItemInfo"></param>
        /// <returns></returns>
        public int Add(UnitItemInfo _unitItemInfo)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_unitItemInfo.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_UnitItem_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _unitItemInfo.CompanyID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitItemInfo.UnitID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _unitItemInfo.Type;
                    cmd.Parameters.Add("@Cont", SqlDbType.NVarChar, 500).Value = _unitItemInfo.Cont.ToXSS();
                    cmd.Parameters.Add("@ImgUrl", SqlDbType.NVarChar, 300).Value = (_unitItemInfo.ImgUrl == null) ? "" : _unitItemInfo.ImgUrl;
                    cmd.Parameters.Add("@ImgName", SqlDbType.NVarChar, 100).Value = (_unitItemInfo.ImgName == null) ? "" : _unitItemInfo.ImgName;
                    cmd.Parameters.Add("@Seq", SqlDbType.Int).Value = _unitItemInfo.Seq;

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
        /// 질문항목삭제
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
                    SqlCommand cmd = new SqlCommand("DP_OPI_UnitItem_Del", cn);
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
        /// 항목순서변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitItemID"></param>
        /// <param name="_seq"></param>
        /// <returns></returns>
        public bool ModSeq(int _companyID, int _unitItemID, int _seq)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_OPI_UnitItem_UpdByUnitItemID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UnitItemID", SqlDbType.Int).Value = _unitItemID;
                    cmd.Parameters.Add("@Seq", SqlDbType.Int).Value = _seq;

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
        /// 질문항목가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public List<BE.UnitItemInfo> GetUnitItem(int _companyID, int _unitID)
        {
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                SqlCommand cmd = new SqlCommand("DP_OPI_UnitItem_SelByUnitID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetUnitItemList(reader);
                }
            }
        }

        #endregion
    }
}
