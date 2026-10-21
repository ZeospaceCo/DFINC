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
    public class Exam : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ExamInfo GetExam(IDataReader _reader)
        {
            return new ExamInfo 
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProposalNo = (_reader["ProposalNo"] == DBNull.Value) ? -1 : _reader["ProposalNo"].ToString().ToNumber<int>(),
                Degree = (_reader["Degree"] == DBNull.Value) ? -1 : _reader["Degree"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                ItemNo = (_reader["ItemNo"] == DBNull.Value) ? -1 : _reader["ItemNo"].ToString().ToNumber<int>(),
                Score1 = _reader["Score1"].ToString().ToDouble<double>(),
                Score2 = _reader["Score2"].ToString().ToDouble<double>(),
                Score3 = _reader["Score3"].ToString().ToDouble<double>(),
                Score4 = _reader["Score4"].ToString().ToDouble<double>(),
                Score5 = _reader["Score5"].ToString().ToDouble<double>(),
                Score6 = _reader["Score6"].ToString().ToDouble<double>(),
                Score7 = _reader["Score7"].ToString().ToDouble<double>(),
                Score8 = _reader["Score8"].ToString().ToDouble<double>(),
                Score9 = _reader["Score9"].ToString().ToDouble<double>(),
                Score10 = _reader["Score10"].ToString().ToDouble<double>(),
                TotalScore = _reader["TotalScore"].ToString().ToDouble<double>(),
                JudgeView = _reader["JudgeView"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<ExamInfo> GetItemSubList(IDataReader reader)
        {
            List<ExamInfo> list = new List<ExamInfo>();
            while (reader.Read())
                list.Add(GetExam(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 진행사항 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <returns></returns>
        public DataTableReader GetStatusList(int _companyID, int _proposalNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Exam_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 평가자의 심사 의견 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_proposalNo">The _proposal no.</param>
        /// <param name="_judgeUserID">The _judge user ID.</param>
        /// <returns></returns>
        public List<BE.ExamInfo> GetExam(int _companyID, int _proposalNo, int _judgeUserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Exam_SelByUserID");
                

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _proposalNo;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _judgeUserID;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetItemSubList(reader);
                }
            }
        }

        #endregion

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 1차 평가자의 평가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.ExamInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PROP_Exam_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProposalNo", SqlDbType.Int).Value = _info.ProposalNo;
                    cmd.Parameters.Add("@Degree", SqlDbType.Int).Value = _info.Degree;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ItemNo", SqlDbType.Int).Value = _info.ItemNo;
                    cmd.Parameters.Add("@Score1", SqlDbType.Float).Value = _info.Score1;
                    cmd.Parameters.Add("@Score2", SqlDbType.Float).Value = _info.Score2;
                    cmd.Parameters.Add("@Score3", SqlDbType.Float).Value = _info.Score3;
                    cmd.Parameters.Add("@Score4", SqlDbType.Float).Value = _info.Score4;
                    cmd.Parameters.Add("@Score5", SqlDbType.Float).Value = _info.Score5;
                    cmd.Parameters.Add("@Score6", SqlDbType.Float).Value = _info.Score6;
                    cmd.Parameters.Add("@Score7", SqlDbType.Float).Value = _info.Score7;
                    cmd.Parameters.Add("@Score8", SqlDbType.Float).Value = _info.Score8;
                    cmd.Parameters.Add("@Score9", SqlDbType.Float).Value = _info.Score9;
                    cmd.Parameters.Add("@Score10", SqlDbType.Float).Value = _info.Score10;
                    cmd.Parameters.Add("@TotalScore", SqlDbType.Float).Value = _info.TotalScore;
                    cmd.Parameters.Add("@JudgeView", SqlDbType.VarChar,1000).Value = _info.JudgeView.ToNull();

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

        #region MOD

        #endregion
    }
}
