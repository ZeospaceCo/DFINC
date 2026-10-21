using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Community.DAL
{
    public class ComAdminMenu : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// Gets the community admin menu.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.ComAdminMenuInfo GetCommunityAdminMenu(IDataReader _reader)
        {
            return new BE.ComAdminMenuInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["ParentID"] == DBNull.Value) ? -1 : (int)_reader["ParentID"],
                (_reader["AdminMenuID"] == DBNull.Value) ? -1 : (int)_reader["AdminMenuID"],
                _reader["Name"].ToString(),
                _reader["Url"].ToString() 
            );
        }

        /// <summary>
        /// Gets the community admin menu list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ComAdminMenuInfo> GetCommunityAdminMenuList(IDataReader reader)
        {
            List<ComAdminMenuInfo> list = new List<ComAdminMenuInfo>();
            while (reader.Read())
                list.Add(GetCommunityAdminMenu(reader));
            return list;
        }

        #region GET() 
        #endregion 
 
        #region ADD() 
        #endregion 
 
        #region MOD() 
        #endregion 
 
        #region DEL() 
        #endregion 
    }
}
