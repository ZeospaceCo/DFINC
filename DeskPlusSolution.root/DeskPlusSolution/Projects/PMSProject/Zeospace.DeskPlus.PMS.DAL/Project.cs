using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Project : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ProjectInfo GetProject(IDataReader _reader)
        {
            return new ProjectInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProjectID = (_reader["ProjectID"] == DBNull.Value) ? -1 : _reader["ProjectID"].ToString().ToNumber<int>(),
                FolderID = (_reader["FolderID"] == DBNull.Value) ? -1 : _reader["FolderID"].ToString().ToNumber<int>(),
                ChargeDeptID = _reader["ChargeDeptID"].ToString().ToNull(),
                SubjectBase = _reader["SubjectBase"].ToString().ToNull(),
                SubjectKo = _reader["SubjectKo"].ToString().ToNull(),
                SubjectEn = _reader["SubjectEn"].ToString().ToNull(),
                SubjectJa = _reader["SubjectJa"].ToString().ToNull(),
                SubjectFr = _reader["SubjectFr"].ToString().ToNull(),
                SubjectZhcn = _reader["SubjectZhcn"].ToString().ToNull(),
                SubjectZhtw = _reader["SubjectZhtw"].ToString().ToNull(),
                Contents = _reader["Contents"].ToString().ToNull().ToEditorXSS(),
                IsOpen = _reader["IsOpen"].ToString().ToNumber<int>(),
                PlanStartDate = (_reader["PlanStartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PlanStartDate"],
                PlanEndDate = (_reader["PlanEndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PlanEndDate"],
                StartDate = (_reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartDate"],
                EndDate = (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"],
                Process = (_reader["Process"] == DBNull.Value) ? -1 : _reader["Process"].ToString().ToNumber<int>(),
                Status = _reader["Status"].ToString().ToNumber<int>(),
                CompleteDate = (_reader["CompleteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["CompleteDate"],
                IsApproval = _reader["IsApproval"].ToString().ToNumber<int>(),
                Opinion = _reader["Opinion"].ToString().ToNull(),
                IsClosed = _reader["IsClosed"].ToString().ToNull(),
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

                FolderBase = _reader["FolderBase"].ToString().ToNull(),
                FolderKo = _reader["FolderKo"].ToString().ToNull(),
                FolderEn = _reader["FolderEn"].ToString().ToNull(),
                FolderJa = _reader["FolderJa"].ToString().ToNull(),
                FolderFr = _reader["FolderFr"].ToString().ToNull(),
                FolderZhcn = _reader["FolderZhcn"].ToString().ToNull(),
                FolderZhtw = _reader["FolderZhtw"].ToString().ToNull(),

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

        protected List<ProjectInfo> GetProjectList(IDataReader reader)
        {
            List<ProjectInfo> list = new List<ProjectInfo>();
            while (reader.Read())
                list.Add(GetProject(reader));
            return list;
        }

        #endregion DataReader

        #region ADD

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-09
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@ChargeDeptID", SqlDbType.VarChar, 100).Value = _info.ChargeDeptID.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectBase", SqlDbType.NVarChar, 100).Value = _info.SubjectBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectKo", SqlDbType.NVarChar, 20).Value = _info.SubjectKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectEn", SqlDbType.NVarChar, 100).Value = _info.SubjectEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectJa", SqlDbType.NVarChar, 100).Value = _info.SubjectJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectFr", SqlDbType.NVarChar, 100).Value = _info.SubjectFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectZhcn", SqlDbType.NVarChar, 100).Value = _info.SubjectZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectZhtw", SqlDbType.NVarChar, 100).Value = _info.SubjectZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 3000).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@IsOpen", SqlDbType.TinyInt).Value = _info.IsOpen;

                    cmd.Parameters.Add("@PlanStartDate", SqlDbType.DateTime).Value = _info.PlanStartDate.ToNullDT();
                    cmd.Parameters.Add("@PlanEndDate", SqlDbType.DateTime).Value = _info.PlanEndDate.ToNullDT();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate.ToNullDT();
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate.ToNullDT();
                    cmd.Parameters.Add("@CompleteDate", SqlDbType.DateTime).Value = _info.CompleteDate.ToNullDT();

                    cmd.Parameters.Add("@Process", SqlDbType.Int).Value = _info.Process;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@IsApproval", SqlDbType.TinyInt).Value = _info.IsApproval;
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 300).Value = _info.Opinion.ToNull();
                    cmd.Parameters.Add("@IsClosed", SqlDbType.Char, 1).Value = _info.IsClosed.ToNull();
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

        #endregion ADD

        #region MOD

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : list 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-02
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Mod(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@ChargeDeptID", SqlDbType.VarChar, 100).Value = _info.ChargeDeptID.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectBase", SqlDbType.NVarChar, 100).Value = _info.SubjectBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectKo", SqlDbType.NVarChar, 20).Value = _info.SubjectKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectEn", SqlDbType.NVarChar, 100).Value = _info.SubjectEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectJa", SqlDbType.NVarChar, 100).Value = _info.SubjectJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectFr", SqlDbType.NVarChar, 100).Value = _info.SubjectFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectZhcn", SqlDbType.NVarChar, 100).Value = _info.SubjectZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@SubjectZhtw", SqlDbType.NVarChar, 100).Value = _info.SubjectZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 3000).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@IsOpen", SqlDbType.TinyInt).Value = _info.IsOpen;

                    cmd.Parameters.Add("@PlanStartDate", SqlDbType.DateTime).Value = _info.PlanStartDate.ToNullDT();
                    cmd.Parameters.Add("@PlanEndDate", SqlDbType.DateTime).Value = _info.PlanEndDate.ToNullDT();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate.ToNullDT();
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate.ToNullDT();

                    cmd.Parameters.Add("@Process", SqlDbType.Int).Value = _info.Process;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;

                    cmd.Parameters.Add("@CompleteDate", SqlDbType.DateTime).Value = _info.CompleteDate.ToNullDT();

                    cmd.Parameters.Add("@IsApproval", SqlDbType.TinyInt).Value = _info.IsApproval;
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 300).Value = _info.Opinion.ToNull().ToXSS();
                    cmd.Parameters.Add("@IsClosed", SqlDbType.Char, 1).Value = _info.IsClosed.ToNull();
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

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 상태값 변경하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-29
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_status">The _status.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _projectID, int _status)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_UpdByStatus", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _status;

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
        /// 메서드명   :
        /// 작성목적   : 폐쇄 여부신청
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-29
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_isClosed">The _is closed.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _projectID, string _isClosed, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_UpdByIsClosed", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@IsClosed", SqlDbType.Char, 1).Value = _isClosed;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;

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
        /// 메서드명   :
        /// 작성목적   : 총평 수정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-03
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool Mod(int _companyID, int _projectID, int _userID, string _content)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_UpdByOpinion", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Opinion", SqlDbType.NVarChar, 300).Value = _content.ToXSS();

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

        #endregion MOD

        #region DEL

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

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

        #endregion DEL

        #region GET

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 프로젝트 정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-18
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <returns></returns>
        public List<ProjectInfo> GetProject(int _companyID, int _projectID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Project_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetProjectList(reader);
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
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(ProjectInfo _info, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelUserList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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
        /// 작성목적   : 페이징 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public DataTableReader GetPageList(ProjectInfo _info, bool _isAdmin)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = (_isAdmin) ? "1" : "0";

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
        /// 작성목적   : 폐쇄 페이징 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public DataTableReader GetClosePageList(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelCloseList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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
        /// 작성목적   : 관련 페이징 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public DataTableReader GetLinkPageList(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelLinkList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _info.PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _info.PageSizePageNo;
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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
        public int GetPageCount(ProjectInfo _info, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelUserPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이징 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_isAdmin">if set to <c>true</c> [_is admin].</param>
        /// <returns></returns>
        public int GetPageCount(ProjectInfo _info, bool _isAdmin)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _info.FolderID;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Char, 1).Value = (_isAdmin) ? "1" : "0";

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

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 폐쇄 페이징 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetClosePageCount(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelClosePageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 관련 페이징 카운트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-16
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public int GetLinkPageCount(ProjectInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Project_SelLinkPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 500).Value = _info.SearchField.ToNull();
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 1000).Value = _info.SearchValue.ToNull();
                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _info.SearchStartDate.ToNull();
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _info.SearchEndDate.ToNull();

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

        /// <summary>
        /// Gets the project count by folder ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_folderID">The _folder ID.</param>
        /// <returns></returns>
        public int GetProjectCountByFolderID(int _companyID, int _folderID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Project_SelByFolderID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FolderID", SqlDbType.Int).Value = _folderID;

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

        #endregion GET
    }
}