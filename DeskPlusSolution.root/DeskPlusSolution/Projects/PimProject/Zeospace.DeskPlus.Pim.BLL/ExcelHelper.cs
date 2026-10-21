using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {
        }

        /// <summary>
        /// 테이블 만들기 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static DataTable AddStringCulumns(params string[] keys)
        {
            DataTable dt = new DataTable();
            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    dt.Columns.Add(keys[i], typeof(string));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return dt;
        }
        public static void AddDataRow(ref DataTable dt, string[] keys, params string[] sValues)
        {
            try
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < keys.Length; i++)
                {
                    dr[keys[i]] = sValues[i];
                }
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static  DataTable MakeTable(string[] arCul, string[,] arVal)
        {
            DataTable dt = AddStringCulumns(arCul);
            string[] arvaltemp = new string[2];
            for (int i = 0; i < arVal.Length / 2; i++)
            {
                arvaltemp[0] = arVal[i, 0];
                arvaltemp[1] = arVal[i, 1];
                AddDataRow(ref dt, arCul, arvaltemp);
            }
            return dt;
        }

    }
}
