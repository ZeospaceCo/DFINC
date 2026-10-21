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
    public class APPBaseDoc : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private APPBaseDocInfo GetListBase(IDataReader _reader)
        {
            return new APPBaseDocInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                APPBaseDocID = (_reader["APPBaseDocID"] == DBNull.Value) ? -1 : _reader["APPBaseDocID"].ToString().ToNumber<int>(),
                DocumentID = (_reader["DocumentID"] == DBNull.Value) ? -1 : _reader["DocumentID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                ParentDocumentID = (_reader["ParentDocumentID"] == DBNull.Value) ? -1 : _reader["ParentDocumentID"].ToString().ToNumber<int>(),
                Name = _reader["Name"].ToString().ToNull(),
                IsLegacy = _reader["IsLegacy"].ToString().ToNull(),
                Path = _reader["Path"].ToString().ToNull(),
                BaseOrder = (_reader["BaseOrder"] == DBNull.Value) ? -1 : _reader["BaseOrder"].ToString().ToNumber<int>(),

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

        protected List<APPBaseDocInfo> GetListBaseList(IDataReader reader)
        {
            List<APPBaseDocInfo> list = new List<APPBaseDocInfo>();
            while (reader.Read())
                list.Add(GetListBase(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자   : shimpark
        /// 작성일자 : 2010-12-06
        /// 작성목적 : 근거문서 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public DataTableReader GetAppBaseDoc(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COOP, "QRY_COOP_APPBaseDoc_Sel");
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region ADD

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 근거문서 추가하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(APPBaseDocInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_APPBaseDoc_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _info.DocumentID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = _info.Name.ToNull();
                    cmd.Parameters.Add("@IsLegacy", SqlDbType.Char, 1).Value = _info.IsLegacy.ToNull();
                    cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 500).Value = _info.Path.ToNull();
                    cmd.Parameters.Add("@BaseOrder", SqlDbType.Int).Value = _info.BaseOrder;

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

        #region MOD
        #endregion

        #region DEL

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 근거문서 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_targetListID">The _target list ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_APPBaseDoc_DelByListID", cn);

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

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2008-08-05
        /// 작성목적 : 근거문서 삭제하기(개별)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_documentID">The _document ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _listID, int _documentID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COOP_APPBaseDoc_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@DocumentID", SqlDbType.Int).Value = _documentID;

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
