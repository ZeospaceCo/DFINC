using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Coop.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.DAL
{
    public class ListOption : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListOptionInfo GetListOption(IDataReader _reader)
        {
            return new ListOptionInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ItemID = (_reader["ItemID"] == DBNull.Value) ? -1 : _reader["ItemID"].ToString().ToNumber<int>(),
                ItemSubID = (_reader["ItemSubID"] == DBNull.Value) ? -1 : _reader["ItemSubID"].ToString().ToNumber<int>(),
                StartDate = _reader["StartDate"].ToString().ToNull(),
                EndDate = _reader["EndDate"].ToString().ToNull(),
                Difficultys = _reader["Difficultys"].ToString().ToNull(),
                Status = _reader["Status"].ToString().ToNull(),
                RequestDate = _reader["RequestDate"].ToString().ToNull(),
                CompleteRequestDate = _reader["CompleteRequestDate"].ToString().ToNull(),
                CompleteDate = _reader["CompleteDate"].ToString().ToNull(),
                IsClose = _reader["IsClose"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<ListOptionInfo> GetListOptionList(IDataReader reader)
        {
            List<ListOptionInfo> list = new List<ListOptionInfo>();
            while (reader.Read())
                list.Add(GetListOption(reader));
            return list;
        }

        #endregion

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 옵션 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listBaseID">The _list base ID.</param>
        /// <param name="_targetListID">The _target list ID.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public bool Add(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ListOption_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@Difficultys", SqlDbType.Char, 1).Value = _info.Difficultys.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
                    cmd.Parameters.Add("@RequestDate", SqlDbType.Char, 8).Value = _info.RequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.Char, 8).Value = _info.CompleteDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
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
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 옵션  수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listBaseID">The _list base ID.</param>
        /// <param name="_targetListID">The _target list ID.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public bool Mod(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ListOption_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@Difficultys", SqlDbType.Char, 1).Value = _info.Difficultys.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
                    cmd.Parameters.Add("@RequestDate", SqlDbType.Char, 8).Value = _info.RequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.Char, 8).Value = _info.CompleteDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
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
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 옵션 상태값만 수정
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listBaseID">The _list base ID.</param>
        /// <param name="_targetListID">The _target list ID.</param>
        /// <param name="_order">The _order.</param>
        /// <returns></returns>
        public bool ModStatus(BE.ListOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ListOption_UpdByStatus", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@Difficultys", SqlDbType.Char, 1).Value = _info.Difficultys.ToNull();
                    cmd.Parameters.Add("@Status", SqlDbType.Char, 1).Value = _info.Status.ToNull();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.Char, 8).Value = _info.CompleteDate.ToNull();
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
        /// 작성자   : shimpark
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ListOption_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 옵션정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public List<BE.ListOptionInfo> GetListOption(int _companyID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ListOption_Sel");          

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListOptionList(reader);
                }
            }
        }

        #endregion
    }
}
