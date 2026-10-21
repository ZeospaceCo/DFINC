using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class Enviroment : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public Enviroment()
        {
        }

        #region Get

        public DataTableReader GetEnviroMent(BE.EnviromentInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Enviroment_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@Kind", SqlDbType.Int).Value = _info.Kind;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        

        #endregion

        #region Add
        public void AddEnviroMent(BE.EnviromentInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Enviroment_Ins", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@Kind", SqlDbType.Int).Value = _info.Kind;
                cmd.Parameters.Add("@ETC1", SqlDbType.VarChar,100).Value = _info.Etc1;
                cmd.Parameters.Add("@ETC2", SqlDbType.VarChar, 100).Value = _info.Etc2;
                cn.Open();
                ExecuteNonQuery(cmd);
                
            }
        }
        #endregion

        #region MOD
        public void ModEnviroMent(BE.EnviromentInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Enviroment_Mod", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@Kind", SqlDbType.Int).Value = _info.Kind;
                cmd.Parameters.Add("@ETC1", SqlDbType.Int).Value = _info.Etc1;
                cmd.Parameters.Add("@ETC2", SqlDbType.Int).Value = _info.Etc2;
                cn.Open();
                ExecuteNonQuery(cmd);

            }
        }

        

        #endregion


        #region Del
        public void DelEnviroMent(BE.EnviromentInfo _info)
        {
            using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Enviroment_Del", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                cmd.Parameters.Add("@Kind", SqlDbType.Int).Value = _info.Kind;
                
                cn.Open();
                ExecuteNonQuery(cmd);

            }
        }

        #endregion


    }
}
