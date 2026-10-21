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
    public class Point : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET() 
        #endregion 
 
        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 사용자 포인트 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="menuID">The menu ID.</param>
        /// <returns></returns>
        public bool Add(PointInfo _info, int _menuID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_CMN_Point_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@CommunityID", SqlDbType.Int).Value = _info.CommunityID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = _info.Type;
                    cmd.Parameters.Add("@Point", SqlDbType.Int).Value = _info.Point;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;

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
 
        #region MOD() 
        #endregion 
 
        #region DEL() 
        #endregion 
    }
}
