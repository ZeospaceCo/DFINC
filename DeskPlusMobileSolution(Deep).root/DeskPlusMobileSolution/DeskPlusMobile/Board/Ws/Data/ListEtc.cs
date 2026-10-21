using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;


namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class ListEtc
    {
           
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-06-26
        /// 작성목적 : 게시물 작성
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ListEtcInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(SQLHelper.ConnSolution(_info.CompanyID)))
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "INT_PROC_LISTETC");

                    SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@CategoryID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                          
                                                            new SqlParameter("@Etc1", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc2", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc3", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc4", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc5", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc6", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc7", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc8", SqlDbType.NVarChar,300),
                                                            new SqlParameter("@Etc9", SqlDbType.NVarChar,300)                                                            
                };
                    parms[0].Value = _info.CompanyID;
                    parms[1].Value = _info.CategoryID;
                    parms[2].Value = _info.ListID;
                    parms[3].Value = _info.Etc1.ToXSS();
                    parms[4].Value = _info.Etc2.ToXSS();
                    parms[5].Value = _info.Etc3.ToXSS();
                    parms[6].Value = _info.Etc4.ToXSS();
                    parms[7].Value = _info.Etc5.ToXSS();
                    parms[8].Value = _info.Etc6.ToXSS();
                    parms[9].Value = _info.Etc7.ToXSS();
                    parms[10].Value = _info.Etc8;
                    parms[11].Value = _info.Etc9;                 

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                    return Convert.ToInt32(obj);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// ListID 로 ETC정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataSet GetListEtc(int _companyID, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_LISTETC");
                sql = string.Format(sql, _companyID, _listID);
                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSeq", null);

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDraft: " + errMsg);
                throw;
            }
        }

    }
}