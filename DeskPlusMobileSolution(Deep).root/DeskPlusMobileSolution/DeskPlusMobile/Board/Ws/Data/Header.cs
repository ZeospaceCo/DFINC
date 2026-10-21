using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class Header
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-06-26
        /// 작성목적 : 말머리 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public List<BE.HeaderInfo> GetHeaderList(int _companyID, int _categoryID, string _preLang)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLBoard, "SEL_QRY_HEADER");
                sql = string.Format(sql, _companyID, _categoryID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsHeader", null);

                List<BE.HeaderInfo> list = new List<BE.HeaderInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.HeaderInfo info = new BE.HeaderInfo();
                    info.HeaderID = dr["HeaderID"].ToString().ToNumber<int>();
                    info.NameBase = dr["NameBase"].ToString();
                    info.NameKo = dr["NameKo"].ToString();
                    info.NameEn = dr["NameEn"].ToString();
                    info.NameJa = dr["NameJa"].ToString();
                    info.NameFr = dr["NameFr"].ToString();
                    info.NameZhcn = dr["NameZhcn"].ToString();
                    info.NameZhtw = dr["NameZhtw"].ToString();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetHeaderLists: " + errMsg);
                throw;
            }

        }
    }
}