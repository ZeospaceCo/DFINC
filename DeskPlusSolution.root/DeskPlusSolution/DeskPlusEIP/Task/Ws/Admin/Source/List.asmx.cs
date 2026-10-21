using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Source
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSTask
    {
        #region GET
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 출처 전체리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Task.BE.SourceInfo> GetPageList(string p)
        {
            List<BE.SourceInfo> info = new List<BE.SourceInfo>();

            DAL.Source dal = new DAL.Source();

            var sourceList = dal.GetSource(CurrentUserInfo.CompanyID, false);
            foreach (var list in sourceList)
            {
                BE.SourceInfo lstinfo = new SourceInfo();

                lstinfo.CompanyID = list.CompanyID;
                lstinfo.SourceID = list.SourceID;
                lstinfo.SourceName = this.GetLangName(
                                    list.NameBase.ToNull(), list.NameKo.ToNull(), 
                                    list.NameEn.ToNull(), list.NameJa.ToNull(), 
                                    list.NameFr.ToNull(), list.NameZhcn.ToNull(), 
                                    list.NameZhtw.ToNull(), PreLang
                                    );

                lstinfo.NameBase = list.NameBase.ToNull();
                lstinfo.NameKo = list.NameKo.ToNull();
                lstinfo.NameEn = list.NameEn.ToNull();
                lstinfo.NameJa = list.NameJa.ToNull();
                lstinfo.NameZhcn = list.NameZhcn.ToNull();
                lstinfo.NameZhtw = list.NameZhtw.ToNull();
                lstinfo.IsActive = list.IsActive.ToNull();

                info.Add(lstinfo);

            }

            return info;
                       
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 출처 선택리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Task.BE.SourceInfo> GetSourceID(string p)
        {
            SetDecryptString(p);
            string sourceID = GetDecryptParam("sourceID", "0");

            List<BE.SourceInfo> info = new List<BE.SourceInfo>();
            
            DAL.Source dal = new DAL.Source();

            var sourceList = dal.GetSourceID(CurrentUserInfo.CompanyID, sourceID.ToString().ToNumber<int>());
            foreach (var list in sourceList)
            {
                BE.SourceInfo lstinfo = new SourceInfo();

                lstinfo.CompanyID = list.CompanyID;
                lstinfo.SourceID = list.SourceID;
                lstinfo.SourceName = this.GetLangName(
                                    list.NameBase.ToNull(), list.NameKo.ToNull(),
                                    list.NameEn.ToNull(), list.NameJa.ToNull(),
                                    list.NameFr.ToNull(), list.NameZhcn.ToNull(),
                                    list.NameZhtw.ToNull(), PreLang
                                    );

                lstinfo.NameBase = list.NameBase.ToNull();
                lstinfo.NameKo = list.NameKo.ToNull();
                lstinfo.NameEn = list.NameEn.ToNull();
                lstinfo.NameJa = list.NameJa.ToNull();
                lstinfo.NameFr = list.NameFr.ToNull();
                lstinfo.NameZhcn = list.NameZhcn.ToNull();
                lstinfo.NameZhtw = list.NameZhtw.ToNull();
                lstinfo.IsActive = list.IsActive.ToNull();

                info.Add(lstinfo);

            }

            return info;

        }
        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
        /// 출처 저장하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSource(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");

                BE.SourceInfo info = new SourceInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;
                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.WriteUserID = CurrentUserInfo.UserID;

                DAL.Source dal = new DAL.Source();

                dal.AddSource(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region MOD

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModSource(string p)
        {
            try
            {
                SetDecryptString(p);

                string baseName = GetDecryptParam("baseName", "");
                string multiName = GetDecryptParam("multiName", "");
                string active = GetDecryptParam("active", "1");
                int sourceID = GetDecryptParam("sourceID", "0").ToString().ToNumber<int>();


                DAL.Source dal = new DAL.Source();

                BE.SourceInfo info = new SourceInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.NameBase = baseName;
                if (multiName != "" && multiName.IndexOf(",") != -1)
                {
                    info.NameKo = multiName.Split(',')[0];
                    info.NameEn = multiName.Split(',')[1];
                    info.NameJa = multiName.Split(',')[2];
                    info.NameFr = multiName.Split(',')[3];
                    info.NameZhcn = multiName.Split(',')[4];
                    info.NameZhtw = multiName.Split(',')[5];
                }

                info.IsActive = active;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.SourceID = sourceID;

                dal.ModSource(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region DEL
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSource(string p)
        {
            SetDecryptString(p);
            int sourceID = GetDecryptParam("sourceID", "").ToString().ToNumber<int>(); //대분류 아이디값            

            DAL.Source dal = new DAL.Source();

           return dal.DelSource(CurrentUserInfo.CompanyID, sourceID);

        }
        #endregion
    }
}
