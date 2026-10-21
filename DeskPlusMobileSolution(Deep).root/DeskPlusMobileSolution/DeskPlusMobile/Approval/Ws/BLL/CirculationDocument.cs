using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Approval.Ws.EDM;
using System.Data;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BLL
{
    public class CirculationDocument
    {
        /// <summary>
        /// documentid 로 회람 문서 삭제하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID)
        {
            try
            {
                string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //데이터 삭제 전에 정보를 담아 첨부파일 삭제할때 사용한다.
                    Data.CirculationDocument cirdal = new Data.CirculationDocument();
                    var cirList = cirdal.GetDocListByDocID(_companyID, _documentID);

                    cirdal.Del(_companyID, _documentID);

                    foreach (var info in cirList)
                    {
                        // 경로 : /DeskPlusMobile/FileData/1/approval/circulation/년월/회람문서id/Attach
                        string filePath = baseFileDirectory + "/" + _companyID + "/approval/circulation/" + info.Number2 + "/" + info.CirculationDocumentID;
                        HttpContext context = HttpContext.Current;
                        if (System.IO.Directory.Exists(context.Server.MapPath(filePath))) System.IO.Directory.Delete(context.Server.MapPath(filePath), true);
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.CirculationDocument[Del] : ", ex);
                throw;
            }
        }
    }
}