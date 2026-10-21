using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content
{
    /// <summary>
    /// QuesView의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class QuesView : Base.BaseWSOpinion
    {

        #region GET
        /// <summary>
        /// 특정설문의 파일리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetAttachList(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Attach attDal = new DAL.Attach();
            DataTableReader dtr = attDal.GetAttachList(CurrentUserInfo.CompanyID, quesID);

            List<object> attobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AttachInfo attInfo = new BE.AttachInfo();
                        attInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        attInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        attInfo.AttachID = dtr["AttachID"].ToString().ToNumber<int>();
                        attInfo.FilePath = dtr["FilePath"].ToString();
                        attInfo.FileName = dtr["FileName"].ToString();
                        attInfo.FileType = dtr["FileType"].ToString();
                        attInfo.FileLength = dtr["FileLength"].ToString().ToNumber<int>();
                        attInfo.FileGUID = dtr["FileGUID"].ToString();
                        attInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(dtr["FileLength"].ToString().ToNumber<int>()) / 1024) + " KB";

                        attobj.Add(attInfo);
                    }
                }
            }
            return attobj;

        }

        /// <summary>
        /// 특정설문에 대한 질문+질문항목내용 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetQuesUnitEtc(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.QuesUnit quDAL = new DAL.QuesUnit();
            DataTableReader dtr = quDAL.GetQuesUnitEtc(CurrentUserInfo.CompanyID, quesID);

            List<object> quObj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                        quInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        quInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        quInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        quInfo.Title = dtr["Title"].ToString().ToReverseXSS();  //JS 에서 특수문자가 그대로 표시되는 문제
                        quInfo.Type = dtr["Type"].ToString();
                        quInfo.IsReq = dtr["IsReq"].ToString();
                        quInfo.Point = dtr["Point"].ToString().ToNumber<int>();
                        quInfo.IsHorizon = dtr["IsHorizon"].ToString();
                        quInfo.UnitItemID = dtr["UnitItemID"].ToString().ToNumber<int>();
                        quInfo.ItemType = dtr["ItemType"].ToString();
                        quInfo.ItemCont = dtr["ItemCont"].ToString();
                        quInfo.ItemImgUrl = dtr["ItemImgUrl"].ToString();

                        quObj.Add(quInfo);
                    }
                }
            }

            return quObj;
        }


        #endregion

        #region SET
        #endregion
    }
}
