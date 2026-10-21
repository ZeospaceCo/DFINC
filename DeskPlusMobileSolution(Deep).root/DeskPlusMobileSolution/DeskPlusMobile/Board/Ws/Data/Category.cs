using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class Category
    {
        public DataSet GetCategoryCompany(int _companyID)
        {
            SqlParameter[] Parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int)
            };

            Parms[0].Value = _companyID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Category_SelCompany", "GetCategory", Parms);
        }

        /// <summary>
        /// Gets the category."
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetCategory(int _companyID)
        {
            SqlParameter[] Parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int)
            };

            Parms[0].Value = _companyID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Category_Sel", "GetCategory", Parms);
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public DataSet GetCategory(int _companyID, int _categoryID)
        {
            SqlParameter[] Parms = new SqlParameter[] {
												new SqlParameter("@CompanyID", SqlDbType.Int),
												new SqlParameter("@CategoryID", SqlDbType.Int)};

            Parms[0].Value = _companyID;
            if (_categoryID > 0)
            {
                Parms[1].Value = _categoryID;
            }
            else
            {
                Parms[1].Value = System.DBNull.Value;
            }

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Category_SelByCID", "ByCategoryID", Parms);
        }

        /// <summary>
        /// Gets the category by parent category ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <returns></returns>
        public DataSet GetCategoryByParentCategoryID(int _companyID, int _parentCategoryID)
        {
            SqlParameter[] Parms = new SqlParameter[] {
												new SqlParameter("@CompanyID", SqlDbType.Int),
												new SqlParameter("@ParentCategoryID", SqlDbType.Int)};
            Parms[0].Value = _companyID;
            Parms[1].Value = _parentCategoryID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Category_SelByPCID", "GetCategoryByParentCategoryID", Parms);
        }

    }
}