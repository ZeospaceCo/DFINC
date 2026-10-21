using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Address.Ws.Data
{
    public class List
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_firstChar"></param>
        /// <param name="_lastChar"></param>
        /// <param name="_gubun"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetTotalCnt(string _companyID, string _userID, string _firstChar, string _lastChar, string _gubun, string _searchValue)
        {
            try
            {
                string searchQuery = String.Empty;
                string sqlQuery = String.Empty;

                //검색어
                if (_searchValue.Trim().Length > 0)
                {
                    _searchValue = _searchValue.Replace("&", "&amp;");
                    _searchValue = _searchValue.Replace("\"", "&quot;");
                    _searchValue = _searchValue.Replace("#", "&#35;");
                    _searchValue = _searchValue.Replace("'", "&#39;");
                    _searchValue = _searchValue.Replace("(", "&#40;");
                    _searchValue = _searchValue.Replace(")", "&#41;");
                    _searchValue = _searchValue.Replace("<", "&lt;");
                    _searchValue = _searchValue.Replace(">", "&gt;");

                    searchQuery = " AND (ManyName LIKE N'%" + _searchValue.Trim() + "%' OR Mail LIKE N'%" + _searchValue.Trim() + "%' OR replace(MobilePhone,'-','')  LIKE N'%" + _searchValue.Trim() + "%' OR DeptManyName LIKE N'%" + _searchValue.Trim() + "%' OR replace(OfficePhone,'-','') LIKE N'%" + _searchValue.Trim() + "%') ";
                }

                switch (_gubun)
                {
                    case "G":
                        sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_LIST_CNT");
                        break;
                    case "C":
                        sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_COMMONUSER_LIST_CNT");
                        break;
                    case "P":
                        sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_PRIVATEUSER_LIST_CNT");
                        break;
                    default:
                        sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAddress, "SEL_QRY_ACCOUNTUSER_LIST_CNT");
                        break;
                }


                sqlQuery = GetSearchQueryCnt(_companyID, _userID, _firstChar, _lastChar, sqlQuery, "NameBase", _gubun, searchQuery);
                return Convert.ToInt32(SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, null));
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetTotalCnt: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_firstChar"></param>
        /// <param name="_lastChar"></param>
        /// <param name="_sqlQuery"></param>
        /// <param name="_name"></param>
        /// <param name="_gubun"></param>
        /// <param name="_searchQuery"></param>
        /// <returns></returns>
        private string GetSearchQueryCnt(string _companyID, string _userID, string _firstChar, string _lastChar, string _sqlQuery, string _name, string _gubun, string _searchQuery)
        {
            string privateQuery = "";

            if (_gubun == "P")
            {
                privateQuery = " AND UserID=" + _userID;
            }

            if (String.IsNullOrEmpty(_firstChar))
            {
                return string.Format(_sqlQuery, _companyID, "", _searchQuery, privateQuery);
            }

            if (_firstChar == "A" || _firstChar == "0")
            {
                _sqlQuery = string.Format(_sqlQuery, _companyID, "and (substring(" + _name + ", 1, 1) >='" + _firstChar + "' and substring(" + _name + ", 1, 1) <= '" + _lastChar + "')", _searchQuery, privateQuery);
            }
            else if (_firstChar == "Z")
            {
                _sqlQuery = string.Format(_sqlQuery, _companyID, "and (substring(" + _name + ", 1, 1) >='" + _firstChar + "')", _searchQuery, privateQuery);
            }
            else
            {
                _sqlQuery = string.Format(_sqlQuery, _companyID, "and (" + _name + " >='" + _firstChar + "' and " + _name + " < '" + _lastChar + "')", _searchQuery, privateQuery);
            }

            return _sqlQuery;
        }

    }
}

