using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Proposal.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
namespace Zeospace.DeskPlus.Proposal.DAL
{
    public class Estimate : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private EstimateInfo GetEstimate(IDataReader _reader)
        {
            return new EstimateInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                EstimateID = (_reader["EstimateID"] == DBNull.Value) ? -1 : _reader["EstimateID"].ToString().ToNumber<int>(),
                Item1 = _reader["Item1"].ToString().ToNull(),
                Score1 = _reader["Score1"].ToString().ToNull(),
                Comment1 = _reader["Comment1"].ToString().ToNull(),
                Sc_type1 = _reader["Sc_type1"].ToString().ToNull(),
                Item2 = _reader["Item2"].ToString().ToNull(),
                Score2 = _reader["Score2"].ToString().ToNull(),
                Comment2 = _reader["Comment2"].ToString().ToNull(),
                Sc_type2 = _reader["Sc_type2"].ToString().ToNull(),
                Item3 = _reader["Item3"].ToString().ToNull(),
                Score3 = _reader["Score3"].ToString().ToNull(),
                Comment3 = _reader["Comment3"].ToString().ToNull(),
                Sc_type3 = _reader["Sc_type3"].ToString().ToNull(),
                Item4 = _reader["Item4"].ToString().ToNull(),
                Score4 = _reader["Score4"].ToString().ToNull(),
                Comment4 = _reader["Comment4"].ToString().ToNull(),
                Sc_type4 = _reader["Sc_type4"].ToString().ToNull(),
                Item5 = _reader["Item5"].ToString().ToNull(),
                Score5 = _reader["Score5"].ToString().ToNull(),
                Comment5 = _reader["Comment5"].ToString().ToNull(),
                Sc_type5 = _reader["Sc_type5"].ToString().ToNull(),
                Item6 = _reader["Item6"].ToString().ToNull(),
                Score6 = _reader["Score6"].ToString().ToNull(),
                Comment6 = _reader["Comment6"].ToString().ToNull(),
                Sc_type6 = _reader["Sc_type6"].ToString().ToNull(),
                Item7 = _reader["Item7"].ToString().ToNull(),
                Score7 = _reader["Score7"].ToString().ToNull(),
                Comment7 = _reader["Comment7"].ToString().ToNull(),
                Sc_type7 = _reader["Sc_type7"].ToString().ToNull(),
                Item8 = _reader["Item8"].ToString().ToNull(),
                Score8 = _reader["Score8"].ToString().ToNull(),
                Comment8 = _reader["Comment8"].ToString().ToNull(),
                Sc_type8 = _reader["Sc_type8"].ToString().ToNull(),
                Item9 = _reader["Item9"].ToString().ToNull(),
                Score9 = _reader["Score9"].ToString().ToNull(),
                Comment9 = _reader["Comment9"].ToString().ToNull(),
                Sc_type9 = _reader["Sc_type9"].ToString().ToNull(),
                Item10 = _reader["Item10"].ToString().ToNull(),
                Score10 = _reader["Score10"].ToString().ToNull(),
                Comment10 = _reader["Comment10"].ToString().ToNull(),
                Sc_type10 = _reader["Sc_type10"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<EstimateInfo> GetEstimateList(IDataReader reader)
        {
            List<EstimateInfo> list = new List<EstimateInfo>();
            while (reader.Read())
                list.Add(GetEstimate(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 평가항목 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.EstimateInfo> GetEstimateList(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Estimate_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetEstimateList(reader);
                }
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 선택한 평가항목 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public List<BE.EstimateInfo> GetEstimateList(int _companyID, int _estimateID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Estimate_SelByItemNo");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@EstimateID", SqlDbType.Int).Value = _estimateID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetEstimateList(reader);
                }
            }
        }
        #endregion

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-04-19
        /// 작성목적 : 평가항목 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.EstimateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_Estimate_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;

                    cmd.Parameters.Add("@Item1", SqlDbType.VarChar,100).Value = _info.Item1.ToNull();
                    cmd.Parameters.Add("@Score1", SqlDbType.Float).Value = _info.Score1.ToDouble<double>();
                    cmd.Parameters.Add("@Comment1", SqlDbType.VarChar,500).Value = _info.Comment1.ToNull();
                    cmd.Parameters.Add("@Sc_type1", SqlDbType.VarChar,500).Value = _info.Sc_type1.ToNull();
                    cmd.Parameters.Add("@Item2", SqlDbType.VarChar,100).Value = _info.Item2.ToNull();
                    cmd.Parameters.Add("@Score2", SqlDbType.Float).Value = _info.Score2.ToDouble<double>();
                    cmd.Parameters.Add("@Comment2", SqlDbType.VarChar,500).Value = _info.Comment2.ToNull();
                    cmd.Parameters.Add("@Sc_type2", SqlDbType.VarChar,500).Value = _info.Sc_type2.ToNull();
                    cmd.Parameters.Add("@Item3", SqlDbType.VarChar,50).Value = _info.Item3.ToNull();
                    cmd.Parameters.Add("@Score3", SqlDbType.Float).Value = _info.Score3.ToDouble<double>();
                    cmd.Parameters.Add("@Comment3", SqlDbType.VarChar,500).Value = _info.Comment3.ToNull();
                    cmd.Parameters.Add("@Sc_type3", SqlDbType.VarChar,500).Value = _info.Sc_type3.ToNull();
                    cmd.Parameters.Add("@Item4", SqlDbType.VarChar,50).Value = _info.Item4.ToNull();
                    cmd.Parameters.Add("@Score4", SqlDbType.Float).Value = _info.Score4.ToDouble<double>();
                    cmd.Parameters.Add("@Comment4", SqlDbType.VarChar,500).Value = _info.Comment4.ToNull();
                    cmd.Parameters.Add("@Sc_type4", SqlDbType.VarChar,500).Value = _info.Sc_type4.ToNull();
                    cmd.Parameters.Add("@Item5", SqlDbType.VarChar,50).Value = _info.Item5.ToNull();
                    cmd.Parameters.Add("@Score5", SqlDbType.Float).Value = _info.Score5.ToDouble<double>();
                    cmd.Parameters.Add("@Comment5", SqlDbType.VarChar,500).Value = _info.Comment5.ToNull();
                    cmd.Parameters.Add("@Sc_type5", SqlDbType.VarChar,500).Value = _info.Sc_type5.ToNull();
                    cmd.Parameters.Add("@Item6", SqlDbType.VarChar,50).Value = _info.Item6.ToNull();
                    cmd.Parameters.Add("@Score6", SqlDbType.Float).Value = _info.Score6.ToDouble<double>();
                    cmd.Parameters.Add("@Comment6", SqlDbType.VarChar,500).Value = _info.Comment6.ToNull();
                    cmd.Parameters.Add("@Sc_type6", SqlDbType.VarChar,500).Value = _info.Sc_type6.ToNull();
                    cmd.Parameters.Add("@Item7", SqlDbType.VarChar,50).Value = _info.Item7.ToNull();
                    cmd.Parameters.Add("@Score7", SqlDbType.Float).Value = _info.Score7.ToDouble<double>();
                    cmd.Parameters.Add("@Comment7", SqlDbType.VarChar,500).Value = _info.Comment7.ToNull();
                    cmd.Parameters.Add("@Sc_type7", SqlDbType.VarChar,500).Value = _info.Sc_type7.ToNull();
                    cmd.Parameters.Add("@Item8", SqlDbType.VarChar,50).Value = _info.Item8.ToNull();
                    cmd.Parameters.Add("@Score8", SqlDbType.Float).Value = _info.Score8.ToDouble<double>();
                    cmd.Parameters.Add("@Comment8", SqlDbType.VarChar,500).Value = _info.Comment8.ToNull();
                    cmd.Parameters.Add("@Sc_type8", SqlDbType.VarChar,500).Value = _info.Sc_type8.ToNull();
                    cmd.Parameters.Add("@Item9", SqlDbType.VarChar,50).Value = _info.Item9.ToNull();
                    cmd.Parameters.Add("@Score9", SqlDbType.Float).Value = _info.Score9.ToDouble<double>();
                    cmd.Parameters.Add("@Comment9", SqlDbType.VarChar,500).Value = _info.Comment9.ToNull();
                    cmd.Parameters.Add("@Sc_type9", SqlDbType.VarChar,500).Value = _info.Sc_type9.ToNull();
                    cmd.Parameters.Add("@Item10", SqlDbType.VarChar,50).Value = _info.Item10.ToNull();
                    cmd.Parameters.Add("@Score10", SqlDbType.Float).Value = _info.Score10.ToDouble<double>();
                    cmd.Parameters.Add("@Comment10", SqlDbType.VarChar,500).Value = _info.Comment10.ToNull();
                    cmd.Parameters.Add("@Sc_type10", SqlDbType.VarChar, 500).Value = _info.Sc_type10.ToNull(); 
                    
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
        #endregion
    }
}
