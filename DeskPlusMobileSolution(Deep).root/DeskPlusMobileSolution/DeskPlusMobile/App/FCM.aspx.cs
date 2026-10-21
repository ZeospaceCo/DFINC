using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace Zeospace.DeskPlusMobile.Web.App
{
    public partial class FCM : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.App.FCM).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadData()
        {
            string SERVER_API_KEY = "AAAAXDcc0Ig:APA91bHBgJhwuz21ppok0i7Z9KsNBnrWb3-YuN6EB5qQuMm8Rx7UNu09ratu4H-ztXvvZS7o3bSrpfPrb0csp1QLvdc0qw0jpxV_anmSciYzsLe0-gUtVWJ8kUxAtE2_yLilgbr7i2Th";

            var value = "message";
            string resultStr = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            var postData =
            new
            {
                data = new
                {
                    title = "제목",
                    message = value,
                    arg1 = "",
                    arg2 = "0",
                    arg3 = "",
                    url = "http://msp.deskplus.co.kr/deskplusmobile/portal/portal.aspx",
                },

                //to = "deviceId"
                to = "dCmQTiXeAeo:APA91bEe_QSp9goIETDod2N4U1uGPxH0pPS2AxeTXuS4UOdiWL3ySlWB3NC4o-doKMd4Ux4LsCPe0aRzyB-bLGuU_EBGzs2gZ5hYWkvdJy2sTBjA2-ihVUuVqvWKRHP8kGt4Qxu93NER"                
                
            };

            //Linq to json
            //string contentMsg = JsonConvert.SerializeObject(postData);
            var serializer = new JavaScriptSerializer();
            var contentMsg = serializer.Serialize(postData);


            Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(contentMsg);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                resultStr = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                resultStr = "";
                Helper.AppException.WriteException("error", e);
            }

            Helper.AppException.WriteException("resultStr : " + resultStr);            

        }
    }
}