using System;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Statistics.BE;
using System.Data.SqlClient;
using System.Data;

namespace Zeospace.DeskPlus.Statistics.DAL
{
    public class PerformMon : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        #endregion

        #region GET
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2017-01-04
        /// 작성목적 : 서버명 가져오기 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetMachineName()
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PerformMonInfo_SelByMachineGroup");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMachineName");
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
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2017-01-05
        /// 작성목적 : CPU 성능 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetCPU(string _machineName)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PerformMon_SelByCPU");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MachineName",SqlDbType.NVarChar,100).Value = _machineName;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetCPU");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2017-01-05
        /// 작성목적 : Memory 성능 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetMemory(string _machineName)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PerformMon_SelByMem");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MachineName", SqlDbType.NVarChar, 100).Value = _machineName;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMem");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2017-01-05
        /// 작성목적 : Disk 여유 공간 % 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetDisk(string _machineName)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PerformMon_SelByDisk");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MachineName", SqlDbType.NVarChar, 100).Value = _machineName;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetDisk");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }


        /// <summary>
        /// <summary>
        /// 작성자   : freeman
        /// 작성일자 : 2017-01-05
        /// 작성목적 : Disk 여유 공간 Bytes 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataSet GetDiskMByte(string _machineName)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution()))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_PerformMon_SelByDiskMByte");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MachineName", SqlDbType.NVarChar, 100).Value = _machineName;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetDiskMBytes");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        #endregion

        #region 엑셀 다운 관련 
        #endregion
    }
}
