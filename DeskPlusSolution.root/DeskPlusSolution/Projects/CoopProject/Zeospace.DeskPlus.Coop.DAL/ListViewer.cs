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
    public class ListViewer : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListViewerInfo GetListViewer(IDataReader _reader)
        {
            return new ListViewerInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                CRUDDate = (_reader["CRUDDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["CRUDDate"],
                CRUDType = _reader["CRUDType"].ToString().ToNull(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                TOCC = _reader["TOCC"].ToString().ToNull(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),                

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

        protected List<ListViewerInfo> GetListViewerList(IDataReader reader)
        {
            List<ListViewerInfo> list = new List<ListViewerInfo>();
            while (reader.Read())
                list.Add(GetListViewer(reader));
            return list;
        }

        #endregion

        #region 추가/수정/삭제
        

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 조회 추가
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(ListViewerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_ListViewer_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@TOCC", SqlDbType.Char, 2).Value = _info.TOCC;

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
        #endregion
    }
}
