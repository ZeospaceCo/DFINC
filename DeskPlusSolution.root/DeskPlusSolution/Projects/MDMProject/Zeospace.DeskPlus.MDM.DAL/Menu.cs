using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.MDM.DAL
{
    public class Menu : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-01-17
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetMenu(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_Menu_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMenu");
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
