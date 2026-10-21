using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Task.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.DAL
{
    public class Template : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private TemplateInfo GetTemplate(IDataReader _reader)
        {
            return new TemplateInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ItemSubID = (_reader["ItemSubID"] == DBNull.Value) ? -1 : _reader["ItemSubID"].ToString().ToNumber<int>(),
                TemplateID = (_reader["TemplateID"] == DBNull.Value) ? -1 : _reader["TemplateID"].ToString().ToNumber<int>(),                
                Contents = _reader["Contents"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyDate = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]

            };
        }

        protected List<TemplateInfo> GetTemplateList(IDataReader reader)
        {
            List<TemplateInfo> list = new List<TemplateInfo>();
            while (reader.Read())
                list.Add(GetTemplate(reader));
            return list;
        }

        #endregion

        #region GET

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-10
        /// 작성목적 : 템플릿 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public List<BE.TemplateInfo> GetTemplateList(int _companyID, int _itemSubID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.TASK, "QRY_TAS_Template_Sel");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;
                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetTemplateList(reader);
                }
            }
        }

        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자: 2014-04-27
        /// 작성목적: 템플릿 추가
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool AddTemplate(BE.TemplateInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Template_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _listinfo.ItemSubID;                    
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _listinfo.Contents.ToEditorXSS();
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _listinfo.WriteUserID;

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

        #region DEL
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자: 2014-04-28
        /// 작성목적: 템플릿 삭제
        /// <param name="_companyID"></param>
        /// <param name="_itemSubID"></param>
        /// <returns></returns>
        public bool DelTemplate(int _companyID,  int _itemSubID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_TAS_Template_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ItemSubID", SqlDbType.Int).Value = _itemSubID;                                        

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
