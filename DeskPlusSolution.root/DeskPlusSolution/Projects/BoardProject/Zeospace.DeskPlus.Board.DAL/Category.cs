using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class Category : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private CategoryInfo GetCategory(IDataReader _reader)
        {
            return new CategoryInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ParentCategoryID = (_reader["ParentCategoryID"] == DBNull.Value) ? -1 : _reader["ParentCategoryID"].ToString().ToNumber<int>(),
                CategoryOrder = (_reader["CategoryOrder"] == DBNull.Value) ? -1 : _reader["CategoryOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
                Explan = _reader["Explan"].ToString().ToNull(),
                OptionShowImage = _reader["OptionShowImage"].ToString().ToNull(),
                OptionUseReply = _reader["OptionUseReply"].ToString().ToNull(),
                OptionUseShortReply = _reader["OptionUseShortReply"].ToString().ToNull(),
                OptionShowViewer = _reader["OptionShowViewer"].ToString().ToNull(),
                OptionUseSecure = _reader["OptionUseSecure"].ToString().ToNull(),
                OptionUseDept = _reader["OptionUseDept"].ToString().ToNull(),
                OptionCustomView = _reader["OptionCustomView"].ToString().ToNull(),
                OptionVote = _reader["OptionVote"].ToString().ToNull(),
                OptionVoteView = _reader["OptionVoteView"].ToString().ToNull(),
                OptionUseView = _reader["OptionUseView"].ToString().ToNull(),
                OptionAnonymity = _reader["OptionAnonymity"].ToString().ToNull(),
                OptionAlarm = (_reader["OptionAlarm"] == DBNull.Value) ? -1 : _reader["OptionAlarm"].ToString().ToNumber<int>(),
                OptionListType = _reader["OptionListType"].ToString().ToNull(),
                OptionMobile = _reader["OptionMobile"].ToString().ToNull(),
                OptionPopUp = _reader["OptionPopUp"].ToString().ToNull(),
                TemplateID = (_reader["TemplateID"] == DBNull.Value) ? -1 : _reader["TemplateID"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            };
        }

        protected List<CategoryInfo> GetCategoryList(IDataReader reader)
        {
            List<CategoryInfo> list = new List<CategoryInfo>();
            while (reader.Read())
                list.Add(GetCategory(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 게시판 카테고리 수정
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public int Add(CategoryInfo _info)
        {
            try
            {
                int categoryID = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Category_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentCategoryID", SqlDbType.Int).Value = _info.ParentCategoryID;
                    cmd.Parameters.Add("@CategoryOrder", SqlDbType.Int).Value = _info.CategoryOrder;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@Explan", SqlDbType.NVarChar, 1000).Value = _info.Explan.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShowImage", SqlDbType.Char, 1).Value = _info.OptionShowImage.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseReply", SqlDbType.Char, 1).Value = _info.OptionUseReply.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseShortReply", SqlDbType.Char, 1).Value = _info.OptionUseShortReply.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShortReplyGood", SqlDbType.Char, 1).Value = _info.OptionShortReplyGood.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShortReplyTop", SqlDbType.Char, 1).Value = _info.OptionShortReplyTop.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShowViewer", SqlDbType.Char, 1).Value = _info.OptionShowViewer.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseSecure", SqlDbType.Char, 1).Value = _info.OptionUseSecure.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseDept", SqlDbType.Char, 1).Value = _info.OptionUseDept.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionCustomView", SqlDbType.NVarChar, 100).Value = _info.OptionCustomView.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionVote", SqlDbType.Char, 1).Value = _info.OptionVote.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionVoteView", SqlDbType.Char, 1).Value = _info.OptionVoteView.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseView", SqlDbType.Char, 1).Value = _info.OptionUseView.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionAnonymity", SqlDbType.Char, 1).Value = _info.OptionAnonymity.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionAlarm", SqlDbType.Int).Value = _info.OptionAlarm;
                    cmd.Parameters.Add("@OptionListType", SqlDbType.Char, 1).Value = _info.OptionListType.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionMobile", SqlDbType.Char, 1).Value = _info.OptionMobile.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionPopUp", SqlDbType.Char, 1).Value = _info.OptionPopUp.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionLinkURL", SqlDbType.Char, 1).Value = _info.OptionLinkURL.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionDownLoad", SqlDbType.Char, 1).Value = _info.OptionDownLoad.ToNull().ToXSS();
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _info.TemplateID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    categoryID = ExecuteScalar(cmd).ToString().ToNumber<int>();

                    if (categoryID < 1)
                    {
                        return -1;
                    }
                }

                return categoryID;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 심재운
        /// 게시판 카테고리 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(CategoryInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Category_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _info.CategoryID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 50).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@Explan", SqlDbType.NVarChar, 1000).Value = _info.Explan.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShowImage", SqlDbType.NVarChar, 100).Value = _info.OptionShowImage.ToXSS();
                    cmd.Parameters.Add("@OptionUseReply", SqlDbType.NText).Value = _info.OptionUseReply.ToXSS();
                    cmd.Parameters.Add("@OptionUseShortReply", SqlDbType.NText).Value = _info.OptionUseShortReply.ToXSS();
                    cmd.Parameters.Add("@OptionShortReplyGood", SqlDbType.Char, 1).Value = _info.OptionShortReplyGood.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShortReplyTop", SqlDbType.Char, 1).Value = _info.OptionShortReplyTop.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionShowViewer", SqlDbType.Char, 1).Value = _info.OptionShowViewer.ToNull();
                    cmd.Parameters.Add("@OptionUseSecure", SqlDbType.Char, 1).Value = _info.OptionUseSecure.ToNull();
                    cmd.Parameters.Add("@OptionUseDept", SqlDbType.Char, 1).Value = _info.OptionUseDept.ToNull();
                    cmd.Parameters.Add("@OptionCustomView", SqlDbType.NVarChar, 100).Value = _info.OptionCustomView.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionVote", SqlDbType.Char, 1).Value = _info.OptionVote.ToNull();
                    cmd.Parameters.Add("@OptionVoteView", SqlDbType.Char, 1).Value = _info.OptionVoteView.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionUseView", SqlDbType.Char, 1).Value = _info.OptionUseView.ToNull();
                    cmd.Parameters.Add("@OptionAnonymity", SqlDbType.Char, 1).Value = _info.OptionAnonymity.ToNull();
                    cmd.Parameters.Add("@OptionAlarm", SqlDbType.Int).Value = _info.OptionAlarm;
                    cmd.Parameters.Add("@OptionListType", SqlDbType.Char, 1).Value = _info.OptionListType.ToNull();
                    cmd.Parameters.Add("@OptionMobile", SqlDbType.Char, 1).Value = _info.OptionMobile.ToNull();
                    cmd.Parameters.Add("@OptionPopUp", SqlDbType.Char, 1).Value = _info.OptionPopUp.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionLinkURL", SqlDbType.Char, 1).Value = _info.OptionLinkURL.ToNull().ToXSS();
                    cmd.Parameters.Add("@OptionDownLoad", SqlDbType.Char, 1).Value = _info.OptionDownLoad.ToNull().ToXSS();
                    cmd.Parameters.Add("@TemplateID", SqlDbType.Int).Value = _info.TemplateID;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;

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
        /// 작성자 : 심재운
        /// 카테고리 삭제
        /// 상세 : 첨부파일, etc, viewer, list, 카테고리 정보를 전부 삭제한다.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _categoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_Category_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

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
        /// 작성자 : 심재운
        /// 용도 : 폴더 카테고리 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <returns></returns>
        public bool ModOrderCategory(int _companyID, string _categoryList)
        {
            try
            {
                if (_categoryList.Length > 0)
                {
                    string[] _categoryListStr = _categoryList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _categoryListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_BOA_Category_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryListStr[i];
                            cmd.Parameters.Add("@CategoryOrder", SqlDbType.Int).Value = (i + 1);

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
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 카테고리 이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_toCategoryID">The _to category ID.</param>
        /// <returns></returns>
        public bool ModMoveCategory(int _companyID, int _categoryID, int _toCategoryID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_BOA_Category_UpdMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                    cmd.Parameters.Add("@ToCategoryID", SqlDbType.Int).Value = _toCategoryID;

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리 정보 가져오기
        /// </summary>
        /// <returns></returns>
        public DataSet GetCategory()
        {
            try
            {
                //SELECT CompanyID, CategoryID, ParentCategoryID  FROM DP_BOA_Category
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Category_Sel");

                using (var cn = new SqlConnection(ConnSolution()))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetCategory");
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
        /// 카테고리  정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public DataSet GetCategory(int _companyID, int _categoryID)
        {
            try
            {
                string query = "";

                if (_categoryID == 0)
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Category_SelByPID");
                    query = string.Format(query, "is null");

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetCategory");
                    }
                }
                else
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Category_SelByID");

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetCategory");
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
        /// 작성자 : 심재운
        /// 하위카테고리로 정보 가져오기 (트리용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <returns></returns>
        public DataSet GetCategoryByParentCategoryID(int _companyID, string _parentCategoryID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Category_SelByPID");

                if (_parentCategoryID == "0")
                {
                    query = string.Format(query, "is null");
                }
                else
                {
                    query = string.Format(query, "=" + _parentCategoryID.ToInject());
                }

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetCategory");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #region 김광일 코드 (차후 분석)

        public string GetCategoryPath(int _companyID, int _DirectCategoryID)
        {
            string _strRt = string.Empty;
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Path_SelByID");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _DirectCategoryID;

                    cn.Open();

                    DataSet ds = ExecuteDataSet(cmd, "tbl");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _strRt = ds.Tables[0].Rows[0]["PathID"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return _strRt;
            }

            return _strRt;
        }

        /// <summary>
        /// 작성자 : 김광일
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pathid"></param>
        /// <returns></returns>
        public DataSet GetCategoryPathList(int _companyID, string _pathid)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_Category_Path");
                    query = string.Format(query, _pathid);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    //cmd.Parameters.Add("@PathId ", SqlDbType.VarChar,100).Value = _pathid;

                    cn.Open();

                    return ExecuteDataSet(cmd, "tbl");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }

        }

        #endregion 김광일 코드 (차후 분석)

        #endregion 가져오기
    }
}