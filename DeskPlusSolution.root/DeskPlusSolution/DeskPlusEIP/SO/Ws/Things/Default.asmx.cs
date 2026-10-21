using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using System.Collections;

namespace Zeospace.DeskPlus.SO.Web.Ws.Things
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Default : System.Web.Services.WebService
    {

        #region Add
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        ///         //public int AddMul(BE.MultipurposeInfo _jsonData)
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = true)]
        public int AddMul(int _cID, int _tID, string _act, string _temp, string _bat, string _con, string _sta)
        {
            #region 다국어 리소스 로드/Title 정보

            #endregion

            try
            {
                Zeospace.DeskPlus.SO.BE.MultipurposeInfo mInfo = new BE.MultipurposeInfo();

                mInfo.CompanyID = _cID;
                mInfo.ThingsID = _tID;
                mInfo.ActionType = _act;
                mInfo.Temperature = _temp;
                mInfo.Battery = _bat;
                mInfo.Contact = _con;
                mInfo.Status = _sta;
                //mInfo.ActionDate = Convert.ToDateTime(_actd);



                Zeospace.DeskPlus.SO.DAL.Multipurpose mDal = new DAL.Multipurpose();
                int rtn = mDal.Add(mInfo);

                return rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Things.Add Multipurpose : " + ex.Message, ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = true)]
        public int AddMot(int _cID, int _tID, string _act, string _temp, string _bat, string _mot)
        {
            #region 다국어 리소스 로드/Title 정보

            #endregion

            try
            {
                Zeospace.DeskPlus.SO.BE.MotionInfo mInfo = new BE.MotionInfo();

                mInfo.CompanyID = _cID;
                mInfo.ThingsID = _tID;
                mInfo.ActionType = _act;
                mInfo.Temperature = _temp;
                mInfo.Battery = _bat;
                mInfo.Motion = _mot;
                //mInfo.ActionDate = Convert.ToDateTime(_actd);



                Zeospace.DeskPlus.SO.DAL.Motion mDal = new DAL.Motion();
                int rtn = mDal.Add(mInfo);

                return rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Things.Add Motion : " + ex.Message, ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = true)]
        public int AddBulb(int _cID, int _tID, string _act, string _swit, string _lev, string _hue, string _satu)
        {
            #region 다국어 리소스 로드/Title 정보

            #endregion

            try
            {
                Zeospace.DeskPlus.SO.BE.BulbInfo mInfo = new BE.BulbInfo();

                mInfo.CompanyID = _cID;
                mInfo.ThingsID = _tID;
                mInfo.ActionType = _act;
                mInfo.Switch = _swit;
                mInfo.Level = _lev;
                mInfo.Hue = _hue;
                mInfo.Saturation = _satu;
                //mInfo.ActionDate = Convert.ToDateTime(_actd);

                Zeospace.DeskPlus.SO.DAL.Bulb mDal = new DAL.Bulb();
                int rtn = mDal.Add(mInfo);

                return rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Things.Add Bulb : " + ex.Message, ex);
                return -1;
            }
        }
        #endregion


        #region Get
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public List<object> GetTemChart(string _thingsID, string _selDate)
        {
            List<object> chartData = new List<object>();

            List<string> labelData = new List<string>();
            List<string> mulData = new List<string>();
            List<string> moData = new List<string>();

            //Multipurspose
            if (_thingsID == "1" || _thingsID == "0")
            {
                DAL.Multipurpose dal = new DAL.Multipurpose();

                DataSet ds = dal.GetMultipurpose(1, 1, _selDate);
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "MultipurposeID ASC";
                DataTable dt = dv.ToTable();

                foreach (DataRow dr in dt.Rows)
                {
                    labelData.Add(Convert.ToDateTime(dr["WriteDate"]).Hour.ToString());
                    mulData.Add(dr["Temperature"].ToString());
                }
            }

            //Motion
            if (_thingsID == "2" || _thingsID == "0")
            {
                DAL.Motion dal = new DAL.Motion();

                DataSet ds = dal.GetMotion(1, 2, _selDate);
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "MotionID ASC";
                DataTable dt = dv.ToTable();

                bool IsLabelData = true;
                if (labelData.Count == 0) IsLabelData = false;

                foreach (DataRow dr in dt.Rows)
                {
                    if (!IsLabelData) labelData.Add(Convert.ToDateTime(dr["WriteDate"]).Hour.ToString());
                    moData.Add(dr["Temperature"].ToString());
                }
            }
            
            

            chartData.Add(labelData);
            chartData.Add(mulData);
            chartData.Add(moData);

            return chartData;
        }

        #endregion
    }
}
