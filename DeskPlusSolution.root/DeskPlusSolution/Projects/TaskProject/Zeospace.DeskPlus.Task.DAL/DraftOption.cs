using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class DraftOption : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private DraftOptionInfo GetDraftOption(IDataReader _reader)
        {
            return new DraftOptionInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                DraftID = (_reader["DraftID"] == DBNull.Value) ? -1 : _reader["DraftID"].ToString().ToNumber<int>(),
                ItemID = (_reader["ItemID"] == DBNull.Value) ? -1 : _reader["ItemID"].ToString().ToNumber<int>(),
                ItemSubID = (_reader["ItemSubID"] == DBNull.Value) ? -1 : _reader["ItemSubID"].ToString().ToNumber<int>(),
                CompleteRequestDate = _reader["CompleteRequestDate"].ToString().ToNull(),
                IsClose = _reader["IsClose"].ToString().ToNull(),
                IsGubun = _reader["IsGubun"].ToString().ToNull(),             
                SourceID = (_reader["SourceID"] == DBNull.Value) ? -1 : _reader["SourceID"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<DraftOptionInfo> GetDraftOptionList(IDataReader reader)
        {
            List<DraftOptionInfo> list = new List<DraftOptionInfo>();
            while (reader.Read())
                list.Add(GetDraftOption(reader));
            return list;
        }

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 옵셥 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public List<BE.DraftOptionInfo> GetDraftOption(int _companyID, int _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_DraftOption_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetDraftOptionList(reader);
                }
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.DraftOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftOption_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
                    cmd.Parameters.Add("@IsGubun", SqlDbType.Char, 1).Value = _info.IsGubun.ToNull();
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;
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
                throw;
            }
        }

       
        #endregion

        #region MOD()     

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 수정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(BE.DraftOptionInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_DraftOption_Upd", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;
                    cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _info.ItemID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _info.ItemSubID;
                    cmd.Parameters.Add("@CompleteRequestDate", SqlDbType.Char, 8).Value = _info.CompleteRequestDate.ToNull();
                    cmd.Parameters.Add("@IsClose", SqlDbType.Char, 1).Value = _info.IsClose.ToNull();
                    cmd.Parameters.Add("@IsGubun", SqlDbType.Char, 1).Value = _info.IsGubun.ToNull();
                    cmd.Parameters.Add("@SourceID", SqlDbType.Int).Value = _info.SourceID;                    

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
    }
}
