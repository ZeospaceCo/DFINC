using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Statistics.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Statistics.Web.Ws.Content.PerformMon
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSStatistics
    {

        #region Get
        /// <summary>
        /// CPU/Memory 정보 가져오기
        /// </summary>
        /// <param name="_machineName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPerform(string _machineName, int _count)
        {
            List<object> chartData = new List<object>();

            List<string> labelData = new List<string>();
            List<string> cpuData = new List<string>();
            List<string> memData = new List<string>();

            //CPU

            DAL.PerformMon dal = new DAL.PerformMon();

            DataSet ds = dal.GetCPU(_machineName);

            DataTable dt;
            if (_count > 0)
            {
                dt = ds.Tables[0].AsEnumerable().Take(_count).CopyToDataTable();
            }
            else
            {
                dt = ds.Tables[0];
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "PerformMonValueID ASC";

            //DataTable dt = dv.ToTable();

            foreach (DataRow dr in dv.ToTable().Rows)
            {
                labelData.Add(Convert.ToDateTime(dr["WriteDate"]).ToString("HH:mm"));
                cpuData.Add(Convert.ToDouble(dr["Value"]).ToString("f2"));
            }

            //Memory
            ds.Clear();
            ds = dal.GetMemory(_machineName);
            dv = ds.Tables[0].DefaultView;
            dv.Sort = "PerformMonValueID ASC";
            dt = dv.ToTable();

            bool IsLabelData = true;
            if (labelData.Count == 0) IsLabelData = false;

            foreach (DataRow dr in dt.Rows)
            {
                if (!IsLabelData) labelData.Add(Convert.ToDateTime(dr["WriteDate"]).ToString("HH:mm"));
                memData.Add(Convert.ToDouble(dr["Value"]).ToString("f2"));
            }

            chartData.Add(labelData);
            chartData.Add(cpuData);
            chartData.Add(memData);

            return chartData;
        }

        /// <summary>
        /// Disk 여유 공간 % 정보 가져오기
        /// </summary>
        /// <param name="_machineName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDisk(string _machineName)
        {
            List<object> chartData = new List<object>();

            List<string> labelData = new List<string>();
            List<string> diskData = new List<string>();

            //Disk
            DAL.PerformMon dal = new DAL.PerformMon();

            DataSet ds = dal.GetDisk(_machineName);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                labelData.Add(dr["InstanceName"].ToString());
                diskData.Add(Convert.ToDouble(dr["Value"]).ToString("f2"));
            }

            chartData.Add(labelData);
            chartData.Add(diskData);

            return chartData;
        }

        /// <summary>
        /// Disk 여유 공간 MBytes 정보 가져오기
        /// </summary>
        /// <param name="_machineName"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDiskMByte(string _machineName)
        {
            List<object> chartData = new List<object>();

            List<string> labelData = new List<string>();
            List<string> diskData = new List<string>();
            List<string> diskDataFormat = new List<string>();

            //Disk
            DAL.PerformMon dal = new DAL.PerformMon();

            DataSet ds = dal.GetDiskMByte(_machineName);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                labelData.Add(dr["InstanceName"].ToString());
                diskData.Add(Convert.ToDouble(dr["Value"]).ToString("f2"));

                diskDataFormat.Add("&nbsp;" + dr["InstanceName"].ToString() + " " + FormatBytes(Convert.ToDouble(dr["Value"])) + "&nbsp;");
            }

            chartData.Add(labelData);
            chartData.Add(diskData);
            chartData.Add(diskDataFormat);

            return chartData;
        }

        //Byte 변환
        enum Unit { MB, GB, ER }
        public string FormatBytes(double bytes)
        {
            int unit = 0;
            while (bytes > 1024)
            {
                bytes /= 1024;
                ++unit;
            }

            string s = bytes.ToString("F") + " ";
            return s + ((Unit)unit).ToString(); 
        }
        #endregion
    }

}
