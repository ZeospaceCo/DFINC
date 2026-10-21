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
    public class Judge : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private JudgeInfo GetJudge(IDataReader _reader)
        {
            return new JudgeInfo 
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                Degree = (_reader["degree"] == DBNull.Value) ? -1 : _reader["degree"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                Name = _reader["Name"].ToString().ToNull(),
                ParentDeptName = _reader["ParentDeptName"].ToString().ToNull(),
                DeptName = _reader["DeptName"].ToString().ToNull(),
                TitleName = _reader["TitleName"].ToString().ToNull(),
                Status = _reader["Status"].ToString().ToNull(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"]
            };
        }

        protected List<JudgeInfo> GetJudgeList(IDataReader reader)
        {
            List<JudgeInfo> list = new List<JudgeInfo>();
            while (reader.Read())
                list.Add(GetJudge(reader));
            return list;
        }

        #endregion

        #region GET
        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 심사위원 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_degree">The _degree.</param>
        /// <returns></returns>
        public DataTableReader GetJudgeList(int _companyID, int _degree)
        {
            string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PROPOSAL, "QRY_PROP_Judge_SelByDegree");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Degree", SqlDbType.Int).Value = _degree;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
        #endregion

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 심사위원 추가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_degree">The _degree.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public bool Add(int _companyID, int _userID, int _degree, string _status)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_Judge_Ins", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@Degree", SqlDbType.Int).Value = _degree;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = _status;

                cn.Open();

                ExecuteNonQuery(cmd);

                return true;
            }
        }
        #endregion

        #region MOD
        #endregion

        #region DEL

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 심사위원 삭제
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PROP_Judge_Del", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                ExecuteNonQuery(cmd);

                return true;
            }
        }
        #endregion
    }
}
