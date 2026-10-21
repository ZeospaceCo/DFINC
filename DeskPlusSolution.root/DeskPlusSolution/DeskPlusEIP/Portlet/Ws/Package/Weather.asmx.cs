using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Xml;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Weather의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Weather : Base.BaseWSPortlet
    {
        public string TempWeather = "";
        public string WeatherImgPath = "";

        [WebMethod]
        public List<object> WeatherList(string p)
        {
            try
            {
                SetDecryptString(p);
                string location = GetDecryptParam("location", "").ToString();

                List<object> weatherList = new List<object>();

                string weatherStatus = getWeather(location);

                WeatherInfo info = new WeatherInfo();
                info.hour = weatherStatus.Split(',')[0].ToString();
                info.weatherImg = weatherStatus.Split(',')[1].ToString();
                info.todayinfo = weatherStatus.Split(',')[2].ToString();
                info.tomorrow = weatherStatus.Split(',')[3].ToString();
                
                weatherList.Add(info);

                return weatherList;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        public string getWeather(string _location)
        {   
            //0서울 60, 127
            //1인천 50, 124
            //2대전광역시 68, 102
            //3대구광역시 90, 88
            //4광주광역시 58, 75
            //5부산광역시 97, 75
            //6제주특별자치도 52, 35
            //7강원도 91, 134
            //8충청남도 63, 109
            //9충청북도 76, 114
            //10전라북도 62, 90
            //11전라남도 61, 68
            //12경상북도 78, 94
            //13경상남도 80, 74

            int x = 0;
            int y = 0;
            string targetName = "";
            string weatherTomorrow = "";
            string toDay = "";

            string userLanguage = CurrentUserInfo.PreferredLanguage;

            if (userLanguage.ToUpper() == "KO" || userLanguage.ToUpper() == "")
            {


                if (_location.ToString().ToUpper() == "SEOUL") { targetName = "서울"; x = 60; y = 127; }
                else if (_location.ToString().ToUpper() == "INCHEON") { targetName = "인천"; x = 50; y = 124; }
                else if (_location.ToString().ToUpper() == "DAEJEON") { targetName = "대전"; x = 68; y = 102; }
                else if (_location.ToString().ToUpper() == "DAEGU") { targetName = "대구"; x = 90; y = 88; }
                else if (_location.ToString().ToUpper() == "KWANGJU") { targetName = "광주"; x = 58; y = 75; }
                else if (_location.ToString().ToUpper() == "BUSAN") { targetName = "부산"; x = 97; y = 75; }
                else if (_location.ToString().ToUpper() == "JEJU") { targetName = "제주"; x = 52; y = 35; }
                else if (_location.ToString().ToUpper() == "KANGWON") { targetName = "강원"; x = 91; y = 134; }
                else if (_location.ToString().ToUpper() == "CHUNGBUK") { targetName = "충북"; x = 63; y = 109; }
                else if (_location.ToString().ToUpper() == "CHUNGNAM") { targetName = "충남"; x = 76; y = 114; }
                else if (_location.ToString().ToUpper() == "JEONBUK") { targetName = "전북"; x = 62; y = 90; }
                else if (_location.ToString().ToUpper() == "JEONNAM") { targetName = "전남"; x = 61; y = 68; }
                else if (_location.ToString().ToUpper() == "KYUNGBUK") { targetName = "경북"; x = 78; y = 94; }
                else if (_location.ToString().ToUpper() == "KYUNGNAM") { targetName = "경남"; x = 80; y = 74; }

                XmlDocument docX = new XmlDocument();

                try
                {
                    string weather = "";

                    docX.Load("http://www.kma.go.kr/wid/queryDFS.jsp?gridx=" + x + "&gridy=" + y + ""); // url로 xml 파일 로드


                    XmlNodeList hourList = docX.GetElementsByTagName("hour"); // 태그 이름으로 노드 리스트 저장
                    XmlNodeList tempList = docX.GetElementsByTagName("temp");
                    XmlNodeList weatherList = docX.GetElementsByTagName("wfKor");
                    XmlNodeList sky = docX.GetElementsByTagName("sky");
                    XmlNodeList pty = docX.GetElementsByTagName("pty");


                    if (pty[0].InnerText == "0")
                    {
                        WeatherImgPath = BaseImgPath + "/Weather/sky_" + sky[0].InnerText + ".gif";
                    }
                    else
                    {
                        WeatherImgPath = BaseImgPath + "/Weather/pty_" + pty[0].InnerText + ".gif";
                    }

                    TempWeather = targetName;
                    toDay = weatherList[0].InnerText + " (" + tempList[0].InnerText + "℃)"; //오늘날씨
                    weatherTomorrow = weatherList[8].InnerText + " (" + tempList[8].InnerText + "℃)"; //내일날씨
                    //TempWeather += hourList[2].InnerText + "시 : " + weatherList[2].InnerText + " (" + tempList[2].InnerText + "℃)\n";
                    DAL.Weather addWeather = new DAL.Weather();
                    addWeather.UpdUserWeather(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, _location.ToString());

                    weather = TempWeather + "," + WeatherImgPath + "," + toDay + "," + weatherTomorrow;

                    return weather;

                }
                catch
                {
                    return targetName + "날씨 \n 기상청 정보 오류";
                }

            }
            else
            {

                if (_location.ToString().ToUpper() == "SEOUL") { targetName = "SEOUL"; x = 60; y = 127; }
                else if (_location.ToString().ToUpper() == "INCHEON") { targetName = "INCHEON"; x = 50; y = 124; }
                else if (_location.ToString().ToUpper() == "DAEJEON") { targetName = "DAEJEON"; x = 68; y = 102; }
                else if (_location.ToString().ToUpper() == "DAEGU") { targetName = "DAEGU"; x = 90; y = 88; }
                else if (_location.ToString().ToUpper() == "KWANGJU") { targetName = "KWANGJU"; x = 58; y = 75; }
                else if (_location.ToString().ToUpper() == "BUSAN") { targetName = "BUSAN"; x = 97; y = 75; }
                else if (_location.ToString().ToUpper() == "JEJU") { targetName = "JEJU"; x = 52; y = 35; }
                else if (_location.ToString().ToUpper() == "KANGWON") { targetName = "KANGWON"; x = 91; y = 134; }
                else if (_location.ToString().ToUpper() == "CHUNGBUK") { targetName = "CHUNGBUK"; x = 63; y = 109; }
                else if (_location.ToString().ToUpper() == "CHUNGNAM") { targetName = "CHUNGNAM"; x = 76; y = 114; }
                else if (_location.ToString().ToUpper() == "JEONBUK") { targetName = "JEONBUK"; x = 62; y = 90; }
                else if (_location.ToString().ToUpper() == "JEONNAM") { targetName = "JEONNAM"; x = 61; y = 68; }
                else if (_location.ToString().ToUpper() == "KYUNGBUK") { targetName = "KYUNGBUK"; x = 78; y = 94; }
                else if (_location.ToString().ToUpper() == "KYUNGNAM") { targetName = "KYUNGNAM"; x = 80; y = 74; }

                XmlDocument docX = new XmlDocument();

                try
                {
                    string weather = "";

                    docX.Load("http://www.kma.go.kr/wid/queryDFS.jsp?gridx=" + x + "&gridy=" + y + ""); // url로 xml 파일 로드


                    XmlNodeList hourList = docX.GetElementsByTagName("hour"); // 태그 이름으로 노드 리스트 저장
                    XmlNodeList tempList = docX.GetElementsByTagName("temp");
                    XmlNodeList weatherList = docX.GetElementsByTagName("wfEn");
                    XmlNodeList sky = docX.GetElementsByTagName("sky");
                    XmlNodeList pty = docX.GetElementsByTagName("pty");


                    if (pty[0].InnerText == "0")
                    {
                        WeatherImgPath = BaseImgPath + "/Weather/sky_" + sky[0].InnerText + ".gif";
                    }
                    else
                    {
                        WeatherImgPath = BaseImgPath + "/Weather/pty_" + pty[0].InnerText + ".gif";
                    }

                    TempWeather = targetName;
                    toDay = weatherList[0].InnerText + " (" + tempList[0].InnerText + "℃)";
                    weatherTomorrow = weatherList[8].InnerText + " (" + tempList[8].InnerText + "℃)";
                    //TempWeather += hourList[2].InnerText + "시 : " + weatherList[2].InnerText + " (" + tempList[2].InnerText + "℃)\n";
                    DAL.Weather addWeather = new DAL.Weather();
                    addWeather.UpdUserWeather(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, _location.ToString());

                    weather = TempWeather + "," + WeatherImgPath + "," + toDay +"," + weatherTomorrow;

                    return weather;

                }
                catch
                {
                    return targetName + "Weather Service Error";
                }
            }
            
            

        }

        public class WeatherInfo
        {
           public string hour = "";
           public string weatherImg = "";
           public string tomorrow = "";
           public string todayinfo = "";
        }
    }
}
