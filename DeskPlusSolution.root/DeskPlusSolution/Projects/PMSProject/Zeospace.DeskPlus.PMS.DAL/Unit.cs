using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Unit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the unit.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private UnitInfo GetUnit(IDataReader _reader)
        {
            return new UnitInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProjectID = (_reader["ProjectID"] == DBNull.Value) ? -1 : _reader["ProjectID"].ToString().ToNumber<int>(),
                UnitID = (_reader["UnitID"] == DBNull.Value) ? -1 : _reader["UnitID"].ToString().ToNumber<int>(),
                Code = _reader["Code"].ToString().ToNull(),
                Subjects = _reader["Subjects"].ToString().ToNull(),
                Contents = _reader["Contents"].ToString().ToNull().ToEditorXSS(),
                Process = (_reader["Process"] == DBNull.Value) ? -1 : _reader["Process"].ToString().ToNumber<int>(),
                PlanMoney = (_reader["PlanMoney"] == DBNull.Value) ? -1 : _reader["PlanMoney"].ToString().ToNumber<int>(),
                ExecuteMoney = (_reader["ExecuteMoney"] == DBNull.Value) ? -1 : _reader["ExecuteMoney"].ToString().ToNumber<int>(),
                Status = _reader["Status"].ToString().ToNull().ToNumber<int>(),
                ChargeDeptID = _reader["ChargeDeptID"].ToString().ToNull(),
                RelateDeptID = _reader["RelateDeptID"].ToString().ToNull(),
                PlanStartDate = (_reader["PlanStartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PlanStartDate"],
                PlanEndDate = (_reader["PlanEndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["PlanEndDate"],
                PlanDay = (_reader["PlanDay"] == DBNull.Value) ? -1 : _reader["PlanDay"].ToString().ToNumber<int>(),
                WeightPercent = (_reader["WeightPercent"] == DBNull.Value) ? -1 : _reader["WeightPercent"].ToString().ToNumber<int>(),
                StartDate = (_reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartDate"],
                EndDate = (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"],
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

        /// <summary>
        /// Gets the unit list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<UnitInfo> GetUnitList(IDataReader reader)
        {
            List<UnitInfo> list = new List<UnitInfo>();
            while (reader.Read())
                list.Add(GetUnit(reader));
            return list;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 단위별 정보 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <returns></returns>
        public DataTableReader GetUnit(int _companyID, int _projectID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Unit_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;

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

        #endregion DataReader

        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-09
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(UnitInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Unit_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 100).Value = _info.Code.ToNull().ToXSS();
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 300).Value = _info.Subjects.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 3000).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@Process", SqlDbType.Int).Value = _info.Process;
                    cmd.Parameters.Add("@PlanMoney", SqlDbType.Int).Value = _info.PlanMoney;
                    cmd.Parameters.Add("@ExecuteMoney", SqlDbType.Int).Value = _info.ExecuteMoney;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@ChargeDeptID", SqlDbType.VarChar, 100).Value = _info.ChargeDeptID.ToNull();
                    cmd.Parameters.Add("@RelateDeptID", SqlDbType.VarChar, 100).Value = _info.RelateDeptID.ToNull();
                    cmd.Parameters.Add("@PlanStartDate", SqlDbType.DateTime).Value = _info.PlanStartDate;
                    cmd.Parameters.Add("@PlanEndDate", SqlDbType.DateTime).Value = _info.PlanEndDate;
                    cmd.Parameters.Add("@PlanDay", SqlDbType.Int).Value = _info.PlanDay;
                    cmd.Parameters.Add("@WeightPercent", SqlDbType.Int).Value = _info.WeightPercent;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
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
        public bool Mod(UnitInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Unit_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _info.UnitID;
                    cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 100).Value = _info.Code.ToNull().ToXSS();
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 300).Value = _info.Subjects.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 3000).Value = _info.Contents.ToEditorXSS();
                    cmd.Parameters.Add("@Process", SqlDbType.Int).Value = _info.Process;
                    cmd.Parameters.Add("@PlanMoney", SqlDbType.Int).Value = _info.PlanMoney;
                    cmd.Parameters.Add("@ExecuteMoney", SqlDbType.Int).Value = _info.ExecuteMoney;
                    cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = _info.Status;
                    cmd.Parameters.Add("@ChargeDeptID", SqlDbType.VarChar, 100).Value = _info.ChargeDeptID.ToNull();
                    cmd.Parameters.Add("@RelateDeptID", SqlDbType.VarChar, 100).Value = _info.RelateDeptID.ToNull();
                    cmd.Parameters.Add("@PlanStartDate", SqlDbType.DateTime).Value = _info.PlanStartDate;
                    cmd.Parameters.Add("@PlanEndDate", SqlDbType.DateTime).Value = _info.PlanEndDate;
                    cmd.Parameters.Add("@PlanDay", SqlDbType.Int).Value = _info.PlanDay;
                    cmd.Parameters.Add("@WeightPercent", SqlDbType.Int).Value = _info.WeightPercent;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_list">The _category list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, int _projectID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] _listStr = _list.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _listStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_PMS_Unit_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                            cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _listStr[i];
                            cmd.Parameters.Add("@Orders", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Del(UnitInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Unit_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _info.UnitID;
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

        #endregion DEL()

        #region GET()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_unitID">The _unit ID.</param>
        /// <returns></returns>
        public List<UnitInfo> GetUnit(int _companyID, int _projectID, int _unitID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Unit_SelByUnitID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@UnitID", SqlDbType.Int).Value = _unitID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetUnitList(reader);
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
        /// 작성목적   : 가중치 합산 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <returns></returns>
        public int GetUnitWeightPercent(int _companyID, int _projectID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Unit_SelSumWeightPercent");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;

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
        /// 작성목적   : 페이징 리스트
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-07
        /// </summary>
        /// <param name="_be">The _be.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(UnitInfo _be)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Unit_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _be.ProjectID;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _be.Status;
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
        public int GetPageCount(UnitInfo _be)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_be.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Unit_SelPageCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _be.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _be.ProjectID;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = _be.Status;
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