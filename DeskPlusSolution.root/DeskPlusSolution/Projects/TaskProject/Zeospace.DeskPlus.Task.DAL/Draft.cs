using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlus.Task.DAL
{
    public class Draft : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-08
        /// 작성목적 : 임시 협업 리스트 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_userID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo, string _searchField, string _searchValue,
            int _userID, int _itemID, int _itemSubID, int _isGubun)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_Draft_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;                
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@IsGubun", SqlDbType.Int).Value = _isGubun;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 임시 협업 총 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_userID"></param>
        /// <param name="_itemID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchField, string _searchValue, int _userID, int _itemID, int _itemSubID, int _isGubun)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_TAS_Draft_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchField", SqlDbType.NVarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = _itemID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cmd.Parameters.Add("@IsGubun", SqlDbType.Int).Value = _isGubun;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 리스트 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _draftID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Draft_Sel");
                query = string.Format(query, _draftID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 임시저장하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(DraftInfo _info)
        {
            try
            {
                _info.ContentText = _info.ContentText.ToEditorXSS();
                _info.ContentText = Regex.Replace(_info.ContentText, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Draft_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = _info.ContentText.ToNull();
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

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 수정하기
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(DraftInfo _info)
        {
            try
            {
                _info.ContentText = _info.ContentText.ToEditorXSS();
                _info.ContentText = Regex.Replace(_info.ContentText, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Draft_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _info.DraftID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = _info.ContentText.ToNull();

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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-24
        /// 작성목적 : 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_draftID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Draft_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DraftID", SqlDbType.Int).Value = _draftID;

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


        #endregion 
    }
}
