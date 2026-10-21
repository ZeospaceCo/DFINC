using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Content
{
    /// <summary>
    /// Download의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Download : Base.BaseWSWebHard
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-09
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                string fileID = GetDecryptParam("fileID", "0");
                string order = GetDecryptParam("order", "0");

                fileID = fileID.TrimEnd(',');

                DAL.File dal = new DAL.File();

                DataTableReader dtr = dal.GetFileList(CurrentUserInfo.CompanyID, fileID, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.WebHard.BE.FileInfo info = new BE.FileInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();                           
                            info.FileID = dtr["FileID"].ToString().ToNumber<int>();
                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            info.FileName = dtr["FileName"].ToString();                     
                           
                            listobj.Add(info);

                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
