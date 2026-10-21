using System;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class SSO : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private SSOInfo GetSSO(IDataReader _reader)
        {
            return new SSOInfo(
                (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
                  (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                  _reader["DisplayName"].ToString(),
                  _reader["EmployeeNumber"].ToString(),
                  _reader["DepartmentNumber"].ToString(),
                  _reader["Mail"].ToString(),
                  _reader["UserPassword"].ToString(),
                  _reader["SN"].ToString(),
                  _reader["_GivenName"].ToString(),
                  _reader["Initials"].ToString(),
                  _reader["Title"].ToString(),
                  _reader["TelephoneNumber"].ToString(),
                  _reader["FacsimileTelephoneNumber"].ToString(),
                  _reader["Mobile"].ToString(),
                  _reader["RoomNumber"].ToString(),
                  _reader["CarLicense"].ToString(),
                  _reader["O"].ToString(),
                  _reader["OU"].ToString(),
                  _reader["EmployeeType"].ToString(),
                  _reader["PreferredLanguage"].ToString(),
                  _reader["LabeledURI"].ToString(),
                  _reader["UserCode"].ToString(),
                  _reader["Description"].ToString()
                );
        }

        protected List<SSOInfo> GetSSOList(IDataReader reader)
        {
            List<SSOInfo> list = new List<SSOInfo>();

            while (reader.Read())
                list.Add(GetSSO(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        #endregion 추가/수정/삭제

        #region 가져오기

        #endregion 가져오기
    }
}