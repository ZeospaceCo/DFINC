using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class MyTemplateEtc : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private MyTemplateEtcInfo GetMyTemplateEtc(IDataReader _reader)
        {
            return new MyTemplateEtcInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                Etc1 = _reader["Etc1"].ToString().ToNull(),
                Etc2 = _reader["Etc2"].ToString().ToNull(),
                Etc3 = _reader["Etc3"].ToString().ToNull(),
                Etc4 = _reader["Etc4"].ToString().ToNull(),
                Etc5 = _reader["Etc5"].ToString().ToNull()
            };
        }

        protected List<MyTemplateEtcInfo> GetMyTemplateEtcList(IDataReader reader)
        {
            List<MyTemplateEtcInfo> list = new List<MyTemplateEtcInfo>();
            while (reader.Read())
                list.Add(GetMyTemplateEtc(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장 etc 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.MyTemplateEtcInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplateEtc_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;

                    cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 600).Value = _info.Etc1.ToXSS();
                    cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 600).Value = _info.Etc2.ToXSS();
                    cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 600).Value = _info.Etc3.ToXSS();
                    cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 600).Value = _info.Etc4.ToXSS();
                    cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 600).Value = _info.Etc5.ToXSS();

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
        public bool Mod(MyTemplateEtcInfo _listinfoetc)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfoetc.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplateEtc_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfoetc.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _listinfoetc.CategoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listinfoetc.ListID;

                    cmd.Parameters.Add("@Etc1", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc1.ToXSS();
                    cmd.Parameters.Add("@Etc2", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc2.ToXSS();
                    cmd.Parameters.Add("@Etc3", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc3.ToXSS();
                    cmd.Parameters.Add("@Etc4", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc4.ToXSS();
                    cmd.Parameters.Add("@Etc5", SqlDbType.NVarChar, 600).Value = _listinfoetc.Etc5.ToXSS();

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
        /// 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_MyTemplateEtc_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
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
        public List<BE.MyTemplateEtcInfo> GetList(int _companyID, int _categoryID, int _ListID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_MyTemplateEtc_SelByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _ListID;
                    cn.Open();

                    return GetMyTemplateEtcList(ExecuteDataTableReader(cmd));
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