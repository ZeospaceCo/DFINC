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
    public class ForwardListTO : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ForwardListTOInfo GetForwardListTO(IDataReader _reader)
        {
            return new ForwardListTOInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ForwardListID = (_reader["ForwardListID"] == DBNull.Value) ? -1 : _reader["ForwardListID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ListTOID = (_reader["ListTOID"] == DBNull.Value) ? -1 : _reader["ListTOID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                Orders = (_reader["Orders"] == DBNull.Value) ? -1 : _reader["Orders"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDateYMD = _reader["WriteDateYMD"].ToString().ToNull(),

                #region ####  부서,직급,직책 다국어 시작 #######
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull()
                #endregion
            };
        }

        protected List<ForwardListTOInfo> GetListTOList(IDataReader reader)
        {
            List<ForwardListTOInfo> list = new List<ForwardListTOInfo>();
            while (reader.Read())
                list.Add(GetForwardListTO(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 전달받은 수신자 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <returns></returns>
        public List<BE.ForwardListTOInfo> GetForwardListTo(int _companyID, int _forwardListID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_ForwardListTO_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ForwardListID", SqlDbType.Int).Value = _forwardListID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetListTOList(reader);
                }
            }
        }

        #endregion
    }
}
