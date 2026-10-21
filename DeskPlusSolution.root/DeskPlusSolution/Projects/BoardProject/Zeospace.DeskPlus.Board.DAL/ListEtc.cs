using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class ListEtc : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the list etc.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private ListEtcInfo GetListEtc(IDataReader _reader)
        {
            return new ListEtcInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                Etc1 = _reader["Etc1"].ToString().ToNull(),
                Etc2 = _reader["Etc2"].ToString().ToNull(),
                Etc3 = _reader["Etc3"].ToString().ToNull(),
                Etc4 = _reader["Etc4"].ToString().ToNull(),
                Etc5 = _reader["Etc5"].ToString().ToNull(),
                Etc6 = _reader["Etc6"].ToString().ToNull(),
                Etc7 = _reader["Etc7"].ToString().ToNull(),
                Etc8 = (_reader["Etc8"] == DBNull.Value) ? -1 : _reader["Etc8"].ToString().ToNumber<int>(),
                Etc9 = (_reader["Etc9"] == DBNull.Value) ? -1 : _reader["Etc9"].ToString().ToNumber<int>()
            };
        }

        protected List<ListEtcInfo> GetListEtcList(IDataReader reader)
        {
            List<ListEtcInfo> list = new List<ListEtcInfo>();
            while (reader.Read())
                list.Add(GetListEtc(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _listinfoetc.</param>
        /// <returns></returns>
        public int Add(ListEtcInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ListEtc_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;

                    cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 300).Value = _info.Etc1.ToXSS();
                    cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 300).Value = _info.Etc2.ToXSS();
                    cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 300).Value = _info.Etc3.ToXSS();
                    cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 300).Value = _info.Etc4.ToXSS();
                    cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 300).Value = _info.Etc5.ToXSS();
                    cmd.Parameters.Add("@Etc6", SqlDbType.NVarChar, 300).Value = _info.Etc6.ToXSS();
                    cmd.Parameters.Add("@Etc7", SqlDbType.NVarChar, 300).Value = _info.Etc7.ToXSS();
                    cmd.Parameters.Add("@Etc8", SqlDbType.Int).Value = _info.Etc8;
                    cmd.Parameters.Add("@Etc9", SqlDbType.Int).Value = _info.Etc9;

                    cn.Open();
                    return ExecuteNonQuery(cmd);
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
        /// etc 정보 수정하기
        /// </summary>
        /// <param name="_listinfoetc">The _listinfoetc.</param>
        /// <returns></returns>
        public bool Mod(ListEtcInfo _listinfoetc)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfoetc.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ListEtc_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfoetc.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _listinfoetc.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listinfoetc.ListID;

                    cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc1.ToXSS();
                    cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc2.ToXSS();
                    cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc3.ToXSS();
                    cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc4.ToXSS();
                    cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc5.ToXSS();
                    cmd.Parameters.Add("@Etc6", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc6.ToXSS();
                    cmd.Parameters.Add("@Etc7", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc7.ToXSS();
                    cmd.Parameters.Add("@Etc8", SqlDbType.Int).Value = _listinfoetc.Etc8;
                    cmd.Parameters.Add("@Etc9", SqlDbType.Int).Value = _listinfoetc.Etc9;

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
        /// Dels the list etc.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public int Del(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_ListEtc_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cn.Open();
                    return ExecuteNonQuery(cmd);
                }
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
        /// 작성자   : daniel
        /// 작성일자 : 2011-04-25
        /// 작성목적 : 리스트기타 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_ListID">The _ list ID.</param>
        /// <returns></returns>
        public List<BE.ListEtcInfo> GetList(int _companyID, int _categoryID, int _ListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListEtc_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                    cn.Open();

                    return GetListEtcList(ExecuteDataTableReader(cmd));
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
        /// ListID로 Etc정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_ListID"></param>
        /// <returns></returns>
        public List<BE.ListEtcInfo> GetListID(int _companyID, int _ListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListEtc_SelByListID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                    cn.Open();

                    return GetListEtcList(ExecuteDataTableReader(cmd));
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
        /// 작성자   : shine
        /// 작성일자 : 2014-02-28
        /// 작성목적 : 리스트기타 가져오기(팝업용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_nowDate">The _now date.</param>
        /// <returns></returns>
        public DataSet GetPopUpList(int _companyID, string _nowDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListEtc_Sel");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@NowDate", SqlDbType.Char, 8).Value = _nowDate;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetPopUpList");
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
        /// 작성자   : shine
        /// 작성일자 : 2014-02-28
        /// 작성목적 : 리스트기타 가져오기(팝업용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_nowDate">The _now date.</param>
        /// <returns></returns>
        public DataSet GetMainPopUpList(int _companyID, string _nowDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListEtc_SelMainPopup");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@NowDate", SqlDbType.Char, 8).Value = _nowDate;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMainPopUpList");
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
        /// 작성자   : shine
        /// 작성일자 : 2014-02-28
        /// 작성목적 : 현재등록된 메인팝업 카운트
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_nowDate">The _now date.</param>
        /// <returns></returns>
        public int GetMainPopUpCount(int _companyID, int _categoryID, int _listID, string _startDate, string _endDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_ListEtc_SelMainPopupCount");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _startDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _endDate;
                    cn.Open();

                    return int.Parse(ExecuteScalar(cmd).ToString());
                }
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