using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User.Extend
{
    /// <summary>
    /// RewardInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class RewardInfo : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 상벌 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReward(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Admin.User.Extend.RewardInfo).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");

                DAL.UserReward biz = new DAL.UserReward();
                List<BE.UserRewardInfo> al = biz.GetUserRewardInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserRewardInfo be in al)
                {
                    BE.UserRewardInfo info = new BE.UserRewardInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.RewardID = be.RewardID;
                    info.RewardDate = be.RewardDate;

                    if (info.RewardDate.Length == 8) info.RewardDate = DateHelper.GetCultureDateChar8(be.RewardDate, PreLangReal);

                    info.Gubun = be.Gubun;
                    info.RewardType = be.RewardType;
                    info.Contents = be.Contents;
                    info.Rewarder = be.Rewarder;
                    info.Orders = be.Orders;
                    info.GubunName = this.GetLangName(be.GubunBase,be.GubunKo, be.GubunEn, be.GubunJa,be.GubunFr, be.GubunZhcn,be.GubunZhtw, PreLang);
                    info.BtnDel = resMng.GetString("Admin_UserList_cbsUserDel");

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReward : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상벌 할당하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReward(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string rewardDate = GetDecryptParam("rewardDate", "");
            string gubun = GetDecryptParam("gubun", "");
            string rewardType = GetDecryptParam("rewardType", "");
            string contents = GetDecryptParam("contents", "");
            string rewarder = GetDecryptParam("rewarder", "");


            if (rewardDate.Length > 0)
            {
                BE.UserRewardInfo be = new BE.UserRewardInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.RewardDate = DateHelper.GetCultureDate(rewardDate.Trim(), "yyyyMMdd", PreLangReal);
                be.Gubun = gubun;
                be.RewardType = rewardType;
                be.Contents = contents;
                be.Rewarder = rewarder;
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.UserReward dal = new DAL.UserReward();
                dal.Add(be);

                return true;
            }

            return false;

        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상벌 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelReward(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserReward dal = new DAL.UserReward();
                return dal.Del(companyID, id);
            }

            return false;
        }
    }
}
