using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.ETC.DAL
{
    public class PayFolderStatus : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader        

        #endregion

        #region GET()

        /// <summary>
        /// 작성자 : 김형석
        /// 회계문서 폴더 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_activeCabinet">if set to <c>true</c> [_active cabinet].</param>
        /// <returns></returns>
        public DataSet GetFolderTree(int _companyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ETC, "QRY_PAY_FolderStatus_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetFolderTree");
            }
        }

        #endregion

        #region ADD()
        #endregion

        #region MOD()
        #endregion

        #region DEL()
        #endregion 
    }
}
