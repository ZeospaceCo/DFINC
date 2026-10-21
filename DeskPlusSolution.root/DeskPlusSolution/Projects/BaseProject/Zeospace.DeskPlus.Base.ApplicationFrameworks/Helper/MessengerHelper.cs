using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// MessengerHelper에 대한 요약 설명입니다.
    /// </summary>
    public abstract class MessengerHelper
    {
        /// <summary>
        /// 조직,사용자,직책,직급 정보가 CRD 되었다는 정보를 남김
        /// </summary>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public static bool UpdateState(string _loginID)
        {
            string msnKind = "";

            if (ConfigurationManager.AppSettings["MESSENGER"] != null || ConfigurationManager.AppSettings["MESSENGER"] != "") msnKind = ConfigurationManager.AppSettings["MESSENGER"];
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mod : " + msnKind, AppSettingType.MESSENGER);

            if (msnKind.ToUpper() == "SUSOFT")  // 새움 메신저일 경우
            {
                try
                {
                    string messengerLocation = "/DeskPlus_svr/bin";  // 새움 Default Path
                    if (AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessengerLocation", "") != null || AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessengerLocation", "") != "") messengerLocation = AppSettingHelper.GetAppSetting(AppSettingType.MESSENGER, "MessengerLocation", "");

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mod : " + messengerLocation, AppSettingType.CONFIG);

                    if (!messengerLocation.EndsWith("/")) messengerLocation += "/";
                    messengerLocation = FileHelper.GetServerMapPath(messengerLocation);

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mod : " + messengerLocation, AppSettingType.CONFIG);

                    if (!Directory.Exists(messengerLocation)) Directory.CreateDirectory(messengerLocation);

                    if (Directory.Exists(messengerLocation))
                    {
                        StreamWriter stream = null;
                        try
                        {
                            stream = new StreamWriter(messengerLocation + "updateinfo.dat", true, Encoding.Default);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mod : " + messengerLocation + "updateinfo.dat", AppSettingType.CONFIG);

                            stream.WriteLine(_loginID);
                        }
                        catch { }
                        finally
                        {
                            if (stream != null)
                            {
                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Mod : " + "error", AppSettingType.CONFIG);
                                stream.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_loginID + " : Messenger Information Modify Error" + ex.ToString());
                    return false;
                }
            }

            return true;
        }
    }
}