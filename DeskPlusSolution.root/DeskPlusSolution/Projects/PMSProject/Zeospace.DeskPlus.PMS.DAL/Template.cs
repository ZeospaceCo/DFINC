using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Template : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private TemplateInfo GetTemplate(IDataReader _reader)
        {
            return new TemplateInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProjectID = (_reader["ProjectID"] == DBNull.Value) ? -1 : _reader["ProjectID"].ToString().ToNumber<int>(),
                TemplateID = (_reader["TemplateID"] == DBNull.Value) ? -1 : _reader["TemplateID"].ToString().ToNumber<int>(),
                TemplateNum = (_reader["TemplateNum"] == DBNull.Value) ? -1 : _reader["TemplateNum"].ToString().ToNumber<int>(),
                Subjects = _reader["Subjects"].ToString().ToNull(),
                Contents = _reader["Contents"].ToString().ToNull().ToEditorXSS(),
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
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],

                #region ####  부서,직급,직책 다국어 시작 #######

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

                #endregion ####  부서,직급,직책 다국어 시작 #######
            };
        }

        protected List<TemplateInfo> GetTemplateList(IDataReader reader)
        {
            List<TemplateInfo> list = new List<TemplateInfo>();
            while (reader.Read())
                list.Add(GetTemplate(reader));
            return list;
        }

        #endregion DataReader

        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-09
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public int Add(TemplateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Template_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    //cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _info.TemplateID;
                    cmd.Parameters.Add("@TemplateNum", SqlDbType.Int).Value = _info.TemplateNum;
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 100).Value = _info.Subjects.ToNull();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : list 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-02
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Mod(TemplateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Template_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _info.TemplateID;
                    cmd.Parameters.Add("@TemplateNum", SqlDbType.Int).Value = _info.TemplateNum;
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 100).Value = _info.Subjects.ToNull();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_templateID">The _template ID.</param>
        /// <returns></returns>
        public bool Del(TemplateInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Template_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _info.TemplateID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()

        #region GET()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 프로젝트별 단위정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-21
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <returns></returns>
        public List<BE.TemplateInfo> GetTemplate(int _companyID, int _projectID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Template_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTemplateList(reader);
                    }
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
        /// 메서드명   :
        /// 작성목적   : 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_TemplateID">The _project template ID.</param>
        /// <returns></returns>
        public List<BE.TemplateInfo> GetTemplate(int _companyID, int _projectID, int _TemplateID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Template_SelByTemplateID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _TemplateID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetTemplateList(reader);
                    }
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
        /// 메서드명   :
        /// 작성목적   : 페이징 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(TemplateInfo _be)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Template_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _be.ProjectID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _be.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _be.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _be.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _be.PageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _be.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _be.SearchEndDate.ToNull();

                    cn.Open();
                    return ExecuteDataTableReader(cmd);
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
        /// 메서드명   :
        /// 작성목적   : 페이징 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetPageCount(TemplateInfo _be)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Template_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _be.ProjectID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _be.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _be.SearchValue.ToNull();
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _be.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _be.SearchEndDate.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion GET()
    }
}