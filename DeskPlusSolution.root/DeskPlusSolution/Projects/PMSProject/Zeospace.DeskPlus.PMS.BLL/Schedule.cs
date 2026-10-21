using System;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class Schedule : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()

        /// <summary>
        /// Gets the schedule by parent schedule ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_parentScheduleID">The _parent schedule ID.</param>
        /// <returns></returns>
        public string GetScheduleByParentScheduleID(int _companyID, int _projectID, int _parentScheduleID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Zeospace.DeskPlus.PMS.DAL.Schedule dal = new DAL.Schedule();
                DataTableReader ds = dal.GetScheduleByParentScheduleID(_companyID, _projectID, _parentScheduleID);
                if (ds.HasRows)
                {
                    while (ds.NextResult())
                    {
                        sb.Append(ds["ScheduleID"].ToString());
                        sb.Append(",");
                    }
                }
                return sb.ToString().TrimEnd(',');
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.PMS.BLL.Schedule[GetScheduleByParentScheduleID]", ex);
                return null;
            }
        }

        #endregion GET()
    }
}