using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public  class GoogleCalendar : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        public GoogleCalendar()
        {
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2015-06-12
        /// 작성목적 : bulk copy
        /// </summary>
        /// <param name="listobj"></param>
        /// <returns></returns>
        public bool GoogleCalendarBulk(List<BE.GoogleCalendarInfo> listobj)
        {
            try
            {
                 
                string[] arcul = { "CompanyID", "UserID", "Title", "Location", "starttime", "endtime", "Content", "Uid", "DeptID" };
                string[] arval = new string[arcul.Length];
                DataTable dt = AddStringCulumns(arcul);
                int companyid = 0;
                int userid = 0;
                
                foreach (BE.GoogleCalendarInfo _be in listobj)
                {
                    
                    companyid = _be.CompanyID;
                    userid = _be.UserID;
                    arval[0] = _be.CompanyID.ToString();
                    arval[1] = _be.UserID.ToString();

                    if (_be.Title == null)
                        arval[2] = "";
                    else
                        arval[2] = _be.Title.ToString().ToNull();
                    
                    if (_be.Location == null)
                        arval[3] = "";
                    else
                        arval[3] = _be.Location.ToString();     
               
                    arval[4] = _be.StartTime.ToString("yyyy-MM-dd HH:mm");
                    arval[5] = _be.EndTime.ToString("yyyy-MM-dd HH:mm");
                    if (_be.Content == null)
                        arval[6] = "";
                    else
                        arval[6] = _be.Content.ToString().ToNull();

                    arval[7] = _be.Uid.ToString();
                    arval[8] = _be.DeptID.ToString();
                    AddDataRow(ref dt, arcul, arval);
                }

                DAL.GoogleCalendar tempdal = new DAL.GoogleCalendar();
                tempdal.DelTempGoogle(companyid, userid);
                tempdal.BulkTempGoogle(dt, companyid);
                DAL.GoogleCalendar dalgoogle = new DAL.GoogleCalendar();
                dalgoogle.SyncGoogle(companyid);

                return true;
                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GoogleCalendarBulk", ex);
                return false;
            }
        }

        #region table
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
        public static DataTable MakeTable(string[] arCul, string[,] arVal)
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
        #endregion

    }
}
